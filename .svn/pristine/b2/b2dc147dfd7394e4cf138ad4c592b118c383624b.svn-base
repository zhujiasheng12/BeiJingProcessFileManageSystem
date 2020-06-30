using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// submit 的摘要说明
    /// </summary>
    public class submit : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var limit = context.Request.Form[0];
            var form = context.Request.Form[1];
            var id = int.Parse(context.Request.Form[2]);
            var obj = Serializer.Deserialize<Form>(form);
            using(Model1  entities1=new Model1 ())
            {
                if (obj.user == "")
                {
                    var row = new JDJS_PDMS_Staff_Table_Row 
                    {
                        Staff = obj.staffName,
                        Posiation  = obj.position,
                        Tel = obj.tel,
                        ParentID=id,
                        Reamrk =obj.remark
                    };
                    entities1.JDJS_PDMS_Staff_Table .Add(row);
                }
                else
                {
                    var rows = entities1.JDJS_PDMS_Staff_Table .Where(r => r.Users == obj.user);
                    if (rows.Count() > 0)
                    {
                        context.Response.Write("登录账户已存在");
                        return;
                    }
                    else
                    {


                        var row = new JDJS_PDMS_Staff_Table_Row 
                        {
                            Staff = obj.staffName,
                            Posiation  = obj.position,
                            Tel = obj.tel,
                            ParentID = id,
                            Users = obj.user,
                            PassWord = "123456",
                            Limit = limit,
                            Reamrk  = obj.remark

                        };
                        entities1.JDJS_PDMS_Staff_Table .Add(row);
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
    class Form
    {
        
        public string staffName;
        public string position;
        public string tel;
        public string user;
        public string password;
        public string remark;
        public string mailbox;
    }
}