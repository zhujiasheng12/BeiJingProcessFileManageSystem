﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 北京工艺文件管理MVC.Models;

namespace 北京工艺文件管理MVC.Controllers
{
    public class EmmployeeController : Controller
    {
        //
        // GET: /Emmployee/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LonOn()
        {
            var userName = Request["userName"];
            var passWord = Request["passWord"];
            string errMsg = "";
            string limlt = ""; ;
            string staffName = "";
            int id = 0;
            int processTypeId = -1;
            string processTypeStr = "";
            string user = "";
            if (EmployeeManage.LonigOn(userName, passWord, out limlt, out user,out staffName, out id, out processTypeId, out processTypeStr, ref errMsg))
            {
                Session["name"] = staffName;
                Session["id"] = id;
                Session["limit"] = limlt;
                Session["processTypeId"] = processTypeId;
                Session["processTypeStr"] = processTypeStr;
                Session["user"] = user;
            }
            return Content(errMsg);

        }
        /// <summary>
        /// 检测登录合法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Check()
        {
            try
            {
                return Json(new {user=Session["user"]==null?"":Session["user"], name = Session["name"] == null ? "" : Session["name"], id = Session["id"] == null ? "" : Session["id"], limlt = Session["limit"] == null ? "" : Session["limit"], processTypeId = Session["processTypeId"] == null ? -1 : Session["processTypeId"], processTypeStr = Session["processTypeStr"] == null ? "" : Session["processTypeStr"] }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取所有的部门与员工
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllStaffAndDepartment()
        {
            try
            {
                return Json(EmployeeManage.GetAllDepartment(0, ""), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取所有的部门
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDepartmentMethod()
        {
            try
            {
                return Json(EmployeeManage.GetAllDepartmentOnly(0, ""), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddDepartmentMethod()
        {
            try
            {
                //int parentID, string departmentName, int creatPersonID, ref string errMsg
                var parentIDStr = Request["parentID"];///部门父节点ID，没有则为0
                var departmentName = Request["departmentName"];//部门名称
                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int parentID = -1;
                int createPersonID = -1;
                if (int.TryParse(parentIDStr, out parentID) && int.TryParse(creatPersonIDsStr, out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.AddDepartment(parentID, departmentName, createPersonID, ref errMsg))
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
                    return Content("请输入正确的Int类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }


        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDepartmentMethod()
        {
            try
            {
                var departmentIdStr = Request["departmentId"];
                var personIdStr = Session["id"];
                int departmentId = -1;
                int personId = -1;
                if (int.TryParse(departmentIdStr, out departmentId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (EmployeeManage.DeleteDepartment(departmentId, personId, ref errMsg))
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
        /// 修改部门信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterDepartmengInfoMethod()
        {
            try
            {
                var departmentIdStr = Request["departmentId"];
                var departmentName = Request["departmentName"];
                var personIdStr = Session["id"];
                int departmentId = -1;
                int personId = -1;
                if (int.TryParse(departmentIdStr, out departmentId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (EmployeeManage.AlterDepartment(departmentId, departmentName, personId, ref errMsg))
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
        /// 添加职位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPositionMethod()
        {
            try
            {
                //int parentID, string positionName, int creatPersonID, List<int> limltList, ref string errMsg
                var parentIDStr = Request["parentID"];///部门父节点ID
                var positionName = Request["positionName"];//职位名称
                var limliStr = Request["limlt"];//该职位的权限ID集合

                List<string> limltListStr = limliStr.Split(',').ToList();
                List<int> limltList = new List<int>();
                foreach (var item in limltListStr)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        limltList.Add(_int);
                    }
                }
                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int parentID = -1;
                int createPersonID = -1;
                if (int.TryParse(parentIDStr, out parentID) && int.TryParse(creatPersonIDsStr, out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.AddPosition(parentID, positionName, createPersonID, limltList, ref errMsg))
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
                    return Content("请输入正确的Int类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }



        /// <summary>
        /// 添加员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEmployeeMethod()
        {
            try
            {

                var parentIDStr = Request["parentID"];///职位父节点ID
                var staffName = Request["staffName"];//员工名称
                var tel = Request["tel"];//员工电话
                var userName = Request["userName"];//员工登录名
                var passWord = Request["passWord"];//员工登录密码
                var mailBox = Request["mailBox"];//员工邮箱
                var processTypeIdStr = Request["processTypeId"];//员工所属加工类型Id
               // var compontProcessTypeIdStr = Request["compontProcessTypeId"];//员工所属加工类型Id
                var compontProcessTypeIdStr = Request["processTypeId"];//员工所属加工类型Id
                int? compontProcessTypeId =null;

                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int parentID = -1;
                int createPersonID = -1;
                int? processTypeId = null;
                if (processTypeIdStr != null && processTypeIdStr != "0")
                {
                    processTypeId = Convert.ToInt32(processTypeIdStr);
                }
                if (compontProcessTypeIdStr != null && compontProcessTypeIdStr != "0")
                {
                    compontProcessTypeId = Convert.ToInt32(compontProcessTypeIdStr);
                }
                if ( int.TryParse(parentIDStr, out parentID) && int.TryParse(creatPersonIDsStr, out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.AddEmployee(parentID, staffName, tel, userName, passWord, mailBox, createPersonID, processTypeId,compontProcessTypeId , ref errMsg))
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
                    return Content("请输入正确的Int类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        /// <summary>
        /// 读取某个部门下的职位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPositionOfDepartment()
        {
            try
            {
                var parentIDStr = Request["parentID"];///部门节点ID
                // GetDepartmentPosition(int parentID  
                int parentID = -1;
                if (int.TryParse(parentIDStr, out parentID))
                {
                    var list = EmployeeManage.GetDepartmentPosition(parentID);
                    return Json(new { code = 0, data = list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("请输入正确的部门ID");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePositionMethod()
        {
            try
            {
                var positionIdStr = Request["positionId"];
                var personIdStr = Session["id"];
                int positionId = -1;
                int personId = -1;
                if (int.TryParse(positionIdStr, out positionId) && int.TryParse(personIdStr.ToString(), out personId))
                {
                    string errMsg = "";
                    if (EmployeeManage.DeletePosition(positionId, personId, ref errMsg))
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
        /// 修改职位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterPositionInfoMethod()
        {
            try
            {
                //int parentID, string positionName, int creatPersonID, List<int> limltList, ref string errMsg
                var positionIdStr = Request["positionId"];///部门父节点ID
                var positionName = Request["positionName"];//职位名称
                var limliStr = Request["limlt"];//该职位的权限ID集合

                List<string> limltListStr = limliStr.Split(',').ToList();
                List<int> limltList = new List<int>();
                foreach (var item in limltListStr)
                {
                    int _int = -1;
                    if (int.TryParse(item, out _int))
                    {
                        limltList.Add(_int);
                    }
                }
                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int positionID = -1;
                int createPersonID = -1;
                if (int.TryParse(positionIdStr, out positionID) && int.TryParse(creatPersonIDsStr, out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.AlterPositionInfo(positionID, positionName, limltList, createPersonID, ref errMsg))
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
                    return Content("请输入正确的Int类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetAuthority()
        {
            try
            {

                return Json(EmployeeManage.GetAuthority(0), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 获取职位的权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuthorityByPositionMethod()
        {
            try
            {
                var position = Request["positionId"];
                int positionId = -1;
                if (int.TryParse(position, out positionId))
                {
                    var list = EmployeeManage.GetAuthorityByPosition(positionId);
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

        [HttpPost]
        public ActionResult AddAuthorityMethod()
        {
            try
            {
                var parentIDStr = Request["parentID"];///权限父节点ID
                var authorityName = Request["authorityName"];//员工名称
                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int parentID = -1;
                int createPersonID = -1;
                if (int.TryParse(parentIDStr, out parentID) && int.TryParse(creatPersonIDsStr, out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.AddAuthority(parentID, authorityName, createPersonID, ref errMsg))
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
                    return Content("请输入正确的int类型");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEmployeeMethod()
        {
            try
            {

                var staffIDStr = Request["staffID"];///员工主键ID


                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int staffID = -1;
                int createPersonID = -1;
                if (int.TryParse(staffIDStr, out staffID) && int.TryParse(creatPersonIDsStr, out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.DeleteEmployee(staffID, createPersonID, ref errMsg))
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
                    return Content("请输入正确的Int类型！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 读取人员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEmployeeInfoMethod()
        {
            try
            {
                var staffIdStr = Request["staffId"];//员工Id
                int staffId = -1;
                if (int.TryParse(staffIdStr, out staffId))
                {
                    var list = EmployeeManage.GetEmployeeInfo(staffId);
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
        /// 修改员工信息提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterStaffInfoMethod()
        {
            //AlterEmployeeInfo(int staffId, string staffName,int personId, string tel, string userName, string passWord, string mailBox, int? processTypeId,List <int> authorityList, ref string errMsg
            try
            {
                var staffIdStr = Request["staffId"];
                var staffName = Request["staffName"];//员工名称
                var tel = Request["tel"];//员工电话
                var positionIdStr = Request["positionId"];//职位
                var userName = Request["userName"];//员工登录名
                var passWord = Request["passWord"];//员工登录密码
                var mailBox = Request["mailBox"];//员工邮箱
                var processTypeIdStr = Request["processTypeId"];//员工所属加工类型Id
                //var compontProcessTypeIdStr = Request["compontProcessTypeId"];//员工所属加工类型Id
                var compontProcessTypeIdStr = Request["processTypeId"];//员工所属加工类型Id
                //var limliStr = Request["limlt"];//该职位的权限ID集合
                int staffId = -1;
                int positionId = 0;
                int? processTypeId = null;
                if (processTypeIdStr != null && processTypeIdStr != "0")
                {
                    processTypeId = Convert.ToInt32(processTypeIdStr);
                }
                int? compontProcessTypeId = null;
                if (compontProcessTypeIdStr != null && compontProcessTypeIdStr != "0")
                {
                    compontProcessTypeId = Convert.ToInt32(compontProcessTypeIdStr);
                }
               // List<string> limltListStr = limliStr.Split(',').ToList();
                //List<int> limltList = new List<int>();
                //foreach (var item in limltListStr)
                //{
                //    int _int = -1;
                //    if (int.TryParse(item, out _int))
                //    {
                //        limltList.Add(_int);
                //    }
                //}
                string creatPersonIDsStr = Session["id"] == null ? "" : Session["id"].ToString();
                int createPersonID = -1;
                
                if (int.TryParse (positionIdStr ,out positionId )&& int.TryParse(staffIdStr, out staffId) && int.TryParse(creatPersonIDsStr.ToString(), out createPersonID))
                {
                    string errMsg = "";
                    if (EmployeeManage.AlterEmployeeInfo(staffId, positionId , staffName, createPersonID, tel, userName, passWord, mailBox, processTypeId,compontProcessTypeId , ref errMsg))
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
        /// 员工修改自己密码提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterStaffItemselfInfoMethod()
        {
            //AlterEmployeeInfo(int staffId, string staffName,int personId, string tel, string userName, string passWord, string mailBox, int? processTypeId,List <int> authorityList, ref string errMsg
            try
            {
                var staffIdStr = Request["staffId"];//员工主键Id

                var passWordOld = Request["passWordOld"];//员工登录密码旧
                var passWordNew = Request["passWordNew"];//员工登录密码新
                int staffId = -1;
                
                if (int.TryParse(staffIdStr, out staffId) )
                {
                    string errMsg = "";
                    if (EmployeeManage.AlterItemselfPassword(staffId, passWordOld, passWordNew, ref errMsg))
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

        //GetAllDepartmentPosition
        /// <summary>
        /// 获取所有的职位
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDepartmentPosition()
        {
            try
            {
                return Json(EmployeeManage.GetAllDepartmentPosition(0, ""), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 读取某个部门下的职位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPositionOfDepartmentByStaff()
        {
            try
            {
                var parentIDStr = Request["staffID"];///部门节点ID
                // GetDepartmentPosition(int parentID  
                int parentID = -1;
                if (int.TryParse(parentIDStr, out parentID))
                {
                    var list = EmployeeManage.GetDepartmentPositionByStaff(parentID);
                    return Json(new { code = 0, data = list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("请输入正确的部门ID");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
