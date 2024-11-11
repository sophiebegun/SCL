using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class MergeS
    {
        // Merge Sort function
        public static void MergeSort(List<int> l, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                Console.WriteLine("mid " + mid);
                MergeSort(l, left, mid);
                MergeSort(l, mid + 1, right);
                Merge(l, left, mid, right);
            }
        }

        // Merge function to merge two sorted subarrays
        public static void Merge(List<int> l, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            List<int> leftL = new List<int>();
            List<int> rightL = new List<int>();

            // Copying data to temp lists leftL and rightL
            for (int i = 0; i < n1; i++)
            {
                Console.WriteLine("d1:" + (left + i) + ", " + l[left + i]);
                leftL.Add(l[left + i]);
            }

            for (int i = 0; i < n2; i++)
            {
                Console.WriteLine("d2:" + (mid + 1 + i) +  ", " + l[mid + 1 + i]);
                rightL.Add(l[mid + 1 + i]);
            }

            int i1 = 0, i2 = 0, k = left;

            Console.WriteLine("n1 " + n1 + "," + "n2 " + n2 + ", k " + k);
            Print(leftL);
            Print(rightL);

            // Merging the two lists back into the main list
            while (i1 < n1 && i2 < n2)
            {
                Console.WriteLine(i1 + " " + i2);
                Console.WriteLine("Left len " + leftL.Count + " Right len " + rightL.Count);

                if (leftL[i1] <= rightL[i2])
                {
                    Console.WriteLine("if");
                    l[k] = leftL[i1];
                    i1++;
                }
                else
                {
                    Console.WriteLine("else");
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
        public static void Print(List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                Console.Write(l[i] + " ");
            }
            Console.WriteLine();
        }

    }
}
