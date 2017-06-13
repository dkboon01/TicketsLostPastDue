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
        [Required(ErrorMessage = "Ticket Number is Required")]
        [Range(1, 999999999, ErrorMessage ="Ticket Number is Required")]
       public int TicketNumber { get; set; }
        [Range( 2, 999999999, ErrorMessage = "Ticket must be created from Inspection")]
        public int InspectionId { get; set; }
        public int ServiceTcktId { get; set; }
        public DateTime ScheduleForStartDate { get; set; }
    }
}