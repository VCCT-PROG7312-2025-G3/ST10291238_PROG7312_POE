using System.Collections;
using System.Collections.Generic;
using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.DataStructures
{
    public class IssueLinkedList : IEnumerable<Issue>
    {
        private IssueNode? _head;
        private IssueNode? _tail;
        public int Count { get; private set; }

        public void AddLast(Issue issue)
        {
            var node = new IssueNode(issue);
            if (_head is null)
            {
                _head = _tail = node;
            }
            else
            {
                _tail!.Next = node;
                _tail = node;
            }
            Count++;
        }

        public Issue? FindById(System.Guid id)
        {
            var current = _head;
            while (current is not null)
            {
                if (current.Value.Id == id)
                {
                    return current.Value;
                }
                current = current.Next;
            }
            return null;
        }

        public IEnumerator<Issue> GetEnumerator()
        {
            var current = _head;
            while (current is not null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
