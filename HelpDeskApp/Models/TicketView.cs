using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public class TicketView
    {
        [Display(Name = "ID")]
        public int TicketID { get; set; }
        [Display(Name = "Title")]
        public string TickietTitle { get; set; }
        [Display(Name = "Description")]
        public string TicketDescriptions { get; set; }
        [Display(Name = "Priority")]
        public int TicketPriority { get; set; }
        [Display(Name = "Created at")]
        public DateTime TicketCreatedAt { get; set; }
        [Display(Name = "Responsible user")]
        public string responsibleUser { get; set; }
        [Display(Name = "Ticket owner")]
        public string ticketOwner { get; set; }
        [Display(Name = "Status")]
        public string ticketStatus { get; set; }
        [Display(Name = "Category")]
        public string ticketCategory { get; set; }
    }
}
