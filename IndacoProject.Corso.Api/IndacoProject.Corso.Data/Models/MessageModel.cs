using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Data.Models
{
    public class MessageModel: IRequest
    {
        public string Id { get; set; }
        
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Oggetto")]
        public string Subject { get; set; }


        [Required]
        [Display(Name = "Messaggio")]
        public string Body { get; set; }
    }
}
