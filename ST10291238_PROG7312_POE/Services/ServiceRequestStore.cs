using System;
using System.Collections.Generic;
using ST10291238_PROG7312_POE.Models;
using ST10291238_PROG7312_POE.DataStructures;

namespace ST10291238_PROG7312_POE.Services
{
    public class ServiceRequestStore
    {
        private readonly RequestBST _tree = new RequestBST();
        private readonly MinHeap _heap = new MinHeap();
        private readonly RequestGraph _graph = new RequestGraph();
        private readonly Dictionary<Guid, ServiceRequest> _requests = new Dictionary<Guid, ServiceRequest>();

        public ServiceRequestStore()
        {
            SeedData();
        }

        private void SeedData()
        {
            var samples = new List<ServiceRequest>
            {
                new() { ResidentName="John Doe", Location="Fairways", Category="Water", Description="Burst pipe", Status="In Progress", Pritority=1 },
                new() { ResidentName="Anna Smith", Location="Mitchell's Plain", Category="Roads", Description="Pothole repair", Status="Completed", Pritority=3 },
                new() { ResidentName="Thabo M", Location="Rondebosch", Category="Electricity", Description="Power outage", Status="Submitted", Pritority=2 },
                new() { ResidentName="Mary P", Location="Newlands", Category="Sanitation", Description="Blocked drain", Status="In Progress", Pritority=1 },
                new() { ResidentName="Sipho K", Location="Wynberg", Category="Parks", Description="Damaged playground", Status="Submitted", Pritority=2 },
                new() { ResidentName="Linda G", Location="Gardens", Category="Lighting", Description="Streetlight not working", Status="Completed", Pritority=3 },
                new() { ResidentName="Jason L", Location="Tableview", Category="Waste", Description="Uncollected bins", Status="In Progress", Pritority=2 },
                new() { ResidentName="Neo R", Location="Goodwood", Category="Roads", Description="Broken traffic light", Status="Submitted", Pritority=1 }
            };

            foreach (var req in samples)
            {
                Add(req);
            }

            _graph.AddEdge(samples[0].Reference, samples[3].Reference);
            _graph.AddEdge(samples[2].Reference, samples[7].Reference);
        }

        public void Add(ServiceRequest request)
        {
            _requests[request.Reference] = request;
            _tree.Insert(request);
            _heap.Insert(request);
        }

        public List<ServiceRequest> GetAllRequests()
        {
            return _tree.InOrder();
        }

        public ServiceRequest GetRequestByReference(Guid id)
        {
            return _requests.ContainsKey(id) ? _requests[id] : null;
        }

        public List<ServiceRequest> GetUrgentRequests()
        {
            return _heap.GetAll();
        }

        public List<Guid> GetRelatedRequests(Guid id)
        {
            return _graph.GetConnections(id);
        }
    }
}
