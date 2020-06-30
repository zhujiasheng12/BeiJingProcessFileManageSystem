﻿namespace 北京工艺文件管理MVC.Database
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using 北京工艺文件管理MVC.DBDatabase;

    public class Model1 : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“Model1”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“北京工艺文件管理MVC.Database.Model1”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“Model1”
        //连接字符串。
        public Model1()
            : base("name=DefaultConnection")
        {
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。
        public virtual DbSet<JDJS_PDMS_CompontInfo_Table> JDJS_PDMS_CompontInfo_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_CompontInfo_Alter_History_Table> JDJS_PDMS_CompontInfo_Alter_History_Table { get; set; }

        public virtual DbSet<JDJS_PDMS_Compont_CraftFile_Alter_History_Table> JDJS_PDMS_Compont_CraftFile_Alter_History_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Compont_CraftInfo_Alter_History_Table> JDJS_PDMS_Compont_CraftInfo_Alter_History_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Compont_Preview_Category_Table> JDJS_PDMS_Compont_Preview_Category_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Compont_ProcessInfo_Table> JDJS_PDMS_Compont_ProcessInfo_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Compont_ProcessType_Table> JDJS_PDMS_Compont_ProcessType_Table { get; set; }

        public virtual DbSet<JDJS_PDMS_CraftFile_Alter_History_Table> JDJS_PDMS_CraftFile_Alter_History_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_CraftInfo_Alter_History_Table> JDJS_PDMS_CraftInfo_Alter_History_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Department_Table> JDJS_PDMS_Department_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Device_Brand_Info> JDJS_PDMS_Device_Brand_Info { get; set; }
        public virtual DbSet<JDJS_PDMS_Device_Info> JDJS_PDMS_Device_Info { get; set; }
        public virtual DbSet<JDJS_PDMS_Device_Type_Info> JDJS_PDMS_Device_Type_Info { get; set; }
        public virtual DbSet<JDJS_PDMS_Limits_Of_Authority_Table> JDJS_PDMS_Limits_Of_Authority_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Location_Info> JDJS_PDMS_Location_Info { get; set; }
        public virtual DbSet<JDJS_PDMS_Position_Authority_Table> JDJS_PDMS_Position_Authority_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Position_Table> JDJS_PDMS_Position_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Staff_Authority_Table> JDJS_PDMS_Staff_Authority_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Staff_Table> JDJS_PDMS_Staff_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Task_Preview_Category_Table> JDJS_PDMS_Task_Preview_Category_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Task_ProcessInfo_Table> JDJS_PDMS_Task_ProcessInfo_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_Task_ProcessType_Table> JDJS_PDMS_Task_ProcessType_Table { get; set; }
        public virtual DbSet<JDJS_PDMS_TaskInfo_Alter_History_Table> JDJS_PDMS_TaskInfo_Alter_History_Table { get; set; }    
        public virtual DbSet<JDJS_PDMS_TaskInfo_Table> JDJS_PDMS_TaskInfo_Table { get; set; }       
    }

}