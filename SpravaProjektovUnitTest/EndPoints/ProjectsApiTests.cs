using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SpravaProjektovUnitTest.EndPoints
{
    public class ProjectsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly string _xmlPath;

        public ProjectsApiTests(WebApplicationFactory<Program> factory)
        {
            // vytvorenie HttpClient, ktorý bude používať rovnaké služby ako API počas testu
            _client = factory.CreateClient();

            // 1. Vytiahneme si presne tie isté služby, aké používa API počas testu
            using (var scope = factory.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                var relativePath = config.GetValue<string>("ProjectDataFilePath")
                    ?? throw new InvalidOperationException("ProjectDataFilePath not configured.");

                // 2. Toto vygeneruje presnú absolútnu cestu k súboru, ktorú API v teste používa
                _xmlPath = Path.Combine(env.ContentRootPath, relativePath);
            }

            // 3. Pred každým testom zaistíme čistý súbor
            ResetXmlFile();
        }

        private void ResetXmlFile()
        {
            // Uistíme sa, že priečinok (ak je napr. v appsettings definovaný ako "Data/projects.xml") existuje
            var directory = Path.GetDirectoryName(_xmlPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Vytvoríme prázdny XML súbor s koreňovým elementom <Projects>
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"windows-1250\"?>");

            stringBuilder.AppendLine("<projects>");
            stringBuilder.AppendLine("<project id = \"prj1\">");
            stringBuilder.AppendLine("<name> Informačný systém firmy ABC </name>");
            stringBuilder.AppendLine("<abbreviation> IS - ABC </abbreviation>");
            stringBuilder.AppendLine("<customer> ABC, s.r.o. </customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("<project id = \"prj2\">");
            stringBuilder.AppendLine("<name> Importný modul ISIS</name>");
            stringBuilder.AppendLine("<abbreviation> Import - ISIS </abbreviation>");
            stringBuilder.AppendLine("<customer> Homer Simpson </customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("<project id = \"prj3\">");
            stringBuilder.AppendLine("<name> Portácia IS - VAK na Oracle</name>");
            stringBuilder.AppendLine("<abbreviation> OracleVAK </abbreviation>");
            stringBuilder.AppendLine("<customer> VAK, s.p.</customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("<project id = \"prj4\">");
            stringBuilder.AppendLine("<name> Elektronicý obchod pre Telecom </name>");
            stringBuilder.AppendLine("<abbreviation> EComTelecom </abbreviation>");
            stringBuilder.AppendLine("<customer> Česky Telecom, a.s.</customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("<project id = \"prj5\">");
            stringBuilder.AppendLine("<name> Rozpoznávanie čiarového kódu pre Delvitu</name>");
            stringBuilder.AppendLine("<abbreviation> CK - Delvita </abbreviation>");
            stringBuilder.AppendLine("<customer> Delvita, a.s.</customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("<project id = \"pr7\">");
            stringBuilder.AppendLine("<name> fffffff </name>");
            stringBuilder.AppendLine("<abbreviation> ffffff </abbreviation>");
            stringBuilder.AppendLine("<customer> fffffffff </customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("<project id = \"string\">");
            stringBuilder.AppendLine("<name> string </name>");
            stringBuilder.AppendLine("<abbreviation> string </abbreviation>");
            stringBuilder.AppendLine("<customer> string </customer>");
            stringBuilder.AppendLine("</project>");
            stringBuilder.AppendLine("</projects>");

            File.WriteAllText(_xmlPath, stringBuilder.ToString());
        }

        public void Dispose()
        {
            // Po každom teste súbor upraceme
            if (File.Exists(_xmlPath))
            {
                File.Delete(_xmlPath);
            }
        }

        [Fact]
        public async Task GetAllProjects_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/v2/projects");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProjectByKey_ReturnsSuccessStatusCode()
        {
            // Arrange
            var key = "test-project-key";

            // Act
            var response = await _client.GetAsync($"/api/v2/projects/{key}");

            // Assert
            // Tu očakávame buď OK (200) alebo NotFound (404) podľa toho, či projekt existuje
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProject_WithValidData_Returns201Created()
        {
            // Arrange - Posielame kompletne validný objekt aj s ID
            var validProject = new
            {
                id = "PRJ-100",
                name = "Nový testovací projekt",
                abbreviation = "NTP",
                customer = "ICZ Slovakia"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v2/projects", validProject);
            var content = await (response.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created,
                $"pretože sme poslali kompletne validné dáta. API odpoveď: {content}");
        }

        [Fact]
        public async Task CreateProject_MissingId_Returns400BadRequest()
        {
            // vynechane id, aby sme otestovali validáciu
            var invalidProject = new
            {
                name = "Projekt bez ID",
                abbreviation = "PID",
                customer = "Tester"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v2/projects", invalidProject);
            var content = await (response.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));

            // Assert - Tu očakávame BadRequest, pretože validácia musí zafungovať
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                $"pretože objektu chýba povinné ID. API odpoveď: {content}");

            // Voliteľné: Môžeš otestovať, či JSON odpoveď naozaj obsahuje text o chýbajúcom ID
            content.Should().Contain("Project ID is required.");
        }

        [Fact]
        public async Task UpdateProject_ReturnsSuccessStatusCode()
        {
            var updatedProject = new
            {
                id = "prj5",
                name = "Nový testovací projekt",
                abbreviation = "NTP",
                customer = "ICZ Slovakia"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/v2/projects", updatedProject);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNoContentOrOkStatusCode()
        {
            // Arrange
            var key = "project-to-delete";

            // Act
            var response = await _client.DeleteAsync($"/api/v2/projects/{key}");

            // Assert
            // API môže pri zmazaní vracať 204 (No Content), 200 (OK) alebo prípadne 404
            Assert.True(response.StatusCode == HttpStatusCode.NoContent ||
                        response.StatusCode == HttpStatusCode.OK ||
                        response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
