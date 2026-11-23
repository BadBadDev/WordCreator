# WordCreator

[![Build](https://github.com/BadBadDev/WordCreator/actions/workflows/build.yml/badge.svg)](https://github.com/BadBadDev/WordCreator/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A small C# console app that finds dictionary words which can be built from a set of entered letters.

```text
$ dotnet run --project src/WordCreator.csproj

Type letters (or 'exit'): listen
Valid words: 18
enlist
inlets
listen
silent
tinsel
```

## Educational purpose and limitations

This repository is primarily an exercise in combination algorithms and C# problem-solving. It is **not** a complete word-puzzle engine and should not be treated as a competitive or production-grade solver.

The original puzzle model explores possible arrangements of letters. That growth is fast:

```text
Ordered arrangements:

n = 1:       1
n = 5:       5 + 20 + 60 + 120 + 120 = 325
n = 10:      ~9.86 million ordered arrangements through all lengths
n = 15:      1.0 trillion+ ordered arrangements through all lengths
n = 20:      far beyond practical memory and CPU limits

The sum grows as Σ P(n, r), r = 1..n
```

The number of ordered arrangements of length `r` is:

```text
P(n, r) = n! / (n-r)!
```

If order does not matter, the number of selections is:

```text
C(n, r) = n! / (r!(n-r)! )
       = P(n, r) / r!
```

For example, 5 distinct letters can produce:

```text
                    P(5,5) = 120
                         |
                    5-letter
                         |
       P(5,4)=120   P(5,3)=60   P(5,2)=20   P(5,1)=5
          \             |            |          /
           \            |            |         /
                     input has 5 letters
                         |
                  do not repeat a letter
```

This app does not generate all permutations. It loads the bundled dictionary, counts available letter frequencies, and scans the dictionary once. That is still bounded by the dictionary size, but the output remains limited to words that can actually be built from the input.

## Build and run

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```powershell
dotnet run --project src/WordCreator.csproj
```

Build the solution:

```powershell
dotnet build WordCreator.sln
```

## Dictionary

`src/dictionary.json` contains approximately 275,000 words. Confirm and preserve the original dictionary's license and attribution before redistributing or publishing derivative datasets. The application currently assumes lowercase ASCII dictionary entries.

## License

Licensed under the MIT License. See [LICENSE](LICENSE).