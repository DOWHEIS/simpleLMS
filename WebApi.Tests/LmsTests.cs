using System;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using WebApi.Models;
using Xunit;

namespace WebApi.Tests
{
// ¯\_(ツ)_/¯

    //¯\_(ツ)_/¯

    public class ApiTests : IClassFixture<WebApplicationFactory<LMSContext>>, IDisposable
    {
        private readonly WebApplicationFactory<LMSContext> _factory;
        private LMSContext _db;

        public ApiTests(WebApplicationFactory<LMSContext> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var descriptor =
                        services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LMSContext>));
                    services.Remove(descriptor);

                    services.AddDbContext<LMSContext>(options => options.UseInMemoryDatabase("TestDb"));
                }));
            _db = CreateNewContext();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private LMSContext CreateNewContext()
        {
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var dbContextOptions =
                serviceProvider.GetService(typeof(DbContextOptions<LMSContext>)) as DbContextOptions<LMSContext>;

            var dbContext = new LMSContext(dbContextOptions);
            dbContext.Database.EnsureCreated();


            return dbContext;
        }


        [Fact]
        public async Task CreateCourseAndRead()
        {
            HttpClient client = _factory.CreateClient();
            Course course = new Course { Name = "Test Course" };

            // Create a course
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/courses", course);
            response.EnsureSuccessStatusCode();

            // Read the course as a single entity
            response = await client.GetAsync("/api/courses/1");
            response.EnsureSuccessStatusCode();
            Course receivedCourse = await response.Content.ReadFromJsonAsync<Course>();

            Assert.Equal(course.Name, receivedCourse.Name);
        }
    }
}