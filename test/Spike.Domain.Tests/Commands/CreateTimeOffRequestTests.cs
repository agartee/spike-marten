using FluentAssertions;
using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spike.Domain.Commands;
using Spike.Domain.Models;
using Spike.Domain.Projections;
using System;
using Weasel.Core;

namespace Spike.Domain.Tests.Commands
{
    public class CreateTimeOffRequestTests
    {
        [Fact]
        public async Task Handle_CreatesEventStreamAndProjections()
        {
            //var configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json", optional: true)
            //.AddEnvironmentVariables()
            //.Build();

            var services = new ServiceCollection();

            services.AddMarten(options =>
            {
                options.Connection("host=localhost;database=mydatabase;username=admin;password=secret");
                options.UseSystemTextJsonForSerialization();
                options.AutoCreateSchemaObjects = AutoCreate.All;

                // todo: add projections
                options.Projections.Add<TimeOffRequestProjection>(ProjectionLifecycle.Inline);
            });

            var serviceProvider = services.BuildServiceProvider();
            var documentStore = serviceProvider.GetRequiredService<IDocumentStore>();

            var handler = new CreateTimeOffRequestHandler(documentStore);
            //var result = await handler.Handle(new CreateTimeOffRequest
            //{
            //    Start = DateTime.Now,
            //    End = DateTime.Now.AddDays(1)
            //}, CancellationToken.None);

            //result.Should().NotBeNull();

            await handler.Testing();
        }
    }
}
