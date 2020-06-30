using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication2
{
    public class PathInfo
    {
        string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "类", "path.xml");     
      public  string upLoadPath()//上传路径
        {            
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/upLoadPath");
            return root.InnerText;
        }
 
     public string downLoadPath()//下载路径
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/downLoadPath");
            return root.InnerText;
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
            var path =Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "工厂管理", "设计布局图");
            return path+"\\";
        }
        public string Webhook()//examineWebsite
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/Webhook");
            return @root.InnerText;
        }
        public string examineWebsite()//
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/examineWebsite");
            return @root.InnerText;
        }
    }
}