﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 北京工艺文件管理MVC.Models;

namespace 北京工艺文件管理MVC.Controllers
{
    public class TaskController : Controller
    {
        //
        // GET: /Task/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取所有任务信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfo()
        {
            DatabaseList da = DatabaseList.GetData();
            return Json(CompontInfoManage.GetTaskInfo(da,0, ""), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有任务信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskInfoCreate()
        {
            DatabaseList da = DatabaseList.GetData(); 
            var list = CompontInfoManage.GetTaskInfoCreate(da,0, "");
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取下载组件文件的文件夹，下载后需要删除
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFileDownLoadPath()
        {
            try
            {
                var str = Request["id"];//组件ID，必须是组件id
                string path = "";
                int compantId=-1;
                if (int.TryParse(str, out compantId))
                {
                    if (TaskInfoManage.DownLoadToolDrawFiles(compantId, ref path))
                    {
                        var model = new { flag = true, res = path };
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        return Json(json);
                    }
                    else
                    {
                        var model = new { flag = false, res = path };
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        return Json(json);
                    }
                }
                else
                {
                    var model = new { flag = false, res = "请输入正确的int类型！" };
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(model);
                    return Json(json);
                }
            }
            catch (Exception ex)
            {
                var model = new { flag = false, res = ex.Message };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                return Json(json);
            }
        }


        /// <summary>
        /// 获取下载组件文件的文件夹，下载后需要删除
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTaskFileDownLoadPath()
        {
            try
            {
                var str = Request["id"];//零件ID，必须是组件id
                string path = "";
                int compantId = -1;
                if (int.TryParse(str, out compantId))
                {
                    if (TaskInfoManage.DownLoadTaskToolDrawFiles(compantId, ref path))
                    {
                        var model = new { flag = true, res = path };
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        return Json(json);
                    }
                    else
                    {
                        var model = new { flag = false, res = path };
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        return Json(json);
                    }
                }
                else
                {
                    var model = new { flag = false, res = "请输入正确的int类型！" };
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(model);
                    return Json(json);
                }
            }
            catch (Exception ex)
            {
                var model = new { flag = false, res = ex.Message};
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                return Json(json);
            }
        }

        /// <summary>
        /// 删除下载组件文件的文件夹
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteFileDownLoadPath()
        {
            try
            {
                var str = Request["path"];//路径
                str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, str);
                if(System.IO. File.Exists(str))
                {
                    System.IO.File.Delete(str);
                    return Content("ok");
                }
                else
                {
                    return Content("请输入正确的路径");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        /// <summary>
        /// 获取所有的模糊搜索的字符串
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTaskInfoStrList()
        {
            DatabaseList da = DatabaseList.GetData();
            var list = CompontInfoManage.GetTaskInfo(da,0, "");
            return Json(CompontInfoManage.GetAllTaskInfoStr(list), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 模糊搜索提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchSubmit()
        {
            try
            {
                var str = Request["str"];
                DatabaseList da = DatabaseList.GetData();
                var list = CompontInfoManage.GetTaskInfo(da,0, "");
                var res = CompontInfoManage.SearchSubmit(list, str, false);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 添加产品组件,有工序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompont()
        {
            try
            {
                string parentId = Request["parentId"];//父节点主键ID
                string compontName = Request["compontName"];//节点名称
                string compontNum = Request["compontNum"];//节点编号
                char[] charArray = { '\\', '/', ':', '*', '?', '>', '<', '|', '"' };
                foreach (var item in charArray)
                {
                    if (compontNum.Contains(item))
                    {
                        return Content("请输入合法的编号！");
                    }
                }
                string planStartTimeStr = Request["planStartTime"] == null ? DateTime.Now.ToString() : Request["planStartTime"];//计划开始时间
                string planEndTimeStr = Request["planEndTime"] == null ? DateTime.Now.ToString() : Request["planEndTime"];//计划结束时间
                string craftPersonIdStr = Request["craftPersonId"] == null ? "0" : Request["craftPersonId"];//工艺责任人主键ID
                string previewCategoryStr = Request["previewCategoryStr"];//预览组别
                var files = Request.Files == null ? null : Request.Files;//图纸文件
                List<string> previewCategoryStrList = previewCategoryStr.Split(',').ToList();
                List<int> previewCategoryList = new List<int>();
                foreach (var item in previewCategoryStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        previewCategoryList.Add(_int);
                    }
                }
                int craftPersonId = -1;
                int creatPersonID = -1;
                int parent = -1;
                DateTime planStartTime = DateTime.Now;
                DateTime planEndTime = DateTime.Now;
                if (DateTime.TryParse(planStartTimeStr, out planStartTime) && DateTime.TryParse(planEndTimeStr, out planEndTime))
                {
                    if (int.TryParse (craftPersonIdStr ,out craftPersonId)&&int.TryParse(Session["id"].ToString(), out creatPersonID) && int.TryParse(parentId, out parent))
                    {
                        string errMsg = "";
                        int compontId = -1;
                        if (CompontInfoManage.AddCompont(parent, compontName, compontNum, creatPersonID, planStartTime, planEndTime, craftPersonId, previewCategoryList, out compontId, ref errMsg))
                        {
                            if (files != null)
                            {
                                SupplementaryInformation pathInfo = new SupplementaryInformation();
                                var pathTask = TaskInfoManage.GetCompontPath(compontId);
                                var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                                if (!Directory.Exists(dirPath))
                                {
                                    Directory.CreateDirectory(dirPath);
                                }
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var filePath = Path.Combine(dirPath, files[i].FileName);
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
                    else
                    {
                        return Content("请输入正确的int类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的DateTime类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }



        }

        /// <summary>
        /// 添加产品组件,无工艺
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompontNoCraft()
        {
            try
            {
                string parentId = Request["parentId"];//父节点主键ID
                string compontName = Request["compontName"];//节点名称
                string compontNum = Request["compontNum"];//节点编号
                char[] charArray = { '\\', '/', ':', '*', '?', '>', '<', '|', '"' };
                foreach (var item in charArray)
                {
                    if (compontNum.Contains(item))
                    {
                        return Content("请输入合法的编号！");
                    }
                }
                string planStartTimeStr = Request["planStartTime"] == null ? DateTime.Now.ToString() : Request["planStartTime"];//计划开始时间
                string planEndTimeStr = Request["planEndTime"] == null ? DateTime.Now.ToString() : Request["planEndTime"];//计划结束时间
                
                var files = Request.Files == null ? null : Request.Files;//图纸文件
                
                int creatPersonID = -1;
                int parent = -1;
                DateTime planStartTime = DateTime.Now;
                DateTime planEndTime = DateTime.Now;
                if (DateTime.TryParse(planStartTimeStr, out planStartTime) && DateTime.TryParse(planEndTimeStr, out planEndTime))
                {
                    if ( int.TryParse(Session["id"].ToString(), out creatPersonID) && int.TryParse(parentId, out parent))
                    {
                        string errMsg = "";
                        int compontId = -1;
                        if (CompontInfoManage.AddCompontNoCraft(parent, compontName, compontNum, creatPersonID, planStartTime, planEndTime,  out compontId, ref errMsg))
                        {
                            if (files != null)
                            {
                                SupplementaryInformation pathInfo = new SupplementaryInformation();
                                var pathTask = TaskInfoManage.GetCompontPath(compontId);
                                var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                                if (!Directory.Exists(dirPath))
                                {
                                    Directory.CreateDirectory(dirPath);
                                }
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var filePath = Path.Combine(dirPath, files[i].FileName);
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
                    else
                    {
                        return Content("请输入正确的int类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的DateTime类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }



        }

        /// <summary>
        /// 添加零件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTask()
        {
            try
            {
                string parentId = Request["parentId"];//父节点主键ID
                string taskName = Request["taskName"];//节点名称

                string taskNum = Request["taskNum"];//节点编号
                char[] charArray = { '\\', '/', ':', '*', '?', '>', '<', '|', '"' };
                foreach (var item in charArray)
                {
                    if (taskNum.Contains(item))
                    {
                        return Content("请输入合法的编号！");
                    }
                }
                string planStartTimeStr = Request["planStartTime"] == null ? DateTime.Now.ToString() : Request["planStartTime"];//计划开始时间
                string planEndTimeStr = Request["planEndTime"] == null ? DateTime.Now.ToString() : Request["planEndTime"];//计划结束时间
                string craftPersonIdStr = Request["craftPersonId"] == null ? "0" : Request["craftPersonId"];//工艺责任人主键ID
                string previewCategoryStr = Request["previewCategoryStr"];//预览组别
                var files = Request.Files == null ? null : Request.Files;//图纸文件
                List<string> previewCategoryStrList = previewCategoryStr.Split(',').ToList();
                List<int> previewCategoryList = new List<int>();
                foreach (var item in previewCategoryStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        previewCategoryList.Add(_int);
                    }
                }
                int creatPersonID = -1;
                int craftPersonId = -1;
                int parent = -1;
                DateTime planStartTime = DateTime.Now;
                DateTime planEndTime = DateTime.Now;
                if (DateTime.TryParse(planStartTimeStr, out planStartTime) && DateTime.TryParse(planEndTimeStr, out planEndTime))
                {
                    if (int.TryParse(Session["id"].ToString(), out creatPersonID) && int.TryParse(parentId, out parent) && int.TryParse(craftPersonIdStr, out craftPersonId))
                    {
                        string errMsg = "";
                        int taskId = -1;
                        if (TaskInfoManage.AddTask(parent, taskName, taskNum, creatPersonID, planStartTime, planEndTime, craftPersonId, previewCategoryList, out taskId, ref errMsg))
                        {
                            if (files != null)
                            {
                                SupplementaryInformation pathInfo = new SupplementaryInformation();
                                var pathTask = TaskInfoManage.GetTaskPath(taskId);
                                var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                                if (!Directory.Exists(dirPath))
                                {
                                    Directory.CreateDirectory(dirPath);
                                }
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var filePath = Path.Combine(dirPath, files[i].FileName);
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
                    else
                    {
                        return Content("请输入正确的int类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的DateTime类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }



        }


        /// <summary>
        /// 删除零件任务图纸
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTaskFileMethod()
        {
            try
            {
                var taskIdStr = Request["taskId"];//零件任务Id
                var filesName = Request["fileNames"];//图纸文件
                List<string> fileName = filesName.Split(',').ToList();

                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {

                    string errMsg = "";
                    if (TaskInfoManage.DeleteTaskFile(taskId, personId, fileName, ref errMsg))
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
        /// 删除组件任务图纸
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCompontFileMethod()
        {
            try
            {
                var taskIdStr = Request["compontId"];//零件任务Id
                var filesName = Request["fileNames"];//图纸文件
                List<string> fileName = filesName.Split(',').ToList();

                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {

                    string errMsg = "";
                    if (CompontInfoManage.DeleteCompontFile(taskId, personId, fileName, ref errMsg))
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
        /// 获取零件任务文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTaskFiles()
        {
            try
            {
                var taskIdStr = Request["taskId"];//零件任务Id
                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetTaskPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                    List<FileInfoClass> fileInfos = new List<FileInfoClass>();
                    if (Directory.Exists(dirPath))
                    {
                        var files = Directory.GetFiles(dirPath);
                        foreach (var item in files)
                        {
                            FileInfo fileinfo = new FileInfo(item);

                            FileInfoClass file = new FileInfoClass();
                            file.downloadPath = Path.Combine("JDJS_PDMS_Files", pathTask, "图纸", fileinfo.Name);
                            file.fileName = fileinfo.Name;
                            file.updateTime = fileinfo.LastWriteTime;
                            file.updateTimeStr = fileinfo.LastWriteTime.ToString().Substring(0, fileinfo.LastWriteTime.ToString().LastIndexOf(':'));
                            file.taskId = taskId;
                            fileInfos.Add(file);
                        }

                    }

                    return Json(new { code = 0, data = fileInfos }, JsonRequestBehavior.AllowGet);



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
        /// 获取组件任务文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCompontFiles()
        {
            try
            {
                var taskIdStr = Request["compontId"];//零件任务Id
                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    SupplementaryInformation pathInfo = new SupplementaryInformation();
                    var pathTask = TaskInfoManage.GetCompontPath(taskId);
                    var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                    List<FileInfoClass> fileInfos = new List<FileInfoClass>();
                    if (Directory.Exists(dirPath))
                    {
                        var files = Directory.GetFiles(dirPath);
                        foreach (var item in files)
                        {
                            FileInfo fileinfo = new FileInfo(item);

                            FileInfoClass file = new FileInfoClass();
                            file.downloadPath = Path.Combine("JDJS_PDMS_Files", pathTask, "图纸", fileinfo.Name);
                            file.fileName = fileinfo.Name;
                            file.updateTime = fileinfo.LastWriteTime;
                            file.updateTimeStr = fileinfo.LastWriteTime.ToString().Substring(0, fileinfo.LastWriteTime.ToString().LastIndexOf(':'));
                            file.taskId = taskId;
                            fileInfos.Add(file);
                        }

                    }

                    return Json(new { code = 0, data = fileInfos }, JsonRequestBehavior.AllowGet);



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
        /// 添加零件任务图纸
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTaskFileMethod()
        {
            try
            {
                var taskIdStr = Request["taskId"];//零件任务Id
                var files = Request.Files == null ? null : Request.Files;//图纸文件
                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    if (files != null)
                    {
                        string errMsg = "";
                        if (TaskInfoManage.AddTaskFile(taskId, personId, ref errMsg))
                        {
                            SupplementaryInformation pathInfo = new SupplementaryInformation();
                            var pathTask = TaskInfoManage.GetTaskPath(taskId);
                            var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(dirPath, files[i].FileName);
                                files[i].SaveAs(filePath);
                            }
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                    return Content("ok");
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
        /// 添加组件任务图纸
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompontFileMethod()
        {
            try
            {
                var taskIdStr = Request["compontId"];//零件任务Id
                var files = Request.Files == null ? null : Request.Files;//图纸文件
                var personIdStr = Session["id"];
                int taskId = -1;
                int personId = -1;
                if (int.TryParse(taskIdStr, out taskId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    if (files != null)
                    {
                        string errMsg = "";
                        if (CompontInfoManage.AddCompontFile(taskId, personId, ref errMsg))
                        {
                            SupplementaryInformation pathInfo = new SupplementaryInformation();
                            var pathTask = TaskInfoManage.GetCompontPath(taskId);
                            var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask, "图纸");
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                            for (int i = 0; i < files.Count; i++)
                            {
                                var filePath = Path.Combine(dirPath, files[i].FileName);
                                files[i].SaveAs(filePath);
                            }
                        }
                        else
                        {
                            return Content(errMsg);
                        }
                    }
                    return Content("ok");
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

        //[HttpPost]
        public ActionResult GetTaskPath()
        {
            try
            {
                string taskId = Request["taskId"];//父节点主键ID
                var path = TaskInfoManage.GetTaskPath(Convert.ToInt32(taskId));
                return Content(path);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// 获取所有的预览组别,是个类，包含一个主键id和一个组别名称name
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllPreviewCategory()
        {
            return Json(TaskInfoManage.GetPreviewCategory(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取所有的预览组别,是个类，包含一个主键id和一个组别名称name
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllCompontPreviewCategory()
        {
            return Json(CompontInfoManage .GetCompontPreviewCategory(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除零件任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePartTaskMethod()
        {
            try
            {
                string taskIdStr = Request["taskId"];//任务主键ID
                int alterPersonID = -1;
                int taskId = -1;
                if (int.TryParse(Session["id"].ToString(), out alterPersonID) && int.TryParse(taskIdStr, out taskId))
                {
                    string errMsg = "";
                    if (TaskInfoManage.DeleteTask(taskId, alterPersonID, ref errMsg))
                    {

                        SupplementaryInformation pathInfo = new SupplementaryInformation();
                        var pathTask = TaskInfoManage.GetTaskPath(taskId);
                        var dirPath = Path.Combine(pathInfo.upLoadPath(), pathTask);
                        if (Directory.Exists(dirPath))
                        {
                            Directory.Delete(dirPath, true);
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
                    return Content("请输入正确的int类型！");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 修改零件任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterPartTaskMethod()
        {
            try
            {
                string taskIdStr = Request["taskId"];//任务主键ID
                string parentId = Request["parentId"];//父节点主键ID
                string taskName = Request["taskName"];//节点名称

                string taskNum = Request["taskNum"];//节点编号
                char[] charArray = { '\\', '/', ':', '*', '?', '>', '<', '|', '"' };
                foreach (var item in charArray)
                {
                    if (taskNum.Contains(item))
                    {
                        return Content("请输入合法的编号！");
                    }
                }
                string planStartTimeStr = Request["planStartTime"] == null ? DateTime.Now.ToString() : Request["planStartTime"];//计划开始时间
                string planEndTimeStr = Request["planEndTime"] == null ? DateTime.Now.ToString() : Request["planEndTime"];//计划结束时间
                string craftPersonIdStr = Request["craftPersonId"] == null ? "0" : Request["craftPersonId"];//工艺责任人主键ID
                string previewCategoryStr = Request["previewCategoryStr"];//预览组别
                List<string> previewCategoryStrList = previewCategoryStr.Split(',').ToList();
                List<int> previewCategoryList = new List<int>();
                foreach (var item in previewCategoryStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        previewCategoryList.Add(_int);
                    }
                }
                int craftPersonID = -1;
                int alterPersonID = -1;
                int taskId = -1;
                int parent = -1;
                DateTime planStartTime = DateTime.Now;
                DateTime planEndTime = DateTime.Now;
                if (DateTime.TryParse(planStartTimeStr, out planStartTime) && DateTime.TryParse(planEndTimeStr, out planEndTime))
                {
                    if (int.TryParse(Session["id"].ToString(), out alterPersonID) && int.TryParse(taskIdStr, out taskId) && int.TryParse(parentId, out parent) && int.TryParse(craftPersonIdStr, out craftPersonID))
                    {
                        string errMsg = "";
                        if (TaskInfoManage.AlterTask(taskId, alterPersonID, parent, taskName, taskNum, planStartTime, planEndTime, craftPersonID, previewCategoryList, ref errMsg))
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
                        return Content("请输入正确的int类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的时间格式！");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 修改组件产品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterCompontMethod()
        {
            try
            {
                string compontIdStr = Request["compontId"];//组件产品主键ID
                string parentId = Request["parentId"];//父节点主键ID
                string compontName = Request["compontName"];//节点名称

                string compontNum = Request["compontNum"];//节点编号

                string planStartTimeStr = Request["planStartTime"] == null ? DateTime.Now.ToString() : Request["planStartTime"];//计划开始时间
                string planEndTimeStr = Request["planEndTime"] == null ? DateTime.Now.ToString() : Request["planEndTime"];//计划结束时间
                string craftPersonIdStr = Request["craftPersonId"] == null ? "0" : Request["craftPersonId"];//工艺责任人主键ID
                string previewCategoryStr = Request["previewCategoryStr"];//预览组别
                List<string> previewCategoryStrList = previewCategoryStr.Split(',').ToList();
                List<int> previewCategoryList = new List<int>();
                foreach (var item in previewCategoryStrList)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        previewCategoryList.Add(_int);
                    }
                }
                int craftPersonId = -1;
                int alterPersonID = -1;
                int compontId = -1;
                int parent = -1;
                DateTime planStartTime = DateTime.Now;
                DateTime planEndTime = DateTime.Now;
                if (DateTime.TryParse(planStartTimeStr, out planStartTime) && DateTime.TryParse(planEndTimeStr, out planEndTime))
                {
                    if (int.TryParse (craftPersonIdStr ,out craftPersonId)&&int.TryParse(Session["id"].ToString(), out alterPersonID) && int.TryParse(compontIdStr, out compontId) && int.TryParse(parentId, out parent))
                    {
                        string errMsg = "";
                        if (CompontInfoManage.AlterCompontInfo(compontId, parent, alterPersonID, compontName, compontNum, planStartTime, planEndTime,craftPersonId ,previewCategoryList, ref errMsg))
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
                        return Content("请输入正确的int类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的时间格式！");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 修改组件产品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterCompontNoCraftMethod()
        {
            try
            {
                string compontIdStr = Request["compontId"];//组件产品主键ID
                string parentId = Request["parentId"];//父节点主键ID
                string compontName = Request["compontName"];//节点名称

                string compontNum = Request["compontNum"];//节点编号

                string planStartTimeStr = Request["planStartTime"] == null ? DateTime.Now.ToString() : Request["planStartTime"];//计划开始时间
                string planEndTimeStr = Request["planEndTime"] == null ? DateTime.Now.ToString() : Request["planEndTime"];//计划结束时间
                
                int alterPersonID = -1;
                int compontId = -1;
                int parent = -1;
                DateTime planStartTime = DateTime.Now;
                DateTime planEndTime = DateTime.Now;
                if (DateTime.TryParse(planStartTimeStr, out planStartTime) && DateTime.TryParse(planEndTimeStr, out planEndTime))
                {
                    if ( int.TryParse(Session["id"].ToString(), out alterPersonID) && int.TryParse(compontIdStr, out compontId) && int.TryParse(parentId, out parent))
                    {
                        string errMsg = "";
                        if (CompontInfoManage.AlterCompontInfoNoCraft(compontId, parent, alterPersonID, compontName, compontNum, planStartTime, planEndTime, ref errMsg))
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
                        return Content("请输入正确的int类型！");
                    }
                }
                else
                {
                    return Content("请输入正确的时间格式！");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 删除组件产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCompontMethod()
        {
            try
            {
                string compontIdStr = Request["compontId"];//任务主键ID
                int alterPersonID = -1;
                int compontId = -1;
                if (int.TryParse(Session["id"].ToString(), out alterPersonID) && int.TryParse(compontIdStr, out compontId))
                {
                    string errMsg = "";
                    if (CompontInfoManage.DeleteCompont(compontId, alterPersonID, ref errMsg))
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
                    return Content("请输入正确的int类型！");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 获取弹窗内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPopUpWindowsContentByLoginPerson()
        {
            try
            {
                int personID = 0;
                if (int.TryParse(Session["id"].ToString(), out personID))
                {
                    return Content(CraftDesignInfoManage.GetPopUpWindowsContentByLoginPerson(personID));
                }
                else
                {
                    return Content("请输入正确的int类型");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 删除已删除任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTaskByDelete()
        {
            return Content(DeleteOldTask.Delete().ToString());
        }

        [HttpGet]
        public ActionResult CompleteTaskByComplete()
        {
            string err = "";
            CompontInfoManage.SetCompontNoCraftState(ref err).ToString();
            return Content(err);
        }
    }
}
