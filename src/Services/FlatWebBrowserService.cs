using TestHumans4Help.Interfaces;
namespace TestHumans4Help.Services;

/// <summary>
/// A simple in-memory web browser implementation that simulates a flat set
/// of HTML pages. This is mainly used for testing web crawling functionality
/// without performing real HTTP requests.
/// </summary>
public class FlatWebBrowserService : IWebBrowser
{
    private readonly Dictionary<string, string> _pages = [];

    /// <summary>
    /// Internal dictionary storing HTML pages, indexed by their URL.
    /// </summary>
    public FlatWebBrowserService()
    {
        string index = """
			<html>
			<h1>INDEX</h1>
			<a href="./child1.html">child1</a>
			<a href="mailto:nullepart@mozilla.org">Envoyer l'email nulle part</a>
			<a href="mailto:nullepart@mozilla.org">Envoyer l'email nulle part</a>
			</html>
			""";

        string child1 = """
				<html>
				<h1>CHILD1</h1>
				<a href="./index.html">index</a>
				<a href="./child2.html">child2</a>
				<a href="mailto:ailleurs@mozilla.org">Envoyer l'email ailleurs</a>
				<a href="mailto:nullepart@mozilla.org">Envoyer l'email nulle part</a>
				</html>
			""";
        string child2 = """
				<html>
				<h1>CHILD2</h1>
				<a href="./index.html">index</a>
				<a href="mailto:loin@mozilla.org">Envoyer l'email loin</a>
				<a href="mailto:nullepart@mozilla.org">Envoyer l'email nulle part</a>
				</html>
			""";


        _pages = new()
        {
            { "./index.html", index },
            { "./child1.html", child1 },
            { "./child2.html", child2 }
        };

    }

    /// <summary>
    /// Retrieves the HTML content associated with a given URL.
    /// </summary>
    /// <param name="url">The URL of the page to retrieve.</param>
    /// <returns>
    /// The HTML content if the URL exists in the internal store;
    /// otherwise, an empty string.
    /// </returns>
    public string GetHtml(string url)
    {
        return _pages.TryGetValue(url, out var page) ? page : string.Empty;

    }
}
