using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.DBDatabase
{
    /// <summary>
    /// 工艺工序修改记录表
    /// </summary>
    public class JDJS_PDMS_Compont_CraftInfo_Alter_History_Table
    {
        [Key]
        public int ID { get; set; }
        public int? ProcessID { get; set; }
        public DateTime? AlterTime { get; set; }
        public string AlterDesc { get; set; }
        public int? staffID { get; set; }
        public DateTime? CreatTime { get; set; }
        public int? CreatPersonID { get; set; }
        public string state { get; set; }

    }
}