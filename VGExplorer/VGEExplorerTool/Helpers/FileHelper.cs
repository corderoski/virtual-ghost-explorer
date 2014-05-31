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
