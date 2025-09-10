using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ST10291238_PROG7312_POE.Models;
using ST10291238_PROG7312_POE.Services;

namespace ST10291238_PROG7312_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IssueStore _store;
        private readonly IWebHostEnvironment _env;

        public HomeController(IssueStore store, IWebHostEnvironment env)
        {
            _store = store;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ReportIssue()
        {
            View(new Issue());
        }

        [HttpPost]
        public IActionResult ReportIssue(Issue model, IFormFile[]? files)
        {
            if (string.IsNullOrWhiteSpace(model.Location))
            {
                ModelState.AddModelError(nameof(model.Location), "Location is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError(nameof(model.Description), "Description is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string safeId = model.Id.ToString("N");
            string uploadRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", safeId);
        }
    }
}
