namespace ST10291238_PROG7312_POE.DataStructures
{
    public class RequestGraph
    {
        private readonly Dictionary<Guid, List<Guid>> adjacencyList = new();

        public void AddEdge(Guid a, Guid b)
        {
            if (!adjacencyList.ContainsKey(a))
            {
                adjacencyList[a] = new List<Guid>();
            }
            if (!adjacencyList.ContainsKey(b))
            {
                adjacencyList[b] = new List<Guid>();
            }

            adjacencyList[a].Add(b);
            adjacencyList[b].Add(a);
        }

        public List<Guid> GetConnections(Guid id)
        {
            return adjacencyList.ContainsKey(id) ? adjacencyList[id] : new List<Guid>();
        }
    }
}
