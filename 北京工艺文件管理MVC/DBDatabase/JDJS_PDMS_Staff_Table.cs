﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.Database
{
    /// <summary>
    /// 人员表
    /// </summary>
    public class JDJS_PDMS_Staff_Table
    {
        [Key]
        public int ID { get; set; }
        public string StaffName { get; set; }
        public int? PosiationID { get; set; }
        public string Tel { get; set; }
        public string Users { get; set; }
        public string PassWord { get; set; }
        public string MailBox { get; set; }
        public int? ProcessTypeID { get; set; }
        public int? CompontProcessTypeID { get; set; }
        public DateTime? CreatTime { get; set; }
        public int? CreatPersonID { get; set; }
        public DateTime? LastAlterTime { get; set; }
        public int? LastAlterPersonID { get; set; }
        public string state { get; set; }


    }
}