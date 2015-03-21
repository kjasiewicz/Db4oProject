using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zadanie1_db4o.Models
{
    public class CreateViewModel
    {
        [Display(Name = "Nr Albumu"), Required(ErrorMessage = "Pole {0} jest wymagane")]
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Imie { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Nazwisko { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Ulica { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Miasto { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string KodPocztowy { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string NrDomu { get; set; }
    }
}