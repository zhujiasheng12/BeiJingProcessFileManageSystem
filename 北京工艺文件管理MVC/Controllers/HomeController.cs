﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 北京工艺文件管理MVC.Database;

namespace 北京工艺文件管理MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Login()
        {
          
            return View();
        }

      public ActionResult 任务创建(){
          return View();
      }
      public ActionResult 创建产品弹窗()
      {
          return View();
      }
      
        public ActionResult 添加零件弹窗() {
            return View();
        }
        public ActionResult 任务进度() {
            return View();
        }
        public ActionResult 任务预览() {
            return View();
        }
        public ActionResult 修改零件弹窗() {
            return View();
        }
        public ActionResult 修改组件弹窗() {
            return View();
        }
        public ActionResult 工艺设计() {
            return View();
        }
        public ActionResult 工艺过程卡() {
            return View();
        }
        public ActionResult Com工艺过程卡()
        {
            return View();
        }
        public ActionResult 添加工序() {
            return View();
        }
        public ActionResult Com添加工序()
        {
            return View();
        }
        public ActionResult 工装图纸() {
            return View();
        }
        public ActionResult Com工装图纸()
        {
            return View();
        }
        public ActionResult 工艺进程单() {
            return View();
        }
        public ActionResult Com工艺进程单()
        {
            return View();
        }
        public ActionResult 工序文件() {
            return View();
        }
        public ActionResult Com工序文件()
        {
            return View();
        }
        public ActionResult 零件图纸()
        {
            return View();
        }
        public ActionResult 组件图纸()
        {
            return View();
        }
        public ActionResult 职位权限() {
            return View();
        }
        public ActionResult 修改职位弹窗() {
            return View();
         
        }
        public ActionResult 安排人员弹窗() {

            return View();
        }
        public ActionResult 修改工序责任人() {
            return View();
        }
    }
 
}
