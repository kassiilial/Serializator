using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SerializerTests.Implementations;
using SerializerTests.Nodes;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var node1 = new ListNode
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
          // node2.Next = node3;
            node3.Previous = node2;
           node3.Next = node4;
           node4.Previous = node3;

            /*using (Stream s = new MemoryStream())
            {
                       IliaKassSerializer serializer = new IliaKassSerializer();
                            
                            await serializer.Serialize(node1, s);
                            Console.WriteLine("Serialize");
                            var firstnode = await serializer.Deserialize(s);
                            Console.WriteLine(firstnode.Data);
                            Console.WriteLine(firstnode.Next.Data);
                            Console.WriteLine(firstnode.Next.Next.Data);
                            Console.WriteLine(firstnode.Next.Next.Next.Data);
            }*/

            var options = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
            var a = JsonConvert.SerializeObject(node1, options);
            Console.WriteLine(a);
            var b = JsonConvert.DeserializeObject<ListNode>(a, options);
            Console.WriteLine(b.Data);
            Console.WriteLine(b.Next.Data);


        }
    }
}