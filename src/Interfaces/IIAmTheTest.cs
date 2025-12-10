namespace TestHumans4Help.Interfaces;
/// <summary>
/// Defines the contract for services capable of generating suggestions
/// based on a search term and a collection of candidate strings.
/// </summary>
internal interface IIAmTheTest
{
    /// <summary>
    /// Generates a list of suggestions from the provided choices whose
    /// similarity difference from the given term is less than or equal
    /// to the specified maximum distance.
    /// </summary>
    /// <param name="term">The search term to compare against candidate strings.</param>
    /// <param name="choices">A collection of candidate strings to evaluate.</param>
    /// <param name="maxDistance">
    /// The maximum allowed difference score for a candidate to be included.
    /// </param>
    /// <returns>
    /// A list of candidate strings whose computed difference is less than or
    /// equal to <paramref name="maxDistance"/>, sorted alphabetically.
    /// </returns>
    List<string> GetSuggestions(string term, IEnumerable<string> choices, int maxDistance);
}
