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
        public void CanSetAndGetValue()
        {
            dynamic one = new Node();
            one.People.Gerald.Age = 34;
            Assert.AreEqual(34, one.People.Gerald.Age);
        }

        [TestMethod]
        public void CanSetMergeAndGetValue()
        {
            dynamic one = new Node();
            one.People.Gerald.Age = 34;
            dynamic two = new Node();
            two.Pets.Fido.Age = 5;

            dynamic three = Node.Merge(one, two);
            Assert.AreEqual(34, three.People.Gerald.Age);
            Assert.AreEqual(5, three.Pets.Fido.Age);
        }

    }
}
