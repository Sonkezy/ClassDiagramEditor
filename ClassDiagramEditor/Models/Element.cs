using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagramEditor.Models
{
    public class Element : AbstractNotifyPropertyChanged
    {
        string name = "name";
        int access = 0;
        int stereo = 0;
        static readonly string[] stereos = new string[] { "", "static", "abstract", "interface" };
        List<Attribute> attributes= new List<Attribute>();
        List<Method> methods = new List<Method>();


        public string Name { get => name; set => SetAndRaise(ref name, value); }
        public int Access { get => access; set => SetAndRaise(ref access, value); }
        public int Stereo { get => stereo; set => SetAndRaise(ref stereo, value); }
        public List<Attribute> Attributes { get => attributes; set => SetAndRaise(ref attributes, value); }
        public List<Method> Methods { get => methods; set => SetAndRaise(ref methods, value); }
        
        public Element() { }
        public string GetName()
        {
            StringBuilder sb = new();
            sb.Append(" -+#~"[access]);
            sb.Append(' ');
            sb.Append(name);
            return sb.ToString();
        }
         public string GetStereo()
        {
            StringBuilder sb = new();
            if (stereo != 0)
            {
                sb.Append("«");
                sb.Append(stereos[stereo]);
                sb.Append('»');
            }
            return sb.ToString();
        }
    }
}
