using System.Threading.Tasks;
using System.Xml.Linq;
namespace classe
{
    public class DirectoryPaths
    {
        public string SourceDir { get; set; }
        public string BackupDir { get; set; }
    }
    public class SaveTools
    {
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

        public static void Differential(FileProcessor fileproc, List<string[]> fileInfoList, string sourceFilePath, string destinationFilePath, string sourceDir, string backupDir, ulong data, string fName, bool temp_lang_cc)
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
            if (temp_lang_cc == true) { File.Delete(sourceFilePath); }

        }

        public static void FullBackUp(FileProcessor fileproc, string destinationFilePath, string fName, string sourceFilePath, string sourceDir, string backupDir, List<string[]> fileInfoList, ulong data, bool temp_lang_cc)
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
            if (temp_lang_cc == true) { File.Delete(sourceFilePath); }
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
        public static async Task save(bool temp_lang_cc, bool sauvegardetype, string langue, string sourceDir, string backupDir)
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
                        await Task.Run(() => Differential(fileproc, fileInfoList, sourceFilePath, destinationFilePath, sourceDir, backupDir, data, fName, temp_lang_cc));
                    }
                    else
                    {
                        // sauvegarde complète
                        await Task.Run(() => FullBackUp(fileproc, destinationFilePath, fName, sourceFilePath, sourceDir, backupDir, fileInfoList, data, temp_lang_cc));
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
        public static void saveIHM(bool temp_lang_cc, bool sauvegardetype, string sourceDir, string backupDir)
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
                    Differential(fileproc, fileInfoList, sourceFilePath, destinationFilePath, sourceDir, backupDir, data, fName, temp_lang_cc);
                }
                else
                {
                    // sauvegarde complète
                    FullBackUp(fileproc, destinationFilePath, fName, sourceFilePath, sourceDir, backupDir, fileInfoList, data, temp_lang_cc);
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
    }
   
}



