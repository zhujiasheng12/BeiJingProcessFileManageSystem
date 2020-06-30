using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.Database
{
    /// <summary>
    /// 工艺过程卡与进程单修改记录表
    /// </summary>
    public class JDJS_PDMS_Compont_CraftFile_Alter_History_Table
    {

        [Key]
        public int ID { get; set; }
        public int? CompontID { get; set; }
        public DateTime? AlterTime { get; set; }
        public string AlterDesc { get; set; }
        public int? staffID { get; set; }
        public DateTime? CreatTime { get; set; }
        public int? CreatPersonID { get; set; }
        public string state { get; set; }
    }
}