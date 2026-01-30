var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CarRental>("carrental");

builder.Build().Run();
