using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 北京工艺文件管理MVC.Database;

namespace 北京工艺文件管理MVC.Models
{
    public class DeleteOldTask
    {

        public static bool Delete()
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.state == "删除");
                    foreach (var item in tasks)
                    {
                        GetPartTaskInfo(item.ID);
                        GetTaskInfo(item.ID);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool  GetTaskInfo(int parentID )
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var compont = model.JDJS_PDMS_CompontInfo_Table.Where(r => r.ParentID == parentID && r.state == "正常");
                    foreach (var item in compont)
                    {
                        item.state = "删除";
                        item.LastAlterTime = DateTime.Now;
                        GetTaskInfo(item.ID);
                        GetPartTaskInfo(item.ID);
                    }
                    model.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据父节点获取零件信息
        /// </summary>
        /// <param name="parentID">父节点主键ID</param>
        /// <param name="orderNum">父节点的任务号</param>
        /// <returns></returns>
        public static bool GetPartTaskInfo( int parentID)
        {
            try
            {
                using (Model1 model = new Model1())
                {
                    var tasks = model.JDJS_PDMS_TaskInfo_Table.Where(r => r.ComponentID == parentID && r.state == "正常");
                    foreach (var item in tasks)
                    {
                        item.state = "删除";
                        item.LastAlterTime = DateTime.Now;
                    }
                    model.SaveChanges();
                }
                return true;
                       
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}