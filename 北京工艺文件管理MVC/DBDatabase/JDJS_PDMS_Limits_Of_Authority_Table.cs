﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.Database
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class JDJS_PDMS_Limits_Of_Authority_Table
    {
        [Key]
        public int ID { get; set; }
        public string AuthorityName{get;set;}
        public int ParentID { get; set; }
        public string DisPlayName { get; set; }
        public DateTime? CreatTime { get; set; }
        public int? CreatPersonID { get; set; }
        public DateTime? LastAlterTime { get; set; }
        public int? LastAlterPersonID { get; set; }
        public string state { get; set; }
    }
}