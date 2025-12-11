namespace TestHumans4Help.Interfaces;
/// <summary>
/// Defines a simple web browser abstraction capable of retrieving the
/// HTML content of a given URL.
/// </summary>
internal interface IWebBrowser
{
    /// <summary>
    /// Retrieves the HTML content associated with the specified URL.
    /// </summary>
    /// <param name="url">The URL of the page to load.</param>
    /// <returns>
    /// A string containing the HTML content of the requested page.
    /// Implementations may return an empty string if the page is not found.
    /// </returns>
    string GetHtml(string url);
}
