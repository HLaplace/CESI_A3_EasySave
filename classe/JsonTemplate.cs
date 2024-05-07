using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace classe
{
    public class JsonTemplate
    {
        public static string GetJson(string inputLanguage, string state)
        {
            // Obtenir le répertoire du projet (répertoire du fichier source)
            string repertoireProjet = AppContext.BaseDirectory;
            // Remonter de quatre niveaux pour obtenir le répertoire du projet
            for (int i = 0; i < 5; i++)
            {
                repertoireProjet = Directory.GetParent(repertoireProjet).FullName;
            }
            repertoireProjet = repertoireProjet + "\\classe" + "\\language\\";

            // Construire le chemin d'accès au fichier JSON en utilisant le répertoire du projet
            string cheminFichier;

            switch (inputLanguage)
            {
                case "FR":
                    cheminFichier = Path.Combine(repertoireProjet, "language_fr.json");
                    break;
                case "ENG":
                    cheminFichier = Path.Combine(repertoireProjet, "language_eng.json");
                    break;
                case "DE":
                    cheminFichier = Path.Combine(repertoireProjet, "language_de.json");
                    break;

                default:
                    //cheminFichier = Path.Combine(repertoireProjet, "language_eng.json");
                    throw new ArgumentException("Invalid language. Please choose between FR, ENG, or DE.");
                    break;

            }

            string jsonContent = File.ReadAllText(cheminFichier);
            JsonDocument jsonDoc = JsonDocument.Parse(jsonContent);
            JsonElement root = jsonDoc.RootElement;

            // Utilisation d'un dictionnaire pour stocker les états et leurs valeurs associées
            var stateDictionary = new Dictionary<string, string>
            {
                {"welcome", "welcome" },
                { "defaultlang", "defaultlang" },
                { "setlang", "setlang" },
                { "error_setlang", "error_setlang" },
                { "choicecutcopy", "choicecutcopy" },
                { "validationchoicecut", "validationchoicecut" },
                { "validationchoicecopy", "validationchoicecopy" },
                { "choicestainname", "choicestainname" },
                { "inputmessages", "inputmessages" },
                { "inputerrormessages", "inputerrormessages" },
                { "inputmessage", "inputmessage" },
                { "savetype", "savetype" },
                { "nsave", "nsave" },
                { "sucess", "sucess" },
                { "SaveN", "SaveN" },
                { "config", "config" },
                { "log_type", "log_type" },
                { "validate", "validate" },
                { "annul", "annul" },
                { "back", "back" },
                { "get_number", "get_number" },
                { "invalid_enter", "invalid_enter" },
                { "invalid_input", "invalid_input" },
                { "fbackup","fbackup"},
                { "dbackup","dbackup"}
            };

            // Vérifier si l'état existe dans le dictionnaire avant de renvoyer la valeur JSON
            if (stateDictionary.ContainsKey(state))
            {
                return root.GetProperty(state).GetString();
            }

            // Si l'état n'est pas trouvé, renvoyer une chaîne vide ou gérer l'erreur selon vos besoins
            return string.Empty;
        }
    }
}