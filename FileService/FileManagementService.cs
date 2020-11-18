﻿
using IronMacbeth.FileManagement.Contract;
using IronMacbeth.UserManagement.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.FileManagement
{
    class FileManagementService : IFileManagement
    {
        public string AddFile(Stream fileStream)
        {
            var fileName = GetFileId();

            FileInfo fileInfo = new FileInfo($"Files\\{fileName}");
            using (var stream = fileInfo.Open(FileMode.CreateNew, FileAccess.Write))
            {
                fileStream.CopyTo(stream);
                stream.Close();
            }

            return fileName;
        }

        private static string GetFileId()
        {
            string result;

            Directory.CreateDirectory("Files");

            FileInfo fileInfo = new FileInfo("Files\\idStorage");
            if (fileInfo.Exists)
            {
                int id;
                using (TextReader reader = fileInfo.OpenText())
                {
                    id = int.Parse(reader.ReadLine());
                }
                id++;
                result = id.ToString();
                using (TextWriter writer = fileInfo.CreateText())
                {
                    writer.Write(result);
                }
            }
            else
            {
                result = "1";
                using (TextWriter writer = fileInfo.CreateText())
                {
                    writer.Write(result);
                }
            }
            return result;
        }

        public Stream GetFile(string fileName)
        {
            // do not dispose stream here. It's disposed by WCF whenever it's done sending it to the client
            var fileStream = File.OpenRead($"Files\\{fileName}");

            return fileStream;
        }


    }
}
