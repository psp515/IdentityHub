﻿using ImageHub.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHub.Api.Tests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private static PostgreSqlContainerFixture postgresFixture;
    public string ConnectionString { get; private set; }

    public IntegrationTestWebAppFactory()
    {
        postgresFixture = new PostgreSqlContainerFixture();
        ConnectionString = string.Empty;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (postgresFixture.Container.State != DotNet.Testcontainers.Containers.TestcontainersStates.Running)
        {
            throw new InvalidOperationException("PostgreSQL container is not running.");
        }

        if (postgresFixture.Container.Health != DotNet.Testcontainers.Containers.TestcontainersHealthStatus.Healthy)
        {
            //throw new InvalidOperationException("PostgreSQL container is not healthy.");
        }

        builder.ConfigureTestServices(services =>
        {
            var descriptorType =
                typeof(DbContextOptions<ApplicationDbContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
                services.Remove(descriptor);

            Console.WriteLine(ConnectionString);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(ConnectionString));
        });
    }

    public async Task InitializeAsync()
    {
        await postgresFixture.InitializeAsync();
        ConnectionString = postgresFixture.Container.GetConnectionString();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new Task DisposeAsync()
    {
        return postgresFixture.DisposeAsync();
    }

}
