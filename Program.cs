using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ZipArchiveApp
{
    static class Program
    {
        static void UnZip()
        {
            Console.WriteLine("Input the path to a zip archive to be extracted including its file name and extention and press [Enter] button:");
            var zipFilePath = Console.ReadLine();
            Console.WriteLine("Input the path to the folder where the zip archive will be extracted and press [Enter] button:");
            var extractFolderPath = Console.ReadLine();

            // User input will be manipulated below - in order to avoid exceptions by the wrong input try-catch block used
            try
            {
                // Zip file name is needed to create a folder with the same name as the name of the zip archive to be extracted
                var zipFileName = Path.GetFileNameWithoutExtension(zipFilePath);
                // Extract folder path defined by the user appended by the zip file name folder
                extractFolderPath = Path.Combine(extractFolderPath, zipFileName);
                // Before extraction of the archive - extract folder created in the user defined folder
                Directory.CreateDirectory(extractFolderPath);
            }
            catch (Exception e) // When exception will be caught its message will be printed to help user fix the problem during next try 
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Can not extract the archive. Try again.");
                // When extraction will be failed by some reason - user will try again from scratch 
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

                // Next line calculate the size (in bytes) of the folder with extracted files (including nested folders)
                double directorySize = Directory.GetFiles(extractFolderPath, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));

                // Size calculation of the original zip file
                double zipFileSize = new FileInfo(zipFilePath).Length;
                // Compession ratio calculation according to the Exersice 2 requirements
                double ratioInPercents = (zipFileSize / directorySize) * 100; // Original formula results multiplied by 100 in order to display ratio in %

                Console.WriteLine($"Compression ratio of your archive was {ratioInPercents} %");
            }
            catch (Exception e) // When exception will be caught its message will be printed to help user fix the problem during next try 
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Can not extract the archive. Try again.");
                // When extraction will be failed by some reason - user will try again from scratch 
                UnZip();
            }
            // Oldest file age calculation code below
            try
            {
                // Finding the oldest file in the directory where the zip archive was extracted
                var directoryInfo = new DirectoryInfo(extractFolderPath);
                var oldestFilePath = directoryInfo.GetFiles("*", SearchOption.AllDirectories).OrderByDescending(o => o.LastWriteTime).LastOrDefault().FullName.ToString();

                if (File.Exists(oldestFilePath))
                {   // Age of the file calculated in days compared to current system time
                    var creation = File.GetLastWriteTime(oldestFilePath);
                    var currentDateTime = DateTime.Now;
                    var timeElapsed = currentDateTime.Subtract(creation); // Get the TimeSpan of the difference between now and the oldest file modification date
                    double daysAgo = timeElapsed.TotalDays;
                    Console.WriteLine($"The age of the oldest file in the provided zip archive is {daysAgo} days.");
                }

            }
            // Exeption occurs when zip archive contains only folders inside, they caught in the below catch block
            catch
            {
                Console.WriteLine("Your zip archive did not contain any files. The age of the oldest file can not be calculated");
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