using System.Text.RegularExpressions;
using TestHumans4Help.Interfaces;
namespace TestHumans4Help.Services;
/// <summary>
/// Service that crawls web pages recursively in order to extract all email
/// addresses found in a page and its child pages.
/// </summary>
internal class MailWebCrawlerService : IMailWebCrawler
{
    /// <summary>
    /// Retrieves all email addresses found in a web page and,
    /// recursively, in its child pages up to the specified maximum depth.
    /// </summary>
    /// <param name="browser">Browser instance used to retrieve HTML content from a URL.</param>
    /// <param name="url">Starting URL to explore.</param>
    /// <param name="maximumDepth">
    /// Maximum recursion depth.
    /// Use <c>-1</c> for unlimited depth.
    /// </param>
    /// <returns>A list containing all email addresses found.</returns>
    public List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth)
    {
        HashSet<string> emails;
        List<string> visitedLinks = [];
        int currentLevel = 0;
        emails = GetLinks(browser, visitedLinks, url, currentLevel, maximumDepth);
        return [.. emails];
    }

    /// <summary>
    /// Crawls a URL, extracts links and email addresses, and continues recursively
    /// according to the defined maximum depth.
    /// </summary>
    /// <param name="browser">Browser used to retrieve the HTML content of a URL.</param>
    /// <param name="visitedLinks">List of already visited URLs to avoid infinite loops.</param>
    /// <param name="url">The URL to analyze.</param>
    /// <param name="currentLevel">Current recursion depth.</param>
    /// <param name="maxLevel">
    /// Maximum allowed recursion depth.
    /// Use <c>-1</c> for unlimited depth.
    /// </param>
    /// <returns>
    /// A set of email addresses found in the page and its recursively analyzed child pages.
    /// </returns>
    private static HashSet<string> GetLinks(IWebBrowser browser, List<string> visitedLinks, string url, int currentLevel, int maxLevel)
    {
        visitedLinks.Add(url);
        HashSet<string> links = [];
        HashSet<string> emails = [];
        List<string> extractedLinks = ExtractLinks(browser.GetHtml(url));
        foreach (string link in extractedLinks)
        {
            HashSet<string> extractedEmails = ExtractEmails(link);
            if (extractedEmails.Count != 0)
            {
                emails.UnionWith(extractedEmails);
                continue;
            }
            if (!visitedLinks.Contains(link) && (currentLevel < maxLevel || maxLevel == -1))
            {
                links.Add(link);
            }
        }
        if (links.Count > 0)
        {
            currentLevel++;
            foreach (string link in links)
            {
                emails.UnionWith(GetLinks(browser, visitedLinks, link, currentLevel, maxLevel));
            }
        }
        return emails;
    }

    /// <summary>
    /// Extracts all URLs (href attributes) contained in the supplied HTML page.
    /// </summary>
    /// <param name="htmlPage">Full HTML content of the page to analyze.</param>
    /// <returns>A list of all extracted URLs.</returns>
    private static List<string> ExtractLinks(string htmlPage)
    {
        List<string> list = [];
        MatchCollection m1 = Regex.Matches(htmlPage, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
        foreach (Match m in m1)
        {
            string value = m.Groups[1].Value;
            Match m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);
            if (m2.Success)
            {
                list.Add(m2.Groups[1].Value);
            }
        }
        return list;
    }


    /// <summary>
    /// Extracts all email addresses contained in the provided string.
    /// </summary>
    /// <param name="link">String to analyze (often a URL or HTML fragment).</param>
    /// <returns>A set of detected email addresses.</returns>
    private static HashSet<string> ExtractEmails(string link)
    {
        HashSet<string> emails = [];
        Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
        MatchCollection emailMatches = emailRegex.Matches(link);
        foreach (Match emailMatch in emailMatches)
        {
            emails.Add(emailMatch.Value);
        }
        return emails;

    }
}
