using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class mergesortc_
    {
        static void MergeSort(List<int> l, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSort(l, left, mid);
                MergeSort(l, mid + 1, right);
                Merge(l, left, mid, right);
            }
        }

        // Merge function to merge two sorted subarrays
        static void Merge(List<int> l, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            List<int> leftL = new List<int>(n1);
            List<int> rightL = new List<int>(n2);

            // Copying data to temp lists leftL and rightL
            for (int i = 0; i < n1; i++)
            {
                leftL.Add(l[left + i]);
            }

            for (int i = 0; i < n2; i++)
            {
                rightL.Add(l[mid + 1 + i]);
            }

            int i1 = 0, i2 = 0, k = left;

            // Merging the two lists back into the main list
            while (i1 < n1 && i2 < n2)
            {
                if (leftL[i1] <= rightL[i2])
                {
                    l[k] = leftL[i1];
                    i1++;
                }
                else
                {
                    l[k] = rightL[i2];
                    i2++;
                }
                k++;
            }

            // Copy remaining elements of leftL, if any
            while (i1 < n1)
            {
                l[k] = leftL[i1];
                i1++;
                k++;
            }

            // Copy remaining elements of rightL, if any
            while (i2 < n2)
            {
                l[k] = rightL[i2];
                i2++;
                k++;
            }
        }

        // Print function to print the list
        static void Print(List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                Console.Write(l[i] + " ");
            }
            Console.WriteLine();
        }

        // Main function to test the merge sort
        public static void TestCSharp(string[] args)
        {
            List<int> l = new List<int> { 12, 11, 13, 5, 6, 7 };

            Console.WriteLine("Given list:");
            Print(l);

            MergeSort(l, 0, l.Count - 1);

            Console.WriteLine("\nSorted list:");
            Print(l);
        }
    }
}
