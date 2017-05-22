using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketsLostPastDue.Models
{
    public class HdrInvoice
    {
        public int InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public int Customer_Id { get; set; }
        public int Customer_Site_Id { get; set; }
        public string invdesc { get; set; } 
        public string memo { get; set; }
        public string ServCompany { get; set; }
        public string Techname { get; set; }
     
    }
}