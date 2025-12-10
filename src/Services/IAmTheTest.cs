using TestHumans4Help.Interfaces;
namespace TestHumans4Help.Services;
/// <summary>
/// Provides functionality for generating suggestions based on a search term
/// and calculating string similarity using a custom difference algorithm.
/// </summary>
public class IAmTheTest:IIAmTheTest
{
    /// <summary>
    /// Generates a list of suggestions from the provided choices whose
    /// similarity difference from the given term is less than or equal to
    /// the specified maximum distance.
    /// </summary>
    /// <param name="term">The search term to compare against candidate strings.</param>
    /// <param name="choices">A collection of candidate strings to evaluate.</param>
    /// <param name="maxDistance">The maximum allowed difference score for a candidate to be included.</param>
    /// <returns>
    /// A list of candidate strings whose computed difference is less than or
    /// equal to <paramref name="maxDistance"/>, sorted alphabetically.
    /// </returns>
    public List<string> GetSuggestions(string term, IEnumerable<string> choices, int maxDistance)
    {
        var results = from name in choices
                      let difference = GetDifference(name.ToLower(), term.ToLower())
                      where difference <= maxDistance
                      select name;
        return results.OrderBy(c=>c).Select(c => c).ToList();
    }
    /// <summary>
    /// Computes a similarity difference score between two strings using a
    /// Levenshtein-based comparison. If the candidate string is longer than
    /// the search string, a sliding-window approach is used to evaluate the
    /// minimum difference among all possible substrings.
    /// </summary>
    /// <param name="choice">The candidate string to evaluate.</param>
    /// <param name="searchString">The reference search term.</param>
    /// <returns>
    /// A difference score where 0 indicates an exact match and higher
    /// values represent greater dissimilarity. Returns <see cref="int.MaxValue"/>
    /// when comparison is invalid (e.g., one of the strings is empty or too short).
    /// </returns>
    private static int GetDifference(string choice, string searchString)
    {

        if (choice == searchString) return 0;
        if (choice.Length == 0) return int.MaxValue;
        if (searchString.Length == 0) return int.MaxValue;
        if (choice.Length < searchString.Length) return int.MaxValue;

        if (choice.Length > searchString.Length)
        {
            int nbIterations = choice.Length - searchString.Length + 1;
            int[] iterationResults = new int[nbIterations];
            for (int i = 0; i < nbIterations; i++)
            {
                string sub = choice.Substring(i, searchString.Length);
                iterationResults[i] = GetDifference(sub, searchString);
            }
            return iterationResults.Min();
        }


        int[,] matrix = new int[choice.Length + 1, searchString.Length + 1];
        for (int i = 0; i <= choice.Length; i++) matrix[i, 0] = i;
        for (int j = 0; j <= searchString.Length; j++) matrix[0, j] = j;
        for (int i = 1; i <= choice.Length; i++)
        {
            for (int j = 1; j <= searchString.Length; j++)
            {
                int cost = (choice[i - 1] == searchString[j - 1]) ? 0 : 1;
                matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[choice.Length, searchString.Length];
    }
}
