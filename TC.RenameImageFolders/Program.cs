using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC.RenameImageFolders
{
    class Program
    {
        private static string root = @"D:\My Archive\My Pictures";

        static void Main(string[] args)
        {
            RenameFolders(root);

            MoveFolders(root);
        }

        private static void MoveFolders(string path)
        {
            foreach (string yearPath in Directory.GetDirectories(path))
            {
                foreach (string monthPath in Directory.GetDirectories(yearPath))
                {
                    string name = Path.GetFileName(monthPath);

                    if (!ValidMonthName(name))
                    {
                        if (ValidDayName(name))
                        {
                            MoveFolder(monthPath);
                        }
                    }
                }
            }
        }

        private static void MoveFolder(string oldPath)
        {
            string name = Path.GetFileName(oldPath);

            string newMonth = Path.Combine(Path.GetDirectoryName(oldPath), name.Substring(0, 7));

            string newPath = Path.Combine(newMonth, name);

            if (!Directory.Exists(newMonth)) Directory.CreateDirectory(newMonth);

            if (Directory.Exists(newPath))
            {
                Console.WriteLine(newPath);
            }
            else
            {
                Directory.Move(oldPath, newPath);
            }
        }

        private static void RenameFolders(string path)
        {
            foreach(string dirPath in Directory.GetDirectories(path))
            {
                string name = Path.GetFileName(dirPath);

                if (ValidYearName(name))
                {
                    // Valid Year
                    RenameFolders(dirPath);
                }
                else if (name.Length == 6 && name.All(Char.IsDigit))
                {
                    RenameFolder(dirPath, name.Insert(4, "-"));
                }
                else if (ValidMonthName(name))
                {
                    // Valid Month
                    RenameFolders(dirPath);
                }
                else if (ValidDayName(name))
                {
                    // Valid Day
                }
                else if (name.Substring(0, 8).All(Char.IsDigit))
                {
                    RenameFolder(dirPath, name.Insert(6, "-").Insert(4, "-"));
                }
                else
                {
                    Console.WriteLine(name);
                }
             
            }
        }

        private static bool ValidYearName(string name)
        {
            return name.Length == 4 && name.All(Char.IsDigit);
        }

        private static bool ValidMonthName(string name)
        {
            return name.Length == 7 && name.Substring(4, 1) == "-";
        }

        private static bool ValidDayName(string name)
        {
            return name.Substring(4, 1) == "-" && name.Substring(7, 1) == "-";
        }

        private static void RenameFolder(string oldPath, string newName)
        {
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);

            if (!Directory.Exists(newPath))
            {
                Directory.Move(oldPath, newPath);
            }
            else
            {
                Console.WriteLine(newPath);
            }
        }
    }
}
