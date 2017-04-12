using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketsLostPastDue.Models
{
    public class ViewModelTicketDetail
    {
        public APITicket apit { get; set; }
        public List<SearchInspections> sysinsp {get; set;}

    }
}