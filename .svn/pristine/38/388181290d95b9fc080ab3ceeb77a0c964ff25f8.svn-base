using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.工厂管理
{
    /// <summary>
    /// DetailsHandler1 的摘要说明
    /// </summary>
    public class DetailsHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (Model1  data = new Model1 ())
            {
                var data1 = data.JDJS_PDMS_Location_Info.Where(r => r.ID == id).First();
                if (data1.ParentID == 0)
                {
                    context.Response.Write(data1.LocationName);
                }
                else
                {
                    var data2 = data.JDJS_PDMS_Location_Info.Where(r => r.ID == data1.ParentID).First();
                    var data3 = data2.LocationName;
                    context.Response.Write(data3);
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