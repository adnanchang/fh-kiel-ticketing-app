using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class SupervisorController : Controller
    {
        TicketingApp db = new TicketingApp();
        // GET: Supervisor
        public ActionResult Index()
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                int userID = GetUserID();
                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var supervisor = db.Supervisor.Where(s => s.recordID == userID).FirstOrDefault();
                var tickets = db.Ticket.Where(t => t.recordID > 0).ToList();
                var ticket = db.Ticket.Where(t => t.recordID > 0).FirstOrDefault();
                var idea = db.Idea.Where(i => i.User.recordID == userID).ToList().Take(5).ToList();

                var sprTicketView = new SuperVisorTicketViewModel
                {
                    user = user,
                    supervisor = supervisor,
                    availableIdeas = idea,
                    ticket = ticket,
                    tickets = tickets
                };
                return View(sprTicketView);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: Supervisor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Supervisor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supervisor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Supervisor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (IsLoggedIn() && IsAuthorized())
            {
                var user = db.User.Where(u => u.recordID == id).FirstOrDefault();
                var supervisor = db.Supervisor.Where(s => s.recordID == id).FirstOrDefault();
                var fields = db.Fields.ToList();
                var supervisorFields = db.Fields.Where(f => f.Supervisor.Any(s => s.recordID == id)).ToList();
                var selectedFields = supervisorFields.Select(x => x.recordID).ToArray();

                var supervisorUser = new SupervisorUserFieldViewModel
                {
                    user = user,
                    supervisor = supervisor,
                    SupervisorFields = supervisorFields,
                    AllFields = fields,
                    selectedFields = selectedFields
                };
                return View(supervisorUser);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: Supervisor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SupervisorUserFieldViewModel supervisorUser)
        {
            try
            {
                // TODO: Add update logic here
                ModelState.Remove("User.firstName");
                ModelState.Remove("User.lastName");
                ModelState.Remove("User.email");
                ModelState.Remove("User.password");
                ModelState.Remove("User.confirmPassword");
                if (ModelState.IsValid)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    var supervisor = db.Supervisor.Where(s => s.recordID == supervisorUser.supervisor.recordID).FirstOrDefault();
                    var user = db.User.Where(u => u.recordID == supervisorUser.user.recordID).FirstOrDefault();
                    var fields = new List<Fields>();

                    user.emailNotification = supervisorUser.user.emailNotification;
                    if (supervisorUser.selectedFields != null)
                    {
                        foreach (int item in supervisorUser.selectedFields)
                        {
                            fields.Add(db.Fields.Where(f => f.recordID == item).FirstOrDefault());
                        }
                        supervisor.Fields.Clear();
                        foreach (var item in fields)
                        {
                            supervisor.Fields.Add(item);
                        }
                    }

                    if (supervisorUser.supervisor.daysForReport != null)
                    {
                        supervisor.daysForReport = supervisorUser.supervisor.daysForReport;
                    }
                    
                    db.SaveChanges();

                    //Checking if scheduler is needed
                    if (supervisor.daysForReport != "0")
                    {
                        //Checking for Quartz Scheduler
                        var scheduler = HttpContext.Application["Scheduler"] as IScheduler;

                        //Checking if the job is already added in the scheduler
                        JobKey jobKey = JobKey.Create("report-job", "report-job-group");
                        var reportJob = scheduler.GetJobDetail(jobKey);

                        if (reportJob == null)
                        {
                            //Preparing data
                            string subject = "Your Ticketing Report";
                            var sysUser = db.User.Where(u => u.recordID == 999999).FirstOrDefault();

                            IJobDetail job = JobBuilder.Create<ReportJob>()
                                .WithIdentity("report-job", "report-job-group")
                                .Build();
                            job.JobDataMap["subject"] = subject;
                            job.JobDataMap["user"] = user;
                            job.JobDataMap["sysUser"] = sysUser;

                            ITrigger trigger = TriggerBuilder.Create()
                                .WithSimpleSchedule(s => s.WithIntervalInSeconds(60).RepeatForever())
                                .Build();

                            scheduler.ScheduleJob(job, trigger);
                        }
                    }
                    
                    
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Supervisor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Supervisor/Delete/5
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

        [HttpGet]
        public ActionResult Notifications()
        {
            if (IsLoggedIn() && IsAuthorized())
            {

                int userID = GetUserID();

                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var unreadNotifications = db.Notification.Where(n =>
                        n.User.recordID == userID &&
                        n.isRead == false).ToList();
                var readNotifications = db.Notification.Where(n =>
                        n.User.recordID == userID &&
                        n.isRead == true).ToList();

                var viewModel = new SupervisorNotificationViewModel
                {
                    user = user,
                    unreadNotifications = unreadNotifications,
                    readNotifications = readNotifications
                };

                db.Configuration.ValidateOnSaveEnabled = false;
                foreach (var item in user.Notification)
                {
                    if (item.isRead == false)
                        item.isRead = true;
                }
                db.SaveChanges();
                return View(viewModel);

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        public ActionResult Notifications(FormCollection formCollection)
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                using (TicketingApp db = new TicketingApp())
                {
                    TempData["message"] = "This is a test notification sent at " + DateTime.Now.ToLongTimeString();
                    TempData["targetURL"] = "/User/Login";
                    TempData["users"] = db.User.ToList();
                    TempData["returnURLName"] = "Login";
                    TempData["returnURLController"] = "User";
                    return RedirectToAction("Create", "Notification", new { area = "" });
                }

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public ActionResult Idea()
        {
            int id = GetUserID();
            var user = db.User.Where(u => u.recordID == id).FirstOrDefault();
            var fields = db.Fields.ToList();

            var supervisorIdeaCreateViewModel = new SupervisorIdeaCreateViewModel
            {
                user = user,
                AllFields = fields
            };
            return View(supervisorIdeaCreateViewModel);
        }

        [HttpPost]
        public ActionResult Idea(Idea idea)
        {
            ModelState.Remove("User.firstName");
            ModelState.Remove("User.lastName");
            ModelState.Remove("User.email");
            ModelState.Remove("User.password");
            ModelState.Remove("User.confirmPassword");
            try
            {
                if (ModelState.IsValid)
                {
                    int userID = GetUserID();
                    var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();

                    idea.User = user;
                    db.Idea.Add(idea);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Message = "Something doesn't look right. Are you sure you filled the form correctly?";
                return View();
            }


        }

        [NonAction]
        public bool IsLoggedIn()
        {
            if (Request.Cookies["UserCookie"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        public bool IsAuthorized()
        {
            if (Request.Cookies["UserCookie"] != null)
            {
                if (Request.Cookies["UserCookie"]["UserRole"].ToString() == "Supervisor")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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

        [NonAction]
        public string GetUserRole()
        {
            string userRole = null;
            if (Request.Cookies["UserCookie"] != null)
            {
                userRole = Request.Cookies["UserCookie"]["UserRole"].ToString();
            }
            return userRole;
        }
    }

    public class ReportJob : IJob
    {
        public string subject { get; set; }
        public string body { get; set; }

        public User user { get; set; }

        public User sysUser { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            subject = dataMap.GetString("subject");
            user = (User)dataMap["user"];
            sysUser = (User)dataMap["sysUser"];

            //The body has to be refreshed everytime we send an email
            body = "This is a test email sent at " + DateTime.Now; 
            using (var message = new MailMessage(sysUser.email, user.email))
            {
                message.Subject = subject;
                message.Body = body;
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential(sysUser.email, sysUser.password)
                })
                {
                    client.Send(message);
                }
            }
        }
    }
}
