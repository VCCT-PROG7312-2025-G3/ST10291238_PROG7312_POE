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
            Directory.CreateDirectory(uploadRoot);

            string joinedPaths = "";
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx" };
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowed.Contains(ext))
                        continue;

                    using var stream = System.IO.File.Create(Path.Combine(uploadRoot, file.FileName));
                    file.CopyTo(stream);


                    var relative = $"/uploads/{safeId}/{file.FileName}";
                    joinedPaths = string.IsNullOrEmpty(joinedPaths) ? relative : $"{joinedPaths};{relative}";
                }
            }

            model.AttachmentPath = joinedPaths;
            _store.Add(model);

            return RedirectToAction(nameof(Success), new { id = model.Id });
        }

        public IActionResult Success(Guid id)
        {
            var issue = _store.Get(id);
            if (issue == null)
            {
                return NotFound();
            }
            return View(issue);
        }
    }
}
