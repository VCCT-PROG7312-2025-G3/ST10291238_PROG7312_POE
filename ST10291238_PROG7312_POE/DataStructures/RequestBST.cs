using System;
using System.Collections.Generic;
using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.DataStructures
{
    public class RequestNode
    {
        public ServiceRequest Data;
        public RequestNode Left, Right;
        public RequestNode(ServiceRequest data) => Data = data;
    }

    public class RequestBST
    {
        private RequestNode _root;

        public void Insert(ServiceRequest req)
        {
            _root = InsertRec(_root, req);
        }

        private RequestNode InsertRec(RequestNode root, ServiceRequest req)
        {
            if (root == null) return new RequestNode(req);
            if (req.CreatedDate < root.Data.CreatedDate)
                root.Left = InsertRec(root.Left, req);
            else
                root.Right = InsertRec(root.Right, req);
            return root;
        }

        public List<ServiceRequest> InOrder()
        {
            var list = new List<ServiceRequest>();
            Traverse(_root, list);
            return list;
        }

        private void Traverse(RequestNode root, List<ServiceRequest> list)
        {
            if (root == null) return;
            Traverse(root.Left, list);
            list.Add(root.Data);
            Traverse(root.Right, list);
        }
    }
}
