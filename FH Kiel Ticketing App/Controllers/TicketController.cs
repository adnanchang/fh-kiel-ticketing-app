using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class TicketController : Controller
    {
        TicketingApp db = new TicketingApp();
        // GET: Ticket
        public ActionResult Index()
        {
            if (IsLoggedIn())
            {

                int userID = GetUserID();
                ViewBag.UserID = GetUserID();
                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();

                var student = db.Student.Where(s => s.recordID == userID).FirstOrDefault();
                var ticket = db.Ticket.Where(t => t.recordID > 0).FirstOrDefault();
                var idea = db.Idea.Where(i => i.User.recordID != userID).ToList();

                var studentUser = new StudentTicketViewModel
                {
                    user = user,
                    student = student,
                    ticket = ticket
                };

                if (student.matrikelNumber == 0)
                {
                    ViewBag.IsDataSet = false;
                }

                return View(studentUser);

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        //Get: Student/Ticket/

        public ActionResult Ticket(int id)
        {

            if (IsLoggedIn())
            {

                int userID = GetUserID();
                ViewBag.UserID = userID;
                ViewBag.UserRole = GetUserRole();

                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();

                var student = db.Student.Where(s => s.recordID == userID).FirstOrDefault();
                var ticket = db.Ticket.Where(t => t.recordID == id)
                                      .Include(t => t.ticketStatus)
                                      .FirstOrDefault();
                var comments = db.Comments.Where(c => c.Ticket.recordID == id).ToList();
                var contributersNames = (from u in db.User
                                         join c in db.Contributors on u.recordID equals c.User.recordID
                                         where c.Ticket.recordID == ticket.recordID




                                         select new StudentTicketViewModel.ContributingUsers // had to create new temp table with two columns
                                         {
                                             lastName = u.lastName,
                                             Role = c.Role
                                         }).ToList();
                var idea = db.Idea.Where(i => i.User.recordID != userID).ToList();
                var proposal = (from p in db.Proposal
                                join i in db.Idea on p.Idea.recordID equals i.recordID
                                join t in db.Ticket on i.recordID equals t.idea.recordID
                                select p
                                    ).FirstOrDefault();

                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"));
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                List<FileInfo> ListOffiles = dirInfo.GetFiles().ToList();
                var studentUser = new StudentTicketViewModel
                {
                    user = user,

                    student = student,
                    ticket = ticket,
                    availableIdeas = idea,
                    contributorsName = contributersNames,
                    comments = comments,
                    proposal = proposal,
                    files = ListOffiles
                };

                if (GetUserRole() != "Supervisor")
                {
                    if (student.matrikelNumber == 0)
                    {
                        ViewBag.IsDataSet = false;
                    }
                }


                return View(studentUser);

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        //Get: Ticket/ChangeStatus?status=

        public ActionResult ChangeStatus(int id, string status)
        {
            var ticket = db.Ticket.Where(t => t.recordID == id).FirstOrDefault();
            int userID = GetUserID();
            var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day,
                                    now.Hour, now.Minute, now.Second);


            /**
             * FIX THIS 
             **/
            //ticket.status = "Proposal Approved";
            //if (status == "rejected")
            //{
            //    ticket.status = "Proposal Rjected";
            //}
            //else if (status == "inprogress")
            //{
            //    ticket.status = "Proposal In Progress";
            //}
            //else if (status == "complete")
            //{
            //    ticket.status = "Completed Successfully";
            //}

            db.SaveChanges();

            var sysUser = db.User.Where(u => u.recordID == 999999).FirstOrDefault();

            var commnets = new Comments
            {
                Ticket = ticket,
                User = sysUser,
                CommentDate = date,
                Content = "Ticket Status Changed to <b> " + ticket.ticketStatus.ticketStatus + "</b> by " + user.firstName + " " + user.lastName,
            };
            db.Comments.Add(commnets);
            db.SaveChanges();

            return RedirectToAction("Ticket", new { id = id });
        }

        //Post: Ticket/PostComment
        [HttpPost]
        public ActionResult PostComment(StudentTicketViewModel model)
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day,
                                    now.Hour, now.Minute, now.Second);
            var ticket = db.Ticket.Where(t => t.recordID == model.ticket.recordID).FirstOrDefault();
            var user = db.User.Where(u => u.recordID == model.user.recordID).FirstOrDefault();
            var comment = new Comments
            {
                User = user,
                Ticket = ticket,
                CommentDate = date,
                Content = model.theComment.Content
            };
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Ticket", new { id = model.ticket.recordID });
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult UploadFile(StudentTicketViewModel model, HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), model.ticket.recordID + "_" + model.user.recordID + "_" + fileName);
                file.SaveAs(path);
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day,
                                        now.Hour, now.Minute, now.Second);
                var ticket = db.Ticket.Where(t => t.recordID == model.ticket.recordID).FirstOrDefault();
                var user = db.User.Where(u => u.recordID == model.user.recordID).FirstOrDefault();
                var sysUser = db.User.Where(u => u.recordID == 999999).FirstOrDefault();
                var comment = new Comments
                {
                    User = sysUser,
                    Ticket = ticket,
                    CommentDate = date,
                    Content = fileName + " upload by <b>" + user.firstName + " " + user.lastName + "</b>"
                };
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Ticket", new { id = model.ticket.recordID });
        }

        // GET: Ticket/ExportPDF
        public ActionResult DownloadPDF(string filename)
        {
            // var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), model.ticket.recordID + "_" + model.user.recordID + "_" + filename);
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/pdf";
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
            Response.Close();
            return null;
        }


        // GET: Ticket/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ticket/Create
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

        // GET: Ticket/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Ticket/Edit/5
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

        // GET: Ticket/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ticket/Delete/5
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
        public ActionResult Join(int? id)
        {
            var status = false;
            if (id == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                string userRole = GetUserRole();
                var date = DateTime.Now;
                int userID = GetUserID();
                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var ticket = db.Ticket.Where(t => t.recordID == id).FirstOrDefault();

                //Preparing viewmodel
                var viewModel = new ViewModelBase
                {
                    user = user
                };

                //Check if he already exists as a contributor
                var cont = db.Contributors.Where(c => c.User.recordID == user.recordID).FirstOrDefault();
                if (cont != null)
                {
                    ViewBag.Status = false;
                    ViewBag.Message = "It seems that you've already sent a request for this ticket. Please be patient.";
                    return View(viewModel);
                }

                //Add a new contributor
                var contributor = new Contributors
                {
                    status = "Pending",
                    Role = userRole,
                    User = user,
                    Ticket = ticket
                };
                db.Contributors.Add(contributor);

                //Add a comment in the ticket about the contributor being added
                var commnets = new Comments
                {
                    Ticket = ticket,
                    CommentDate = date,
                    Content = user.firstName + " " + user.lastName + " wants to join the ticket. Go to the members of the ticket to take further action."
                };
                db.Comments.Add(commnets);
                db.SaveChanges();
                status = true;
                ViewBag.Status = status;
                ViewBag.Message = "Your request has been sent. You will be notified once any action is taken.";

                return View(viewModel);
            }
        }

        //Get:/FileUpload/
        [HttpGet]
        public ActionResult Upload()
        {
            return View(); //create partial popup view with no layout.
        }
        [HttpPost]
        public ActionResult Upload(int Id)
        {
            return View();
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
                if (Request.Cookies["UserCookie"]["UserRole"].ToString() == "Student")
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
}
