using ClassDiagramEditor.Models;
using ClassDiagramEditor.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Diagnostics;
using Avalonia.Controls;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.LogicalTree;
using DynamicData.Aggregation;

namespace ClassDiagramEditor.ViewModels
{
    public class DiagramItemViewModel : ViewModelBase
    {
        Element item;
        string name = "default_name";
        bool typeSelected = false;
        string type = "";
        List<Attribute> attributes = new List<Attribute> { new Attribute() };
        List<Method> methods = new List<Method> { new Method() };
        readonly StackPanel[] panels;
        ushort width = 150;
        ushort height = 150;
        DiagramItem source;
        Point position = new Point(0,0);
        public DiagramItemViewModel() : this(new DiagramItem(),new Element { Attributes = new List<Attribute> { new Attribute() } , 
                                                              Methods = new List<Method> { new Method() } }) { }
        public DiagramItemViewModel(DiagramItem source,Element item) {
            Item = item;
            name = item.GetName();
            type = item.GetStereo();
            attributes = item.Attributes;
            methods = item.Methods;
            if(item.Stereo != 0)
            {
                typeSelected = true;
            }
            Source= source;
            var grid = source.Find<Grid>("panels_grid");
            List<StackPanel> tmp_panels = new();
            foreach (var ch in grid.GetLogicalChildren())
                if (ch is StackPanel sp) tmp_panels.Add(sp);
            panels = tmp_panels.ToArray();
            if (typeSelected)
            {
                panels[0].Children.Add(new TextBlock() { Text = type, Tag = "item", Classes = new Classes("type") });
            }
            panels[0].Children.Add(new TextBlock() { Text = name, Tag = "item", Classes = new Classes("name") });
            foreach (var tmp_item in attributes) panels[1].Children.Add(new TextBlock() { Text = tmp_item.ToString(), Tag = "item", Classes = new Classes("attribute")});
            foreach (var tmp_item in methods) panels[2].Children.Add(new TextBlock() { Text = tmp_item.ToString(), Tag = "item", Classes = new Classes("method") });
            FontSize();

        }
        public void Update(Element item)
        {
            Item = item;
            Name = item.GetName();
            type = item.GetStereo();
            attributes = item.Attributes;
            methods = item.Methods;
            if (item.Stereo != 0)
            {
                typeSelected = true;
            }
            panels[0].Children.Clear();
            panels[1].Children.Clear();
            panels[2].Children.Clear();
            if (typeSelected)
            {
                panels[0].Children.Add(new TextBlock() { Text = type, Tag = "item", Classes = new Classes("type") });
            }
            panels[0].Children.Add(new TextBlock() { Text = name, Tag = "item", Classes = new Classes("name") });
            foreach (var tmp_item in attributes) panels[1].Children.Add(new TextBlock() { Text = tmp_item.ToString(), Tag = "item", Classes = new Classes("attribute") });
            foreach (var tmp_item in methods) panels[2].Children.Add(new TextBlock() { Text = tmp_item.ToString(), Tag = "item", Classes = new Classes("method") });
            FontSize();
        }
        public void FontSize()
        {
            int font_size;
            int item_count = 1;
            if (typeSelected) item_count++;
            item_count += attributes.Count;
            item_count += methods.Count;
            font_size = (Height * 9 / 10) / item_count * 72 / 80;
            int max_length = name.Length;
            if (typeSelected && type.Length > max_length) max_length = type.Length;
            foreach (var item in attributes)
            {
                if(item.ToString().Length > max_length)max_length= item.ToString().Length;
            }
            foreach (var item in methods)
            {
                if (item.ToString().Length > max_length) max_length = item.ToString().Length;
            }
            if (((Width * 9 / 10) / max_length *2 ) < font_size) font_size = Width / max_length * 2;
            /*System.Diagnostics.Debug.WriteLine("Fontsize: \n", ((TextBlock)panels[0].Children[0]).FontSize.ToString());
            System.Diagnostics.Debug.WriteLine("Fontsize: \n", font_size.ToString());*/
            if(font_size > 32) font_size = 32;
            ((TextBlock)panels[0].Children[0]).FontSize = font_size;
            if (typeSelected) ((TextBlock)panels[0].Children[1]).FontSize = font_size;
            for (int i = 0; i < attributes.Count; i++) ((TextBlock)panels[1].Children[i]).FontSize = font_size;
            for (int i = 0; i < methods.Count; i++) ((TextBlock)panels[2].Children[i]).FontSize = font_size;
        }
        public void Resize(Point delta)
        {
            Width = (ushort)delta.X;
            Height = (ushort)delta.Y;
            FontSize();
        }
        public Element Item
        {
            get => item;
            set => this.RaiseAndSetIfChanged(ref item, value);
        }
        public ushort Width
        {
            get => width;
            set => this.RaiseAndSetIfChanged(ref width, value);
        }
        public ushort Height
        {
            get => height;
            set => this.RaiseAndSetIfChanged(ref height, value);
        }
        public DiagramItem Source
        {
            get => source;
            set => this.RaiseAndSetIfChanged(ref source, value);
        }
        public void Move(Point position)
        {
            Source.Margin = new(position.X,position.Y,0,0);
            this.position = position;
        }
        public Point Position
        {
            get => position;
            set => this.RaiseAndSetIfChanged(ref position, value);
        }

        public string Name { 
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value); 
        }/*
        public bool TypeSelected { 
            get => typeSelected;
            set => this.RaiseAndSetIfChanged(ref typeSelected, value);
        }
        public string Type { 
            get => type;
            set => this.RaiseAndSetIfChanged(ref type, value);
        }
        public ObservableCollection<Method> Methods { 
            get => methods;
            set => this.RaiseAndSetIfChanged(ref methods, value);
        }
        public ObservableCollection<Attribute> Attributes{ 
            get => attributes;
            set => this.RaiseAndSetIfChanged(ref attributes, value);
        }*/
    }
}
