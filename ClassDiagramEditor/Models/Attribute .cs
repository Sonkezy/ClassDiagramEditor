using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagramEditor.Models
{
    public class Attribute : AbstractNotifyPropertyChanged
    {
        string name = "name";
        string type = "type";
        int access = 0;
        bool _readonly = false;
        bool _static = false;
        int stereo = 0;
        static readonly string[] stereos = new string[] { "event", "property", "required" };
        string _default = "def";

        public string Name { get => name; set => SetAndRaise(ref name, value); }
        public string Type { get => type; set => SetAndRaise(ref type, value); }
        public int Access { get => access; set => SetAndRaise(ref access, value); }
        public bool IsReadonly { get => _readonly; set => SetAndRaise(ref _readonly, value); }
        public bool IsStatic { get => _static; set => SetAndRaise(ref _static, value); }
        public int Stereo { get => stereo; set => SetAndRaise(ref stereo, value); }
        public string Default { get => _default; set => SetAndRaise(ref _default, value); }
        public string Data { get => this.ToString(); }

        public Attribute() { }
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("-+#~"[access]);
            if (stereo != 0)
            {
                sb.Append(" «");
                sb.Append(stereos[stereo - 1]);
                sb.Append('»');
            }
            if (_static) sb.Append(" static");
            if (_readonly) sb.Append(" readonly");
            sb.Append(' ');
            sb.Append(name);
            sb.Append(" : ");
            sb.Append(type);
            if (_default != "")
            {
                sb.Append(" = ");
                sb.Append(_default);
            }
            return sb.ToString();
        }
    }
}
