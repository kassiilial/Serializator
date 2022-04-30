using System.Collections.Generic;
using NUnit.Framework;
using SerializerTests.Implementations;
using SerializerTests.Nodes;

namespace TestSerializer
{
    public class TestDeepCopy
    {
        private ListNode node1 = new ();
        private IliaKassSerializer IliaKassSerializer = new ();
        [SetUp]
        public void Setup()
        {
            node1 = new ListNode
            {
                Previous = null,
                Next = null,
                Random = null,
                Data = 1.ToString()
            };
            var node2 = new ListNode
            {
                Previous = null,
                Next = null,
                Random = null,
                Data = 2.ToString()
            };
            var node3 = new ListNode
            {
                Previous = null,
                Next = null,
                Random = null,
                Data = 3.ToString()
            };
            var node4 = new ListNode
            {
                Previous = null,
                Next = null,
                Random = null,
                Data = 4.ToString()
            };
            node1.Next = node2;
            node2.Previous = node1;
            node2.Next = node3;
            node3.Previous = node2;
            node3.Next = node4;
            node4.Previous = node3;

            node1.Random = node4;
            node2.Random = node3;
            node3.Random = node3;
            node4.Random = node1;
        }
        
        [Test]
        public void DeepCopy_Is_Copy_Of_Null_Object()
        {
            node1 = null;
            //Arrange&Act
            var nodeCopy = IliaKassSerializer.DeepCopy(node1).GetAwaiter().GetResult();
            
            //Assert
            Assert.That(nodeCopy, Is.Null);
        }

        [Test]
        public void DeepCopy_Is_Not_The_Same_Object()
        {
            //Arrange&Act
            var nodeCopy = IliaKassSerializer.DeepCopy(node1);

            //Assert
            Assert.That(nodeCopy, !Is.SameAs(node1));
        }
        
        [Test]
        public void DeepCopy_Is_Copy_Of_Object()
        {
            //Arrange&Act
            var nodeCopy = IliaKassSerializer.DeepCopy(node1);
            
            //Assert
            Assert.That(nodeCopy.Result.Data, Is.EqualTo(node1.Data));
        }
        
        [Test]
        public void DeepCopy_Is_Not_The_Same_Collection()
        {
            //Arrange&Act
            var nodeCopy =  IliaKassSerializer.DeepCopy(node1).GetAwaiter().GetResult();
            var originalList = new List<ListNode>();
            var copyList = new List<ListNode>();

            while (nodeCopy.Next != null)
            {
                originalList.Add(nodeCopy);
                nodeCopy = nodeCopy.Next;
            }
            
            while (node1.Next != null)
            {
                copyList.Add(node1);
                node1 = node1.Next;
            }
            
            //Assert
            Assert.That(copyList, !Is.SameAs(originalList));
        }
        
        [Test]
        public void DeepCopy_Is_Copy_Of_Collection()
        {
            //Arrange&Act
            var nodeCopy = IliaKassSerializer.DeepCopy(node1).GetAwaiter().GetResult();
            
            var originalList = new List<string>();
            var copyList = new List<string>();

            while (nodeCopy.Next != null)
            {
                originalList.Add(nodeCopy.Data);
                nodeCopy = nodeCopy.Next;
            }
            
            while (node1.Next != null)
            {
                copyList.Add(node1.Data);
                node1 = node1.Next;
            }
            
            //Assert
            Assert.That(copyList, Is.EqualTo(originalList));
        }
    }
}