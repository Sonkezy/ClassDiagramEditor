using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagramEditor.Models
{
    public class Method : AbstractNotifyPropertyChanged
    {
        
        string name = "mn";
        string type = "mt";
        int access = 0;
        int stereo = 0;
        static readonly string[] stereos = new string[] { "virtual", "static", "abstract", "«create»" };
        ObservableCollection<Property> properties = new();

        
        public string Name { get => name; set => SetAndRaise(ref name, value); }
        public string Type { get => type; set => SetAndRaise(ref type, value); }
        public int Access { get => access; set => SetAndRaise(ref access, value); }
        public int Stereo { get => stereo; set => SetAndRaise(ref stereo, value); }
        public ObservableCollection<Property> Properties { get => properties; }
        public string Data { get => this.ToString(); }

        public Method() {

            buttonAddProperty = ReactiveCommand.Create(() => { FuncAddProperty(); });
            buttonRemoveProperty = ReactiveCommand.Create<Property>(property => { FuncRemoveProperty(property); });
        }
        public void FuncAddProperty()
        {
            Properties.Add(new Property());
        }
        public void FuncRemoveProperty(Property property)
        {
            Properties.Remove(property);
        }
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("-+#~"[access]);
            if (stereo != 0)
            {
                sb.Append(' ');
                sb.Append(stereos[stereo]);
            }
            sb.Append(' ');
            sb.Append(name);
            sb.Append(" (");
            sb.Append(string.Join(", ", properties));
            sb.Append("): ");
            sb.Append(type);
            return sb.ToString();
        }
        public ReactiveCommand<Unit, Unit> buttonAddProperty { get; }
        public ReactiveCommand<Property, Unit> buttonRemoveProperty { get; }
    }
}
