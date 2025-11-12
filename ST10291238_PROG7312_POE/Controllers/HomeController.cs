using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ST10291238_PROG7312_POE.Models;
using ST10291238_PROG7312_POE.Services;
using ST10291238_PROG7312_POE.DataStructures;
using Microsoft.AspNetCore.Hosting;

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

            if (_serviceStore == null)
                _serviceStore = new ServiceRequestStore();
        }

        // --------------------------- MAIN MENU ---------------------------
        public IActionResult Index() => View();

        // --------------------------- REPORT ISSUES ---------------------------
        [HttpGet]
        public IActionResult ReportIssue() => View(new Issue());

        [HttpPost]
        public IActionResult ReportIssue(Issue model, IFormFile[]? files)
        {
            if (model.Id == Guid.Empty) model.Id = Guid.NewGuid();
            if (model.CreatedUtc == default) model.CreatedUtc = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(model.Status)) model.Status = "Submitted";

            ModelState.Remove(nameof(model.Id));
            ModelState.Remove(nameof(model.CreatedUtc));
            ModelState.Remove(nameof(model.Status));
            ModelState.Remove(nameof(model.AttachmentPaths));

            if (string.IsNullOrWhiteSpace(model.Location))
                ModelState.AddModelError(nameof(model.Location), "Location is required.");

            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError(nameof(model.Description), "Description is required.");

            if (!ModelState.IsValid)
                return View(model);

            string safeId = model.Id.ToString("N");
            string uploadRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", safeId);
            Directory.CreateDirectory(uploadRoot);

            string joinedPaths = "";
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length == 0) continue;
                    var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx" };
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowed.Contains(ext)) continue;

                    using var stream = System.IO.File.Create(Path.Combine(uploadRoot, file.FileName));
                    file.CopyTo(stream);
                    var relative = $"/uploads/{safeId}/{file.FileName}";
                    joinedPaths = string.IsNullOrEmpty(joinedPaths) ? relative : $"{joinedPaths};{relative}";
                }
            }

            model.AttachmentPaths = joinedPaths;
            _store.Add(model);
            return RedirectToAction(nameof(Success), new { id = model.Id });
        }

        public IActionResult Success(Guid id)
        {
            var issue = _store.Get(id);
            return issue == null ? NotFound() : View(issue);
        }

        // --------------------------- EVENTS & ANNOUNCEMENTS ---------------------------
        private static readonly List<Event> SampleEvents = new()
        {
            new Event { Title="Youth Career Expo", Category="Education", Description="Career guidance and networking opportunities for students.", Date=DateTime.Today.AddDays(4), Location="Town Hall" },
            new Event { Title="Municipal Budget Meeting", Category="Government", Description="Public presentation of the upcoming municipal budget and development priorities.", Date=DateTime.Today.AddDays(6), Location="Civic Centre" },
            new Event { Title="Water Conservation Workshop", Category="Environment", Description="Learn simple techniques to reduce water usage at home.", Date=DateTime.Today.AddDays(1), Location="Eco Centre" },
            new Event { Title="Loadshedding Notice", Category="Utilities", Description="Scheduled power outage.", Date=DateTime.Today.AddDays(5), Location="Zone 11" },
            new Event { Title="Community Soccer Tournament", Category="Sports", Description="Eight-team knockout competition for all local clubs.", Date=DateTime.Today.AddDays(12), Location="Athlone Stadium" },
            new Event { Title="Entrepreneurship Workshop", Category="Business", Description="Training session on business registration and funding opportunities.", Date=DateTime.Today.AddDays(9), Location="Innovation Hub" },
            new Event { Title="Road Repairs Update", Category="Infrastructure", Description="Repair works on the M3 to be completed this weekend.", Date=DateTime.Today.AddDays(7), Location="M3 Southbound" },
            new Event { Title="Community Choir Concert", Category="Culture", Description="Evening of gospel and traditional music.", Date=DateTime.Today.AddDays(11), Location="Arts Centre" },
            new Event { Title="Health Screening Campaign", Category="Health", Description="Free blood pressure and diabetes screening for residents.", Date=DateTime.Today.AddDays(3), Location="Community Clinic" },
            new Event { Title="Senior Citizen Picnic", Category="Community", Description="A social day for seniors with games, food, and music.", Date=DateTime.Today.AddDays(14), Location="Botanical Gardens" }
        };

        private static readonly Stack<string> searchHistory = new();
        private static readonly Dictionary<string, int> searchPattern = new();

        [HttpGet]
        public IActionResult Event(string? category, DateTime? date)
        {
            var events = SampleEvents.ToList();

            if (!string.IsNullOrEmpty(category))
            {
                events = events.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

                searchHistory.Push(category);
                if (searchPattern.ContainsKey(category))
                    searchPattern[category]++;
                else
                    searchPattern[category] = 1;
            }

            if (date.HasValue)
                events = events.Where(e => e.Date.Date == date.Value.Date).ToList();

            var categories = SampleEvents.Select(e => e.Category).Distinct().OrderBy(c => c).ToList();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd");

            IEnumerable<Event> recommendations = Enumerable.Empty<Event>();

            if (searchPattern.Count > 0)
            {
                var topCategory = searchPattern.OrderByDescending(d => d.Value).First().Key;
                recommendations = SampleEvents
                    .Where(e => e.Category.Equals(topCategory, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(e => e.Date)
                    .Take(3);
            }

            ViewBag.Recommendations = recommendations;
            var ordered = events.OrderBy(e => e.Date).ToList();
            return View(ordered);
        }

        // --------------------------- SERVICE REQUEST STATUS ---------------------------
        private static ServiceRequestStore _serviceStore;

        [HttpGet]
        public IActionResult ServiceRequests()
        {
            var requests = _serviceStore.GetAllRequests();
            var urgent = _serviceStore.GetUrgentRequests().OrderBy(r => r.Pritority).Take(3).ToList();
            ViewBag.Urgent = urgent;
            return View(requests);
        }

        [HttpGet]
        public IActionResult TrackServiceRequest(Guid id)
        {
            var req = _serviceStore.GetRequestByReference(id);
            if (req == null)
                return NotFound();

            ViewBag.Related = _serviceStore.GetRelatedRequests(id);
            return View(req);
        }
    }
}
