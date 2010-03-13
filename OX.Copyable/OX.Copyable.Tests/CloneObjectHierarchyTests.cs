using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OX.Copyable.Tests
{
    /// <summary>
    /// Summary description for CloneObjectHierarchyTests
    /// </summary>
    [TestClass]
    public class CloneObjectHierarchyTests
    {
        public CloneObjectHierarchyTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        class Node
        {
            private Node _prev;
            private Node _next;
            private string name;
            public Node Prev { get { return _prev; } set { _prev = value; } }
            public Node Next { get { return _next; } set { _next = value; } }
            public string Name { get { return name; } set { name = value; } }
        }

        [TestMethod]
        public void TestObjectHierarchyClone()
        {
            Node n1 = new Node();
            n1.Name = "Node 1";
            Node n2 = new Node();
            n2.Name = "Node 2";
            
            n1.Next = n2;
            Node n1c = (Node)n1.Copy();

            Assert.AreNotSame(n1, n1c);
            Assert.AreNotSame(n2, n1c.Next);
            Assert.IsNotNull(n1c.Next);
            Assert.AreEqual(n1.Name, n1c.Name);
            Assert.AreEqual(n2.Name, n1c.Next.Name);
        }

        [TestMethod]
        public void TestCyclicObjectHierarchyClone()
        {
            Node n1 = new Node();
            n1.Name = "Node 1";
            Node n2 = new Node();
            n2.Name = "Node 2";

            n1.Next = n2;
            n2.Prev = n1;
            Node n1c = (Node)n1.Copy();

            Assert.AreNotSame(n1, n1c);
            Assert.AreNotSame(n2, n1c.Next);
            Assert.IsNotNull(n1c.Next);
            Assert.AreEqual(n1.Name, n1c.Name);
            Assert.AreEqual(n2.Name, n1c.Next.Name);
            Assert.AreSame(n1c, n1c.Next.Prev);
        }

        [TestMethod]
        public void TestCloneHumanInOtherAssembly()
        {
            Human father = new Human();
            father.Gender = Gender.Male;
            father.Name = "Dad";
            father.Children = new List<Human>();

            Human son = new Human();
            son.Gender = Gender.Male;
            son.Name = "Sonny";

            father.Children.Add(son);

            // Crazy science
            Human sensation = (Human)father.Copy();

            Assert.AreNotSame(father, sensation);
            Assert.AreNotSame(father.Children, sensation.Children);
            Assert.IsNotNull(sensation.Children);
            Assert.AreEqual(1, sensation.Children.Count);
            Assert.AreNotSame(father.Children[0], sensation.Children[0]);
            Assert.AreEqual(father.Name, sensation.Name);
            Assert.AreEqual(father.Children[0].Name, sensation.Children[0].Name);
        }
    }
}
