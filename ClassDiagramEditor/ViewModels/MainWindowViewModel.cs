using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using ClassDiagramEditor.Models;
using ClassDiagramEditor.Views;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ClassDiagramEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        Canvas canvas = new Canvas();
        Mapper map;
        Window mainWindow;
        ObservableCollection<DiagramItemViewModel> models = new ObservableCollection<DiagramItemViewModel>();
        public MainWindowViewModel(Window mainWindow)
        {
            this.mainWindow = mainWindow;
            map = new Mapper(this);
            canvas = mainWindow.Find<Canvas>("canvas");
            mainWindow.PointerPressed += (object? sender, PointerPressedEventArgs e) =>
            {
                if (e.Source != null && e.Source is Control @control) map.Press(@control, e.GetCurrentPoint(canvas).Position);
            };
            mainWindow.PointerMoved += (object? sender, PointerEventArgs e) =>
            {
                if (e.Source != null && e.Source is Control @control) map.Move(@control, e.GetCurrentPoint(canvas).Position);
            };
            mainWindow.PointerReleased += (object? sender, PointerReleasedEventArgs e) =>
            {
                if (e.Source != null && e.Source is Control @control) map.Release(@control, e.GetCurrentPoint(canvas).Position);
            };
        }
        public ObservableCollection<DiagramItemViewModel> Models
        {
            get => models; 
            set => this.RaiseAndSetIfChanged(ref models, value);
        }
        public Window MainWindow
        {
            get => this.mainWindow;
        }
       
        public void NewEmptyElement(Point position)
        {
            Element emp = new Element();
            emp.Stereo = 1;
            emp.Access = 1;
            emp.Attributes.Add(new Attribute());
            emp.Attributes.Add(new Attribute());
            emp.Methods.Add(new Method());
            var control = new DiagramItem(emp);
            DiagramItemViewModel model = (DiagramItemViewModel) control.DataContext;
            model.Move(position - new Point(75, 75));
            canvas.Children.Add(control);
            Models.Add(model);
            map.AddItem(model);
        }
        

    }
}