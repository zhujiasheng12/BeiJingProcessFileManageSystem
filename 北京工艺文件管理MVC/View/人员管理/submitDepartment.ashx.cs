﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// submitDepartment 的摘要说明
    /// </summary>
    public class submitDepartment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var department = context.Request["department"];
            var remark = context.Request["remark"];
            using (Model1  entities1=new Model1 ())
            {
                var row = new JDJS_PDMS_Staff_Table_Row  { Staff= department, ParentID = id,
                Reamrk =remark};
                entities1.JDJS_PDMS_Staff_Table .Add(row);
                entities1.SaveChanges();
                context.Response.Write("ok");
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