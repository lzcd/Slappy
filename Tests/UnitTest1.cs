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
        public void CanSetAndGetValueByIndex()
        {
            dynamic one = new Node();
            one["People"]["Gerald"]["Age"] = 34;
            Assert.AreEqual(34, one["People"]["Gerald"]["Age"]);
        }

        [TestMethod]
        public void CanSetMergeAndGetValue()
        {
            dynamic bobs = new Node();
            bobs.Recipe.Eggs.Count = 5;
            bobs.Commit();

            dynamic alices = bobs.Clone();
            alices.Recipe.Butter.Amount = 4;
            alices.Recipe.Eggs.Count = 3;
            alices.Commit();

            bobs.Recipe.Salt.Amount = 7;
            bobs.Commit();

            bobs.Pull(alices);
        }

    }
}
