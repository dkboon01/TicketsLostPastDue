using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using SedonaServices.Models;
using Newtonsoft.Json;
using TicketsLostPastDue.Models;
using System.Text;
using System.Net.Http;
//using System.Net.Http.WebRequests;

namespace TicketsLostPastDue.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // SedAPILogin(true);
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

            SedAPILogin(true);
            // Ticket_GetData(159384);
           // int tckno = 0;
            Ticket tckno = Ticket_Search(TicketNo);
            if (!ModelState.IsValid && tckno.InspectionId >1 && tckno.TicketNumber >1)
            {



                //  Ticket ticketnumer = ticketid;
                ViewModelTicketDetail tcktdtl = new Models.ViewModelTicketDetail();
                tcktdtl.apit = Ticket_GetData(tckno.ServiceTcktId);

                tcktdtl.sysinsp = SearchInspection(tcktdtl.apit.CustomerSiteId, tcktdtl.apit.ServiceCompanyID);



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
                    return View("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter a value greater than 1 and the ticket has to be created by an inspection");
                return View("Index");
            }
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Lost(APITicket tck)
        {
            return View();
        }
        public ActionResult OOB(APITicket tck)
        {
            return View();
        }
        private static bool SedAPILogin(bool dmAuthenticate)
        {

            string username = ConfigurationManager.AppSettings["lgusrin"];

            string password = ConfigurationManager.AppSettings["lgusrps"];
            string environment = ConfigurationManager.AppSettings["environment"];
            string envURL;
            string uri;

            //   if (environment == "P")
            //   {
            envURL = "https://sedoffapi.silcofs.com";
            //  uri = "https://www.digitalmeasures.com/login/service/v4";
            uri = "https://sedoffapi.silcofs.com";
            //  }
            //else
            //{
            //    envURL = "https://beta.digitalmeasures.com";
            //    uri = "https://beta.digitalmeasures.com/login/service/v4";

            //}
            CredentialCache credentialCache = new CredentialCache();

            credentialCache.Add(new Uri(envURL), "Basic", new NetworkCredential(username, password));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.AllowAutoRedirect = true;
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;
            request.AutomaticDecompression = DecompressionMethods.GZip;

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
            string strPCIInfo = "";
            // string ticketno;

            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(strPCIInfo); //convert string to bytes
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

                    System.Diagnostics.Debug.Write("Ticket Number:  " + j.TicketNumber.ToString());
                    System.Diagnostics.Debug.Write("Type Code from Customer: " + j.Customer.TypeCode.ToString());
                   // System.Diagnostics.Debug.WriteLine( " API ticket" + )

                    System.Diagnostics.Debug.Write(responsedata);


                }

            }
            return j;
        }
        private Ticket  Ticket_Search(Ticket ticket)
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
                //string json = "{\"user\":\"test\"," +
                //            "\"password\":\"bla\"}";
                //
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
                        //inspectionid = Convert.ToInt32(it.  )
                        //  System.Diagnostics.Debug.Write(it.ServiceTicketId.ToString());
                        if (it.IsInspection == true)
                        {
                          TicketInsp.InspectionId  = Convert.ToInt32(it.InspectionId);
                           TicketInsp.ServiceTcktId  = Convert.ToInt32(it.ServiceTicketId);
                            TicketInsp.TicketNumber = Convert.ToInt32(it.TicketNumber);
                        }
                        else
                        {
                            TicketInsp.InspectionId = 0;
                            TicketInsp.ServiceTcktId = 0 ;
                        }
                    }

                    
                    

                  //  System.Diagnostics.Debug.Write("Ticket ID:  " + jm.Fi .ToString());
                  
                    //   System.Diagnostics.Debug.Write("Type Code from Customer: " + j.Customer.TypeCode.ToString());

                    System.Diagnostics.Debug.Write(responsedata);

                    // intserviceid.Equals(j.ServiceTicketId);
                  //  intserviceid = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)jm).First).First).Last.Parent.Last).Value;                                
                  
                }

            }
            return TicketInsp;
        }
        private static List<SearchInspections> SearchInspection(int siteid, int servcomp )
        {
           // SearchInspections j = new SearchInspections();
            List<SearchInspections> inspcont = new List<SearchInspections>();
            string environment = ConfigurationManager.AppSettings["environment"];

            //  bool success = false;
         //   string username = ConfigurationManager.AppSettings["lgusrin"];
           // string password = ConfigurationManager.AppSettings["lgusrps"];
            Byte[] documentBytes;  //holds the post body information in bytes

            //Prepare the request 
            CredentialCache credentialCache = new CredentialCache();
            //// if (environment == "P")
            // {
            //  credentialCache.Add(new Uri("http://localhost:50249"), "Basic", new NetworkCredential(username, password));
         

            string uri;

          //  uri = "https://sedoffapi.silcofs.com/api/ServiceTicket/" + serviceticketid;
            uri = "http://localhost:50249/api/search?siteid=" + siteid +"&servcomp=" + servcomp;
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
            string strPCIInfo = "";
            // string ticketno;

            System.Text.ASCIIEncoding obj = new System.Text.ASCIIEncoding();
            documentBytes = obj.GetBytes(strPCIInfo); //convert string to bytes
            request.ContentLength = documentBytes.Length;


            HttpWebResponse responsemsg = (HttpWebResponse)request.GetResponse();
            if (responsemsg.StatusCode == HttpStatusCode.OK)
            {
                // success = true;


                using (StreamReader reader = new StreamReader(responsemsg.GetResponseStream()))
                {
                    var responsedata = reader.ReadToEnd();
                 inspcont = JsonConvert.DeserializeObject<List<SearchInspections>>(responsedata);

                   
                

                 //   System.Diagnostics.Debug.Write("Ticket Number:  " + j.ToString());
                //    System.Diagnostics.Debug.Write("Type Code from Customer: " + j.Customer.TypeCode.ToString());
                    // System.Diagnostics.Debug.WriteLine( " API ticket" + )

                    System.Diagnostics.Debug.Write(responsedata);


                }

            }
            return inspcont ;
        }
    } 

         
        }
    
