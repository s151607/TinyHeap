using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDCC_TECHQ3;

namespace TinyHeapTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AllocTinyHeap_ValidAllocSize()
        {
            TinyHeap tinyHeap = new TinyHeap();
            Assert.AreEqual(tinyHeap.AllocTinyHeap(1000*1000*5), true);

            tinyHeap.DeleteTinyHeap();
        }

        [TestMethod]
        public void AllocTinyHeap_AllocSizeLessThanZero()
        {
            TinyHeap tinyHeap = new TinyHeap();
            Assert.AreEqual(tinyHeap.AllocTinyHeap(-1337), false);
        }

        // Unable to reliably force OOM exception as AllocHGlobal is limited to ~2GB due to it
        // taking int32 as input. Could do several allocations, but when the exception triggers
        // would be dependant on the machine running it.
        /*[TestMethod]
        public void AllocTinyHeap_AllocSizeTooLarge()
        {
            TinyHeap tinyHeap = new TinyHeap();
            Assert.ThrowsException<OutOfMemoryException>(() => tinyHeap.AllocTinyHeap(2147483647));
        }*/
    }
}
