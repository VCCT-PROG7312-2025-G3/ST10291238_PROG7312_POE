using System;
using System.Collections.Generic;
using System.Linq;
using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.Services
{
    public class EventStore
    {
        private readonly SortedDictionary<DateTime, Queue<Event>> eventsByDate;
        private readonly Dictionary<string, HashSet<Event>> eventsByCategory;
        private readonly HashSet<string> categories;
        private readonly Stack<string> searchHistory;
        private readonly Dictionary<string, int> searchPattern;

        public EventStore()
        {
            eventsByDate = new SortedDictionary<DateTime, Queue<Event>>();
            eventsByCategory = new Dictionary<string, HashSet<Event>>();
            categories = new HashSet<string>();
            searchHistory = new Stack<string>();
            searchPattern = new Dictionary<string, int>();
        }

        public void AddEvent(Event e)
        {
            if (!eventsByDate.ContainsKey(e.Date.Date))
            {
                eventsByDate[e.Date.Date] = new Queue<Event>();
            }
            eventsByDate[e.Date.Date].Enqueue(e);

            if (!eventsByCategory.ContainsKey(e.Category))
            {
                eventsByCategory[e.Category] = new HashSet<Event>();
            }
            eventsByCategory[e.Category].Add(e);

            categories.Add(e.Category);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            foreach (var d in eventsByDate)
            {
                foreach (var e in d.Value)
                {
                    yield return e;
                }
            }
        }

        public IEnumerable<string> GetCategories() => categories;

        public IEnumerable<Event> Search(string category, DateTime? date)
        {
            var result = new List<Event>();

            if (!string.IsNullOrWhiteSpace(category) && eventsByCategory.ContainsKey(category))
            {
                result.AddRange(eventsByCategory[category]);
            }
            else
            {
                result.AddRange(GetAllEvents());
            }

            if (date.HasValue && eventsByDate.ContainsKey(date.Value.Date))
            {
                result.AddRange(eventsByDate[date.Value.Date]);
            }
            else
            {
                result.AddRange(GetAllEvents());
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                searchHistory.Push(category);
                if (searchPattern.ContainsKey(category))
                {
                    searchPattern[category]++;
                }
                else
                {
                    searchPattern[category] = 1;
                }
            }

            return result.Distinct().OrderBy(e => e.Date);
        }

        public IEnumerable<Event> Recommend()
        {
            if (searchPattern.Count == 0)
            {
                return Enumerable.Empty<Event>();
            }

            var topCategory = searchPattern.OrderByDescending(d => d.Value).First().Key;
            return eventsByCategory.ContainsKey(topCategory) ? eventsByCategory[topCategory] : Enumerable.Empty<Event>();
        }
    }
}

