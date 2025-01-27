var builder = DistributedApplication.CreateBuilder(args);


var redis = builder.AddRedis("redis")
                   .WithImageTag("alpine");


var app1 = builder.AddProject<Projects.Starwars_App_BlazorServerApp>("starwars-app-blazorserverapp-1", launchProfileName:"https")
                  .WithExternalHttpEndpoints() //Azure App Service 
                  .WithReference(redis)
                  .WaitFor(redis);

var app2 = builder.AddProject<Projects.Starwars_App_BlazorServerApp>("starwars-app-blazorserverapp-2", launchProfileName: "https2")
                  .WithExternalHttpEndpoints() //Azure App Service 
                  .WithReference(redis)
                  .WaitFor(redis);

builder.AddProject<Projects.Starwars_ReverseProxy>("reverse-proxy")
       .WithExternalHttpEndpoints() 
       .WaitFor(app1)
       .WaitFor(app2);


builder.Build().Run();
