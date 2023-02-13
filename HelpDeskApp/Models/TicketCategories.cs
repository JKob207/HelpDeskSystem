using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public class TicketCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        IList<Tickets> Tickets { get; set; }

        public static IEnumerable<SelectListItem> GetSelectItems()
        {
            yield return new SelectListItem { Text = "Network problem", Value = "1" };
            yield return new SelectListItem { Text = "Device problem", Value = "2" };
            yield return new SelectListItem { Text = "Software problem", Value = "3" };
            yield return new SelectListItem { Text = "Other", Value = "4" };
        }
    }
}
