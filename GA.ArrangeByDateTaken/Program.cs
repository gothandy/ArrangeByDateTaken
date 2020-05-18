using GA.ImageProperties;
using System;
using System.IO;

namespace GA.ArrangeByDateTaken
{
    class Program
    {
        enum Action
        {
            Copy,
            Move
        }

        static void Main(string[] args)
        {
            string oldFolder = args[0];

            Console.WriteLine($"Arrange photo's in '{oldFolder}' by date taken.");
            Console.WriteLine("Copy (C) or Move (M)?");

            var key = Console.ReadKey().KeyChar;

            if (key == 'c' || key == 'C') DoFiles(oldFolder, Action.Copy);
            if (key == 'm' || key == 'M') DoFiles(oldFolder, Action.Move);

            Console.Read();
        }

        private static void DoFiles(string oldFolder, Action action)
        {
            foreach (string oldPath in Directory.GetFiles(oldFolder))
            {
                string newFolder = GetNewFolder(oldPath);

                switch (action) {

                    case Action.Copy:
                        CopyFile(newFolder, oldPath);
                        break;

                    case Action.Move:
                        MoveFile(newFolder, oldPath);
                        break;
                }

                Console.Write(".");
            }
        }

        private static string GetNewFolder(string oldPath)
        {
            string root;
            DateTime dateTaken = DateTime.MinValue;

            string extension = Path.GetExtension(oldPath).ToUpper();

            switch (extension)
            {
                case ".MP4":
                    root = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                    dateTaken = File.GetCreationTime(oldPath);
                    break;

                default:
                    root = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    break;
            }

            try
            {
                FastImage img = new FastImage(oldPath);

                dateTaken = img.DateTaken;
            }
            catch
            {
                // No Date Info use default.
            }

            if (dateTaken == DateTime.MinValue)
            {
                return String.Format("{0}\\No Date", root);
            }
            else
            {
                return String.Format("{0}\\{1:d4}\\{1:d4}-{2:d2}\\{1:d4}-{2:d2}-{3:d2}",
                        root, dateTaken.Year, dateTaken.Month, dateTaken.Day);
            }
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
