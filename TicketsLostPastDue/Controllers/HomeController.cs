using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TicketsLostPastDue.Models;
//using System.Net.Http.WebRequests;

namespace TicketsLostPastDue.Controllers
{
   
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            

             SedAPILogin(true);
            // Ticket_GetData(159384);
            // FindTicket (ticketnumber);
            return View();
        }
        //[HttpGet]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindTicket(Ticket TicketNo
            , string LostButton, string OOBButton, string RefusedButton, string DoneButton)
        {
            System.Diagnostics.Debug.WriteLine("Ticket number keyed in " + TicketNo.ToString());

        //    SedAPILogin(true);
            // Ticket_GetData(159384);
            // int tckno = 0;
            Ticket tckno = Ticket_Search(TicketNo);

            // Tickets with an inspection id = 1 are created by a call from the customer
            //Not created because of an inspection record
            if (!ModelState.IsValid && tckno.InspectionId > 1 && tckno.TicketNumber > 1)
            {



                //  ViewModelfor the Ticket
                ViewModelTicketDetail tcktdtl = new Models.ViewModelTicketDetail();

                //Do I need to add the check to make sure inspection record tied to ticket is not on routeid = 1072 and not next inspection date 18991230
                tcktdtl.apit = Ticket_GetData(tckno.ServiceTcktId);
                // Go retrieve inspection id to see if <> 1072 and next action date <> 12/30/1899
                SVInspection insp = GetInspection(tcktdtl.apit.InspectionId);
                if (object.ReferenceEquals(insp,null))
                    {
                       ModelState.AddModelError("", "Can not Find Inspection for this ticket. Verify that the ticket has an inspection and that the Custom API to find the inspection is online");
                       return View("Index");
                }
                else
                if (!(insp.Route_Id.Equals(1072)) && !(insp.Next_Inspection_Date == "1899-12-30T00:00:00") )
                {
                    if (DoneButton != null)  //only the Done Page shows invoice history
                    {
                        tcktdtl.invs = GetInvoices(tcktdtl.apit.CustomerSiteId, tcktdtl.apit.CustomerId).ToList();
                        // List<HdrInvoice> hdrinv = new List<HdrInvoice>

                        tcktdtl.hdrinv = (from hdr in tcktdtl.invs
                                          group hdr by new { hdr.invoicenumber, hdr.InvoiceDate, hdr.Customer_Id, hdr.Customer_Site_Id, hdr.invdesc, hdr.memo, hdr.ServCompany, hdr.Techname } into orderGroup
                                          select new HdrInvoice()
                                          {
                                              InvoiceNumber = orderGroup.Key.invoicenumber,
                                              InvoiceDate = orderGroup.Key.InvoiceDate,
                                              Customer_Id = orderGroup.Key.Customer_Id,
                                              Customer_Site_Id = orderGroup.Key.Customer_Site_Id,
                                              invdesc = orderGroup.Key.invdesc,
                                              memo = orderGroup.Key.memo,
                                              ServCompany = orderGroup.Key.ServCompany,
                                              Techname = orderGroup.Key.Techname

                                          }).ToList();


                        //    InvoiceDate = orderGroup.InvoiceDate,
                        //   

                    }


                    tcktdtl.lastinspectdt = insp.Last_Inspection_Date;
                    //This can be taken out once the Sedona API has the Sv_inspection table added 
                    switch (insp.Inspection_Cycle_Id)
                    {
                        case 2:
                            tcktdtl.tickinspectdesc = "Annual";
                            break;
                        case 3:
                            tcktdtl.tickinspectdesc = "Biennial";
                            break;
                        case 4:
                            tcktdtl.tickinspectdesc = "Semi-Annual";
                            break;
                        case 5:
                            tcktdtl.tickinspectdesc = "Quarterly";
                            break;
                        case 6:
                            tcktdtl.tickinspectdesc = "Monthly";
                            break;
                        case 7:
                            tcktdtl.tickinspectdesc = "Bi-Monthky";
                            break;
                        case 8:
                            tcktdtl.tickinspectdesc = "Semi-Monthly";
                            break;
                        case 9:
                            tcktdtl.tickinspectdesc = "Weekly";
                            break;
                        case 10:
                            tcktdtl.tickinspectdesc = "Bi-Weekly";
                            break;
                        default:
                            tcktdtl.tickinspectdesc = "Annual";
                            break;
                    }

                    //tcktdtl.tickinspectdesc = 
                    // Know validate if it is a good ticket

                    //the ticket already has an inspection id so after retrieving all inspections for site and service company then take the one out we already have
                    var insplist = SearchInspection(tcktdtl.apit.CustomerSiteId, tcktdtl.apit.ServiceCompanyID).ToList();
                    tcktdtl.sysinsp = (from p in insplist
                                       where p.inspectionid != tcktdtl.apit.InspectionId
                                       select p).ToList();

                    tcktdtl.competitorslist = GetCompetitors(LostButton, OOBButton, RefusedButton, DoneButton);
                    //
                    if (!ModelState.IsValid)
                    {
                        if (LostButton != null)
                        {
                            return View("Lost", tcktdtl);
                        }
                        else
                           if (OOBButton != null)
                        {
                            return View("OOB", tcktdtl);

                        }
                        else
                               if (RefusedButton != null)
                        {
                            return View("Refused", tcktdtl);
                        }
                        else
                                   if (DoneButton != null)
                        {
                            return View("Done", tcktdtl);
                        }
                        else
                        {
                            return View("Index");
                        }
                    }

                    else
                    {
                        ModelState.AddModelError("", "Ticket has already been processed. Inspection is inactive");
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ticket has already been processed. Inspection is inactive");
                    return View("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter a value greater than 1 and the ticket has to be created by an inspection");
                return View("Index");
            }
        }



        [HttpPost]
        public ActionResult Lost(ViewModelTicketDetail ticketmodel)
        {
            int cstsystemid = Convert.ToInt32(ticketmodel.apit.CustomerSystemId);
            int cstinspectionid = Convert.ToInt32(ticketmodel.apit.InspectionId);
            string competitorsel = ticketmodel.Selectedcompetitor;
            string competitorothertxt = ticketmodel.competitorothertxt;
            int ticketid = ticketmodel.apit.ServiceTicketId;

           //// //validation check
           //// if (object.ReferenceEquals(ticketmodel.Selectedcompetitor, null))
           //// {
           ////     ModelState.AddModelError("", "Competitor is required");
           //// }
           //// else
           //// if ((competitorsel.Equals(null)) || (competitorsel.Trim().Length.Equals(0)))
           //// {
           ////     ModelState.AddModelError("", "competitor is required");
           //// }

           //// else
           ////  if (object.ReferenceEquals(ticketmodel.competitorothertxt, null))
           ////      {
           ////     ModelState.AddModelError("", "Other text is required");
           ////      }

           //// else
           ////if ((competitorothertxt.Equals(null)) && (competitorsel == "Other"))
           //// {
           ////     ModelState.AddModelError("", "Other text is required");
           //// }

           // if (!ModelState.IsValid)
           //     {
           //    // ViewModelTicketDetail vw = new ViewModelTicketDetail();
           //    // vw = ticketmodel;
           //    // tcktdtl.apit = Ticket_GetData(tckno.ServiceTcktId);
           //     // Go retrieve inspection id to see if <> 1072 and next action date <> 12/30/1899
           //    //SVInspection insp = GetInspection(vw.apit.InspectionId);

           //     return View();
           // }
           // else
           // {
                //Part 2. Load values from the apit - the ticket that was entered by users
              
                SuccessFailViewModel sf = new SuccessFailViewModel();
                sf.TicketNumber = ticketmodel.apit.TicketNumber;
                bool success = false;
                // a) Update the customer system userdef record with "Lost" competitor informatoin 
                try
                {
                    success = UpdARCustomerSystemUserDef(cstsystemid, competitorsel, competitorothertxt);
                }
                catch
                {
                    success = false;
                }

                if (success)
                {   // b) update the inspection 
                    try
                    {
                        //************6/8 dkb
                        success = UpdInspection(ticketmodel.apit.InspectionId);


                        if (success)
                        {
                            //c) update the sv_service_ticket, sv_service_ticket_dispatch , sy_edit_log - update the rest of these table s
                            //second update the one for the ticket entered
                            //  success = UpdateServiceTicket(ticketid);
                            // success = UpdInspection(ticketmodel.apit.InspectionId)
                            //  }
                            //   success = UpdateServiceTicket(ticketid);
                            success = UpdEditLog(ticketmodel.tickinspectdesc, competitorsel, "Lost", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, ticketmodel.apit.SystemCode);
                            //Part 2. check to see if any other checkbox for inspections is true then process them
                            // checking to make sure there are other inspections
                            if (ticketmodel.sysinsp != null && ticketmodel.sysinsp.Any())
                            {
                                foreach (SearchInspections insp in ticketmodel.sysinsp)
                                {
                                    if (insp.IsSelected == true)  //is an inspection selected
                                    {
                                        if (ticketmodel.apit.InspectionId != insp.inspectionid)    // make sure we have not already updated the inspection
                                        {
                                            try
                                            {
                                                success = UpdARCustomerSystemUserDef(insp.customer_system_id, competitorsel, competitorothertxt);
                                                try
                                                {
                                                    success = UpdInspection(insp.inspectionid);
                                                    try
                                                    {
                                                        success = UpdEditLog(insp.inspcycldesc, competitorsel, "Lost", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, insp.syscode);
                                                    }
                                                    catch
                                                    {
                                                        success = false;
                                                    }

                                                }
                                                catch
                                                {
                                                    success = false;
                                                }

                                            }



                                            catch
                                            {
                                                success = false;
                                            }
                                        }


                                    }

                                }
                            }
                            else
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                }


                if (success)
                {
                    return View("Success", sf);
                }
                else
                {
                    return View("Failed", sf);
                }


          //  }
        }
        public ActionResult OOB(ViewModelTicketDetail ticketmodel)
        {

            //Part 1. Load values from the apit - the ticket that was entered by users
            int cstsystemid = Convert.ToInt32(ticketmodel.apit.CustomerSystemId);
            int cstinspectionid = Convert.ToInt32(ticketmodel.apit.InspectionId);
            string competitorsel = ticketmodel.Selectedcompetitor;
            string competitorothertxt = ticketmodel.competitorothertxt;
            int ticketid = ticketmodel.apit.ServiceTicketId;
            bool success = false;
            // a) Update the customer system userdef record with "Lost" competitor informatoin 
            try
            {
                success = UpdARCustomerSystemUserDef(cstsystemid, competitorsel, competitorothertxt);
            }
            catch
            {
                success = false;
            }

            if (success)
            {   // b) update the inspection 
                try
                {
                    success = UpdInspection(ticketmodel.apit.InspectionId);


                    if (success)
                    {
                        //c) update the sv_service_ticket, sv_service_ticket_dispatch , sy_edit_log - update the rest of these table s
                        //second update the one for the ticket entered
                        //  success = UpdateServiceTicket(ticketid);
                        // success = UpdInspection(ticketmodel.apit.InspectionId)
                        //  }
                        //   success = UpdateServiceTicket(ticketid);
                        success = UpdEditLog(ticketmodel.tickinspectdesc, competitorsel, "OOB", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, ticketmodel.apit.SystemCode);
                        //Part 2. check to see if any other checkbox for inspections is true then process them
                        // checking to make sure there are other inspections
                        if (ticketmodel.sysinsp != null && ticketmodel.sysinsp.Any())
                        {
                            foreach (SearchInspections insp in ticketmodel.sysinsp)
                            {
                                if (insp.IsSelected == true)  //is an inspection selected
                                {
                                    if (ticketmodel.apit.InspectionId != insp.inspectionid)    // make sure we have not already updated the inspection
                                    {
                                        try
                                        {
                                            success = UpdARCustomerSystemUserDef(insp.customer_system_id, competitorsel, competitorothertxt);
                                            try
                                            {
                                                success = UpdInspection(insp.inspectionid);
                                                try
                                                {
                                                    success = UpdEditLog(insp.inspcycldesc, competitorsel, "OOB", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, insp.syscode);
                                                }
                                                catch
                                                {
                                                    success = false;
                                                }

                                            }
                                            catch
                                            {
                                                success = false;
                                            }

                                        }



                                        catch
                                        {
                                            success = false;
                                        }
                                    }


                                }

                            }
                        }
                        else
                        {
                            success = true;
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }
            }
            SuccessFailViewModel sf = new SuccessFailViewModel();
            sf.TicketNumber = ticketmodel.apit.TicketNumber;
            if (success)
            {
                return View("Success", sf);
            }
            else
            {
                return View("Failed", sf);
            }
        }
        public ActionResult Refused(ViewModelTicketDetail ticketmodel)
        {

            //Part 1. Load values from the apit - the ticket that was entered by users
            int cstsystemid = Convert.ToInt32(ticketmodel.apit.CustomerSystemId);
            int cstinspectionid = Convert.ToInt32(ticketmodel.apit.InspectionId);
            //   string competitorsel = ticketmodel.Selectedcompetitor;
            string competitorsel = "Refused";
            string competitorothertxt = ticketmodel.competitorothertxt;
            int ticketid = ticketmodel.apit.ServiceTicketId;
            bool success = false;
            // a) Update the customer system userdef record with "Lost" competitor informatoin 
            try
            {
                success = UpdARCustomerSystemUserDef(cstsystemid, competitorsel, competitorothertxt);
            }
            catch
            {
                success = false;
            }

            if (success)
            {   // b) update the inspection 
                try
                {
                    success = UpdInspection(ticketmodel.apit.InspectionId);


                    if (success)
                    {
                        //c) update the sv_service_ticket, sv_service_ticket_dispatch , sy_edit_log - update the rest of these table s
                        //second update the one for the ticket entered
                        //  success = UpdateServiceTicket(ticketid);
                        // success = UpdInspection(ticketmodel.apit.InspectionId)
                        //  }
                        //   success = UpdateServiceTicket(ticketid);
                        success = UpdEditLog(ticketmodel.tickinspectdesc, competitorsel, "Refused", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, ticketmodel.apit.SystemCode);
                        //Part 2. check to see if any other checkbox for inspections is true then process them
                        // checking to make sure there are other inspections
                        if (ticketmodel.sysinsp != null && ticketmodel.sysinsp.Any())
                        {
                            foreach (SearchInspections insp in ticketmodel.sysinsp)
                            {
                                if (insp.IsSelected == true)  //is an inspection selected
                                {
                                    if (ticketmodel.apit.InspectionId != insp.inspectionid)    // make sure we have not already updated the inspection
                                    {
                                        try
                                        {
                                            success = UpdARCustomerSystemUserDef(insp.customer_system_id, competitorsel, competitorothertxt);
                                            try
                                            {
                                                success = UpdInspection(insp.inspectionid);
                                                try
                                                {
                                                    success = UpdEditLog(insp.inspcycldesc, competitorsel, "Refused", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, insp.syscode);
                                                }
                                                catch
                                                {
                                                    success = false;
                                                }

                                            }
                                            catch
                                            {
                                                success = false;
                                            }

                                        }



                                        catch
                                        {
                                            success = false;
                                        }
                                    }


                                }

                            }
                        }
                        else
                        {
                            success = true;
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }
            }
            SuccessFailViewModel sf = new SuccessFailViewModel();
                sf.TicketNumber = ticketmodel.apit.TicketNumber;
            if (success)
            {
                return View("Success", sf);
            }
            else
            {
                return View("Failed", sf);
            }
        }
        public ActionResult Done(ViewModelTicketDetail ticketmodel, string Done)
        {
            switch (Done)
            {
                case "Keep On Past Due":
                    //Add the save to the ticket notes here 
                    bool success = InsertTicketNotes(ticketmodel.apit.TicketNumber, ticketmodel.ticknote);
                    SuccessFailViewModel sf = new SuccessFailViewModel();
                    sf.TicketNumber = ticketmodel.apit.TicketNumber;
                    if (success)
                    {
                       // return View("Index");
                        return View("Success", sf);
                        
                    }
                    else
                    {
                        //return View();
                        return View("Failed", sf);

                    }
                case "Inactivate Inspection":



                    //Part 1. Load values from the apit - the ticket that was entered by users
                    int cstsystemid = Convert.ToInt32(ticketmodel.apit.CustomerSystemId);
                    int cstinspectionid = Convert.ToInt32(ticketmodel.apit.InspectionId);
                    //   string competitorsel = ticketmodel.Selectedcompetitor;
                    string competitorsel = "Refused";
                    string competitorothertxt = ticketmodel.competitorothertxt;
                    int ticketid = ticketmodel.apit.ServiceTicketId;
                    success = false;
                    // a) Update the customer system userdef record with "Lost" competitor informatoin 
                    try
                    {
                        success = UpdARCustomerSystemUserDef(cstsystemid, competitorsel, competitorothertxt);
                    }
                    catch
                    {
                        success = false;
                    }

                    if (success)
                    {   // b) update the inspection 
                        try
                        {
                            success = UpdInspection(ticketmodel.apit.InspectionId);


                            if (success)
                            {
                                //c) update the sv_service_ticket, sv_service_ticket_dispatch , sy_edit_log - update the rest of these table s
                                //second update the one for the ticket entered
                                //  success = UpdateServiceTicket(ticketid);
                                // success = UpdInspection(ticketmodel.apit.InspectionId)
                                //  }
                                //   success = UpdateServiceTicket(ticketid);
                                success = UpdEditLog(ticketmodel.tickinspectdesc, competitorsel, "Refused/Done", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, ticketmodel.apit.SystemCode);
                                //Part 2. check to see if any other checkbox for inspections is true then process them
                                // checking to make sure there are other inspections
                                if (ticketmodel.sysinsp != null && ticketmodel.sysinsp.Any())
                                {
                                    foreach (SearchInspections insp in ticketmodel.sysinsp)
                                    {
                                        if (insp.IsSelected == true)  //is an inspection selected
                                        {
                                            if (ticketmodel.apit.InspectionId != insp.inspectionid)    // make sure we have not already updated the inspection
                                            {
                                                try
                                                {
                                                    success = UpdARCustomerSystemUserDef(insp.customer_system_id, competitorsel, competitorothertxt);
                                                    try
                                                    {
                                                        success = UpdInspection(insp.inspectionid);
                                                        try
                                                        {
                                                            success = UpdEditLog(insp.inspcycldesc, competitorsel, "Refused", ticketmodel.apit.Site.SiteNumber, ticketmodel.apit.Customer.CustomerNumber, insp.syscode);
                                                        }
                                                        catch
                                                        {
                                                            success = false;
                                                        }

                                                    }
                                                    catch
                                                    {
                                                        success = false;
                                                    }

                                                }



                                                catch
                                                {
                                                    success = false;
                                                }
                                            }


                                        }

                                    }
                                }
                                else
                                {
                                    success = true;
                                }
                            }
                            else
                            {
                                success = false;
                            }
                        }
                        catch
                        {
                            success = false;
                        }
                    }
                    SuccessFailViewModel sfb = new SuccessFailViewModel();
                    sfb.TicketNumber = ticketmodel.apit.TicketNumber;
                    if (success)
                    {
                       
                        return View("Success", sfb);
                    }
                    else
                    {
                        return View("Failed", sfb);
                    }
                default:
                    {
                        return View();
                    }
            }
        }
        private static bool SedAPILogin(bool dmAuthenticate)
        {

            string username = ConfigurationManager.AppSettings["lgusrin"];

            string password = ConfigurationManager.AppSettings["lgusrps"];
            string environment = ConfigurationManager.AppSettings["environment"];
            string envURL;
            string uri;


              envURL = "https://sedoffapi.silcofs.com";

             uri = "https://sedoffapi.silcofs.com";
             // envURL = "https://10.0.0.89";

             // uri = "https://10.0.0.89";
            CredentialCache credentialCache = new CredentialCache();

            credentialCache.Add(new Uri(envURL), "Basic", new NetworkCredential(username, password));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;
          //  request.Timeout = 600000;

            request.Method = "GET";
            request.ContentType = "text/json";
            HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
            if (responsemsg.StatusCode == HttpStatusCode.OK)
            {
                dmAuthenticate = true;
            }


            return dmAuthenticate;
        }
        private static APITicket Ticket_GetData(int serviceticketid)
        {
            APITicket j = new APITicket();
            string environment = ConfigurationManager.AppSettings["environment"];

            //  bool success = false;
            string username = ConfigurationManager.AppSettings["lgusrin"];
            string password = ConfigurationManager.AppSettings["lgusrps"];
            Byte[] documentBytes;  //holds the post body information in bytes

            //Prepare the request 
            CredentialCache credentialCache = new CredentialCache();
            //// if (environment == "P")
            // {
            credentialCache.Add(new Uri("https://sedoffapi.silcofs.com"), "Basic", new NetworkCredential(username, password));


            string uri;

            uri = "https://sedoffapi.silcofs.com/api/ServiceTicket/" + serviceticketid;

            //}

            // string uri = "https://beta.digitalmeasures.com/login/service/v4/SchemaData/INDIVIDUAL-ACTIVITIES-Engineering";
            //  string uri = "https://www.digitalmeasures.com/login/service/v4/SchemaData/INDIVIDUAL-ACTIVITIES-Engineering";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Method = "GET";
            request.ContentType = "text/json";

            //Call store proc for the information for a user 
            string body = "";
            // string ticketno;

            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(body); //convert string to bytes
            request.ContentLength = documentBytes.Length;


            HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
            if (responsemsg.StatusCode == HttpStatusCode.OK)
            {
                // success = true;


                using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                {
                    var responsedata = reader.ReadToEnd();
                    j = JsonConvert.DeserializeObject<APITicket>(responsedata);

                    //  foreach (var itm in j)
                    // {
                    // APITicket TicketNumber = itm.TicketNumber;
                    //  }

                    //  System.Diagnostics.Debug.Write("Ticket Number:  " + j.TicketNumber.ToString());
                    // System.Diagnostics.Debug.Write("Type Code from Customer: " + j.Customer.TypeCode.ToString());
                    // System.Diagnostics.Debug.WriteLine( " API ticket" + )

                    //  System.Diagnostics.Debug.Write(responsedata);


                }

            }
            return j;
        }
        private Ticket Ticket_Search(Ticket ticket)
        {
            //  int SrvTckt = 0;
            // int inspectionid = 0;
            Ticket TicketInsp = new Ticket();
            string environment = ConfigurationManager.AppSettings["environment"];

            // bool success = false;
            string username = ConfigurationManager.AppSettings["lgusrin"];
            string password = ConfigurationManager.AppSettings["lgusrps"];
            //  Byte[] documentBytes;  //holds the post body information in bytes

            //Prepare the request 
            CredentialCache credentialCache = new CredentialCache();

            credentialCache.Add(new Uri("https://sedoffapi.silcofs.com"), "Basic", new NetworkCredential(username, password));


            string uri;

            uri = "https://sedoffapi.silcofs.com/api/serviceticket/search";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {

                string searchbyticket = "{\"TicketNumber\" : " + ticket.TicketNumber + "}";

                //  string json = "{ \"method\" : \"guru.test\", \"params\" : [ \"Guru\" ], \"id\" : 123 }";
                streamWriter.Write(searchbyticket);
                streamWriter.Flush();
                streamWriter.Close();
            }


            HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
            if (responsemsg.StatusCode == HttpStatusCode.OK)
            {
                // success = true;


                using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                {
                    var responsedata = reader.ReadToEnd();
                    dynamic jm = JsonConvert.DeserializeObject(responsedata);
                    foreach (var it in jm)
                    {

                        if (it.IsInspection == true)
                        {
                            TicketInsp.InspectionId = Convert.ToInt32(it.InspectionId);
                            TicketInsp.ServiceTcktId = Convert.ToInt32(it.ServiceTicketId);
                            TicketInsp.TicketNumber = Convert.ToInt32(it.TicketNumber);
                            TicketInsp.ScheduleForStartDate = Convert.ToDateTime(it.ScheduledForStartDate);
                        }
                        else
                        {
                            TicketInsp.InspectionId = 0;
                            TicketInsp.ServiceTcktId = 0;
                        }
                    }


                  //  System.Diagnostics.Debug.Write(responsedata);


                }

            }
            return TicketInsp;
        }
        private static List<SearchInspections> SearchInspection(int siteid, int servcomp)
        {
            // SearchInspections j = new SearchInspections();
            List<SearchInspections> inspcont = new List<SearchInspections>();
           // string environment = ConfigurationManager.AppSettings["environment"];

            
        
            Byte[] documentBytes;  //holds the post body information in bytes

            
            string uri = "https://silcosedonacustomapi.silcofs.com/api/search?siteid=" + siteid + "&servcomp=" + servcomp;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);


            string authInfo = ConfigurationManager.AppSettings["apilgusrin"] + ":" + ConfigurationManager.AppSettings["apilgusrps"];
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;


           // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
           
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Method = "GET";
            request.ContentType = "text/json";

            //Call store proc for the information for a user 
            string body = "";
            // string ticketno;

            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(body); //convert string to bytes
            request.ContentLength = documentBytes.Length;


            HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
            if (responsemsg.StatusCode == HttpStatusCode.OK)
            {
                // success = true;


                using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                {
                    var responsedata = reader.ReadToEnd();
                    inspcont = JsonConvert.DeserializeObject<List<SearchInspections>>(responsedata);

                    // System.Diagnostics.Debug.Write(responsedata);


                }

            }
            return inspcont;
        }
        private static List<Competitors> GetCompetitors(string LostButton, string OOBButton, string RefusedButton, string DoneButton)
        {

            List<Competitors> comp = new List<Competitors>();
            List<Competitors> complist = new List<Competitors>();
             Byte[] documentBytes;  //holds the post body information in bytes

           

            string uri = "https://silcosedonacustomapi.silcofs.com/api/AR_Userdef_8";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string authInfo = ConfigurationManager.AppSettings["apilgusrin"] + ":" + ConfigurationManager.AppSettings["apilgusrps"];
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
         
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Method = "GET";
            request.ContentType = "text/json";

            //Call store proc for the information for a user 
            string body = "";
            // string ticketno;

            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(body); //convert string to bytes
            request.ContentLength = documentBytes.Length;


            HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
            if (responsemsg.StatusCode == HttpStatusCode.OK)
            {
                // success = true;


                using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                {
                    var responsedata = reader.ReadToEnd();
                    comp = JsonConvert.DeserializeObject<List<Competitors>>(responsedata);
                    var compl = comp.ToList();
                    foreach (var c in comp)
                    {
                        if (LostButton != null)   //Lost Button  - competitor list does not neeed OOB or Refused 
                        {
                            if (!c.Description.Contains("N/A") && !c.Description.Contains("OOB") && !c.Description.Contains("Refused") && !c.Inactive.Contains("Y"))
                            {
                                complist.Add(c);
                            }
                        }

                        else
                            if (OOBButton != null)
                        {
                            if (c.Description.Contains("OOB - Not Confirmed"))  // only show the OOB not confirmed
                            {
                                complist.Add(c);
                            }
                        }
                        else
                                if (RefusedButton != null)  // only show Refused  d 
                        {
                            if (c.Description.Contains("OOB"))
                            {
                                complist.Add(c);
                            }
                        }

                        //  System.Diagnostics.Debug.Write(responsedata);

                    }
                }

            }

            return complist;
        }
        private static bool UpdARCustomerSystemUserDef(int systemid, string competitortxt, string competitorothertxt)
        {
            bool success = false;

            string environment = ConfigurationManager.AppSettings["environment"];


            string username = ConfigurationManager.AppSettings["lgusrin"];
            string password = ConfigurationManager.AppSettings["lgusrps"];
            Byte[] documentBytes;  //holds the post body information in bytes

            //Prepare the request 
            CredentialCache credentialCache = new CredentialCache();

            credentialCache.Add(new Uri("https://sedoffapi.silcofs.com"), "Basic", new NetworkCredential(username, password));


            string uri;

            uri = "https://sedoffapi.silcofs.com/api/CustomerSystemUserdef/" + systemid;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;


            request.Method = "PUT";
            request.ContentType = "application/json";

            //Call store proc for the information for a user 
            string body = "{\"CustomerSystemId\":" + systemid.ToString() + ",\"Table8Code\": \"" + competitortxt + "\" " + ",\"Text4\" : \"" + competitorothertxt + "\"" + ",\"Date2\" : \"" + DateTime.Today.ToString() + "\"}";
            // string ticketno;


            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(body); //convert string to bytes
            request.ContentLength = documentBytes.Length;
            //writing the stream
            using (Stream requestStream = request.GetRequestStream())
            {

                requestStream.Write(documentBytes, 0, documentBytes.Length);
                requestStream.Flush();
                requestStream.Close();
            }

            //Read the response back after writing the stream
            try
            {
                HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
                if ((responsemsg.StatusCode == HttpStatusCode.OK) || (responsemsg.StatusCode == HttpStatusCode.NoContent))
                {
                    success = true;

                }
                else
                {
                    success = false;
                }
            }
            catch
            {
                success = false;


            }
            //  HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();

            return success;
        }
     

        private static bool UpdInspection(int inspectionid)
        {
            bool success = false;
            
          //  Byte[] documentBytes;  //holds the post body information in bytes

            //Prepare the request 
            CredentialCache credentialCache = new CredentialCache();



            //  string uri = "https://silcosedonacustomapi/api/SV_Inspection/" + inspectionid;

            string uri = "https://silcosedonacustomapi.silcofs.com/api/SV_Inspection/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string authInfo = ConfigurationManager.AppSettings["apilgusrin"] + ":" + ConfigurationManager.AppSettings["apilgusrps"];
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;


            request.Method = "PUT";
            request.ContentType = "application/json";

            //Call store proc for the information for a user
            try
            {
                using (var streamWtr = new StreamWriter(request.GetRequestStream()))
                {
                    //STOPPED NOT RETURNING A RESPONSE 

                    //  string edtlg = "{\"user\" : \"" + puser + "\"" + ", \"inspectiontype\": \"" + inspcycledesc + "\"" + ", \"systemcode\": \"" + systemcode + "\"" + ", \"sitecode\": \"" + siteno + "\"" + ", \"action\": \"" + action + "\"" + ", \"code\": \"" + competitor + "\"" + ", \"customernumber\": \"" + customernumber + "\"" + "}";
                    //+ ",\"systemcode\": \"" + systemcode + "\"" + ",\"sitecode\": \"" + siteno + "\""  + ",\"action\": \"" + action + "\""+ ",\"code\": \"" + competitor + "\"" +  ",\"customernumber\": \"" + customernumber + "\""+ "}";
                    //  string strPCIInfo = "{\"Inspection_Id\":" + inspectionid + ",\"Next_Inspection_Date\": \"" + "1899-12-30T00:00:00" + "\"" + ",\"Route_Id\" : " + 1072 + "}";

                    //  string json = "{ \"method\" : \"guru.test\", \"params\" : [ \"Guru\" ], \"id\" : 123 }";
                    string body = "{\"Inspection_Id\":" + inspectionid + ",\"Next_Inspection_Date\": \"" + "1899-12-30T00:00:00" + "\"" + ",\"Route_Id\" : " + 1072 + "}";

                    streamWtr.Write(body);
                    streamWtr.Flush();
                    streamWtr.Close();
                }
            }
            catch(Exception ex)
            {

            }



            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();


   
            try
            {
                HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
                if ((responsemsg.StatusCode == HttpStatusCode.OK) || (responsemsg.StatusCode == HttpStatusCode.NoContent))
                {
                    success = true;

                }
                else
                {
                    success = false;
                }
            }
            catch
            {
                success = false;


            }
            //  HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();

            return success;
        }
        private static bool InsertTicketNotes(string TicketNumber, string note)   ///Stopped Here  adding  a method to post a note to the ticket
        {
            bool success = false;


            string environment = ConfigurationManager.AppSettings["environment"];


            Byte[] documentBytes;  //holds the post body information in bytes

            //Prepare the request 
            CredentialCache credentialCache = new CredentialCache();


            string userloggedin = System.Web.HttpContext.Current.Session["sessionLoginName"].ToString();

            string uri = "https://sedoffapi.silcofs.com/api/ServiceTicketNote/";
          
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;


            request.Method = "POST";
            request.ContentType = "application/json";

            //Call store proc for the information for a user 
            string body = "{\"ServiceTicketNumber\":" + TicketNumber + ",\"AccessLevel\": \"" + 2 + "\"" + ",\"Note\" : \"" + note + "\"" + ",\"UserCode\": \"" + userloggedin + "\"" + "}";
            // string ticketno;


            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(body); //convert string to bytes
            request.ContentLength = documentBytes.Length;
            //writing the stream
            using (Stream requestStream = request.GetRequestStream())
            {

                requestStream.Write(documentBytes, 0, documentBytes.Length);
                requestStream.Flush();
                requestStream.Close();
            }

            //Read the response back after writing the stream
            try
            {
                HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
                if ((responsemsg.StatusCode == HttpStatusCode.OK) || (responsemsg.StatusCode == HttpStatusCode.Created))
                {
                    success = true;

                }
                else
                {
                    success = false;
                }
            }
            catch
            {
                success = false;


            }
            //  HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();

            return success;
        }
        private static SVInspection GetInspection(int inspectionid)
        {

            SVInspection j = new SVInspection();
            try
            {
                    CredentialCache credentialCache = new CredentialCache();
               
                    Byte[] documentBytes;  //holds the post body information in bytes
                    string uri = "https://silcosedonacustomapi.silcofs.com/api/SV_Inspection/" + inspectionid;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                   
                string authInfo = ConfigurationManager.AppSettings["apilgusrin"] + ":" + ConfigurationManager.AppSettings["apilgusrps"];
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;

                request.AllowAutoRedirect = true;
                  request.PreAuthenticate = true;
               
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    request.Method = "GET";
                    request.ContentType = "application/json";

                    string body = "";


                    System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
                    documentBytes = obj.GetBytes(body); //convert string to bytes
                    request.ContentLength = documentBytes.Length;

                    HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
                    if ((responsemsg.StatusCode == HttpStatusCode.OK) || (responsemsg.StatusCode == HttpStatusCode.NoContent))
                    {
                        // success = true;
                        using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                        {
                            var responsedata = reader.ReadToEnd();
                            j = JsonConvert.DeserializeObject<SVInspection>(responsedata);  ///Here is the problem on retrive inspection *********************



                            // System.Diagnostics.Debug.Write(responsedata);


                        }

                    }
                    else
                    {

                         j = null;

                    }
               
            }
            catch (Exception e)
            {
                 j = null;
            }
            return j;
        }
        private static bool UpdEditLog(string inspcycledesc, string competitor, string action, string siteno, string customernumber, string systemcode)
            {


                string puser = System.Web.HttpContext.Current.Session["sessionLoginName"].ToString();

              //  string environment = ConfigurationManager.AppSettings["environment"];
                // string customernumber = "";
                bool success = false;


                string uri;

                uri = "https://silcosedonacustomapi.silcofs.com/api/EditLog/";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AllowAutoRedirect = true;
                request.PreAuthenticate = true;
            // request.Credentials = credentialCache;
            string authInfo = ConfigurationManager.AppSettings["apilgusrin"] + ":" + ConfigurationManager.AppSettings["apilgusrps"];
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            request.AutomaticDecompression = DecompressionMethods.GZip;

                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //STOPPED NOT RETURNING A RESPONSE 

                    string edtlg = "{\"user\" : \"" + puser + "\"" + ", \"inspectiontype\": \"" + inspcycledesc + "\"" + ", \"systemcode\": \"" + systemcode + "\"" + ", \"sitecode\": \"" + siteno + "\"" + ", \"action\": \"" + action + "\"" + ", \"code\": \"" + competitor + "\"" + ", \"customernumber\": \"" + customernumber + "\"" + "}";
                    //+ ",\"systemcode\": \"" + systemcode + "\"" + ",\"sitecode\": \"" + siteno + "\""  + ",\"action\": \"" + action + "\""+ ",\"code\": \"" + competitor + "\"" +  ",\"customernumber\": \"" + customernumber + "\""+ "}";
                    //  string strPCIInfo = "{\"Inspection_Id\":" + inspectionid + ",\"Next_Inspection_Date\": \"" + "1899-12-30T00:00:00" + "\"" + ",\"Route_Id\" : " + 1072 + "}";

                    //  string json = "{ \"method\" : \"guru.test\", \"params\" : [ \"Guru\" ], \"id\" : 123 }";
                    streamWriter.Write(edtlg);
                    streamWriter.Flush();
                    streamWriter.Close();
                }


                HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
                if (responsemsg.StatusCode == HttpStatusCode.OK)
                {
                    success = true;


                }
                return success;
            }

        private static List<Invoice> GetInvoices(int siteid, int customerid)
            {

                List<Invoice> inv = new List<Invoice>();
               
                Byte[] documentBytes;  //holds the post body information in bytes

               
                string uri = "https://silcosedonacustomapi.silcofs.com/api/Invoice?siteid=" + siteid + "&customerid=" + customerid;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                 string authInfo = ConfigurationManager.AppSettings["apilgusrin"] + ":" + ConfigurationManager.AppSettings["apilgusrps"];
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;
                request.AllowAutoRedirect = true;
                request.PreAuthenticate = true;
               
                request.AutomaticDecompression = DecompressionMethods.GZip;


                request.Method = "GET";
                request.ContentType = "application/json";


                string body = "";


                System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
                documentBytes = obj.GetBytes(body); //convert string to bytes
                request.ContentLength = documentBytes.Length;

                HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
                if ((responsemsg.StatusCode == HttpStatusCode.OK))
                {
                    using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                    {
                        var responsedata = reader.ReadToEnd();
                        inv = JsonConvert.DeserializeObject<List<Invoice>>(responsedata);  ///Here is the problem on retrive inspection *********************

                    }

                }
                else
                {

                    ;

                }


                return inv;
            }
 




    }
}
         
    
