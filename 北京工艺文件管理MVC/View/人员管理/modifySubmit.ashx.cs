using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modifySubmit 的摘要说明
    /// </summary>
    public class modifySubmit : IHttpHandler
    {

        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var limit = context.Request.Form[0];
            var form = context.Request.Form[1];
            var id = int.Parse(context.Request.Form[2]);
            var obj = Serializer.Deserialize<Form>(form);
            using (Model1  entities1 = new Model1 ())
            {
                if (obj.user == "")
                {
                    var row = entities1.JDJS_PDMS_Staff_Table .Where(r => r.ID == id).First();
                    row.Limit = null;
                    row.PassWord =null;
                    row.Users = null;
                    row.Tel = obj.tel;
                    row.Posiation  = obj.position;
                    row.Staff = obj.staffName;

                }
                else
                {
                    
                    var rows = entities1.JDJS_PDMS_Staff_Table ;
                    List<repeat> repeats = new List<repeat>();
                    foreach (var item in rows)
                    {
                        repeats.Add(new repeat { user = item.Users ,id=item.ID});
                    }
                    repeats.RemoveAll(r => r.id ==id);
                    var rows1 = repeats.Where(r => r.user == obj.user);
                    if (rows1.Count() > 0)
                    {
                        context.Response.Write("登录账户已存在");
                        return;
                    }
                    else
                    {
                        if (obj.password == "")
                        {
                            context.Response.Write("密码不能为空");
                            return;
                        }
                        else
                        {


                            var row = entities1.JDJS_PDMS_Staff_Table .Where(r => r.ID == id).First();


                            row.Staff = obj.staffName;
                            row.Posiation  = obj.position;
                            row.Tel = obj.tel;

                            row.Users = obj.user;
                            row.PassWord = obj.password;
                            row.Limit = limit;
                            row.Reamrk  = obj.remark;
                            row.MailBox = obj.mailbox;

                        }
                        
                    }
                }
                entities1.SaveChanges();
            }


            context.Response.Write("ok");
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class repeat
    {
        public string user;
        public int id;
    }
}