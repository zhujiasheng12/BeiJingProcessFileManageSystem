﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// adminDel 的摘要说明
    /// </summary>
    public class adminDel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try {
            
           
            var ids = context.Request.Form["ids"].Split(',');
            var taskNums = context.Request.Form["taskNums"].Split(',');
           
            for (int i = 0; i < ids.Count(); i++)
			{
                if (ids[i] != "") {
                    var id = Convert.ToInt32(ids[i]);
                    string err = "";
                    if (DeleteTask(id, ref err))
                    {
                        WebApplication2.PathInfo pathInfo = new WebApplication2.PathInfo();
                        var dirPath = Path.Combine(pathInfo.upLoadPath(), taskNums[i]);
                        if (Directory.Exists(dirPath)) {
                            Directory.Delete(dirPath,true);
                        }
                    }
                    else
                    {
                        context.Response.Write(err);
                        return;
                    }
                }
                }
		

            context.Response.Write("ok");
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskID">任务主键ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteTask(int taskID, ref string errMsg)
        {
            return true;
            //try
            //{
            //    using (WebApplication2.Model1 mod = new WebApplication2.Model1())
            //    {
            //        using (EF::System.Data.Entity.DbContextTransaction mytran = mod.Database.BeginTransaction())
            //        {
            //            try
            //            {
            //                var tasks = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID);
            //                foreach (WebApplication2.JDJS_PDMS_TaskInfo_Table_Row task in tasks)
            //                {
            //                    mod.JDJS_PDMS_TaskInfo_Table.Remove(task);
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