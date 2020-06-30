using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 北京工艺文件管理MVC.Database;
using 北京工艺文件管理MVC.DBDatabase;

namespace 北京工艺文件管理MVC.Models
{
    public class DatabaseList
    {
        //单例模式，每次获取这个类的实例对象都是同一个
        private static DatabaseList data = new DatabaseList() ;
        private DatabaseList()
        {
            using (Model1 model = new Model1())
            {
                this.JDJS_PDMS_Compont_CraftFile_Alter_History_Table = model.JDJS_PDMS_Compont_CraftFile_Alter_History_Table.ToList();
                this.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table = model.JDJS_PDMS_Compont_CraftInfo_Alter_History_Table.ToList();
                this.JDJS_PDMS_Compont_Preview_Category_Table = model.JDJS_PDMS_Compont_Preview_Category_Table.ToList();
                this.JDJS_PDMS_Compont_ProcessInfo_Table = model.JDJS_PDMS_Compont_ProcessInfo_Table.ToList();
                this.JDJS_PDMS_Compont_ProcessType_Table = model.JDJS_PDMS_Compont_ProcessType_Table.ToList();
                this.JDJS_PDMS_CompontInfo_Alter_History_Table = model.JDJS_PDMS_CompontInfo_Alter_History_Table.ToList();
                this.JDJS_PDMS_CompontInfo_Table = model.JDJS_PDMS_CompontInfo_Table.ToList();
                this.JDJS_PDMS_CraftFile_Alter_History_Table = model.JDJS_PDMS_CraftFile_Alter_History_Table.ToList();
                this.JDJS_PDMS_CraftInfo_Alter_History_Table = model.JDJS_PDMS_CraftInfo_Alter_History_Table.ToList();
                this.JDJS_PDMS_Department_Table = model.JDJS_PDMS_Department_Table.ToList();
                this.JDJS_PDMS_Device_Brand_Info = model.JDJS_PDMS_Device_Brand_Info.ToList();
                this.JDJS_PDMS_Device_Info = model.JDJS_PDMS_Device_Info.ToList();
                this.JDJS_PDMS_Device_Type_Info = model.JDJS_PDMS_Device_Type_Info.ToList();
                this.JDJS_PDMS_Limits_Of_Authority_Table = model.JDJS_PDMS_Limits_Of_Authority_Table.ToList();
                this.JDJS_PDMS_Location_Info = model.JDJS_PDMS_Location_Info.ToList();
                this.JDJS_PDMS_Position_Authority_Table = model.JDJS_PDMS_Position_Authority_Table.ToList();
                this.JDJS_PDMS_Position_Table = model.JDJS_PDMS_Position_Table.ToList();
                this.JDJS_PDMS_Staff_Authority_Table = model.JDJS_PDMS_Staff_Authority_Table.ToList();
                this.JDJS_PDMS_Staff_Table = model.JDJS_PDMS_Staff_Table.ToList();
                this.JDJS_PDMS_Task_Preview_Category_Table = model.JDJS_PDMS_Task_Preview_Category_Table.ToList();
                this.JDJS_PDMS_Task_ProcessInfo_Table = model.JDJS_PDMS_Task_ProcessInfo_Table.ToList();
                this.JDJS_PDMS_Task_ProcessType_Table = model.JDJS_PDMS_Task_ProcessType_Table.ToList();
                this.JDJS_PDMS_TaskInfo_Alter_History_Table = model.JDJS_PDMS_TaskInfo_Alter_History_Table.ToList();
                this.JDJS_PDMS_TaskInfo_Table = model.JDJS_PDMS_TaskInfo_Table.ToList();
            }
                
        }
        public static DatabaseList GetData()
        {
            if (data == null)
            {
                data = new DatabaseList();
            }
            return data;
        }
        public List<JDJS_PDMS_Compont_CraftFile_Alter_History_Table> JDJS_PDMS_Compont_CraftFile_Alter_History_Table=new List<JDJS_PDMS_Compont_CraftFile_Alter_History_Table> ();
        public List<JDJS_PDMS_Compont_CraftInfo_Alter_History_Table> JDJS_PDMS_Compont_CraftInfo_Alter_History_Table=new List<JDJS_PDMS_Compont_CraftInfo_Alter_History_Table> ();
        public List<JDJS_PDMS_Compont_Preview_Category_Table> JDJS_PDMS_Compont_Preview_Category_Table=new List<JDJS_PDMS_Compont_Preview_Category_Table> ();
        public List<JDJS_PDMS_Compont_ProcessInfo_Table> JDJS_PDMS_Compont_ProcessInfo_Table=new List<JDJS_PDMS_Compont_ProcessInfo_Table> ();
        public List<JDJS_PDMS_Compont_ProcessType_Table> JDJS_PDMS_Compont_ProcessType_Table=new List<JDJS_PDMS_Compont_ProcessType_Table> ();
        public List<JDJS_PDMS_CompontInfo_Alter_History_Table> JDJS_PDMS_CompontInfo_Alter_History_Table=new List<JDJS_PDMS_CompontInfo_Alter_History_Table> ();
        public List<JDJS_PDMS_CompontInfo_Table> JDJS_PDMS_CompontInfo_Table=new List<JDJS_PDMS_CompontInfo_Table> ();
        public List<JDJS_PDMS_CraftFile_Alter_History_Table> JDJS_PDMS_CraftFile_Alter_History_Table=new List<JDJS_PDMS_CraftFile_Alter_History_Table> ();
        public List<JDJS_PDMS_CraftInfo_Alter_History_Table> JDJS_PDMS_CraftInfo_Alter_History_Table=new List<JDJS_PDMS_CraftInfo_Alter_History_Table> ();
        public List<JDJS_PDMS_Department_Table> JDJS_PDMS_Department_Table=new List<JDJS_PDMS_Department_Table> ();
        public List<JDJS_PDMS_Device_Brand_Info> JDJS_PDMS_Device_Brand_Info=new List<JDJS_PDMS_Device_Brand_Info> ();
        public List<JDJS_PDMS_Device_Info> JDJS_PDMS_Device_Info=new List<JDJS_PDMS_Device_Info> ();
        public List<JDJS_PDMS_Device_Type_Info> JDJS_PDMS_Device_Type_Info=new List<JDJS_PDMS_Device_Type_Info> ();
        public List<JDJS_PDMS_Limits_Of_Authority_Table> JDJS_PDMS_Limits_Of_Authority_Table=new List<JDJS_PDMS_Limits_Of_Authority_Table> ();
        public List<JDJS_PDMS_Location_Info> JDJS_PDMS_Location_Info=new List<JDJS_PDMS_Location_Info> ();
        public List<JDJS_PDMS_Position_Authority_Table> JDJS_PDMS_Position_Authority_Table=new List<JDJS_PDMS_Position_Authority_Table> ();
        public List<JDJS_PDMS_Position_Table> JDJS_PDMS_Position_Table=new List<JDJS_PDMS_Position_Table> ();
        public List<JDJS_PDMS_Staff_Authority_Table> JDJS_PDMS_Staff_Authority_Table=new List<JDJS_PDMS_Staff_Authority_Table> ();
        public List<JDJS_PDMS_Staff_Table>JDJS_PDMS_Staff_Table=new List<JDJS_PDMS_Staff_Table> ();
        public List<JDJS_PDMS_Task_Preview_Category_Table> JDJS_PDMS_Task_Preview_Category_Table=new List<JDJS_PDMS_Task_Preview_Category_Table> ();
        public List<JDJS_PDMS_Task_ProcessInfo_Table> JDJS_PDMS_Task_ProcessInfo_Table=new List<JDJS_PDMS_Task_ProcessInfo_Table> ();
        public List<JDJS_PDMS_Task_ProcessType_Table> JDJS_PDMS_Task_ProcessType_Table=new List<JDJS_PDMS_Task_ProcessType_Table> ();
        public List<JDJS_PDMS_TaskInfo_Alter_History_Table> JDJS_PDMS_TaskInfo_Alter_History_Table=new List<JDJS_PDMS_TaskInfo_Alter_History_Table> ();
        public List<JDJS_PDMS_TaskInfo_Table> JDJS_PDMS_TaskInfo_Table=new List<JDJS_PDMS_TaskInfo_Table> ();
    }
}