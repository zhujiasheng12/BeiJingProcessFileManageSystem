using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// machOption 的摘要说明
    /// </summary>
    public class machOption : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using (Model1  entities=new Model1())
            {
                var rows = entities.JDJS_PDMS_Device_Info;
                List<Option> options = new List<Option>();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                foreach (var item in rows)
                {
                    Option option = new Option();
                    option.number = item.MachNum;
                    options.Add(option);
                };
                var rowsWork = entities.JDJS_PDMS_Location_Info;
                foreach (var item in rowsWork)
                {
                    Option option = new Option();
                    option.number = item.LocationName;
                    options.Add(option);
                }
                var data = options.Where((r, i) => options.FindIndex(p=>p.number==r.number) == i);
                var sort = data.OrderBy(r => r.number);
                var json = serializer.Serialize(sort);
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
    class Option
    {
        public string number;
    }
}