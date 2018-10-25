using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertCodeFromUTF8
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.Error.WriteLine("Args' count is empty.");
                    Environment.Exit(1);
                }

                if (Directory.Exists(args[0]) == false)
                {
                    Console.Error.WriteLine("Target does not exist.");
                    Environment.Exit(1);
                }

                List<string> searchPatterns = new List<string>(args);
                searchPatterns.RemoveAt(0);

                Console.WriteLine(string.Format("Convert all files in {0}  from UTF8.", args[0]));
                DateTime start = DateTime.Now;
                ConvertDirctory(new DirectoryInfo(args[0]), searchPatterns);
                DateTime end = DateTime.Now;
                Console.WriteLine(string.Format("Convert finished, cost {0}.", end - start));
#if DEBUG
                Console.Read();
#endif
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        private static void ConvertDirctory(DirectoryInfo di, IEnumerable<string> searchPatterns)
        {
            foreach (var pattern in searchPatterns)
            {
                var files = di.GetFiles(pattern);
                foreach (var i in files)
                {
                    ConvertFile(i);
                }
            }

            var dirs = di.GetDirectories();
            foreach (var i in dirs)
            {
                ConvertDirctory(i, searchPatterns);
            }
        }
        
        private static void ConvertFile(FileInfo fi)
        {
            string src;
            using (StreamReader stream = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                src = stream.ReadToEnd();
            }

            StringBuilder builder = new StringBuilder();
            bool hasChanged = false;
            foreach (var c in src)
            {
                if ((int)c <= 128)
                {
                    builder.Append(c);
                }
                else
                {
                    hasChanged = true;
                    builder.Append(ConvertChar(c));
                }
            }

            if (hasChanged == false)
            {
                return;
            }

            using (StreamWriter stream = new StreamWriter(fi.FullName, false, Encoding.UTF8))
            {
                stream.Write(builder.ToString());
                stream.Close();
            }
        }
        
        private static string ConvertChar(char c)
        {
            var bytes = Encoding.UTF8.GetBytes(new char[] { c });
            StringBuilder res = new StringBuilder();
            foreach (var i in bytes)
            {
                res.Append("\\" + Convert.ToString(i, 8));
            }

            return res.ToString();
        }
    }
}
