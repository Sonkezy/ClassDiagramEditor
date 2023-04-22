using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagramEditor.Models
{
    public class Property : AbstractNotifyPropertyChanged
    {
        string name = "name";
        string type = "type";
        string _default = "default";

        public string Name { get => name; set => SetAndRaise(ref name, value); }
        public string Type { get => type; set => SetAndRaise(ref type, value); }
        public string Default { get => _default; set => SetAndRaise(ref _default, value); }

        public override string ToString() => $"{name} : {type}" + (_default == "" ? "" : " = " + _default);
    }
}
