﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 北京工艺文件管理MVC.Database;

namespace 北京工艺文件管理MVC.Models
{
    public class EmployeeManage
    {
        public static bool LonigOn(string userName, string passWord, out string limlt, out string user, out string staffName, out int id, out int processTypeId, out string processTypeStr, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.Users == userName && r.state == "正常").FirstOrDefault();
                    if (staff == null)
                    {
                        errMsg = "该用户名不存在！";
                        limlt = "";
                        staffName = "";
                        id = 0;
                        processTypeId = -1;
                        processTypeStr = "";
                        user = "";
                        return false;
                    }
                    if (staff.PassWord != passWord)
                    {
                        errMsg = "用户名或密码错误，请确认后再试！";
                        limlt = "";
                        staffName = "";
                        id = 0;
                        processTypeId = -1;
                        processTypeStr = "";
                        user = "";
                        return false;
                    }
                    List<int> limitIDs = new List<int>();
                    var limltPosition = model.JDJS_PDMS_Position_Authority_Table.Where(r => r.PositionID == staff.PosiationID && r.state == "正常");
                    foreach (var item in limltPosition)
                    {
                        if (!limitIDs.Contains(Convert.ToInt32(item.AuthorityID)))
                        {
                            limitIDs.Add(Convert.ToInt32(item.AuthorityID));
                        }
                    }
                    var limltStaff = model.JDJS_PDMS_Staff_Authority_Table.Where(r => r.StaffID == staff.ID && r.state == "正常");
                    foreach (var item in limltStaff)
                    {
                        if (!limitIDs.Contains(Convert.ToInt32(item.AuthorityID)))
                        {
                            limitIDs.Add(Convert.ToInt32(item.AuthorityID));
                        }
                    }
                    string limitStr = "";
                    foreach (var item in model.JDJS_PDMS_Limits_Of_Authority_Table.ToList())
                    {
                        if (!limitIDs.Contains(item.ID))
                        {
                            limitStr += item.DisPlayName + ",";
                        }
                    }

                    errMsg = "ok";
                    limlt = limitStr;
                    staffName = staff.StaffName;
                    id = staff.ID;
                    processTypeId = -1;
                    user = staff.Users;
                    processTypeStr = "";
                    if (staff.ProcessTypeID != null)
                    {
                        processTypeId = Convert.ToInt32(staff.ProcessTypeID);

                        var type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == staff.ProcessTypeID && r.state == "正常").FirstOrDefault();
                        if (type != null)
                        {
                            processTypeStr = type.ProcessingType;
                        }

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                limlt = "";
                staffName = "";
                id = 0;
                processTypeId = -1;
                processTypeStr = "";
                user = "";
                return false;
            }
        }
        /// <summary>
        /// 获取所有的部门已经人员信息
        /// </summary>
        /// <param name="parentID">父节点ID</param>
        /// <param name="number">唯一标识</param>
        /// <returns></returns>
        public static List<StaffInfoReadClass> GetAllDepartment(int parentID, string number)
        {
            try
            {
                List<StaffInfoReadClass> staffInfoList = new List<StaffInfoReadClass>();
                using (Model1 model = new Model1())
                {
                    var departments = model.JDJS_PDMS_Department_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in departments)
                    {
                        StaffInfoReadClass staff = new StaffInfoReadClass();
                        staff.allID = number + item.ID.ToString();
                        staff.id = item.ID;
                        staff.mailbox = "";
                        staff.passWord = "";
                        staff.position = "";
                        staff.staff = item.DepartmentName;
                        staff.tel = "";
                        staff.users = "";
                        staff.ispart = false;
                        staff.children = new List<StaffInfoReadClass>();
                        staff.children.AddRange(EmployeeManage.GetAllDepartment(item.ID, staff.allID + "_"));
                        staff.children.AddRange(EmployeeManage.GetAllPosition(item.ID, staff.allID + "_"));
                        staffInfoList.Add(staff);
                    }
                }
                return staffInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// 获取所有的部门已经人员信息
        /// </summary>
        /// <param name="parentID">父节点ID</param>
        /// <param name="number">唯一标识</param>
        /// <returns></returns>
        public static List<StaffInfoReadClass> GetAllDepartmentPosition(int parentID, string number)
        {
            try
            {
                List<StaffInfoReadClass> staffInfoList = new List<StaffInfoReadClass>();
                using (Model1 model = new Model1())
                {
                    var departments = model.JDJS_PDMS_Department_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in departments)
                    {
                        StaffInfoReadClass staff = new StaffInfoReadClass();
                        staff.allID = number + item.ID.ToString();
                        staff.id = item.ID;
                        staff.mailbox = "";
                        staff.passWord = "";
                        staff.position = "";
                        staff.enabled = "false";
                        staff.staff = item.DepartmentName;
                        staff.tel = "";
                        staff.users = "";
                        staff.ispart = false;
                        staff.children = new List<StaffInfoReadClass>();
                        staff.children.AddRange(EmployeeManage.GetAllDepartmentPosition(item.ID, staff.allID + "_"));
                        staff.children.AddRange(EmployeeManage.GetAllPositionOnly(item.ID, staff.allID + "_"));
                        staffInfoList.Add(staff);
                    }
                }
                return staffInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取部门下所有职位的员工
        /// </summary>
        /// <param name="parentID">部门ID</param>
        /// <param name="number">唯一标识</param>
        /// <returns></returns>
        public static List<StaffInfoReadClass> GetAllPositionOnly(int parentID, string number)
        {
            try
            {
                List<StaffInfoReadClass> staffInfoList = new List<StaffInfoReadClass>();
                using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == parentID && r.state == "正常");
                    foreach (var item in position)
                    {
                        StaffInfoReadClass staff = new StaffInfoReadClass();
                        staff.allID = number + item.ID.ToString();
                        staff.id = item.ID;
                        staff.mailbox = "";
                        staff.passWord = "";
                        staff.position = "";
                        staff.enabled = "false";
                        staff.staff = item.PositionName;
                        staff.allID = number + item.ID;
                        staff.enabled = "true";
                        staff.tel = "";
                        staff.users = "";
                        staff.ispart = false;
                        staff.children = new List<StaffInfoReadClass>();
                        staffInfoList.Add(staff);
                    }
                }
                return staffInfoList;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 只获取所有的部门
        /// </summary>
        /// <param name="parentID">父节点ID</param>
        /// <param name="number">唯一标识</param>
        /// <returns></returns>
        public static List<StaffInfoReadClass> GetAllDepartmentOnly(int parentID, string number)
        {
            try
            {
                List<StaffInfoReadClass> staffInfoList = new List<StaffInfoReadClass>();
                using (Model1 model = new Model1())
                {
                    var departments = model.JDJS_PDMS_Department_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in departments)
                    {
                        StaffInfoReadClass staff = new StaffInfoReadClass();
                        staff.allID = number + item.ID.ToString();
                        staff.id = item.ID;
                        staff.mailbox = "";
                        staff.passWord = "";
                        staff.position = "";
                        staff.staff = item.DepartmentName;
                        staff.tel = "";
                        staff.users = "";
                        staff.children = new List<StaffInfoReadClass>();
                        staff.children.AddRange(EmployeeManage.GetAllDepartmentOnly(item.ID, staff.allID + "_"));
                        staffInfoList.Add(staff);
                    }
                }
                return staffInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="personId">人员id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteDepartment(int departmentId, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var department = model.JDJS_PDMS_Department_Table.Where(r => r.ID == departmentId && r.state == "正常").FirstOrDefault();
                    if (department == null)
                    {
                        errMsg = "该部门不存在，请确认后再试！";
                        return false;
                    }
                    var departmentChilder = model.JDJS_PDMS_Department_Table.Where(r => r.ParentID == department.ID && r.state == "正常");
                    if (departmentChilder.Count() > 0)
                    {
                        errMsg = "该部门存在下属部门，请将下属部门删除再删除该部门！";
                        return false;
                    }
                    var positionChilder = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == department.ID && r.state == "正常");
                    if (positionChilder.Count() > 0)
                    {
                        errMsg = "该部门存在下属职位，请将下属职位删除再删除该部门！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Department_Table.Where(r => r.ID == departmentId).FirstOrDefault().state = "删除";
                            department.state = "删除";
                            department.LastAlterPersonID = personId;
                            department.LastAlterTime = DateTime.Now;
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
        /// 修改部门名称
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="name">名称</param>
        /// <param name="personId">人员Id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterDepartment(int departmentId, string name, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var department = model.JDJS_PDMS_Department_Table.Where(r => r.ID == departmentId && r.state == "正常").FirstOrDefault();
                    if (department == null)
                    {
                        errMsg = "该部门不存在，请确认后再试！";
                        return false;
                    }
                    if (department.DepartmentName != name)
                    {
                        var departmengOld = model.JDJS_PDMS_Department_Table.Where(r => r.ParentID == department.ParentID && r.state == "正常" && r.DepartmentName == name);
                        if (departmengOld.Count() > 0)
                        {
                            errMsg = "该部门名称已存在，请确认后再试！";
                            return false;
                        }
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            department.DepartmentName = name;
                            department.LastAlterTime = DateTime.Now;
                            department.LastAlterPersonID = personId;
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
        /// 获取部门下所有职位的员工
        /// </summary>
        /// <param name="parentID">部门ID</param>
        /// <param name="number">唯一标识</param>
        /// <returns></returns>
        public static List<StaffInfoReadClass> GetAllPosition(int parentID, string number)
        {
            try
            {
                List<StaffInfoReadClass> staffInfoList = new List<StaffInfoReadClass>();
                using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == parentID && r.state == "正常");
                    foreach (var item in position)
                    {
                        int positionID = item.ID;
                        staffInfoList.AddRange(EmployeeManage.GetAllStaff(positionID, item.PositionName, number + positionID + "_"));
                    }
                }
                return staffInfoList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取职位下所有员工
        /// </summary>
        /// <param name="parentID">职位ID</param>
        /// <param name="position">职位名称</param>
        /// <param name="number">唯一标识</param>
        /// <returns></returns>
        public static List<StaffInfoReadClass> GetAllStaff(int parentID, string position, string number)
        {
            try
            {
                List<StaffInfoReadClass> staffInfoList = new List<StaffInfoReadClass>();
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.PosiationID == parentID && r.state == "正常");
                    foreach (var item in staff)
                    {
                        int staffID = item.ID;
                        StaffInfoReadClass staffInfo = new StaffInfoReadClass();
                        staffInfo.allID = number + item.ID;
                        staffInfo.children = new List<StaffInfoReadClass>();
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

                        staffInfo.compontProcessTypeId = item.CompontProcessTypeID == null ? 0 : Convert.ToInt32(item.CompontProcessTypeID);
                        staffInfo.compontProcessTypeStr = "";
                        if (item.CompontProcessTypeID != null)
                        {
                            var type = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == item.CompontProcessTypeID && r.state == "正常").FirstOrDefault();
                            if (type != null)
                            {
                                staffInfo.compontProcessTypeStr = type.ProcessingType;
                            }
                        }

                        staffInfo.passWord = item.PassWord;
                        staffInfo.position = position;
                        staffInfo.staff = item.StaffName;
                        staffInfo.tel = item.Tel;
                        staffInfo.users = item.Users;
                        staffInfo.ispart = true;
                        staffInfoList.Add(staffInfo);

                    }
                }
                return staffInfoList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="parentID">父节点ID</param>
        /// <param name="departmentName">部门名称</param>
        /// <param name="creatPersonID">创建人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddDepartment(int parentID, string departmentName, int creatPersonID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var department = model.JDJS_PDMS_Department_Table.Where(r => r.ParentID == parentID && r.DepartmentName == departmentName && r.state == "正常");
                    if (department.Count() > 0)
                    {
                        errMsg = "该部门已存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_Department_Table jd = new JDJS_PDMS_Department_Table()
                            {
                                CreatPersonID = creatPersonID,
                                CreatTime = DateTime.Now,
                                DepartmentName = departmentName,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                ParentID = parentID,
                                state = "正常"
                            };
                            model.JDJS_PDMS_Department_Table.Add(jd);
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
        /// 增加部门内的职位
        /// </summary>
        /// <param name="parentID">父节点ID</param>
        /// <param name="positionName">职位名称</param>
        /// <param name="creatPersonID"><创建人/param>
        /// <param name="limltList">职位权限</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddPosition(int parentID, string positionName, int creatPersonID, List<int> limltList, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == parentID && r.PositionName == positionName && r.state == "正常");
                    if (position.Count() > 0)
                    {
                        errMsg = "该职位已存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_Position_Table jd = new JDJS_PDMS_Position_Table()
                            {
                                CreatPersonID = creatPersonID,
                                CreatTime = DateTime.Now,
                                DepartmentID = parentID,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                PositionName = positionName,
                                state = "正常"
                            };
                            model.JDJS_PDMS_Position_Table.Add(jd);
                            model.SaveChanges();
                            if (limltList != null && limltList.Count() > 0)
                            {
                                int positionID = jd.ID;
                                foreach (var item in limltList)
                                {
                                    JDJS_PDMS_Position_Authority_Table jdPos = new JDJS_PDMS_Position_Authority_Table()
                                    {
                                        AuthorityID = item,
                                        CreatPersonID = creatPersonID,
                                        CreatTime = DateTime.Now,
                                        LastAlterPersonID = creatPersonID,
                                        LastAlterTime = DateTime.Now,
                                        PositionID = positionID,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_Position_Authority_Table.Add(jdPos);
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
        /// 删除职位
        /// </summary>
        /// <param name="positionId">职位主键ID</param>
        /// <param name="personId">人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool DeletePosition(int positionId, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.ID == positionId && r.state == "正常").FirstOrDefault();
                    if (position == null)
                    {
                        errMsg = "该职位不存在，请确认后再试！";
                        return false;
                    }
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.PosiationID == positionId && r.state == "正常");
                    if (staff.Count() > 0)
                    {
                        errMsg = "该职位下存在员工，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            position.state = "删除";
                            position.LastAlterPersonID = personId;
                            position.LastAlterTime = DateTime.Now;
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
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
        /// 修改职位信息
        /// </summary>
        /// <param name="positionId">职位Id</param>
        /// <param name="positionName">职位名称</param>
        /// <param name="positionAuthorityIntList">职位权限ID列表</param>
        /// <param name="personId">人员Id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterPositionInfo(int positionId, string positionName, List<int> positionAuthorityIntList, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.ID == positionId && r.state == "正常").FirstOrDefault();
                    if (position == null)
                    {
                        errMsg = "该职位不存在，请确认后再试！";
                        return false;
                    }
                    if (position.PositionName != positionName)
                    {
                        var positionOld = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == position.DepartmentID && r.state == "正常" && r.PositionName == positionName);
                        if (positionOld.Count() > 0)
                        {
                            errMsg = "该职位名称已存在，请确认后再试！";
                            return false;
                        }
                    }
                    List<int> authorityIdListOld = new List<int>();
                    var authorityS = model.JDJS_PDMS_Position_Authority_Table.Where(r => r.PositionID == positionId && r.state == "正常");
                    foreach (var item in authorityS)
                    {
                        authorityIdListOld.Add(Convert.ToInt32(item.AuthorityID));
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            position.PositionName = positionName;
                            position.LastAlterPersonID = personId;
                            position.LastAlterTime = DateTime.Now;
                            foreach (var item in authorityIdListOld)
                            {
                                if (!positionAuthorityIntList.Contains(item))//需要删除旧的
                                {
                                    var authorityOld = model.JDJS_PDMS_Position_Authority_Table.Where(r => r.PositionID == positionId && r.AuthorityID == item && r.state == "正常");
                                    foreach (var real in authorityOld)
                                    {
                                        real.state = "删除";
                                        real.LastAlterPersonID = personId;
                                        real.LastAlterTime = DateTime.Now;

                                    }
                                }
                            }

                            foreach (var item in positionAuthorityIntList)
                            {
                                if (!authorityIdListOld.Contains(item))//添加新的
                                {
                                    JDJS_PDMS_Position_Authority_Table jd = new JDJS_PDMS_Position_Authority_Table()
                                    {
                                        AuthorityID = item,
                                        CreatPersonID = personId,
                                        CreatTime = DateTime.Now,
                                        LastAlterPersonID = personId,
                                        LastAlterTime = DateTime.Now,
                                        PositionID = positionId,
                                        state = "正常"
                                    };
                                    model.JDJS_PDMS_Position_Authority_Table.Add(jd);
                                }
                            }
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
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
        /// 添加员工
        /// </summary>
        /// <param name="parentID">职位主键ID</param>
        /// <param name="staffName">员工名称</param>
        /// <param name="tel">电话</param>
        /// <param name="userName">登录名</param>
        /// <param name="passWord">密码</param>
        /// <param name="mailBox">邮箱</param>
        /// <param name="creatPersonID">创建人员ID</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AddEmployee(int parentID, string staffName, string tel, string userName, string passWord, string mailBox, int creatPersonID, int? processTypeId, int? compontProcessTypeId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => (r.Users == userName && r.state == "正常"));
                    if (staff.Count() > 0)
                    {
                        errMsg = "该登录名已存在，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_Staff_Table jd = new JDJS_PDMS_Staff_Table()
                            {
                                CreatPersonID = creatPersonID,
                                CreatTime = DateTime.Now,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                MailBox = mailBox,
                                PassWord = passWord,
                                PosiationID = parentID,
                                StaffName = staffName,
                                ProcessTypeID = processTypeId,
                                CompontProcessTypeID = compontProcessTypeId,
                                state = "正常",
                                Tel = tel,
                                Users = userName
                            };
                            model.JDJS_PDMS_Staff_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Staff_Table.Add(jd);
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

        public static List<PositionInfo> GetDepartmentPositionByStaff(int staffID)
        {
            try
            {
                List<PositionInfo> positionInfoList = new List<PositionInfo>();
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffID && r.state == "正常").FirstOrDefault();
                    if (staff != null)
                    {
                        var position = model.JDJS_PDMS_Position_Table.Where(r => r.ID == staff.PosiationID && r.state == "正常").FirstOrDefault();
                        if (position != null)
                        {
                            var positions = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == position.DepartmentID && r.state == "正常");
                            foreach (var item in positions)
                            {
                                PositionInfo positionInfo = new PositionInfo();
                                positionInfo.id = item.ID;

                                positionInfo.positionName = item.PositionName;
                                positionInfoList.Add(positionInfo);
                            }
                        }
                    }
                }
                return positionInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取某个部门下所有的职位
        /// </summary>
        /// <param name="parentID">部门主键ID</param>
        /// <returns>职位信息，包含主键id和职位名称positionName</returns>
        public static List<PositionInfo> GetDepartmentPosition(int parentID)
        {
            try
            {
                List<PositionInfo> positionInfoList = new List<PositionInfo>();
                using (Model1 model = new Model1())
                {
                    var positions = model.JDJS_PDMS_Position_Table.Where(r => r.DepartmentID == parentID && r.state == "正常");
                    foreach (var item in positions)
                    {
                        PositionInfo positionInfo = new PositionInfo();
                        positionInfo.id = item.ID;

                        positionInfo.positionName = item.PositionName;
                        positionInfoList.Add(positionInfo);
                    }
                }
                return positionInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="parentID">权限父节点ID</param>
        /// <returns></returns>
        public static List<AuthorityInfo> GetAuthority(int parentID)
        {
            try
            {
                List<AuthorityInfo> authorityInfoList = new List<AuthorityInfo>();
                using (Model1 model = new Model1())
                {
                    var limlts = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in limlts)
                    {
                        AuthorityInfo authority = new AuthorityInfo();
                        authority.authorityName = item.AuthorityName;
                        authority.disPlayName = item.DisPlayName;
                        authority.id = item.ID;
                        authority.@checked = false;
                        authority.state = "open";
                        authority.parentID = parentID;
                        authority.children = new List<AuthorityInfo>();
                        authority.children = EmployeeManage.GetAuthority(item.ID);
                        if (authority.children.Count() > 0)
                        {
                            authority.state = "closed";
                        }

                        authorityInfoList.Add(authority);
                    }
                }
                return authorityInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }




        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="parentID">权限父节点ID</param>
        /// <returns></returns>
        public static List<AuthorityInfo> GetAuthorityByPositionList(int parentID, List<int> positionAuthority)
        {
            try
            {
                List<AuthorityInfo> authorityInfoList = new List<AuthorityInfo>();
                using (Model1 model = new Model1())
                {
                    var limlts = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in limlts)
                    {
                        AuthorityInfo authority = new AuthorityInfo();
                        authority.authorityName = item.AuthorityName;
                        authority.disPlayName = item.DisPlayName;
                        authority.id = item.ID;
                        authority.@checked = false;
                        if (positionAuthority.Contains(item.ID))
                        {
                            authority.@checked = true;
                        }
                        authority.state = "open";
                        authority.parentID = parentID;
                        authority.children = new List<AuthorityInfo>();
                        authority.children = EmployeeManage.GetAuthorityByPositionList(item.ID, positionAuthority);
                        if (authority.children.Count() > 0)
                        {
                            authority.@checked = false;
                            authority.state = "closed";
                        }

                        authorityInfoList.Add(authority);
                    }
                }
                return authorityInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// 根据职位获取权限列表
        /// </summary>
        /// <param name="parentID">权限父节点ID</param>
        /// <returns></returns>
        public static List<AuthorityInfo> GetAuthorityByPosition(int positionId)
        {
            try
            {
                List<AuthorityInfo> authorityInfoList = new List<AuthorityInfo>();
                List<int> authorityIdList = new List<int>();
                using (Model1 model = new Model1())
                {
                    var authorityList = model.JDJS_PDMS_Position_Authority_Table.Where(r => r.PositionID == positionId && r.state == "正常");
                    foreach (var item in authorityList)
                    {
                        authorityIdList.Add(Convert.ToInt32(item.AuthorityID));
                    }

                }

                authorityInfoList = GetAuthorityByPositionList(0, authorityIdList);


                return authorityInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }







        /// <summary>
        /// 根据人员的职位获取人员唯一ID
        /// </summary>
        /// <param name="positionID"></param>
        /// <returns></returns>
        public static string GetPersonAllIdByStaffIDOnPosition(int positionID)
        {
            try
            {
                string str = "";
                using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.ID == positionID && r.state == "正常").FirstOrDefault();
                    if (position == null)
                    {
                        return "";
                    }
                    str = position.ID.ToString();
                    str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffOnDepartment(Convert.ToInt32(position.DepartmentID)) + "_");

                }
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string GetPersonAllIdByStaffOnDepartment(int id)
        {
            try
            {
                string str = "";
                using (Model1 model = new Model1())
                {
                    var department = model.JDJS_PDMS_Department_Table.Where(r => r.ID == id && r.state == "正常").FirstOrDefault();
                    if (department == null)
                    {
                        return "";
                    }
                    str = department.ID.ToString();
                    if (department.ParentID != 0)
                    {
                        str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffOnDepartment(Convert.ToInt32(department.ParentID)) + "_");
                    }
                    return str;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        /// <summary>
        /// 根据人员的职位获取人员唯一ID
        /// </summary>
        /// <param name="positionID"></param>
        /// <returns></returns>
        public static string GetPersonAllIdByStaffIDOnPositionCreate(DatabaseList model, int positionID)
        {
            try
            {
                string str = "";
                //using (Model1 model = new Model1())
                {
                    var position = model.JDJS_PDMS_Position_Table.Where(r => r.ID == positionID && r.state == "正常").FirstOrDefault();
                    if (position == null)
                    {
                        return "";
                    }
                    str = position.ID.ToString();
                    str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffOnDepartmentCreate(model, Convert.ToInt32(position.DepartmentID)) + "_");

                }
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string GetPersonAllIdByStaffOnDepartmentCreate(DatabaseList model, int id)
        {
            try
            {
                string str = "";
                //using (Model1 model = new Model1())
                {
                    var department = model.JDJS_PDMS_Department_Table.Where(r => r.ID == id && r.state == "正常").FirstOrDefault();
                    if (department == null)
                    {
                        return "";
                    }
                    str = department.ID.ToString();
                    if (department.ParentID != 0)
                    {
                        str = str.Insert(0, EmployeeManage.GetPersonAllIdByStaffOnDepartmentCreate(model, Convert.ToInt32(department.ParentID)) + "_");
                    }
                    return str;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="authorityName"></param>
        /// <param name="creatPersonID"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool AddAuthority(int parentID, string authorityName, int creatPersonID, ref string errMsg)
        {
            try
            {
                string displayName = "";
                using (Model1 model = new Model1())
                {
                    var limlt = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ParentID == parentID && r.AuthorityName == authorityName && r.state == "正常");
                    if (limlt.Count() > 0)
                    {
                        errMsg = "该权限已存在，请确认后再试！";
                        return false;
                    }
                    if (parentID != 0)
                    {
                        var parentLimlt = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ID == parentID && r.state == "正常").FirstOrDefault();
                        if (parentLimlt == null)
                        {
                            errMsg = "该父级权限不存在，请确认后再试！";
                            return false;
                        }
                        displayName = parentLimlt.DisPlayName + "_";
                    }

                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_PDMS_Limits_Of_Authority_Table jd = new JDJS_PDMS_Limits_Of_Authority_Table()
                            {
                                AuthorityName = authorityName,
                                CreatPersonID = creatPersonID,
                                CreatTime = DateTime.Now,
                                DisPlayName = displayName + authorityName,
                                LastAlterPersonID = creatPersonID,
                                LastAlterTime = DateTime.Now,
                                ParentID = parentID,
                                state = "正常"
                            };
                            model.JDJS_PDMS_Limits_Of_Authority_Table.Add(jd);
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
        /// 删除权限
        /// </summary>
        /// <param name="authorityID"></param>
        /// <param name="alterPersonID"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool DeleteAuthority(int authorityID, int alterPersonID, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {

                    var limlt = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ID == authorityID && r.state == "正常").FirstOrDefault();
                    if (limlt != null)
                    {
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                limlt.LastAlterPersonID = alterPersonID;
                                limlt.LastAlterTime = DateTime.Now;
                                limlt.state = "删除";
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
                        var limltList = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ParentID == limlt.ID && r.state == "正常");
                        foreach (var item in limltList)
                        {
                            EmployeeManage.DeleteAuthority(item.ID, alterPersonID, ref errMsg);
                        }


                    }
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
        /// 删除员工
        /// </summary>
        /// <param name="staffId">员工主键Id</param>
        /// <param name="personId">人员id</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool DeleteEmployee(int staffId, int personId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId && r.state == "正常").FirstOrDefault();
                    if (staff == null)
                    {
                        errMsg = "该员工不存在，请确认后再试！";
                        return false;

                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == personId).FirstOrDefault().state = "删除";
                            staff.state = "删除";
                            staff.LastAlterPersonID = personId;
                            staff.LastAlterTime = DateTime.Now;
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
        /// 获取人员信息
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public static StaffInfo GetEmployeeInfo(int staffId)
        {
            try
            {
                StaffInfo staffInfo = new StaffInfo();
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId && r.state == "正常").FirstOrDefault();
                    if (staff != null)
                    {
                        staffInfo.id = staff.ID;
                        staffInfo.mailbox = staff.MailBox;
                        staffInfo.passWord = staff.PassWord;
                        staffInfo.staff = staff.StaffName;
                        staffInfo.tel = staff.Tel;
                        staffInfo.users = staff.Users;
                        staffInfo.position = "";
                        var position = model.JDJS_PDMS_Position_Table.Where(r => r.ID == staff.PosiationID && r.state == "正常").FirstOrDefault();
                        if (position != null)
                        {
                            staffInfo.position = position.PositionName;
                        }
                        staffInfo.processTypeId = staff.ProcessTypeID == null ? 0 : Convert.ToInt32(staff.ProcessTypeID);
                        staffInfo.processTypeStr = "";
                        if (staff.ProcessTypeID != null)
                        {
                            var type = model.JDJS_PDMS_Task_Preview_Category_Table.Where(r => r.ID == staff.ProcessTypeID && r.state == "正常").FirstOrDefault();
                            if (type != null)
                            {
                                staffInfo.processTypeStr = type.ProcessingType;
                            }
                        }
                        staffInfo.compontProcessTypeId = staff.CompontProcessTypeID == null ? 0 : Convert.ToInt32(staff.CompontProcessTypeID);
                        staffInfo.compontProcessTypeStr = "";
                        if (staff.CompontProcessTypeID != null)
                        {
                            var type = model.JDJS_PDMS_Compont_Preview_Category_Table.Where(r => r.ID == staff.CompontProcessTypeID && r.state == "正常").FirstOrDefault();
                            if (type != null)
                            {
                                staffInfo.compontProcessTypeStr = type.ProcessingType;
                            }
                        }

                        staffInfo.authority = new List<StaffAuthorityInfo>();
                        staffInfo.authority = GetAuthorityByStaff(0, staff.ID, Convert.ToInt32(staff.PosiationID), GetAuthorityIdByStaff(staff.ID, Convert.ToInt32(staff.PosiationID)));



                    }
                }
                return staffInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据人员id获取权限主键Id列表
        /// </summary>
        /// <param name="staffId">员工Id</param>
        /// <returns>0：职位的权限    1：人员特殊权限</returns>
        public static Dictionary<int, List<int>> GetAuthorityIdByStaff(int staffId, int positionId)
        {
            Dictionary<int, List<int>> authorityIds = new Dictionary<int, List<int>>();
            authorityIds.Add(0, new List<int>());
            authorityIds.Add(1, new List<int>());
            using (Model1 model = new Model1())
            {
                var positionAuthority = model.JDJS_PDMS_Position_Authority_Table.Where(r => r.PositionID == positionId && r.state == "正常");
                foreach (var item in positionAuthority)
                {
                    authorityIds[0].Add(Convert.ToInt32(item.AuthorityID));
                }
                var staffAuthority = model.JDJS_PDMS_Staff_Authority_Table.Where(r => r.StaffID == staffId && r.state == "正常");
                foreach (var item in staffAuthority)
                {
                    authorityIds[1].Add(Convert.ToInt32(item.AuthorityID));
                }
            }
            return authorityIds;
        }

        /// <summary>
        /// 获取人员权限
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="staffId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public static List<StaffAuthorityInfo> GetAuthorityByStaff(int parentID, int staffId, int positionId, Dictionary<int, List<int>> listIds)
        {

            try
            {
                List<StaffAuthorityInfo> staffInfos = new List<StaffAuthorityInfo>();
                using (Model1 model = new Model1())
                {
                    var limlts = model.JDJS_PDMS_Limits_Of_Authority_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in limlts)
                    {
                        StaffAuthorityInfo authority = new StaffAuthorityInfo();
                        authority.authorityName = item.AuthorityName;
                        authority.disPlayName = item.DisPlayName;
                        authority.id = item.ID;
                        authority.@checked = false;
                        if (listIds[0].Contains(item.ID) || listIds[1].Contains(item.ID))
                        {
                            authority.@checked = true;
                        }
                        authority.enabled = "true";
                        if (listIds[0].Contains(item.ID))
                        {
                            authority.enabled = "false";
                        }
                        authority.state = "open";
                        authority.parentID = parentID;
                        authority.children = new List<StaffAuthorityInfo>();
                        authority.children = GetAuthorityByStaff(item.ID, staffId, positionId, listIds);
                        if (authority.children.Count() > 0)
                        {
                            //authority.enabled = "true";
                            authority.@checked = false;
                            authority.state = "closed";
                        }

                        staffInfos.Add(authority);
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
        /// 修改员工信息
        /// </summary>
        /// <param name="staffId">员工主键id</param>
        /// <param name="staffName">员工名称</param>
        /// <param name="personId">修改人员id</param>
        /// <param name="tel">电话</param>
        /// <param name="userName">登录名</param>
        /// <param name="passWord">密码</param>
        /// <param name="mailBox">邮箱</param>
        /// <param name="processTypeId">组别类型可空</param>
        /// <param name="authorityList">权限列表</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool AlterEmployeeInfo(int staffId, int positionId, string staffName, int personId, string tel, string userName, string passWord, string mailBox, int? processTypeId, int? compontProcessTypeId, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId && r.state == "正常").FirstOrDefault();
                    if (staff == null)
                    {
                        errMsg = "该员工不存在，请确认后再试！";
                        return false;
                    }
                    if (staff.Users != userName)
                    {
                        var staffold = model.JDJS_PDMS_Staff_Table.Where(r => r.Users == userName && r.state == "正常");
                        if (staffold.Count() > 0)
                        {
                            errMsg = "该登录名已存在，请确认后再试！";
                            return false;
                        }
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            DatabaseList da = DatabaseList.GetData();
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().StaffName = staffName;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().LastAlterTime = DateTime.Now;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().Tel = tel;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().LastAlterPersonID = personId;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().MailBox = mailBox;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().PassWord = passWord;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().ProcessTypeID = processTypeId;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().CompontProcessTypeID = compontProcessTypeId;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().Users = userName;
                            da.JDJS_PDMS_Staff_Table.Where(r => r.ID == staffId).First().PosiationID = positionId;

                            staff.StaffName = staffName;
                            staff.Tel = tel;
                            staff.LastAlterPersonID = personId;
                            staff.LastAlterTime = DateTime.Now;
                            staff.MailBox = mailBox;
                            staff.PassWord = passWord;
                            staff.ProcessTypeID = processTypeId;
                            staff.CompontProcessTypeID = compontProcessTypeId;
                            staff.Users = userName;
                            staff.PosiationID = positionId;

                            //var list = GetAuthorityIdByStaff(staff.ID, Convert.ToInt32(staff.PosiationID));
                            //foreach (var item in list[0])
                            //{
                            //    if (authorityList.Contains(item))
                            //    {
                            //        authorityList.Remove(item);
                            //    }
                            //}
                            //foreach (var item in list[1])
                            //{
                            //    if (!authorityList.Contains(item))
                            //    {
                            //        var staffAuthority = model.JDJS_PDMS_Staff_Authority_Table.Where(r => r.StaffID == staffId && r.AuthorityID == item && r.state == "正常");
                            //        foreach (var real in staffAuthority)
                            //        {
                            //            real.state = "删除";
                            //            real.LastAlterPersonID = personId;
                            //            real.LastAlterTime = DateTime.Now;
                            //        }
                            //    }
                            //}
                            //foreach (var item in authorityList)
                            //{
                            //    if (!list[1].Contains(item))
                            //    {
                            //        JDJS_PDMS_Staff_Authority_Table jd = new JDJS_PDMS_Staff_Authority_Table()
                            //        {
                            //            AuthorityID = item,
                            //            CreatPersonID = personId,
                            //            CreatTime = DateTime.Now,
                            //            LastAlterPersonID = personId,
                            //            LastAlterTime = DateTime.Now,
                            //            StaffID = staffId,
                            //            state = "正常"
                            //        };
                            //        model.JDJS_PDMS_Staff_Authority_Table.Add(jd);
                            //    }
                            //}

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
        /// 员工修改自己密码
        /// </summary>
        /// <param name="perosnId"></param>
        /// <param name="passwordOld"></param>
        /// <param name="passwordNew"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool AlterItemselfPassword(int perosnId, string passwordOld, string passwordNew, ref string errMsg)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var staff = model.JDJS_PDMS_Staff_Table.Where(r => r.ID == perosnId && r.state == "正常").FirstOrDefault();
                    if (staff == null)
                    {
                        errMsg = "该员工不存在，请确认后再试！";
                        return false;
                    }
                    if (staff.PassWord != passwordOld)
                    {
                        errMsg = "原始密码错误，请确认后再试！";
                        return false;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            staff.PassWord = passwordNew;
                            staff.LastAlterPersonID = perosnId;
                            staff.LastAlterTime = DateTime.Now;
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

    }
    public class EmployeeInfo
    {

    }

    public class AuthorityInfo
    {
        public int id { get; set; }
        public string authorityName { get; set; }
        public int parentID { get; set; }
        public bool @checked { get; set; }
        public string disPlayName { get; set; }
        public string state { get; set; }
        public List<AuthorityInfo> children { get; set; }
    }

    public class StaffAuthorityInfo
    {
        public int id { get; set; }
        public string authorityName { get; set; }
        public int parentID { get; set; }
        public bool @checked { get; set; }
        public string enabled { get; set; }
        public string disPlayName { get; set; }
        public string state { get; set; }
        public List<StaffAuthorityInfo> children { get; set; }
    }



    public class PositionInfo
    {
        /// <summary>
        /// 职位主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 职位名称
        /// </summary>
        public string positionName;
    }


    public class StaffInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string staff;
        /// <summary>
        /// 职位
        /// </summary>
        public string position;
        /// <summary>
        /// 组别类型ID
        /// </summary>
        public int processTypeId;
        //组别名称
        public string processTypeStr;
        /// <summary>
        /// 组别类型ID
        /// </summary>
        public int compontProcessTypeId;
        //组别名称
        public string compontProcessTypeStr;
        /// <summary>
        /// 电话
        /// </summary>
        public string tel;
        /// <summary>
        /// 用户名
        /// </summary>
        public string users;
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string mailbox;
        public List<StaffAuthorityInfo> authority;
    }


    public class StaffInfoReadClass
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 唯一列，用来区分
        /// </summary>
        public string allID;
        /// <summary>
        /// 员工姓名或者部门名
        /// </summary>
        public string staff;
        /// <summary>
        /// 职位
        /// </summary>
        public string position;

        public string enabled;
        /// <summary>
        /// 组别类型ID
        /// </summary>
        public int processTypeId;
        //组别名称
        public string processTypeStr;

        /// <summary>
        /// 组别类型ID
        /// </summary>
        public int compontProcessTypeId;
        //组别名称
        public string compontProcessTypeStr;
        /// <summary>
        /// 电话
        /// </summary>
        public string tel;
        /// <summary>
        /// 用户名
        /// </summary>
        public string users;
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string mailbox;
        public bool ispart;
        public List<StaffInfoReadClass> children = new List<StaffInfoReadClass>();
    }
}