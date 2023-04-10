using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;
using Xunit;
using WebApi.Models;

namespace WebApi.Tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<LMSContext>>
    {
        private readonly WebApplicationFactory<LMSContext> _factory;
        private readonly HttpClient _client;

        public ApiTests(WebApplicationFactory<LMSContext> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateAndReadCourse()
        {
            var courseToCreate = new Course { Name = "New Course" };

            var createResponse = await _client.PostAsJsonAsync("/api/courses", courseToCreate);
            createResponse.EnsureSuccessStatusCode();
            var createdCourse = await createResponse.Content.ReadFromJsonAsync<Course>();

            var readResponse = await _client.GetAsync($"/api/courses/{createdCourse.Id}");
            readResponse.EnsureSuccessStatusCode();
            var retrievedCourse = await readResponse.Content.ReadFromJsonAsync<Course>();

            Assert.NotNull(createdCourse);
            Assert.NotNull(retrievedCourse);
            Assert.Equal(createdCourse.Id, retrievedCourse.Id);
            Assert.Equal(courseToCreate.Name, retrievedCourse.Name);
        }

        [Fact]
        public async Task CreateCourseWithTwoModulesAndReadCourseWithModules()
        {
            var courseToCreate = new Course
            {
                Name = "Course with Modules", Modules = new List<Module>
                {
                    new Module { Name = "Module 1" },
                    new Module { Name = "Module 2" },
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/courses", courseToCreate);
            createResponse.EnsureSuccessStatusCode();
            var createdCourse = await createResponse.Content.ReadFromJsonAsync<Course>();

            var readResponse = await _client.GetAsync($"/api/courses/{createdCourse.Id}/modules");
            readResponse.EnsureSuccessStatusCode();
            var modules = await readResponse.Content.ReadFromJsonAsync<List<Module>>();

            Assert.NotNull(createdCourse);
            Assert.Equal(2, modules.Count);
        }

        [Fact]
        public async Task CreateAndReadThreeCourses()
        {
            var coursesToCreate = new List<Course>
            {
                new Course { Name = "Course 1" },
                new Course { Name = "Course 2" },
                new Course { Name = "Course 3" },
            };

            foreach (var course in coursesToCreate)
            {
                var createResponse = await _client.PostAsJsonAsync("/api/courses", course);
                createResponse.EnsureSuccessStatusCode();
            }

            var readResponse = await _client.GetAsync("/api/courses");
            readResponse.EnsureSuccessStatusCode();
            var courses = await readResponse.Content.ReadFromJsonAsync<List<Course>>();

            Assert.Equal(3, courses.Count);
        }

        [Fact]
        public async Task CreateThreeAssignmentsDeleteOneAndReadRemainingAssignments()
        {
            // Arrange
            var assignmentsToCreate = new List<Assignment>
            {
                new Assignment { Name = "Assignment 1", Grade = 12, DueDate = new DateTime(2021, 10, 10) },
                new Assignment { Name = "Assignment 2", Grade = 15, DueDate = new DateTime(2021, 10, 15) },
                new Assignment { Name = "Assignment 3", Grade = 20, DueDate = new DateTime(2021, 10, 20) }
            };
            foreach (var assignment in assignmentsToCreate)
            {
                var createResponse = await _client.PostAsJsonAsync("/api/assignments", assignment);
                createResponse.EnsureSuccessStatusCode();
            }

            var deleteResponse = await _client.DeleteAsync("/api/assignments/2");
            deleteResponse.EnsureSuccessStatusCode();

            var readResponse = await _client.GetAsync("/api/assignments");
            readResponse.EnsureSuccessStatusCode();
            var assignments = await readResponse.Content.ReadFromJsonAsync<List<Assignment>>();

            Assert.Equal(2, assignments.Count);
            Assert.DoesNotContain(assignments, a => a.Name == "Assignment 2");
        }

        [Fact]
        public async Task CreateAndDeleteModule()
        {
            var moduleToCreate = new Module { Name = "New Module" };

            var createResponse = await _client.PostAsJsonAsync("/api/modules", moduleToCreate);
            createResponse.EnsureSuccessStatusCode();
            var createdModule = await createResponse.Content.ReadFromJsonAsync<Module>();

            var deleteResponse = await _client.DeleteAsync($"/api/modules/{createdModule.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            var readResponse = await _client.GetAsync("/api/modules");
            readResponse.EnsureSuccessStatusCode();
            var modules = await readResponse.Content.ReadFromJsonAsync<List<Module>>();

            Assert.DoesNotContain(modules, m => m.Id == createdModule.Id);
        }

        [Fact]
        public async Task CreateCourseAndAddModule()
        {
            var courseToCreate = new Course { Name = "New Course" };

            var createResponse = await _client.PostAsJsonAsync("/api/courses", courseToCreate);
            createResponse.EnsureSuccessStatusCode();
            var createdCourse = await createResponse.Content.ReadFromJsonAsync<Course>();

            var moduleToAdd = new Module { Name = "New Module" };

            var addModuleResponse =
                await _client.PostAsJsonAsync($"/api/courses/{createdCourse.Id}/modules", moduleToAdd);
            addModuleResponse.EnsureSuccessStatusCode();

            var readResponse = await _client.GetAsync($"/api/courses/{createdCourse.Id}/modules");
            readResponse.EnsureSuccessStatusCode();
            var modules = await readResponse.Content.ReadFromJsonAsync<List<Module>>();

            Assert.Single(modules);
            Assert.Equal(moduleToAdd.Name, modules[0].Name);
        }

        [Fact]
        public async Task CreateModuleAndAddAssignment()
        {
            var moduleToCreate = new Module { Name = "New Module" };

            var createResponse = await _client.PostAsJsonAsync("/api/modules", moduleToCreate);
            createResponse.EnsureSuccessStatusCode();
            var createdModule = await createResponse.Content.ReadFromJsonAsync<Module>();

            var assignmentToAdd = new Assignment
                { Name = "New Assignment", Grade = 18, DueDate = new DateTime(2021, 11, 1) };

            var addAssignmentResponse =
                await _client.PostAsJsonAsync($"/api/modules/{createdModule.Id}/assignments", assignmentToAdd);
            addAssignmentResponse.EnsureSuccessStatusCode();

            var readResponse = await _client.GetAsync($"/api/modules/{createdModule.Id}/assignments");
            readResponse.EnsureSuccessStatusCode();
            var assignments = await readResponse.Content.ReadFromJsonAsync<List<Assignment>>();

            Assert.Single(assignments);
            Assert.Equal(assignmentToAdd.Name, assignments[0].Name);
        }
    }
}