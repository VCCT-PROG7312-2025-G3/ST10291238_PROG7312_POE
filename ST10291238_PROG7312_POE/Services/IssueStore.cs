using System;
using ST10291238_PROG7312_POE.DataStructures;
using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.Services
{
    public class IssueStore
    {
        private readonly IssueLinkedList _issues = new();
        private readonly object _lock = new();

        public void Add(Issue issue)
        {
           lock (_lock)
           {
                _issues.AddLast(issue);
           }
        }

        public Issue? Get(Guid id)
        {
            lock (_lock)
            {
                return _issues.FindById(id);
            }
        }

        public int Count
        {
            get { lock (_lock) { return _issues.Count; } }
        }
    }
}
