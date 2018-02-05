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

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class ProposalController : Controller
    {
        public bool IsPostBack { get; private set; }

        // GET: Proposal
        public ActionResult Index()
        {
            using (TicketingApp db = new TicketingApp())
            {
                var proposals = db.Proposal.ToList();

                if (proposals != null)
                    return View(proposals);
                else
                    return View();
            }
        }

        // GET: Proposal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proposal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proposal/Create
        [HttpPost]
        public ActionResult Create(Proposal proposal)
        {
            try
            {
                // TODO: Add insert logic here

                using (TicketingApp db = new TicketingApp())
                {
                    db.Proposal.Add(proposal);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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


        // GET: Proposal/ExportPDF
        public ActionResult ExportPDF(int recordID)
        {
            Proposal proposals;

            using (TicketingApp db = new TicketingApp())
            {
                proposals = db.Proposal.Where(u => u.recordID == recordID).FirstOrDefault();

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


                phrase.Add(new Chunk("Introduction :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.introduction + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Overall Description :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.overallDescription + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Function Requirements :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.functionalRequirements + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


                phrase.Add(new Chunk("Non-Function Requirements :\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));
                phrase.Add(new Chunk(proposals.nonFunctionalRequirements + "\n\n", FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK)));


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
