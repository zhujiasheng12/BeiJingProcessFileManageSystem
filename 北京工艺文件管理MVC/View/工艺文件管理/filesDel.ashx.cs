﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// filesDel 的摘要说明
    /// </summary>
    public class filesDel : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;
            var fileNames = form["fileNames"].Split(',');
            var taskId = Convert.ToInt32(form["taskId"]);

            var taskNum = form["taskNum"];
            var dir = form["dir"];
            int fileType = -1;
            switch (dir)
            {
                case "工装图纸":
                    fileType = 0;
                    break;

                case "工艺过程卡":
                    fileType = 1;
                    break;

                case "加工程序":
                    fileType = 2;
                    break;

                case "刀具表":
                    fileType = 3;
                    break;

                case "工艺进程单":
                    fileType = 4;
                    break;
            }
            var staffId = Convert.ToInt32(context.Session["id"]);
            string err = "";
            WebApplication2.PathInfo pathInfo = new WebApplication2.PathInfo();
            var dirPath = Path.Combine(pathInfo.upLoadPath(), taskNum, dir);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            for (int i = 0; i < fileNames.Count(); i++)
            {

                var filePath = Path.Combine(pathInfo.upLoadPath(), taskNum, dir, fileNames[i]);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            DirectoryInfo dirs = new DirectoryInfo(dirPath);
            var files = dirs.GetFiles();
            if (files.Count() == 0)
            {
                //删除日期
                if (DeleteProcessFiles(taskId, staffId, fileType, ref  err)) { }
                else
                {
                    context.Response.Write(err);
                    return;
                }
            }
            context.Response.Write("ok");
        }
        /// <summary>
        /// 删除工艺文件
        /// </summary>
        /// <param name="taskID">任务主键ID</param>
        /// <param name="staffID">员工ID</param>
        /// <param name="fileType">文件类型 0：工艺过程卡。
        ///                                 1：工艺进程单。
        ///                                 2：编程文件。
        ///                                 3：加工文件。
        ///                                 4：其它文件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteProcessFiles(int taskID, int staffID, int fileType, ref string errMsg)
        {
            return true;
            //try
            //{
            //    if (fileType == -1)
            //    {
            //        return true;
            //    }
            //    using (WebApplication2.Model1 mod = new WebApplication2.Model1())
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
            //                string fileTypeStr = "";
                            
            //                switch (fileType)
            //                {
            //                    case 0:
            //                        task.ProcessCardUploadTime = null ;
            //                        task.ProcessCardUploadPath = "";
            //                        task.State = 3;
            //                        fileTypeStr = "删除工艺过程卡";
            //                        break;
            //                    case 1:
            //                        task.ProcessScheduleUpTime = null;
            //                        task.ProcessScheduleUpPath = "";
            //                        task.State = 3;
            //                        fileTypeStr = "删除工艺进程单";
            //                        break;
            //                    case 2:
            //                        task.ProgramFileUploadTime = null;
            //                        task.ProgramFileUploadPath = "";
            //                        task.State = 3;
            //                        fileTypeStr = "删除编程文件";
            //                        break;
            //                    case 3:
            //                        task.ProcessFileUploadTime = null;
            //                        task.ProcessFileUploadPath = "";
            //                        task.State = 3;
            //                        fileTypeStr = "删除加工文件";
            //                        break;
            //                    case 4:
            //                        task.OtherFileTime = null;
            //                        task.OtherFilePath = "";
            //                        task.State = 3;
            //                        fileTypeStr = "删除其它文件";
            //                        break;
            //                    default:
            //                        break;
            //                }

            //                if (task.ProcessCardUploadTime == null && task.ProcessFileUploadTime == null && task.ProcessScheduleUpTime == null && task.ProgramFileUploadTime == null && task.OtherFileTime == null)
            //                {
            //                    task.State = 2;
            //                }
            //                WebApplication2.JDJS_PDMS_CraftFile_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_CraftFile_Alter_History_Table_Row()
            //                 {
            //                     AlterDesc = fileTypeStr,
            //                     staffID = staffID,
            //                     AlterTime = DateTime.Now,
            //                     TaskID = taskID
            //                 };
            //                mod.JDJS_PDMS_CraftFile_Alter_History_Table.Add(jd);
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