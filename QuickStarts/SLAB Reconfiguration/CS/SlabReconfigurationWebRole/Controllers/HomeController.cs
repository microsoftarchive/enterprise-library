#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Quick Start
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Web.Mvc;
using SlabReconfigurationWebRole.Models;

namespace SlabReconfigurationWebRole.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(MessageModel messageModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                MvcApplication.MessageSender.SendMessage(messageModel.Recipient, messageModel.Message);
            }
            catch (InvalidOperationException e)
            {
                return View("Error", (object)e.Message);
            }

            return RedirectToAction("Success");
        }

    }
}
