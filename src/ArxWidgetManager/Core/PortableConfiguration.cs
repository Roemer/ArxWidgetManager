using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FlauLib.Tools
{
    public static class PortableConfiguration
    {
        private class ConfigurationItem
        {
            public string Section { get; set; }
            public string Content { get; set; }
        }

        private const string DefaultFilename = "settings";
        private static readonly object LockObject = new Object();
        private static readonly Regex SectionRegex = new Regex(@"\s*###CONFSECTION\s*(.*?)\s*###\s*(.*?)\s*###ENDCONFSECTION###\s*", RegexOptions.Compiled | RegexOptions.Singleline);

        public static void Save(string section, object settings, string fileName = DefaultFilename)
        {
            var items = LoadInternal(fileName);
            var foundItem = Find(section, items);
            if (foundItem != null)
            {
                // Replace
                foundItem.Content = ConvertToString(settings);
            }
            else
            {
                // Add
                items.Add(CreateItem(section, settings));
            }
            SafeInternal(items, fileName);
        }

        public static T Load<T>(string section, string fileName = DefaultFilename, bool createIfNotExists = true) where T : new()
        {
            var items = LoadInternal(fileName);
            var foundItem = Find(section, items);
            if (foundItem != null)
            {
                // Item found
                return JsonConvert.DeserializeObject<T>(foundItem.Content);
            }

            var settings = new T();
            if (createIfNotExists)
            {
                // Add new item
                items.Add(CreateItem(section, settings));
                SafeInternal(items, fileName);
            }
            return settings;
        }

        private static ConfigurationItem CreateItem(string section, object settings)
        {
            return new ConfigurationItem { Section = section, Content = ConvertToString(settings) };
        }

        private static string ConvertToString(object settings)
        {
            var parameters = new JsonSerializerSettings { Formatting = Formatting.Indented };
            return JsonConvert.SerializeObject(settings, parameters);
        }

        private static ConfigurationItem Find(string section, List<ConfigurationItem> items)
        {
            var foundItem = items.Find(x => x.Section.Equals(section, StringComparison.CurrentCultureIgnoreCase));
            return foundItem;
        }

        private static string CreateRealFileName(string fileName)
        {
            return fileName + ".json";
        }

        private static void SafeInternal(IEnumerable<ConfigurationItem> items, string fileName)
        {
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.AppendFormat(@"###CONFSECTION {0}###", item.Section).AppendLine();
                sb.AppendLine(item.Content);
                sb.AppendLine(@"###ENDCONFSECTION###").AppendLine();
            }

            var tempFile = fileName + ".temp";
            var realFile = CreateRealFileName(fileName);
            var bakFile = fileName + ".bak";
            lock (LockObject)
            {
                File.WriteAllText(tempFile, sb.ToString());
                if (File.Exists(realFile))
                {
                    if (File.Exists(bakFile))
                    {
                        File.Delete(bakFile);
                    }
                    File.Replace(tempFile, realFile, bakFile);
                }
                else
                {
                    File.Move(tempFile, realFile);
                }
            }
        }

        private static List<ConfigurationItem> LoadInternal(string fileName)
        {
            var settingsList = new List<ConfigurationItem>();
            var realFile = CreateRealFileName(fileName);
            lock (LockObject)
            {
                if (File.Exists(realFile))
                {
                    var fileContent = File.ReadAllText(realFile);
                    var matches = SectionRegex.Matches(fileContent);
                    foreach (Match match in matches)
                    {
                        var matchSection = match.Groups[1].Captures[0].Value;
                        var matchContent = match.Groups[2].Captures[0].Value;
                        settingsList.Add(new ConfigurationItem { Section = matchSection, Content = matchContent });
                    }
                }
            }
            return settingsList;
        }
    }
}
