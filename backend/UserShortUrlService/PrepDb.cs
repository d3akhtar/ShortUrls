using UserShortUrlService.Data;
using UserShortUrlService.SyncDataServices.Grpc;

namespace UserShortUrlService
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try{
                    var db = scope.ServiceProvider.GetService<AppDbContext>();
                
                    var userDataClient = scope.ServiceProvider.GetRequiredService<IUserDataClient>();
                    
                    var users = userDataClient.ReturnAllUsers();

                    foreach (var user in users){
                        db.Users.Add(user);
                    }

                    db.SaveChanges();
                }
                catch(Exception ex){
                    Console.WriteLine("--> Error while seeding data with users, error: " + ex.Message);
                }
            }
        }
    }
}