﻿using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using 北京工艺文件管理MVC.Database;
using 北京工艺文件管理MVC.DBDatabase;

namespace 北京工艺文件管理MVC.Models
{
    public class CraftDesignInfoManage
    {
        /// <summary>
        /// 获取工艺责任人所有的任务
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="orderNum"></param>
        /// <param name="creaftPersonID"></param>
        /// <returns></returns>
        public static List<CompontInfo> GetTaskInfoByCreaftPerson(DatabaseList model, int parentID, string orderNum, int craftPersonID)
        {
            try
            {
                List<CompontInfo> compontInfos = new List<CompontInfo>();
               // using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        CompontInfo compontInfo = new CompontInfo();
                        compontInfo.componentID = parentID;
                        compontInfo.taskName = item.ComponentName;
                        compontInfo.taskNum = item.ComponentNum;
                        compontInfo.allConponentNum = orderNum + item.ComponentNum;
                        compontInfo.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        compontInfo.creatPersonName = "";
                        var staffCreat = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                        if (staffCreat != null)
                        {
                            compontInfo.creatPersonName = staffCreat.StaffName;
                        }
                        compontInfo.staffID = compontInfo.creatPersonID;
                        compontInfo.staffName = compontInfo.creatPersonName;
                        compontInfo.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        compontInfo.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        compontInfo.id = item.ID;
                        compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        compontInfo.lastAlterPersonName = "";
                        var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        if (stafflastAlter != null)
                        {
                            compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        }
                        compontInfo.ispart = false;
                        compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));


                        {
                            compontInfo.toolingDrawingsUploadFlag = "无";
                            compontInfo.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == craftPersonID && (real.ProcessFileUploadIsComplete == 0||real.ProcessFileUploadIsComplete ==null))
                                {
                                    compontInfo.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == craftPersonID && real.ProcessFileUploadIsComplete == 1)
                                {
                                    compontInfo.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == craftPersonID && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == craftPersonID && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }


                        {
                            compontInfo.acceptTaskTime = item.AcceptTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTaskTime);
                            compontInfo.acceptTaskTimeStr = item.AcceptTaskTime == null ? "" : item.AcceptTaskTime.ToString().Substring(0, item.AcceptTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.arrangeTaskTime = item.ArrangeTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.ArrangeTaskTime);
                            compontInfo.arrangeTaskTimeStr = item.ArrangeTaskTime == null ? "" : item.ArrangeTaskTime.ToString().Substring(0, item.ArrangeTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.completeTime = item.CompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.CompleteTime);
                            compontInfo.completeTimeStr = item.CompleteTime == null ? "" : item.CompleteTime.ToString().Substring(0, item.CompleteTime.ToString().LastIndexOf(':'));
                            compontInfo.craftPersonID = item.CraftPersonID == null ? 0 : Convert.ToInt32(item.CraftPersonID);
                            compontInfo.craftPersonName = "";
                            if (item.CraftPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CraftPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.craftPersonName = staff.StaffName;
                                    string str = compontInfo.craftPersonID.ToString();
                                    str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model,Convert.ToInt32(staff.PosiationID)) + "_");
                                    compontInfo.personAllId = str;
                                }
                            }
                            compontInfo.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                            compontInfo.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(":"));
                            compontInfo.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanCompletionTime);
                            compontInfo.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(item.PlanCompletionTime.ToString().LastIndexOf(':'));
                            compontInfo.previewCategory = new List<int>();
                            var types = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var type in types)
                            {
                                compontInfo.previewCategory.Add(Convert.ToInt32(type.CategoryTypeID));
                            }
                            compontInfo.previewCategoryStr = "";
                            foreach (var type in compontInfo.previewCategory)
                            {
                                var jd = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == type && r.state == "正常").FirstOrDefault();
                                if (jd != null)
                                {
                                    compontInfo.previewCategoryStr += jd.ProcessingType + ",";
                                }
                            }
                            if (compontInfo.previewCategoryStr.Length > 0)
                            {
                                compontInfo.previewCategoryStr = compontInfo.previewCategoryStr.Substring(0, compontInfo.previewCategoryStr.Length - 1);
                            }
                            compontInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            compontInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            compontInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            compontInfo.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                            compontInfo.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                            compontInfo.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            compontInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            compontInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            if (item.ProcessFileUploadTime == null)
                            {
                                DateTime time = new DateTime();
                                if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.ComponentName + "-" + item.ComponentNum + "-10", out time))
                                {
                                    compontInfo.processFileUploadTime = time;
                                    compontInfo.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                                }
                            }
                            compontInfo.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                            compontInfo.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                            compontInfo.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            compontInfo.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            compontInfo.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            compontInfo.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            compontInfo.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                            compontInfo.staffName = "";
                            if (item.staffID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.staffName = staff.StaffName;
                                }
                            }
                            
                            compontInfo.toolingDrawingsUploadIsNeed = "无需工装";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            if (processes.Count() < 1)
                            {
                                compontInfo.toolingDrawingsUploadIsNeed = "";
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadIsNeed == 1)
                                {
                                    compontInfo.toolingDrawingsUploadIsNeed = "";
                                    if (real.ToolingDrawingsUploadIsComplete != 1)
                                    {
                                        compontInfo.toolingDrawingsUploadCompleteTimeStr = "";
                                        compontInfo.ToolingDrawingsUploadTimeStr = "";
                                    }
                                    
                                }
                                if (real.ProcessFileUploadIsComplete != 1)
                                {
                                    compontInfo.processFileUploadCompleteTimeStr = "";
                                    compontInfo.processFileUploadTimeStr = "";
                                }
                            }
                            if (compontInfo.toolingDrawingsUploadIsNeed == "无需工装")
                            {
                                compontInfo.ToolingDrawingsUploadTimeStr = "无需工装";
                            }
                            compontInfo.state = item.state;
                            compontInfo.taskState = item.TaskState == null ? 0 : Convert.ToInt32(item.TaskState);
                            compontInfo.version = item.Version == null ? 0 : Convert.ToInt32(item.Version);
                            switch (compontInfo.taskState)
                            {
                                case 0:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 1:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 2:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 3:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 4:
                                    compontInfo.taskStateStr = "工序处理中";
                                    break;
                                case 5:
                                    compontInfo.taskStateStr = "已完成";
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

                        }

                        compontInfo.children = CraftDesignInfoManage.GetTaskInfoByCreaftPerson(model,item.ID, compontInfo.allConponentNum + ".", craftPersonID);
                        var unendFlag = true;
                        {
                            var count = compontInfo.children.Where(r => r.taskStateStr != "已完成").Count();
                            if (count > 0)
                            {
                                unendFlag = false;
                            }

                        }
                        if (compontInfo.children != null)
                        {
                            foreach (var real in compontInfo.children)
                            {
                                compontInfo.allTaskNum += real.allTaskNum;
                                compontInfo.unfinishedNum += real.unfinishedNum;
                            }
                        }
                        int unfinish = 0;
                        var list = CraftDesignInfoManage.GetPartTaskInfoByCreaftPerson(model,item.ID, compontInfo.allConponentNum + ".", craftPersonID, out unfinish);

                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
                        if (compontInfo.children == null || compontInfo.children.Count() < 1)
                        {
                            if (item.CraftPersonID != craftPersonID)
                            {
                                continue;
                            }
                        }
                        if (!(compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum < 1 && (unendFlag)))
                        {
                            compontInfo.taskStateStr = "待完成";
                        }
                        compontInfos.Add(compontInfo);
                    }
                }
                
                compontInfos = compontInfos.OrderByDescending(r => r.unfinishedNum).ThenBy(r => r.planCompletionTime).ToList();
                return compontInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据父节点获取工艺责任人零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoByCreaftPerson(DatabaseList model, int parentID, string orderNum, int craftPersonID, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常" && r.CraftPersonID == craftPersonID);
                    //if (tasks.Count() < 1)
                    //{
                    //    unfinishNum = 0;
                    //    return null;
                    //}
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
                        task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        task.lastAlterPersonName = "";
                        if (item.LastAlterPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.lastAlterPersonName = staff.StaffName;
                            }
                        }
                        task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }
                        task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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


                        {
                            task.toolingDrawingsUploadFlag = "无";
                            task.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == craftPersonID && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    task.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == craftPersonID && real.ProcessFileUploadIsComplete == 1)
                                {
                                    task.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == craftPersonID && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    task.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == craftPersonID && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    task.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }
                        
                       
                        task.toolingDrawingsUploadIsNeed = "无需工装";
                        var processesss = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        if (processesss.Count() < 1)
                        {
                            task.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processesss)
                        {
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                task.toolingDrawingsUploadIsNeed = "";
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    task.toolingDrawingsUploadCompleteTimeStr = "";
                                    task.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                task.processFileUploadCompleteTimeStr = "";
                                task.processFileUploadTimeStr = "";
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
                                task.taskStateStr = "任务未开始";
                                break;
                            case 1:
                                task.taskStateStr = "任务未开始";
                                break;
                            case 2:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 3:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 4:
                                task.taskStateStr = "工序处理中";
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
        /// 获取工艺责任人所有的任务
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="orderNum"></param>
        /// <param name="creaftPersonID"></param>
        /// <returns></returns>
        public static List<CompontInfo> GetTaskInfoByCreaftPersonCreate(DatabaseList model, int parentID, string orderNum, int craftPersonID)
        {
            try
            {
                List<CompontInfo> compontInfos = new List<CompontInfo>();
                // using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        CompontInfo compontInfo = new CompontInfo();
                        compontInfo.componentID = parentID;
                        compontInfo.taskName = item.ComponentName;
                        compontInfo.taskNum = item.ComponentNum;
                        compontInfo.allConponentNum = orderNum + item.ComponentNum;
                        compontInfo.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        compontInfo.creatPersonName = "";
                        var staffCreat = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                        if (staffCreat != null)
                        {
                            compontInfo.creatPersonName = staffCreat.StaffName;
                        }
                        compontInfo.staffID = compontInfo.creatPersonID;
                        compontInfo.staffName = compontInfo.creatPersonName;
                        compontInfo.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        compontInfo.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        compontInfo.id = item.ID;
                        compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        compontInfo.lastAlterPersonName = "";
                        var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        if (stafflastAlter != null)
                        {
                            compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        }
                        compontInfo.ispart = false;
                        compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));


                        {
                            compontInfo.toolingDrawingsUploadFlag = "无";
                            compontInfo.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == craftPersonID && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    compontInfo.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == craftPersonID && real.ProcessFileUploadIsComplete == 1)
                                {
                                    compontInfo.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == craftPersonID && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == craftPersonID && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }


                        {
                            compontInfo.acceptTaskTime = item.AcceptTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTaskTime);
                            compontInfo.acceptTaskTimeStr = item.AcceptTaskTime == null ? "" : item.AcceptTaskTime.ToString().Substring(0, item.AcceptTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.arrangeTaskTime = item.ArrangeTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.ArrangeTaskTime);
                            compontInfo.arrangeTaskTimeStr = item.ArrangeTaskTime == null ? "" : item.ArrangeTaskTime.ToString().Substring(0, item.ArrangeTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.completeTime = item.CompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.CompleteTime);
                            compontInfo.completeTimeStr = item.CompleteTime == null ? "" : item.CompleteTime.ToString().Substring(0, item.CompleteTime.ToString().LastIndexOf(':'));
                            compontInfo.craftPersonID = item.CraftPersonID == null ? 0 : Convert.ToInt32(item.CraftPersonID);
                            compontInfo.craftPersonName = "";
                            if (item.CraftPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CraftPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.craftPersonName = staff.StaffName;
                                    string str = compontInfo.craftPersonID.ToString();
                                    str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model, Convert.ToInt32(staff.PosiationID)) + "_");
                                    compontInfo.personAllId = str;
                                }
                            }
                            compontInfo.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                            compontInfo.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(":"));
                            compontInfo.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanCompletionTime);
                            compontInfo.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(item.PlanCompletionTime.ToString().LastIndexOf(':'));
                            compontInfo.previewCategory = new List<int>();
                            var types = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var type in types)
                            {
                                compontInfo.previewCategory.Add(Convert.ToInt32(type.CategoryTypeID));
                            }
                            compontInfo.previewCategoryStr = "";
                            foreach (var type in compontInfo.previewCategory)
                            {
                                var jd = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == type && r.state == "正常").FirstOrDefault();
                                if (jd != null)
                                {
                                    compontInfo.previewCategoryStr += jd.ProcessingType + ",";
                                }
                            }
                            if (compontInfo.previewCategoryStr.Length > 0)
                            {
                                compontInfo.previewCategoryStr = compontInfo.previewCategoryStr.Substring(0, compontInfo.previewCategoryStr.Length - 1);
                            }
                            compontInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            compontInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            compontInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            compontInfo.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                            compontInfo.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                            compontInfo.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            compontInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            compontInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            if (item.ProcessFileUploadTime == null)
                            {
                                DateTime time = new DateTime();
                                if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.ComponentName + "-" + item.ComponentNum + "-10", out time))
                                {
                                    compontInfo.processFileUploadTime = time;
                                    compontInfo.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                                }
                            }
                            compontInfo.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                            compontInfo.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                            compontInfo.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            compontInfo.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            compontInfo.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            compontInfo.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            compontInfo.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                            compontInfo.staffName = "";
                            if (item.staffID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.staffName = staff.StaffName;
                                }
                            }

                            compontInfo.toolingDrawingsUploadIsNeed = "无需工装";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            if (processes.Count() < 1)
                            {
                                compontInfo.toolingDrawingsUploadIsNeed = "";
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadIsNeed == 1)
                                {
                                    compontInfo.toolingDrawingsUploadIsNeed = "";
                                    if (real.ToolingDrawingsUploadIsComplete != 1)
                                    {
                                        compontInfo.toolingDrawingsUploadCompleteTimeStr = "";
                                        compontInfo.ToolingDrawingsUploadTimeStr = "";
                                    }

                                }
                                if (real.ProcessFileUploadIsComplete != 1)
                                {
                                    compontInfo.processFileUploadCompleteTimeStr = "";
                                    compontInfo.processFileUploadTimeStr = "";
                                }
                            }
                            if (compontInfo.toolingDrawingsUploadIsNeed == "无需工装")
                            {
                                compontInfo.ToolingDrawingsUploadTimeStr = "无需工装";
                            }
                            compontInfo.state = item.state;
                            compontInfo.taskState = item.TaskState == null ? 0 : Convert.ToInt32(item.TaskState);
                            compontInfo.version = item.Version == null ? 0 : Convert.ToInt32(item.Version);
                            switch (compontInfo.taskState)
                            {
                                case 0:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 1:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 2:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 3:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 4:
                                    compontInfo.taskStateStr = "工序处理中";
                                    break;
                                case 5:
                                    compontInfo.taskStateStr = "已完成";
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

                        }

                        compontInfo.children = CraftDesignInfoManage.GetTaskInfoByCreaftPersonCreate(model, item.ID, compontInfo.allConponentNum + ".", craftPersonID);
                        var unendFlag = true;
                        {
                            var count = compontInfo.children.Where(r => r.taskStateStr != "已完成").Count();
                            if (count > 0)
                            {
                                unendFlag = false;
                            }

                        }
                        if (compontInfo.children != null)
                        {
                            foreach (var real in compontInfo.children)
                            {
                                compontInfo.allTaskNum += real.allTaskNum;
                                compontInfo.unfinishedNum += real.unfinishedNum;
                            }
                        }
                        int unfinish = 0;
                        var list = CraftDesignInfoManage.GetPartTaskInfoByCreaftPersonCreate(model, item.ID, compontInfo.allConponentNum + ".", craftPersonID, out unfinish);

                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
                        if (compontInfo.children == null || compontInfo.children.Count() < 1)
                        {
                            if (item.CraftPersonID != craftPersonID)
                            {
                                continue;
                            }
                        }
                        if (!(compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum < 1 && (unendFlag)))
                        {
                            compontInfo.taskStateStr = "待完成";
                        }
                        compontInfos.Add(compontInfo);
                    }
                }

                compontInfos = compontInfos.OrderByDescending(r => r.creatTime).ToList();
                return compontInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据父节点获取工艺责任人零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoByCreaftPersonCreate(DatabaseList model, int parentID, string orderNum, int craftPersonID, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常" && r.CraftPersonID == craftPersonID);
                    //if (tasks.Count() < 1)
                    //{
                    //    unfinishNum = 0;
                    //    return null;
                    //}
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
                                str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model, Convert.ToInt32(staff.PosiationID)) + "_");
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
                        task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        task.lastAlterPersonName = "";
                        if (item.LastAlterPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.lastAlterPersonName = staff.StaffName;
                            }
                        }
                        task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }
                        task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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


                        {
                            task.toolingDrawingsUploadFlag = "无";
                            task.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == craftPersonID && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    task.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == craftPersonID && real.ProcessFileUploadIsComplete == 1)
                                {
                                    task.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == craftPersonID && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    task.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == craftPersonID && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    task.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }


                        task.toolingDrawingsUploadIsNeed = "无需工装";
                        var processesss = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        if (processesss.Count() < 1)
                        {
                            task.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processesss)
                        {
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                task.toolingDrawingsUploadIsNeed = "";
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    task.toolingDrawingsUploadCompleteTimeStr = "";
                                    task.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                task.processFileUploadCompleteTimeStr = "";
                                task.processFileUploadTimeStr = "";
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
                                task.taskStateStr = "任务未开始";
                                break;
                            case 1:
                                task.taskStateStr = "任务未开始";
                                break;
                            case 2:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 3:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 4:
                                task.taskStateStr = "工序处理中";
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
        /// 工艺责任人接收任务
        /// </summary>
        /// <param name="taskID">接收的任务ID</param>
        /// <param name="acceptPersonID">接收人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AcceptTask(int taskID, int acceptPersonID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = task.TaskName + "该任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != acceptPersonID)
                    {
                        errMsg = task.TaskName + "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    if (task.TaskState != 1)
                    {
                        errMsg = task.TaskName + "该任务不满足接收条件，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().AcceptTaskTime = DateTime.Now;
                            da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 2;
                            task.AcceptTaskTime = DateTime.Now;

                            task.TaskState = 2;
                            if (task.ProcessCardUploadTime != null && task.ProcessScheduleUpTime != null)
                            {
                                task.TaskState = 3;
                                da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 3;
                            }
                            task.LastAlterPersonID = acceptPersonID;
                            task.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                            {
                                AfterVersion = task.Version,
                                AlterDesc = "工艺责任人接收任务",
                                AlterTime = DateTime.Now,
                                CreatPersonID = acceptPersonID,
                                CreatTime = DateTime.Now,
                                staffID = acceptPersonID,
                                state = "正常",
                                TaskID = task.ID
                            };
                            model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = task.TaskName + "接收成功";
                            return true;

                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = task.TaskName + ex.Message;
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
        /// 创建工序
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="processNum"></param>
        /// <param name="createPersonID"></param>
        /// <param name="processTypeID"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CreateMachiningProcess(int taskID, int processNum,bool flag, int createPersonID, int processTypeID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该零件任务不存在";
                        return false;
                    }
                    if (task.CraftPersonID != createPersonID)
                    {
                        errMsg = "该零件任务责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskID && r.ProcessNum == processNum && r.state == "正常");
                    if (process.Count() > 0)
                    {
                        errMsg = "该零件任务工序" + processNum.ToString() + "已存在，请确认后再试！";
                        return false;
                    }
                    //for (int i = 1; i < processNum; i++)
                    //{
                    //    var processOld = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskID && r.ProcessNum == i && r.state == "正常");
                    //    if (processOld.Count() < 1)
                    //    {
                    //        errMsg = "该零件任务工序" + i.ToString() + "不存在，无法创建工序" + processNum.ToString() + "，请确认后再试！";

                    //        return false;
                    //    }
                    //}
                    int flagCom = 0;
                    int flagNeed = 1;
                    if (!flag)
                    {
                        flagCom = 1;
                        flagNeed = 0;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_Task_ProcessInfo_Table jd = new JDJS_PDMS_Task_ProcessInfo_Table()
                            {
                                CreatPersonID = createPersonID,
                                CreatTime = DateTime.Now,
                                LastAlterPersonID = createPersonID,
                                LastAlterTime = DateTime.Now,
                                ProcessNum = processNum,
                                ProcessingTypeID = processTypeID,
                                state = "正常",
                                TaskID = taskID,
                                ProcessFileUploadIsComplete =0,
                                ToolingDrawingsUploadIsComplete = flagCom,
                                ToolingDrawingsUploadIsNeed =flagNeed ,

                            };
                            model.JDJS_PDMS_Task_ProcessInfo_Table.Add(jd);
                            model.SaveChanges();
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Task_ProcessInfo_Table.Add(jd);
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
        /// 获取零件任务工序信息
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <returns>工序信息列表</returns>
        public static List<CraftInfo> GetPartsProcessInfo(int taskId)
        {
            try
            {
                List<CraftInfo> craftInfos = new List<CraftInfo>();
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        return null;
                    }
                    var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskId && r.state == "正常");
                    foreach (var item in processes)
                    {
                        CraftInfo craftInfo = new CraftInfo();
                        craftInfo.id = item.ID;
                        craftInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        craftInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        craftInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        craftInfo.otherFileUploadPersonID = item.OtherFileUploadPersonID == null ? 0 : Convert.ToInt32(item.OtherFileUploadPersonID);
                        craftInfo.otherFileUploadPersonName = "";
                        if (item.OtherFileUploadPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.OtherFileUploadPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                craftInfo.otherFileUploadPersonName = staff.StaffName;
                            }
                        }
                        craftInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        craftInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        craftInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        craftInfo.processFileUploadPersonID = item.ProcessFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ProcessFileUploadPersonID);
                        craftInfo.processFileUploadPersonName = "";
                        if (item.ProcessFileUploadPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ProcessFileUploadPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                craftInfo.processFileUploadPersonName = staff.StaffName;
                            }
                        }
                        craftInfo.processFileUploadIsComplete = item.ProcessFileUploadIsComplete == null ? false : (item.ProcessFileUploadIsComplete == 0 ? false : true);
                        craftInfo.processFileUploadCompleteTime = item.ProcessFileUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadCompleteTime);
                        craftInfo.processFileUploadCompleteTimeStr = item.ProcessFileUploadCompleteTime == null ? "" : item.ProcessFileUploadCompleteTime.ToString().Substring(0, item.ProcessFileUploadCompleteTime.ToString().LastIndexOf(':'));
                        craftInfo.toolingDrawingsUploadIsComplete = item.ToolingDrawingsUploadIsComplete == null ? false : (item.ToolingDrawingsUploadIsComplete == 0 ? false : true);
                        craftInfo.toolingDrawingsUploadCompleteTime = item.ToolingDrawingsUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadCompleteTime);
                        craftInfo.toolingDrawingsUploadCompleteTimeStr = item.ToolingDrawingsUploadCompleteTime == null ? "" : item.ToolingDrawingsUploadCompleteTime.ToString().Substring(0, item.ToolingDrawingsUploadCompleteTime.ToString().LastIndexOf(':'));
                        craftInfo.toolingDrawingsUploadIsNeed = item.ToolingDrawingsUploadIsNeed == null ? true : (item.ToolingDrawingsUploadIsNeed == 0 ? false : true);
                        craftInfo.processingTypeID = item.ProcessingTypeID;
                        craftInfo.processingTypeStr = "";
                        var processType = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                        if (processType != null)
                        {
                            craftInfo.processingTypeStr = processType.ProcessingType;
                        }

                        craftInfo.processNum = item.ProcessNum;
                        craftInfo.taskID = Convert.ToInt32(item.TaskID);
                        craftInfo.taskName = task.TaskName;
                        craftInfo.toolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        craftInfo.toolingDrawingsUploadPersonID = item.ToolingDrawingsUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolingDrawingsUploadPersonID);
                        craftInfo.toolingDrawingsUploadPersonName = "";
                        if (item.ToolingDrawingsUploadPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolingDrawingsUploadPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                craftInfo.toolingDrawingsUploadPersonName = staff.StaffName;
                            }
                        }
                        craftInfo.toolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        craftInfo.toolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        craftInfo.toolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                        craftInfo.toolTableFileUploadPersonID = item.ToolTableFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolTableFileUploadPersonID);
                        craftInfo.toolTableFileUploadPersonName = "";
                        if (item.ToolingDrawingsUploadPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolTableFileUploadPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                craftInfo.toolTableFileUploadPersonName = staff.StaffName;
                            }
                        }
                        craftInfo.toolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                        craftInfo.toolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                        craftInfos.Add(craftInfo);

                    }
                }
                craftInfos = craftInfos.OrderBy(r => r.processNum).ToList();
                return craftInfos;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// 获取零件任务工序信息
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <returns>工序信息列表</returns>
        public static List<CraftInfo> GetPartsProcessInfoByPerson(int taskId, int personId)
        {
            try
            {
                List<CraftInfo> craftInfos = new List<CraftInfo>();
                using (Model1 model = new Model1())
                {
                    int typeId = -1;
                    var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (person == null)
                    {
                        return null;
                    }
                    if (person.ProcessTypeID != null)
                    {
                        typeId = Convert.ToInt32(person.ProcessTypeID);
                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        return null;
                    }
                    if (typeId == -1)
                    {
                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            CraftInfo craftInfo = new CraftInfo();
                            craftInfo.id = item.ID;
                            craftInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            craftInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            craftInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            craftInfo.otherFileUploadPersonID = item.OtherFileUploadPersonID == null ? 0 : Convert.ToInt32(item.OtherFileUploadPersonID);
                            craftInfo.otherFileUploadPersonName = "";
                            if (item.OtherFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.OtherFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.otherFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            craftInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            craftInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.processFileUploadPersonID = item.ProcessFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ProcessFileUploadPersonID);
                            craftInfo.processFileUploadPersonName = "";
                            if (item.ProcessFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ProcessFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.processFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.processFileUploadIsComplete = item.ProcessFileUploadIsComplete == null ? false : (item.ProcessFileUploadIsComplete == 0 ? false : true);
                            craftInfo.processFileUploadCompleteTime = item.ProcessFileUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadCompleteTime);
                            craftInfo.processFileUploadCompleteTimeStr = item.ProcessFileUploadCompleteTime == null ? "" : item.ProcessFileUploadCompleteTime.ToString().Substring(0, item.ProcessFileUploadCompleteTime.ToString().LastIndexOf(':'));
                            craftInfo.toolingDrawingsUploadIsComplete = item.ToolingDrawingsUploadIsComplete == null ? false : (item.ToolingDrawingsUploadIsComplete == 0 ? false : true);
                            craftInfo.toolingDrawingsUploadIsNeed = item.ToolingDrawingsUploadIsNeed == null ? true : (item.ToolingDrawingsUploadIsNeed == 0 ? false : true);
                            craftInfo.toolingDrawingsUploadCompleteTime = item.ToolingDrawingsUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadCompleteTime);
                            craftInfo.toolingDrawingsUploadCompleteTimeStr = item.ToolingDrawingsUploadCompleteTime == null ? "" : item.ToolingDrawingsUploadCompleteTime.ToString().Substring(0, item.ToolingDrawingsUploadCompleteTime.ToString().LastIndexOf(':'));
                            craftInfo.processingTypeID = item.ProcessingTypeID;
                            craftInfo.processingTypeStr = "";
                            var processType = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.TaskID);
                            craftInfo.taskName = task.TaskName;
                            craftInfo.toolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            craftInfo.toolingDrawingsUploadPersonID = item.ToolingDrawingsUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolingDrawingsUploadPersonID);
                            craftInfo.toolingDrawingsUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolingDrawingsUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolingDrawingsUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            craftInfo.toolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.toolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            craftInfo.toolTableFileUploadPersonID = item.ToolTableFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolTableFileUploadPersonID);
                            craftInfo.toolTableFileUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolTableFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolTableFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            craftInfo.toolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfos.Add(craftInfo);

                        }
                    }
                    else
                    {
                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskId && r.ProcessingTypeID == typeId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            CraftInfo craftInfo = new CraftInfo();
                            craftInfo.id = item.ID;
                            craftInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            craftInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            craftInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            craftInfo.otherFileUploadPersonID = item.OtherFileUploadPersonID == null ? 0 : Convert.ToInt32(item.OtherFileUploadPersonID);
                            craftInfo.otherFileUploadPersonName = "";
                            if (item.OtherFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.OtherFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.otherFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            craftInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            craftInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.processFileUploadPersonID = item.ProcessFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ProcessFileUploadPersonID);
                            craftInfo.processFileUploadPersonName = "";
                            if (item.ProcessFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ProcessFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.processFileUploadPersonName = staff.StaffName;
                                }
                            }

                            craftInfo.processingTypeID = item.ProcessingTypeID;
                            craftInfo.processingTypeStr = "";
                            var processType = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.TaskID);
                            craftInfo.taskName = task.TaskName;
                            craftInfo.toolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            craftInfo.toolingDrawingsUploadPersonID = item.ToolingDrawingsUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolingDrawingsUploadPersonID);
                            craftInfo.toolingDrawingsUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolingDrawingsUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolingDrawingsUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            craftInfo.toolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.toolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            craftInfo.toolTableFileUploadPersonID = item.ToolTableFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolTableFileUploadPersonID);
                            craftInfo.toolTableFileUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolTableFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolTableFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            craftInfo.toolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfos.Add(craftInfo);

                        }
                    }
                }
                craftInfos = craftInfos.OrderBy(r => r.processNum).ToList();
                return craftInfos;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 获取零件任务工序信息
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <returns>工序信息列表</returns>
        public static List<CraftInfo> GetPartsProcessInfoByPersonGroup(int taskId, int personId)
        {
            try
            {
                List<CraftInfo> craftInfos = new List<CraftInfo>();
                using (Model1 model = new Model1())
                {
                    int typeId = -1;
                    var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (person == null)
                    {
                        return null;
                    }
                    if (person.ProcessTypeID != null)
                    {
                        typeId = Convert.ToInt32(person.ProcessTypeID);
                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        return null;
                    }
                    if (typeId == -1)
                    {
                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            var old = craftInfos.Where(r => r.processingTypeID == item.ProcessingTypeID).FirstOrDefault();
                            if (old != null)
                            {
                                continue;
                            }
                            CraftInfo craftInfo = new CraftInfo();
                            craftInfo.id = item.ID;
                            craftInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            craftInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            craftInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            craftInfo.otherFileUploadPersonID = item.OtherFileUploadPersonID == null ? 0 : Convert.ToInt32(item.OtherFileUploadPersonID);
                            craftInfo.otherFileUploadPersonName = "";
                            if (item.OtherFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.OtherFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.otherFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            craftInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            craftInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.processFileUploadPersonID = item.ProcessFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ProcessFileUploadPersonID);
                            craftInfo.processFileUploadPersonName = "";
                            if (item.ProcessFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ProcessFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.processFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.processFileUploadIsComplete = item.ProcessFileUploadIsComplete == null ? false : (item.ProcessFileUploadIsComplete == 0 ? false : true);
                            craftInfo.processFileUploadCompleteTime = item.ProcessFileUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadCompleteTime);
                            craftInfo.processFileUploadCompleteTimeStr = item.ProcessFileUploadCompleteTime == null ? "" : item.ProcessFileUploadCompleteTime.ToString().Substring(0, item.ProcessFileUploadCompleteTime.ToString().LastIndexOf(':'));
                            craftInfo.toolingDrawingsUploadIsComplete = item.ToolingDrawingsUploadIsComplete == null ? false : (item.ToolingDrawingsUploadIsComplete == 0 ? false : true);
                            craftInfo.toolingDrawingsUploadIsNeed = item.ToolingDrawingsUploadIsNeed == null ? true : (item.ToolingDrawingsUploadIsNeed == 0 ? false : true);
                            craftInfo.toolingDrawingsUploadCompleteTime = item.ToolingDrawingsUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadCompleteTime);
                            craftInfo.toolingDrawingsUploadCompleteTimeStr = item.ToolingDrawingsUploadCompleteTime == null ? "" : item.ToolingDrawingsUploadCompleteTime.ToString().Substring(0, item.ToolingDrawingsUploadCompleteTime.ToString().LastIndexOf(':'));
                            craftInfo.processingTypeID = item.ProcessingTypeID;
                            craftInfo.processingTypeStr = "";
                            var processType = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.TaskID);
                            craftInfo.taskName = task.TaskName;
                            craftInfo.toolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            craftInfo.toolingDrawingsUploadPersonID = item.ToolingDrawingsUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolingDrawingsUploadPersonID);
                            craftInfo.toolingDrawingsUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolingDrawingsUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolingDrawingsUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            craftInfo.toolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.toolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            craftInfo.toolTableFileUploadPersonID = item.ToolTableFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolTableFileUploadPersonID);
                            craftInfo.toolTableFileUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolTableFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolTableFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            craftInfo.toolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfos.Add(craftInfo);

                        }
                    }
                    else
                    {
                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskId && r.ProcessingTypeID == typeId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            var old = craftInfos.Where(r => r.processingTypeID == item.ProcessingTypeID).FirstOrDefault();
                            if (old != null)
                            {
                                continue;
                            }
                            CraftInfo craftInfo = new CraftInfo();
                            craftInfo.id = item.ID;
                            craftInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            craftInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            craftInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            craftInfo.otherFileUploadPersonID = item.OtherFileUploadPersonID == null ? 0 : Convert.ToInt32(item.OtherFileUploadPersonID);
                            craftInfo.otherFileUploadPersonName = "";
                            if (item.OtherFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.OtherFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.otherFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            craftInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            craftInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.processFileUploadPersonID = item.ProcessFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ProcessFileUploadPersonID);
                            craftInfo.processFileUploadPersonName = "";
                            if (item.ProcessFileUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ProcessFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.processFileUploadPersonName = staff.StaffName;
                                }
                            }

                            craftInfo.processingTypeID = item.ProcessingTypeID;
                            craftInfo.processingTypeStr = "";
                            var processType = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.TaskID);
                            craftInfo.taskName = task.TaskName;
                            craftInfo.toolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            craftInfo.toolingDrawingsUploadPersonID = item.ToolingDrawingsUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolingDrawingsUploadPersonID);
                            craftInfo.toolingDrawingsUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolingDrawingsUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolingDrawingsUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            craftInfo.toolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            craftInfo.toolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            craftInfo.toolTableFileUploadPersonID = item.ToolTableFileUploadPersonID == null ? 0 : Convert.ToInt32(item.ToolTableFileUploadPersonID);
                            craftInfo.toolTableFileUploadPersonName = "";
                            if (item.ToolingDrawingsUploadPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.ToolTableFileUploadPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    craftInfo.toolTableFileUploadPersonName = staff.StaffName;
                                }
                            }
                            craftInfo.toolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            craftInfo.toolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            craftInfos.Add(craftInfo);

                        }
                    }
                }
                craftInfos = craftInfos.OrderBy(r => r.processNum).ToList();
                return craftInfos;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// 删除零件任务的工序
        /// </summary>
        /// <param name="processId">工序主键ID</param>
        /// <param name="personId">删除人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeletePartsProcess(int processId, int personId, ref string errMsg)
        {
            try
            {
                int taskID = -1;
                string processNum = "";
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;

                    }
                    processNum = process.ProcessNum.ToString(); ;
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的零件任务不存在，请确认后再试！";
                        return false;
                    }
                    taskID = task.ID;
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = "该零件任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    //var processPuls = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == task.ID && r.state == "正常" && r.ProcessNum > process.ProcessNum);
                    //if (processPuls.Count() > 0)
                    //{
                    //    errMsg = "该零件工序存在下道工序，请依次删除！";
                    //    return false;
                    //}
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId).First().state = "删除";
                            process.state = "删除";
                            process.LastAlterPersonID = personId;
                            process.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_CraftInfo_Alter_History_Table()
                            {
                                AlterDesc = "删除工序",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                ProcessID = processId,
                                staffID = personId,
                                state = "正常"

                            };
                            model.JDJS_PDMS_CraftInfo_Alter_History_Table.Add(jd);
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

                }
                SupplementaryInformation pathInfo = new SupplementaryInformation();
                var pathTask = TaskInfoManage.GetTaskPath(taskID);
                var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, processNum);
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }

                return true;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改工序信息
        /// </summary>
        /// <param name="processId">欲修改的工序ID</param>
        /// <param name="personId">修改人员ID</param>
        /// <param name="processTypeId">加工类型 车 磨 镗</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterPartsProcess(int processId, bool flag,int personId, int processNum, int processTypeId, ref string errMsg)
        {
            try
            {
                int flagCom = 0;
                int flagNeed = 1;
                if (!flag)
                {
                    flagCom = 1;
                    flagNeed = 0;
                }
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;

                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的零件任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = "该零件任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    //if (process.ProcessingTypeID == processTypeId && process.ProcessNum == processNum)
                    //{
                    //    errMsg = "ok";
                    //    return true;
                    //}
                    else
                    {

                        int processIdOld = process.ProcessingTypeID;
                        var processTypeOld = "";
                        var processTypeNew = "";
                        var type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == processIdOld && r.state == "正常").FirstOrDefault();
                        if (type != null)
                        {
                            processTypeOld = type.ProcessingType;
                        }
                        type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == processTypeId && r.state == "正常").FirstOrDefault();
                        if (type != null)
                        {
                            processTypeNew = type.ProcessingType;
                        }



                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId).First().ToolingDrawingsUploadIsNeed = flagNeed;
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId).First().ToolingDrawingsUploadIsComplete = flagCom;
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId).First().ProcessingTypeID = processTypeId;
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId).First().ProcessNum = processNum;
                                process.ToolingDrawingsUploadIsNeed = flagNeed;
                                process.ToolingDrawingsUploadIsComplete = flagCom;
                                if (process.ProcessingTypeID != processTypeId)
                                {
                                    process.ProcessingTypeID = processTypeId;
                                    JDJS_PDMS_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_CraftInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改工序加工组别，由" + processTypeOld + "修改为" + processTypeNew,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = personId,
                                        CreatTime = DateTime.Now,
                                        ProcessID = processId,
                                        staffID = personId,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CraftInfo_Alter_History_Table.Add(jd);
                                }
                                if (process.ProcessNum != processNum)
                                {
                                    string processNumOld = "";
                                    processNumOld = process.ProcessNum.ToString();
                                    process.ProcessNum = processNum;
                                    JDJS_PDMS_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_CraftInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改工序号，由" + processNumOld + "修改为" + processNum.ToString(),
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = personId,
                                        CreatTime = DateTime.Now,
                                        ProcessID = processId,
                                        staffID = personId,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CraftInfo_Alter_History_Table.Add(jd);
                                    var pathStr = TaskInfoManage.GetTaskPath(task.ID);
                                    SupplementaryInformation sup = new SupplementaryInformation();
                                    if (Directory.Exists(Path.Combine(sup.upLoadPath(), pathStr, processNumOld)))
                                    {
                                        Directory.Move(Path.Combine(sup.upLoadPath(), pathStr, processNumOld), Path.Combine(sup.upLoadPath(), pathStr, processNum.ToString()));
                                    }
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
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 上传工艺进程单工艺过程卡或其它文件
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="personId">人员ID</param>
        /// <param name="fileFlag">文件标志位0：工艺进程卡 。1：工艺进程单。2：其它文件</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool UpdateTaskCraftFiles(int taskId, int personId, int fileFlag, ref string errMsg)
        {
            try
            {
                if (fileFlag == 0)
                {
                    using (Model1 model = new Model1())
                    {
                        var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                        if (task == null)
                        {
                            errMsg = "该任务不存在，请确认后再试！";
                            return false;
                        }
                        if (task.CraftPersonID != personId)
                        {
                            errMsg = "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                            return false;
                        }
                        if (task.TaskState == 0 || task.TaskState == 1)
                        {
                            errMsg = "请先接收该零件任务，再上传工艺文件！";
                            return false;
                        }
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().ProcessCardUploadTime = DateTime.Now;
                                task.ProcessCardUploadTime = DateTime.Now;
                                task.LastAlterPersonID = personId;
                                task.LastAlterTime = DateTime.Now;
                                JDJS_PDMS_TaskInfo_Alter_History_Table jdinfo = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId,
                                    AfterVersion = task.Version + 1
                                };

                                model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jdinfo);
                                task.TaskState = 3;
                                da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 3;
                                JDJS_PDMS_CraftFile_Alter_History_Table jd = new JDJS_PDMS_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId
                                };
                                model.JDJS_PDMS_CraftFile_Alter_History_Table.Add(jd);
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
                else if (fileFlag == 1)
                {
                    using (Model1 model = new Model1())
                    {
                        var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                        if (task == null)
                        {
                            errMsg = "该任务不存在，请确认后再试！";
                            return false;
                        }
                        if (task.CraftPersonID != personId)
                        {
                            errMsg = "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                            return false;
                        }
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().ProcessScheduleUpTime = DateTime.Now;
                                task.ProcessScheduleUpTime = DateTime.Now;
                                task.LastAlterPersonID = personId;
                                task.LastAlterTime = DateTime.Now;
                                JDJS_PDMS_TaskInfo_Alter_History_Table jdinfo = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId,
                                    AfterVersion = task.Version + 1
                                };
                                model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jdinfo);
                                //task.TaskState = 3;
                                JDJS_PDMS_CraftFile_Alter_History_Table jd = new JDJS_PDMS_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺进程单",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId
                                };
                                model.JDJS_PDMS_CraftFile_Alter_History_Table.Add(jd);
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
                else
                {
                    using (Model1 model = new Model1())
                    {
                        var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                        if (task == null)
                        {
                            errMsg = "该任务不存在，请确认后再试！";
                            return false;
                        }
                        if (task.CraftPersonID != personId)
                        {
                            errMsg = "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                            return false;
                        }
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                task.OtherFileTime = DateTime.Now;
                                task.LastAlterPersonID = personId;
                                task.LastAlterTime = DateTime.Now;
                                JDJS_PDMS_TaskInfo_Alter_History_Table jdinfo = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId,
                                    AfterVersion = task.Version + 1
                                };
                                model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jdinfo);
                                //task.TaskState = 3;
                                JDJS_PDMS_CraftFile_Alter_History_Table jd = new JDJS_PDMS_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = "上传其它文件",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId
                                };
                                model.JDJS_PDMS_CraftFile_Alter_History_Table.Add(jd);
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
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除工艺过程卡，工艺进程单，其它文件
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <param name="personId">人员ID</param>
        /// <param name="fileFlag">文件标志位0：过程卡。1：进程单 2：其它文件</param>
        /// <param name="dirPath">文件所在路径</param>
        /// <param name="fileNames">文件名</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteTaskCrafeFiles(int taskId, int personId, int fileFlag, string dirPath, List<string> fileNames, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId).FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }

                }
                foreach (var item in fileNames)
                {
                    if (File.Exists(Path.Combine(dirPath, item)))
                    {
                        File.Delete(Path.Combine(dirPath, item));
                    }
                }
                var listFile = Directory.GetFiles(dirPath);
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                            string alterDesc = "";
                            switch (fileFlag)
                            {
                                case 0:
                                    alterDesc = "删除工艺过程卡";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        task.ProcessCardUploadTime = null;
                                        task.TaskState = 2;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                    }
                                    break;
                                case 1:
                                    alterDesc = "删除工艺进程单";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        task.ProcessScheduleUpTime = null;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                    }
                                    break;
                                case 2:
                                    alterDesc = "删除其它文件";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        task.OtherFileTime = null;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            foreach (var item in fileNames)
                            {
                                JDJS_PDMS_CraftFile_Alter_History_Table jd = new JDJS_PDMS_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = alterDesc + item,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId
                                };
                                model.JDJS_PDMS_CraftFile_Alter_History_Table.Add(jd);
                                JDJS_PDMS_TaskInfo_Alter_History_Table jds = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                {
                                    AlterDesc = alterDesc + item,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    TaskID = taskId,
                                    AfterVersion = task.Version + 1
                                };
                                model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jds);
                            }

                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = "文件已删除，但出现未知异常:" + ex.Message;
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
        /// 工艺过程卡提交
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <param name="personId">人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool ProcessSubmitByCraftPerson(int taskId, int personId, ref string errMsg)
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
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = task.TaskName + "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    if (task.ProcessCardUploadTime == null)
                    {
                        errMsg = task.TaskName + "请先上传工艺过程卡！";
                        return false;
                    }
                    if (task.TaskState != 3)
                    {
                        errMsg = task.TaskName + "该任务不满足工艺过程卡提交条件！";
                        return false;
                    }
                    var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskId && r.state == "正常");
                    if (processes.Count() < 1)
                    {
                        errMsg = task.TaskName + "请先创建加工工序！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 4;
                            task.TaskState = 4;
                            task.LastAlterPersonID = personId;
                            task.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                            {
                                AfterVersion = task.Version + 1,
                                AlterDesc = "工艺过程卡提交",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                TaskID = taskId

                            };
                            model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = task.TaskName + "提交完成";
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
        /// 获取工艺过程卡提交后的任务信息
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="orderNum"></param>
        /// <param name="creaftPersonID"></param>
        /// <returns></returns>
        public static List<CompontInfo> GetTaskInfoByGroup(DatabaseList model, int parentID, string orderNum, int personId)
        {
            try
            {
                List<CompontInfo> compontInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        CompontInfo compontInfo = new CompontInfo();
                        compontInfo.componentID = parentID;
                        compontInfo.taskName = item.ComponentName;
                        compontInfo.taskNum = item.ComponentNum;
                        compontInfo.allConponentNum = orderNum + item.ComponentNum;
                        compontInfo.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        compontInfo.creatPersonName = "";
                        var staffCreat = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                        if (staffCreat != null)
                        {
                            compontInfo.creatPersonName = staffCreat.StaffName;
                        }
                        compontInfo.staffID = compontInfo.creatPersonID;
                        compontInfo.staffName = compontInfo.creatPersonName;
                        compontInfo.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        compontInfo.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        compontInfo.id = item.ID;
                        compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        compontInfo.lastAlterPersonName = "";
                        var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        if (stafflastAlter != null)
                        {
                            compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        }
                        compontInfo.ispart = false;
                        compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        {
                            compontInfo.toolingDrawingsUploadFlag = "无";
                            compontInfo.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == personId && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    compontInfo.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == personId  && real.ProcessFileUploadIsComplete == 1)
                                {
                                    compontInfo.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == personId && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == personId && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }

                        {
                            compontInfo.acceptTaskTime = item.AcceptTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTaskTime);
                            compontInfo.acceptTaskTimeStr = item.AcceptTaskTime == null ? "" : item.AcceptTaskTime.ToString().Substring(0, item.AcceptTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.arrangeTaskTime = item.ArrangeTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.ArrangeTaskTime);
                            compontInfo.arrangeTaskTimeStr = item.ArrangeTaskTime == null ? "" : item.ArrangeTaskTime.ToString().Substring(0, item.ArrangeTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.completeTime = item.CompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.CompleteTime);
                            compontInfo.completeTimeStr = item.CompleteTime == null ? "" : item.CompleteTime.ToString().Substring(0, item.CompleteTime.ToString().LastIndexOf(':'));
                            compontInfo.craftPersonID = item.CraftPersonID == null ? 0 : Convert.ToInt32(item.CraftPersonID);
                            compontInfo.craftPersonName = "";
                            if (item.CraftPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CraftPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.craftPersonName = staff.StaffName;
                                    string str = compontInfo.craftPersonID.ToString();
                                    str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model,Convert.ToInt32(staff.PosiationID)) + "_");
                                    compontInfo.personAllId = str;
                                }
                            }
                            compontInfo.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                            compontInfo.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(":"));
                            compontInfo.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanCompletionTime);
                            compontInfo.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(item.PlanCompletionTime.ToString().LastIndexOf(':'));
                            compontInfo.previewCategory = new List<int>();
                            var types = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var type in types)
                            {
                                compontInfo.previewCategory.Add(Convert.ToInt32(type.CategoryTypeID));
                            }
                            compontInfo.previewCategoryStr = "";
                            foreach (var type in compontInfo.previewCategory)
                            {
                                var jd = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == type && r.state == "正常").FirstOrDefault();
                                if (jd != null)
                                {
                                    compontInfo.previewCategoryStr += jd.ProcessingType + ",";
                                }
                            }
                            if (compontInfo.previewCategoryStr.Length > 0)
                            {
                                compontInfo.previewCategoryStr = compontInfo.previewCategoryStr.Substring(0, compontInfo.previewCategoryStr.Length - 1);
                            }
                            compontInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            compontInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            compontInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            compontInfo.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                            compontInfo.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                            compontInfo.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            compontInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            compontInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            //if (item.ProcessFileUploadTime == null)
                            //{
                            //    DateTime time = new DateTime();
                            //    if (GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            //    {
                            //        task.processFileUploadTime = time;
                            //        task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            //    }
                            //}
                            compontInfo.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                            compontInfo.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                            compontInfo.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            compontInfo.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            compontInfo.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            compontInfo.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            compontInfo.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                            compontInfo.staffName = "";
                            if (item.staffID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.staffName = staff.StaffName;
                                }
                            }
                            
                            
                            compontInfo.toolingDrawingsUploadIsNeed = "无需工装";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            if (processes.Count() < 1)
                            {
                                compontInfo.toolingDrawingsUploadIsNeed = "";
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadIsNeed == 1)
                                {
                                    compontInfo.toolingDrawingsUploadIsNeed = "";
                                    if (real.ToolingDrawingsUploadIsComplete != 1)
                                    {
                                        compontInfo.toolingDrawingsUploadCompleteTimeStr = "";
                                        compontInfo.ToolingDrawingsUploadTimeStr = "";
                                    }

                                }
                                if (real.ProcessFileUploadIsComplete != 1)
                                {
                                    compontInfo.processFileUploadCompleteTimeStr = "";
                                    compontInfo.processFileUploadTimeStr = "";
                                }
                            }
                            if (compontInfo.toolingDrawingsUploadIsNeed == "无需工装")
                            {
                                compontInfo.ToolingDrawingsUploadTimeStr = "无需工装";
                            }
                            compontInfo.state = item.state;
                            compontInfo.taskState = item.TaskState == null ? 0 : Convert.ToInt32(item.TaskState);
                            compontInfo.version = item.Version == null ? 0 : Convert.ToInt32(item.Version);
                            switch (compontInfo.taskState)
                            {
                                case 0:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 1:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 2:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 3:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 4:
                                    compontInfo.taskStateStr = "工序处理中";
                                    break;
                                case 5:
                                    compontInfo.taskStateStr = "已完成";
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

                        }

                        compontInfo.children = CraftDesignInfoManage.GetTaskInfoByGroup(model,item.ID, compontInfo.allConponentNum + ".", personId);
                        var unendFlag = true;
                        {
                            var count = compontInfo.children.Where(r => r.taskStateStr != "已完成").Count();
                            if (count > 0)
                            {
                                unendFlag = false;
                            }

                        }
                        if (compontInfo.children != null)
                        {
                            foreach (var real in compontInfo.children)
                            {
                                compontInfo.allTaskNum += real.allTaskNum;
                                compontInfo.unfinishedNum += real.unfinishedNum;
                            }
                        }
                        int unfinish = 0;
                        var list = CraftDesignInfoManage.GetPartTaskInfoByGroup(model,item.ID, compontInfo.allConponentNum + ".", personId, out unfinish);

                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
                        bool isOut = true;
                        var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId).FirstOrDefault();
                        if (compontInfo.children == null || compontInfo.children.Count() < 1)
                        {
                            if (item.CraftPersonID != personId)
                            {
                                var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID);
                                if (process.Count() > 0)
                                {
                                    foreach (var real in process)
                                    {
                                        if (person.CompontProcessTypeID == real.ProcessingTypeID&&item.TaskState !=5)
                                        {
                                            isOut = false;
                                        }
                                        //if (real.ToolingDrawingsUploadPersonID == personId)
                                        //{
                                        //    isOut = false;
                                        //}
                                    }
                                }
                                if (isOut)
                                {
                                    continue;
                                }
                            }
                            
                        }
                        if (!(compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum < 1 && (unendFlag)))
                        {
                            compontInfo.taskStateStr = "待完成";
                        }
                        compontInfos.Add(compontInfo);
                    }
                }
                compontInfos = compontInfos.OrderByDescending(r => r.unfinishedNum).ThenBy(r => r.planCompletionTime).ToList();
                return compontInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoByGroup(DatabaseList model, int parentID, string orderNum, int personId, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常" && (r.TaskState == 4 || r.CraftPersonID == personId));
                    //if (tasks.Count() < 1)
                    //{
                    //    unfinishNum = 0;
                    //    return null;
                    //}
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
                        task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        task.lastAlterPersonName = "";
                        if (item.LastAlterPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.lastAlterPersonName = staff.StaffName;
                            }
                        }
                        task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }
                        task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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
                        {
                            task.toolingDrawingsUploadFlag = "无";
                            task.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID ==personId && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    task.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == personId  && real.ProcessFileUploadIsComplete == 1)
                                {
                                    task.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == personId && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    task.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == personId && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    task.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }

                        
                       
                        task.toolingDrawingsUploadIsNeed = "无需工装";
                        var processesss = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        bool isOut = true;
                        if (item.CraftPersonID == personId)
                        {
                            isOut = false;
                        }
                        var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId).FirstOrDefault();
                        if (processesss.Count() < 1)
                        {
                            task.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processesss)
                        {
                            if (real.ProcessingTypeID == person.ProcessTypeID)
                            {
                                isOut = false;
                            }
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                task.toolingDrawingsUploadIsNeed = "";
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    task.toolingDrawingsUploadCompleteTimeStr = "";
                                    task.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                task.processFileUploadCompleteTimeStr = "";
                                task.processFileUploadTimeStr = "";
                            }
                        }
                        if (task.toolingDrawingsUploadIsNeed == "无需工装")
                        {
                            task.ToolingDrawingsUploadTimeStr = "无需工装";
                        }


                        if (isOut)
                        {
                            continue;
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
                                task.taskStateStr = "任务未开始";
                                break;
                            case 1:
                                task.taskStateStr = "任务未开始";
                                break;
                            case 2:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 3:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 4:
                                task.taskStateStr = "工序处理中";
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
        /// 获取工艺过程卡提交后的任务信息
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="orderNum"></param>
        /// <param name="creaftPersonID"></param>
        /// <returns></returns>
        public static List<CompontInfo> GetTaskInfoByGroupCreate(DatabaseList model, int parentID, string orderNum, int personId)
        {
            try
            {
                List<CompontInfo> compontInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        CompontInfo compontInfo = new CompontInfo();
                        compontInfo.componentID = parentID;
                        compontInfo.taskName = item.ComponentName;
                        compontInfo.taskNum = item.ComponentNum;
                        compontInfo.allConponentNum = orderNum + item.ComponentNum;
                        compontInfo.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        compontInfo.creatPersonName = "";
                        var staffCreat = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                        if (staffCreat != null)
                        {
                            compontInfo.creatPersonName = staffCreat.StaffName;
                        }
                        compontInfo.staffID = compontInfo.creatPersonID;
                        compontInfo.staffName = compontInfo.creatPersonName;
                        compontInfo.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        compontInfo.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        compontInfo.id = item.ID;
                        compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        compontInfo.lastAlterPersonName = "";
                        var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        if (stafflastAlter != null)
                        {
                            compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        }
                        compontInfo.ispart = false;
                        compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        {
                            compontInfo.toolingDrawingsUploadFlag = "无";
                            compontInfo.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == personId && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    compontInfo.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == personId && real.ProcessFileUploadIsComplete == 1)
                                {
                                    compontInfo.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == personId && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == personId && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    compontInfo.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }

                        {
                            compontInfo.acceptTaskTime = item.AcceptTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTaskTime);
                            compontInfo.acceptTaskTimeStr = item.AcceptTaskTime == null ? "" : item.AcceptTaskTime.ToString().Substring(0, item.AcceptTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.arrangeTaskTime = item.ArrangeTaskTime == null ? DateTime.Now : Convert.ToDateTime(item.ArrangeTaskTime);
                            compontInfo.arrangeTaskTimeStr = item.ArrangeTaskTime == null ? "" : item.ArrangeTaskTime.ToString().Substring(0, item.ArrangeTaskTime.ToString().LastIndexOf(':'));
                            compontInfo.completeTime = item.CompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.CompleteTime);
                            compontInfo.completeTimeStr = item.CompleteTime == null ? "" : item.CompleteTime.ToString().Substring(0, item.CompleteTime.ToString().LastIndexOf(':'));
                            compontInfo.craftPersonID = item.CraftPersonID == null ? 0 : Convert.ToInt32(item.CraftPersonID);
                            compontInfo.craftPersonName = "";
                            if (item.CraftPersonID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CraftPersonID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.craftPersonName = staff.StaffName;
                                    string str = compontInfo.craftPersonID.ToString();
                                    str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model, Convert.ToInt32(staff.PosiationID)) + "_");
                                    compontInfo.personAllId = str;
                                }
                            }
                            compontInfo.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                            compontInfo.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(":"));
                            compontInfo.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanCompletionTime);
                            compontInfo.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(item.PlanCompletionTime.ToString().LastIndexOf(':'));
                            compontInfo.previewCategory = new List<int>();
                            var types = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            foreach (var type in types)
                            {
                                compontInfo.previewCategory.Add(Convert.ToInt32(type.CategoryTypeID));
                            }
                            compontInfo.previewCategoryStr = "";
                            foreach (var type in compontInfo.previewCategory)
                            {
                                var jd = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == type && r.state == "正常").FirstOrDefault();
                                if (jd != null)
                                {
                                    compontInfo.previewCategoryStr += jd.ProcessingType + ",";
                                }
                            }
                            if (compontInfo.previewCategoryStr.Length > 0)
                            {
                                compontInfo.previewCategoryStr = compontInfo.previewCategoryStr.Substring(0, compontInfo.previewCategoryStr.Length - 1);
                            }
                            compontInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            compontInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            compontInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            compontInfo.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                            compontInfo.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                            compontInfo.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                            compontInfo.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                            compontInfo.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                            //if (item.ProcessFileUploadTime == null)
                            //{
                            //    DateTime time = new DateTime();
                            //    if (GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            //    {
                            //        task.processFileUploadTime = time;
                            //        task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            //    }
                            //}
                            compontInfo.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                            compontInfo.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                            compontInfo.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            compontInfo.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            compontInfo.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            compontInfo.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            compontInfo.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            compontInfo.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                            compontInfo.staffName = "";
                            if (item.staffID != null)
                            {
                                var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                                if (staff != null)
                                {
                                    compontInfo.staffName = staff.StaffName;
                                }
                            }


                            compontInfo.toolingDrawingsUploadIsNeed = "无需工装";
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                            if (processes.Count() < 1)
                            {
                                compontInfo.toolingDrawingsUploadIsNeed = "";
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadIsNeed == 1)
                                {
                                    compontInfo.toolingDrawingsUploadIsNeed = "";
                                    if (real.ToolingDrawingsUploadIsComplete != 1)
                                    {
                                        compontInfo.toolingDrawingsUploadCompleteTimeStr = "";
                                        compontInfo.ToolingDrawingsUploadTimeStr = "";
                                    }

                                }
                                if (real.ProcessFileUploadIsComplete != 1)
                                {
                                    compontInfo.processFileUploadCompleteTimeStr = "";
                                    compontInfo.processFileUploadTimeStr = "";
                                }
                            }
                            if (compontInfo.toolingDrawingsUploadIsNeed == "无需工装")
                            {
                                compontInfo.ToolingDrawingsUploadTimeStr = "无需工装";
                            }
                            compontInfo.state = item.state;
                            compontInfo.taskState = item.TaskState == null ? 0 : Convert.ToInt32(item.TaskState);
                            compontInfo.version = item.Version == null ? 0 : Convert.ToInt32(item.Version);
                            switch (compontInfo.taskState)
                            {
                                case 0:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 1:
                                    compontInfo.taskStateStr = "任务未开始";
                                    break;
                                case 2:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 3:
                                    compontInfo.taskStateStr = "工艺未提交";
                                    break;
                                case 4:
                                    compontInfo.taskStateStr = "工序处理中";
                                    break;
                                case 5:
                                    compontInfo.taskStateStr = "已完成";
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

                        }

                        compontInfo.children = CraftDesignInfoManage.GetTaskInfoByGroupCreate(model, item.ID, compontInfo.allConponentNum + ".", personId);
                        var unendFlag = true;
                        {
                            var count = compontInfo.children.Where(r => r.taskStateStr != "已完成").Count();
                            if (count > 0)
                            {
                                unendFlag = false;
                            }

                        }
                        if (compontInfo.children != null)
                        {
                            foreach (var real in compontInfo.children)
                            {
                                compontInfo.allTaskNum += real.allTaskNum;
                                compontInfo.unfinishedNum += real.unfinishedNum;
                            }
                        }
                        int unfinish = 0;
                        var list = CraftDesignInfoManage.GetPartTaskInfoByGroupCreate(model, item.ID, compontInfo.allConponentNum + ".", personId, out unfinish);

                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
                        bool isOut = true;
                        var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId).FirstOrDefault();
                        if (compontInfo.children == null || compontInfo.children.Count() < 1)
                        {
                            if (item.CraftPersonID != personId)
                            {
                                var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID);
                                if (process.Count() > 0)
                                {
                                    foreach (var real in process)
                                    {
                                        if (person.CompontProcessTypeID == real.ProcessingTypeID && item.TaskState != 5)
                                        {
                                            isOut = false;
                                        }
                                        //if (real.ToolingDrawingsUploadPersonID == personId)
                                        //{
                                        //    isOut = false;
                                        //}
                                    }
                                }
                                if (isOut)
                                {
                                    continue;
                                }
                            }

                        }
                        if (!(compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum < 1 && (unendFlag)))
                        {
                            compontInfo.taskStateStr = "待完成";
                        }
                        compontInfos.Add(compontInfo);
                    }
                }
                compontInfos = compontInfos.OrderByDescending(r => r.creatTime).ToList();
                return compontInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoByGroupCreate(DatabaseList model, int parentID, string orderNum, int personId, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常" && (r.TaskState == 4 || r.CraftPersonID == personId));
                    //if (tasks.Count() < 1)
                    //{
                    //    unfinishNum = 0;
                    //    return null;
                    //}
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
                                str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model, Convert.ToInt32(staff.PosiationID)) + "_");
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
                        task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        task.lastAlterPersonName = "";
                        if (item.LastAlterPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.lastAlterPersonName = staff.StaffName;
                            }
                        }
                        task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }
                        task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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
                        {
                            task.toolingDrawingsUploadFlag = "无";
                            task.processFileUploadFlag = "无";
                            var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                            foreach (var real in processes)
                            {
                                if (real.ProcessFileUploadPersonID == personId && (real.ProcessFileUploadIsComplete == 0 || real.ProcessFileUploadIsComplete == null))
                                {
                                    task.processFileUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ProcessFileUploadPersonID == personId && real.ProcessFileUploadIsComplete == 1)
                                {
                                    task.processFileUploadFlag = "已完成";
                                }
                            }
                            foreach (var real in processes)
                            {
                                if (real.ToolingDrawingsUploadPersonID == personId && (real.ToolingDrawingsUploadIsComplete == 0 || real.ToolingDrawingsUploadIsComplete == null))
                                {
                                    task.toolingDrawingsUploadFlag = "未完成";
                                    break;
                                }
                                else if (real.ToolingDrawingsUploadPersonID == personId && real.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    task.toolingDrawingsUploadFlag = "已完成";
                                }
                            }

                        }



                        task.toolingDrawingsUploadIsNeed = "无需工装";
                        var processesss = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == item.ID && r.state == "正常");
                        bool isOut = true;
                        if (item.CraftPersonID == personId)
                        {
                            isOut = false;
                        }
                        var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId).FirstOrDefault();
                        if (processesss.Count() < 1)
                        {
                            task.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processesss)
                        {
                            if (real.ProcessingTypeID == person.ProcessTypeID)
                            {
                                isOut = false;
                            }
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                task.toolingDrawingsUploadIsNeed = "";
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    task.toolingDrawingsUploadCompleteTimeStr = "";
                                    task.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                task.processFileUploadCompleteTimeStr = "";
                                task.processFileUploadTimeStr = "";
                            }
                        }
                        if (task.toolingDrawingsUploadIsNeed == "无需工装")
                        {
                            task.ToolingDrawingsUploadTimeStr = "无需工装";
                        }


                        if (isOut)
                        {
                            continue;
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
                                task.taskStateStr = "任务未开始";
                                break;
                            case 1:
                                task.taskStateStr = "任务未开始";
                                break;
                            case 2:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 3:
                                task.taskStateStr = "工艺未提交";
                                break;
                            case 4:
                                task.taskStateStr = "工序处理中";
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
                taskInfos = taskInfos.OrderByDescending (r=>r.creatTime).ToList();
                return taskInfos;
            }
            catch (Exception ex)
            {
                unfinishNum = 0;
                return null;
            }

        }

        /// <summary>
        /// 预览任务
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="orderNum"></param>
        /// <param name="creaftPersonID"></param>
        /// <returns></returns>
        public static List<CompontInfo> GetTaskInfoByPreview_Category(DatabaseList model, int parentID, string orderNum, int personId)
        {
            try
            {
                List<CompontInfo> compontInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        CompontInfo compontInfo = new CompontInfo();
                        compontInfo.componentID = parentID;
                        compontInfo.taskName = item.ComponentName;
                        compontInfo.taskNum = item.ComponentNum;
                        compontInfo.allConponentNum = orderNum + item.ComponentNum;
                        compontInfo.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        compontInfo.creatPersonName = "";
                        var staffCreat = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                        if (staffCreat != null)
                        {
                            compontInfo.creatPersonName = staffCreat.StaffName;
                        }
                        compontInfo.staffID = compontInfo.creatPersonID;
                        compontInfo.staffName = compontInfo.creatPersonName;
                        compontInfo.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        compontInfo.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        compontInfo.id = item.ID;
                        compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        compontInfo.lastAlterPersonName = "";
                        var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        if (stafflastAlter != null)
                        {
                            compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        }
                        compontInfo.ispart = false;
                        compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));

                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.ComponentName  + "-" + item.ComponentNum + "-10", out time))
                            {
                                compontInfo .processFileUploadTime = time;
                                compontInfo.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }
                       
                        
                        compontInfo.toolingDrawingsUploadIsNeed = "无需工装";
                        var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                        if (processes.Count() < 1)
                        {
                            compontInfo.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processes)
                        {
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                compontInfo.toolingDrawingsUploadIsNeed = "";
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    compontInfo.toolingDrawingsUploadCompleteTimeStr = "";
                                    compontInfo.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                compontInfo.processFileUploadCompleteTimeStr = "";
                                compontInfo.processFileUploadTimeStr = "";
                            }
                        }
                        if (compontInfo.toolingDrawingsUploadIsNeed == "无需工装")
                        {
                            compontInfo.ToolingDrawingsUploadTimeStr = "无需工装";
                        }
                        compontInfo.children = CraftDesignInfoManage.GetTaskInfoByPreview_Category(model,item.ID, compontInfo.allConponentNum + ".", personId);

                        if (compontInfo.children != null)
                        {
                            foreach (var real in compontInfo.children)
                            {
                                compontInfo.allTaskNum += real.allTaskNum;
                                compontInfo.unfinishedNum += real.unfinishedNum;
                            }
                        }
                        int unfinish = 0;
                        var list = CraftDesignInfoManage.GetPartTaskInfoByPreview_Category(model, item.ID, compontInfo.allConponentNum + ".", personId, out unfinish);

                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
                        if (compontInfo.children == null || compontInfo.children.Count() < 1)
                        {
                            continue;
                        }
                        if (compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum > 0)
                        {
                            compontInfo.taskStateStr = "待完成";
                        }
                        compontInfos.Add(compontInfo);
                    }
                }
                compontInfos = compontInfos.OrderByDescending(r => r.unfinishedNum).ThenBy(r => r.planCompletionTime).ToList();
                return compontInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 预览根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoByPreview_Category(DatabaseList model, int parentID, string orderNum, int personId, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常" && (r.TaskState == 0 || r.TaskState == 1 || r.TaskState == 2 || r.TaskState == 3));
                    //if (tasks.Count() < 1)
                    //{
                    //    unfinishNum = 0;
                    //    return null;
                    //}
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
                        task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        task.lastAlterPersonName = "";
                        if (item.LastAlterPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.lastAlterPersonName = staff.StaffName;
                            }
                        }
                        task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }
                        
                        task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    task.toolingDrawingsUploadCompleteTimeStr = "";
                                    task.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                task.processFileUploadCompleteTimeStr = "";
                                task.processFileUploadTimeStr = "";
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
                        if (types.Count() < 1)
                        {
                            continue;
                        }
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
                        int typeid = -1;
                        var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                        if (person != null)
                        {
                            typeid = person.ProcessTypeID == null ? -1 : Convert.ToInt32(person.ProcessTypeID);
                        }
                        if (!task.previewCategory.Contains(typeid))
                        {
                            continue;
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
        /// 预览任务
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="orderNum"></param>
        /// <param name="creaftPersonID"></param>
        /// <returns></returns>
        public static List<CompontInfo> GetTaskInfoByPreview_CategoryCreate(DatabaseList model, int parentID, string orderNum, int personId)
        {
            try
            {
                List<CompontInfo> compontInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        CompontInfo compontInfo = new CompontInfo();
                        compontInfo.componentID = parentID;
                        compontInfo.taskName = item.ComponentName;
                        compontInfo.taskNum = item.ComponentNum;
                        compontInfo.allConponentNum = orderNum + item.ComponentNum;
                        compontInfo.creatPersonID = item.CreatPersonID == null ? 0 : Convert.ToInt32(item.CreatPersonID);
                        compontInfo.creatPersonName = "";
                        var staffCreat = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.CreatPersonID && r.state == "正常").FirstOrDefault();
                        if (staffCreat != null)
                        {
                            compontInfo.creatPersonName = staffCreat.StaffName;
                        }
                        compontInfo.staffID = compontInfo.creatPersonID;
                        compontInfo.staffName = compontInfo.creatPersonName;
                        compontInfo.creatTime = item.CreatTime == null ? DateTime.Now : Convert.ToDateTime(item.CreatTime);
                        compontInfo.creatTimeStr = item.CreatTime == null ? "" : item.CreatTime.ToString().Substring(0, item.CreatTime.ToString().LastIndexOf(':'));
                        compontInfo.id = item.ID;
                        compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        compontInfo.lastAlterPersonName = "";
                        var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        if (stafflastAlter != null)
                        {
                            compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        }
                        compontInfo.ispart = false;
                        compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));

                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.ComponentName + "-" + item.ComponentNum + "-10", out time))
                            {
                                compontInfo.processFileUploadTime = time;
                                compontInfo.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }


                        compontInfo.toolingDrawingsUploadIsNeed = "无需工装";
                        var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == item.ID && r.state == "正常");
                        if (processes.Count() < 1)
                        {
                            compontInfo.toolingDrawingsUploadIsNeed = "";
                        }
                        foreach (var real in processes)
                        {
                            if (real.ToolingDrawingsUploadIsNeed == 1)
                            {
                                compontInfo.toolingDrawingsUploadIsNeed = "";
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    compontInfo.toolingDrawingsUploadCompleteTimeStr = "";
                                    compontInfo.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                compontInfo.processFileUploadCompleteTimeStr = "";
                                compontInfo.processFileUploadTimeStr = "";
                            }
                        }
                        if (compontInfo.toolingDrawingsUploadIsNeed == "无需工装")
                        {
                            compontInfo.ToolingDrawingsUploadTimeStr = "无需工装";
                        }
                        compontInfo.children = CraftDesignInfoManage.GetTaskInfoByPreview_CategoryCreate(model, item.ID, compontInfo.allConponentNum + ".", personId);

                        if (compontInfo.children != null)
                        {
                            foreach (var real in compontInfo.children)
                            {
                                compontInfo.allTaskNum += real.allTaskNum;
                                compontInfo.unfinishedNum += real.unfinishedNum;
                            }
                        }
                        int unfinish = 0;
                        var list = CraftDesignInfoManage.GetPartTaskInfoByPreview_CategoryCreate(model, item.ID, compontInfo.allConponentNum + ".", personId, out unfinish);

                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
                        if (compontInfo.children == null || compontInfo.children.Count() < 1)
                        {
                            continue;
                        }
                        if (compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum > 0)
                        {
                            compontInfo.taskStateStr = "待完成";
                        }
                        compontInfos.Add(compontInfo);
                    }
                }
                compontInfos = compontInfos.OrderByDescending(r => r.creatTime).ToList();
                return compontInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 预览根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static List<CompontInfo> GetPartTaskInfoByPreview_CategoryCreate(DatabaseList model, int parentID, string orderNum, int personId, out int unfinishNum)
        {
            try
            {
                int unfinish = 0;
                List<CompontInfo> taskInfos = new List<CompontInfo>();
                //using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常" && (r.TaskState == 0 || r.TaskState == 1 || r.TaskState == 2 || r.TaskState == 3));
                    //if (tasks.Count() < 1)
                    //{
                    //    unfinishNum = 0;
                    //    return null;
                    //}
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
                                str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffIDOnPositionCreate(model, Convert.ToInt32(staff.PosiationID)) + "_");
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
                        task.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        task.lastAlterPersonName = "";
                        if (item.LastAlterPersonID != null)
                        {
                            var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                            if (staff != null)
                            {
                                task.lastAlterPersonName = staff.StaffName;
                            }
                        }
                        task.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        task.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        task.allConponentNum = orderNum + item.TaskNum;
                        task.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                        task.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                        task.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                        task.planCompletionTime = item.PlanCompletionTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanCompletionTime);
                        task.planCompletionTimeStr = item.PlanCompletionTime == null ? "" : item.PlanCompletionTime.ToString().Substring(0, item.PlanCompletionTime.ToString().LastIndexOf(':'));
                        task.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        task.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        task.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        task.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        task.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                        task.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                        task.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                        task.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
                        task.processFileUploadTime = item.ProcessFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessFileUploadTime);
                        task.processFileUploadTimeStr = item.ProcessFileUploadTime == null ? "" : item.ProcessFileUploadTime.ToString().Substring(0, item.ProcessFileUploadTime.ToString().LastIndexOf(':'));
                        if (item.ProcessFileUploadTime == null)
                        {
                            DateTime time = new DateTime();
                            if (TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(item.TaskName + "-" + item.TaskNum + "-10", out time))
                            {
                                task.processFileUploadTime = time;
                                task.processFileUploadTimeStr = time == null ? "" : time.ToString().Substring(0, time.ToString().LastIndexOf(':'));
                            }
                        }

                        task.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                        task.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                        task.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                        task.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                        task.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                        task.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                        task.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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
                                if (real.ToolingDrawingsUploadIsComplete != 1)
                                {
                                    task.toolingDrawingsUploadCompleteTimeStr = "";
                                    task.ToolingDrawingsUploadTimeStr = "";
                                }

                            }
                            if (real.ProcessFileUploadIsComplete != 1)
                            {
                                task.processFileUploadCompleteTimeStr = "";
                                task.processFileUploadTimeStr = "";
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
                        if (types.Count() < 1)
                        {
                            continue;
                        }
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
                        int typeid = -1;
                        var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                        if (person != null)
                        {
                            typeid = person.ProcessTypeID == null ? -1 : Convert.ToInt32(person.ProcessTypeID);
                        }
                        if (!task.previewCategory.Contains(typeid))
                        {
                            continue;
                        }


                        taskInfos.Add(task);


                    }
                }
                unfinishNum = unfinish;
                taskInfos = taskInfos.OrderByDescending (r=>r.creatTime).ToList();
                return taskInfos;
            }
            catch (Exception ex)
            {
                unfinishNum = 0;
                return null;
            }

        }


        /// <summary>
        /// 根据工序主键Id获取文件所在位置
        /// </summary>
        /// <param name="processId">工序主键Id</param>
        /// <param name="fileFlag">文件类型标志  0：工装图纸  1：加工程序  2：其它文件  3 刀具表</param>
        /// <returns></returns>
        public static string GetFilePathByProcessId(int processId, int fileFlag)
        {
            try
            {
                string str = "";
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        return "";
                    }
                    int taskId = Convert.ToInt32(process.TaskID);
                    var path = TaskInfoManage.GetTaskPath(taskId);
                    string filePathCase = "";
                    switch (fileFlag)
                    {
                        case 0:
                            filePathCase = "工装图纸";
                            break;
                        case 1:
                            filePathCase = "加工程序";
                            break;
                        case 2:
                            filePathCase = "其它文件";
                            break;
                        case 3:
                            filePathCase = "刀具表";
                            break;
                        default:
                            return "";
                    }

                    str = Path.Combine(path, process.ProcessNum.ToString(), filePathCase);
                    return str;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        /// <summary>
        /// 上传工序文件
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="fileFlag">文件类型  0：工装图纸 1：加工程序  2：其它文件   3：刀具表</param>
        /// <param name="personId">上传人员id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool UploadProcessFile(int processId, int fileFlag, int personId, ref string errMsg)
        {
            try
            {
                string processNum = "";
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;

                    }
                    var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (person == null)
                    {
                        errMsg = "当前登录用户不存在，请确认后再试！";
                        return false;
                    }
                    if (personId != process.ToolingDrawingsUploadPersonID)
                    {
                        errMsg = "当前登录用户与该工序责任人不匹配，请确认后再试";
                        return false;
                    }
                    if (person.ProcessTypeID != process.ProcessingTypeID)
                    {
                        errMsg = "当前登录用户与该工序所需的加工人员类型不匹配，请确认后再试";
                        return false;
                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的任务不存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            string str = "";
                            switch (fileFlag)
                            {
                                case 0://工装图纸
                                    //process.ToolingDrawingsUploadPersonID = personId;
                                    process.ToolingDrawingsUploadTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    
                                    task.LastAlterPersonID = personId;
                                    task.LastAlterTime = DateTime.Now;
                                    //task.ToolingDrawingsUploadTime = DateTime.Now;
                                    str = "上传工装图纸";
                                    break;
                                case 1://加工程序
                                    //process.ProcessFileUploadPersonID = personId;
                                    //process.ProcessFileUploadTime = DateTime.Now;
                                    //task.ProcessFileUploadTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    task.LastAlterPersonID = personId;
                                    task.LastAlterTime = DateTime.Now;
                                    str = "上传加工程序";
                                    break;
                                case 2://其它文件
                                    process.OtherFileUploadPersonID = personId;
                                    process.OtherFileTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    str = "上传其它文件";
                                    break;//刀具表
                                case 3:
                                   // process.ToolTableFileUploadPersonID = personId;
                                    process.ToolTableFileUploadTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    task.LastAlterPersonID = personId;
                                    task.LastAlterTime = DateTime.Now;
                                    task.ToolTableFileUploadTime = DateTime.Now;
                                    str = "上传刀具表";
                                    break;
                                default://

                                    break;
                            }
                            if (str != "")
                            {
                                JDJS_PDMS_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_CraftInfo_Alter_History_Table()
                                {
                                    AlterDesc = str,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    ProcessID = processId,
                                    staffID = personId,
                                    state = "正常"
                                };
                                model.JDJS_PDMS_CraftInfo_Alter_History_Table.Add(jd);
                                model.SaveChanges();
                                mytran.Commit();
                                errMsg = "ok";
                                return true;
                            }
                            else
                            {
                                mytran.Rollback();
                                errMsg = "请输入正确的文件类型！";
                                return false;
                            }
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
        /// 删除工序文件
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <param name="personId">人员ID</param>
        /// <param name="fileFlag">文件标志位0：过程卡。1：进程单 2：其它文件</param>
        /// <param name="dirPath">文件所在路径</param>
        /// <param name="fileNames">文件名</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteProcessFiles(int processId, int personId, int fileFlag, string dirPath, List<string> fileNames, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId).FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;
                    }
                    var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (person == null)
                    {
                        errMsg = "当前登录用户不存在，请确认后再试！";
                        return false;
                    }
                    if (process.ToolingDrawingsUploadPersonID != personId)
                    {
                        errMsg = "该用户没有删除文件权限";
                        return false;
                    }
                    if (person.ProcessTypeID != process.ProcessingTypeID)
                    {
                        errMsg = "当前登录用户与该工序所需的加工人员类型不匹配，请确认后再试";
                        return false;
                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的任务不存在，请确认后再试！";
                        return false;
                    }

                }
                foreach (var item in fileNames)
                {
                    if (File.Exists(Path.Combine(dirPath, item)))
                    {
                        File.Delete(Path.Combine(dirPath, item));
                    }
                }
                var listFile = Directory.GetFiles(dirPath);
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                            var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID && r.state == "正常").FirstOrDefault();
                            if (process.ToolingDrawingsUploadPersonID != personId)
                            {
                                errMsg = "该用户没有删除文件权限";
                                return false;
                            }
                            string alterDesc = "";
                            switch (fileFlag)
                            {
                                case 0:
                                    alterDesc = "删除工装图纸";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        //process.ToolingDrawingsUploadPersonID = null;
                                        //process.ToolingDrawingsUploadTime = null;
                                        //task.ToolingDrawingsUploadTime = null;
                                        process.LastAlterTime = DateTime.Now;
                                        process.LastAlterPersonID = personId;

                                        task.LastAlterPersonID = personId;
                                        task.LastAlterTime = DateTime.Now;
                                    }
                                    break;
                                case 1:
                                    alterDesc = "删除加工程序";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        //process.ProcessFileUploadPersonID = null;
                                        //process.ProcessFileUploadTime = null;
                                       // task.ProcessFileUploadTime = null;
                                        process.LastAlterTime = DateTime.Now;
                                        process.LastAlterPersonID = personId;

                                        task.LastAlterPersonID = personId;
                                        task.LastAlterTime = DateTime.Now;
                                    }
                                    break;
                                case 2:
                                    alterDesc = "删除其它文件";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        //process.OtherFileUploadPersonID = null;
                                       // process.OtherFileTime = null;

                                        process.LastAlterTime = DateTime.Now;
                                        process.LastAlterPersonID = personId;
                                    }
                                    break;
                                case 3:
                                    alterDesc = "删除刀具表";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        //process.ToolTableFileUploadPersonID = null;
                                        //process.ToolTableFileUploadTime = null;

                                        process.LastAlterTime = DateTime.Now;
                                        process.LastAlterPersonID = personId;

                                        //task.ToolTableFileUploadTime = null;


                                        task.LastAlterPersonID = personId;
                                        task.LastAlterTime = DateTime.Now;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            foreach (var item in fileNames)
                            {
                                JDJS_PDMS_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_CraftInfo_Alter_History_Table()
                                {
                                    AlterDesc = alterDesc + item,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    ProcessID = processId,
                                    staffID = personId,
                                    state = "正常"

                                };
                                model.JDJS_PDMS_CraftInfo_Alter_History_Table.Add(jd);
                            }

                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = "文件已删除，但出现未知异常:" + ex.Message;
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
        /// 提交零件任务完成
        /// </summary>
        /// <param name="taskID">零件任务Id</param>
        /// <param name="personId">人员id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool CompleteTaskSubmit(int taskID, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = task.TaskName + "该任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = task.TaskName + "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    if (task.ProcessCardUploadTime == null || task.ProcessScheduleUpTime == null /*|| task.ToolingDrawingsUploadTime == null||task.ProcessFileUploadTime ==null*/)
                    {

                        errMsg = task.TaskName + "该任务工艺文件未上传，请确认后再试！";
                        return false;

                    }
                    //if (task.ProcessFileUploadTime == null)
                    //{
                    //    DateTime time = new DateTime();
                    //    if (!TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(task.TaskName + "-" + task.TaskNum + "-10", out time))
                    //    {
                    //        errMsg = task.TaskName + "该任务文件未上传，请确认后再试！";
                    //        return false;
                    //    }
                    //}
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskID && r.state == "正常");
                    foreach (var item in process)
                    {
                        string num = item.ProcessNum.ToString();
                        while (num.Length < 2)
                        {
                            num += "0";
                        }
                        string str = task.TaskName + "-" + task.TaskNum + "-" + num;
                        DateTime time = new DateTime();
                        if (item.ToolingDrawingsUploadIsComplete != 1 || item.ProcessFileUploadIsComplete != 1)
                        {
                            errMsg = task.TaskName + "该任务工序文件未上传，请确认后再试！";
                            return false;
                        }
                        //if (!TaskInfoManage.GetIFDatabaseInfoAboutPercessFile(str, out time))
                        //{
                        //    errMsg = task.TaskName + "该任务文件未上传，请确认后再试！";
                        //    return false;
                        //}
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = DateTime.Now;
                            da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 5;
                            task.CompleteTime = DateTime.Now;
                            task.TaskState = 5;
                            task.LastAlterTime = DateTime.Now;
                            task.LastAlterPersonID = personId;
                            JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                            {
                                AfterVersion = task.Version + 1,
                                AlterDesc = "任务完成提交",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                TaskID = taskID

                            };
                            model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = task.TaskName + "提交完成";
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
        /// 根据工序Id获取所有可接收该工序的员工
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static List<StaffInfo> GetStaffInfoByProcessWorkType(int processId)
        {
            List<StaffInfo> staffInfos = new List<StaffInfo>();
            try
            {
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process != null)
                    {
                        int typeId = process.ProcessingTypeID;
                        var staffs = model.JDJS_PDMS_Staff_Table.Where(r => r.ProcessTypeID == typeId && r.state == "正常");
                        foreach (var item in staffs)
                        {
                            int staffID = item.ID;
                            StaffInfo staffInfo = new StaffInfo();


                            staffInfo.id = item.ID;
                            staffInfo.mailbox = item.MailBox;
                            staffInfo.processTypeId = item.ProcessTypeID == null ? 0 : Convert.ToInt32(item.ProcessTypeID);
                            staffInfo.processTypeStr = "";
                            if (item.ProcessTypeID != null)
                            {
                                var type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == item.ProcessTypeID && r.state == "正常").FirstOrDefault();
                                if (type != null)
                                {
                                    staffInfo.processTypeStr = type.ProcessingType;
                                }
                            }
                            staffInfo.passWord = item.PassWord;
                            staffInfo.staff = item.StaffName;
                            staffInfo.tel = item.Tel;
                            staffInfo.users = item.Users;
                            staffInfos.Add(staffInfo);
                        }
                    }
                }
                return staffInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 为工序安排负责人
        /// </summary>
        /// <param name="processId">工序主键</param>
        /// <param name="personId">当前登录用户主键</param>
        /// <param name="staffId">安排员工主键</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool ArrangeProcessPerson(int processId, int personId, int staffId, ref string errMsg)
        {
            try
            {
                string receiveMail = "";
                string type = "";
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试";
                        return false;
                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID).FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的零件任务不存在，请确认后再试";
                        return false;
                    }
                    var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (person == null)
                    {
                        errMsg = "当前登录用户不存在，请确认后再试";
                        return false;
                    }
                    if (person.ProcessTypeID != null && person.ProcessTypeID != process.ProcessingTypeID)
                    {
                        errMsg = "当前登录用户无此权限，请确认后再试";
                        return false;
                    }
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId && r.state == "正常").FirstOrDefault();
                    if (staff.ProcessTypeID != process.ProcessingTypeID)
                    {
                        errMsg = "该员工无此组别权限，请确认后再试";
                        return false;
                    }
                    type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == staff.ProcessTypeID).FirstOrDefault().ProcessingType;
                    
                    receiveMail = staff.MailBox == null ? "" : staff.MailBox; 
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == process.TaskID && r.state == "正常" && r.ProcessingTypeID == process.ProcessingTypeID);
                            foreach (var item in processes)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == item.ID).First().OtherFileUploadPersonID = staffId;
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == item.ID).First().ProcessFileUploadPersonID = staffId;
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == item.ID).First().ToolingDrawingsUploadPersonID = staffId;
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == item.ID).First().ToolTableFileUploadPersonID = staffId;
                                item.OtherFileUploadPersonID = staffId;
                                item.ProcessFileUploadPersonID = staffId;
                                item.ToolingDrawingsUploadPersonID = staffId;
                                item.ToolTableFileUploadPersonID = staffId;
                            }
                            process.OtherFileUploadPersonID = staffId;
                            process.ProcessFileUploadPersonID = staffId;
                            process.ToolingDrawingsUploadPersonID = staffId;
                            process.ToolTableFileUploadPersonID = staffId;
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            string err = "";
                            string content = "安排新任务：" + task.TaskName + " " + task.TaskNum + " " + type + "工工序，请访问工艺文件管理系统处理！";
                            MailSendMethod.SendMail(receiveMail, content, "安排新任务通知", new List<string>(),ref err);
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
        /// 改变工序文件完成状态
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="personId">人员Id</param>
        /// <param name="fileFlag">文件标志  0：工装图纸   1：加工文件</param>
        /// <param name="isComplete">是否完成</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool ChangeProcessCompleteStatus(int processId, int personId, int fileFlag, bool isComplete, ref string errMsg)
        {
            try {
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;
                    }
                    var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == process.TaskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的任务不存在，请确认后再试！";
                        return false;
                    }
                    if (personId != process.ToolingDrawingsUploadPersonID)
                    {
                        errMsg = "当前登录用户无权限，请确认后再试！";
                        return false;
                    }
                    int flag = isComplete ? 1 : 0;
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try {
                            if (fileFlag == 0)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadIsComplete = flag;
                                process.ToolingDrawingsUploadIsComplete = flag;
                                if (isComplete)
                                {
                                    bool isCom = true;
                                    da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadCompleteTime = DateTime.Now;
                                    da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().ToolingDrawingsUploadTime = DateTime.Now;
                                    process.ToolingDrawingsUploadCompleteTime = DateTime.Now;
                                    process.ToolingDrawingsUploadTime = DateTime.Now;
                                    task.ToolingDrawingsUploadTime = DateTime.Now;
                                    var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == process.TaskID && r.ID != process.ID && r.state == "正常");
                                    foreach (var item in processes)
                                    {
                                        if (item.ProcessFileUploadIsComplete != 1)
                                        {
                                            isCom = false;
                                        }
                                        if (item.ToolingDrawingsUploadIsComplete != 1)
                                        {
                                            isCom = false;
                                        }
                                    }
                                    if (process.ProcessFileUploadIsComplete != 1)
                                    {
                                        isCom = false;
                                    }
                                    if (task.ProcessCardUploadTime == null)
                                    {
                                        isCom = false;
                                    }
                                    if (isCom)
                                    {
                                        
                                        da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 5;
                                        da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = DateTime.Now;
                                        task.TaskState = 5;
                                        task.CompleteTime = DateTime.Now;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                        JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                        {
                                            AfterVersion = task.Version + 1,
                                            AlterDesc = "任务完成提交",
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = personId,
                                            CreatTime = DateTime.Now,
                                            staffID = personId,
                                            state = "正常",
                                            TaskID = task.ID

                                        };
                                        model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    }
                                }
                                else
                                {
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 4;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = null;
                                    task.TaskState = 4;
                                    task.CompleteTime = null;
                                    da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadCompleteTime = null;
                                    da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadTime = null;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().ToolingDrawingsUploadTime =null;
                                    process.ToolingDrawingsUploadCompleteTime = null;
                                    task.ToolingDrawingsUploadTime = null;
                                }
                            }
                            else if (fileFlag == 1)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadIsComplete = flag;
                                process.ProcessFileUploadIsComplete = flag;
                                if (isComplete)
                                {
                                    bool isCom = true;

                                    da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadCompleteTime = DateTime.Now;
                                    //da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().ProcessFileUploadTime = DateTime.Now;
                                    process.ProcessFileUploadCompleteTime = DateTime.Now;
                                    task.ProcessFileUploadTime = DateTime.Now;
                                    var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == process.TaskID && r.ID != process.ID && r.state == "正常");
                                    foreach (var item in processes)
                                    {
                                        if (item.ProcessFileUploadIsComplete != 1)
                                        {
                                            isCom = false;
                                        }
                                        if (item.ToolingDrawingsUploadIsComplete != 1)
                                        {
                                            isCom = false;
                                        }
                                    }
                                    if (process.ToolingDrawingsUploadIsComplete != 1)
                                    {
                                        isCom = false;
                                    }
                                    if (task.ProcessCardUploadTime == null)
                                    {
                                        isCom = false;
                                    }
                                    if (isCom)
                                    {
                                        
                                        da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 5;
                                        da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = DateTime.Now;
                                        task.TaskState = 5;
                                        task.CompleteTime = DateTime.Now;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                        JDJS_PDMS_TaskInfo_Alter_History_Table jd = new JDJS_PDMS_TaskInfo_Alter_History_Table()
                                        {
                                            AfterVersion = task.Version + 1,
                                            AlterDesc = "任务完成提交",
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = personId,
                                            CreatTime = DateTime.Now,
                                            staffID = personId,
                                            state = "正常",
                                            TaskID = task.ID

                                        };
                                        model.JDJS_PDMS_TaskInfo_Alter_History_Table.Add(jd);
                                    }
                                }
                                else
                                {
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 4;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = null;
                                    da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadCompleteTime = null;
                                    //da.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == task.ID).First().ProcessFileUploadTime = null;
                                    task.TaskState = 4;
                                    task.CompleteTime = null;
                                    process.ProcessFileUploadCompleteTime = null;
                                    task.ProcessFileUploadTime = null;
                                }
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
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }


        }
        /// <summary>
        /// 解析工序文件，批量创建工序
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="personId">人员Id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool ReadProcessFile(string path,int taskId,int personId,ref string errMsg)
        {
            try
            {
                //加载Word文档
                Document doc = new Document();
                doc.LoadFromFile(path);

                //使用GetText方法获取文档中的所有文本

                //获取第一个节
                Section section = doc.Sections[0];

                //获取第一个表格
                Table table = section.Tables[0] as Table;
                //StringBuilder sb = new StringBuilder();
                Dictionary<int, List<string>> arrayStr = new Dictionary<int, List<string>>();
                int k = 0;
                int j = 0;
                //遍历表格中的段落并提取文本
                foreach (TableRow row in table.Rows)
                {
                    j = 0;
                    arrayStr.Add(k, new List<string>());
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Paragraph paragraph in cell.Paragraphs)
                        {
                            arrayStr[k].Add(paragraph.Text);
                            //sb.AppendLine(paragraph.Text);
                        }
                        j++;
                    }
                    k++;
                }
                List<ProcessInfoFromFile> infos = new List<ProcessInfoFromFile>();

                bool isOk = false;
                int flag = 0;
                foreach (var item in arrayStr)
                {
                    if (isOk)
                    {
                        if (item.Value.Count() > 0)
                        {
                            if (item.Value[0].Trim() != "")
                            {

                                int processNum = 0;
                                while (arrayStr.ContainsKey(flag + processNum) && (arrayStr[flag + processNum][0] == item.Value[0] || arrayStr[flag + processNum][0] == ""))
                                {
                                    if (arrayStr[flag + processNum].Count() >= 3 && arrayStr[flag + processNum][2].StartsWith("工序"))
                                    {
                                        string numStr = arrayStr[flag + processNum][2];
                                        int index = numStr.IndexOf("：");
                                        numStr = numStr.Substring(2, index - 2);


                                        ProcessInfoFromFile info = new ProcessInfoFromFile();
                                        info.processNum = Convert.ToInt32(item.Value[0] + numStr);
                                        info.processType = item.Value[1];
                                        info.taskId = taskId;
                                        info.createPersonId = personId;
                                        int addNum = 1;
                                        info.flag = false;
                                        if (item.Value.Count() >= 3 && item.Value[2].StartsWith("工装："))
                                        {
                                            info.flag = true;
                                        }
                                        else
                                        {
                                            while (arrayStr.ContainsKey(flag + processNum + addNum) && arrayStr[flag + processNum + addNum][0].Trim() == "")
                                            {
                                                if (arrayStr[flag + processNum + addNum][2].Trim().StartsWith("工序"))
                                                {
                                                    break;
                                                }
                                                foreach (var real in arrayStr[flag + processNum + addNum])
                                                {
                                                    if (real.StartsWith("工装："))
                                                    {
                                                        info.flag = true;
                                                        break;
                                                    }
                                                }
                                                addNum++;
                                            }
                                        }
                                        infos.Add(info);
                                    }
                                    else if (!arrayStr[flag + processNum][2].StartsWith("工装："))
                                    {

                                        ProcessInfoFromFile info = new ProcessInfoFromFile();
                                        info.processNum = Convert.ToInt32(item.Value[0]);
                                        info.processType = item.Value[1];
                                        info.taskId = taskId;
                                        info.createPersonId = personId;
                                        int addNum = 1;
                                        info.flag = false;
                                        if (item.Value.Count() >= 3 && item.Value[2].StartsWith("工装："))
                                        {
                                            info.flag = true;
                                        }
                                        else
                                        {
                                            while (arrayStr.ContainsKey(flag + addNum) && arrayStr[flag + addNum][0].Trim() == "")
                                            {
                                                foreach (var real in arrayStr[flag + addNum])
                                                {
                                                    if (real.StartsWith("工装："))
                                                    {
                                                        info.flag = true;
                                                    }
                                                }
                                                addNum++;
                                            }
                                        }
                                        infos.Add(info);
                                    }
                                    processNum++;
                                }




                            }
                        }
                    }
                    else
                    {
                        if (item.Value[0] == "工序号")
                        {
                            isOk = true;
                        }
                    }
                    flag++;
                }
                doc.Close();
                doc.Dispose();
                string err = "";
                if (CreateMachiningProcessBatch(infos, ref err))
                {
                    
                    errMsg = "ok";
                    return true;
                }
                else
                {
                    errMsg = err;
                    return false;
                }

                
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 批量创建工序
        /// </summary>
        /// <param name="infos">工序信息</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool CreateMachiningProcessBatch(List<ProcessInfoFromFile > infos, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in infos)
                            {
                                int taskID = item.taskId;
                                int createPersonID = item.createPersonId;
                                int processNum = item.processNum;
                                bool flag = item.flag;
                                int processTypeID = 0;
                                var type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ProcessingType == item.processType && r.state == "正常").FirstOrDefault();
                                if (type == null)
                                {
                                    continue;
                                }
                                processTypeID = type.ID;
                                var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                                if (task == null)
                                {
                                    errMsg = "该零件任务不存在";
                                    return false;
                                }
                                if (task.CraftPersonID != createPersonID)
                                {
                                    errMsg = "该零件任务责任人与当前登录用户不匹配，请确认后再试！";
                                    return false;
                                }
                                var process = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskID && r.ProcessNum == processNum && r.state == "正常");
                                if (process.Count() > 0)
                                {
                                    errMsg = "该零件任务工序" + processNum.ToString() + "已存在，请确认后再试！";
                                    return false;
                                }
                                //for (int i = 1; i < processNum; i++)
                                //{
                                //    var processOld = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == taskID && r.ProcessNum == i && r.state == "正常");
                                //    if (processOld.Count() < 1)
                                //    {
                                //        errMsg = "该零件任务工序" + i.ToString() + "不存在，无法创建工序" + processNum.ToString() + "，请确认后再试！";

                                //        return false;
                                //    }
                                //}
                                int flagCom = 0;
                                int flagNeed = 1;
                                if (!flag)
                                {
                                    flagCom = 1;
                                    flagNeed = 0;
                                }
                                //using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                                {
                                    {
                                        JDJS_PDMS_Task_ProcessInfo_Table jd = new JDJS_PDMS_Task_ProcessInfo_Table()
                                        {
                                            CreatPersonID = createPersonID,
                                            CreatTime = DateTime.Now,
                                            LastAlterPersonID = createPersonID,
                                            LastAlterTime = DateTime.Now,
                                            ProcessNum = processNum,
                                            ProcessingTypeID = processTypeID,
                                            state = "正常",
                                            TaskID = taskID,
                                            ProcessFileUploadIsComplete = 0,
                                            ToolingDrawingsUploadIsComplete = flagCom,
                                            ToolingDrawingsUploadIsNeed = flagNeed,

                                        };
                                        model.JDJS_PDMS_Task_ProcessInfo_Table.Add(jd);
                                        model.SaveChanges();
                                        DatabaseList da = DatabaseList.GetData();
                                        da.JDJS_PDMS_Task_ProcessInfo_Table.Add(jd);
                                    }
                                    
                                }
                            }
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
        /// 根据人员获取弹窗内容
        /// </summary>
        /// <param name="personID">人员主键ID</param>
        /// <returns></returns>
        public static string GetPopUpWindowsContentByLoginPerson(int personId)
        {
            string message = "";
            List<string> strList = new List<string>();
            try
            {

                using (Model1 model = new Model1())
                {
                    var perosn = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (personId != null)
                    {
                        var taskGroup = perosn.ProcessTypeID;
                        var comGroup = perosn.CompontProcessTypeID;

                        

                        var processesCompant = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.state == "正常" && (r.ToolingDrawingsUploadPersonID == null && r.ProcessFileUploadPersonID == null));
                        foreach (var item in processesCompant)
                        {
                            if (item.ProcessingTypeID == comGroup)
                            {
                                var compant = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == item.CompontID && r.state == "正常").FirstOrDefault();
                                if (compant != null&&compant.TaskState ==4)
                                {
                                    string str = "组件任务：" + compant.ComponentNum + " 需要工艺安排。<br/>";
                                    if (!strList.Contains(str))
                                    {
                                        strList.Add(str);
                                    }
                                }
                            }
                        }

                        var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.state == "正常" && (r.ToolingDrawingsUploadPersonID == null && r.ProcessFileUploadPersonID == null));
                        foreach (var item in processes)
                        {
                            if (item.ProcessingTypeID == taskGroup)
                            {
                                var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == item.TaskID && r.state == "正常").FirstOrDefault();
                                if (task != null&&task.TaskState ==4)
                                {
                                    string str = "零件任务：" + task.TaskNum + " 需要工艺安排。<br/>";
                                    if (!strList.Contains(str))
                                    {
                                        strList.Add(str);
                                    }
                                }
                            }
                        }
                    }

                }
                foreach (var item in strList)
                {
                    message += item;
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }

    public class ProcessInfoFromFile
    {
        //int taskID, int processNum,bool flag, int createPersonID, int processTypeID
        public int taskId;
        public int processNum;
        /// <summary>
        /// 是否需要工装
        /// </summary>
        public bool flag;
        public int createPersonId;
        public int processTypeId;
        public string processType;
    }

    public class CraftInfo
    {
        public int id { get; set; }
        public int taskID { get; set; }
        public string taskName { get; set; }
        public int processNum { get; set; }
        public int processingTypeID { get; set; }
        public string processingTypeStr { get; set; }

        /// <summary>
        /// 刀具表文件
        /// </summary>
        public DateTime toolTableFileUploadTime { get; set; }
        public string toolTableFileUploadTimeStr { get; set; }
        public string toolTableFileUploadPath { get; set; }
        public int toolTableFileUploadPersonID { get; set; }
        public string toolTableFileUploadPersonName { get; set; }

        /// <summary>
        /// 工装图纸文件
        /// </summary>
        public DateTime toolingDrawingsUploadTime { get; set; }
        public string toolingDrawingsUploadTimeStr { get; set; }
        public string toolingDrawingsUploadPath { get; set; }
        public int toolingDrawingsUploadPersonID { get; set; }
        public string toolingDrawingsUploadPersonName { get; set; }
        public bool toolingDrawingsUploadIsComplete { get; set; }
        public bool toolingDrawingsUploadIsNeed { get; set; }
        public DateTime toolingDrawingsUploadCompleteTime { get; set; }
        public string toolingDrawingsUploadCompleteTimeStr { get; set; }
        /// <summary>
        /// 加工文件
        /// </summary>
        public DateTime processFileUploadTime { get; set; }
        public string processFileUploadTimeStr { get; set; }
        public string processFileUploadPath { get; set; }
        public int processFileUploadPersonID { get; set; }
        public string processFileUploadPersonName { get; set; }
        public bool processFileUploadIsComplete { get; set; }
        public DateTime processFileUploadCompleteTime { get; set; }
        public string processFileUploadCompleteTimeStr { get; set; }

        /// <summary>
        /// 其它文件
        /// </summary>
        public DateTime otherFileTime { get; set; }
        public string otherFileTimeStr { get; set; }
        public string otherFilePath { get; set; }
        public int otherFileUploadPersonID { get; set; }
        public string otherFileUploadPersonName { get; set; }
    }


    public class FileInfoClass
    {
        public int taskId;
        public int processId;
        public int processNum;
        public int processPersonId;
        public string processPersonName;
        public string fileName;
        public string updateTimeStr;
        public DateTime updateTime;
        public string downloadPath;
    }
}