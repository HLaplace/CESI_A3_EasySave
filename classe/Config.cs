using System.IO;
using System.Text.Json;

namespace classe
{
    public class Config
    {
        public string Langue { get; set; }
        public string backgoundcolor { get; set; }
        public string forgroundcolor { get; set; }
        public int Entier3 { get; set; }

        public static Config LoadConfig()
        {
            string configFilePath = "Config.json";

            if (!File.Exists(configFilePath))
            {
                Config defaultConfig = new Config
                {
                    Langue = "ENG",
                    backgoundcolor = "#000000",
                    forgroundcolor = "#ffffff",
                    Entier3 = 42
                };

                SaveConfig(defaultConfig);
                return defaultConfig;
            }

            string json = File.ReadAllText(configFilePath);
            return JsonSerializer.Deserialize<Config>(json);
        }

        public static void SaveConfig(Config config)
        {
            string configFilePath = "Config.json";
            string configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFilePath, configJson);
            //Console.WriteLine($"Configuration sauvegardée dans : {configFilePath}");
        }
        public static void AfficherConfiguration(Config config)
        {
            Console.WriteLine($"Langue: {config.Langue}");
            Console.WriteLine($"backgoundcolor: {config.backgoundcolor}");
            Console.WriteLine($"forgroundcolor: {config.forgroundcolor}");
            Console.WriteLine($"Entier3: {config.Entier3}");
        }
    }
}