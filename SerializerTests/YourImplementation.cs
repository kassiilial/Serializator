using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace SerializerTests.Implementations
{
    //Specify your class\file name and complete implementation.
    public class IliaKassSerializer : IListSerializer
    {
        private Dictionary<ListNode, ListNode> visitedNode = new();
        //the constructor with no parameters is required and no other constructors can be used.
        public IliaKassSerializer()
        {
            //...
        }

        public async Task<ListNode> DeepCopy(ListNode head)
        {
            if (head == null)
            {
                return null;
            }
            if (visitedNode.ContainsKey(head))
            {
                return visitedNode[head];
            }
            var copyNode = new ListNode
            {
                Data = head.Data
            };
            visitedNode.TryAdd(head, copyNode);
            copyNode.Next = await DeepCopy(head.Next);
            copyNode.Previous = await DeepCopy(head.Previous);
            copyNode.Random = await DeepCopy(head.Random);
            return copyNode;
        }
        public async Task<ListNode> Deserialize([NotNull]Stream s)
        {
            var options = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
            var streamReader = new StreamReader(s);
            return JsonConvert.DeserializeObject<ListNode>(await streamReader.ReadToEndAsync(), options);
        }
        public async Task Serialize([NotNull]ListNode head, [NotNull]Stream s)
        {
            var options = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
            var json = JsonConvert.SerializeObject(head, options);
            var streamWriter = new StreamWriter(s);
            await streamWriter.WriteAsync(json);
            await streamWriter.FlushAsync();
            s.Position = 0;
        }
    }
}
