﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// distribution 的摘要说明
    /// </summary>
    public class distribution : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var ids = context.Request.Form["ids"].Split(',');
            List<int> idsInt=new List<int>();
            for (int i = 0; i < ids.Count(); i++)
			{
                if(ids[i]!=""){
                var id=Convert.ToInt32(ids[i]);
                     idsInt.Add(id);
                }
			
			}
            var staffId =Convert.ToInt32( context.Request.Form["staffId"]);
            string err = "";
          if(  ArrangeTask(idsInt,staffId , ref  err)){
              context.Response.Write("ok");
    }else{
        context.Response.Write(err);
    }
        }
        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="taskIDList">任务主键ID列表</param>
        /// <param name="staffID">安排人员主键ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool ArrangeTask(List<int> taskIDList, int staffID, ref string errMsg)
        {
            return true;
            //try
            //{
            //    using (WebApplication2.Model1 mod = new WebApplication2. Model1())
            //    {
            //        using (EF::System.Data.Entity.DbContextTransaction mytran = mod.Database.BeginTransaction())
            //        {
            //            try
            //            {
            //                var staff = mod.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffID).FirstOrDefault();
            //                if (staff == null)
            //                {
            //                    errMsg = "该员工不存在！";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                foreach (int item in taskIDList)
            //                {
                                
            //                    var task = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == item).FirstOrDefault();
            //                    if (task.State == 1)
            //                    {
            //                        continue;
            //                    }
            //                    if (task != null)
            //                    {
            //                        task.CraftPersonID = staffID;
            //                        task.ArrangeTaskTime = DateTime.Now;
            //                        task.State = 1;
            //                    }
            //                }
            //                mod.SaveChanges();
            //                mytran.Commit();
            //                return true;
            //            }
            //            catch (Exception ex)
            //            {
            //                mytran.Rollback();
            //                errMsg = ex.Message;
            //                return false;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errMsg = ex.Message;
            //    return false;
            //}
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