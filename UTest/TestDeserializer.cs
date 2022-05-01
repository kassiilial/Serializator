using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using SerializerTests.Implementations;

namespace UTest
{
    public class TestDeserializer
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Deserialize_Not_Correct_Data()
        {
            //Arrange
            var serializer = new IliaKassSerializer();
            var json = @"{""Name"":""Ilia""]";
            var s = new MemoryStream(Encoding.UTF8.GetBytes(json));
            
            //Assert&Act
            Assert.That(()=>serializer.Deserialize(s).GetAwaiter().GetResult(), Throws.TypeOf<ArgumentException>());
        }
    }
}