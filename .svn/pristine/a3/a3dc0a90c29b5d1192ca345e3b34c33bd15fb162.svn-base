using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace WebApplication2.人员管理
{
    /// <summary>
    /// 获取所在部门 的摘要说明
    /// </summary>
    public class 获取所在部门 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var userId = Convert.ToInt32(context.Session["id"]);
            using(Model1  entities=new Model1 ())
            {
                var departmentId = entities.JDJS_PDMS_Staff_Table .Where(r => r.ID == userId).FirstOrDefault().ParentID;
                var department = entities.JDJS_PDMS_Staff_Table .Where(r => r.ID == departmentId).FirstOrDefault().Staff;
                context.Response.Write(department);
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