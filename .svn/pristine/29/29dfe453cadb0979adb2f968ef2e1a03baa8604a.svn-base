
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// accept 的摘要说明
    /// </summary>
    public class accept : IHttpHandler, IRequiresSessionState
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
                       DateTime? time = null;
                        if (AcceptTaskMethod(id, staffId, time, ref  err))
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
        /// 工艺责任人接收任务
        /// </summary>
        /// <param name="taskID">任务主键ID</param>
        /// <param name="staffID">接收员工ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AcceptTaskMethod(int taskID, int staffID, DateTime? PlanCompletTime, ref string errMsg)
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
            //                var task = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).FirstOrDefault();
            //                if (task == null)
            //                {
            //                    mytran.Rollback();
            //                    errMsg = "该任务不存在！";
            //                    return false;

            //                }
            //                if (task.State != 1)
            //                {
            //                    mytran.Rollback();
            //                    errMsg = "任务："+task.TaskNum+"不满足接收条件！";
            //                    return false;
            //                }
            //                if (task.CraftPersonID != staffID)
            //                {
            //                    mytran.Rollback();
            //                    errMsg = "该任务的工艺责任人与任务接收人员不匹配！";
            //                    return false;
            //                }
            //                task.AcceptTaskTime = DateTime.Now;
            //                task.State = 2;
            //                if (PlanCompletTime == null)
            //                {
            //                    task.PlanCompletionTime = task.DemandTime;
            //                }
            //                else
            //                {
            //                    task.PlanCompletionTime = PlanCompletTime;
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