using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.测试
{
    /// <summary>
    /// 读svg 的摘要说明
    /// </summary>
    public class 读svg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PathInfo path = new PathInfo();
            var cncLayoutPath = path.cncLayoutPath();
            var workId = int.Parse(context.Request["workId"]);
           if( File.Exists(cncLayoutPath + workId + ".svg"))
            {
                string dd = "";
                StreamReader sr = new StreamReader(cncLayoutPath + workId+".svg", System.Text.Encoding.UTF8);

                while (!sr.EndOfStream)
                {

                    dd += sr.ReadLine();
                }
                sr.Close();
                context.Response.Write(dd);
                return;
            }
            else
            {
                using(Model1  entities=new Model1 ())
                {
                    var obj = from cnc in entities.JDJS_PDMS_Device_Info .Where(r => r.LocationID == workId)
                             select new
                             {
                                 cnc.ID,
                                 cnc.MachNum
                             };
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(obj);
                    context.Response.Write(json);

                }
            }

           
           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}