
using AuthService.Data;
using AuthService.Data.Repository;
using AuthService.Model;
using AuthService.SyncDataServices.Smtp;
using Microsoft.EntityFrameworkCore;

namespace AuthService.BackgroundServices
{
    public class VerificationChecker : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private Dictionary<string,DateTime> contactTimeDictionary;

        public VerificationChecker(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            contactTimeDictionary = new();

            _scopeFactory = scopeFactory;

            // migrate when this service starts instead of inside Startup.cs
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                db.Database.Migrate(); // want to crash if connection fails so it auto restarts
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DealWithUnverifiedUsers, null, TimeSpan.Zero, TimeSpan.FromDays(2));

            return Task.CompletedTask;
        }

        private void DealWithUnverifiedUsers(object state)
        {
            Console.WriteLine("-> Getting unverified users...");
            using(var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var unverifiedUsers = userManager.GetUnverifiedUsers();
                
                List<User> usersToBeDeleted = new();

                foreach(var user in unverifiedUsers)
                {
                    Console.WriteLine("Unverified user email is: " + user.Email);

                    if (contactTimeDictionary.ContainsKey(user.Email)){
                        var totalDaysBeforeLastEmail = (DateTime.Now - contactTimeDictionary[user.Email]).TotalDays;
                        if (totalDaysBeforeLastEmail > 14) contactTimeDictionary.Remove(user.Email);

                        continue;
                    }

                    var totalDays = (DateTime.Now - user.CreatedAt).TotalDays;

                    if (totalDays > 5 && totalDays < 21){
                        Console.WriteLine("--> Sending verification email to " + user.Email);

                        var verificationUrl = _configuration["Jwt:Issuer"] + "/api/auth/verify?token=" + user.VerificationToken;

                        emailService.SendEmail(
                            subject: "Account Verification",
                            body: 
                            $@" <h3>ShortUrls Account Verification</h3>
                                <p>You still haven't verified your account. Click on the given link to verify your account. Your account will be deleted in {21 - totalDays} days if it hasn't been verified</p>
                                <form action={verificationUrl} method={"POST"}>
                                <button type={"submit"}
                                style={
                                    @"
                                        border: none;
                                        outline: none;
                                        background: none;
                                        cursor: pointer;
                                        color: #0000EE;
                                        padding: 0;
                                        text-decoration: underline;
                                        font-family: inherit;
                                        font-size: inherit;
                                    "
                                    }
                                >
                                        Verify
                                    </button>
                                </form>     
                            ",
                            receiverEmail: user.Email
                        );

                        contactTimeDictionary.Add(user.Email, DateTime.Now);
                    }
                    else if (totalDays >= 21){
                        Console.WriteLine("--> User unverified for a long time, deleting user...");

                        if (contactTimeDictionary.ContainsKey(user.Email)) contactTimeDictionary.Remove(user.Email);

                        usersToBeDeleted.Add(user);
                    }
                }

                // Delete after for loop or else I get an error
                if (usersToBeDeleted.Count > 0) {
                    userManager.DeleteUsers(usersToBeDeleted);
                    userManager.SaveChanges();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}