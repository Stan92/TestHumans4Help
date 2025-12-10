using Microsoft.Extensions.DependencyInjection;
using TestHumans4Help.Interfaces;
using TestHumans4Help.Services;
namespace TestHumans4Help;

/// <summary>
/// Entry point of the TestHumans4Help application.
/// Initializes dependency injection, retrieves the suggestion service,
/// and displays matching suggestions based on a given search term.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main method of the application.
    /// Configures services, retrieves the <see cref="IIAmTheTest"/> implementation,
    /// executes a suggestion search, and displays the results in the console.
    /// </summary>
    static void Main()
    {

        var services = ConfigureServices();
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        var iAmTheTestService = serviceProvider.GetRequiredService<IIAmTheTest>();
        IEnumerable<string> choices = ["gros", "gras", "graisse", "agressif", "go", "ros", "gro"];
        string term = "gros";
        int maxDistance = 2;
        var results = iAmTheTestService.GetSuggestions(term, choices, maxDistance);
        if (results.Count == 0)
        {
            Console.WriteLine("Aucune suggestion");
            return;
        }
        foreach (var result in results)
        {
            Console.WriteLine("Suggestion: {0}", result);
        }
    }

    /// <summary>
    /// Configures and registers application services in the dependency injection container.
    /// </summary>
    /// <returns>
    /// A <see cref="ServiceCollection"/> containing all registered services,
    /// including the <see cref="IIAmTheTest"/> implementation.
    /// </returns>
    private static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IIAmTheTest, IAmTheTest>();
        return services;
    }
}
