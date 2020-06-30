
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// adminEdit 的摘要说明
    /// </summary>
    public class adminEdit : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try {

                var form = context.Request.Form;
                var taskId = Convert.ToInt32(context.Request.Form["taskId"]);
                var groupNum = context.Request.Form["groupNum"];
                var groupName = context.Request.Form["groupName"];
                var TaskNum = form["TaskNum"];
                var TaskName = form["TaskName"];
                var date = Convert.ToDateTime(form["date"]);
                var loginUserId = Convert.ToInt32(context.Session["id"]);
                string err = "";
                string path = "";
                var craftPersonId = Convert.ToInt32(context.Request.Form["craftPersonId"]);

                if (AlterTaskInfo(taskId, loginUserId, groupNum, groupName, TaskNum,TaskName, date, craftPersonId, ref  err,out path))
                {
                    
                    WebApplication2.PathInfo pathInfo = new WebApplication2.PathInfo();
                    var newPath = Path.Combine(pathInfo.upLoadPath(), groupNum + TaskNum);
                    var oldPath = Path.Combine(pathInfo.upLoadPath(), path);
                    if (newPath != oldPath)
                    {
                        if (Directory.Exists(oldPath))
                        {
                            DirectoryInfo dir = new DirectoryInfo(oldPath);
                            dir.MoveTo(newPath);
                        }
                    }
                    context.Response.Write("ok");

                }else{
                    context.Response.Write(err);
                }

            }
            catch(Exception ex) {
                context.Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// 修改任务信息
        /// </summary>
        /// <param name="taskID">需要修改的任务主键ID</param>
        /// <param name="staffID">修改的人员主键ID</param>
        /// <param name="groupNum">新的产品号</param>
        /// <param name="groupName">新的产品名称</param>
        /// <param name="taskNum">新的零件号</param>
        /// <param name="taskName">新的零件名称</param>
        /// <param name="demandTime">新的需求时间</param>
        /// <param name="craftPersonID">新的工艺责任人主键ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterTaskInfo(int taskID, int staffID, string groupNum, string groupName, string taskNum, string taskName, DateTime demandTime, int craftPersonID, ref string errMsg,out string path)
        {
            return true;
            //try
            //{
            //    using (WebApplication2. Model1 mod = new WebApplication2. Model1())
            //    {
            //        using (EF::System.Data.Entity.DbContextTransaction mytran = mod.Database.BeginTransaction())
            //        {
            //            try
            //            {
            //                var staff1 = mod.JDJS_PDMS_Staff_Table.Where(r => r.ID == craftPersonID).FirstOrDefault();
            //                if (staff1 == null)
            //                {
            //                    errMsg = "该员工不存在！";
            //                    mytran.Rollback();
            //                    path = "";
            //                    return false;
            //                }
            //                var staff = mod.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffID).FirstOrDefault();
            //                if (staff == null)
            //                {
            //                    errMsg = "该员工不存在！";
            //                    mytran.Rollback();
            //                    path = "";
            //                    return false;
            //                }
            //                var task = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).FirstOrDefault();
            //                if (task == null)
            //                {
            //                    errMsg = "该任务不存在！";
            //                    path = "";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                path = task.TaskNum;
            //                if (task.staffID != staffID)
            //                {
            //                    errMsg = "您不是该任务创建者，无法修改该任务！";
            //                    path = "";
            //                    mytran.Rollback();
            //                    return false;
            //                }
            //                int version = 0;
            //                if (task.Version != null)
            //                {
            //                    version = Convert.ToInt32(task.Version);
            //                }
            //                bool isAlter = false;
            //                string name = groupNum + taskNum;
            //                if (task.TaskNum != name)
            //                {
            //                    var taskOld = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.TaskNum == name);
            //                    {
            //                        if (taskOld.Count() > 0)
            //                        {
            //                            errMsg = "该零件编号已存在，请重新输入！";
            //                            mytran.Rollback();
            //                            path = "";
            //                            return false;
            //                        }
            //                    }


            //                }
            //                if (task.ComponentNum == groupNum && task.ComponentName != groupName)
            //                {
            //                    var comTask = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentNum == groupNum);
            //                    foreach (var item in comTask)
            //                    {
            //                        item.ComponentName = groupName;
            //                        if (item.Version == null)
            //                        {
            //                            item.Version = 1;
            //                            continue;
            //                        }
            //                        item.Version = item.Version + 1;
            //                    }
            //                  WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2. JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                    {
            //                        AfterVersion = version + 1,
            //                        AlterDesc = "修改产品名称",
            //                        AlterTime = DateTime.Now,
            //                        TaskID = taskID,
            //                        staffID = staffID

            //                    };
            //                    mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                    isAlter = true;
            //                }
            //                if (task.ComponentNum != groupNum)
            //                {
            //                    var group = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentNum == groupNum);
            //                    if (group.Count() < 1)
            //                    {
            //                        task.ComponentNum = groupNum;
            //                        task.TaskNum = groupNum + taskNum;
            //                        WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                        {
            //                            AfterVersion = version + 1,
            //                            AlterDesc = "增加产品编号",
            //                            AlterTime = DateTime.Now,
            //                            TaskID = taskID,
            //                            staffID = staffID

            //                        };
            //                        mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                        isAlter = true;
            //                    }
            //                    else
            //                    {
            //                        if (group.FirstOrDefault().ComponentName != groupName)
            //                        {
            //                            errMsg = String.Format("该产品编号{1}对应的名称是{0}，请重新输入！", group.FirstOrDefault().ComponentName, groupNum);
            //                            mytran.Rollback();
            //                            path = "";
            //                            return false;
            //                        }
            //                        else
            //                        {
            //                            if (group.Where(r => r.TaskNum == groupNum + taskNum).Count() > 0)
            //                            {
            //                                errMsg = String.Format("该产品编号{1}对应的零件编号{0}已存在，请重新输入！", taskNum, groupNum);
            //                                mytran.Rollback();
            //                                path = "";
            //                                return false;
            //                            }
            //                            else
            //                            {
            //                                task.ComponentNum = groupNum;
            //                                task.ComponentName = groupName;
            //                                task.TaskNum = groupNum + taskNum;
            //                                task.TaskName = taskName;
            //                                WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                                {
            //                                    AfterVersion = version + 1,
            //                                    AlterDesc = "修改产品编号",
            //                                    AlterTime = DateTime.Now,
            //                                    TaskID = taskID,
            //                                    staffID = staffID

            //                                };
            //                                mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                                isAlter = true;
            //                            }

            //                        }
            //                    }
            //                }
            //                if (task.ComponentNum == groupNum && task.TaskNum.Substring(task.ComponentNum.Length) != taskNum)
            //                {
            //                    var taskOld = mod.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentNum == groupNum && r.TaskNum == groupNum + taskNum);
            //                    if (taskOld.Count() > 0)
            //                    {
            //                        errMsg = String.Format("该产品编号{1}对应的零件编号{0}已存在，请重新输入！", taskNum, groupNum);
            //                        mytran.Rollback();
            //                        path = "";
            //                        return false;
            //                    }
            //                    else
            //                    {
            //                        task.TaskNum = groupNum + taskNum;
            //                        WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                        {
            //                            AfterVersion = version + 1,
            //                            AlterDesc = "修改零件编号",
            //                            AlterTime = DateTime.Now,
            //                            TaskID = taskID,
            //                            staffID = staffID

            //                        };
            //                        mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                        isAlter = true;
            //                    }
            //                }
            //                if (task.TaskName != taskName)
            //                {

            //                    task.TaskName = taskName;
            //                    WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                    {
            //                        AfterVersion = version + 1,
            //                        AlterDesc = "修改任务名称",
            //                        AlterTime = DateTime.Now,
            //                        TaskID = taskID,
            //                        staffID = staffID

            //                    };
            //                    mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                    isAlter = true;
            //                }
            //                if (task.DemandTime != demandTime)
            //                {

            //                    task.DemandTime = demandTime;
            //                    WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                    {
            //                        AfterVersion = version + 1,
            //                        AlterDesc = "修改任务需求时间",
            //                        AlterTime = DateTime.Now,
            //                        TaskID = taskID,
            //                        staffID = staffID

            //                    };
            //                    mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                    isAlter = true;
            //                }
            //                if (task.CraftPersonID != craftPersonID)
            //                {

            //                    task.CraftPersonID = craftPersonID;
            //                    WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row jd = new WebApplication2.JDJS_PDMS_TaskInfo_Alter_History_Table_Row()
            //                    {
            //                        AfterVersion = version + 1,
            //                        AlterDesc = "修改任务工艺责任人",
            //                        AlterTime = DateTime.Now,
            //                        TaskID = taskID,
            //                        staffID = staffID

            //                    };
            //                    mod.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
            //                    task.State = 1;
            //                    task.ArrangeTaskTime = DateTime.Now;
            //                    task.AcceptTaskTime = null;

            //                    isAlter = true;
            //                }
            //                if (isAlter)
            //                {
            //                    task.Version = version + 1;
            //                }

            //                mod.SaveChanges();

            //                mytran.Commit();
            //                return true;

            //            }
            //            catch (Exception ex)
            //            {
            //                mytran.Rollback();
            //                errMsg = ex.Message;
            //                path = "";
            //                return false;
            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    errMsg = ex.Message;
            //    path = "";
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