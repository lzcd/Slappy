using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Slappy;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            dynamic one = new Node();
            one.People.Gerald.Age = 34;
            Assert.AreEqual(34, one.People.Gerald.Age);
        }
    }
}
