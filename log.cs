// Log.cs
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Xml.Linq;
namespace classe
{
    public class Log
    {
        public bool logJson;
        public bool logXml;
        public bool logTxt;
        private readonly string SaveLogFile = "Savelogfile.json";
        private readonly string logJsonPath = "log.json";
        private readonly string logXmlPath = "log.xml";
        private readonly string logTxtPath = "log.txt";
        public void SaveLog(string Spath, string Tpath, string Sname)
        {
            var logData = new
            {
                Sname,
                Spath,
                Tpath,
            };

            try
            {
                // Chemin complet du fichier à la racine du projet
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveLogFile);

                string jsonContent = JsonSerializer.Serialize(logData, new JsonSerializerOptions { WriteIndented = true });

                if (!File.Exists(fullPath))
                {
                    File.WriteAllText(fullPath, jsonContent);
                }
                else
                {
                    File.AppendAllText(fullPath, $"{Environment.NewLine}{jsonContent}");
                }

                Console.WriteLine($"Log JSON written to: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to {SaveLogFile}: {ex.Message}");
            }
        }
        public string[] ReadLogFile()
        {
            try
            {
                string SaveLogFile = "Savelogfile.json"; // Nom du fichier de log
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveLogFile);

                if (File.Exists(fullPath))
                {
                    // Lire le contenu du fichier
                    string fileContent = File.ReadAllText(fullPath);
                    JsonDocument jsonLog = JsonDocument.Parse(fileContent);
                    JsonElement logElement = jsonLog.RootElement;

                    if (jsonLog.RootElement.TryGetProperty("Sname", out JsonElement snameElement) &&
                jsonLog.RootElement.TryGetProperty("Tpath", out JsonElement tpathElement) &&
                jsonLog.RootElement.TryGetProperty("Spath", out JsonElement spathElement))
                    {
                        // Vérifier si les propriétés existent avant d'essayer de les extraire
                        string Sname = snameElement.GetString();
                        string Tpath = tpathElement.GetString();
                        string Spath = spathElement.GetString();

                        // Retourner les valeurs
                        return new string[] { Sname, Tpath, Spath };
                    }
                    else
                    {
                        Console.WriteLine("Certaines propriétés nécessaires n'existent pas dans le fichier JSON.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Le fichier {SaveLogFile} n'existe pas.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier de log : {ex.Message}");
                return null;
            }
        }


        public void ConfigurerTypeLog(bool logJson, bool logXml, bool logTxt, string sauvegardename, string status, string error, string sourceDir, string backupDir, int fileCount, ulong dataSize, int fileType, TimeSpan duration)
        {
            if (logJson == true)
            {
                LogJson(sauvegardename, status, error, sourceDir, backupDir, fileCount, dataSize, fileType, duration);
            }
            if (logXml == true)
            {
                LogXml(sauvegardename, status, error, sourceDir, backupDir, duration, dataSize);
            }
            if (logTxt == true)
            {
                logcomplet(sauvegardename, status, error, sourceDir, backupDir, duration, dataSize);
            }
        }



        public void LogJson(string sauvegardename, string status, string error, string sourceDir, string backupDir, int fileCount, ulong dataSize, int fileType, TimeSpan duration)
        {
            var logData = new
            {
                Sauvegardename = sauvegardename,
                Status = status,
                Error = error,
                SourceDir = sourceDir,
                BackupDir = backupDir,
                FileCount = fileCount,
                DataSize = dataSize,
                FileType = fileType,
                Duration = duration
            };

            try
            {
                string jsonContent = JsonSerializer.Serialize(logData, new JsonSerializerOptions { WriteIndented = true });

                if (!File.Exists(logJsonPath))
                {
                    File.WriteAllText(logJsonPath, jsonContent);
                }
                else
                {
                    File.AppendAllText(logJsonPath, $"{Environment.NewLine}{jsonContent}");
                }

                Console.WriteLine($"Log JSON written to: {logJsonPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log.json: {ex.Message}");
            }
        }

        public void LogXml(string sauvegardename, string status, string error, string sourceDir, string backupDir, TimeSpan duration, ulong dataSize)
        {
            try
            {
                string xmlContent = $@"<Log>
                                     <Sauvegardename>{sauvegardename}</Sauvegardename>
                                     <Status>{status}</Status>
                                     <Error>{error}</Error>
                                     <SourceDir>{sourceDir}</SourceDir>
                                     <BackupDir>{backupDir}</BackupDir>
                                     <Duration>{duration}</Duration>
                                     <DataSize>{dataSize}</DataSize>
                                   </Log>";

                if (!File.Exists(logXmlPath))
                {
                    File.WriteAllText(logXmlPath, xmlContent);
                }
                else
                {
                    File.AppendAllText(logXmlPath, $"{Environment.NewLine}{xmlContent}");
                }

                Console.WriteLine($"Log XML written to: {logXmlPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log.xml: {ex.Message}");
            }
        }

        public string logcomplet(
                    string save_name,
                    string status,
                    string error_message,
                    string file_source_path,
                    string file_target_path,
                    TimeSpan duration,
                    ulong file_size)
        {

            try
            {
                long valeurLong = (long)file_size;

                string logFilePath = "log_application.txt";

                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine($"Horodatage : {save_name}");
                    sw.WriteLine($"Sauvegarde : {save_name}");
                    sw.WriteLine($"Statut : {status}");
                    sw.WriteLine($"Error : {error_message}");
                    sw.WriteLine($"Source : {file_source_path}");
                    sw.WriteLine($"Destination : {file_target_path}");
                    sw.WriteLine($"Date/Heure fin de tache : {DateTime.Now}");
                    sw.WriteLine($"duration : {duration}");
                    sw.WriteLine($"Size (bytes): {valeurLong}");
                    sw.WriteLine(); // Ligne vide pour séparer les entrées dans le fichier log
                }

                return "Informations ajoutées au fichier log avec succès.";
            }
            catch (Exception ex)
            {
                return "Erreur lors de l'écriture dans le fichier log : {ex.Message}";
            }
        }

        internal void LogJson(string v1, string v2, string v3, string sourceDir, string backupDir, int count, ulong v4, int v5)
        {
            throw new NotImplementedException();
        }
    }
}

/*
         Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start(); // Start measuring tim

        Log fonc_logreel = new Log();
        fonc_logreel.LogJson(
            "Sauvegarde name", // string sauvegardename
            "SUCCES", // string status
            "No Error", // string error
            "C:\\Archive\\CESI_1\\BDD", // string sourceDir
            "C:\\Archive\\CESI_1\\BTP", // string backupDir
            1, // int fileCount
            1000,// ulong dataSize
            1, // int fileType
            stopwatch.Elapsed); // TimeSpan duration


        fonc_logreel.LogXml(
            "Sauvegarde name", // string sauvegardename
            "SUCCES", // string status
            "No Error", // string error
            "C:\\Archive\\CESI_1\\BDD", // string sourceDir
            "C:\\Archive\\CESI_1\\BTP", // string backupDir
            stopwatch.Elapsed,// ulong dataSize
            1000); // TimeSpan duration
      
 
 */