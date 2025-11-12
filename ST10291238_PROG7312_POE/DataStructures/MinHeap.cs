using System;
using System.Collections.Generic;
using ST10291238_PROG7312_POE.Models;

namespace ST10291238_PROG7312_POE.DataStructures
{
    public class MinHeap
    {
        private readonly List<ServiceRequest> _heap = new List<ServiceRequest>();

        private int Parent(int i) => (i - 1) / 2;
        private int Left(int i) => 2 * i + 1;
        private int Right(int i) => 2 * i + 2;

        public void Insert(ServiceRequest r)
        {
            _heap.Add(r);
            int i = _heap.Count - 1;

            while (i >= 0 && _heap[Parent(i)].Pritority > _heap[i].Pritority)
            {
                ( _heap[i], _heap[Parent(i)] ) = (_heap[Parent(i)], _heap[i] );
                i = Parent(i);
            }
        }

        public List<ServiceRequest> GetAll() => _heap;
    }
}
