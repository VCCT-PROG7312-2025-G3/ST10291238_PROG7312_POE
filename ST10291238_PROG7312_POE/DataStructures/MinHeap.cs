using System;
using System.Collections.Generic;
using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.DataStructures
{
    public class MinHeap
    {
        private readonly List<ServiceRequest> heap = new List<ServiceRequest>();

        private int Parent(int i) => (i - 1) / 2;
        private int LeftChild(int i) => 2 * i + 1;
        private int RightChild(int i) => 2 * i + 2;

        private void Swap(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public void Insert(ServiceRequest item)
        {
            heap.Add(item);
            int i = heap.Count - 1;

            while (i >= 0 && heap[Parent(i)].Pritority > heap[i].Pritority)
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        public ServiceRequest Peek() => heap.Count > 0 ? heap[0] : null;

        public List<ServiceRequest> GetAll() => heap;
    }
}
