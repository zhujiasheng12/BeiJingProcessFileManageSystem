using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.Database
{
    /// <summary>
    /// 职位表
    /// </summary>
    public class JDJS_PDMS_Position_Table
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int ID { get; set; }
        public int? DepartmentID { get; set; }
        public string PositionName { get; set; }
        public DateTime? CreatTime { get; set; }
        public int? CreatPersonID { get; set; }
        public DateTime? LastAlterTime { get; set; }
        public int? LastAlterPersonID { get; set; }
        public string state { get; set; }

    }
}