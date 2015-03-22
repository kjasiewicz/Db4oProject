using Db4objects.Db4o;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadanie1_db4o.Infrastructure;
using Zadanie1_db4o.Models;
using System.IO;

namespace Zadanie1_db4o.Controllers
{
    public class HomeController : Controller
    {
        private const string path = "D://studia/baza";
        public ActionResult Index(string id)
        {
            //otwarcie pliku z bazą i pobranie danych wszystkich studentów 
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var q = db.Query<Student>().ToList().Where(x=>id==null || (x.Nazwisko.Contains(id) || x.Imie.Contains(id) || x.StudentId.ToString().Contains(id) )).Select(x => new IndexViewModel
                {
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    StudentId = x.StudentId,
                    IloscTelefonow = x.Telefon.Count()
                });
                return View(q.ToList());
            }

        }
        //pobranie szczegółówych danych o studencie
        public ActionResult Szczegoly(int id)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student = db.Query<Student>(x => x.StudentId == id).First();
                return View(student);
            }

        }
        [HttpGet]
        public ActionResult Usun(int id)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student = db.Query<Student>(x => x.StudentId == id).Select(k => new DeleteViewModel
                {
                    Imie = k.Imie,
                    Nazwisko = k.Nazwisko,
                    StudentId = k.StudentId
                }).First();
                return View(student);
            }

        }
        [HttpPost, ActionName("Usun")]
        public ActionResult UsunPost(int id)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student = db.Query<Student>(x => x.StudentId == id).First();
                db.Delete(student);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EdytujDane(int id)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student = db.Query<Student>(x => x.StudentId == id).First();
                return View(new EditViewModel
                {
                    Imie = student.Imie,
                    Nazwisko = student.Nazwisko,
                    KodPocztowy = student.Adres.KodPocztowy,
                    Miasto = student.Adres.Miasto,
                    NrDomu = student.Adres.NrDomu,
                    Ulica = student.Adres.Ulica,
                    StudentId = student.StudentId,
                    oldId = student.StudentId
                });
            }

        }
        [HttpPost]
        public ActionResult EdytujDane(EditViewModel model)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student1 = db.Query<Student>(x => x.StudentId == model.StudentId).ToList();
                if (ModelState.IsValid && student1.Count == 0)
                {
                    var student = db.Query<Student>(x => x.StudentId == model.oldId).First();
                    student.Imie = model.Imie;
                    student.Nazwisko = model.Nazwisko;
                    student.Adres.KodPocztowy = model.KodPocztowy;
                    student.Adres.Miasto = model.Miasto;
                    student.Adres.NrDomu = model.NrDomu;
                    student.StudentId = model.StudentId;
                    db.Store(student);
                    db.Commit();
                    return RedirectToAction("Szczegoly", new
                    {
                        id = model.StudentId
                    });
                }
                if (student1.Count != 0)
                {
                    ModelState.AddModelError("StudentId", "taki nr albumu już istnieje");
                }
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateViewModel());
        }
        [HttpPost]
        public ActionResult Create(CreateViewModel model)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student1 = db.Query<Student>(x => x.StudentId == model.StudentId).ToList();
                if (ModelState.IsValid && student1.Count == 0)
                {
                    var student = new Student
                    {
                        Imie = model.Imie,
                        Nazwisko = model.Nazwisko,
                        StudentId = model.StudentId,
                        Adres = new Adres
                        {
                            KodPocztowy = model.KodPocztowy,
                            Miasto = model.Miasto,
                            NrDomu = model.NrDomu,
                            Ulica = model.Ulica
                        },
                        Telefon = new List<Telefon>()
                    };
                    db.Store(student);
                    db.Commit();
                    return RedirectToAction("Szczegoly", new
                    {
                        id = model.StudentId
                    });
                }
                if (student1.Count != 0)
                {
                    ModelState.AddModelError("StudentId", "taki nr albumu już istnieje");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AddPhone(int id)
        {
            return View(new AddPhoneViewModel
            {
                StudentId = id
            });
        }
        [HttpPost]
        public ActionResult AddPhone(AddPhoneViewModel model)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var telefon1 = db.Query<Telefon>(x => x.NumerTelefonu == model.NumerTelefonu).ToList();
                if (ModelState.IsValid && telefon1.Count == 0)
                {
                    var student = db.Query<Student>(x => x.StudentId == model.StudentId).First();
                    student.Telefon.Add(new Telefon
                    {
                        CzyKomorkowy = model.CzyKomorkowy,
                        NazwaOperatora = model.NazwaOperatora,
                        NumerTelefonu = model.NumerTelefonu

                    });

                    db.Store(student.Telefon);
                    db.Commit();
                    return RedirectToAction("Szczegoly", new
                    {
                        id = model.StudentId
                    });
                }
                if (telefon1.Count != 0)
                {
                    ModelState.AddModelError("NumerTelefonu", "taki nr telefonu już istnieje");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult EditPhone(int id)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var telefon = db.Query<Telefon>(x => x.NumerTelefonu == id).First();
                var student = db.Query<Student>(x => x.Telefon.Contains(telefon)).First().StudentId;
                return View(new EditPhoneViewModel
                {
                    StudentId = student,
                    CzyKomorkowy = telefon.CzyKomorkowy,
                    NazwaOperatora = telefon.NazwaOperatora,
                    NumerTelefonu = telefon.NumerTelefonu,
                    OldPhone = telefon.NumerTelefonu
                });
            }
        }
        [HttpPost]
        public ActionResult EditPhone(EditPhoneViewModel model)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var telefon1 = db.Query<Telefon>(x => x.NumerTelefonu == model.OldPhone).First();
                var czyjestzajety = db.Query<Telefon>(x => x.NumerTelefonu == model.NumerTelefonu).Count() == 0;
                if (ModelState.IsValid && czyjestzajety)
                {
                    telefon1.CzyKomorkowy = model.CzyKomorkowy;
                    telefon1.NazwaOperatora = model.NazwaOperatora;
                    telefon1.NumerTelefonu = model.NumerTelefonu;

                    db.Store(telefon1);
                    db.Commit();
                    return RedirectToAction("Szczegoly", new
                    {
                        id = model.StudentId
                    });
                }
                if (!czyjestzajety)
                {
                    ModelState.AddModelError("NumerTelefonu", "taki nr telefonu już istnieje");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DeletePhone(int id)
        {
            using(var db= Db4oEmbedded.OpenFile(path))
	        {
            var telefon1=db.Query<Telefon>(x=>x.NumerTelefonu==id).First();
		    var student=db.Query<Student>(x=>x.Telefon.Contains(telefon1)).First().StudentId;
            return View(new DeletePhoneViewModel
            {
                NumerTelefonu = id,
                StudentId=student
            });
	        }
           
        }
        [HttpPost]
        public ActionResult DeletePhone(DeletePhoneViewModel model)
        {
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var telefon1 = db.Query<Telefon>(x => x.NumerTelefonu == model.NumerTelefonu).First();
                var student = db.Query<Student>(x => x.StudentId == model.StudentId).First();
                db.Delete(student.Telefon.Remove(telefon1));
                db.Delete(telefon1);
                db.Store(student.Telefon);
                db.Commit();
            }
            return RedirectToAction("Szczegoly", new
            {
                id = model.StudentId
            });
        }
        [HttpPost]
        public ActionResult Search(string wyszukaj)
        {
            return RedirectToAction("Index", new
            {
                id = wyszukaj
            });
        }
        public ActionResult FillDb()
        {
            //Recreating bazy
            System.IO.File.Delete(path);
            using (var db = Db4oEmbedded.OpenFile(path))
            {
                var student1 = new Student
                {
                    Imie = "Katarzyna",
                    Nazwisko = "Jasiewicz",
                    StudentId = 43571,
                    Adres = new Adres
                    {
                        Miasto = "Kęty",
                        NrDomu = "62",
                        Ulica = "Partyzantów",
                        KodPocztowy = "32-650",
                    },
                    Telefon = new List<Telefon>
                {
                   new Telefon{NumerTelefonu=506320031,NazwaOperatora="Play",CzyKomorkowy=true}
                }
                };
                db.Store(student1);
                db.Commit();
                return View();
            }
        }

    }
}