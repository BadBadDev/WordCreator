using System.Text.Json;

namespace WordCreator;

public static class Program
{
    private static readonly ConsoleColor[] ConsoleColors =
    [
        ConsoleColor.Red,
        ConsoleColor.Green,
        ConsoleColor.Yellow,
        ConsoleColor.Blue,
        ConsoleColor.Magenta,
        ConsoleColor.Cyan
    ];

    public static void Main(string[] args)
    {
        var dictionaryPath = Path.Combine(AppContext.BaseDirectory, "dictionary.json");
        if (!File.Exists(dictionaryPath))
        {
            Console.Error.WriteLine("Could not find dictionary.json next to the application.");
            return;
        }

        try
        {
            var dictionary = JsonSerializer.Deserialize<HashSet<string>>(
                File.ReadAllText(dictionaryPath),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dictionary is null)
            {
                Console.Error.WriteLine("Could not load the dictionary.");
                return;
            }

            var solver = new WordSolver(dictionary);
            Run(solver);
        }
        catch (JsonException)
        {
            Console.Error.WriteLine("dictionary.json is not valid JSON.");
        }
    }

    private static void Run(WordSolver solver)
    {
        while (true)
        {
            Console.Write("\nType letters (or 'exit'): ");
            var input = Console.ReadLine();

            if (input is null || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var letters = new string(input
                .Where(char.IsLetter)
                .Select(char.ToLowerInvariant)
                .ToArray());

            if (letters.Length == 0)
            {
                Console.WriteLine("Please enter at least one letter.");
                continue;
            }

            var words = solver.FindWords(letters);
            Console.WriteLine($"Valid words: {words.Count}");

            var colorCounter = 0;
            foreach (var word in words)
            {
                Console.ForegroundColor = ConsoleColors[colorCounter];
                Console.WriteLine(word);
                colorCounter = (colorCounter + 1) % ConsoleColors.Length;
            }

            Console.ResetColor();
        }
    }
}
