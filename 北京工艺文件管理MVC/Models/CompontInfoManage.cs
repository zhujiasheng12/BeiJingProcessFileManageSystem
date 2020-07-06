﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using 北京工艺文件管理MVC.Database;
using 北京工艺文件管理MVC.DBDatabase;

namespace 北京工艺文件管理MVC.Models
{
    public class CompontInfoManage
    {
        public static List<CompontInfo> GetTaskInfo(DatabaseList model,int parentID, string orderNum)
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
                        //compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        //compontInfo.lastAlterPersonName = "";
                        //var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        //if (stafflastAlter != null)
                        //{
                        //    compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        //}
                        compontInfo.ispart = false;
                        //compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        //compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        //compontInfo.demandTime = compontInfo.planEndTime;
                        //compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
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
                            //compontInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            compontInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            compontInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            //compontInfo.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                            compontInfo.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                            compontInfo.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                            //compontInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
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
                            //compontInfo.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                            compontInfo.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                            compontInfo.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                            //compontInfo.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            compontInfo.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            compontInfo.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            //compontInfo.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
                            compontInfo.ToolTableFileUploadTime = item.ToolTableFileUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolTableFileUploadTime);
                            compontInfo.ToolTableFileUploadTimeStr = item.ToolTableFileUploadTime == null ? "" : item.ToolTableFileUploadTime.ToString().Substring(0, item.ToolTableFileUploadTime.ToString().LastIndexOf(':'));
                            //compontInfo.staffID = item.staffID == null ? 0 : Convert.ToInt32(item.staffID);
                            //compontInfo.staffName = "";
                            //if (item.staffID != null)
                            //{
                            //    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.staffID && r.state == "正常").FirstOrDefault();
                            //    if (staff != null)
                            //    {
                            //        compontInfo.staffName = staff.StaffName;
                            //    }
                            //}
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
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 1:
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 2:
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 3:
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 4:
                                    compontInfo.taskStateStr = "待完成";
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
                        
                        compontInfo.children = CompontInfoManage.GetTaskInfo(model,item.ID, compontInfo.allConponentNum + ".");
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
                        List<CompontInfo> list = TaskInfoManage.GetPartTaskInfo(model, item.ID, compontInfo.allConponentNum + ".", out unfinish);

                       
                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;

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


        public static List<CompontInfo> GetTaskInfoCreate(DatabaseList model, int parentID, string orderNum)
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
                        //compontInfo.lastAlterPersonID = item.LastAlterPersonID == null ? 0 : Convert.ToInt32(item.LastAlterPersonID);
                        //compontInfo.lastAlterPersonName = "";
                        //var stafflastAlter = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == item.LastAlterPersonID && r.state == "正常").FirstOrDefault();
                        //if (stafflastAlter != null)
                        //{
                        //    compontInfo.lastAlterPersonName = stafflastAlter.StaffName;
                        //}
                        compontInfo.ispart = false;
                        //compontInfo.lastAlterTime = item.LastAlterTime == null ? DateTime.Now : Convert.ToDateTime(item.LastAlterTime);
                        //compontInfo.lastAlterTimeStr = item.LastAlterTime == null ? "" : item.LastAlterTime.ToString().Substring(item.LastAlterTime.ToString().LastIndexOf(':'));
                        compontInfo.planEndTime = item.PlanEndTime == null ? DateTime.Now.AddYears(1) : Convert.ToDateTime(item.PlanEndTime);
                        compontInfo.planEndTimeStr = item.PlanEndTime == null ? "" : item.PlanEndTime.ToString().Substring(0, item.PlanEndTime.ToString().LastIndexOf(':'));
                        compontInfo.planStartTime = item.PlanStartTime == null ? DateTime.Now : Convert.ToDateTime(item.PlanStartTime);
                        compontInfo.planStartTimeStr = item.PlanStartTime == null ? "" : item.PlanStartTime.ToString().Substring(0, item.PlanStartTime.ToString().LastIndexOf(':'));
                        compontInfo.demandTime = compontInfo.planEndTime;
                        compontInfo.demandTimeStr = compontInfo.planEndTimeStr;
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
                            //compontInfo.demandTime = item.DemandTime == null ? DateTime.Now : Convert.ToDateTime(item.DemandTime);
                            //compontInfo.demandTimeStr = item.DemandTime == null ? "" : item.DemandTime.ToString().Substring(0, item.DemandTime.ToString().LastIndexOf(":"));
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
                            //compontInfo.otherFilePath = item.OtherFilePath == null ? "" : item.OtherFilePath;
                            compontInfo.otherFileTime = item.OtherFileTime == null ? DateTime.Now : Convert.ToDateTime(item.OtherFileTime);
                            compontInfo.otherFileTimeStr = item.OtherFileTime == null ? "" : item.OtherFileTime.ToString().Substring(0, item.OtherFileTime.ToString().LastIndexOf(':'));
                            //compontInfo.processCardUploadPath = item.ProcessCardUploadPath == null ? "" : item.ProcessCardUploadPath;
                            compontInfo.processCardUploadTime = item.ProcessCardUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessCardUploadTime);
                            compontInfo.processCardUploadTimeStr = item.ProcessCardUploadTime == null ? "" : item.ProcessCardUploadTime.ToString().Substring(0, item.ProcessCardUploadTime.ToString().LastIndexOf(':'));
                            //compontInfo.processFileUploadPath = item.ProcessFileUploadPath == null ? "" : item.ProcessFileUploadPath;
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
                            //compontInfo.processScheduleUpPath = item.ProcessScheduleUpPath == null ? "" : item.ProcessScheduleUpPath;
                            compontInfo.processScheduleUpTime = item.ProcessScheduleUpTime == null ? DateTime.Now : Convert.ToDateTime(item.ProcessScheduleUpTime);
                            compontInfo.processScheduleUpTimeStr = item.ProcessScheduleUpTime == null ? "" : item.ProcessScheduleUpTime.ToString().Substring(0, item.ProcessScheduleUpTime.ToString().LastIndexOf(':'));
                           // compontInfo.ToolingDrawingsUploadPath = item.ToolingDrawingsUploadPath == null ? "" : item.ToolingDrawingsUploadPath;
                            compontInfo.ToolingDrawingsUploadTime = item.ToolingDrawingsUploadTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadTime);
                            compontInfo.ToolingDrawingsUploadTimeStr = item.ToolingDrawingsUploadTime == null ? "" : item.ToolingDrawingsUploadTime.ToString().Substring(0, item.ToolingDrawingsUploadTime.ToString().LastIndexOf(':'));
                            //compontInfo.ToolTableFileUploadPath = item.ToolTableFileUploadPath == null ? "" : item.ToolTableFileUploadPath;
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
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 1:
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 2:
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 3:
                                    compontInfo.taskStateStr = "待完成";
                                    break;
                                case 4:
                                    compontInfo.taskStateStr = "待完成";
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
                        var unendFlag = true;
                        compontInfo.children = CompontInfoManage.GetTaskInfoCreate(model,item.ID, compontInfo.allConponentNum + ".");

                        {
                            var count = compontInfo.children.Where(r => r.taskStateStr !="已完成").Count();
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
                        List<CompontInfo> list = TaskInfoManage.GetPartTaskInfoCreate(model,item.ID, compontInfo.allConponentNum + ".", out unfinish);                    
                        compontInfo.children.AddRange(list);
                        compontInfo.allTaskNum += list.Count();
                        compontInfo.unfinishedNum += unfinish;

                        if (!(compontInfo.taskStateStr == "已完成" && compontInfo.unfinishedNum < 1&&(unendFlag)))
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
        /// 添加产品组件
        /// </summary>
        /// <param name="parent">父节点主键ID</param>
        /// <param name="compontName">节点名称</param>
        /// <param name="compontNum">节点号</param>
        /// <param name="creatPersonID">创建人ID</param>
        /// <param name="planStartTime">计划开始时间</param>
        /// <param name="planEndTime">计划结束时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddCompont(int parent, string compontName, string compontNum, int creatPersonID, DateTime planStartTime, DateTime planEndTime,int craftPersonID, List<int> previewCategory, out int taskId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parent && r.ComponentNum == compontNum && r.state == "正常");
                            if (compont.Count() > 0)
                            {
                                mytran.Rollback();
                                errMsg = "该编号已存在，请重新输入！";
                                taskId = 0;
                                return false;
                            }
                            var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parent && r.TaskNum == compontNum && r.state == "正常");
                            if (task.Count() > 0)
                            {
                                mytran.Rollback();
                                errMsg = "该编号已存在，请重新输入！";
                                taskId = 0;
                                return false;
                            }
                            DateTime? gonghzuang = null;
                            if (compontNum.Contains("JDSGZ"))
                            {
                                gonghzuang = DateTime.Now;
                            }
                            JDJS_PDMS_CompontInfo_Table jd = new JDJS_PDMS_CompontInfo_Table()
                            {
                                ComponentName = compontName,
                                ComponentNum = compontNum,
                                CreatPersonID = creatPersonID,
                                CraftPersonID =craftPersonID ,
                                CreatTime = DateTime.Now,
                                DemandTime =planEndTime ,
                                staffID =creatPersonID ,
                                CompontState =1,
                                ArrangeTaskTime =DateTime .Now ,
                                PlanCompletionTime =planEndTime ,
                                TaskState =1,
                                Version =1,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                ParentID = parent,
                                PlanEndTime = planEndTime,
                                PlanStartTime = planStartTime,
                                state = "正常",
                                ProcessCardUploadTime = gonghzuang,
                                ProcessScheduleUpTime = gonghzuang,
                            };
                            
                            model.JDJS_PDMS_CompontInfo_Table.Add(jd);
                            model.SaveChanges();
                            taskId = jd.ID;
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_CompontInfo_Table.Add(jd);
                            foreach (var item in previewCategory)
                            {
                                JDJS_PDMS_Compont_ProcessType_Table type = new JDJS_PDMS_Compont_ProcessType_Table()
                                {
                                    CategoryTypeID = item,
                                    CreatPersonID = creatPersonID,
                                    CreatTime = DateTime.Now,
                                    LastAlterPersonID = creatPersonID,
                                    LastAlterTime = DateTime.Now,
                                    state = "正常",
                                    CompontID  = taskId

                                };
                                model.JDJS_PDMS_Compont_ProcessType_Table.Add(type);
                                model.SaveChanges();
                                da.JDJS_PDMS_Compont_ProcessType_Table.Add(type);
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
                            taskId = 0;
                            return false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                taskId = 0;
                return false;
            }
        }

        /// <summary>
        /// 添加产品组件
        /// </summary>
        /// <param name="parent">父节点主键ID</param>
        /// <param name="compontName">节点名称</param>
        /// <param name="compontNum">节点号</param>
        /// <param name="creatPersonID">创建人ID</param>
        /// <param name="planStartTime">计划开始时间</param>
        /// <param name="planEndTime">计划结束时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddCompontNoCraft(int parent, string compontName, string compontNum, int creatPersonID, DateTime planStartTime, DateTime planEndTime, out int taskId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parent && r.ComponentNum == compontNum && r.state == "正常");
                            if (compont.Count() > 0)
                            {
                                mytran.Rollback();
                                errMsg = "该编号已存在，请重新输入！";
                                taskId = 0;
                                return false;
                            }
                            var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parent && r.TaskNum == compontNum && r.state == "正常");
                            if (task.Count() > 0)
                            {
                                mytran.Rollback();
                                errMsg = "该编号已存在，请重新输入！";
                                taskId = 0;
                                return false;
                            }
                            DateTime? gonghzuang = null;
                            if (compontNum.Contains("JDSGZ"))
                            {
                                gonghzuang = DateTime.Now;
                            }
                            JDJS_PDMS_CompontInfo_Table jd = new JDJS_PDMS_CompontInfo_Table()
                            {
                                ComponentName = compontName,
                                ComponentNum = compontNum,
                                CreatPersonID = creatPersonID,
                                CreatTime = DateTime.Now,
                                DemandTime = planEndTime,
                                staffID = creatPersonID,
                                CompontState = 5,
                                ArrangeTaskTime = DateTime.Now,
                                PlanCompletionTime = planEndTime,
                                TaskState = 5,
                                Version = 1,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                ParentID = parent,
                                PlanEndTime = planEndTime,
                                PlanStartTime = planStartTime,
                                state = "正常",
                                ProcessCardUploadTime = gonghzuang,
                                ProcessScheduleUpTime = gonghzuang,
                            };
                            model.JDJS_PDMS_CompontInfo_Table.Add(jd);
                            model.SaveChanges();
                            taskId = jd.ID;
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_CompontInfo_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            taskId = 0;
                            return false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                taskId = 0;
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
        public static bool AddCompontFile(int taskId, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该任务不存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                            {
                                
                                AlterDesc = "上传零件任务图纸",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                CompontID  = task.ID
                            };
                            model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
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
        /// 删除组件图纸
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="personId">人员Id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteCompontFile(int taskId, int personId, List<string> fileNames, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该任务不存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                            {
                                AlterDesc = "删除零件任务图纸",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                CompontID  = task.ID
                            };
                            model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
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
                    var pathTask = TaskInfoManage.GetCompontPath(taskId);
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
        /// 删除产品组件
        /// </summary>
        /// <param name="compontID">组件ID</param>
        /// <param name="deletePersonID">删除人</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteCompont(int compontID, int deletePersonID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == compontID && r.state == "正常");
                    var partTasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == compontID && r.state == "正常");
                    if (tasks.Count() > 0 || partTasks.Count() > 0)
                    {
                        //errMsg = "该节点下还有其它节点或零件未删除，请先删除子节点后再试！";
                        //return false;
                    }
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该节点不存在！";
                        return false;
                    }

                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetCompontPath(compontID);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask);
                    if (Directory.Exists(dirPath))
                    {
                        Directory.Delete(dirPath, true);
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().state = "删除";
                            task.state = "删除";
                            if (DeleteCompontByParent(compontID, ref errMsg))
                            {
                                if (DeleteTaskByCompontID(compontID, ref errMsg))
                                { }
                                else
                                {
                                    throw new Exception(errMsg);
                                }
                            }
                            else
                            {
                                throw new Exception(errMsg);
                            }
                            
                            task.LastAlterPersonID = deletePersonID;
                            task.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                            {
                                AlterDesc = "删除组件产品",
                                AlterTime = DateTime.Now,
                                CompontID = compontID,
                                CreatPersonID = deletePersonID,
                                CreatTime = DateTime.Now,
                                staffID = deletePersonID,
                                state = "正常"
                            };
                            model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
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
        /// 根据父节点删除组件任务
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <returns></returns>
        public static bool DeleteCompontByParent(int parentID,ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var componts = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                            foreach (var item in componts)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == item.ID).FirstOrDefault().state = "删除";
                                item.state = "删除";
                                if (DeleteCompontByParent(item.ID, ref errMsg))
                                {
                                    if (DeleteTaskByCompontID(item.ID, ref errMsg))
                                    { }
                                    else
                                    {
                                        throw new Exception(errMsg);
                                    }
                                }
                                else
                                {
                                    throw new Exception(errMsg);
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
        /// 根据父节点删除零件任务
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool DeleteTaskByCompontID(int parentID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var componts = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常");
                            foreach (var item in componts)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_TaskInfo_Table.Where(r => r.ID == item.ID).FirstOrDefault().state = "删除";
                                item.state = "删除";                               
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
        /// 获取所有的组别
        /// </summary>
        /// <returns></returns>
        public static List<CompontPreviewCategoryInfo> GetCompontPreviewCategory()
        {
            try
            {
                List<CompontPreviewCategoryInfo> previewCategoryInfoList = new List<CompontPreviewCategoryInfo>();
                using (Model1 model = new Model1())
                {
                    var category = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.state == "正常");
                    foreach (var item in category)
                    {
                        CompontPreviewCategoryInfo cat = new CompontPreviewCategoryInfo();
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
        /// 修改组件产品信息
        /// </summary>
        /// <param name="compontID">需要修改的组件产品ID</param>
        /// <param name="parentID">父节点ID</param>
        /// <param name="alterPersonID">修改人员ID</param>
        /// <param name="compontName">产品组件名称</param>
        /// <param name="compontNum">编号</param>
        /// <param name="planStartTime">计划开始时间</param>
        /// <param name="planEndTime">计划结束时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterCompontInfo(int compontID, int parentID, int alterPersonID, string compontName, string compontNum, DateTime planStartTime, DateTime planEndTime, int craftPersonId, List<int> previewCategory, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID && r.state == "正常").FirstOrDefault();
                    if (compont == null)
                    {
                        errMsg = "该产品组件不存在，请确认后再试！";
                        return false;
                    }
                    if (compont.ParentID != parentID)
                    {
                        errMsg = "暂不支持修改父节点";
                        return false;
                    }
                    if (compont.ComponentNum == compontNum)
                    { //如果编号没有修改则只改其它的就行了
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                bool isAlter = false;
                                if (compont.ComponentName != compontName)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ComponentName = compontName;
                                    string name = compont.ComponentName;
                                    compont.ComponentName = compontName;
                                    isAlter = true;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品组件名称，由" + name + "修改为" + compontName,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                }
                                if (compont.PlanStartTime != planStartTime)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanStartTime = planStartTime;
                                    string time = compont.PlanStartTime == null ? "" : compont.PlanStartTime.ToString();
                                    compont.PlanStartTime = planStartTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划开始时间,由" + time + "修改为" + planStartTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (compont.PlanEndTime != planEndTime)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanEndTime = planEndTime;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().DemandTime = planEndTime;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanCompletionTime = planEndTime;
                                    string time = compont.PlanEndTime == null ? "" : compont.PlanEndTime.ToString();
                                    compont.PlanEndTime = planEndTime;
                                    compont.DemandTime = planEndTime;
                                    compont.PlanCompletionTime = planEndTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划结束时间,由" + time + "修改为" + planEndTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }

                                List<int> typeList = new List<int>();
                                var type = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID  == compontID  && r.state == "正常");
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
                                        JDJS_PDMS_Compont_ProcessType_Table typeNew = new JDJS_PDMS_Compont_ProcessType_Table()
                                        {
                                            CategoryTypeID = item,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            LastAlterPersonID = alterPersonID,
                                            LastAlterTime = DateTime.Now,
                                            state = "正常",
                                            CompontID  = compontID 

                                        };
                                        model.JDJS_PDMS_Compont_ProcessType_Table.Add(typeNew);
                                        model.SaveChanges();
                                        DatabaseList da = DatabaseList.GetData();
                                        da.JDJS_PDMS_Compont_ProcessType_Table.Add(typeNew);
                                        JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                        {
                                           
                                            AlterDesc = "增加预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            CompontID  = compontID
                                        };
                                        model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }

                                foreach (var item in typeList)
                                {
                                    if (!previewCategory.Contains(item))
                                    {
                                        DatabaseList da = DatabaseList.GetData();
                                        var taskOld = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == compontID  && r.CategoryTypeID == item && r.state == "正常");
                                        foreach (var real in taskOld)
                                        {
                                            da.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.ID == real.ID).First().state = "删除";
                                        }
                                        model.JDJS_PDMS_Compont_ProcessType_Table.RemoveRange(taskOld);
                                        JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                        {
                                            
                                            AlterDesc = "删除预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            CompontID = compontID 
                                        };
                                        model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }
                                if (compont.CraftPersonID != craftPersonId)
                                {
                                    string nameOld = "";
                                    string nameNew = "";
                                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == compont.CraftPersonID && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameOld = staff.StaffName;
                                    }
                                    staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == craftPersonId && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameNew = staff.StaffName;
                                    }
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = craftPersonId;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ArrangeTaskTime = DateTime.Now;
                                    compont.CraftPersonID = craftPersonId;
                                    compont.ArrangeTaskTime = DateTime.Now;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        
                                        AlterDesc = "修改零件任务工艺责任人Z，由" + nameOld + "修改为" + nameNew,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        CompontID  = compontID 
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                    compont.TaskState = 1;
                                }

                                if (isAlter)
                                {
                                    compont.LastAlterPersonID = alterPersonID;
                                    compont.LastAlterTime = DateTime.Now;
                                }
                                

                                model.SaveChanges();
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
                    else
                    {
                        var compontAll = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.ComponentNum == compontNum && r.state == "正常");
                        if (compontAll.Count() > 0)
                        {
                            errMsg = "该编号已存在，请重新输入！";
                            return false;
                        }
                        var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.TaskNum == compontNum && r.state == "正常");
                        if (task.Count() > 0)
                        {
                            errMsg = "该编号已存在，请重新输入！";
                            return false;
                        }
                        string compontNumOld = compont.ComponentNum;
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ComponentNum = compontNum;
                                string num = compont.ComponentNum;
                                compont.ComponentNum = compontNum;
                                JDJS_PDMS_CompontInfo_Alter_History_Table jd1 = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                {
                                    AlterDesc = "修改产品组件编号，由" + num + "修改为" + compontNum,
                                    AlterTime = DateTime.Now,
                                    CompontID = compontID,
                                    CreatPersonID = alterPersonID,
                                    CreatTime = DateTime.Now,
                                    staffID = alterPersonID,
                                    state = "正常"
                                };
                                model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd1);


                                bool isAlter = true;
                                if (compont.ComponentName != compontName)
                                {
                                   // DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ComponentName = compontName;
                                    string name = compont.ComponentName;
                                    compont.ComponentName = compontName;
                                    isAlter = true;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品组件名称，由" + name + "修改为" + compontName,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                }
                                if (compont.PlanStartTime != planStartTime)
                                {
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanStartTime = planStartTime;
                                    string time = compont.PlanStartTime == null ? "" : compont.PlanStartTime.ToString();
                                    compont.PlanStartTime = planStartTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划开始时间,由" + time + "修改为" + planStartTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (compont.PlanEndTime != planEndTime)
                                {
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanEndTime = planEndTime;
                                    string time = compont.PlanEndTime == null ? "" : compont.PlanEndTime.ToString();
                                    compont.PlanEndTime = planEndTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划结束时间,由" + time + "修改为" + planEndTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }

                                List<int> typeList = new List<int>();
                                var type = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == compontID && r.state == "正常");
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
                                        JDJS_PDMS_Compont_ProcessType_Table typeNew = new JDJS_PDMS_Compont_ProcessType_Table()
                                        {
                                            CategoryTypeID = item,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            LastAlterPersonID = alterPersonID,
                                            LastAlterTime = DateTime.Now,
                                            state = "正常",
                                            CompontID = compontID

                                        };
                                        model.JDJS_PDMS_Compont_ProcessType_Table.Add(typeNew);
                                        model.SaveChanges();
                                        //DatabaseList da = DatabaseList.GetData();
                                        da.JDJS_PDMS_Compont_ProcessType_Table.Add(typeNew);
                                        JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                        {

                                            AlterDesc = "增加预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            CompontID = compontID
                                        };
                                        model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }

                                foreach (var item in typeList)
                                {
                                    if (!previewCategory.Contains(item))
                                    {
                                        //DatabaseList da = DatabaseList.GetData();
                                        var taskOld = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == compontID && r.CategoryTypeID == item && r.state == "正常");
                                        foreach (var real in taskOld)
                                        {
                                            da.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.ID == real.ID).First().state = "删除";
                                        }
                                        model.JDJS_PDMS_Compont_ProcessType_Table.RemoveRange(taskOld);
                                        JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                        {

                                            AlterDesc = "删除预览组别" + item.ToString(),
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = alterPersonID,
                                            CreatTime = DateTime.Now,
                                            staffID = alterPersonID,
                                            state = "正常",
                                            CompontID = compontID
                                        };
                                        model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                        isAlter = true;
                                    }
                                }
                                if (compont.CraftPersonID != craftPersonId)
                                {
                                    string nameOld = "";
                                    string nameNew = "";
                                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == compont.CraftPersonID && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameOld = staff.StaffName;
                                    }
                                    staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == craftPersonId && r.state == "正常").FirstOrDefault();
                                    if (staff != null)
                                    {
                                        nameNew = staff.StaffName;
                                    }
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = craftPersonId;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ArrangeTaskTime = DateTime.Now;
                                    compont.CraftPersonID = craftPersonId;
                                    compont.ArrangeTaskTime = DateTime.Now;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {

                                        AlterDesc = "修改零件任务工艺责任人Z，由" + nameOld + "修改为" + nameNew,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常",
                                        CompontID = compontID
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                    compont.TaskState = 1;
                                }
                                if (isAlter)
                                {
                                    compont.LastAlterPersonID = alterPersonID;
                                    compont.LastAlterTime = DateTime.Now;
                                }
                                model.SaveChanges();
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
                        var pathTask = TaskInfoManage.GetCompontPath(parentID);
                        var dirPathOld = Path.Combine(pathInfo.upLoadPath(), pathTask, compontNumOld);
                        var dirPathNew = Path.Combine(pathInfo.upLoadPath(), pathTask, compontNum);
                        if (Directory.Exists(dirPathOld))
                        {
                            DirectoryInfo dir = new DirectoryInfo(dirPathOld);
                            if (dir.Exists)
                            {
                                dir.MoveTo(dirPathNew);
                            }
                        }

                        return true;
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
        /// 修改组件产品信息,无工艺
        /// </summary>
        /// <param name="compontID">需要修改的组件产品ID</param>
        /// <param name="parentID">父节点ID</param>
        /// <param name="alterPersonID">修改人员ID</param>
        /// <param name="compontName">产品组件名称</param>
        /// <param name="compontNum">编号</param>
        /// <param name="planStartTime">计划开始时间</param>
        /// <param name="planEndTime">计划结束时间</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterCompontInfoNoCraft(int compontID, int parentID, int alterPersonID, string compontName, string compontNum, DateTime planStartTime, DateTime planEndTime, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID && r.state == "正常").FirstOrDefault();
                    if (compont == null)
                    {
                        errMsg = "该产品组件不存在，请确认后再试！";
                        return false;
                    }
                    if (compont.ParentID != parentID)
                    {
                        errMsg = "暂不支持修改父节点";
                        return false;
                    }
                   
                    if (compont.ComponentNum == compontNum)
                    { //如果编号没有修改则只改其它的就行了
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                compont.TaskState = 5;
                                compont.CompontState = 5;
                                bool isAlter = false;
                                if (compont.ComponentName != compontName)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().TaskState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CompontState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ComponentName = compontName;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                    string name = compont.ComponentName;
                                    compont.ComponentName = compontName;
                                    isAlter = true;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品组件名称，由" + name + "修改为" + compontName,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                }
                                if (compont.PlanStartTime != planStartTime)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().TaskState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CompontState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanStartTime = planStartTime;
                                    string time = compont.PlanStartTime == null ? "" : compont.PlanStartTime.ToString();
                                    compont.PlanStartTime = planStartTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划开始时间,由" + time + "修改为" + planStartTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (compont.PlanEndTime != planEndTime)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().TaskState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CompontState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanEndTime = planEndTime;
                                    string time = compont.PlanEndTime == null ? "" : compont.PlanEndTime.ToString();
                                    compont.PlanEndTime = planEndTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划结束时间,由" + time + "修改为" + planEndTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                DatabaseList da1 = DatabaseList.GetData();
                                da1.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                List<int> typeList = new List<int>();
                                var type = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == compontID && r.state == "正常");
                                foreach (var item in type)
                                {
                                    
                                    da1.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.ID == item.ID).First().state = "删除";
                                    item.state = "删除";
                                    item.LastAlterPersonID = alterPersonID;
                                    item.LastAlterTime = DateTime.Now;
                                }



                                compont.AcceptTaskTime = null;
                                compont.ArrangeTaskTime = null;
                                compont.CompontState = 5;
                                compont.CraftPersonID = null;
                                compont.TaskState = 5;

                                if (true)
                                {
                                    DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().TaskState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CompontState = 5;
                                    compont.LastAlterPersonID = alterPersonID;
                                    compont.LastAlterTime = DateTime.Now;
                                }


                                model.SaveChanges();
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
                    else
                    {
                        var compontAll = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.ComponentNum == compontNum && r.state == "正常");
                        if (compontAll.Count() > 0)
                        {
                            errMsg = "该编号已存在，请重新输入！";
                            return false;
                        }
                        var task = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.TaskNum == compontNum && r.state == "正常");
                        if (task.Count() > 0)
                        {
                            errMsg = "该编号已存在，请重新输入！";
                            return false;
                        }
                        compont.TaskState = 5;
                        compont.CompontState = 5;
                        string compontNumOld = compont.ComponentNum;
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().TaskState = 5;
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CompontState = 5;
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ComponentNum = compontNum;
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                string num = compont.ComponentNum;
                                compont.ComponentNum = compontNum;
                                JDJS_PDMS_CompontInfo_Alter_History_Table jd1 = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                {
                                    AlterDesc = "修改产品组件编号，由" + num + "修改为" + compontNum,
                                    AlterTime = DateTime.Now,
                                    CompontID = compontID,
                                    CreatPersonID = alterPersonID,
                                    CreatTime = DateTime.Now,
                                    staffID = alterPersonID,
                                    state = "正常"
                                };
                                model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd1);


                                bool isAlter = true;
                                if (compont.ComponentName != compontName)
                                {
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().ComponentName = compontName;
                                    string name = compont.ComponentName;
                                    compont.ComponentName = compontName;
                                    isAlter = true;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品组件名称，由" + name + "修改为" + compontName,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                }
                                if (compont.PlanStartTime != planStartTime)
                                {
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanStartTime = planStartTime;
                                    string time = compont.PlanStartTime == null ? "" : compont.PlanStartTime.ToString();
                                    compont.PlanStartTime = planStartTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划开始时间,由" + time + "修改为" + planStartTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                if (compont.PlanEndTime != planEndTime)
                                {
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().PlanEndTime = planEndTime;
                                    string time = compont.PlanEndTime == null ? "" : compont.PlanEndTime.ToString();
                                    compont.PlanEndTime = planEndTime;
                                    JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改产品计划结束时间,由" + time + "修改为" + planEndTime,
                                        AlterTime = DateTime.Now,
                                        CompontID = compontID,
                                        CreatPersonID = alterPersonID,
                                        CreatTime = DateTime.Now,
                                        staffID = alterPersonID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    isAlter = true;
                                }
                                var type = model.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.CompontID == compontID && r.state == "正常");
                                foreach (var item in type)
                                {
                                    da.JDJS_PDMS_Compont_ProcessType_Table.Where(r => r.ID == item.ID).First().state = "删除";
                                    item.state = "删除";
                                    item.LastAlterPersonID = alterPersonID;
                                    item.LastAlterTime = DateTime.Now;
                                }

                                compont.AcceptTaskTime = null;
                                compont.ArrangeTaskTime = null;
                                compont.CompontState = 5;
                                compont.CraftPersonID = null;
                                compont.TaskState = 5;
                                if (isAlter)
                                {
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CraftPersonID = null;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().TaskState = 5;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == compontID).First().CompontState = 5;
                                    compont.LastAlterPersonID = alterPersonID;
                                    compont.LastAlterTime = DateTime.Now;
                                }
                                model.SaveChanges();
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
                        var pathTask = TaskInfoManage.GetCompontPath(parentID);
                        var dirPathOld = Path.Combine(pathInfo.upLoadPath(), pathTask, compontNumOld);
                        var dirPathNew = Path.Combine(pathInfo.upLoadPath(), pathTask, compontNum);
                        if (Directory.Exists(dirPathOld))
                        {
                            DirectoryInfo dir = new DirectoryInfo(dirPathOld);
                            if (dir.Exists)
                            {
                                dir.MoveTo(dirPathNew);
                            }
                        }

                        return true;
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
        /// 获取所有的模糊搜索字符串
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllTaskInfoStr(List<CompontInfo> list)
        {

            List<string> strList = new List<string>();
            strList = GetAllTaskInfoStrMethod(list);
            return strList;
        }


        public static List<string> GetAllTaskInfoStrMethod(List<CompontInfo> list)
        {
            List<string> strList = new List<string>();
            foreach (var item in list)
            {
                strList.Add(item.taskName);
                strList.Add(item.taskNum);
                if (item.craftPersonName != null)
                {
                    strList.Add(item.craftPersonName);
                }
                if (item.children != null && item.children.Count() > 0)
                {
                    strList.AddRange(GetAllTaskInfoStrMethod(item.children));
                }
            }
            return strList;



        }

        public static List<CompontInfo> SearchSubmit(List<CompontInfo> infoList, string str, bool flag)
        {
            for (int i = 0; i < infoList.Count(); i++)
            {
                infoList[i].flag = flag;
                if (infoList[i].taskName == str || infoList[i].taskNum == str || (infoList[i].craftPersonName != null && infoList[i].craftPersonName == str))
                {
                    infoList[i].flag = true;

                }

                if (infoList[i].children != null && infoList[i].children.Count() > 0)
                {
                    SearchSubmit(infoList[i].children, str, infoList[i].flag);

                    for (int j = 0; j < infoList[i].children.Count(); j++)
                    {
                        if (infoList[i].children[j].flag == false)
                        {
                            infoList[i].children.Remove(infoList[i].children[j]);
                            j--;
                        }
                        else
                        {
                            infoList[i].flag = true;
                        }
                    }
                }
                if (infoList[i].flag == false)
                {
                    infoList.Remove(infoList[i]);
                    i--;
                }
            }
            return infoList;
        }


    }

    public class CompontPreviewCategoryInfo
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

    public class CompontInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public int componentID { get; set; }
        /// <summary>
        /// 预览组别主键ID
        /// </summary>
        public List<int> previewCategory { get; set; }
        /// <summary>
        /// 预览组别
        /// </summary>
        public string previewCategoryStr { get; set; }
        /// <summary>
        /// 全部零件数量
        /// </summary>
        public int allTaskNum { get; set; }
        /// <summary>
        /// 未完成零件数量
        /// </summary>
        public int unfinishedNum { get; set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskNum { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string taskName { get; set; }
        /// <summary>
        /// 责任人名称
        /// </summary>
        public string staffName { get; set; }
        /// <summary>
        /// 责任人主键ID
        /// </summary>
        public int staffID { get; set; }
        /// <summary>
        /// 人员标识ID
        /// </summary>
        public string personAllId { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 全部任务编号
        /// </summary>
        public string allConponentNum { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string planStartTimeStr { get; set; }
        public DateTime planStartTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public string planEndTimeStr { get; set; }
        public DateTime planEndTime { get; set; }
        /// <summary>
        /// 需求时间
        /// </summary>
        public string demandTimeStr { get; set; }
        public DateTime demandTime { get; set; }
        /// <summary>
        /// 工艺责任人名称
        /// </summary>
        public string craftPersonName { get; set; }
        public int craftPersonID { get; set; }
        /// <summary>
        /// 安排任务时间
        /// </summary>
        public string arrangeTaskTimeStr { get; set; }
        public DateTime arrangeTaskTime { get; set; }
        /// <summary>
        /// 接收任务时间
        /// </summary>
        public string acceptTaskTimeStr { get; set; }
        public DateTime acceptTaskTime { get; set; }

        public string planCompletionTimeStr { get; set; }
        public DateTime planCompletionTime { get; set; }
        /// <summary>
        /// 工艺过程卡
        /// </summary>
        public string processCardUploadTimeStr { get; set; }
        public DateTime processCardUploadTime { get; set; }
        public string processCardUploadPath { get; set; }
        /// <summary>
        /// 进程单
        /// </summary>
        public string processScheduleUpTimeStr { get; set; }
        public DateTime processScheduleUpTime { get; set; }
        public string processScheduleUpPath { get; set; }

        /// <summary>
        /// 刀具表
        /// </summary>
        public DateTime ToolTableFileUploadTime { get; set; }
        public string ToolTableFileUploadTimeStr { get; set; }
        public string ToolTableFileUploadPath { get; set; }
        public int ToolTableFileUploadPersonID { get; set; }
        public string ToolTableFileUploadPersonName { get; set; }

        /// <summary>
        /// 工装图纸
        /// </summary>
        public DateTime ToolingDrawingsUploadTime { get; set; }
        public string ToolingDrawingsUploadTimeStr { get; set; }
        public string ToolingDrawingsUploadPath { get; set; }
        public int ToolingDrawingsUploadPersonID { get; set; }
        public string ToolingDrawingsUploadPersonName { get; set; }
        public bool toolingDrawingsUploadIsComplete { get; set; }
        public DateTime toolingDrawingsUploadCompleteTime { get; set; }
        public string toolingDrawingsUploadCompleteTimeStr { get; set; }
        /// <summary>
        /// 工装图纸是否完成，是否有
        /// </summary>
        public string toolingDrawingsUploadFlag { get; set; }
        /// <summary>
        /// 是否需要工装图纸
        /// </summary>
        public string toolingDrawingsUploadIsNeed;
        /// <summary>
        /// 加工文件
        /// </summary>
        public string processFileUploadTimeStr { get; set; }
        public DateTime processFileUploadTime { get; set; }
        public string processFileUploadPath { get; set; }
        public int processFileUploadPersonID { get; set; }
        public string processFileUploadPersonName { get; set; }
        public bool processFileUploadIsComplete { get; set; }
        public DateTime processFileUploadCompleteTime { get; set; }
        public string processFileUploadCompleteTimeStr { get; set; }
        /// <summary>
        /// 加工文件是否完成，是否有
        /// </summary>
        public string processFileUploadFlag { get; set; }

        /// <summary>
        /// 其它文件
        /// </summary>
        public string otherFileTimeStr { get; set; }
        public DateTime otherFileTime { get; set; }
        public string otherFilePath { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public string completeTimeStr { get; set; }
        public DateTime completeTime { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string taskStateStr { get; set; }
        public int taskState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string creatTimeStr { get; set; }
        public DateTime creatTime { get; set; }
        /// <summary>
        /// 创建人员
        /// </summary>
        public string creatPersonName { get; set; }
        public int creatPersonID { get; set; }
        /// <summary>
        /// 最终修改时间
        /// </summary>
        public string lastAlterTimeStr { get; set; }
        public DateTime lastAlterTime { get; set; }
        /// <summary>
        /// 最终修改人员
        /// </summary>
        public string lastAlterPersonName { get; set; }
        public int lastAlterPersonID { get; set; }
        /// <summary>
        /// 该条记录状态
        /// </summary>
        public string state { get; set; }

        public bool ispart;

        public bool flag;

        public List<CompontInfo> children { get; set; }
    }
}