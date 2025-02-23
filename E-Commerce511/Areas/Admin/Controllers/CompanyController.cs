﻿using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        CompanyRepository companyRepository = new CompanyRepository();


        public IActionResult Index()
        {
            var companies = companyRepository.Get();

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
                companyRepository.Create(company);
                companyRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [HttpGet]
        public IActionResult Edit(int companyId)
        {
            var company = companyRepository.GetOne(e=>e.Id == companyId);

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

            companyRepository.Edit(company);
            companyRepository.Commit();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int companyId)
        {
            var company = companyRepository.GetOne(e => e.Id == companyId);

            if (company != null)
            {
                companyRepository.Delete(company);
                companyRepository.Commit();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
