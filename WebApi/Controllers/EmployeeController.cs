using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        public static int NextEmployeeIndex = 0;
        public IHttpActionResult GetAllEmployees()
        {
            IList<EmployeeViewModel> Employees = null;
            using (var ctx = new SklepEntities())
            {
                Employees = ctx.Pracownicy.Include("Pracownik_id").Select(I => new EmployeeViewModel()
                {
                    EmployeeId = I.Pracownik_id,
                    EmployeeName = I.Imie,
                    EmployeeSurname = I.Nazwisko,
                    EmployeeSalary=I.Pensja,
                    Gender=I.Plec
                    
                }).ToList();
            }
            NextEmployeeIndex = Employees.Last().EmployeeId + 1;
            return Ok(Employees);
        }
        public IHttpActionResult GetEmployeeById(int id)
        {
            IList<EmployeeViewModel> Employees = null;
            using (var ctx = new SklepEntities())
            {
                Employees = ctx.Pracownicy.Include("Pracownik_id").Where(E=>E.Pracownik_id==id).Select(I => new EmployeeViewModel()
                {
                    EmployeeId = I.Pracownik_id,
                    EmployeeName = I.Imie,
                    EmployeeSurname = I.Nazwisko,
                    EmployeeSalary = I.Pensja,
                    Gender = I.Plec

                }).ToList();
            }
        
            return Ok(Employees);
        }
     
        public IHttpActionResult PostNewEmployee(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                ctx.Pracownicy.Add(new Pracownicy()
                {
                    Pracownik_id = NextEmployeeIndex,
                    Imie = employee.EmployeeName,
                    Nazwisko = employee.EmployeeSurname,
                    Pensja = employee.EmployeeSalary,
                    Plec = employee.Gender

                }); ctx.SaveChanges();
            }
            return Ok();
        }
     
        public IHttpActionResult PutEmployee(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                var ExistingEmployee = ctx.Pracownicy.Where(I => I.Pracownik_id== employee.EmployeeId).FirstOrDefault();


                if (ExistingEmployee != null)
                {
                    ExistingEmployee.Imie = employee.EmployeeName;
                    ExistingEmployee.Nazwisko = employee.EmployeeSurname;
                    ExistingEmployee.Pensja = employee.EmployeeSalary;
                    ExistingEmployee.Plec = employee.Gender;

                    ctx.SaveChanges();
                }
                else return NotFound();

            }
            return Ok();
        }

        public IHttpActionResult DeleteEmployeeById(int id)
        {
            if (id < 0)
                return BadRequest("Invalid id");
           
            using (var ctx = new SklepEntities())
            {
                if (ctx.Faktury.Include("Pracownik_id").Any(K => K.Pracownik_id== id))
                {
                    return BadRequest("Id exists");
                }
                else
                {
                    var Employee = ctx.Pracownicy.Where(I => I.Pracownik_id == id).FirstOrDefault();
                    ctx.Entry(Employee).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }
            return Ok();
        }
    }
}
