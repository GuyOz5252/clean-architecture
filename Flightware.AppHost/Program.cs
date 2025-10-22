var builder = DistributedApplication.CreateBuilder(args);

var postgresUsername = builder.AddParameter("postgres-username");
var postgresPassword = builder.AddParameter("postgres-password");

var postgres = builder.AddPostgres("postgres")
    .WithImage("postgres:17")
    .WithPgAdmin()
    .WithUserName(postgresUsername)
    .WithPassword(postgresPassword)
    .AddDatabase("flightware");

builder.AddProject<Projects.Flightware_Api>("flightware-api")
    .WithReference(postgres)
    .WaitFor(postgres);

await builder.Build().RunAsync();
