using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.机床监控
{
    /// <summary>
    /// SelectHandler1 的摘要说明
    /// </summary>
    public class SelectHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            using (Model1  data=new Model1())
            {
                var data1 = from brand in data.JDJS_PDMS_Device_Brand_Info
                            select new
                            {
                                brand.Brand,brand.ID
                            };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data2 = serializer.Serialize(data1);
                context.Response.Write(data2);
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