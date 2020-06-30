using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modifyPasswordSubmit 的摘要说明
    /// </summary>
    public class modifyPasswordSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var oldPassword = context.Request["oldPassword"];
            var newPassword = context.Request["newPassword"];
            var summitPassword = context.Request["submitPassword"];
            using(WebApplication2 . Model1 entities1 =new WebApplication2 . Model1 ())
            {
                var row = entities1.JDJS_PDMS_Staff_Table .Where(r => r.ID == id).First();
                if (row.PassWord == oldPassword)
                {
                    if(newPassword== summitPassword)
                    {
                        row.PassWord = newPassword;
                        entities1.SaveChanges();
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write("两次新密码不一致");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("原始密码错误");
                    return;
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