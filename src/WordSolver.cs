namespace WordCreator;

public sealed class WordSolver
{
    private readonly TrieNode _root = new();

    public WordSolver(IEnumerable<string> dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        // Performance improvement: words share prefix nodes instead of storing separate prefix strings.
        foreach (var word in dictionary.Where(word => !string.IsNullOrWhiteSpace(word)))
        {
            var node = _root;
            foreach (var letter in word.Trim().ToLowerInvariant())
            {
                if (!node.Children.TryGetValue(letter, out var child))
                {
                    child = new TrieNode();
                    node.Children.Add(letter, child);
                }

                node = child;
            }

            node.IsWord = true;
        }
    }

    public IReadOnlyList<string> FindWords(string letters)
    {
        ArgumentNullException.ThrowIfNull(letters);

        var availableLetters = letters
            .Where(char.IsLetter)
            .Select(char.ToLowerInvariant)
            .ToArray();
        var matches = new SortedSet<string>();
        var used = new bool[availableLetters.Length];
        var candidate = new char[availableLetters.Length];

        GeneratePermutations(availableLetters, used, candidate, 0, _root, matches);
        return matches.ToArray();
    }

    private void GeneratePermutations(
        char[] letters,
        bool[] used,
        char[] candidate,
        int length,
        TrieNode node,
        SortedSet<string> matches)
    {
        if (node.IsWord)
        {
            matches.Add(new string(candidate, 0, length));
        }

        for (var i = 0; i < letters.Length; i++)
        {
            if (used[i])
            {
                continue;
            }

            // Performance improvement: a missing trie edge means no word starts with this candidate.
            if (!node.Children.TryGetValue(letters[i], out var child))
            {
                continue;
            }

            used[i] = true;
            candidate[length] = letters[i];
            GeneratePermutations(letters, used, candidate, length + 1, child, matches);
            used[i] = false;
        }
    }

    private sealed class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new();
        public bool IsWord { get; set; }
    }
}
