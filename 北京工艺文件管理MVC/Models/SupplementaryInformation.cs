﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace 北京工艺文件管理MVC.Models
{
    public class SupplementaryInformation
    {
        string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "path.xml");
        public  string upLoadPath()//上传路径
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(xmlPath);
            //XmlNode root = doc.SelectSingleNode("/root/upLoadPath");
            //return root.InnerText;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JDJS_PDMS_Files"); ;
        }

        public string GetSendAccount()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/SendAccount");
            return root.InnerText;
        }
        public string GetSenderName()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/SenderName");
            return root.InnerText;
        }
        public string GetSenderPassword()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/SenderPassword");
            return root.InnerText;
        }
        public string GetServerIP()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ServerIP");
            return root.InnerText;
        }
        public string GetServerPort()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ServerPort");
            return root.InnerText;
        }


        public string GetIFDatabaseStr()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/IFDatabaseConnectString");
            return root.InnerText;
        }

        public string downLoadPath()//下载路径
        {
            return "JDJS_PDMS_Files";
            //XmlDocument doc = new XmlDocument();
            //doc.Load(xmlPath);
            //XmlNode root = doc.SelectSingleNode("/root/downLoadPath");
            //return root.InnerText;
        }

        public string DaMaMachName()//打印机位置
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/DaMaMachName");
            return @root.InnerText;
        }
        public string cncLayoutPath()//机床布局图位置
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "工厂管理", "设计布局图");
            return path + "\\";
        }
        public string ftpUrl()//ftpUrl
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ftpUrl");
            return @root.InnerText;
        }
        public string ftpUser()//ftpUrl
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ftpUser");
            return @root.InnerText;
        }
        public string ftpPassword()//ftpUrl
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ftpPassword");
            return @root.InnerText;
        }
    }
}