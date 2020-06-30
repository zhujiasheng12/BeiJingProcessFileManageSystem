using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// JcglProductHandler1 的摘要说明
    /// </summary>
    public class JcglProductHandler1 : IHttpHandler
    {
        public static List<JcglProduct> jcglproducts = new List<JcglProduct>();
        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string page = context.Request["page"];
            int page1 = int.Parse(page);
            int limit = int.Parse(context.Request["limit"]);
          
            context.Response.ContentType = "text/plain";


            using (Model1  pm2 = new Model1 ())
            {
                var user = from type in pm2.JDJS_PDMS_Device_Type_Info
                           join brand in pm2.JDJS_PDMS_Device_Brand_Info on type.BrandID equals brand.ID
                           join mach in pm2.JDJS_PDMS_Device_Info on type.ID equals mach.TypeID
                           join work in pm2.JDJS_PDMS_Location_Info on mach.LocationID equals work.ID
                           select new
                           {
                               brand.Brand,
                               type.Type,
                               mach.MachNum,
                               mach.IP,
                               mach.ID,
                               work.LocationName
                           };
                var key = context.Request["key"];
                if (key != null)
                {
                    user = user.Where(r => r.MachNum == key | r.LocationName == key);
                }

                var uses = user.OrderBy(r => r.ID).Skip((page1 - 1) * limit).Take(limit);
               
                var model = new { code = 0, msg = "", count = user.Count(), data = uses };
                string json = serializer.Serialize(model);

                context.Response.Write(json);
            };
           
         
           
           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class JcglProduct
    {
        public string id
        {
            get;
            set;
        }
        public string jcpp
        {
            get;
            set;
        }
        public string jcxh
        {
            get;
            set;
        }
        public string jcbh
        {
            get;
            set;
        }
        public string jcip
        {
            get;
            set;
        }
        public string zt
        {
            get;
            set;
        }
        public string cz
        {
            get;
            set;
        }
    }
}