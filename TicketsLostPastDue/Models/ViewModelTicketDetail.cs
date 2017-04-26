using System;
using System.Collections.Generic;
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
        public string Selectedcompetitor { get; set; }
        public string competitorothertxt { get; set; }
        //public bool[] Selected { get; set; }
        // public string SelectedDescription { get; set; }

    }
}