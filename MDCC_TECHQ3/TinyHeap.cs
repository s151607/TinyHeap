using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MDCC_TECHQ3
{
    //Based on the methods i assume it should be a minimal version heap memory and not a min-heap.
    // Found it awkward to design as C# doesn't allow me to manually free memory and GC won't take it
    // until all references to it are removed. Therefore all the references given out with tinyAlloc must
    // be removed before DeleteTinyHeap works.
    public class TinyHeap
    {
        int heapSize;
        unsafe void* heap = null;
        // BitArray to keep track of which blocks are used. Used is represented by 1.
        BitArray usedBlocks;

        // Allocates unmanaged memory for the heap. Takes an int as arg representing
        // the size (in bytes) to allocate. Returns true on succes, false if failed.
        public unsafe bool AllocTinyHeap(int bytes)
        {
            // Return false on negative input.
            if(bytes < 0)
            {
                return false;
            }

            // If another heap is allocated while one exists, delete the old first.
            if(heap != null)
            {
                DeleteTinyHeap();
            }

            // Attempts to allocate memory. Return false if allocation is too large.
            try
            {
              heap = (void*)Marshal.AllocHGlobal(bytes);
            }
            catch (OutOfMemoryException) 
            {
                return false;
            }

            heapSize = bytes;
            usedBlocks = new BitArray(bytes);
            return true;
        }

        // Frees any memory allocated to the heap. Does nothing if called on heap
        // with no allocation.
        public unsafe void DeleteTinyHeap()
        {
            Marshal.FreeHGlobal((IntPtr)heap);
            heap = null;
            heapSize = 0;
        }

        // Allocates space on the heap if a large enough block is present.
        // Returns a pointer to the start of the block or null if none is found.
        public unsafe byte* TinyAlloc(int bytes)
        {
            // Use the LocateFreeBlock helper method to find a block.   
            int startIndex = LocateFreeBlock(bytes);
            if (startIndex == -1)
            {
                return null;
            }
          
            // Mark the newly used mem as occuppied. 
            for(int i = startIndex; i < (startIndex + bytes); i++)
            {
                usedBlocks[i] = true;
            }

            return ((byte*)heap) + startIndex;
        }

        // Takes a pointer to the start of a block of memory to be freed and a size.
        // Accessing the mem after freeing is undefined.
        public unsafe void TinyFree(void* ptr, int bytes)
        {
            // Finds startindex of the memory to be freed.
            int index = (int)ptr - (int)heap;

            // Marks the memory as free
            for (int i = index; i < (index + bytes); i++)
            {
                try
                {
                    usedBlocks[i] = false;
                }
                catch (System.IndexOutOfRangeException) { }
            }
        }

        // Takes a byte amount and look for a block of memory in the heap large enough.
        // Returns a the index of the start of the block or -1 if no block is found.
        private int LocateFreeBlock(int bytes)
        {
            // Fail if trying to locate more than heapSize
            if (bytes > heapSize) return -1;

            int counter = 0;
            for (int i = 0; i < heapSize; i++)
            {
                if (counter == bytes) return i - bytes;
                else if (usedBlocks[i] == true)
                {
                    counter++;
                }
                else if (usedBlocks[i] == false)
                {
                    counter = 0;
                }           
            }
            // Fail if no block large enough is found
            return -1;
        }

        static void Main(string[] args)
        {
 
        }
    }
}
