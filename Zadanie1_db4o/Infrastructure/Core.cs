using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadanie1_db4o.Infrastructure
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public Adres Adres { get; set; }
        public List<Telefon> Telefon { get; set; }
    }
    public class Adres
    {
        public string Ulica { get; set; }
        public string Miasto { get; set; }
        public string KodPocztowy { get; set; }
        public string NrDomu { get; set; }
    }
    public class Telefon
    {
        public int NumerTelefonu { get; set; }
        public string NazwaOperatora { get; set; }
        public bool CzyKomorkowy { get; set; }
    }
}