using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace GraphicEditor.Models.Serializers
{
    class JSONSerializer<T>
    {
        public static void Save(string path, T item)
        {
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize<T>(file, item);
            }
            

        }
        public static T Load(string path)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (T)JsonSerializer.Deserialize<T>(file);
            }

        }
    }
}
