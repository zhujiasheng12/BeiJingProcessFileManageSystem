﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// complete 的摘要说明
    /// </summary>
    public class complete : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {


                var ids = context.Request.Form["ids"].Split(',');


                for (int i = 0; i < ids.Count(); i++)
                {
                    if (ids[i] != "")
                    {
                        var id = Convert.ToInt32(ids[i]);
                        var staffId = Convert.ToInt32(context.Session["id"]);
                        string err = "";
                        if (CompleteTaskMethod(id, staffId,  ref  err))
                        {

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
        /// w完成任务
        /// </summary>
        /// <param name="taskID">任务主键ID</param>
        /// <param name="staffID">员工主键ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool CompleteTaskMethod(int taskID, int staffID, ref string errMsg)
        {
            return true;
            //try
            //{
            //    //上传文件的处理方法
            //    using (WebApplication2. Model1 mod = new WebApplication2. Model1())
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
            //                var task = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).FirstOrDefault();
            //                if (task == null)
            //                {
            //                    errMsg = "该任务不存在！";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                if (task.CraftPersonID != staffID)
            //                {
            //                    errMsg = "该任务的工艺责任人不匹配！";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                if (task.ProcessCardUploadTime == null || task.ProcessFileUploadTime == null || task.ProcessScheduleUpTime == null || task.ProgramFileUploadTime == null||task.OtherFileTime ==null)
            //                {
            //                    errMsg = "该任务文件没有全部上传，请上传相关文件后点击完成！";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                if (task.State == 4)
            //                {
            //                    errMsg = "该任务已完成，请勿重复提交！";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                task.State = 4;
            //                task.CompleteTime = DateTime.Now;
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