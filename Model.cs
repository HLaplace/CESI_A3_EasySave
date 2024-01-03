
using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace classe
{
    public class FileProcessor
    {
        private string directoryPath;

        public string sauvegardename;

        private bool _sauvegardetype;
        public virtual bool sauvegardetype
        {
            get { return _sauvegardetype; }
            set { _sauvegardetype = value; }
        }
        public FileProcessor()
        {
            directoryPath = @"";
        }

        public string checksource(string langue) // méthode pour vérifier que les répertoires sont valides 
        {
            while (true)
            {
                Console.WriteLine(JsonTemplate.GetJson(langue, "inputmessages"));

                //Console.WriteLine(inputMessage);
                directoryPath = Console.ReadLine();
                try
                {
                    string[] fileList = Directory.GetFiles(directoryPath);
                    break;
                }
                catch (DirectoryNotFoundException dirNotFound)
                {
                    Console.WriteLine($"Erreur : {dirNotFound.Message}");
                    Console.WriteLine(JsonTemplate.GetJson(langue, "inputerrormessages"));

                    //Console.WriteLine(inputErrorMessage);
                }
            }
            return directoryPath;// Retournez le chemin du répertoire
        }

        public string checktarget(string langue) // méthode pour vérifier que les répertoires sont valides 
        {
            while (true)
            {
                Console.WriteLine(JsonTemplate.GetJson(langue, "inputmessage"));

                //Console.WriteLine(inputMessage);
                directoryPath = Console.ReadLine();
                try
                {
                    string[] fileList = Directory.GetFiles(directoryPath);
                    break;
                }
                catch (DirectoryNotFoundException dirNotFound)
                {
                    Console.WriteLine($"Erreur : {dirNotFound.Message}");
                    Console.WriteLine(JsonTemplate.GetJson(langue, "inputerrormessages"));

                    //Console.WriteLine(inputErrorMessage);
                }
            }
            return directoryPath;// Retournez le chemin du répertoire
        }

        public List<string[]> GetFileInfo(string sourceDir) // méthode pour lister les attributs des fichiers du répertoires
        {
            long number_of_byte_to_save = 0;
            List<string[]> fileInformationList = new List<string[]>();

            string[] fileList = Directory.GetFiles(sourceDir);

            foreach (string filePath in fileList)
            {

                string fileName = Path.GetFileName(filePath);
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    number_of_byte_to_save += fileInfo.Length;
                    string[] fileInfoArray = { fileName, Path.GetExtension(fileName), fileInfo.Length.ToString() };

                    Console.WriteLine($"Nom: {fileName}, Extension: {Path.GetExtension(fileName)}, Taille: {number_of_byte_to_save}");
                    fileInformationList.Add(fileInfoArray);
                }
                catch (IOException error)
                {
                    Console.WriteLine(error.Message);
                }
            }
            Console.WriteLine("fileInformationList :" + fileInformationList);
            return fileInformationList;

        }
    }
}