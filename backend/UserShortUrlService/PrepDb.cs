using UserShortUrlService.Data;
using UserShortUrlService.SyncDataServices.Grpc;
using UserShortUrlService.SyncDataServices.Http;

namespace UserShortUrlService
{
    public static class PrepDb
    {
        public static async void PrepPopulation(IApplicationBuilder app)
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

                    // Testing http client
                    /* 
                    var shortUrlDataClient = scope.ServiceProvider.GetRequiredService<IShortUrlDataClient>();
                    if (await shortUrlDataClient.IsShortUrlMapped("1WEC6JN")){ // first code is always this
                        Console.WriteLine("Code 1WEC6JN mapped");
                    }
                    else{
                        Console.WriteLine("Code 1WEC6JN not mapped");
                    }
                    if (await shortUrlDataClient.IsShortUrlMapped("doesntexist")){
                        Console.WriteLine("Code doesntexist mapped");
                    }
                    else{
                        Console.WriteLine("Code doesntexist not mapped");
                    } */
                }
                catch(Exception ex){
                    Console.WriteLine("--> Error while seeding data with users, error: " + ex.Message);
                }
            }
        }
    }
}