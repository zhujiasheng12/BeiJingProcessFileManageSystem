using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// SelectHandler1 的摘要说明
    /// </summary>
    public class SelectHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using(Model1  pm=new Model1 ())
            {
               


                //var brands = from b in pm.JDJS_WMS_Device_Brand_Info
                //             select new
                //             {
                //                 b.ID,
                //                 b.Brand
                //             };

                var brands = pm.JDJS_PDMS_Device_Brand_Info .Select(r => new { r.ID, r.Brand });
                string json = serializer.Serialize(brands);
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