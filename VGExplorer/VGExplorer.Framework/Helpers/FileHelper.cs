using System;
using System.IO;
using VGExplorer.Framework.Entities;

namespace VGExplorer.Framework.Helpers
{
    public class FileHelper
    {

        public const String VGE_FILE_FILTER = "VGE File (*.vge)|*.vge";

        private const String VGE_CONF_FILE = "vge.ini";

        public static Configuration GetAppConfiguration(string path)
        {
            var file = path + Path.DirectorySeparatorChar + VGE_CONF_FILE;
            Configuration obj = null;

            if (!File.Exists(file))
            {
                obj = new Configuration();
                File.WriteAllText(file, JsonHelper.Serialize(obj));
                return obj;
            }

            var content = File.ReadAllText(file);
            obj = JsonHelper.Deserialize<Configuration>(content);
            return obj;
        }


        public static String GetFileSizeString(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            var order = 0;
            while (size >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                size = size / 1024;
            }

            return String.Format("{0} {1}", size, sizes[order]);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String OpenStream(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void SaveStream(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
            }
            catch
            {
                throw;
            }
        }



    }
}
