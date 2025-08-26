using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System.Net;
using TechTalk.SpecFlow;

namespace NadinSoft.CRUD.AcceptanceTest.Hook;

/// <summary>
/// Provides SpecFlow hooks for managing Docker containers during test execution.
/// </summary>
[Binding]
public class DockerControllerHooks
{
    /// <summary>
    /// Holds the running Docker Compose services for the test run.
    /// </summary>
    private static ICompositeService _compositeService = null!;

    /// <summary>
    /// The SpecFlow object container used for dependency injection.
    /// </summary>
    private static IObjectContainer _objectContainer = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerControllerHooks"/> class.
    /// </summary>
    /// <param name="objectContainer">The SpecFlow object container used for dependency injection.</param>
    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    /// <summary>
    /// Starts the Docker Compose services before the test run begins.
    /// </summary>
    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        string dockerComposePath = GetDockerComposeLocation();

        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath + "/docker-compose.yml")
            .RemoveOrphans()
            .WaitForHttp(
                "NadinSoft_API",
                "http://localhost:8080/api/Product/get-all",
                continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0)
            .Build()
            .Start();
    }

    /// <summary>
    /// Stops and disposes Docker Compose services after the test run ends.
    /// </summary>
    [AfterTestRun]
    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose();
    }

    /// <summary>
    /// Adds an <see cref="HttpClient"/> to the SpecFlow object container before each scenario.
    /// </summary>
    [BeforeScenario]
    public static void AddHttpClient()
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:8080/api/"),
        };
        _objectContainer.RegisterInstanceAs(httpClient);
    }

    /// <summary>
    /// Gets the directory path containing the <c>docker-compose.yml</c> file.
    /// </summary>
    /// <returns>The absolute path to the directory containing <c>docker-compose.yml</c>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the solution root cannot be found.</exception>
    private static string GetDockerComposeLocation()
    {
        string currentDir = AppDomain.CurrentDomain.BaseDirectory;

        while (Directory.GetParent(currentDir) != null)
        {
            string dockerComposePath = Path.Combine(currentDir, "docker-compose.yml");
            if (File.Exists(dockerComposePath))
            {
                return currentDir;
            }

            DirectoryInfo? parentDir = Directory.GetParent(currentDir);
            if (parentDir != null)
            {
                currentDir = parentDir.FullName;
            }
        }

        throw new InvalidOperationException("Solution root not found.");
    }
}