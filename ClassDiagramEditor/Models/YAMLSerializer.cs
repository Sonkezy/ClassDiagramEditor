using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GraphicEditor.Models.Serializers
{
    class YAMLSerializer<T>
    {
        public static void Save(string path, T item)
        {
            var serializer = new SerializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();
            using (StreamWriter file = new StreamWriter(path))
            {
                serializer.Serialize(file, item);
            }
            



        }
        public static T Load(string path)
        {
            var deserializer = new DeserializerBuilder()
             .IgnoreUnmatchedProperties()
             .WithNamingConvention(UnderscoredNamingConvention.Instance)
             .Build();
            using (StreamReader file = new StreamReader(path))
            {
                return deserializer.Deserialize<T>(file);
            }

        }
    }
}
