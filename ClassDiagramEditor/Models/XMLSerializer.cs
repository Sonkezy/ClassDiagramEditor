using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GraphicEditor.Models.Serializers
{
    class XMLSerializer<T>
    {
        public static void Save(string path, T item)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(file, item);
            }

        }
        public static T Load(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (T)serializer.Deserialize(file);
            }

        }
    }
}
