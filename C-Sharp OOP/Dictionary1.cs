namespace C_Sharp_OOP
{
    using System.Text.Json;

    namespace ConsoleApp1
    {
        public class Dictionary1
        {
            private const string DictionariesFolder = "Dictionaries";

            public string Name { get; set; }
            public Dictionary<string, List<string>> Words { get; set; }

            public Dictionary1(string name)
            {
                Name = name;
                Words = new Dictionary<string, List<string>>();
            }

            public void AddWord(string word, List<string> translations)
            {
                try
                {
                    if (Words.ContainsKey(word))
                    {
                        foreach (var translation in translations)
                        {
                            if (!Words[word].Contains(translation))
                            {
                                Words[word].Add(translation);
                            }
                        }
                    }
                    else
                    {
                        Words[word] = new List<string>(translations);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while adding word: {ex.Message}");
                }
            }

            public void ReplaceWord(string oldWord, string newWord)
            {
                try
                {
                    if (Words.ContainsKey(oldWord))
                    {
                        List<string> translations = Words[oldWord];
                        Words.Remove(oldWord);
                        Words[newWord] = new List<string>(translations);
                    }
                    else
                    {
                        Console.WriteLine("The specified word does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while replacing word: {ex.Message}");
                }
            }

            public void ReplaceTranslation(string word, string oldTranslation, string newTranslation)
            {
                try
                {
                    if (Words.ContainsKey(word))
                    {
                        var translations = Words[word];
                        if (translations.Contains(oldTranslation))
                        {
                            translations.Remove(oldTranslation);
                            translations.Add(newTranslation);
                        }
                        else
                        {
                            Console.WriteLine("The specified translation does not exist.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The specified word does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while replacing translation: {ex.Message}");
                }
            }

            public void DeleteWord(string word)
            {
                try
                {
                    if (Words.ContainsKey(word))
                    {
                        Words.Remove(word);
                    }
                    else
                    {
                        Console.WriteLine("The specified word does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while deleting word: {ex.Message}");
                }
            }

            public void DeleteTranslation(string word, string translation)
            {
                try
                {
                    if (Words.ContainsKey(word))
                    {
                        var translations = Words[word];
                        if (translations.Contains(translation))
                        {
                            translations.Remove(translation);
                        }
                        else
                        {
                            Console.WriteLine("The specified translation does not exist.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The specified word does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while deleting translation: {ex.Message}");
                }
            }

            public void ShowAllDictionary()
            {
                try
                {
                    if (Words.Count == 0)
                    {
                        Console.WriteLine("Dictionary is empty");
                        return;
                    }

                    Console.WriteLine($"Dictionary: {Name}");
                    Console.WriteLine("------------------------------");

                    foreach (var word in Words)
                    {
                        Console.WriteLine($"Word: {word.Key}");

                        for (int i = 0; i < word.Value.Count; i++)
                        {
                            Console.WriteLine($"\tTranslation {i + 1}: {word.Value[i]}");
                        }
                        Console.WriteLine("------------------------------");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while showing dictionary: {ex.Message}");
                }
            }

            public List<string> ShowAllTranslationToWord(string word)
            {
                try
                {
                    if (Words.ContainsKey(word))
                    {
                        return Words[word];
                    }
                    else
                    {
                        return new List<string>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while showing translations: {ex.Message}");
                    return new List<string>();
                }
            }

            public void ExportWordToFile(string word)
            {
                try
                {
                    if (Words.ContainsKey(word))
                    {
                        string fileName = $"{Name.Replace(" ", "_")}_dictionary.json";
                        Dictionary<string, List<string>> exportData;

                        if (File.Exists(fileName))
                        {
                            string existingJson = File.ReadAllText(fileName);
                            exportData = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(existingJson);
                        }
                        else
                        {
                            exportData = new Dictionary<string, List<string>>();
                        }

                        if (exportData.ContainsKey(word))
                        {
                            foreach (var translation in Words[word])
                            {
                                if (!exportData[word].Contains(translation))
                                {
                                    exportData[word].Add(translation);
                                }
                            }
                        }
                        else
                        {
                            exportData[word] = Words[word];
                        }

                        string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(fileName, json);

                        Console.WriteLine($"Word '{word}' successfully exported to file {fileName}");
                    }
                    else
                    {
                        Console.WriteLine("The specified word does not exist in the dictionary.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while exporting word: {ex.Message}");
                }
            }

            public void ExportDictionaryToFile()
            {
                try
                {
                    if (!Directory.Exists(DictionariesFolder))
                    {
                        Directory.CreateDirectory(DictionariesFolder);
                    }

                    string fileName = Path.Combine(DictionariesFolder, $"{Name.Replace(" ", "_")}_dictionary.json");
                    Dictionary<string, List<string>> exportData;

                    if (File.Exists(fileName))
                    {
                        string existingJson = File.ReadAllText(fileName);
                        exportData = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(existingJson);
                    }
                    else
                    {
                        exportData = new Dictionary<string, List<string>>();
                    }

                    foreach (var entry in Words)
                    {
                        exportData[entry.Key] = entry.Value;
                    }

                    string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(fileName, json);

                    Console.WriteLine($"Dictionary '{Name}' successfully exported to file {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while exporting dictionary: {ex.Message}");
                }
            }
        }
    }

}
