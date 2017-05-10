using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketsLostPastDue.Models
{
    public class SVInspection
    {
        public int Inspection_Id { get; set; }
        public int Customer_System_Id { get; set; }
        public int Inspection_Cycle_Id { get; set; }
        public int Problem_Id { get; set; }
        public string Description { get; set; }
        public string Next_Inspection_Date { get; set; }
        public string Last_Inspection_Date { get; set; }
        public string Notes { get; set; }
        public int Last_Service_Ticket_Id { get; set; }
        public int Service_Level_id { get; set; }
        public int Job_Id { get; set; }
        public int Route_Id { get; set; }
        public int Service_Company_Id { get; set; }
        public int Inspection_Item_Id { get; set; }
        public decimal Inspection_Charge_Amount { get; set; }
        public int Group_Number { get; set; }
        public int Estimated_Hours { get; set; }
        public int Service_Tech_Id { get; set; }
        public string High_Frequency_Omit { get; set; }
        public int Customer_Recurring_Id { get; set; }
        public decimal Cycle_Amount { get; set; }
        public int Recurring_Item_Id { get; set; }
        public bool Terminated { get; set; }
        public string Terminated_Date { get; set; }
        public string Terminated_By { get; set; }
        public string Exclude_In_Frequence_Omit { get; set; }
        public int Increment_Code_Id { get; set; }






    }
}