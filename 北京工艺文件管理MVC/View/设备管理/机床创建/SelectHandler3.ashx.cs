using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// SelectHandler3 的摘要说明
    /// </summary>
    public class SelectHandler3 : IHttpHandler
    {
      
        public void ProcessRequest(HttpContext context)
        {
            string brand = context.Request["brand"];
            context.Response.ContentType = "text/plain";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (Model1  pm1 = new Model1 ())
            {


                var use2 = 
                           from brand1 in pm1.JDJS_PDMS_Device_Brand_Info
                           from type in pm1.JDJS_PDMS_Device_Type_Info
                           where  type.BrandID == brand1 .ID && brand1.Brand==brand
                           select new
                           {
                              type.Type
                           };
                          
                //var user2 = pm1.JDJS_WMS_Device_Type_Info.Where(r => r.BrandID ==1);
                
               

                    string json = serializer.Serialize(use2);
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