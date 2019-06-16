using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locket
{
    sealed class SystemData
    {
        #region Property
        public static Dictionary<string, string> FILES = new Dictionary<string, string>();
        public static int COUNT;

        public static string EXTENSION = "";
        public static string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Locket");

        private static string FILENAME_PATH
        {
            get
            {
                string path = PATH + "\\confiure.locket";
                if (!System.IO.File.Exists(path))
                {
                    FileStream stream = System.IO.File.Create(path);
                    stream.Close();
                    System.IO.File.WriteAllText(path, "##########\r\nfile\r\n##########\r\ncount=0");
                }
                return path;
            }
        }


        public static string TEMP
        {
            get
            {
                string path = PATH + "\\temp";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string FILE_PATH
        {
            get
            {
                string path = PATH + "\\file";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string VIDEO_PATH
        {
            get
            {
                string path = PATH + "\\video";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string IMAGE_PATH
        {
            get
            {
                string path = PATH + "\\image";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }
        #endregion

        #region Method
        private SystemData()
        {

        }

        public static void SaveFile()
        {
            string content = "##########\r\n";
            content += "file\r\n";
            foreach (var key in FILES.Keys)
            {
                content += key + "=" + FILES[key] + "\r\n";
            }
            content += "##########\r\n";
            content += "count=" + COUNT;

            File.WriteAllText(FILENAME_PATH, content);
        }

        public static void RetrieveFile()
        {
            bool beginfile = false;
            foreach (string item in File.ReadAllLines(FILENAME_PATH))
            {
                if (!item.StartsWith("#"))
                {
                    if (beginfile)
                    {
                        int index = item.IndexOf('=');

                        string key = item.Substring(0, index);
                        string data = item.Substring(index + 1);

                        FILES.Add(key, data);
                    }
                    if (item.StartsWith("file")) beginfile = true;
                    if (item.StartsWith("count"))
                    {
                        int index = item.IndexOf('=');
                        COUNT = Convert.ToInt32(item.Substring(index + 1));
                    }
                }
                else
                {
                    beginfile = false;
                }

            }
        }
        #endregion

    }
}
