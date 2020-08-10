using MailSend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 北京工艺文件管理MVC.Models
{
    public class MailSendMethod
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiveAccount">接受人邮箱地址</param>
        /// <param name="mailContent">邮件内容</param>
        /// <param name="mailTitle">邮件主题</param>
        /// <param name="accessories">附件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否成功</returns>
        public static bool SendMail(string receiveAccount,string mailContent,string mailTitle,List<string> accessories, ref string errMsg)
        {
            try 
            {
                SupplementaryInformation sup = new SupplementaryInformation();
                string err = "";
                if (MailSend.Mail.Send(sup.GetSendAccount(), receiveAccount, sup.GetSenderName(), sup.GetSenderPassword(), sup.GetServerIP(), sup.GetServerPort(), mailContent, mailTitle, accessories, ref err))
                {
                    errMsg = "ok";
                    return true;
                }
                else
                {
                    errMsg = err;
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }
    }
}