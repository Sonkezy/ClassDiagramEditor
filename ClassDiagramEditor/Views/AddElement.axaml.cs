using Avalonia;
using Avalonia.Controls;
using ClassDiagramEditor.Models;
using ClassDiagramEditor.ViewModels;

namespace ClassDiagramEditor.Views
{
    public partial class AddElement : Window
    {
        public AddElement()
        {
            InitializeComponent();
            DataContext= this;
        }
        public AddElement(Mapper source, Point position)
        {
            InitializeComponent();
            DataContext = new AddElementViewModel(source, position, this);
        }
        public AddElement(DiagramItemViewModel model, Mapper source, Point position)
        {
            InitializeComponent();
            DataContext = new AddElementViewModel(source, model, position, this);
        }
    }
}
