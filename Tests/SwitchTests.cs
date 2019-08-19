using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Argh.Utilities.Control;

namespace Argh.Tests
{
    internal class TestClass : ISwitchable
    {
        public string name;
        public string Name => name;
        public bool enabled = false;

        public void SetActive(bool val)
        {
            enabled = val;
        }
    }

    [TestFixture]
    public class ObjectSwitcherTest
    {
        private IObjectSwitcher<TestClass> switcher;
        private Dictionary<string, TestClass> testObjects;

        [SetUp]
        public void SetUp()
        {
            testObjects = new Dictionary<string, TestClass>();
            switcher = new ObjectSwitcher<TestClass>();
            for (int i = 0; i < 10; i++)
            {
                var obj = new TestClass() { name = i.ToString() };
                testObjects.Add(obj.name, obj);
                switcher.availableObjects.Add(obj);
            }
        }

        [Test]
        public void EnableOneObject()
        {
            switcher.ActivateObject("1");
            Assert.That(testObjects["1"].enabled, Is.True);
            foreach (var o in testObjects)
            {
                if (o.Value.name != "1") Assert.That(o.Value.enabled, Is.False);
            }
        }

        [Test]
        public void HideAll()
        {
            switcher.DeactivateAll();
            foreach (var o in testObjects)
            {
                Assert.That(o.Value.enabled, Is.False);
            }
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}