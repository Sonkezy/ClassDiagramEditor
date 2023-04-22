using Avalonia.Controls;
using ClassDiagramEditor.Models;
using ClassDiagramEditor.ViewModels;

namespace ClassDiagramEditor.Views
{
    public partial class DiagramItem : UserControl
    {
        public DiagramItem() : this(new Element()) { }
        public DiagramItem(Element emp)
        {
            InitializeComponent();
            DataContext = new DiagramItemViewModel(this, emp);
        }
    }
}
