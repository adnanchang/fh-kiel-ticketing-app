using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        // GET: Notification/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Notification/Create
        public ActionResult Create()
        {
            string message = TempData["message"].ToString();
            string targetURL = TempData["targetURL"].ToString();
            List<User> users = (List<User>)TempData["users"];
            string returnURLName = TempData["returnURLName"].ToString();
            string returnURLController = TempData["returnURLController"].ToString();
            return Create(message, targetURL, users, returnURLName, returnURLController);
        }

        // POST: Notification/Create
        [HttpPost]
        public ActionResult Create(string message, string targetURL, List<User> users, string returnURLName, string returnURLController)
        {
            try
            {
                // Let's create a notification
                foreach (var user in users)
                {
                    var notification = new Notification();
                    notification.message = message;
                    notification.url = targetURL;
                    notification.isRead = false;

                    using (TicketingApp db = new TicketingApp())
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        var temp = db.User.Where(u => u.recordID == user.recordID).FirstOrDefault();
                        temp.Notification.Add(notification);
                        db.Notification.Add(notification);
                        db.SaveChanges();
                    }

                    if (user.emailNotification == true)
                    {
                        SendNotificationEmail(message, targetURL, user);
                    }
                }

                return RedirectToAction(returnURLName, returnURLController);
                
            }
            catch
            {
                return View();
            }
        }

        // GET: Notification/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Notification/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Notification/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notification/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Sends a notification Email to the user
        /// </summary>
        /// <param name="message">The message of the notification</param>
        /// <param name="targetURL">The target URL</param>
        /// <param name="user">The user it has to be sent to</param>
        [NonAction]
        public void SendNotificationEmail(string message, string targetURL, User user)
        {
            //var verifyUrl = targetUrl + "?ac=" + activationCode + "&id=" + recordID;
            var finalLink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, targetURL);

            var subject = "Ticketing App Alert";
            var fromEmail = new MailAddress("noreply.fhticketingapp@gmail.com", "The Ticketing Robot");
            var toEmail = new MailAddress(user.email);
            var fromPassword = "thisisalongpassword";
            message += " <br/><br/><a href='" + finalLink + "'>" + finalLink + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromPassword)
            };

            using (var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            })

                smtp.Send(mail);
        }

        [NonAction]
        public int GetUserID()
        {
            int userID = -1;
            if (Request.Cookies["UserCookie"] != null)
            {
                userID = Convert.ToInt32(Request.Cookies["UserCookie"]["UserID"].ToString());
            }
            return userID;
        }
    }
}
