﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using 北京工艺文件管理MVC.Database;

namespace 北京工艺文件管理MVC.Models
{
    public class TaskInfoManage
    {
        public static bool GetIFDatabaseInfoAboutPercessFile(string taskInfo,out DateTime time)
        {
            try
            {
                using (IFDBDatabase.JDXA_DNC_DB_JSEntities model = new IFDBDatabase.JDXA_DNC_DB_JSEntities())
                {
                    var file = model.jdxa_file_info.Where(r => r.FILE_NAME == taskInfo).FirstOrDefault();
                    if (file == null)
                    {
                        time = DateTime.Now;
                        return false;
                    }
                    else
                    {
                        time = Convert.ToDateTime(file.CREATE_TIME);
                        return true;
                    }
                }
                //time = DateTime.Now;
                //return false;
            }
            catch (Exception ex)
            {
                time = DateTime.Now;
                return false;
            }

        }

        /// <summary>
        /// 根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfo(DatabaseList model, int parentID, string orderNum, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常");
                    foreach (var item in tasks)
                    {
                        CompontInfo task = new CompontInfo();
                        task.componentID = parentID;
                        task.acceptTaskTime = item.AcceptTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTaskTime);
                        task.acceptTaskTimeStr = item.AcceptTaskTime == null ? "" : item.AcceptTaskTime.ToString().Substring(0, item.AcceptTaskTime.ToString().LastIndexOf(':'));
                        task.arrangeTaskTime = item.ArrangeTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.ArrangeTaskTime);
                        task.arrangeTaskTimeStr = item.ArrangeTaskTime == null ? "" : item.ArrangeTaskTime.ToString().Substring(0, item.ArrangeTaskTime.ToString().LastIndexOf(':'));
                        task.completeTime = item.CompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.CompleteTime);
                        task.completeTimeStr = item.CompleteTime == null ? "" : item.CompleteTime.ToString().Substring(0, item.CompleteTime.ToString().LastIndexOf(':'));
                        task.craftPersonID = item.CraftPersonID == null ? 0 : Convert.ToInt32(item.CraftPersonID);
                        task.craftPersonName = "";
                        
                        if (item.CraftPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CraftPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.craftPersonName = staff.StaffName;
                                string str = task.craftPersonID.ToString();
                                str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model,Convert.ToInt32(staff.PosiationID)) + "_");
                                task.personAllId = str;
                            }
                        }

                        task.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        task.creatPersonName = "";
                        if (item.CreatPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.creatPersonName = staff.StaffName;
                            }
                        }
                        task.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        task.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        task.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                        task.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(':'));
                        task.id = item.ID;
                        //task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        //task.lastAlterPersonName = "";
                        //if (item.LastAlterPersonID != null)
                        //{
                        //    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        //    if (staff != null)
                        //    {
                        //        task.lastAlterPersonName = staff.StaffName;
                        //    }
                        //}
                        //task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        //task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        //task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        //task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                       // task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        { 
                            DateTime time=new DateTime ();
                            if (GetIFDatabaseInfoAboutPercessFile(item.TaskName+"-" +item.TaskNum +"-10",out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }

                        //task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        //task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                       // task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                        task.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                        task.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                        task.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                        task.staffName = "";
                        if (item.staffID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.staffName = staff.StaffName;
                            }
                        }
                        task.toolingDrawingsUploadIsNeed = "无需工装";
                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        if (processes.Count() < 1)
                        {
                            task.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processes)
                        {
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                task.toolingDrawingsUploadIsNeed = "";
                            }
                        }
                        if(task.toolingDrawingsUploadIsNeed == "无需工装")
                        {
                            task.ToolingDrawingsUploadTimeStr = "无需工装";
                        }
                        task.state = item.state;
                        task.taskName = item.TaskName;
                        task.taskNum = item.TaskNum;
                        task.taskState = item.TaskState == null ? 0 : Convert.ToInt32(item.TaskState);
                        task.version = item.Version == null ? 0 : Convert.ToInt32(item.Version);
                        task.allTaskNum = 1;
                        task.unfinishedNum = 1;
                        unfinish++;
                        switch (task.taskState)
                        {
                            case 0:
                                task.taskStateStr = "待完成";
                                break;
                            case 1:
                                task.taskStateStr = "待完成";
                                break;
                            case 2:
                                task.taskStateStr = "待完成";
                                break;
                            case 3:
                                task.taskStateStr = "待完成";
                                break;
                            case 4:
                                task.taskStateStr = "待完成";
                                break;
                            case 5:
                                task.taskStateStr = "已完成";
                                unfinish--;
                                task.unfinishedNum--;
                                break;
                            case 6:
                                break;
                            case 7:
                                break;
                            case 8:
                                break;
                            default:
                                break;

                        }
                        task.previewCategory = new List<int>();
                        var types = model.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        foreach (var type in types)
                        {
                            task.previewCategory.Add(Convert.ToInt32(type.CategoryTypeID));
                        }
                        task.ispart = true;
                        task.previewCategoryStr = "";
                        foreach (var type in task.previewCategory)
                        {
                            var jd = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == type && r.state == "正常").FirstOrDefault();
                            if (jd != null)
                            {
                                task.previewCategoryStr += jd.ProcessingType + ",";
                            }
                        }
                        if (task.previewCategoryStr.Length > 0)
                        {
                            task.previewCategoryStr = task.previewCategoryStr.Substring(0, task.previewCategoryStr.Length - 1);
                        }


                        taskInfos.Add(task);


                    }
                }
                unfinishNum = unfinish;
                taskInfos = taskInfos.OrderBy(r => r.taskState).ThenBy(r => r.planEndTime).ToList();
                return taskInfos;
            }
            catch (Exception ex)
            {
                unfinishNum = 0;
                return null;
            }

        }


        /// <summary>
        /// 根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoCreate(DatabaseList model, int parentID, string orderNum, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常");
                    foreach (var item in tasks)
                    {
                        CompontInfo task = new CompontInfo();
                        task.componentID = parentID;
                        task.acceptTaskTime = item.AcceptTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTaskTime);
                        task.acceptTaskTimeStr = item.AcceptTaskTime == null ? "" : item.AcceptTaskTime.ToString().Substring(0, item.AcceptTaskTime.ToString().LastIndexOf(':'));
                        task.arrangeTaskTime = item.ArrangeTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.ArrangeTaskTime);
                        task.arrangeTaskTimeStr = item.ArrangeTaskTime == null ? "" : item.ArrangeTaskTime.ToString().Substring(0, item.ArrangeTaskTime.ToString().LastIndexOf(':'));
                        task.completeTime = item.CompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.CompleteTime);
                        task.completeTimeStr = item.CompleteTime == null ? "" : item.CompleteTime.ToString().Substring(0, item.CompleteTime.ToString().LastIndexOf(':'));
                        task.craftPersonID = item.CraftPersonID == null ? 0 : Convert.ToInt32(item.CraftPersonID);
                        task.craftPersonName = "";
                        if (item.CraftPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CraftPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.craftPersonName = staff.StaffName;
                                string str = task.craftPersonID.ToString();
                                str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model,Convert.ToInt32(staff.PosiationID)) + "_");
                                task.personAllId = str;
                            }
                        }

                        task.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        task.creatPersonName = "";
                        if (item.CreatPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.creatPersonName = staff.StaffName;
                            }
                        }
                        task.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        task.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        task.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                        task.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(':'));
                        task.id = item.ID;
                        //task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        //task.lastAlterPersonName = "";
                        //if (item.LastAlterPersonID != null)
                        //{
                        //    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        //    if (staff != null)
                        //    {
                        //        task.lastAlterPersonName = staff.StaffName;
                        //    }
                        //}
                        //task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        //task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        //task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                       // task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        //task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }

                       // task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        //task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                       // task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                        task.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                        task.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                        task.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                        task.staffName = "";
                        if (item.staffID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.staffName = staff.StaffName;
                            }
                        }
                        task.toolingDrawingsUploadIsNeed = "无需工装";
                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        if (processes.Count() < 1)
                        {
                            task.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processes)
                        {
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                task.toolingDrawingsUploadIsNeed = "";
                            }
                        }
                        if (task.toolingDrawingsUploadIsNeed == "无需工装")
                        {
                            task.ToolingDrawingsUploadTimeStr = "无需工装";
                        }
                        task.state = item.state;
                        task.taskName = item.TaskName;
                        task.taskNum = item.TaskNum;
                        task.taskState = item.TaskState == null ? 0 : Convert.ToInt32(item.TaskState);
                        task.version = item.Version == null ? 0 : Convert.ToInt32(item.Version);
                        task.allTaskNum = 1;
                        task.unfinishedNum = 1;
                        unfinish++;
                        switch (task.taskState)
                        {
                            case 0:
                                task.taskStateStr = "待完成";
                                break;
                            case 1:
                                task.taskStateStr = "待完成";
                                break;
                            case 2:
                                task.taskStateStr = "待完成";
                                break;
                            case 3:
                                task.taskStateStr = "待完成";
                                break;
                            case 4:
                                task.taskStateStr = "待完成";
                                break;
                            case 5:
                                task.taskStateStr = "已完成";
                                unfinish--;
                                task.unfinishedNum--;
                                break;
                            case 6:
                                break;
                            case 7:
                                break;
                            case 8:
                                break;
                            default:
                                break;

                        }
                        task.previewCategory = new List<int>();
                        var types = model.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        foreach (var type in types)
                        {
                            task.previewCategory.Add(Convert.ToInt32(type.CategoryTypeID));
                        }
                        task.ispart = true;
                        task.previewCategoryStr = "";
                        foreach (var type in task.previewCategory)
                        {
                            var jd = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == type && r.state == "正常").FirstOrDefault();
                            if (jd != null)
                            {
                                task.previewCategoryStr += jd.ProcessingType + ",";
                            }
                        }
                        if (task.previewCategoryStr.Length > 0)
                        {
                            task.previewCategoryStr = task.previewCategoryStr.Substring(0, task.previewCategoryStr.Length - 1);
                        }


                        taskInfos.Add(task);


                    }
                }
                unfinishNum = unfinish;
                taskInfos = taskInfos.OrderByDescending(r => r.creatTime).ToList();
                return taskInfos;
            }
            catch (Exception ex)
            {
                unfinishNum = 0;
                return null;
            }

        }
        /// <summary>
        /// 获取任务所在文件夹
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static string GetTaskPath(int taskID)
        {
            string path = "";
            int ComponentID = -1;
            using (Model1 model = new Model1())
            {
                var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).FirstOrDefault();
                if (task == null)
                {
                    return "";
                }
                ComponentID = Convert.ToInt32(task.ComponentID);

                path = path.Insert(0, task.TaskNum + @"\");
            }
            path = path.Insert(0, TaskInfoManage.GetCompontPath(ComponentID));
            return path;
        }
        /// <summary>
        /// 获取组件所在文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetCompontPath(int id)
        {
            using (Model1 model = new Model1())
            {
                string path = "";
                var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == id && r.state == "正常").FirstOrDefault();
                if (task != null)
                {
                    path = path.Insert(0, task.ComponentNum + @"\");
                    if (task.ParentID != 0)
                    {
                        path = path.Insert(0, TaskInfoManage.GetCompontPath(Convert.ToInt32(task.ParentID)));
                    }


                }
                return path;
            }
        }

        /// <summary>
        /// 创建零件
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="taskName">零件名称</param>
        /// <param name="taskNum">零件图号</param>
        /// <param name="creatPersonID">创建人</param>
        /// <param name="planStartTime">计划开始时间</param>
        /// <param name="planEndTime">计划结束时间</param>
        /// <param name="craftPersonID">工艺责任人</param>
        /// <param name="previewCategory">预览组别</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddTask(int parent, string taskName, string taskNum, int creatPersonID, DateTime planStartTime, DateTime planEndTime, int craftPersonID, List<int> previewCategory, out int taskId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parent && r.TaskNum == taskNum && r.state == "正常");
                            if (task.Count() > 0)
                            {
                                mytran.Rollback();
                                errMsg = "该零件图号已存在，请重新输入！";
                                taskId = -1;
                                return false;
                            }
                            var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parent && r.ComponentNum == taskNum && r.state == "正常");
                            if (compont.Count() > 0)
                            {
                                mytran.Rollback();
                                errMsg = "该编号已存在，请重新输入！";
                                taskId = -1;
                                return false;
                            }
                            JDJS_PDMS_TaskInfo_Table jd = new JDJS_PDMS_TaskInfo_Table()
                            {
                                ArrangeTaskTime = DateTime.Now,
                                ComponentID = parent,
                                CraftPersonID = craftPersonID,
                                CreatPersonID = creatPersonID,
                                CreatTime = DateTime.Now,
                                DemandTime = planEndTime,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                PlanCompletionTime = planEndTime,
                                PlanEndTime = planEndTime,
                                PlanStartTime = planStartTime,
                                staffID = creatPersonID,
                                state = "正常",
                                TaskName = taskName,
                                TaskNum = taskNum,
                                Version = 0,
                                TaskState = 1

                            };
                            model.JDJS_PDMS_TaskInfo_Table.Add(jd);
                            model.SaveChanges();
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_TaskInfo_Table.Add(jd);
                            var jdID = jd.ID;
                            foreach (var item in previewCategory)
                            {
                                JDJS_PDMS_Task_ProcessType_Table type = new JDJS_PDMS_Task_ProcessType_Table()
                                {
                                    CategoryTypeID = item,
                                    CreatPersonID = creatPersonID,
                                    CreatTime = DateTime.Now,
                                    LastAlterPersonID = creatPersonID,
                                    LastAlterTime = DateTime.Now,
                                    state = "正常",
                                    TaskID = jdID

                                };
                                model.JDJS_PDMS_Task_ProcessType_Table.Add(type);
                                model .SaveChanges ();
                                da.JDJS_PDMS_Task_ProcessType_Table.Add(type);

                            }
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            taskId = jdID;
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            taskId = -1;
                            return false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                taskId = -1;
                return false;
            }
        }


        public static bool DownLoadTaskToolDrawFiles(int taskID, ref string path)
        {
            try
            {
                string compantPath = GetTaskPath(taskID);
                string num = "";
                using (Model1 model = new Model1())
                {
                    var compant = model.JDJS_PDMS_TaskInfo_Table .Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (compant != null)
                    {
                        num = compant.TaskNum;
                    }
                }
                if (num != "")
                {
                    ZipHelpClass zip = new ZipHelpClass();
                    SupplementaryInformation sup = new SupplementaryInformation();
                    string pathDir = sup.upLoadPath();
                    if (!Directory.Exists(Path.Combine(pathDir, "ZipFiles")))
                    {
                        Directory.CreateDirectory(Path.Combine(pathDir, "ZipFiles"));
                    }
                    if (!Directory.Exists(Path.Combine(pathDir, "工装图纸")))
                    {
                        Directory.CreateDirectory(Path.Combine(pathDir, "工装图纸"));
                    }
                    if (zip.CreateDirectory(Path.Combine(pathDir, compantPath), Path.Combine(pathDir, "工装图纸", num)))
                    {
                        if (Directory.Exists(Path.Combine(pathDir, "工装图纸", num)))
                        {
                            zip.ZipFile(Path.Combine(pathDir, "工装图纸", num), Path.Combine(pathDir, "ZipFiles", num + ".zip"));
                            path = Path.Combine(sup.downLoadPath(), "ZipFiles", num + ".zip");
                            return true;
                        }
                        else
                        {
                            path = "无工装图纸可下载！";
                            return false;
                        }
                    }
                    else
                    {
                        path = "出现未知错误";
                        return false;
                    }
                }
                else
                {
                    path = "";
                    return false;
                }

            }
            catch (Exception ex)
            {
                path = ex.Message;
                return false;
            }
        }


        public static bool DownLoadToolDrawFiles(int compantID,ref string path)
        {
            try
            {
                string compantPath = GetCompontPath(compantID);
                string num = "";
                using (Model1 model = new Model1())
                {
                    var compant = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compantID&&r.state =="正常").FirstOrDefault();
                    if (compant != null)
                    {
                        num = compant.ComponentNum;
                    }
                }
                if (num != "")
                {
                    ZipHelpClass zip = new ZipHelpClass();
                    SupplementaryInformation sup = new SupplementaryInformation();
                    string pathDir = sup.upLoadPath();
                    if (!Directory.Exists(Path.Combine(pathDir, "ZipFiles")))
                    {
                        Directory.CreateDirectory(Path.Combine(pathDir, "ZipFiles"));
                    }
                    if (!Directory.Exists(Path.Combine(pathDir, "工装图纸")))
                    {
                        Directory.CreateDirectory(Path.Combine(pathDir, "工装图纸"));
                    }
                    if (zip.CreateDirectory(Path.Combine(pathDir, compantPath), Path.Combine(pathDir, "工装图纸", num)))
                    {
                        if (Directory.Exists(Path.Combine(pathDir, "工装图纸", num)))
                        {
                            zip.ZipFile(Path.Combine(pathDir, "工装图纸", num), Path.Combine(pathDir, "ZipFiles", num + ".zip"));
                            path = Path.Combine(sup.downLoadPath(), "ZipFiles", num + ".zip");
                            return true;
                        }
                        else
                        {
                            path = "无工装图纸可下载！";
                            return false;
                        }
                    }
                    else
                    {
                        path = "出现未知错误";
                        return false;
                    }
                }
                else
                {
                    path = "";
                    return false;
                }

            }
            catch (Exception ex)
            {
                path = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 上传零件图纸
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="personId">人员Id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddTaskFile(int taskId, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该任务不存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                            {
                                AfterVersion =task.Version +1,
                                AlterDesc ="上传零件任务图纸",
                                AlterTime=DateTime .Now ,
                                CreatPersonID =personId ,
                                CreatTime =DateTime .Now ,
                                staffID =personId ,
                                state ="正常",
                                TaskID =task.ID 
                            };
                            model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            errMsg = "ok";
                            mytran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除零件图纸
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="personId">人员Id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteTaskFile(int taskId, int personId,List<string> fileNames, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该任务不存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                            {
                                AfterVersion = task.Version + 1,
                                AlterDesc = "删除零件任务图纸",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                TaskID = task.ID
                            };
                            model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            errMsg = "ok";
                            mytran.Commit();
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            return false;
                        }
                    }

                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                    foreach (var item in fileNames)
                    {
                        var path = Path.Combine(dirPath, item);
                        if (File.Exists(path))
                        {
                            File.Delete(Path.Combine(dirPath, item));
                        }
                    }
                    errMsg = "ok";
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }



        /// <summary>
        /// 获取所有的组别
        /// </summary>
        /// <returns></returns>
        public static List<PreviewCategoryInfo> GetPreviewCategory()
        {
            try
            {
                List<PreviewCategoryInfo> previewCategoryInfoList = new List<PreviewCategoryInfo>();
                using (Model1 model = new Model1())
                {
                    var category = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.state == "正常");
                    foreach (var item in category)
                    {
                        PreviewCategoryInfo cat = new PreviewCategoryInfo();
                        cat.id = item.ID;
                        cat.Name = item.ProcessingType;
                        previewCategoryInfoList.Add(cat);
                    }
                }
                return previewCategoryInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 删除零件任务
        /// </summary>
        /// <param name="taskID">要删除的零件任务ID</param>
        /// <param name="alterPersonID">删除人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteTask(int taskID, int alterPersonID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该零件不存在";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().state = "删除";
                            task.state = "删除";
                            task.LastAlterPersonID = alterPersonID;
                            task.LastAlterTime = DateTime.Now;
                            task.Version++;
                            JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                            {
                                AfterVersion = task.Version,
                                AlterDesc = "删除零件任务",
                                AlterTime = DateTime.Now,
                                CreatPersonID = alterPersonID,
                                CreatTime = DateTime.Now,
                                staffID = alterPersonID,
                                state = "正常",
                                TaskID = taskID
                            };
                            model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";


                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改零件任务信息
        /// </summary>
        /// <param name="taskID">要修改的零件任务ID</param>
        /// <param name="alterPersonID">修改人员ID</param>
        /// <param name="parentID">零件任务父节点</param>
        /// <param name="taskName">零件任务名称</param>
        /// <param name="taskNum">零件任务编号</param>
        /// <param name="planStartTime">计划开始时间</param>
        /// <param name="planEndTime">计划结束时间</param>
        /// <param name="craftPersonID">工艺责任人</param>
        /// <param name="previewCategory">预览组别</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterTask(int taskID, int alterPersonID, int parentID, string taskName, string taskNum, DateTime? planStartTime, DateTime? planEndTime, int craftPersonID, List<int> previewCategory, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该零件任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.ComponentID == parentID && task.TaskNum == taskNum)
                    {
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                
                                bool isAlter = false;
                                if (task.TaskName != taskName)
                                {
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().TaskName = taskName;
                                    string name = task.TaskName;
                                    task.TaskName = taskName;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务名称，由" + name + "修改为" + taskName,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (task.PlanStartTime != planStartTime)
                                {
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().PlanStartTime = planStartTime;
                                    string time = task.PlanStartTime == null ? "" : task.PlanStartTime.ToString();
                                    task.PlanStartTime = planStartTime;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务计划开始时间，由" + time + "修改为" + planStartTime.ToString(),
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (task.PlanEndTime != planEndTime)
                                {
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().PlanEndTime = planEndTime;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().PlanCompletionTime = planEndTime;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().DemandTime = planEndTime;

                                    string time = task.PlanEndTime == null ? "" : task.PlanEndTime.ToString();
                                    task.PlanEndTime = planEndTime;
                                    task.PlanCompletionTime = planEndTime;
                                    task.DemandTime = planEndTime;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务计划结束时间，由" + time + "修改为" + planEndTime.ToString(),
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }

                                List<int> typeList = new List<int>();
                                var type = model.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.TaskID == taskID && r.state == "正常");
                                foreach (var item in type)
                                {
                                    if (item.CategoryTypeID != null)
                                    {
                                        typeList.Add(Convert.ToInt32(item.CategoryTypeID));

                                    }
                                }
                                foreach (var item in previewCategory)
                                {
                                    if (!typeList.Contains(item))
                                    {
                                        JDJS_PDMS_Task_ProcessType_Table typeNew = new JDJS_PDMS_Task_ProcessType_Table()
                                        {
                                            CategoryTypeID = item,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            LastAlterPersonID = alterPersonID,
                                            LastAlterTime = DateTime.Now,
                                            state = "正常",
                                            TaskID = taskID

                                        };
                                        model.JDJS_PDMS_Task_ProcessType_Table.Add(typeNew);
                                        model.SaveChanges();
                                        da.JDJS_PDMS_Task_ProcessType_Table.Add(typeNew);
                                        JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                        {
                                            AfterVersion = task.Version + 1,
                                            AlterDesc = "增加预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            TaskID = taskID
                                        };
                                        model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }

                                foreach (var item in typeList)
                                {
                                    if (!previewCategory.Contains(item))
                                    {
                                        var taskOld = model.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.TaskID == taskID && r.CategoryTypeID == item && r.state == "正常");
                                        foreach (var real in taskOld)
                                        {
                                            da.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.ID == real.ID).First().state = "删除";
                                        }
                                        model.JDJS_PDMS_Task_ProcessType_Table.RemoveRange(taskOld);
                                        JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                        {
                                            AfterVersion = task.Version + 1,
                                            AlterDesc = "删除预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            TaskID = taskID
                                        };
                                        model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }
                                if (task.CraftPersonID != craftPersonID)
                                {
                                    string nameOld = "";
                                    string nameNew = "";
                                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == task.CraftPersonID && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameOld = staff.StaffName;
                                    }
                                    staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == craftPersonID && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameNew = staff.StaffName;
                                    }
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().CraftPersonID = craftPersonID;
                                    task.CraftPersonID = craftPersonID;
                                    task.ArrangeTaskTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().ArrangeTaskTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().TaskState = 1;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务工艺责任人Z，由" + nameOld + "修改为" + nameNew,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                    task.TaskState = 1;
                                }
                                if (isAlter)
                                {
                                    task.Version++;
                                }

                                model.SaveChanges();
                                mytran.Commit();
                                errMsg = "ok";
                                return true;
                            }
                            catch (Exception ex)
                            {
                                mytran.Rollback();
                                errMsg = ex.Message;
                                return false;

                            }
                        }
                    }

                    else if (task.ComponentID == parentID && task.TaskNum != taskNum)
                    {

                        var taskAll = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.TaskNum == taskNum && r.state == "正常");
                        if (taskAll.Count() > 0)
                        {
                            errMsg = "该零件图号已存在，请重新输入！";
                            return false;
                        }
                        var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.ComponentNum == taskNum && r.state == "正常");
                        if (compont.Count() > 0)
                        {
                            errMsg = "该编号已存在，请重新输入！";
                            return false;
                        }

                        SupplementaryInformation pathInfo = new SupplementaryInformation();
                        var pathTaskOld = TaskInfoManage.GetTaskPath(taskID);
                        var dirPathOld = Path.Combine(pathInfo.upLoadPath(), pathTaskOld);

                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().TaskNum = taskNum;
                                string num = task.TaskNum;
                                task.TaskNum = taskNum;
                                JDJS_PDMS_TaskInfo_Alter_History_Table jdOne = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                {
                                    AfterVersion = task.Version + 1,
                                    AlterDesc = "修改零件任务编号，由" + num + "修改为" + taskNum,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = alterPersonID,
                                    CreatTime = DateTime.Now,
                                    staffID = alterPersonID,
                                    state = "正常",
                                    TaskID = taskID
                                };
                                model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jdOne);


                                bool isAlter = true;
                                if (task.TaskName != taskName)
                                {
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().TaskName = taskName;
                                    string name = task.TaskName;
                                    task.TaskName = taskName;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务名称，由" + name + "修改为" + taskName,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (task.PlanStartTime != planStartTime)
                                {
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().PlanStartTime = planStartTime;
                                    string time = task.PlanStartTime == null ? "" : task.PlanStartTime.ToString();
                                    task.PlanStartTime = planStartTime;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务计划开始时间，由" + time + "修改为" + planStartTime.ToString(),
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (task.PlanEndTime != planEndTime)
                                {
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().PlanEndTime = planEndTime;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().PlanCompletionTime = planEndTime;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().DemandTime = planEndTime;

                                    string time = task.PlanEndTime == null ? "" : task.PlanEndTime.ToString();
                                    task.PlanEndTime = planEndTime;
                                    task.PlanCompletionTime = planEndTime;
                                    task.DemandTime = planEndTime;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务计划结束时间，由" + time + "修改为" + planEndTime.ToString(),
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }

                                List<int> typeList = new List<int>();
                                var type = model.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.TaskID == taskID && r.state == "正常");
                                foreach (var item in type)
                                {
                                    if (item.CategoryTypeID != null)
                                    {
                                        typeList.Add(Convert.ToInt32(item.CategoryTypeID));

                                    }
                                }
                                foreach (var item in previewCategory)
                                {
                                    if (!typeList.Contains(item))
                                    {
                                        JDJS_PDMS_Task_ProcessType_Table typeNew = new JDJS_PDMS_Task_ProcessType_Table()
                                        {
                                            CategoryTypeID = item,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            LastAlterPersonID = alterPersonID,
                                            LastAlterTime = DateTime.Now,
                                            state = "正常",
                                            TaskID = taskID

                                        };
                                        model.JDJS_PDMS_Task_ProcessType_Table.Add(typeNew);
                                        model.SaveChanges();
                                        da.JDJS_PDMS_Task_ProcessType_Table.Add(typeNew);
                                        JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                        {
                                            AfterVersion = task.Version + 1,
                                            AlterDesc = "增加预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            TaskID = taskID
                                        };
                                        model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }

                                foreach (var item in typeList)
                                {
                                    if (!previewCategory.Contains(item))
                                    {
                                        var taskOld = model.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.TaskID == taskID && r.CategoryTypeID == item && r.state == "正常");
                                        foreach (var real in taskOld)
                                        {
                                            da.JDJS_PDMS_Task_ProcessType_Table.Where(r => r.ID == real.ID).First().state = "删除";
                                        }
                                        model.JDJS_PDMS_Task_ProcessType_Table.RemoveRange(taskOld);
                                        JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                        {
                                            AfterVersion = task.Version + 1,
                                            AlterDesc = "删除预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            TaskID = taskID
                                        };
                                        model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }
                                if (task.CraftPersonID != craftPersonID)
                                {
                                    string nameOld = "";
                                    string nameNew = "";
                                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == task.CraftPersonID && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameOld = staff.StaffName;
                                    }
                                    staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == craftPersonID && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameNew = staff.StaffName;
                                    }
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().CraftPersonID = craftPersonID;
                                    task.CraftPersonID = craftPersonID;
                                    task.ArrangeTaskTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().ArrangeTaskTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID).First().TaskState = 1;
                                    JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                    {
                                        AfterVersion = task.Version + 1,
                                        AlterDesc = "修改零件任务工艺责任人Z，由" + nameOld + "修改为" + nameNew,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        TaskID = taskID
                                    };
                                    model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                    task.TaskState = 1;
                                }
                                if (isAlter)
                                {
                                    task.Version++;
                                }

                                model.SaveChanges();
                                mytran.Commit();
                                errMsg = "ok";

                            }
                            catch (Exception ex)
                            {
                                mytran.Rollback();
                                errMsg = ex.Message;
                                return false;

                            }
                        }

                        var pathTaskNew = TaskInfoManage.GetCompontPath(parentID);
                        var dirPathNew = Path.Combine(pathInfo.upLoadPath(), pathTaskNew, taskNum);

                        DirectoryInfo dir = new DirectoryInfo(pathTaskOld);
                        if (dir.Exists)
                        {
                            dir.MoveTo(pathTaskNew);
                        }
                        return true;

                    }
                    else
                    {
                        errMsg = "父节点暂不支持修改！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;

            }
        }

    }

    public class PreviewCategoryInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 组别名称
        /// </summary>
        public string Name;
    }
}