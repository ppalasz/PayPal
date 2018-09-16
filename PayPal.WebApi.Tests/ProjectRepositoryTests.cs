using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayPal.Business.DAL;
using PayPal.Business.DAL.Models;
using Xunit;

namespace PayPal.Business.Tests
{
    public class ProjectRepositoryTests
    {
        private readonly string _connectionStringName = "PayPal";

        private readonly DbContextOptions<ProjectContext> _options;

        public ProjectRepositoryTests()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            IConfigurationSection connectionStrings = configuration.GetSection("ConnectionStrings");
            string connectionString = connectionStrings[_connectionStringName];

            DbContextOptionsBuilder<ProjectContext> optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();
            _options = optionsBuilder.UseSqlServer(connectionString).Options;
        }
        
        [Fact]
        public void ProjectCrudTest()
        {
            using (ProjectContext projectContext = new ProjectContext(_options))
            {
                Project project = new Project()
                {
                    ProjectName="test",
                    CreationDate = DateTime.Now,
                    ProjectTypeId = 1,
                    DueDate = DateTime.Now,
                    Repetition = 0,
                    StatusId = 1,
                    SourceLanguage = "pl",
                    TargetLanguage = "en",
                    Url="www.test.pl"
                };

                projectContext.Project.Add(project);
                projectContext.SaveChanges();

                long? projectId = project?.ProjectId;

                Assert.NotNull(projectId);

                project = (from p in projectContext.Project where p.ProjectId == projectId select p)?.SingleOrDefault();

                Assert.NotNull(project);

                projectContext.Project.Remove(project);
                projectContext.SaveChanges();

                List<Project> projects = (from p in projectContext.Project where p.ProjectId == projectId select p)?.ToList();

                Assert.Empty(projects);
            }
        }
    }
}