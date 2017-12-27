using GA.ImageProperties;
using System;
using System.IO;

namespace GA.ArrangeByDateTaken
{
    class Program
    {
        static void Main(string[] args)
        {

            string root = @"C:\Users\Andrew Davies\OneDrive\Pictures";
            string oldFolder = args[0];

            Console.WriteLine("Arrange photo's in '{0}' by date taken.", oldFolder);
            Console.WriteLine("Copy (C) or Move (M)?");
            var key = Console.ReadKey().KeyChar;

            if (key == 'c' || key == 'C') CopyFiles(root, oldFolder);
            if (key == 'm' || key == 'M') MoveFiles(root, oldFolder);

            Console.Read();
        }

        private static void CopyFiles(string root, string oldFolder)
        {
            foreach (string oldPath in Directory.GetFiles(oldFolder))
            {
                string newFolder = GetNewFolder(root, oldPath);

                CopyFile(newFolder, oldPath);

                Console.Write(".");
            }
        }

        private static void MoveFiles(string root, string oldFolder)
        {
            foreach (string oldPath in Directory.GetFiles(oldFolder))
            {
                string newFolder = GetNewFolder(root, oldPath);

                MoveFile(newFolder, oldPath);

                Console.Write(".");
            }
        }

        private static string GetNewFolder(string root, string oldPath)
        {
            string newFolder;

            try
            {
                FastImage img = new FastImage(oldPath);

                newFolder = String.Format("{0}\\{1:d4}\\{1:d4}-{2:d2}\\{1:d4}-{2:d2}-{3:d2}",
                    root, img.DateTaken.Year, img.DateTaken.Month, img.DateTaken.Day);
            }
            catch
            {
                newFolder = String.Format("{0}\\No Date", root);
            }

            return newFolder;
        }

        private static void CopyFile(string newFolder, string oldPath)
        {
            try
            {
                Directory.CreateDirectory(newFolder);

                string newPath = String.Format("{0}\\{1}", newFolder, Path.GetFileName(oldPath));

                if (!File.Exists(newPath)) File.Copy(oldPath, newPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} {1}", oldPath, e.Message);
            }
        }

        private static void MoveFile(string newFolder, string oldPath)
        {
            try
            {
                Directory.CreateDirectory(newFolder);

                string newPath = String.Format("{0}\\{1}", newFolder, Path.GetFileName(oldPath));

                if (File.Exists(newPath))
                {
                    File.Delete(oldPath);
                }
                else
                {
                    File.Move(oldPath, newPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} {1}", oldPath, e.Message);
            }
        }
    }
}
