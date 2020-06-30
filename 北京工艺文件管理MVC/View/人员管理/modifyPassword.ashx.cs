using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modifyPassword 的摘要说明
    /// </summary>
    public class modifyPassword : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (Model1  entities1 = new Model1 ())
            {
                var row = entities1.JDJS_PDMS_Staff_Table .Where(r => r.ID == id).First();
                var model = new { staff = row.Staff, user = row.Users };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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