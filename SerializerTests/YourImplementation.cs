using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace SerializerTests.Implementations
{
    //Specify your class\file name and complete implementation.
    public class IliaKassSerializer : IListSerializer
    {
        //the constructor with no parameters is required and no other constructors can be used.
        public IliaKassSerializer()
        {
            //...
        }

        public async Task<ListNode> DeepCopy(ListNode head)
        {
            var list = new List<ListNode>();
            while (head.Next != null)
            {
                var node = new ListNode
                {
                    Data = head.Data
                };
                list.Add(node);
            }
            return await AddLinksToNodes(list);
        }
        public async Task<ListNode> Deserialize(Stream s)
        {
            TextReader textReader = new StreamReader(s);
            JsonReader reader = new JsonTextReader(textReader);
            var node = new ListNode();
            var list = new List<ListNode>();
            while (await reader.ReadAsync())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        node = new ListNode();
                        list.Add(node);
                        break;
                    case JsonToken.String:
                        node.Data =  reader.Value.ToString();
                        break;
                }
            }
            return await AddLinksToNodes(list);
        }
        public async Task Serialize(ListNode head, Stream s)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using JsonWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            await writer.WriteStartArrayAsync();
            while (head.Next != null)
            {
                await writer.WriteStartObjectAsync();
                await writer.WritePropertyNameAsync("Data");
                await writer.WriteValueAsync(head.Data);
                await writer.WriteEndObjectAsync();
                head = head.Next;
            }
            await writer.WriteStartObjectAsync();
            await writer.WritePropertyNameAsync("Data");
            await writer.WriteValueAsync(head.Data);
            await writer.WriteEndObjectAsync();
            await writer.WriteEndAsync();
            var streamWriter = new StreamWriter(s);
            await streamWriter.WriteAsync(sb);
            await streamWriter.FlushAsync();
            s.Position = 0;
        }
        private async Task<ListNode> AddLinksToNodes(List<ListNode> list)
        {
            if (list.Count==1)
            {
                return list[0];
            }
            list[0].Next = list[1];
            for (int i = 1; i < list.Count-1; i++)
            {
                list[i].Next = list[i + 1];
                list[i].Previous = list[i - 1];
            }
            list[^1].Previous = list[^2];
            return list[0];
        }
    }
}
