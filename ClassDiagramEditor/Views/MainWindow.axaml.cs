using Avalonia.Controls;
using ClassDiagramEditor.ViewModels;

namespace ClassDiagramEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

    }
}