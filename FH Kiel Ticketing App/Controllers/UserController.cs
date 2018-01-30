using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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

        //Login
        public ActionResult Login()
        {
            if (Request.Cookies["UserCookie"] != null)
            {
                return RedirectToAction("Index", Request.Cookies["UserCookie"]["UserRole"].ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string returnUrl = "")
        {
            KillUserCookie();
            using (TicketingApp db = new TicketingApp())
            {
                var loggedInUser = db.User.Where(u => u.email == user.email).FirstOrDefault();
                if (loggedInUser != null)
                {
                    if (Crypto.VerifyHashedPassword(loggedInUser.password, user.password))
                    {
                        //Check if user is verified
                        if (!loggedInUser.isEmailVerified)
                        {
                            ModelState.AddModelError("", "Your email isn't verified. Please verify it before logging in.");
                            return View();
                        }

                        //Checking the role
                        MailAddress address = new MailAddress(loggedInUser.email);
                        string identifier = address.Host;

                        var role = db.RoleIdentifier
                            .Join(db.RoleIdentifierDetails,
                                roleIdentifier => roleIdentifier.recordID,
                                roleIdentifierDetails => roleIdentifierDetails.RoleIdentifier.recordID,
                                (roleIdentifier, roleIdentifierDetails) => new { RoleIdentifier = roleIdentifier, RoleIdentifierDetails = roleIdentifierDetails })
                            .Where(roleAndDetails => roleAndDetails.RoleIdentifierDetails.identifier == identifier).FirstOrDefault();

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            // Making the Cookie
                            HttpCookie httpCookie = new HttpCookie("UserCookie");

                            httpCookie["UserID"] = loggedInUser.recordID.ToString();
                            httpCookie["UserRole"] = role.RoleIdentifier.role;
                            httpCookie.Expires = DateTime.Now.AddMinutes(30);
                            Response.Cookies.Add(httpCookie);
                            return RedirectToAction("Index", Request.Cookies["UserCookie"]["UserRole"].ToString());
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The username or password is wrong");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "The username or password is wrong");
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            KillUserCookie();
            return RedirectToAction("Login");
        }

        [NonAction]
        private void KillUserCookie()
        {
            if (Request.Cookies["UserCookie"] != null)
            {
                var c = new HttpCookie("UserCookie");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
        }

        //Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            bool status = false;
            string message = "";
            string roleOfUser = "";
            if (ModelState.IsValid)
            {
                //Check if Email already exists
                var emailExists = DoesEmailExist(user.email);

                if (emailExists)
                {
                    ModelState.AddModelError("EmailExists", "The email you provided already exists");
                    return View(user);
                }

                //Generating Activation Code 
                user.activationCode = Guid.NewGuid();

                //Hashing the Password
                user.password = Crypto.HashPassword(user.password);
                user.confirmPassword = user.password; // To avoid EntityValidationError

                user.isEmailVerified = false;
                user.emailNotification = true;

                using (TicketingApp db = new TicketingApp())
                {
                    //Checking the role of the user registering
                    MailAddress address = new MailAddress(user.email);
                    string identifier = address.Host;

                    var role = db.RoleIdentifier
                        .Join(db.RoleIdentifierDetails,
                            roleIdentifier => roleIdentifier.recordID,
                            roleIdentifierDetails => roleIdentifierDetails.RoleIdentifier.recordID,
                            (roleIdentifier, roleIdentifierDetails) => new { RoleIdentifier = roleIdentifier, RoleIdentifierDetails = roleIdentifierDetails })
                        .Where(roleAndDetails => roleAndDetails.RoleIdentifierDetails.identifier == identifier).FirstOrDefault();
                    if (role != null)
                    {
                        db.User.Add(user);
                        if (role.RoleIdentifier.role == "Student")
                        {
                            Student student = new Student();
                            student.recordID = user.recordID;
                            student.userType = "Student";
                            db.Student.Add(student);
                            db.SaveChanges();
                            roleOfUser = "Student";
                        }
                        else if (role.RoleIdentifier.role == "Supervisor")
                        {
                            Supervisor supervisor = new Supervisor();
                            supervisor.recordID = user.recordID;
                            supervisor.userType = "Supervisor";
                            db.Supervisor.Add(supervisor);
                            db.SaveChanges();
                            roleOfUser = "Supervisor";
                        }

                        // Sending Activation Email
                        string subject = "Your account is successfully created";

                        string body = "<br/><br/>So you want to join the ticketing world? One more step and you're done." +
                            " Please Click on the link below to verify your account.";
                        string targetUrl = "/User/VerifyAccount/";
                        SendVerificationEmail(user.recordID, user.email, user.activationCode.ToString(), targetUrl, subject, body);
                        status = true;
                        message = "Your account is now created. Please check your email for an activation code.";
                    }
                    else
                    {
                        message = "Your email maybe valid but seems like you're not recognized by our system. Please check if it's correct.";
                    }

                }
                ModelState.Clear();
            }
            else
            {
                message = "Something weird happened. Developers, could you check the Register Action in your controller?";
            }

            ViewBag.role = roleOfUser;
            ViewBag.message = message;
            ViewBag.status = status;

            return View(user);
        }

        //To check if email exists
        [NonAction]
        public bool DoesEmailExist(string email)
        {
            using (TicketingApp db = new TicketingApp())
            {
                var user = db.User.Where(u => u.email == email).FirstOrDefault();
                return user != null;
            }
        }

        [NonAction]
        public void SendVerificationEmail(int recordID, string email, string activationCode, string targetUrl, string subject, string body)
        {
            var verifyUrl = targetUrl + "?ac=" + activationCode + "&id=" + recordID;
            var finalLink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("noreply.fhticketingapp@gmail.com", "The Ticketing Robot");
            var toEmail = new MailAddress(email);
            var fromPassword = "thisisalongpassword";
            body += " <br/><br/><a href='" + finalLink + "'>" + finalLink + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
        }

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string ac, int? id)
        {

            bool status = false;
            User user = new Models.User();
            using (TicketingApp db = new TicketingApp())
            {
                // To avoid ConfirmPassword does not match issue
                db.Configuration.ValidateOnSaveEnabled = false;
                if (ac != null)
                {
                    user = db.User.Where(u => u.activationCode == new Guid(ac) && u.recordID == id).FirstOrDefault();
                }
                else
                {
                    user = null;
                }

                if (user != null)
                {
                    user.isEmailVerified = true;
                    user.activationCode = Guid.Empty;
                    db.SaveChanges();
                    status = true;
                }
                else
                {
                    ViewBag.Message = "We cannot find your data in our system. Are you sure you're registered? Or maybe you're already verified";
                }
            }

            ViewBag.Status = status;
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            bool status = false;
            string message = "";

            using (TicketingApp db = new TicketingApp())
            {
                var user = db.User.Where(u => u.email == email).FirstOrDefault();

                if (user != null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;

                    //Giving the user a new GUID
                    user.activationCode = Guid.NewGuid();
                    db.SaveChanges();

                    // Sending Password Reset Email
                    string subject = "Ticketing App - Forgot Password";

                    string body = "<br/><br/>If this wasn't done by you then please ignore this email. " +
                        "Otherwise Please Click on the link below to reset your password.";
                    string targetUrl = "/User/ResetPassword/";

                    SendVerificationEmail(user.recordID, user.email, user.activationCode.ToString(), targetUrl, subject, body);
                    status = true;
                    message = "Please check your email for a reset password link.";
                }
                else
                {
                    message = "We can't seem to find your email. Are you sure it's enterred correctly?";
                }
            }
            ViewBag.Status = status;
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(int? id, string ac)
        {
            bool status = false;
            string message = null;
            User user = new Models.User();

            using (TicketingApp db = new TicketingApp())
            {
                if (ac != null)
                {
                    user = db.User.Where(u => u.activationCode == new Guid(ac) && u.recordID == id).FirstOrDefault();
                }
                else
                {
                    user = null;
                }

                db.Configuration.ValidateOnSaveEnabled = false;
                if (user != null)
                {
                    user.activationCode = Guid.Empty;
                    db.SaveChanges();
                }
                else
                {
                    message = "Maybe you're not authorized to see this page. We're also confused how did you end up here";
                }
            }
            ViewBag.Status = status;
            ViewBag.Message = message;
            return View("ResetPassword", new User
            {
                recordID = user.recordID,
                firstName = user.firstName,
                email = user.email
            });
        }


        [HttpPost]
        public ActionResult ResetPassword(User postedUser)
        {
            bool status = false;
            string message = "";

            using (TicketingApp db = new TicketingApp())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var user = db.User.Where(u => u.recordID == postedUser.recordID).FirstOrDefault();

                if (user != null)
                {
                    user.password = Crypto.HashPassword(postedUser.password);
                    user.confirmPassword = user.password; // To avoid EntityValidationError
                    db.SaveChanges();
                    status = true;
                    message = "Your password has been successfully changed.";
                }
                else
                {
                    message = "Something ugly happened behind the scenes. Maybe you can try again later.";
                }

            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View();
        }
    }


}
