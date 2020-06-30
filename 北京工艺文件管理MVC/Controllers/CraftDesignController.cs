﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 北京工艺文件管理MVC.Database;
using 北京工艺文件管理MVC.Models;

namespace 北京工艺文件管理MVC.Controllers
{
    public class CraftDesignController : Controller
    {
        //
        // GET: /CraftDesign/

        public ActionResult Index()
        {
            using (Model1 model = new Model1())
            {
                JDJS_PDMS_Department_Table depart = new JDJS_PDMS_Department_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DepartmentName = "北京精雕科技集团",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ParentID = 0,
                    state = "正常"
                };
                model.JDJS_PDMS_Department_Table.Add(depart);
                JDJS_PDMS_Position_Table positio = new JDJS_PDMS_Position_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DepartmentID = 1,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    PositionName = "管理员",
                    state = "正常"
                };
                model.JDJS_PDMS_Position_Table.Add(positio);
                JDJS_PDMS_Staff_Table staff = new JDJS_PDMS_Staff_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    MailBox = "jingdiao@jd.com",
                    PassWord = "123456",
                    PosiationID = 1,
                    StaffName = "管理员",
                    state = "正常",
                    Tel = "00000000000",
                    Users = "admin"
                };
                model.JDJS_PDMS_Staff_Table.Add(staff);
                JDJS_PDMS_Task_Preview_Category_Table jd1 = new JDJS_PDMS_Task_Preview_Category_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ProcessingType = "车",
                    state = "正常"

                };
                model.JDJS_PDMS_Task_Preview_Category_Table.Add(jd1);
                JDJS_PDMS_Task_Preview_Category_Table jd2 = new JDJS_PDMS_Task_Preview_Category_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ProcessingType = "钳",
                    state = "正常"

                };
                model.JDJS_PDMS_Task_Preview_Category_Table.Add(jd2);
                JDJS_PDMS_Task_Preview_Category_Table jd3 = new JDJS_PDMS_Task_Preview_Category_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ProcessingType = "磨",
                    state = "正常"

                };
                model.JDJS_PDMS_Task_Preview_Category_Table.Add(jd3);

                JDJS_PDMS_Compont_Preview_Category_Table jd4 = new JDJS_PDMS_Compont_Preview_Category_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ProcessingType = "车",
                    state = "正常"

                };
                model.JDJS_PDMS_Compont_Preview_Category_Table.Add(jd4);
                JDJS_PDMS_Compont_Preview_Category_Table jd5 = new JDJS_PDMS_Compont_Preview_Category_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ProcessingType = "钳",
                    state = "正常"

                };
                model.JDJS_PDMS_Compont_Preview_Category_Table.Add(jd5);
                JDJS_PDMS_Compont_Preview_Category_Table jd6 = new JDJS_PDMS_Compont_Preview_Category_Table()
                {
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    ProcessingType = "磨",
                    state = "正常"

                };
                model.JDJS_PDMS_Compont_Preview_Category_Table.Add(jd6);


                JDJS_PDMS_Limits_Of_Authority_Table limlt1 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "工艺文件管理系统",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺文件管理系统",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 0,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt1);

                JDJS_PDMS_Limits_Of_Authority_Table limlt2 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "工艺",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 1,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt2);

                JDJS_PDMS_Limits_Of_Authority_Table limlt3 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "任务创建",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺_任务创建",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 2,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt3);
                JDJS_PDMS_Limits_Of_Authority_Table limlt4 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "任务进度",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺_任务进度",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 2,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt4);
                JDJS_PDMS_Limits_Of_Authority_Table limlt5 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "工艺设计",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺_工艺设计",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 2,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt5);
                JDJS_PDMS_Limits_Of_Authority_Table limlt6 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "任务预览",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺_任务预览",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 2,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt6);

                JDJS_PDMS_Limits_Of_Authority_Table limlt7 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "人员",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "人员",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 1,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt7);
                JDJS_PDMS_Limits_Of_Authority_Table limlt8 = new JDJS_PDMS_Limits_Of_Authority_Table()
                {
                    AuthorityName = "工艺安排",
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    DisPlayName = "工艺_工艺安排",
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    state = "正常",
                    ParentID = 2,
                };
                model.JDJS_PDMS_Limits_Of_Authority_Table.Add(limlt8);

                JDJS_PDMS_Position_Authority_Table limlt9 = new JDJS_PDMS_Position_Authority_Table()
                {
                    AuthorityID = 7,
                    CreatPersonID = 1,
                    CreatTime = DateTime.Now,
                    LastAlterPersonID = 1,
                    LastAlterTime = DateTime.Now,
                    PositionID = 1,
                    state = "正常"

                };
                model.JDJS_PDMS_Position_Authority_Table.Add(limlt9);
                model.SaveChanges();
            }
            return View();
        }

        /// <summary>
        /// 获取产品组件零件信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfo()
        {
            try
            {
                int craftPersonID = -1;
                if (int.TryParse(Session["id"].ToString(), out craftPersonID))
                {
                    if (Session["processTypeStr"].ToString() == "")
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            var list = CraftDesignInfoManage.GetTaskInfoByCreaftPerson(da,0, "", craftPersonID);
                            return Json(list, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Content(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            var list = CraftDesignInfoManage.GetTaskInfoByGroup(da,0, "", craftPersonID);

                            return Json(list, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Content(ex.Message);
                        }
                    }
                }
                else
                {
                    return Content("系统异常，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 获取产品组件零件信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfoCreate()
        {
            try
            {
                int craftPersonID = -1;
                if (int.TryParse(Session["id"].ToString(), out craftPersonID))
                {
                    if (Session["processTypeStr"].ToString() == "")
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            var list = CraftDesignInfoManage.GetTaskInfoByCreaftPersonCreate(da, 0, "", craftPersonID);
                            return Json(list, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Content(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            var list = CraftDesignInfoManage.GetTaskInfoByGroupCreate(da, 0, "", craftPersonID);

                            return Json(list, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Content(ex.Message);
                        }
                    }
                }
                else
                {
                    return Content("系统异常，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 根据登录人员获取该人员负责的零件任务
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfoByCraftPerson()
        {
            try
            {
                int craftPersonID = -1;
                if (int.TryParse(Session["id"].ToString(), out craftPersonID))
                {
                    DatabaseList da = DatabaseList.GetData();
                    var list = CraftDesignInfoManage.GetTaskInfoByCreaftPerson(da,0, "", craftPersonID);
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("系统异常，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 根据登录人员获取该人员负责的零件任务
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfoByCraftPersonCreate()
        {
            try
            {
                int craftPersonID = -1;
                if (int.TryParse(Session["id"].ToString(), out craftPersonID))
                {
                    DatabaseList da = DatabaseList.GetData();
                    var list = CraftDesignInfoManage.GetTaskInfoByCreaftPersonCreate(da, 0, "", craftPersonID);
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("系统异常，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        /// <summary>
        /// 预览组别查看信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfoByPreview_Category()
        {
            try
            {
                int craftPersonID = -1;
                if (int.TryParse(Session["id"].ToString(), out craftPersonID))
                {
                    DatabaseList da = DatabaseList.GetData();
                    var list = CraftDesignInfoManage.GetTaskInfoByPreview_Category(da,0, "", craftPersonID);
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("系统异常，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 预览组别查看信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfoByPreview_CategoryCreate()
        {
            try
            {
                int craftPersonID = -1;
                if (int.TryParse(Session["id"].ToString(), out craftPersonID))
                {
                    DatabaseList da = DatabaseList.GetData();
                    var list = CraftDesignInfoManage.GetTaskInfoByPreview_CategoryCreate(da, 0, "", craftPersonID);
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("系统异常，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 接收多个任务,返回的是一个字符串列表序列化的字符串，包含每个任务接收情况，成功为ok,失败有错误信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AcceptTaskMethodByCraftPerson()
        {
            try
            {
                var taskIdStr = Request["taskIdList"];//接收的任务ID列表,主键ID以英文半角逗号,隔开
                List<string> taskIdStrList = taskIdStr.Split(',').ToList();
                List<int> taskId = new List<int>();
                foreach (var item in taskIdStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        taskId.Add(_int);
                    }
                }
                List<string> result = new List<string>();
                int personId = -1;
                if (int.TryParse(Session["id"].ToString(), out personId))
                {
                    foreach (var item in taskId)
                    {
                        string errMsg = "";
                        if (CraftDesignInfoManage.AcceptTask(item, personId, ref errMsg))
                        {
                            result.Add(errMsg);
                        }
                        else
                        {
                            result.Add(errMsg);
                        }
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 工艺责任人创建工序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMachiningProcessByCraftPerson()
        {
            //CreateMachiningProcess(int taskID, int processNum, int createPersonID, int processTypeID, ref string errMsg
            try
            {
                var taskIDStr = Request["taskId"];//创建工序的零件ID
                var processNumStr = Request["processNum"];//工序号
                var processTypeIDStr = Request["processTypeId"];//工序类型ID，车 镗 磨
                var isNeed = Request["isNeed"];//需要 or 不需要 工装图纸
                bool flag = false;
                if (isNeed == "需要")
                {
                    flag = true;
                }
                var personIDStr = Session["id"];
                int taskId = -1;
                int processNum = -1;
                int processTypeId = -1;
                int personId = -1;
                if (int.TryParse(taskIDStr, out taskId) && int.TryParse(processNumStr, out processNum) && int.TryParse(processTypeIDStr, out processTypeId) && int.TryParse(personIDStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.CreateMachiningProcess(taskId, processNum, flag, personId, processTypeId, ref errMsg))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("创建失败," + errMsg);
                    }
                }
                else
                {
                    return Content("创建失败，请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 获取零件任务工序信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartsProcessInfoByPartTask()
        {
            try
            {
                var taskIdStr = Request["taskId"];//零件任务ID
                int taskId = -1;
                if (int.TryParse(taskIdStr, out taskId))
                {
                    var lisrResult = CraftDesignInfoManage.GetPartsProcessInfo(taskId);
                    return Json(new { code = 0, data = lisrResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");

                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 删除工序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePartsProcessMethod()
        {
            try
            {
                var processIdStr = Request["processId"];//工序主键ID
                var personIdStr = Session["id"];
                int processId = -1;
                int personId = -1;
                if (int.TryParse(processIdStr, out processId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.DeletePartsProcess(processId, personId, ref errMsg))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 修改工序信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterPartsProcessMethod()
        {
            try
            {
                var processIdStr = Request["processId"];//修改的工序主键ID
                var processNumStr = Request["processNum"];//修改的工序号
                var isNeed = Request["isNeed"];//是否需要工装图纸  需要   or  不需要
                var processTypeIdStr = Request["processTypeId"];//加工类型  车  磨  镗  主键ID
                var personStr = Session["id"];
                int processId = -1;
                int processTypeId = -1;
                int personId = -1;
                int processNum = -1;
                bool flag = false;
                if (isNeed == "需要")
                {
                    flag = true;
                }
                if (int.TryParse(processIdStr, out processId) && int.TryParse(processTypeIdStr, out processTypeId) && int.TryParse(personStr.ToString(), out personId) && int.TryParse(processNumStr, out processNum))
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.AlterPartsProcess(processId, flag, personId, processNum, processTypeId, ref errMsg))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }


                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        /// <summary>
        /// 上传工艺过程卡，工艺进程单，其它文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveTaskCraftFile()
        {
            var taskIdStr = Request["taskId"];//零件任务主键id
            var files = Request.Files;//文件
            var flag = Request["flag"];//文件标志位，”工艺过程卡“或者“工艺进程单”或者“其它文件”

            var personIdStr = Session["id"];//上传文件人员Id 
            int taskId = -1;
            int personId = -1;
            if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
            {
                if (flag.ToString() == "工艺过程卡")
                {
                    string errMsg = "";
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺过程卡");
                    if (files != null)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            var fileNames = Directory.GetFiles(dirPath);
                            foreach (var item in fileNames)
                            {
                                FileInfo info = new FileInfo(item);
                                for (int i = 0; i < files.Count; i++)
                                {
                                    if (files[i].FileName == info.Name)
                                    {
                                        return Content("已经存在");
                                    }
                                }
                            }
                        }
                    }
                    if (CraftDesignInfoManage.UpdateTaskCraftFiles(taskId, personId, 0, ref errMsg))
                    {

                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        for (int i = 0; i < files.Count; i++)
                        {
                            var filePath = Path.Combine(dirPath, files[i].FileName);
                            files[i].SaveAs(filePath);
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else if (flag.ToString() == "工艺进程单")
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺进程单");
                    if (files != null)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            var fileNames = Directory.GetFiles(dirPath);
                            foreach (var item in fileNames)
                            {
                                FileInfo info = new FileInfo(item);
                                for (int i = 0; i < files.Count; i++)
                                {
                                    if (files[i].FileName == info.Name)
                                    {
                                        return Content("已经存在");
                                    }
                                }
                            }
                        }
                    }
                    string errMsg = "";
                    if (CraftDesignInfoManage.UpdateTaskCraftFiles(taskId, personId, 1, ref errMsg))
                    {

                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        for (int i = 0; i < files.Count; i++)
                        {
                            var filePath = Path.Combine(dirPath, files[i].FileName);
                            files[i].SaveAs(filePath);
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else if (flag.ToString() == "其它文件")
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.UpdateTaskCraftFiles(taskId, personId, 2, ref errMsg))
                    {
                        SupplementaryInformation pathInfo = new SupplementaryInformation();
                        var pathTask = TaskInfoManage.GetTaskPath(taskId);
                        var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "其它文件");
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        for (int i = 0; i < files.Count; i++)
                        {
                            var filePath = Path.Combine(dirPath, files[i].FileName);
                            files[i].SaveAs(filePath);
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else
                {
                    return Content("请输入正确的文件！");
                }
            }
            else
            {
                return Content("请输入正确的Int数据类型！");
            }
        }



        /// <summary>
        /// 上传工艺过程卡，工艺进程单，其它文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveTaskCraftFileExict()
        {
            var taskIdStr = Request["taskId"];//零件任务主键id
            var files = Request.Files;//文件
            var flag = Request["flag"];//文件标志位，”工艺过程卡“或者“工艺进程单”或者“其它文件”
            var saveFlag = Request["saveFlag"];//保存标志位   0：覆盖  1：升级


            var personIdStr = Session["id"];//上传文件人员Id 
            int taskId = -1;
            int personId = -1;
            if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
            {
                if (flag.ToString() == "工艺过程卡")
                {
                    string errMsg = "";
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺过程卡");
                    if (CraftDesignInfoManage.UpdateTaskCraftFiles(taskId, personId, 0, ref errMsg))
                    {

                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        if (saveFlag == "0")//.覆盖
                        {
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(dirPath, files[i].FileName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    FileInfo info = new FileInfo(filePath);
                                    int index = info.Name.LastIndexOf('.');
                                    int j = 1;
                                    string newpath = "";
                                    while (true)
                                    {
                                        j++;
                                        var newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                        newpath = Path.Combine(dirPath, newname);
                                        if (!System.IO.File.Exists(newpath))
                                        {
                                            j--;
                                            if (j == 1)
                                            {
                                                newname = info.Name;
                                                newpath = Path.Combine(dirPath, newname);
                                            }
                                            else
                                            {
                                                newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                                newpath = Path.Combine(dirPath, newname);
                                            }
                                            break;
                                        }
                                    }
                                    files[i].SaveAs(newpath);
                                }
                                else
                                {
                                    files[i].SaveAs(filePath);
                                }
                                files[i].SaveAs(filePath);
                            }
                        }
                        else if (saveFlag == "1")//升级
                        {
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(dirPath, files[i].FileName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    FileInfo info = new FileInfo(filePath);
                                    int index = info.Name.LastIndexOf('.');
                                    int j = 1;
                                    string newpath = "";
                                    while (true)
                                    {
                                        j++;
                                        var newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                        newpath = Path.Combine(dirPath, newname);
                                        if (!System.IO.File.Exists(newpath))
                                        {
                                            break;
                                        }
                                    }
                                    files[i].SaveAs(newpath);
                                }
                                else
                                {
                                    files[i].SaveAs(filePath);
                                }
                                files[i].SaveAs(filePath);
                            }
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else if (flag.ToString() == "工艺进程单")
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺进程单");

                    string errMsg = "";
                    if (CraftDesignInfoManage.UpdateTaskCraftFiles(taskId, personId, 1, ref errMsg))
                    {

                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        if (saveFlag == "0")//.覆盖
                        {
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(dirPath, files[i].FileName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    FileInfo info = new FileInfo(filePath);
                                    int index = info.Name.LastIndexOf('.');
                                    int j = 1;
                                    string newpath = "";
                                    while (true)
                                    {
                                        j++;
                                        var newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                        newpath = Path.Combine(dirPath, newname);
                                        if (!System.IO.File.Exists(newpath))
                                        {
                                            j--;
                                            if (j == 1)
                                            {
                                                newname = info.Name;
                                                newpath = Path.Combine(dirPath, newname);
                                            }
                                            else
                                            {
                                                newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                                newpath = Path.Combine(dirPath, newname);
                                            }
                                            break;
                                        }
                                    }
                                    files[i].SaveAs(newpath);
                                }
                                else
                                {
                                    files[i].SaveAs(filePath);
                                }
                                files[i].SaveAs(filePath);
                            }
                        }
                        else if (saveFlag == "1")//升级
                        {
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(dirPath, files[i].FileName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    FileInfo info = new FileInfo(filePath);
                                    int index = info.Name.LastIndexOf('.');
                                    int j = 1;
                                    string newpath = "";
                                    while (true)
                                    {
                                        j++;
                                        var newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                        newpath = Path.Combine(dirPath, newname);
                                        if (!System.IO.File.Exists(newpath))
                                        {
                                            break;
                                        }
                                    }
                                    files[i].SaveAs(newpath);
                                }
                                else
                                {
                                    files[i].SaveAs(filePath);
                                }
                                files[i].SaveAs(filePath);
                            }
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else if (flag.ToString() == "其它文件")
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.UpdateTaskCraftFiles(taskId, personId, 2, ref errMsg))
                    {
                        SupplementaryInformation pathInfo = new SupplementaryInformation();
                        var pathTask = TaskInfoManage.GetTaskPath(taskId);
                        var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "其它文件");
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        for (int i = 0; i < files.Count; i++)
                        {
                            var filePath = Path.Combine(dirPath, files[i].FileName);
                            files[i].SaveAs(filePath);
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else
                {
                    return Content("请输入正确的文件！");
                }
            }
            else
            {
                return Content("请输入正确的Int数据类型！");
            }
        }

        /// <summary>
        /// 获取工艺过程卡，工艺进程单，其它文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReadTaskCraftFile()
        {

            var taskIdStr = Request["taskId"];//零件任务主键id
            var flag = Request["flag"];//文件标志位，”工艺过程卡“或者“工艺进程单”或者“其它文件”


            var personIdStr = Session["id"];//上传文件人员Id 
            int taskId = -1;
            int personId = -1;
            if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
            {
                if (flag.ToString() == "工艺过程卡")
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺过程卡");
                    List<FileInfoClass> fileInfos = new List<FileInfoClass>();
                    List<string> filesPath = new List<string>();
                    if (Directory.Exists(dirPath))
                    {
                        filesPath = Directory.GetFiles(dirPath).ToList();
                        foreach (var item in filesPath)
                        {
                            FileInfo fileinfo = new FileInfo(item);
                            FileInfoClass file = new FileInfoClass();
                            file.fileName = fileinfo.Name;
                            file.updateTime = fileinfo.LastWriteTime;
                            file.updateTimeStr = fileinfo.LastWriteTime.ToString().Substring(0, fileinfo.LastWriteTime.ToString().LastIndexOf(':'));
                            file.downloadPath = Path.Combine("JDJS_PDMS_Files", pathTask, "工艺过程卡", fileinfo.Name);
                            fileInfos.Add(file);

                        }
                    }
                    fileInfos = fileInfos.OrderByDescending(r => r.updateTime).ToList();
                    return Json(new { code = 0, data = fileInfos }, JsonRequestBehavior.AllowGet);

                }
                else if (flag.ToString() == "工艺进程单")
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺进程单");
                    List<FileInfoClass> fileInfos = new List<FileInfoClass>();
                    List<string> filesPath = new List<string>();
                    if (Directory.Exists(dirPath))
                    {
                        filesPath = Directory.GetFiles(dirPath).ToList();
                        foreach (var item in filesPath)
                        {
                            FileInfo fileinfo = new FileInfo(item);
                            FileInfoClass file = new FileInfoClass();
                            file.fileName = fileinfo.Name;
                            file.updateTime = fileinfo.LastWriteTime;
                            file.updateTimeStr = fileinfo.LastWriteTime.ToString().Substring(0, fileinfo.LastWriteTime.ToString().LastIndexOf(':'));
                            file.downloadPath = Path.Combine("JDJS_PDMS_Files", pathTask, "工艺进程单", fileinfo.Name);
                            fileInfos.Add(file);

                        }
                    }
                    fileInfos = fileInfos.OrderByDescending(r => r.updateTime).ToList();
                    return Json(new { code = 0, data = fileInfos }, JsonRequestBehavior.AllowGet);

                }
                else if (flag.ToString() == "其它文件")
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "其它文件");
                    List<FileInfoClass> fileInfos = new List<FileInfoClass>();
                    List<string> filesPath = new List<string>();
                    if (Directory.Exists(dirPath))
                    {
                        filesPath = Directory.GetFiles(dirPath).ToList();
                        foreach (var item in filesPath)
                        {
                            FileInfo fileinfo = new FileInfo(item);
                            FileInfoClass file = new FileInfoClass();
                            file.fileName = fileinfo.Name;
                            file.updateTime = fileinfo.LastWriteTime;
                            file.updateTimeStr = fileinfo.LastWriteTime.ToString().Substring(0, fileinfo.LastWriteTime.ToString().LastIndexOf(':'));
                            file.downloadPath = Path.Combine("JDJS_PDMS_Files", pathTask, "其它文件", fileinfo.Name);
                            fileInfos.Add(file);

                        }
                    }
                    fileInfos = fileInfos.OrderByDescending(r => r.updateTime).ToList();
                    return Json(new { code = 0, data = fileInfos }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Content("请输入正确的文件！");
                }
            }
            else
            {
                return Content("请输入正确的Int数据类型！");
            }

        }

        /// <summary>
        /// 工艺过程卡提交 与接收任务相同
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessSubmitByCraftPersonMethod()
        {
            try
            {
                var taskIdStr = Request["taskIdList"];//接收的任务ID列表,主键ID以英文半角逗号,隔开
                List<string> taskIdStrList = taskIdStr.Split(',').ToList();
                List<int> taskId = new List<int>();
                foreach (var item in taskIdStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        taskId.Add(_int);
                    }
                }
                List<string> result = new List<string>();
                var personIdStr = Session["id"];
                int personId = -1;
                if (int.TryParse(personIdStr.ToString(), out personId))
                {
                    foreach (var item in taskId)
                    {
                        string errMsg = "";
                        if (CraftDesignInfoManage.ProcessSubmitByCraftPerson(item, personId, ref errMsg))
                        {
                            result.Add(errMsg);
                        }
                        else
                        {
                            result.Add(errMsg);
                        }
                    }

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// 删除工艺文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTaskCraftFile()
        {
            try
            {
                var taskIdStr = Request["taskId"];//零件任务主键id
                var flag = Request["flag"];//文件标志位，”工艺过程卡“或者“工艺进程单”或者“其它文件”
                var fileNames = Request["fileNames"];//需要删除的文件名称列表

                var personIdStr = Session["id"];
                List<string> fileName = fileNames.Split(',').ToList();
                int taskId = -1;
                int personId = -1;
                string errMsg = "";
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    pathTask = Path.Combine(pathInfo.upLoadPath(), pathTask);
                    if (flag == "工艺过程卡")
                    {
                        if (CraftDesignInfoManage.DeleteTaskCrafeFiles(taskId, personId, 0, pathTask + @"\工艺过程卡\", fileName, ref errMsg))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                    else if (flag == "工艺进程单")
                    {
                        if (CraftDesignInfoManage.DeleteTaskCrafeFiles(taskId, personId, 1, pathTask + @"\工艺进程单\", fileName, ref errMsg))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                    else if (flag == "其它文件")
                    {
                        if (CraftDesignInfoManage.DeleteTaskCrafeFiles(taskId, personId, 2, pathTask + @"\其它文件\", fileName, ref errMsg))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                    else
                    {
                        return Content("请输入正确的文件类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }




            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 获取工序的文件  文件类型标志  “工装图纸”  “加工程序”  “其它文件”“刀具表”
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProcessFileListMethod()
        {
            try
            {
                var processIdStr = Request["processId"];
                var fileFlag = Request["fileFlag"];//文件类型标志  “工装图纸”  “加工程序”  “其它文件”“刀具表”
                int processId = -1;
                if (int.TryParse(processIdStr, out processId))
                {
                    int fileflag = -1;
                    switch (fileFlag)
                    {
                        case "工装图纸":
                            fileflag = 0;
                            break;
                        case "加工程序":
                            fileflag = 1;
                            break;
                        case "其它文件":
                            fileflag = 2;
                            break;
                        case "刀具表":
                            fileflag = 3;
                            break;
                        default:
                            fileflag = -1;
                            break;
                    }
                    SupplementaryInformation supplePath = new SupplementaryInformation();
                    string path1 = CraftDesignInfoManage.GetFilePathByProcessId(processId, fileflag);
                    if (path1 != "")
                    {
                        string path = Path.Combine(supplePath.upLoadPath(), path1);
                        List<FileInfoClass> fileInfos = new List<FileInfoClass>();
                        if (Directory.Exists(path))
                        {
                            var files = Directory.GetFiles(path);
                            foreach (var item in files)
                            {
                                FileInfo fileinfo = new FileInfo(item);
                                FileInfoClass file = new FileInfoClass();
                                file.processId = processId;
                                file.fileName = fileinfo.Name;
                                file.updateTime = fileinfo.LastWriteTime;
                                file.updateTimeStr = fileinfo.LastWriteTime.ToString().Substring(0, fileinfo.LastWriteTime.ToString().LastIndexOf(':'));
                                file.downloadPath = Path.Combine("JDJS_PDMS_Files", path1, fileinfo.Name);
                                fileInfos.Add(file);
                            }
                        }
                        fileInfos = fileInfos.OrderByDescending(r => r.updateTime).ToList();
                        return Json(new { code = 0, data = fileInfos }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Content("系统异常！");
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 上传工艺文件 文件类型标志  “工装图纸”  “加工程序”  “其它文件” “刀具表”
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadProcessFileMethod()
        {
            try
            {
                var processIdStr = Request["processId"];//工序Id
                var fileFlag = Request["fileFlag"];//文件类型标志  “工装图纸”  “加工程序”  “其它文件” “刀具表”
                var files = Request.Files;//文件
                int processId = -1;
                var personIdStr = Session["id"];
                int personId = -1;
                if (int.TryParse(processIdStr, out processId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    int fileflag = -1;
                    string errMsg = "";
                    switch (fileFlag)
                    {
                        case "工装图纸":
                            fileflag = 0;
                            break;
                        case "加工程序":
                            fileflag = 1;
                            break;
                        case "其它文件":
                            fileflag = 2;
                            break;
                        case "刀具表":
                            fileflag = 3;
                            break;
                        default:
                            fileflag = -1;
                            break;
                    }
                    if (fileflag == 0 && files != null)
                    {
                        SupplementaryInformation pathInfo = new SupplementaryInformation();
                        string path1 = CraftDesignInfoManage.GetFilePathByProcessId(processId, fileflag);
                        string dirPath = Path.Combine(pathInfo.upLoadPath(), path1);
                        if (files != null)
                        {
                            if (Directory.Exists(dirPath))
                            {
                                var fileNames = Directory.GetFiles(dirPath);
                                foreach (var item in fileNames)
                                {
                                    FileInfo info = new FileInfo(item);
                                    for (int i = 0; i < files.Count; i++)
                                    {
                                        if (files[i].FileName == info.Name)
                                        {
                                            return Content("已经存在");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (CraftDesignInfoManage.UploadProcessFile(processId, fileflag, personId, ref errMsg))
                    {
                        SupplementaryInformation supplePath = new SupplementaryInformation();
                        string path1 = CraftDesignInfoManage.GetFilePathByProcessId(processId, fileflag);
                        if (path1 != "")
                        {
                            string path = Path.Combine(supplePath.upLoadPath(), path1);
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(path, files[i].FileName);
                                files[i].SaveAs(filePath);
                            }
                            return Content("ok");
                        }
                        else
                        {
                            return Content("请输入正确的文件类型！");
                        }
                    }
                    else
                    {
                        return Content(errMsg);
                    }


                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 上传工艺文件 文件类型标志  “工装图纸”  “加工程序”  “其它文件” “刀具表”
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadProcessFileMethodExict()
        {
            try
            {
                var processIdStr = Request["processId"];//工序Id
                var fileFlag = Request["fileFlag"];//文件类型标志  “工装图纸”  “加工程序”  “其它文件” “刀具表”
                var saveFlag = Request["saveFlag"];//保存标志位   0：覆盖  1：升级
                var files = Request.Files;//文件
                int processId = -1;
                var personIdStr = Session["id"];
                int personId = -1;
                if (int.TryParse(processIdStr, out processId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    int fileflag = -1;
                    string errMsg = "";
                    switch (fileFlag)
                    {
                        case "工装图纸":
                            fileflag = 0;
                            break;
                        case "加工程序":
                            fileflag = 1;
                            break;
                        case "其它文件":
                            fileflag = 2;
                            break;
                        case "刀具表":
                            fileflag = 3;
                            break;
                        default:
                            fileflag = -1;
                            break;
                    }
                    if (CraftDesignInfoManage.UploadProcessFile(processId, fileflag, personId, ref errMsg))
                    {
                        SupplementaryInformation supplePath = new SupplementaryInformation();
                        string path1 = CraftDesignInfoManage.GetFilePathByProcessId(processId, fileflag);
                        if (path1 != "")
                        {
                            string path = Path.Combine(supplePath.upLoadPath(), path1);
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            if (saveFlag == "0")//.覆盖
                            {
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var filePath = Path.Combine(path, files[i].FileName);
                                    if (System.IO.File.Exists(filePath))
                                    {
                                        FileInfo info = new FileInfo(filePath);
                                        int index = info.Name.LastIndexOf('.');
                                        int j = 1;
                                        string newpath = "";
                                        while (true)
                                        {
                                            j++;
                                            var newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                            newpath = Path.Combine(path, newname);
                                            if (!System.IO.File.Exists(newpath))
                                            {
                                                j--;
                                                if (j == 1)
                                                {
                                                    newname = info.Name;
                                                    newpath = Path.Combine(path, newname);
                                                }
                                                else
                                                {
                                                    newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                                    newpath = Path.Combine(path, newname);
                                                }
                                                break;
                                            }
                                        }
                                        files[i].SaveAs(newpath);
                                    }
                                    else
                                    {
                                        files[i].SaveAs(filePath);
                                    }
                                    files[i].SaveAs(filePath);
                                }
                            }
                            else if (saveFlag == "1")//升级
                            {
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var filePath = Path.Combine(path, files[i].FileName);
                                    if (System.IO.File.Exists(filePath))
                                    {
                                        FileInfo info = new FileInfo(filePath);
                                        int index = info.Name.LastIndexOf('.');
                                        int j = 1;
                                        string newpath = "";
                                        while (true)
                                        {
                                            j++;
                                            var newname = info.Name.Substring(0, index) + "-V" + j.ToString() + info.Name.Substring(index);
                                            newpath = Path.Combine(path, newname);
                                            if (!System.IO.File.Exists(newpath))
                                            {
                                                break;
                                            }
                                        }
                                        files[i].SaveAs(newpath);
                                    }
                                    else
                                    {
                                        files[i].SaveAs(filePath);
                                    }
                                    files[i].SaveAs(filePath);
                                }
                            }

                            //for (int i = 0; i < files.Count; i++)
                            //{
                            //    var filePath = Path.Combine(path, files[i].FileName);
                            //    files[i].SaveAs(filePath);
                            //}
                            return Content("ok");
                        }
                        else
                        {
                            return Content("请输入正确的文件类型！");
                        }
                    }
                    else
                    {
                        return Content(errMsg);
                    }


                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        /// <summary>
        /// 删除工序文件 文件类型标志  “工装图纸”  “加工程序”  “其它文件” “刀具表”
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProcessFile()
        {
            try
            {
                var processIdStr = Request["processId"];//工序任务主键id
                var flag = Request["flag"];//文件标志位，//文件类型标志  “工装图纸”  “加工程序”  “其它文件” “刀具表”
                var fileNames = Request["fileNames"];//需要删除的文件名称列表

                var personIdStr = Session["id"];
                List<string> fileName = fileNames.Split(',').ToList();
                int processId = -1;
                int personId = -1;
                string errMsg = "";
                if (int.TryParse(processIdStr, out processId) && int.TryParse(personIdStr.ToString(), out personId))
                {

                    int fileflag = -1;
                    switch (flag)
                    {
                        case "工装图纸":
                            fileflag = 0;
                            break;
                        case "加工程序":
                            fileflag = 1;
                            break;
                        case "其它文件":
                            fileflag = 2;
                            break;
                        case "刀具表":
                            fileflag = 3;
                            break;
                        default:
                            fileflag = -1;
                            break;
                    }

                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = CraftDesignInfoManage.GetFilePathByProcessId(processId, fileflag);
                    pathTask = Path.Combine(pathInfo.upLoadPath(), pathTask);

                    if (CraftDesignInfoManage.DeleteProcessFiles(processId, personId, fileflag, pathTask, fileName, ref errMsg))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }




            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 完成任务提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CompleteTaskSubmitMethod()
        {
            try
            {
                var taskIdStr = Request["taskIdList"];//接收的任务ID列表,主键ID以英文半角逗号,隔开
                List<string> taskIdStrList = taskIdStr.Split(',').ToList();
                List<int> taskId = new List<int>();
                foreach (var item in taskIdStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        taskId.Add(_int);
                    }
                }
                List<string> result = new List<string>();
                var personIdStr = Session["id"];
                int personId = -1;
                if (int.TryParse(personIdStr.ToString(), out personId))
                {
                    foreach (var item in taskId)
                    {
                        string errMsg = "";
                        if (CraftDesignInfoManage.CompleteTaskSubmit(item, personId, ref errMsg))
                        {
                            result.Add(errMsg);
                        }
                        else
                        {
                            result.Add(errMsg);
                        }
                    }

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 安排工序人员获取零件任务工序信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartsProcessInfoByPartTaskAndPerson()
        {
            try
            {
                var taskIdStr = Request["taskId"];//零件任务ID
                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    var lisrResult = CraftDesignInfoManage.GetPartsProcessInfoByPersonGroup(taskId, personId);
                    return Json(new { code = 0, data = lisrResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");

                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 获取可以接收工序任务的所有员工
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTypePersonByProcess()
        {
            try
            {
                var processIdStr = Request["processId"];//零件任务ID
                //GetStaffInfoByProcessWorkType(int processId)
                int processId = -1;
                if (int.TryParse(processIdStr, out processId))
                {
                    var list = CraftDesignInfoManage.GetStaffInfoByProcessWorkType(processId);
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 工艺安排负责人提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArrangeProcessPerson()
        {
            try
            {
                var processIdStr = Request["processId"];//工序Id
                var staffIdStr = Request["staffId"];//员工Id
                var personIdStr = Session["id"];
                int processId = -1;
                int staffId = -1;
                int personId = -1;
                if (int.TryParse(processIdStr, out processId) && int.TryParse(staffIdStr, out staffId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.ArrangeProcessPerson(processId, personId, staffId, ref errMsg))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            //ArrangeProcessPerson(int processId, int personId, int staffId, ref string errMsg
        }
        /// <summary>
        /// 改变工序文件状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeProcessFileUploadStatus()
        {
            try
            {
                var processIdStr = Request["processId"];
                var fileFlagStr = Request["fileFlag"];//文件标志位    ”工装图纸“或者 “加工程序”
                var isCompleteStr = Request["isComplete"];//是否完成   “0”未完成    “1”完成
                var personIdStr = Session["id"];
                int processId = -1;
                int fileFlag = -1;
                bool isComplete = false;
                int personId = -1;
                if (fileFlagStr == "工装图纸")
                {
                    fileFlag = 0;
                }
                else if (fileFlagStr == "加工程序")
                {
                    fileFlag = 1;
                }
                if (isCompleteStr == "1")
                {
                    isComplete = true;
                }
                if (int.TryParse(processIdStr, out processId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (CraftDesignInfoManage.ChangeProcessCompleteStatus(processId, personId, fileFlag, isComplete, ref errMsg))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content(errMsg);
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }


                //ChangeProcessCompleteStatus(int processId, int personId, int fileFlag, bool isComplete, ref string errMsg
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 创建工序通过解析文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatProcessByFile()
        {
            try
            {
                var taskIdStr = Request["taskId"];
                var personIdStr = Session["id"];
                var file = Request.Files;
                int taskId = 0;
                int personId = 0;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "工艺文件");
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    if (!Directory.Exists(Path.Combine(dirPath, file[0].FileName)))
                    {
                        file[0].SaveAs(Path.Combine(dirPath, file[0].FileName));
                        if (CraftDesignInfoManage.ReadProcessFile(Path.Combine(dirPath, file[0].FileName), taskId, personId, ref errMsg))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(Path.Combine(dirPath, file[0].FileName));
                        file[0].SaveAs(Path.Combine(dirPath, file[0].FileName));
                        if (CraftDesignInfoManage.ReadProcessFile(Path.Combine(dirPath, file[0].FileName), taskId, personId, ref errMsg))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                }
                else
                {
                    return Content("请输入正确的Int数据类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
