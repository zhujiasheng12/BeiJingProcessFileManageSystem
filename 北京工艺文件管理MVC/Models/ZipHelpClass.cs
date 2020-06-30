﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace 北京工艺文件管理MVC.Models
{
    public class ZipHelpClass
    {
        public void ZipFile(string strFile, string strZip)
        {
            var len = strFile.Length;
            var strlen = strFile[len - 1];
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile += Path.DirectorySeparatorChar;
            }
            ZipOutputStream outstream = new ZipOutputStream(File.Create(strZip));
            outstream.SetLevel(6);
            zip(strFile, outstream, strFile);
            outstream.Finish();
            outstream.Close();
            Directory.Delete(strFile,true );
        }

        public void zip(string strFile, ZipOutputStream outstream, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            //获取指定目录下所有文件和子目录文件名称
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            //遍历文件
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    zip(file, outstream, staticFile);
                }
                //否则，直接压缩文件
                else
                {
                    //打开文件
                    FileStream fs = File.OpenRead(file);
                    //定义缓存区对象
                    byte[] buffer = new byte[fs.Length];
                    //通过字符流，读取文件
                    fs.Read(buffer, 0, buffer.Length);
                    //得到目录下的文件（比如:D:\Debug1\test）,test
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Flush();
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    outstream.PutNextEntry(entry);
                    //写文件
                    outstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// 将工装图纸拿出来
        /// </summary>
        /// <param name="path"></param>
        /// <param name="descPath"></param>
        /// <returns></returns>
        public  bool CreateDirectory(string path, string descPath)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            var dirInfo = dir.GetDirectories();
            foreach (var item in dirInfo)
            {
                if (item.Name == "工装图纸")
                {
                    var files = item.GetFiles();
                    if (!Directory.Exists(Path.Combine(descPath, item.Name)))
                    {
                        Directory.CreateDirectory(Path.Combine(descPath, item.Name));
                    }
                    foreach (var real in files)
                    {
                        File.Copy(real.FullName, Path.Combine(descPath, item.Name, real.Name));
                    }

                }
                else
                {
                    if (item.GetDirectories().Count() > 0)
                    {
                        CreateDirectory(item.FullName, Path.Combine(descPath, item.Name));
                    }
                }

            }

            return true;
        }
    }
}