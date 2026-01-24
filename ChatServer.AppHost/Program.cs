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

var elasticsearch = builder.AddElasticsearch("elasticsearch")
    .WithDataVolume()
    .WithEnvironment("ELASTIC_PASSWORD", "Password1")
    .WithEnvironment("xpack.security.enabled", "false")
    .WithEnvironment("discovery.type", "single-node");

var kibana = builder.AddContainer("kibana", "docker.elastic.co/kibana/kibana", "8.11.0")
    .WithHttpEndpoint(port: 5601, targetPort: 5601, name: "kibana")
    .WithEnvironment("ELASTICSEARCH_HOSTS", "http://elasticsearch:9200")
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

builder.AddContainer("filebeat", "docker.elastic.co/beats/filebeat", "8.11.0")
    .WithBindMount("../filebeat.yml", "/usr/share/filebeat/filebeat.yml")
    .WithBindMount("C:/mnt/Logs", "/app/logs") 
    .WithEnvironment("ELASTICSEARCH_HOSTS", "http://elasticsearch:9200")
    .WithEnvironment("ELASTICSEARCH_USERNAME", "elastic")
    .WithEnvironment("ELASTICSEARCH_PASSWORD", "changeme")
    .WithArgs("-e", "--strict.perms=false")
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch)
    .WaitFor(kibana);

await builder.Build().RunAsync();
