//using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.IO;

using classe;
using static classe.Log;
using static classe.FileProcessor;
using System.Globalization;
using System.Net.NetworkInformation;
using static classe.SaveTools;
// coucou
class Program
{
    public static LanguageBase lang; // Utilisez une classe de base pour la variable lang
    public static FileProcessor fileproc;

    static async Task Main()
    {
        Config config = Config.LoadConfig();
        fileproc = new FileProcessor();
        SaveTools savetools = new SaveTools();
        MessageAccueil.Menu();

        // language choice
        string langue = LanguageTools.SelectLanguage(config.Langue);
        config.Langue = langue;
        //LanguageTools.lang_str = config.Langue;
        Config.SaveConfig(config);

        // number of file
        Console.WriteLine(JsonTemplate.GetJson(langue, "SaveN"));
        int nbr_save = LanguageTools.GetNumberOfFolders();
        Task[] saveTasks = new Task[nbr_save];
        List<SauvegardeInfo> sauvegardes = new List<SauvegardeInfo>();

        for (int i = 0; i < nbr_save; i++)
        {
            Console.WriteLine(JsonTemplate.GetJson(langue, "nsave"));
            LanguageTools.ChooseCutCopy(lang);
            LanguageTools.t_save();
            DirectoryPaths directoryPaths = SaveTools.AskDirectory(langue, i);

            // Ajouter les informations de sauvegarde à la liste
            sauvegardes.Add(new SauvegardeInfo
            {
                TempLangCC = LanguageTools.temp_lang_cc,
                SauvegardeType = LanguageTools.sauvegardetype,
                Langue = langue,
                SourceDir = directoryPaths.SourceDir,
                BackupDir = directoryPaths.BackupDir
            });
        }

        // Maintenant que toutes les saisies sont faites, exécutez les sauvegardes
        foreach (var sauvegarde in sauvegardes)
        {
            await Task.Run(async () =>
            {
                await SaveTools.save(sauvegarde.TempLangCC, sauvegarde.SauvegardeType, sauvegarde.Langue, sauvegarde.SourceDir, sauvegarde.BackupDir);
            });
        }
    }
}

