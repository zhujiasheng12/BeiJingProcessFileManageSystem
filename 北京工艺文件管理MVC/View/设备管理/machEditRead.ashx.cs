using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// machEditRead 的摘要说明
    /// </summary>
    public class machEditRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (Model1  entities = new Model1 ())
            {
                var row = from cnc in entities.JDJS_PDMS_Device_Info
                          join type in entities.JDJS_PDMS_Device_Type_Info on cnc .TypeID equals type .ID 
                          join brand in entities.JDJS_PDMS_Device_Brand_Info on type .BrandID equals brand .ID 
                          join work in entities.JDJS_PDMS_Location_Info on cnc .LocationID equals work.ID 
                          select new
                          {
                              cnc.MachNum, brand.Brand, type.Type,cnc.IP,work.LocationName,work.ID
                          };
                var MachState ="";
               // var machStateId = entities.JDJS_WMS_Device_Status_Table.Where(r => r.Status == MachState).FirstOrDefault().ID;
                var model = new
                {
                    MachNum = row.FirstOrDefault().MachNum,
                    Brand = row.FirstOrDefault().Brand,
                    Type = row.FirstOrDefault().Type,
                    IP = row.FirstOrDefault().IP,
                    machStateId = MachState,
                    Name = row.FirstOrDefault().LocationName,
                    id = row.FirstOrDefault().ID

                };
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