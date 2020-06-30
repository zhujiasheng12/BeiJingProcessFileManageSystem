﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using 北京工艺文件管理MVC.Database;

namespace WebApplication2.View.工艺文件管理
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {



                var form = context.Request.Form;
                var files = context.Request.Files;
                var groupNum = context.Request.Form["groupNum"];
                var groupName = context.Request.Form["groupName"];
                var TaskNum =groupNum+ form["TaskNum"];
                var TaskName = form["TaskName"];
                var date = Convert.ToDateTime(form["date"]);
                var userId = Convert.ToInt32(context.Session["id"]);
                string err = "";

                var staffId = form["staffId"] == "" ? "0" : form["staffId"];
                
                int staffID = Convert.ToInt32(staffId);


                if (AddTask(staffID, groupNum, groupName,TaskNum, TaskName, userId, date, ref err))
                {
                    PathInfo pathInfo = new PathInfo();
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), TaskNum, "图纸");
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    for (int i = 0; i < files.Count; i++)
                    {
                        var filePath = Path.Combine(dirPath, files[i].FileName);
                        files[i].SaveAs(filePath);
                    }


                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write(err);
                };
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }

        }
        /// <summary>
        /// 向数据库添加任务
        /// </summary>
        /// <param name="taskNum">任务号</param>
        /// <param name="taskName">任务名</param>
        /// <param name="creatPersonID">创建人员主键ID</param>
        /// <param name="demandTime">需求完成时间</param>
        /// <param name="errMsg">错误信息，如果返回false需要看这个</param>
        /// <returns>是否成功</returns>
        public static bool AddTask(int staffID,string ComponentNum, string ComponentName, string taskNum, string taskName, int creatPersonID, DateTime demandTime, ref string errMsg)
        {
            //try
            //{
            //    using (Model1 mod = new Model1())
            //    {
            //        using (System.Data.Entity.DbContextTransaction mytran = mod.Database.BeginTransaction())
            //        {
            //            try
            //            {
                           
            //                var staff = mod.JDJS_PDMS_Staff_Table.Where(r => r.ID == creatPersonID).FirstOrDefault();
            //                if (staff == null)
            //                {
            //                    mytran.Rollback();
            //                    errMsg = "该员工不存在，请确认后再试！";
            //                    return false;
            //                }
            //                var taskOldCom = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentNum == ComponentNum).FirstOrDefault();
            //                if (taskOldCom != null)
            //                {
            //                    if (taskOldCom.ComponentName != ComponentName)
            //                    {
            //                        mytran.Rollback();
            //                        errMsg = "该产品单号已存在，组件名称为" + taskOldCom.ComponentName;
            //                        return false;
            //                    }
            //                }
            //                var taskOld = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.TaskNum == taskNum);
            //                if (taskOld.Count() > 0)
            //                {
            //                    mytran.Rollback();
            //                    errMsg = "该零件编号已存在，请修改后再试！";
            //                    return false;
            //                }
            //                if (staffID == 0)
            //                {
            //                    JDJS_PDMS_TaskInfo_Table_Row task = new JDJS_PDMS_TaskInfo_Table_Row()
            //                    {
            //                        ComponentNum = ComponentNum,
            //                        ComponentName = ComponentName,

            //                        staffID = creatPersonID,
            //                        State = 0,
            //                        TaskName = taskName,
            //                        TaskNum = taskNum,
            //                        CreatTime = DateTime.Now,
            //                        DemandTime = demandTime
            //                    };
            //                    mod.JDJS_PDMS_TaskInfo_Table.Add(task);
            //                }
            //                else
            //                {
            //                    var staff1 = mod.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffID).FirstOrDefault();
            //                    if (staff1 == null)
            //                    {
            //                        mytran.Rollback();
            //                        errMsg = "该员工不存在，请确认后再试！";
            //                        return false;
            //                    }
            //                    JDJS_PDMS_TaskInfo_Table_Row task = new JDJS_PDMS_TaskInfo_Table_Row()
            //                    {
            //                        ComponentNum = ComponentNum,
            //                        ComponentName = ComponentName,
            //                        CraftPersonID =staffID ,
            //                        ArrangeTaskTime =DateTime .Now ,

            //                        staffID = creatPersonID,
            //                        State = 1,
            //                        TaskName = taskName,
            //                        TaskNum = taskNum,
            //                        CreatTime = DateTime.Now,
            //                        DemandTime = demandTime
            //                    };
            //                    mod.JDJS_PDMS_TaskInfo_Table.Add(task);
            //                }
            //                mod.SaveChanges();
            //                mytran.Commit();
            //                return true;
            //            }
            //            catch (Exception ex)
            //            {
            //                errMsg = ex.Message;
            //                mytran.Rollback();
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