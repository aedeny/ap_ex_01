﻿using System;
using System.Web.Mvc;
using Communication;
using Infrastructure.Enums;
using Web.Models;

namespace Web.Controllers
{
    public class SettingsController : Controller
    {
        private Settings _settings;

        // GET: Settings
        public ActionResult Index()
        {
            _settings = new Settings();
            return View(_settings);
        }

        // GET: First/Delete/5
        public ActionResult DeleteDirectoryHandler(string dirHandlerToDelete)
        {
            string command = (int) CommandEnum.CloseDirectoryHandlerCommand + "|" + dirHandlerToDelete;
            GuiTcpClientSingleton.Instance.Writer.Write(command);
            return RedirectToAction("Index");
        }

        public ActionResult GoToDeleteConfirmationPage(string dirHandlerToDelete)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            throw new NotImplementedException();
        }
    }
}