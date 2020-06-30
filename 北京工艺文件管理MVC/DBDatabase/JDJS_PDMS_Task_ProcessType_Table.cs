using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.Database
{
    /// <summary>
    /// 零件与预览类型对应表
    /// </summary>
    public class JDJS_PDMS_Task_ProcessType_Table
    {
        [Key]
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int CategoryTypeID { get; set; }
        public DateTime? CreatTime { get; set; }
        public int? CreatPersonID { get; set; }
        public DateTime? LastAlterTime { get; set; }
        public int? LastAlterPersonID { get; set; }
        public string state { get; set; }
    }
}