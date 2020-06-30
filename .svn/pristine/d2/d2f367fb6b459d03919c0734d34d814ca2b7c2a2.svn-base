using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 工艺文件管理new.View.工艺文件管理
{
    /// <summary>
    /// optionsRead 的摘要说明
    /// </summary>
    public class optionsRead : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            string err="";
            var model = FuzzySearchReadTask(ref err);
            if (err == "")
            {
                var json = serializer.Serialize(model);
                context.Response.Write(json);
            }
            else { 
            
            }

        }
        /// <summary>
        /// 获取模糊搜索的字符串
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static Dictionary<string, List<string>> FuzzySearchReadTask(ref string errMsg)
        {
            try
            {
                Dictionary<string, List<string>> strList = new Dictionary<string, List<string>>();
                strList.Add("groupNum", new List<string>());
                strList.Add("groupName", new List<string>());
                strList.Add("taskNum", new List<string>());
                strList.Add("taskName", new List<string>());

                using (WebApplication2. Model1 mod = new WebApplication2. Model1())
                {

                    var tasks = mod.JDJS_PDMS_TaskInfo_Table;
                    if (tasks.Count() < 1)
                    {
                        errMsg = "当前无任务";
                        return null;
                    }
                    foreach (WebApplication2. JDJS_PDMS_TaskInfo_Table_Row task in tasks)
                    {
                        string groupNum = task.ComponentNum;
                        string taskNum = task.TaskNum;
                        string groupName = task.ComponentName;
                        string taskName = task.TaskName;
                        if (!strList["groupNum"].Contains(groupNum))
                        {
                            strList["groupNum"].Add(groupNum);
                        }
                        if (!strList["groupName"].Contains(groupName))
                        {
                            strList["groupName"].Add(groupName);
                        }
                        if (!strList["taskNum"].Contains(taskNum.Substring (task.ComponentNum .Length )))
                        {
                            strList["taskNum"].Add(taskNum.Substring(task.ComponentNum.Length));
                        }
                        if (!strList["taskName"].Contains(groupNum))
                        {
                            strList["taskName"].Add(groupNum);
                        }


                    }
                }

                return strList;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}