var builder = DistributedApplication.CreateBuilder(args);

var postgresUsername = builder.AddParameter("postgres-username");
var postgresPassword = builder.AddParameter("postgres-password");

var postgres = builder.AddPostgres("postgres")
    .WithImage("postgres:17")
    .WithPgAdmin()
    .WithUserName(postgresUsername)
    .WithPassword(postgresPassword)
    .AddDatabase("chat-server-db");

builder.AddProject<Projects.ChatServer>("chat-server")
    .WithReference(postgres)
    .WaitFor(postgres);

await builder.Build().RunAsync();
