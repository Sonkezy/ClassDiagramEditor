using Avalonia;
using Avalonia.Controls;
using ClassDiagramEditor.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagramEditor.ViewModels
{
    public class AddElementViewModel : ViewModelBase
    {
        DiagramItemViewModel model = new DiagramItemViewModel(); 
        Mapper map;
        Point position;
        Window window;
        Element item = new Element();
        string name;
        int stereo;
        int access;
        ObservableCollection<Attribute> attributes= new ObservableCollection<Attribute>();
        ObservableCollection<Method> methods = new ObservableCollection<Method>();
        public AddElementViewModel(Mapper map, Point position, Window source)
        {
            this.map = map;
            this.position = position;
            this.window = source;
            Name = item.Name;
            Stereo= item.Stereo;
            Access= item.Access;
            Apply = ReactiveCommand.Create(() => { FuncApply(); });
            buttonAddAttribute = ReactiveCommand.Create(()=> { FuncAddAtribute(); });
            buttonRemoveAttribute = ReactiveCommand.Create<Attribute>(attribute => { FuncRemoveAttribute(attribute); });
            buttonAddMethod = ReactiveCommand.Create(() => { FuncAddMethod(); });
            buttonRemoveMethod = ReactiveCommand.Create<Method>(method => { FuncRemoveMethod(method); });
        }
        public AddElementViewModel(Mapper map, DiagramItemViewModel model, Point position, Window source)
        {
            this.model = model;
            this.map = map;
            this.position = position;
            this.window = source;
            item = model.Item;
            Name = item.Name;
            Stereo = item.Stereo;
            Access = item.Access;
            Attributes = new ObservableCollection<Attribute>(item.Attributes);
            Methods = new ObservableCollection<Method>(item.Methods);
            Apply = ReactiveCommand.Create(() => { FuncUpdate(); });
            buttonAddAttribute = ReactiveCommand.Create(() => { FuncAddAtribute(); });
            buttonRemoveAttribute = ReactiveCommand.Create<Attribute>(attribute => { FuncRemoveAttribute(attribute); });
            buttonAddMethod = ReactiveCommand.Create(() => { FuncAddMethod(); });
            buttonRemoveMethod = ReactiveCommand.Create<Method>(method => { FuncRemoveMethod(method); });
        }
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }
        public int Stereo
        {
            get => stereo;
            set => this.RaiseAndSetIfChanged(ref stereo, value);
        }
        public int Access
        {
            get => access;
            set => this.RaiseAndSetIfChanged(ref access, value);
        }
        public ObservableCollection<Attribute> Attributes
        {
            get => attributes;
            set => this.RaiseAndSetIfChanged(ref attributes, value);
        }
        public ObservableCollection<Method> Methods
        {
            get => methods;
            set => this.RaiseAndSetIfChanged(ref methods, value);
        }
        public void FuncApply()
        {
            item.Name = Name;
            item.Access = Access;
            item.Stereo = Stereo;
            item.Attributes = new List<Attribute>(Attributes.ToArray());
            item.Methods = new List<Method>(Methods.ToArray());
            map.AddItem(item, position);
            window.Close();
        }
        public void FuncUpdate()
        {
            item.Name = Name;
            item.Access = Access;
            item.Stereo = Stereo;
            item.Attributes = new List<Attribute>(Attributes.ToArray());
            item.Methods = new List<Method>(Methods.ToArray());
            map.Update(model,item);
            window.Close();
        }
        public void FuncAddAtribute() {
            Attributes.Add(new Attribute());
        }
        public void FuncRemoveAttribute(Attribute attribute)
        {
            Attributes.Remove(attribute);
        }
        public void FuncAddMethod()
        {
            Methods.Add(new Method());
        }
        public void FuncRemoveMethod(Method method)
        {
            Methods.Remove(method);
        }
        public ReactiveCommand<Unit, Unit> Apply { get; }
        public ReactiveCommand<Unit, Unit> buttonAddAttribute { get; }
        public ReactiveCommand<Attribute, Unit> buttonRemoveAttribute { get; }
        public ReactiveCommand<Unit, Unit> buttonAddMethod { get; }
        public ReactiveCommand<Method, Unit> buttonRemoveMethod { get; }

    }
}
