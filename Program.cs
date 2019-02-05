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
            string zipFilePath;
            double zipFileSize;
            string extractFolderPath;
            double initialDirectorySize;
            double directorySizeWithUnzippedFiles;
            double unzippedFilesSize;
            double ratioInPercents;


            Console.WriteLine("Input the path to a zip archive to be extracted including its file name and extention and press [Enter] button:");
            zipFilePath = Console.ReadLine();
            Console.WriteLine("Input the path to the folder where the zip archive will be extracted and press [Enter] button:");
            extractFolderPath = Console.ReadLine();

            try
            {
                DirectoryInfo info = new DirectoryInfo(extractFolderPath);
                initialDirectorySize = info.EnumerateFiles().Sum(file => file.Length);

                zipFileSize = new FileInfo(zipFilePath).Length;

                ZipFile.ExtractToDirectory(zipFilePath, extractFolderPath);
                Console.WriteLine($"Zip archive successfully extracted to the \"{extractFolderPath}\" folder");

                directorySizeWithUnzippedFiles = info.EnumerateFiles().Sum(file => file.Length);

                unzippedFilesSize = directorySizeWithUnzippedFiles - initialDirectorySize;
                // Compession ratio calculation based on https://en.wikipedia.org/wiki/Data_compression_ratio (see " space savings" equation)
                ratioInPercents = (1 - zipFileSize / unzippedFilesSize) * 100;

                Console.WriteLine($"Compression ratio of your archive was {ratioInPercents} %");
            }
            catch
            {
                Console.WriteLine("Something went wrong. Try again.");
                UnZip();
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