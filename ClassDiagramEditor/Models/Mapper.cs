using Avalonia;
using Avalonia.Controls;
using ClassDiagramEditor.Views;
using ClassDiagramEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Avalonia.LogicalTree;
using System.Xml.Linq;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Data;
using Avalonia.Media;
using System.Drawing.Printing;
using Avalonia.VisualTree;
using DynamicData.Kernel;

namespace ClassDiagramEditor.Models
{
    public class Mapper
    {
        readonly List<DiagramItemViewModel> models = new();
        readonly MainWindowViewModel main_model;
        Point camera_pos;
        Point old_position;
        int mode = 0;
        int click_counter = 0;
        bool is_pressed = false;
        int selected_index = -1;
        int move_selected_index = -1;
        Ellipse point = new Ellipse() { Tag= "point", Width = 10, Height = 10, Fill = Brush.Parse("AliceBlue"), Stroke = Brush.Parse("Blue"), StrokeThickness = 1, Margin = new(0,0,0,0), ZIndex = 2 ,IsVisible = false};
        ArrowConstructor arrow = new ArrowConstructor();
        List<ArrowConstructor> arrows = new List<ArrowConstructor>();
        public Mapper(MainWindowViewModel model)
        {
            main_model = model;
            main_model.MainWindow.Find<Canvas>("canvas").Children.Add(point);
            main_model.MainWindow.Find<Canvas>("canvas").Children.Add(arrow.Arrow);

        }
        public void AddItem(DiagramItemViewModel model)
        {
            models.Add(model);
        }
        public void AddItem(Element item, Point position)
        {
            var control = new DiagramItem(item);
            DiagramItemViewModel model = new DiagramItemViewModel();
            if (control.DataContext is not null) model = (DiagramItemViewModel)control.DataContext;
            model.Move(position - new Point(75, 75));
            var canvas = main_model.MainWindow.Find<Canvas>("canvas");
            canvas.Children.Add(control);
            models.Add(model);
        }
        public void Update(DiagramItemViewModel model,Element item)
        {
            selected_index = models.IndexOf(model);
            models[selected_index].Update(item);
        }
        private void RemoveItem(DiagramItemViewModel item)
        {
            models.Remove(item);
            if (item.Source.Parent is null) return;
            var canvas = (Canvas)(item.Source.Parent);
            canvas.Children.Remove(item.Source);
        }
        public bool ItemChecker(Control item)
        {
            var c = (string?)item.Tag;
            mode = c switch
            {
                "point" => 5,
                "arrow" => 6,
                _ => 0,
            };
            if (mode != 0) return true;
            while (item.Parent != null)
            {
                
                if (item is DiagramItem && item.DataContext is not null)
                {
                    DiagramItemViewModel tmp = (DiagramItemViewModel)item.DataContext;
                    if (models.IndexOf(tmp) != -1)
                    {
                        
                        mode = c switch
                        {
                            "item" => 1,
                            "resize" => 2,
                            "delete" => 3,
                            "border" => 4,
                            _ => 0,
                        };
                        selected_index = models.IndexOf(tmp);
                        return true;
                    }
                    
                }
                item = (Control)item.Parent;
            }
            selected_index = -1;
            return false;
        }
        public bool ItemChecker(Control item, int check)
        {
            /*if(item is Canvas)
            {
                System.Diagnostics.Debug.WriteLine("Canvas color \n", ((Canvas)item).Background.ToString());
            }*/
            var c = (string?)item.Tag;
            int _mode = c switch
            {
                "point" => 5,
                "arrow" => 6,
                _ => 0,
            };
            if (_mode != 0) return true;

            while (item.Parent != null)
            {
                if (item is DiagramItem && item.DataContext is not null)
                {
                    DiagramItemViewModel tmp = (DiagramItemViewModel)item.DataContext;
                    if (models.IndexOf(tmp) != -1)
                    {

                        _mode = c switch
                        {
                            "item" => 1,
                            "resize" => 2,
                            "delete" => 3,
                            "border" => 4,
                            _ => 0,
                        };
                        move_selected_index = models.IndexOf(tmp);
                        if(_mode == check) return true;
                    }
                }
                item = (Control)item.Parent;
            }
            move_selected_index = -1;
            return false;
        }
        public void Press(Control item, Point position)
        {
            is_pressed = true;
            old_position= position;
            ItemChecker(item);
            if(mode == 5 || mode == 4)
            {
                arrow.StartPoint = position;
                System.Diagnostics.Debug.WriteLine("Start Arrow \n", position.ToString());

            }
        }

        public void Move(Control item, Point position)
        {
            if (Math.Abs(position.X - old_position.X) > 1)
            {
                click_counter = 0;
            }
            
            camera_pos = position;
            if (is_pressed && mode == 1)
            {
                models[selected_index].Move(camera_pos);
            }
            if (is_pressed && mode == 2)
            {

                Point delta  = camera_pos - models[selected_index].Position;
                if(delta.X < 100) delta= new Point(100,delta.Y);
                if (delta.Y < 100) delta = new Point(delta.X, 100);
                models[selected_index].Resize(delta);
            }
            if(ItemChecker(item,4))
            {
                var parent = point.GetVisualParent<Canvas>();
                parent.Children.Remove(point);
                System.Diagnostics.Debug.WriteLine("Border \n", position.ToString());
                point.IsVisible= true;
                point.Margin = new(position.X-5, position.Y-5,0,0);
                parent.Children.Add(point);
            }
            else
            {
                point.IsVisible = false;
            }
            if(is_pressed && mode == 5)
            {
                var parent = arrow.Arrow.GetVisualParent<Canvas>();
                parent.Children.Remove(arrow.Arrow);
                System.Diagnostics.Debug.WriteLine("Arrow \n", position.ToString());
                arrow.Arrow.IsVisible = true;
                arrow.EndPoint = position - new Point(3,3);
                arrow.Update();
                parent.Children.Add(arrow.Arrow);
            }
            else
            {
                arrow.Arrow.IsVisible = false;
            }
        }
        public void Release(Control item, Point position)
        {
            click_counter++;
            is_pressed = false;
            if (click_counter == 2 && mode == 0)
            {
                AddElement window= new AddElement(this, position);
                window.ShowDialog(main_model.MainWindow);
                System.Diagnostics.Debug.WriteLine("New \n", position.ToString());
                
            }
            if (click_counter == 2 && mode == 1)
            {
                AddElement window = new AddElement(models[selected_index], this, position);
                window.ShowDialog(main_model.MainWindow);
                System.Diagnostics.Debug.WriteLine("New \n", position.ToString());

            }
            if (mode == 3 && selected_index >=0)
            {
                RemoveItem(models[selected_index]);
                System.Diagnostics.Debug.WriteLine("Delete \n", position.ToString());
            }
            if(mode == 5 && ItemChecker(item, 5) && move_selected_index != selected_index)
            {
                ArrowConstructor new_arrow = new ArrowConstructor();
                new_arrow.StartPoint = arrow.StartPoint;
                new_arrow.EndPoint = arrow.EndPoint;
                new_arrow.Type= arrow.Type;
                new_arrow.Arrow.IsVisible = true;
                new_arrow.Update();
                arrows.Add(new_arrow);
                var parent = arrow.Arrow.GetVisualParent<Canvas>();
                parent.Children.Add(new_arrow.Arrow);
            }

            selected_index = -1;
            mode = 0;

        }


    }

}
