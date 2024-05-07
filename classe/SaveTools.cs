using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using System.Diagnostics;

namespace classe
{
    public class DirectoryPaths
    {
        public string SourceDir { get; set; }
        public string BackupDir { get; set; }
    }
    public class SaveTools
    {
        public static bool pauseRequested = false;
        public static bool stopRequested = false;

        public static void Pause()
        {
            pauseRequested = true;
        }

        public static void Stop()
        {
            stopRequested = true;
        }

        public static void Restart()
        {
            pauseRequested = false;
        }

        public string sourceDir { get; set; }
        public string backupDir { get; set; }
        public static SaveTools savetools = new SaveTools();
        public static LanguageBase lang = new LanguageBase();
        public static FileProcessor fileproc = new FileProcessor();

        public static string CreateFolder(string baseDirectory)
        {
            string folderName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            string folderPath = Path.Combine(baseDirectory, folderName);

            if (!Directory.Exists(folderPath))
            {
                // Créez le dossier
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Console.WriteLine("");
            }

            return folderPath;
        }

        public static void Differential(FileProcessor fileproc, List<string[]> fileInfoList, string sourceFilePath, string destinationFilePath, string sourceDir, string backupDir, ulong data, string fName, bool bool_suprresion_fichiers)
        {
            Log fonc_logreel = new();
            string[] dirInfo = Directory.GetFileSystemEntries(backupDir);
            string pathsourcetxt = Path.Combine(fName, sourceFilePath);
            List<string> writeTime = new List<string>();
            try
            {
                foreach (string path in dirInfo)
                {
                    string MostRecentFile = Path.Combine(path, fName);
                    if (File.GetLastWriteTime(MostRecentFile) < File.GetLastWriteTime(pathsourcetxt))
                    {
                        writeTime.Add(path);
                        for (int i = 0; i < writeTime.Count - 1; i++)
                        {
                            string file = Path.Combine(writeTime[i], fName);
                            string file1 = Path.Combine(writeTime[i + 1], fName);
                            if (File.GetLastWriteTime(file) < File.GetLastWriteTime(file1))
                            {
                                File.Copy(sourceFilePath, destinationFilePath, true);
                                Console.WriteLine("try success");
                                //fonc_logreel.logtempreel(fileproc.sauvegardename, "success", "NONE", sourceDir, backupDir, fileInfoList.Count, data, 2);
                            }
                            else
                            {
                                Console.WriteLine("File hasn't been modified");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Le fichier n'a pas été modifié depuis la dernière sauvegarde.");
                    }
                }
            }
            catch (Exception copyError)
            {
                Console.WriteLine(copyError.Message); // Catch exception if the file was already copied.
                Console.WriteLine("try error");
                //fonc_logreel.logtempreel(fileproc.sauvegardename, "success", "Error incremente", sourceDir, backupDir, fileInfoList.Count, data, 2);
            }
            if (bool_suprresion_fichiers == true) { File.Delete(sourceFilePath); }

        }

        public static void FullBackUp(FileProcessor fileproc, string destinationFilePath, string fName, string sourceFilePath, string sourceDir, string backupDir, List<string[]> fileInfoList, ulong data, bool bool_suprresion_fichiers)
        {
            Log log = new();
            // sauvegarde compléte
            try
            {
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                    Console.WriteLine($"Fichier existant '{fName}' supprimé avant la copie.");
                }

                File.Copy(sourceFilePath, destinationFilePath, true);
                Console.WriteLine($"Fichier '{fName}' copié avec succès.");
                log.SaveLog(sourceDir, backupDir, fName);
                //fonc_logreel.logtempreel(fileproc.sauvegardename, "success", "NONE", sourceDir, backupDir, fileInfoList.Count, data, 2);
            }
            catch (IOException copyError)
            {
                Console.WriteLine($"Erreur lors de la copie de '{fName}': {copyError.Message}");
                //fonc_logreel.logtempreel(fileproc.sauvegardename, "success", "Error incremente", sourceDir, backupDir, fileInfoList.Count, data, 2);
            }
            if (bool_suprresion_fichiers == true) { File.Delete(sourceFilePath); }
        }

        public static void Progress(int currentFileIndex, int totalFiles)
        {
            double progressPercentage = (double)currentFileIndex / totalFiles * 100;
            Console.Write($"\r[{new string('=', (int)progressPercentage / 2)}{new string(' ', 50 - (int)progressPercentage / 2)}] {progressPercentage:F2}% ");
        }

        public static DirectoryPaths AskDirectory(string langue, int nbr_save)
        {
            Console.WriteLine(JsonTemplate.GetJson(langue, "choicestainname"));
            fileproc.sauvegardename = Console.ReadLine();
            fileproc.sauvegardename = fileproc.sauvegardename + '_' + nbr_save;

            // Utilisation des propriétés de l'instance plutôt que d'une instance statique
            savetools.sourceDir = fileproc.checksource(langue);
            savetools.backupDir = fileproc.checktarget(langue);

            return new DirectoryPaths
            {
                SourceDir = savetools.sourceDir,
                BackupDir = savetools.backupDir
            };
        }

        public static async Task save(bool bool_suprresion_fichiers, bool sauvegardetype, string langue, string sourceDir, string backupDir)
        {

            List<string[]> fileInfoList = fileproc.GetFileInfo(savetools.sourceDir);
            //Console.WriteLine("debut de la sauvegarde");
            DateTime time_start = DateTime.Now;
            ulong data = 0;
            Console.WriteLine("Progress:");
            int totalFiles = fileInfoList.Count;
            int currentFileIndex = 0;
            List<Task> saveTasks = new List<Task>();
            foreach (string[] fileInfo in fileInfoList)
            {


                string fName = fileInfo[0];
                string sourceFilePath = Path.Combine(sourceDir, fName);
                string destinationFilePath1 = CreateFolder(backupDir);
                string destinationFilePath = Path.Combine(destinationFilePath1, fName);
                Console.WriteLine(fileInfo[2]);
                currentFileIndex++;

                foreach (string[] currentFile in fileInfoList)
                {
                    //Console.WriteLine($"Nom: {currentFile[0]}, Extension: {currentFile[1]}, Taille: {currentFile[2]}");
                    data += ulong.Parse(currentFile[2]);
                    Progress(currentFileIndex, totalFiles);
                    
                }
                // la sauvegarde on la lance en dessous 

                Task saveTask = Task.Run(async () =>
                {
                    if (sauvegardetype)
                    {
                        // sauvegarde Diff
                        await Task.Run(() => Differential(fileproc, fileInfoList, sourceFilePath, destinationFilePath, sourceDir, backupDir, data, fName, bool_suprresion_fichiers));
                    }
                    else
                    {
                        // sauvegarde complète
                        await Task.Run(() => FullBackUp(fileproc, destinationFilePath, fName, sourceFilePath, sourceDir, backupDir, fileInfoList, data, bool_suprresion_fichiers));
                    }
                });

                saveTasks.Add(saveTask);
            }
            await Task.WhenAll(saveTasks);


            DateTime time_stop = DateTime.Now;
            TimeSpan duration = time_start - time_stop;

            Console.WriteLine(fileInfoList);

            Log fonc_log = new();
            fonc_log.logcomplet(
                fileproc.sauvegardename,
            "SUCCED",
                "Cut original file = " + lang.cutcopy + "test.sauvegardetype : " + fileproc.sauvegardetype,
                sourceDir,
                backupDir,
                duration,
                data); //fileInfo in fileInfoList
        }

        public static void saveIHM(bool bool_suprresion_fichiers, bool sauvegardetype, string sourceDir, string backupDir)
        {

            List<string[]> fileInfoList = fileproc.GetFileInfo(sourceDir);
            //Console.WriteLine("debut de la sauvegarde");
            DateTime time_start = DateTime.Now;
            ulong data = 0;
            Console.WriteLine("Progress:");
            int totalFiles = fileInfoList.Count;
            int currentFileIndex = 0;
            List<Task> saveTasks = new List<Task>();
            foreach (string[] fileInfo in fileInfoList)
            {


                string fName = fileInfo[0];
                string sourceFilePath = Path.Combine(sourceDir, fName);
                string destinationFilePath1 = CreateFolder(backupDir);
                string destinationFilePath = Path.Combine(destinationFilePath1, fName);
                Console.WriteLine(fileInfo[2]);
                currentFileIndex++;

                foreach (string[] currentFile in fileInfoList)
                {
                    //Console.WriteLine($"Nom: {currentFile[0]}, Extension: {currentFile[1]}, Taille: {currentFile[2]}");
                    data += ulong.Parse(currentFile[2]);
                    Progress(currentFileIndex, totalFiles);
                }
                // la sauvegarde on la lance en dessous 


                if (sauvegardetype)
                {
                    // sauvegarde Diff
                    Differential(fileproc, fileInfoList, sourceFilePath, destinationFilePath, sourceDir, backupDir, data, fName, bool_suprresion_fichiers);
                }
                else
                {
                    // sauvegarde complète
                    FullBackUp(fileproc, destinationFilePath, fName, sourceFilePath, sourceDir, backupDir, fileInfoList, data, bool_suprresion_fichiers);
                }
            }

            DateTime time_stop = DateTime.Now;
            TimeSpan duration = time_start - time_stop;

            Console.WriteLine(fileInfoList);

            Log fonc_log = new();
            fonc_log.logcomplet(
                fileproc.sauvegardename,
            "SUCCED",
                "Cut original file = " + lang.cutcopy + "test.sauvegardetype : " + fileproc.sauvegardetype,
                sourceDir,
                backupDir,
                duration,
                data); //fileInfo in fileInfoList
        }

        static int CompterFichiersDansDossier(string cheminDossier)
        {
            try
            {
                string[] fichiers = Directory.GetFiles(cheminDossier);
                return fichiers.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
                return -1; // Une valeur négative peut être utilisée pour indiquer une erreur
            }
        }

        public static void SauvegarderDossier(string dossierSource, string dossierCible, bool viderDossierSource, bool sauvegardeDifferentielle, bool cryptosoft)
        {
            try
            {
                // Obtient la liste des fichiers du dossier source
                string[] fichiers = Directory.GetFiles(dossierSource);
                int totalFichiers = fichiers.Length;
                int fichiersCopies = 0;

                // Crée un thread pour afficher la barre de progression
                Thread progressBarThread = new Thread(() =>
                {
                    while (fichiersCopies < totalFichiers && !stopRequested)
                    {
                        if (!pauseRequested)
                        {
                            double progress = (double)fichiersCopies / totalFichiers;
                            AfficherBarreProgression(progress);
                        }
                        Thread.Sleep(500);
                    }
                });
                progressBarThread.Start();

                foreach (string fichier in fichiers)
                {
                    if (stopRequested)
                        break;

                    while (pauseRequested)
                    {
                        Thread.Sleep(1000); // Attendre avant de vérifier à nouveau
                    }

                    string nomFichier = Path.GetFileName(fichier);
                    string cheminDestination = Path.Combine(dossierCible, nomFichier);

                    if (sauvegardeDifferentielle)
                    {
                        string cheminSource = Path.Combine(dossierCible, nomFichier);
                        if (File.Exists(cheminSource))
                        {
                            DateTime lastWriteTimeSource = File.GetLastWriteTime(fichier);
                            DateTime lastWriteTimeDestination = File.GetLastWriteTime(cheminSource);
                            if (lastWriteTimeSource <= lastWriteTimeDestination)
                            {
                                // Le fichier source est plus ancien ou a la même date de modification que le fichier de destination, donc on ne le copie pas.
                                fichiersCopies++;
                                continue;
                            }
                        }
                    }

                    File.Copy(fichier, cheminDestination, true);
                    fichiersCopies++;

                    // Mise à jour de la barre de progression
                    double progress = (double)fichiersCopies / totalFichiers;
                    AfficherBarreProgression(progress);
                }

                // Si demandé, vide le dossier source après la sauvegarde
                if (viderDossierSource && !stopRequested)
                {
                    SupprimerFichiersDansDossier(dossierSource);
                }

                if (cryptosoft)
                {
                    // Exécute Cryptosoft.exe avec les paramètres appropriés
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "testingproject.exe";
                    startInfo.Arguments = $"{dossierCible} C";
                    Process.Start(startInfo);
                }

                if (stopRequested)
                    Console.WriteLine("Sauvegarde interrompue.");
                else
                    Console.WriteLine("\nSauvegarde terminée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }




        static void SupprimerFichiersDansDossier(string cheminDossier)
        {
            try
            {
                string[] fichiers = Directory.GetFiles(cheminDossier);
                foreach (string fichier in fichiers)
                {
                    File.Delete(fichier);
                }
                Console.WriteLine($"Suppression des fichiers dans le dossier '{cheminDossier}' terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la suppression des fichiers : {ex.Message}");
            }
        }

        static long TailleDossier(string cheminDossier)
        {
            // Vérifier si le dossier existe
            if (!Directory.Exists(cheminDossier))
            {
                // Si le dossier n'existe pas, renvoyer -1
                return -1;
            }

            // Obtenir la taille du dossier
            long taille = 0;
            string[] fichiers = Directory.GetFiles(cheminDossier, "*", SearchOption.AllDirectories);
            foreach (string fichier in fichiers)
            {
                FileInfo infoFichier = new FileInfo(fichier);
                taille += infoFichier.Length;
            }

            return taille;
        }

        static void AfficherBarreProgression(double progress)
        {
            int barLength = 40;
            Console.Write("\r[");
            int progressLength = (int)(progress * barLength);
            for (int i = 0; i < barLength; i++)
            {
                if (i < progressLength)
                    Console.Write("=");
                else
                    Console.Write(" ");
            }
            Console.Write($"] {Math.Round(progress * 100)}%");
        }

    }
   
}



