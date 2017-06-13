using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketsLostPastDue.Models
{
    public class ViewModelTicketDetail
    {
        public APITicket apit { get; set; }
        public List<SearchInspections> sysinsp {get; set;}
        public List<Competitors> competitorslist { get; set;  }
     //   public SelectList competitor { get; set; }
        [Required(ErrorMessage= "Competitor is Required")]
        public string Selectedcompetitor { get; set; }
        [Required(ErrorMessage ="Field must be filled in")]
        public string competitorothertxt { get; set; }
        public string tickinspectdesc { get; set; }
  
        public string lastinspectdt { get; set; }
        [Required(ErrorMessage ="A Note is Required for this Ticket")]
        public string ticknote { get; set; }
        public List<HdrInvoice> hdrinv { get; set; }
        public List<Invoice> invs { get; set; }
        //public bool[] Selected { get; set; }
        // public string SelectedDescription { get; set; }

    }
}