using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.View.工艺文件管理
{
    /// <summary>
    /// filesRead 的摘要说明
    /// </summary>
    public class filesRead : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            var taskNum = context.Request.QueryString["taskNum"];
            var dir=context.Request.QueryString["dir"];
            PathInfo pathInfo=new PathInfo();

            var dirPath = Path.Combine(pathInfo.upLoadPath(), taskNum, dir);
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            var files = dirInfo.GetFiles();
            List<FileRead> fileRead = new List<FileRead>();
            for (int i = 0; i < files.Count(); i++)
            {
                fileRead.Add(new FileRead { fileName = files[i].Name,uploadTime=files[i].LastWriteTime.ToString () });

            }
            var model = new { code = 0, data = fileRead };
            var json = serializer.Serialize(model);
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class FileRead {
        public string fileName;
        public string uploadTime;
    }
}