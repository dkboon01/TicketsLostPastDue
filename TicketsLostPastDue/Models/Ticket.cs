using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsLostPastDue.Models
{
    public class Ticket
    {
        [Required]
       public int TicketNumber { get; set; }
    }
}