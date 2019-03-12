using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Lazy.Tests
{
    [TestClass]
    public class SingleLazyTest
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
            var lazy = LazyFactory.SingleThreadLazy(supplier);
            Assert.IsTrue(lazy.IsFirstCalculation());
            while (step < 5)
            {
                lazy.Get();
                Assert.IsFalse(lazy.IsFirstCalculation());
                step++;
            }           
        }        
                                       
        [TestMethod]
        public void GetTest_CanSupplierReturnNullByGet()
        {
            Func<string> supplierNull = () => null;
            var lazy = LazyFactory.SingleThreadLazy(supplierNull);
            Assert.IsNull(lazy.Get());
        }
        
        [TestMethod]
        public void GetTest_IsReturnCorrectResult()
        {
            var lazy = LazyFactory.SingleThreadLazy(supplier);
            Assert.AreEqual(1, lazy.Get());
        }
    }
}