using Microsoft.Extensions.DependencyInjection;
using TestHumans4Help.Interfaces;
using TestHumans4Help.Services;
namespace TestHumans4Help;

/// <summary>
/// Entry point of the TestHumans4Help application.
/// Sets up dependency injection, runs suggestion tests,
/// and executes email crawling tests using the mock web browser.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main entry method of the application.
    /// Initializes all services, performs a suggestion search test,
    /// runs multiple email crawling tests with increasing depth,
    /// and displays all results in the console.
    /// </summary>
    static void Main()
    {

        var services = ConfigureServices();
        using ServiceProvider serviceProvider = services.BuildServiceProvider();


        // -----------------------------
        // Suggestion Service Test
        // -----------------------------
        var suggestionService = serviceProvider.GetRequiredService<ISuggestion>();
        IEnumerable<string> choices = ["gros", "gras", "graisse", "agressif", "go", "ros", "gro"];
        string term = "gros";
        int maxDistance = 2;
        var results = suggestionService.GetSuggestions(term, choices, maxDistance);
        string suggestionMessage = results.Count == 0 ? "Aucune suggestion" : $"Suggestion: {string.Join(", ", results)}";
        Console.WriteLine( $"{"".PadLeft(30,'=')} Suggestion {"".PadLeft(30, '=')}");
        Console.WriteLine(suggestionMessage);
        Console.WriteLine("");


        // -----------------------------
        // Mail Web Crawler Test
        // -----------------------------
        var webBrowserService = serviceProvider.GetRequiredService<IWebBrowser>();
        var mailWebCrawlerService = serviceProvider.GetRequiredService<IMailWebCrawler>();
        for (int maximumDepth = 0; maximumDepth < 4; maximumDepth++)
        {
            var emails = mailWebCrawlerService.GetEmailsInPageAndChildPages(webBrowserService, "./index.html", maximumDepth);
            string emailMessage = emails.Count == 0 ? "Aucun mail trouvé" : $"Mails: {string.Join(", ", emails)}";
            Console.WriteLine($"{"".PadLeft(30, '=')} Emails [MaxDepth={maximumDepth}]  {"".PadLeft(30, '=')}");
            Console.WriteLine(emailMessage);
            Console.WriteLine("");
        }

        // Wait for user input before closing
        Console.WriteLine("Pressez une touche pour quitter");
        Console.ReadKey();
    }

    /// <summary>
    /// Configures and registers all application services in the dependency injection container.
    /// </summary>
    /// <returns>
    /// A <see cref="ServiceCollection"/> instance containing all registered services,
    /// including implementations for <see cref="ISuggestion"/>,
    /// <see cref="IWebBrowser"/>, and <see cref="IMailWebCrawler"/>.
    /// </returns>
    private static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISuggestion, SuggestionService>();
        services.AddSingleton<IWebBrowser, FlatWebBrowserService>();
        services.AddSingleton<IMailWebCrawler, MailWebCrawlerService>();
        return services;
    }
}
