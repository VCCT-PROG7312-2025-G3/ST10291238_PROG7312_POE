using System;
using System.Collections.Generic;
using ST10291238_PROG7312_POE.Models;


namespace ST10291238_PROG7312_POE.DataStructures
{
    public class RequestNode
    {
        public ServiceRequest Data { get; set; }
        public RequestNode Left { get; set; }
        public RequestNode Right { get; set; }

        public RequestNode(ServiceRequest data)
        {
            Data = data;
        }
    }

    public class RequestBST
    {
        private RequestNode _root;

        public void Insert(ServiceRequest request)
        {
            _root = InsertRec(_root, request);
        }

        private RequestNode InsertRec(RequestNode root, ServiceRequest request)
        {
            if (root == null)
            {
                return new RequestNode(request);
            }

            if (request.Pritority < root.Data.Pritority)
            {
                root.Left = InsertRec(root.Left, request);
            }
            else
            {
                root.Right = InsertRec(root.Right, request);
            }

            return root;
        }

        public List<ServiceRequest> InOrder()
        {
            var list = new List<ServiceRequest>();
            InOrderRec(_root, list);
            return list;
        }

        private void InOrderRec(RequestNode root, List<ServiceRequest> list)
        {
            if (root == null) return;
            InOrderRec(root.Left, list);
            list.Add(root.Data);
            InOrderRec(root.Right, list);
        }

        public ServiceRequest FindByReference(Guid reference)
        {
            return FindRec(_root, reference);
        }

        private ServiceRequest FindRec(RequestNode root, Guid reference)
        {
            if (root == null)
            {
                return null;
            }
                
            if (root.Data.Reference == reference)
            {
                return root.Data;
            }

            var left = FindRec(root.Left, reference);
            if (left != null)
            {
                return left;
            }

            return FindRec(root.Right, reference);
        }
    }
}
