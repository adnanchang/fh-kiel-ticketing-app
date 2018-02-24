using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System;
using System.Web;
using System.Collections.Generic;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class ProposalController : Controller
    {
        TicketingApp db = new TicketingApp();
        public bool IsPostBack { get; private set; }

        // GET: Proposal
        public ActionResult Index()
        {
            if (IsLoggedIn() && IsAuthorized())
            {

                int userID = GetUserID();

                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var student = db.Student.Where(s => s.recordID == userID).FirstOrDefault();
                var ticket = db.Ticket.Where(t => t.recordID > 0).FirstOrDefault();
                var idea = db.Idea.Where(i => i.User.recordID != userID).ToList();
                var proposal = db.Proposal.Where(p => p.User.recordID == userID).ToList();

                var proposalUser = new ProposalUserViewModel
                {
                    user = user,
                    student = student,
                    ticket = ticket,
                    availableIdeas = idea,
                    proposals = proposal
                };

                if (student.matrikelNumber == 0)
                {
                    ViewBag.IsDataSet = false;
                }

                return View(proposalUser);

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: Proposal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proposal/Create
        public ActionResult Create(int? id)
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                int userID = GetUserID();

                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var idea = db.Idea.Where(i => i.User.recordID != userID).ToList();
                var fields = db.Fields.ToList();
                var supervisors = db.Supervisor.ToList();

                ProposalIdeaFieldViewModel viewModel = null;
                if (id != null)
                {
                    var thisIdea = db.Idea.Where(i => i.recordID == id).FirstOrDefault();
                    //To get field name
                    int fieldID = Convert.ToInt32(thisIdea.field);
                    var field = db.Fields.Where(f => f.recordID == fieldID).FirstOrDefault();
                    thisIdea.field = field.field;
                    viewModel = new ProposalIdeaFieldViewModel
                    {
                        user = user,
                        availableIdeas = idea,
                        idea = thisIdea,
                        AllFields = fields,
                        AllSupervisor = supervisors
                    };
                    ViewBag.IdeaExists = true;
                }
                else
                {
                    viewModel = new ProposalIdeaFieldViewModel
                    {
                        user = user,
                        availableIdeas = idea,
                        AllFields = fields,
                        AllSupervisor = supervisors
                    };
                }

                return View(viewModel);

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: Proposal/Create
        [HttpPost]
        public ActionResult Create(ProposalIdeaFieldViewModel proposalIdeaFieldViewModel, int? id)
        {
            if (IsLoggedIn() && IsAuthorized())
            {

                proposalIdeaFieldViewModel.AllFields = db.Fields.ToList();
                proposalIdeaFieldViewModel.AllSupervisor = db.Supervisor.ToList();

                int userID = GetUserID();
                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                Idea idea = null;

                // No need to create a new idea if it already exists i.e. if a Professor has made an idea
                if (id != null)
                {
                    idea = db.Idea.Where(i => i.recordID == id).FirstOrDefault();
                    proposalIdeaFieldViewModel.proposal.nameOfProject = idea.title;
                }
                else
                {
                    idea = new Idea
                    {
                        title = proposalIdeaFieldViewModel.proposal.nameOfProject,
                        description = proposalIdeaFieldViewModel.proposal.abstrac,
                        type = proposalIdeaFieldViewModel.idea.type,
                        field = proposalIdeaFieldViewModel.idea.field,
                        User = user
                    };
                    db.Idea.Add(idea);
                    db.SaveChanges();
                }

                int ideaRecordId = idea.recordID;
                var ideaCreated = db.Idea.Where(i => i.recordID == ideaRecordId).FirstOrDefault();
                proposalIdeaFieldViewModel.proposal.User = user;
                proposalIdeaFieldViewModel.proposal.Idea = ideaCreated;
                db.Proposal.Add(proposalIdeaFieldViewModel.proposal);

                db.SaveChanges();

                var now = DateTime.Now;
                DateTime date = DateTime.Now;
                var ticketStatus = db.TicketStatus.Where(ts => ts.ticketStatus == "Proposal Awaiting Approval").FirstOrDefault();
                var ticket = new Ticket
                {
                    title = proposalIdeaFieldViewModel.proposal.nameOfProject,
                    timesRejected = 0,
                    User = user,
                    idea = ideaCreated,
                    creationDate = date,
                    tickettype = ideaCreated.type,
                    ticketStatus = ticketStatus
                };

                db.Ticket.Add(ticket);
                db.SaveChanges();
                int ticketRecordId = ticket.recordID;
                var ticketCreated = db.Ticket.Where(t => t.recordID == ticketRecordId).FirstOrDefault();

                string userRole = GetUserRole();
                var contributor = new Contributors
                {
                    status = "Pending",
                    Role = userRole,
                    User = user,
                    Ticket = ticketCreated
                };
                db.Contributors.Add(contributor);
                db.SaveChanges();
                var surperUser = db.User.Where(u => u.recordID == proposalIdeaFieldViewModel.supervisor).FirstOrDefault();

                var identifier = surperUser.email;
                var surperUserRole = db.RoleIdentifier
                       .Join(db.RoleIdentifierDetails,
                           roleIdentifier => roleIdentifier.recordID,
                           roleIdentifierDetails => roleIdentifierDetails.RoleIdentifier.recordID,
                           (roleIdentifier, roleIdentifierDetails) => new { RoleIdentifier = roleIdentifier, RoleIdentifierDetails = roleIdentifierDetails })
                       .Where(roleAndDetails => identifier.Contains(roleAndDetails.RoleIdentifierDetails.identifier)).FirstOrDefault();

                contributor.User = surperUser;
                contributor.Role = surperUserRole.RoleIdentifier.role;
                db.Contributors.Add(contributor);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: Proposal/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proposal/Edit/5
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

        // GET: Proposal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proposal/Delete/5
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


        // GET: Proposal/ExportPDF
        public ActionResult ExportPDF(int id)
        {
            Proposal proposals;
            Idea ideas;

            using (TicketingApp db = new TicketingApp())
            {
                proposals = db.Proposal.Where(u => u.recordID == id).FirstOrDefault();
                ideas = db.Idea.Where(i => i.recordID == proposals.Idea.recordID).FirstOrDefault();

            }

            Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            //var output = new FileStream(Server.MapPath("MyFirstPDF.pdf"), FileMode.Create);


            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();


                //var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/ABsIS_Logo.jpg"));
                //logo.SetAbsolutePosition(430, 770);
                //logo.ScaleAbsoluteHeight(30);
                //logo.ScaleAbsoluteWidth(70);
                //document.Add(logo);

                PdfPTable table1 = new PdfPTable(1);
                //table1.PaddingTop = 2f;
                PdfPCell cell11 = new PdfPCell();
                cell11.Border = Rectangle.NO_BORDER;
                Phrase phrase = null;

                phrase = new Paragraph();

                phrase.Add(new Chunk("Project Proposal\n\n\n", FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.BLACK)));

                phrase.Add(new Chunk("Name of the Project :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.nameOfProject + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));

                phrase.Add(new Chunk("Abstract :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.abstrac + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));

                phrase.Add(new Chunk("Proposal Type :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(ideas.type + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Introduction :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.introduction + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Overall Description :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.overallDescription + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Function Requirements :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.functionalRequirements + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Non-Function Requirements :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.nonFunctionalRequirements + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));

                phrase.Add(new Chunk("Project Technologies :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.projectTechnologies + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Result: :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.result + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));

                cell11.AddElement(phrase);
                cell11.VerticalAlignment = Element.ALIGN_RIGHT;

                table1.AddCell(cell11);



                document.Add(table1);
                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=Proposal.pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
            return null;
        }

        void generateThesisPDF()
        {
            string fileNameExisting = @"G:\old.pdf"; ;
            string fileNameNew = @"G:\new.pdf";
            using (FileStream existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
            using (FileStream newFileStream = new FileStream(fileNameNew, FileMode.Create))
            {
                // PDF öffnen
                PdfReader pdfReader = new PdfReader(existingFileStream);


                PdfStamper stamper = new PdfStamper(pdfReader, newFileStream);

                var form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;
                foreach (string fieldKey in fieldKeys)
                {
                    var value = pdfReader.AcroFields.GetField(fieldKey);
                    switch (fieldKey)
                    {
                        case "Name_Prof":
                            form.SetField(fieldKey, "Prof XYZ");
                            break;
                        case "Name":
                            form.SetField(fieldKey, "Murarka");
                            break;
                        case "Vorname":
                            form.SetField(fieldKey, "Pooja");
                            break;
                        case "Matrikel-Nr":
                            //form.SetFieldProperty(fieldKey, "texttype", 12f, null);
                            form.SetField(fieldKey, "900090");
                            break;
                        case "Studiengang":
                            //course of studies
                            form.SetField(fieldKey, "Information Engineering");
                            break;
                        case "Arb_Beginn":
                            form.SetField(fieldKey, "22.03.2018");
                            break;
                        case "Thema":
                            form.SetField(fieldKey, "Blah blah blah blah blah");
                            break;
                        case "Prüfer1":
                            form.SetField(fieldKey, "Mr 1");
                            break;
                        case "Prüfer2":
                            form.SetField(fieldKey, "Mr 2");
                            break;
                        case "öffentl":
                            //published                          
                            form.SetField(fieldKey, "nein");
                            break;
                    }

                }

                // Make text box unworkable (looks like normal text)
                //stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();
            }
        }
    }
}
