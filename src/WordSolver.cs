namespace WordCreator;

public sealed class WordSolver
{
    private readonly IReadOnlyCollection<string> _dictionary;

    public WordSolver(IEnumerable<string> dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        _dictionary = dictionary
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => word.Trim().ToLowerInvariant())
            .Distinct()
            .ToArray();
    }

    public IReadOnlyList<string> FindWords(string letters)
    {
        ArgumentNullException.ThrowIfNull(letters);

        var availableCounts = CountLetters(letters);
        var matches = new SortedSet<string>();

        foreach (var word in _dictionary)
        {
            if (word.Length > letters.Length)
            {
                continue;
            }

            if (CanBuild(word, availableCounts))
            {
                matches.Add(word);
            }
        }

        return matches.ToArray();
    }

    private static Dictionary<char, int> CountLetters(string value)
    {
        var counts = new Dictionary<char, int>();

        foreach (var character in value.Where(char.IsLetter))
        {
            var letter = char.ToLowerInvariant(character);
            counts.TryGetValue(letter, out var count);
            counts[letter] = count + 1;
        }

        return counts;
    }

    private static bool CanBuild(string word, IReadOnlyDictionary<char, int> availableCounts)
    {
        var requiredCounts = CountLetters(word);

        foreach (var (letter, count) in requiredCounts)
        {
            if (!availableCounts.TryGetValue(letter, out var available) || count > available)
            {
                return false;
            }
        }

        return true;
    }
}
