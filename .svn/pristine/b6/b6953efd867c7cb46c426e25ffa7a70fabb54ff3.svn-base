using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// departmentModify 的摘要说明
    /// </summary>
    public class departmentModify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var department = context.Request["department"];
            var remark = context.Request["remark"];
            using (Model1  entities1=new Model1 ())
            {
                var row = entities1.JDJS_PDMS_Staff_Table .Where(r => r.ID == id).First();
                row.Staff= department;
                row.Reamrk  = remark;
                entities1.SaveChanges();
                context.Response.Write("ok");
                
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