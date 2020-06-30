using Spire.Doc;
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
    public class CompontCraftInfoManage
    {
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = task.ComponentName + "该任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != acceptPersonID)
                    {
                        errMsg = task.ComponentName + "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    if (task.TaskState != 1)
                    {
                        errMsg = task.ComponentName + "该任务不满足接收条件，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().AcceptTaskTime = DateTime.Now;
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState  = 2;
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompontState = 2;

                            task.AcceptTaskTime = DateTime.Now;
                            task.TaskState = 2;
                            task.CompontState = 2;
                            task.LastAlterPersonID = acceptPersonID;
                            task.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                            {
                                AlterDesc = "工艺责任人接收任务",
                                AlterTime = DateTime.Now,
                                CreatPersonID = acceptPersonID,
                                CreatTime = DateTime.Now,
                                staffID = acceptPersonID,
                                state = "正常",
                                CompontID  = task.ID
                            };
                            model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = task.ComponentName + "接收成功";
                            return true;

                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = task.ComponentName  + ex.Message;
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
        public static bool CreateMachiningProcess(int taskID, int processNum, bool flag,int createPersonID, int processTypeID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该组件任务不存在";
                        return false;
                    }
                    if (task.CraftPersonID != createPersonID)
                    {
                        errMsg = "该零件任务责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskID && r.ProcessNum == processNum && r.state == "正常");
                    if (process.Count() > 0)
                    {
                        errMsg = "该组件任务工序" + processNum.ToString() + "已存在，请确认后再试！";
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
                            JDJS_PDMS_Compont_ProcessInfo_Table jd = new JDJS_PDMS_Compont_ProcessInfo_Table()
                            {
                                CreatPersonID = createPersonID,
                                CreatTime = DateTime.Now,
                                LastAlterPersonID = createPersonID,
                                LastAlterTime = DateTime.Now,
                                ProcessNum = processNum,
                                ProcessingTypeID = processTypeID,
                                state = "正常",
                                CompontID = taskID,
                                ProcessFileUploadIsComplete = 0,
                                ToolingDrawingsUploadIsComplete = flagCom,
                                ToolingDrawingsUploadIsNeed = flagNeed,

                            };
                            model.JDJS_PDMS_Compont_ProcessInfo_Table.Add(jd);
                            model.SaveChanges();
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Compont_ProcessInfo_Table.Add(jd);
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
        /// 获取组件任务工序信息
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <returns>工序信息列表</returns>
        public static List<CompontCraftInfo> GetPartsProcessInfo(int taskId)
        {
            try
            {
                List<CompontCraftInfo> craftInfos = new List<CompontCraftInfo>();
                using (Model1 model = new Model1())
                {
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        return null;
                    }
                    var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskId && r.state == "正常");
                    foreach (var item in processes)
                    {
                        CompontCraftInfo craftInfo = new CompontCraftInfo();
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
                        craftInfo.toolingDrawingsUploadIsNeed = item.ToolingDrawingsUploadIsNeed == null ? false : (item.ToolingDrawingsUploadIsNeed == 0 ? false : true);
                        craftInfo.toolingDrawingsUploadCompleteTime = item.ToolingDrawingsUploadCompleteTime == null ? DateTime.Now : Convert.ToDateTime(item.ToolingDrawingsUploadCompleteTime);
                        craftInfo.toolingDrawingsUploadCompleteTimeStr = item.ToolingDrawingsUploadCompleteTime == null ? "" : item.ToolingDrawingsUploadCompleteTime.ToString().Substring(0, item.ToolingDrawingsUploadCompleteTime.ToString().LastIndexOf(':'));

                        craftInfo.processingTypeID = item.ProcessingTypeID;
                        craftInfo.processingTypeStr = "";
                        var processType = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                        if (processType != null)
                        {
                            craftInfo.processingTypeStr = processType.ProcessingType;
                        }

                        craftInfo.processNum = item.ProcessNum;
                        craftInfo.taskID = Convert.ToInt32(item.CompontID);
                        craftInfo.taskName = task.ComponentName;
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
        /// 获取组件任务工序信息
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <returns>工序信息列表</returns>
        public static List<CompontCraftInfo> GetPartsProcessInfoByPerson(int taskId, int personId)
        {
            try
            {
                List<CompontCraftInfo> craftInfos = new List<CompontCraftInfo>();
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        return null;
                    }
                    if (typeId == -1)
                    {
                        var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            CompontCraftInfo craftInfo = new CompontCraftInfo();
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
                            var processType = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.CompontID);
                            craftInfo.taskName = task.ComponentName ;
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
                        var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskId && r.ProcessingTypeID == typeId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            CompontCraftInfo craftInfo = new CompontCraftInfo();
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
                            var processType = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.CompontID);
                            craftInfo.taskName = task.ComponentName ;
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
        /// 获取组件任务工序信息
        /// </summary>
        /// <param name="taskId">零件任务ID</param>
        /// <returns>工序信息列表</returns>
        public static List<CompontCraftInfo> GetPartsProcessInfoByPersonGroup(int taskId, int personId)
        {
            try
            {
                List<CompontCraftInfo> craftInfos = new List<CompontCraftInfo>();
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        return null;
                    }
                    if (typeId == -1)
                    {
                        var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            var old = craftInfos.Where(r => r.processingTypeID == item.ProcessingTypeID).FirstOrDefault();
                            if (old != null)
                            {
                                continue;
                            }
                            CompontCraftInfo craftInfo = new CompontCraftInfo();
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
                            var processType = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.CompontID);
                            craftInfo.taskName = task.ComponentName;
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
                        var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskId && r.ProcessingTypeID == typeId && r.state == "正常");
                        foreach (var item in processes)
                        {
                            var old = craftInfos.Where(r => r.processingTypeID == item.ProcessingTypeID).FirstOrDefault();
                            if (old != null)
                            {
                                continue;
                            }
                            CompontCraftInfo craftInfo = new CompontCraftInfo();
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
                            var processType = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.ProcessingTypeID && r.state == "正常").FirstOrDefault();
                            if (processType != null)
                            {
                                craftInfo.processingTypeStr = processType.ProcessingType;
                            }

                            craftInfo.processNum = item.ProcessNum;
                            craftInfo.taskID = Convert.ToInt32(item.CompontID);
                            craftInfo.taskName = task.ComponentName;
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
        /// 删除组件任务的工序
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;

                    }
                    processNum = process.ProcessNum.ToString(); ;
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == process.CompontID  && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的组件任务不存在，请确认后再试！";
                        return false;
                    }
                    taskID = task.ID;
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = "该组件任务工艺责任人与当前登录用户不匹配，请确认后再试！";
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
                            da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId).First().state = "删除";
                            process.state = "删除";
                            process.LastAlterPersonID = personId;
                            process.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_Compont_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftInfo_Alter_History_Table()
                            {
                                AlterDesc = "删除工序",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                ProcessID = processId,
                                staffID = personId,
                                state = "正常"

                            };
                            model.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table.Add(jd);
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
        public static bool AlterPartsProcess(int processId,bool flag, int personId, int processNum, int processTypeId, ref string errMsg)
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;

                    }
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == process.CompontID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该工序对应的组件任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = "该组件任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    //if (process.ProcessingTypeID == processTypeId && process.ProcessNum == processNum )
                    //{
                    //    errMsg = "ok";
                    //    return true;
                    //}
                    //else
                    {

                        int processIdOld = process.ProcessingTypeID;
                        var processTypeOld = "";
                        var processTypeNew = "";
                        var type = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == processIdOld && r.state == "正常").FirstOrDefault();
                        if (type != null)
                        {
                            processTypeOld = type.ProcessingType;
                        }
                        type = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == processTypeId && r.state == "正常").FirstOrDefault();
                        if (type != null)
                        {
                            processTypeNew = type.ProcessingType;
                        }



                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId).First().ToolingDrawingsUploadIsNeed = flagNeed;
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId).First().ToolingDrawingsUploadIsComplete = flagCom;
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId).First().ProcessingTypeID = processTypeId;
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId).First().ProcessNum = processNum;

                                process.ToolingDrawingsUploadIsNeed = flagNeed;
                                process.ToolingDrawingsUploadIsComplete = flagCom;
                                if (process.ProcessingTypeID != processTypeId)
                                {
                                    process.ProcessingTypeID = processTypeId;
                                    JDJS_PDMS_Compont_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改工序加工组别，由" + processTypeOld + "修改为" + processTypeNew,
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = personId,
                                        CreatTime = DateTime.Now,
                                        ProcessID = processId,
                                        staffID = personId,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table.Add(jd);
                                }
                                if (process.ProcessNum != processNum)
                                {
                                    string processNumOld = "";
                                    processNumOld = process.ProcessNum.ToString();
                                    process.ProcessNum = processNum;
                                    JDJS_PDMS_Compont_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftInfo_Alter_History_Table()
                                    {
                                        AlterDesc = "修改工序号，由" + processNumOld + "修改为" + processNum.ToString(),
                                        AlterTime = DateTime.Now,
                                        CreatPersonID = personId,
                                        CreatTime = DateTime.Now,
                                        ProcessID = processId,
                                        staffID = personId,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table.Add(jd);
                                    var pathStr=TaskInfoManage.GetCompontPath(task.ID);
                                    SupplementaryInformation sup = new SupplementaryInformation();
                                    if (Directory.Exists(Path.Combine(sup.upLoadPath(), pathStr, processNumOld)))
                                    {
                                        Directory.Move(Path.Combine(sup.upLoadPath(), pathStr, processNumOld), Path.Combine(sup.upLoadPath(), pathStr, processNum.ToString ()));
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
                        var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                        if (task == null)
                        {
                            errMsg = "该组件任务不存在，请确认后再试！";
                            return false;
                        }
                        if (task.CraftPersonID != personId)
                        {
                            errMsg = "该组件任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                            return false;
                        }
                        if (task.TaskState == 0 || task.TaskState == 1)
                        {
                            errMsg = "请先接收该组件任务，再上传工艺文件！";
                            return false;
                        }
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().ProcessCardUploadTime = DateTime.Now;
                                task.ProcessCardUploadTime = DateTime.Now;
                                task.LastAlterPersonID = personId;
                                task.LastAlterTime = DateTime.Now;
                                JDJS_PDMS_CompontInfo_Alter_History_Table jdinfo = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID  = taskId,
                                };
                                model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jdinfo);
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 3;
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompontState = 3;
                                task.CompontState = 3;
                                task.TaskState = 3;
                                JDJS_PDMS_Compont_CraftFile_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID = taskId
                                };
                                model.JDJS_PDMS_Compont_CraftFile_Alter_History_Table.Add(jd);
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
                        var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
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
                                da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().ProcessScheduleUpTime = DateTime.Now;
                                task.ProcessScheduleUpTime = DateTime.Now;
                                task.LastAlterPersonID = personId;
                                task.LastAlterTime = DateTime.Now;
                                JDJS_PDMS_CompontInfo_Alter_History_Table jdinfo = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID  = taskId,
                                };
                                model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jdinfo);
                                //task.TaskState = 3;
                                JDJS_PDMS_Compont_CraftFile_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺进程单",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID = taskId
                                };
                                model.JDJS_PDMS_Compont_CraftFile_Alter_History_Table.Add(jd);
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
                        var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
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
                                JDJS_PDMS_CompontInfo_Alter_History_Table jdinfo = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                {
                                    AlterDesc = "上传工艺过程卡",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID = taskId,
                                };
                                model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jdinfo);
                                //task.TaskState = 3;
                                JDJS_PDMS_Compont_CraftFile_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = "上传其它文件",
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID = taskId
                                };
                                model.JDJS_PDMS_Compont_CraftFile_Alter_History_Table.Add(jd);
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId).FirstOrDefault();
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
                            var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
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
                                JDJS_PDMS_Compont_CraftFile_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftFile_Alter_History_Table()
                                {
                                    AlterDesc = alterDesc + item,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID = taskId
                                };
                                model.JDJS_PDMS_Compont_CraftFile_Alter_History_Table.Add(jd);
                                JDJS_PDMS_CompontInfo_Alter_History_Table jds = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                {
                                    AlterDesc = alterDesc + item,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    staffID = personId,
                                    state = "正常",
                                    CompontID= taskId,
                                };
                                model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jds);
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskId && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = "该任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID == null)
                    {
                        errMsg = task.ComponentName + "该任务未安排工艺，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = task.ComponentName  + "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    if (task.ProcessCardUploadTime == null)
                    {
                        errMsg = task.ComponentName + "请先上传工艺过程卡！";
                        return false;
                    }
                    if (task.TaskState != 3)
                    {
                        errMsg = task.ComponentName + "该任务不满足工艺过程卡提交条件！";
                        return false;
                    }
                    var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskId && r.state == "正常");
                    if (processes.Count() < 1)
                    {
                        errMsg = task.ComponentName + "请先创建加工工序！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 4;
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompontState = 4;
                            task.TaskState = 4;
                            task.CompontState = 4;
                            task.LastAlterPersonID = personId;
                            task.LastAlterTime = DateTime.Now;
                            JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                            {
                               
                                AlterDesc = "工艺过程卡提交",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                CompontID  = taskId

                            };
                            model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = task.ComponentName + "提交完成";
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        return "";
                    }
                    int taskId = Convert.ToInt32(process.CompontID);
                    var path = TaskInfoManage.GetCompontPath(taskId);
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == process.CompontID && r.state == "正常").FirstOrDefault();
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
                                    //process.ToolingDrawingsUploadTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    task.LastAlterPersonID = personId;
                                    task.LastAlterTime = DateTime.Now;
                                    //task.ToolingDrawingsUploadTime = DateTime.Now;
                                    str = "上传工装图纸";
                                    break;
                                case 1://加工程序
                                    //process.ProcessFileUploadPersonID = personId;
                                   // process.ProcessFileUploadTime = DateTime.Now;
                                   // task.ProcessFileUploadTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    task.LastAlterPersonID = personId;
                                    task.LastAlterTime = DateTime.Now;
                                    str = "上传加工程序";
                                    break;
                                case 2://其它文件
                                    //process.OtherFileUploadPersonID = personId;
                                    process.OtherFileTime = DateTime.Now;
                                    process.LastAlterPersonID = personId;
                                    process.LastAlterTime = DateTime.Now;
                                    str = "上传其它文件";
                                    break;//刀具表
                                case 3:
                                    //process.ToolTableFileUploadPersonID = personId;
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
                                JDJS_PDMS_Compont_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftInfo_Alter_History_Table()
                                {
                                    AlterDesc = str,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    ProcessID = processId,
                                    staffID = personId,
                                    state = "正常"
                                };
                                model.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table.Add(jd);
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId).FirstOrDefault();
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == process.CompontID && r.state == "正常").FirstOrDefault();
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
                            var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                            var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == process.CompontID&& r.state == "正常").FirstOrDefault();
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
                                        //process.LastAlterPersonID = personId;

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
                                        //task.ProcessFileUploadTime = null;
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
                                        //process.OtherFileTime = null;

                                        process.LastAlterTime = DateTime.Now;
                                        process.LastAlterPersonID = personId;
                                    }
                                    break;
                                case 3:
                                    alterDesc = "删除刀具表";
                                    if (listFile == null || listFile.Count() < 1)
                                    {
                                        //process.ToolTableFileUploadPersonID = null;
                                       // process.ToolTableFileUploadTime = null;

                                        process.LastAlterTime = DateTime.Now;
                                        process.LastAlterPersonID = personId;

                                        task.ToolTableFileUploadTime = null;


                                        task.LastAlterPersonID = personId;
                                        task.LastAlterTime = DateTime.Now;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            foreach (var item in fileNames)
                            {
                                JDJS_PDMS_Compont_CraftInfo_Alter_History_Table jd = new JDJS_PDMS_Compont_CraftInfo_Alter_History_Table()
                                {
                                    AlterDesc = alterDesc + item,
                                    AlterTime = DateTime.Now,
                                    CreatPersonID = personId,
                                    CreatTime = DateTime.Now,
                                    ProcessID = processId,
                                    staffID = personId,
                                    state = "正常"

                                };
                                model.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table.Add(jd);
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
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                    if (task == null)
                    {
                        errMsg = task.ComponentName + "该任务不存在，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID == null)
                    {
                        errMsg = task.ComponentName + "该任务未安排工艺，请确认后再试！";
                        return false;
                    }
                    if (task.CraftPersonID != personId)
                    {
                        errMsg = task.ComponentName  + "该任务工艺责任人与当前登录用户不匹配，请确认后再试！";
                        return false;
                    }
                    if (task.ProcessCardUploadTime == null || task.ProcessScheduleUpTime == null /*|| task.ToolingDrawingsUploadTime == null||task.ProcessFileUploadTime ==null*/)
                    {

                        errMsg = task.ComponentName  + "该任务工艺文件未上传，请确认后再试！";
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID== taskID && r.state == "正常");
                    foreach (var item in process)
                    {
                        string num = item.ProcessNum.ToString();
                        while (num.Length < 2)
                        {
                            num += "0";
                        }
                        string str = task.ComponentName  + "-" + task.ComponentNum  + "-" + num;
                        DateTime time = new DateTime();
                        if (item.ToolingDrawingsUploadIsComplete != 1 || item.ProcessFileUploadIsComplete != 1)
                        {
                            errMsg = task.ComponentName  + "该任务工序文件未上传，请确认后再试！";
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
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 5;
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompontState = 5;
                            da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = DateTime.Now;

                            task.CompleteTime = DateTime.Now;
                            task.TaskState = 5;
                            task.CompontState = 5;
                            task.LastAlterTime = DateTime.Now;
                            task.LastAlterPersonID = personId;
                            JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                            {
                                
                                AlterDesc = "任务完成提交",
                                AlterTime = DateTime.Now,
                                CreatPersonID = personId,
                                CreatTime = DateTime.Now,
                                staffID = personId,
                                state = "正常",
                                CompontID = taskID

                            };
                            model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = task.ComponentName + "提交完成";
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
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process != null)
                    {
                        int typeId = process.ProcessingTypeID;
                        var staffs = model.JDJS_PDMS_Staff_Table.Where(r => r.CompontProcessTypeID == typeId && r.state == "正常");
                        foreach (var item in staffs)
                        {
                            int staffID = item.ID;
                            StaffInfo staffInfo = new StaffInfo();


                            staffInfo.id = item.ID;
                            staffInfo.mailbox = item.MailBox;
                            staffInfo.compontProcessTypeId = item.CompontProcessTypeID == null ? 0 : Convert.ToInt32(item.CompontProcessTypeID);
                            staffInfo.compontProcessTypeStr = "";
                            if (item.ProcessTypeID != null)
                            {
                                var type = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.CompontProcessTypeID && r.state == "正常").FirstOrDefault();
                                if (type != null)
                                {
                                    staffInfo.compontProcessTypeStr = type.ProcessingType;
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
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试";
                        return false;
                    }
                    var person = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId && r.state == "正常").FirstOrDefault();
                    if (person == null)
                    {
                        errMsg = "当前登录用户不存在，请确认后再试";
                        return false;
                    }
                    if (person.CompontProcessTypeID != null && person.CompontProcessTypeID != process.ProcessingTypeID)
                    {
                        errMsg = "当前登录用户无此权限，请确认后再试";
                        return false;
                    }
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId && r.state == "正常").FirstOrDefault();
                    if (staff.CompontProcessTypeID != process.ProcessingTypeID)
                    {
                        errMsg = "该员工无此组别权限，请确认后再试";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == process.CompontID && r.state == "正常" && r.ProcessingTypeID == process.ProcessingTypeID);
                            foreach (var item in processes)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == item.ID).First().OtherFileUploadPersonID = staffId;
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == item.ID).First().ProcessFileUploadPersonID = staffId;
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == item.ID).First().ToolingDrawingsUploadPersonID = staffId;
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == item.ID).First().ToolTableFileUploadPersonID = staffId;
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
            try
            {
                using (Model1 model = new Model1())
                {
                    var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == processId && r.state == "正常").FirstOrDefault();
                    if (process == null)
                    {
                        errMsg = "该工序不存在，请确认后再试！";
                        return false;
                    }
                    var task = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == process.CompontID && r.state == "正常").FirstOrDefault();
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
                        try
                        {
                            if (fileFlag == 0)
                            {

                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadIsComplete = flag;
                                process.ToolingDrawingsUploadIsComplete = flag;
                                if (isComplete)
                                {
                                    process.ToolingDrawingsUploadCompleteTime = DateTime.Now;
                                    process.ToolingDrawingsUploadTime = DateTime.Now;
                                    task.ToolingDrawingsUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadCompleteTime = DateTime.Now;
                                    da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().ToolingDrawingsUploadTime = DateTime.Now;
                                    bool isCom = true;
                                    var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == process.CompontID && r.ID != process.ID && r.state == "正常");
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
                                        
                                        da.JDJS_PDMS_CompontInfo_Table .Where(r => r.ID == task.ID).First().TaskState = 5;
                                        da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = DateTime.Now;
                                        task.TaskState = 5;
                                        task.CompontState = 5;
                                        task.CompleteTime = DateTime.Now;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                        JDJS_PDMS_CompontInfo_Alter_History_Table  jd = new JDJS_PDMS_CompontInfo_Alter_History_Table ()
                                        {
                                           
                                            AlterDesc = "任务完成提交",
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = personId,
                                            CreatTime = DateTime.Now,
                                            staffID = personId,
                                            state = "正常",
                                            CompontID =task.ID 

                                        };
                                        model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    }

                                }
                                else
                                {
                                    process.ToolingDrawingsUploadCompleteTime = null;
                                    process.ToolingDrawingsUploadTime = null;
                                    task.ToolingDrawingsUploadTime = null;
                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 4;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = null;
                                    da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadCompleteTime = null;
                                    //da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ToolingDrawingsUploadTime = null;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().ToolingDrawingsUploadTime = null;
                                }
                            }
                            else if (fileFlag == 1)
                            {
                                DatabaseList da = DatabaseList.GetData();
                                da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadIsComplete = flag;
                                process.ProcessFileUploadIsComplete = flag;
                                if (isComplete)
                                {
                                    process.ProcessFileUploadCompleteTime = DateTime.Now;
                                    process.ProcessFileUploadTime = DateTime.Now;
                                    task.ProcessFileUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadCompleteTime = DateTime.Now;
                                    da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().ProcessFileUploadTime = DateTime.Now;

                                    bool isCom = true;
                                    var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == process.CompontID && r.ID != process.ID && r.state == "正常");
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
                                       // DatabaseList da = DatabaseList.GetData();
                                        da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 5;
                                        da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = DateTime.Now;
                                        task.TaskState = 5;
                                        task.CompontState = 5;
                                        task.CompleteTime = DateTime.Now;
                                        task.LastAlterTime = DateTime.Now;
                                        task.LastAlterPersonID = personId;
                                        JDJS_PDMS_CompontInfo_Alter_History_Table jd = new JDJS_PDMS_CompontInfo_Alter_History_Table()
                                        {

                                            AlterDesc = "任务完成提交",
                                            AlterTime = DateTime.Now,
                                            CreatPersonID = personId,
                                            CreatTime = DateTime.Now,
                                            staffID = personId,
                                            state = "正常",
                                            CompontID = task.ID

                                        };
                                        model.JDJS_PDMS_CompontInfo_Alter_History_Table.Add(jd);
                                    }
                                }
                                else
                                {
                                    process.ProcessFileUploadCompleteTime = null;
                                    process.ProcessFileUploadTime = null;
                                    task.ProcessFileUploadTime = null;
                                    da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadCompleteTime = null;
                                    //da.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.ID == process.ID).First().ProcessFileUploadTime = DateTime.Now;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().ProcessFileUploadTime = null;

                                    //DatabaseList da = DatabaseList.GetData();
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().TaskState = 4;
                                    da.JDJS_PDMS_CompontInfo_Table.Where(r => r.ID == task.ID).First().CompleteTime = null;
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
        public static bool ReadProcessFile(string path, int taskId, int personId, ref string errMsg)
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
        public static bool CreateMachiningProcessBatch(List<ProcessInfoFromFile> infos, ref string errMsg)
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
                                var type = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ProcessingType == item.processType && r.state == "正常").FirstOrDefault();
                                if (type == null)
                                {
                                    continue;
                                }
                                processTypeID = type.ID;
                                var task = model.JDJS_PDMS_CompontInfo_Table .Where(r => r.ID == taskID && r.state == "正常").FirstOrDefault();
                                if (task == null)
                                {
                                    errMsg = "该组件任务不存在";
                                    return false;
                                }
                                if (task.CraftPersonID != createPersonID)
                                {
                                    errMsg = "该组件任务责任人与当前登录用户不匹配，请确认后再试！";
                                    return false;
                                }
                                var process = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == taskID && r.ProcessNum == processNum && r.state == "正常");
                                if (process.Count() > 0)
                                {
                                    errMsg = "该组件任务工序" + processNum.ToString() + "已存在，请确认后再试！";
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
                                        JDJS_PDMS_Compont_ProcessInfo_Table  jd = new  JDJS_PDMS_Compont_ProcessInfo_Table ()
                                        {
                                            CreatPersonID = createPersonID,
                                            CreatTime = DateTime.Now,
                                            LastAlterPersonID = createPersonID,
                                            LastAlterTime = DateTime.Now,
                                            ProcessNum = processNum,
                                            ProcessingTypeID = processTypeID,
                                            state = "正常",
                                            CompontID = taskID,
                                            ProcessFileUploadIsComplete = 0,
                                            ToolingDrawingsUploadIsComplete = flagCom,
                                            ToolingDrawingsUploadIsNeed = flagNeed,

                                        };
                                        model.JDJS_PDMS_Compont_ProcessInfo_Table.Add(jd);
                                        model.SaveChanges();
                                        DatabaseList da = DatabaseList.GetData();
                                        da.JDJS_PDMS_Compont_ProcessInfo_Table.Add(jd);

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

    }

    public class CompontCraftInfo
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


    public class CompontFileInfoClass
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