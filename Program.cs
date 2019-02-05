using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ZipArchiveApp
{
    static class Program
    {
        static public void UnZip()
        {
            string zipFilePath;
            double zipFileSize;
            string extractFolderPath;
            double directorySize;
            double ratioInPercents;
            string zipFileName;
            string oldestFile = "";

            Console.WriteLine("Input the path to a zip archive to be extracted including its file name and extention and press [Enter] button:");
            zipFilePath = Console.ReadLine();
            Console.WriteLine("Input the path to the folder where the zip archive will be extracted and press [Enter] button:");
            extractFolderPath = Console.ReadLine();

            // User input will be manipulated below - in order to avoid exeption by the wrong input try-catch block used
            try
            {
                // Zip file name is needed to create a folder with the same name as the name of the zip archive to be extracted
                zipFileName = Path.GetFileNameWithoutExtension(zipFilePath);
                // Extract folder path defined by the user appended by the zip file name folder
                extractFolderPath = Path.Combine(extractFolderPath, zipFileName);
                // Before extraction of the archive - extract folder created in the user defined folder
                Directory.CreateDirectory(extractFolderPath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Try again.");
                UnZip();
            }


            /* 
              In the below try-catch block the program does extraction of the user defined zip archive to the 
              user defined folder and also calculates and prints compession ratio of the zip archive;
              try-catch block is used to catch errors which may occur during zip archive extraction;
              nothing was implemented for catching particular errors - user will be prompted to try again.
             */
            try
            {

                ZipFile.ExtractToDirectory(zipFilePath, extractFolderPath);
                Console.WriteLine($"Zip archive successfully extracted to the \"{extractFolderPath}\" folder");

                // Next two lines calculate the size (in bytes) of the folder with extracted files
                DirectoryInfo info = new DirectoryInfo(extractFolderPath);
                directorySize = info.EnumerateFiles().Sum(file => file.Length);
                // Size calculation of the original zip file
                zipFileSize = new FileInfo(zipFilePath).Length;
                // Compession ratio calculation based on https://en.wikipedia.org/wiki/Data_compression_ratio (see " space savings" equation)
                ratioInPercents = (1 - zipFileSize / directorySize) * 100;

                Console.WriteLine($"Compression ratio of your archive was {ratioInPercents} %");
            }
            catch
            {
                // "Something went wrong." message will be shown instead of printing the exeption in order to not scary the user
                Console.WriteLine("Something went wrong. Try again."); 
                UnZip();
            }
            try
            {
                oldestFile = (new DirectoryInfo(extractFolderPath).GetFiles().OrderByDescending(o => o.LastWriteTime).LastOrDefault()).ToString();
            }
            catch
            {
                Console.WriteLine("Your zip archive did not contain any files. The age of the oldest file can not be calculated");
            }
            
            string oldestFilePath = Path.Combine(extractFolderPath, oldestFile);
            if (File.Exists(oldestFilePath))
            {
                DateTime creation = File.GetLastWriteTime(oldestFilePath);
                var currentDateTime = DateTime.Now;
                TimeSpan timeElapsed = currentDateTime.Subtract(creation); // Get the TimeSpan of the difference between now and the oldest file modification date
                double daysAgo = timeElapsed.TotalDays;
                Console.WriteLine($"The age of the oldest file in the provided zip archive is {daysAgo} days.");
            }
        }
        
        static void Main()
        {
            UnZip();
            Console.WriteLine("Press any key to close the screen...");
            Console.ReadKey();
        }
    }
}