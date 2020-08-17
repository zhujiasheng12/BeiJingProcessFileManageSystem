using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 北京工艺文件管理MVC.Database;

namespace 北京工艺文件管理MVC.Models
{
    public class TaskStatistics
    {
        public static List<StatisticsInfo> GetStatisticsInfo(DateTime? startTime=null,DateTime? endTime=null )
        {
            DateTime start = startTime == null ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(startTime);
            DateTime end= endTime == null ? DateTime.Now : Convert.ToDateTime(endTime);
            List<StatisticsInfo> infos = new List<StatisticsInfo>();
            try
            {
                using (Model1 model = new Model1())
                {
                    var staffs = model.JDJS_PDMS_Staff_Table;
                    foreach (var staff in staffs)
                    {
                        int personId = staff.ID;
                        var componts = model.JDJS_PDMS_CompontInfo_Table;
                        foreach (var compont in componts)
                        {
                            #region 组件创建
                            if (compont.CreatPersonID == personId&&Convert.ToDateTime (compont.CreatTime) >=start && Convert.ToDateTime(compont.CreatTime)<=end)
                            {
                                var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                if (oldPerson == null)
                                {
                                    StatisticsInfo info = new StatisticsInfo()
                                    {
                                        PersonId = personId,
                                        PersonName = staff.StaffName,
                                        CreatTaskNum =1
                                    };
                                    infos.Add(info);
                                }
                                else
                                {
                                    oldPerson.CreatTaskNum++;
                                }
                            }
                            #endregion
                            if (compont.CraftPersonID != null)
                            {


                                #region 工艺过程卡
                                if (compont.ProcessCardUploadTime != null)
                                {
                                    if (compont.CraftPersonID == personId && Convert.ToDateTime(compont.ProcessCardUploadTime) >= start && Convert.ToDateTime(compont.ProcessCardUploadTime) <= end)
                                    {
                                        var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                        if (oldPerson == null)
                                        {
                                            StatisticsInfo info = new StatisticsInfo()
                                            {
                                                PersonId = personId,
                                                PersonName = staff.StaffName,
                                                ProcessCardNumComplete = 1
                                            };
                                            infos.Add(info);
                                        }
                                        else
                                        {
                                            oldPerson.ProcessCardNumComplete++;
                                        }
                                    }
                                }
                                #endregion
                                #region 工艺进程单
                                if (compont.ProcessScheduleUpTime != null)
                                {
                                    if (compont.CraftPersonID == personId && Convert.ToDateTime(compont.ProcessScheduleUpTime) >= start && Convert.ToDateTime(compont.ProcessScheduleUpTime) <= end)
                                    {
                                        var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                        if (oldPerson == null)
                                        {
                                            StatisticsInfo info = new StatisticsInfo()
                                            {
                                                PersonId = personId,
                                                PersonName = staff.StaffName,
                                                ProcessScheduleNumComplete = 1
                                            };
                                            infos.Add(info);
                                        }
                                        else
                                        {
                                            oldPerson.ProcessScheduleNumComplete++;
                                        }
                                    }
                                }
                                #endregion
                                var processes = model.JDJS_PDMS_Compont_ProcessInfo_Table.Where(r => r.CompontID == compont.ID);
                                foreach (var process in processes)
                                {
                                    #region 工装图纸
                                    if (process.ToolingDrawingsUploadIsNeed == 1 && process.ToolingDrawingsUploadIsComplete == 1)
                                    {
                                        if (process.ToolingDrawingsUploadCompleteTime != null)
                                        {
                                            if (process.ToolingDrawingsUploadPersonID == personId && Convert.ToDateTime(process.ToolingDrawingsUploadCompleteTime) >= start && Convert.ToDateTime(process.ToolingDrawingsUploadCompleteTime) <= end)
                                            {
                                                var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                                if (oldPerson == null)
                                                {
                                                    StatisticsInfo info = new StatisticsInfo()
                                                    {
                                                        PersonId = personId,
                                                        PersonName = staff.StaffName,
                                                        ToolingDrawingsNumComplete = 1
                                                    };
                                                    infos.Add(info);
                                                }
                                                else
                                                {
                                                    oldPerson.ToolingDrawingsNumComplete++;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 加工文件
                                    if (process.ProcessFileUploadIsComplete == 1)
                                    {
                                        if (process.ProcessFileUploadCompleteTime != null)
                                        {
                                            if (process.ProcessFileUploadPersonID == personId && Convert.ToDateTime(process.ProcessFileUploadCompleteTime) >= start && Convert.ToDateTime(process.ProcessFileUploadCompleteTime) <= end)
                                            {
                                                var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                                if (oldPerson == null)
                                                {
                                                    StatisticsInfo info = new StatisticsInfo()
                                                    {
                                                        PersonId = personId,
                                                        PersonName = staff.StaffName,
                                                        ProcessFileNumComplete = 1
                                                    };
                                                    infos.Add(info);
                                                }
                                                else
                                                {
                                                    oldPerson.ProcessFileNumComplete++;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        var tasks = model.JDJS_PDMS_TaskInfo_Table;
                        foreach (var task in tasks)
                        {
                            #region 零件创建
                            if (task.CreatPersonID == personId && Convert.ToDateTime(task.CreatTime) >= start && Convert.ToDateTime(task.CreatTime) <= end)
                            {
                                var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                if (oldPerson == null)
                                {
                                    StatisticsInfo info = new StatisticsInfo()
                                    {
                                        PersonId = personId,
                                        PersonName = staff.StaffName,
                                        CreatTaskNum = 1
                                    };
                                    infos.Add(info);
                                }
                                else
                                {
                                    oldPerson.CreatTaskNum++;
                                }
                            }
                            #endregion
                            #region 工艺过程卡
                            if (task.ProcessCardUploadTime != null)
                            {
                                if (task.CraftPersonID == personId && Convert.ToDateTime(task.ProcessCardUploadTime) >= start && Convert.ToDateTime(task.ProcessCardUploadTime) <= end)
                                {
                                    var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                    if (oldPerson == null)
                                    {
                                        StatisticsInfo info = new StatisticsInfo()
                                        {
                                            PersonId = personId,
                                            PersonName = staff.StaffName,
                                            ProcessCardNumComplete = 1
                                        };
                                        infos.Add(info);
                                    }
                                    else
                                    {
                                        oldPerson.ProcessCardNumComplete++;
                                    }
                                }
                            }
                            #endregion
                            #region 工艺进程单
                            if (task.ProcessScheduleUpTime != null)
                            {
                                if (task.CraftPersonID == personId && Convert.ToDateTime(task.ProcessScheduleUpTime) >= start && Convert.ToDateTime(task.ProcessScheduleUpTime) <= end)
                                {
                                    var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                    if (oldPerson == null)
                                    {
                                        StatisticsInfo info = new StatisticsInfo()
                                        {
                                            PersonId = personId,
                                            PersonName = staff.StaffName,
                                            ProcessScheduleNumComplete = 1
                                        };
                                        infos.Add(info);
                                    }
                                    else
                                    {
                                        oldPerson.ProcessScheduleNumComplete++;
                                    }
                                }
                            }
                            #endregion
                            var processes = model.JDJS_PDMS_Task_ProcessInfo_Table.Where(r => r.TaskID == task.ID);
                            foreach (var process in processes)
                            {
                                #region 工装图纸
                                if (process.ToolingDrawingsUploadIsNeed == 1 && process.ToolingDrawingsUploadIsComplete == 1)
                                {
                                    if (process.ToolingDrawingsUploadCompleteTime != null)
                                    {
                                        if (process.ToolingDrawingsUploadPersonID == personId && Convert.ToDateTime(process.ToolingDrawingsUploadCompleteTime) >= start && Convert.ToDateTime(process.ToolingDrawingsUploadCompleteTime) <= end)
                                        {
                                            var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                            if (oldPerson == null)
                                            {
                                                StatisticsInfo info = new StatisticsInfo()
                                                {
                                                    PersonId = personId,
                                                    PersonName = staff.StaffName,
                                                    ToolingDrawingsNumComplete = 1
                                                };
                                                infos.Add(info);
                                            }
                                            else
                                            {
                                                oldPerson.ToolingDrawingsNumComplete++;
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region 加工文件
                                if (process.ProcessFileUploadIsComplete == 1)
                                {
                                    if (process.ProcessFileUploadCompleteTime != null)
                                    {
                                        if (process.ProcessFileUploadPersonID == personId && Convert.ToDateTime(process.ProcessFileUploadCompleteTime) >= start && Convert.ToDateTime(process.ProcessFileUploadCompleteTime) <= end)
                                        {
                                            var oldPerson = infos.Where(r => r.PersonId == personId).FirstOrDefault();
                                            if (oldPerson == null)
                                            {
                                                StatisticsInfo info = new StatisticsInfo()
                                                {
                                                    PersonId = personId,
                                                    PersonName = staff.StaffName,
                                                    ProcessFileNumComplete = 1
                                                };
                                                infos.Add(info);
                                            }
                                            else
                                            {
                                                oldPerson.ProcessFileNumComplete++;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            infos = infos.OrderByDescending(r => r.ProcessCardNumComplete + r.ProcessFileNumComplete + r.ProcessScheduleNumComplete + r.ToolingDrawingsNumComplete).ToList () ;
            return infos;

        }
        
    }
    public class StatisticsInfo
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// 创建任务数量
        /// </summary>
        public int CreatTaskNum { get; set; }
        /// <summary>
        /// 工艺过程卡完成数量
        /// </summary>
        public int ProcessCardNumComplete { get; set; }
        /// <summary>
        /// 工艺过程卡未完成数量
        /// </summary>
        public int ProcessCardNumNotComplete { get; set; }
        /// <summary>
        /// 工艺进程单完成数量
        /// </summary>
        public int ProcessScheduleNumComplete { get; set; }
        /// <summary>
        /// 工艺进程单未完成数量
        /// </summary>
        public int ProcessScheduleNumNotComplete { get; set; }
        /// <summary>
        /// 工装图纸完成数量
        /// </summary>
        public int ToolingDrawingsNumComplete { get; set; }
        /// <summary>
        /// 工装图纸未完成数量
        /// </summary>
        public int ToolingDrawingsNumNotComplete { get; set; }
        /// <summary>
        /// 加工文件完成数量
        /// </summary>
        public int ProcessFileNumComplete { get; set; }
        /// <summary>
        /// 加工文件未完成数量
        /// </summary>
        public int ProcessFileNumNotComplete { get; set; }

    }
}