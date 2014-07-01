using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VGExplorerTool.Helpers
{
    public class FileHelper
    {

        public const String VGEFileFilter = "VGE File (*.vge)|*.vge";

        private const String VGE_CONF_FILE = "vge.ini";


        
        internal static Entities.Configuration GetAppConfiguration()
        {
            var file = Environment.CurrentDirectory + Path.DirectorySeparatorChar + VGE_CONF_FILE;
            Entities.Configuration obj = null;

            if (!File.Exists(file))
            {
                obj = new Entities.Configuration();
                File.WriteAllText(file, JsonHelper.Serialize(obj));
                return obj;
            }

            var content = File.ReadAllText(file);
            obj = JsonHelper.Deserialize<Entities.Configuration>(content);

            return obj;
        }


        public static String GetFileSizeString(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
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
