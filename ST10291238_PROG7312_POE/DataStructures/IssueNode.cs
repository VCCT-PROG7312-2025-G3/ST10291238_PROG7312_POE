using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.DataStructures
{
    public class IssueNode
    {
        public Issue Value { get; }
        public IssueNode? Next { get; set; }

        public IssueNode(Issue value) => Value = value;
    }
}
