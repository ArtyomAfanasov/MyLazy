using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Lazy.Tests
{
    [TestClass]
    public class MultipleLazyTest
    {       
        private int sum;
        private Func<int> supplier;

        [TestInitialize]
        public void Initialize()
        {
            sum = 0;
            supplier = () => ++sum;           
        }                              
        
        [TestMethod]
        public void GetTest_IsCalculationCalledOnce()
        {
            int step = 0;
            var lazyForThreads = LazyFactory.MultipleThreadLazy(supplier);
            Assert.IsTrue(lazyForThreads.IsFirstCalculation());
            while (step < 5)
            {
                lazyForThreads.Get();
                Assert.IsFalse(lazyForThreads.IsFirstCalculation());
                step++;
            }
        }
             
        [TestMethod]
        public void GetTest_CanSupplierReturnNullByGet()
        {
            Func<string> supplierNull = () => null;
            var lazyForThreads = LazyFactory.MultipleThreadLazy(supplierNull);
            Assert.IsNull(lazyForThreads.Get());
        }        

        [TestMethod]
        public void GetTest_NotNullSupplierDontReturnDefault()
        {
            var lazy = LazyFactory.MultipleThreadLazy(supplier);
            var threads = new Thread[100000];
            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(() => lazy.Get());
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreNotEqual(0, lazy.Get());
        }

        [TestMethod]
        public void GetTest_IsReturnCorrectResult()
        {            
            var lazy = LazyFactory.MultipleThreadLazy(supplier);            
            var threads = new Thread[100000];
            for (int i = 0; i < threads.Length; ++i)
            {                                
                threads[i] = new Thread(() => lazy.Get());
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            
            Assert.AreEqual(1, lazy.Get());
        }
        
        // Is the result correct
        [TestMethod]
        public void GetTest_IsCorrectResult()
        {           
            var lazyForThreads = LazyFactory.MultipleThreadLazy(supplier);
            Assert.AreEqual(1, lazyForThreads.Get());
        }     
    }
}