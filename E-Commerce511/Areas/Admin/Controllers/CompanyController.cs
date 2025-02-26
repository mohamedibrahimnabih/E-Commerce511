using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var companies = dbContext.Companies;

            return View(companies.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Company());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            // Validation
            //ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                dbContext.Companies.Add(company);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpGet]
        public IActionResult Edit(int companyId)
        {
            var company = dbContext.Companies.FirstOrDefault(e => e.Id == companyId);

            if (company != null)
            {
                return View(company);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            // Validation

            dbContext.Companies.Update(company);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int companyId)
        {
            var company = dbContext.Companies.FirstOrDefault(e => e.Id == companyId);

            if (company != null)
            {
                dbContext.Companies.Remove(company);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}