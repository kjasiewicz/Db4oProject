using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zadanie1_db4o.Models
{
    public class DeletePhoneViewModel
    {
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public int NumerTelefonu { get; set; }
      
    }
}