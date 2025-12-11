namespace TestHumans4Help.Interfaces;
/// <summary>
/// Defines a service capable of crawling a web page and its child pages
/// in order to extract all email addresses found.
/// </summary>
internal interface IMailWebCrawler
{
    /// <summary>
    /// Retrieves all email addresses found in the specified URL and,
    /// recursively, in its child pages up to the specified depth.
    /// </summary>
    /// <param name="browser">The browser instance used to load HTML content.</param>
    /// <param name="url">The starting URL to crawl.</param>
    /// <param name="maximumDepth">
    /// Maximum recursion depth. Use <c>-1</c> for unlimited depth.
    /// </param>
    /// <returns>A list of all detected email addresses.</returns>
    List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth);
}
