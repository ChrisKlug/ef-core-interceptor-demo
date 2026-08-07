using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
                .WithDataVolume("efcore-interceptor-demo-data")
                .WithLifetime(ContainerLifetime.Persistent);

var db1 = sql.AddDatabase("default", "Default");
var db2 = sql.AddDatabase("kite", "Kite");
var db3 = sql.AddDatabase("wingfoil", "Wingfoil");

builder.AddProject<FiftyNine_EfCore_InterceptorDemo_Web>("web", "https")
       .WithReference(db1)
       .WithReference(db2)
       .WithReference(db3)
       .WaitFor(db1)
       .WaitFor(db2)
       .WaitFor(db3);

builder.Build().Run();
