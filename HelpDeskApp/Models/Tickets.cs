using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public class Tickets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Descriptions")]
        public string Descriptions { get; set; }
        [Required]
        [Display(Name = "Priority")]
        public int Priority { get; set; }
        [Display(Name = "Created at")]
        public DateTime createdAt { get; set; }
        [Display(Name = "Responsible user")]
        public Users responsibleUser { get; set; }
        [Display(Name = "Ticket owner")]
        public Users ticketOwner { get; set; }
        [Display(Name = "Ticket status")]
        public TicketStatuses ticketStatus { get; set; }
        [Required]
        [Display(Name = "Ticket category")]
        public TicketCategories ticketCategory { get; set; }
    }
}
