using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Rendering;
using Avalonia.Styling;
using AvaloniaApplication1.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel _model;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            _model = this.DataContext as MainWindowViewModel;
            base.OnLoaded(e);
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            start();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            var point = e.GetPosition(this);
            foreach (Control ctrl in grid1.Children)
            {
                ctrl.RenderTransform = new TranslateTransform(point.X , 0);
                //ctrl.Margin = new Thickness(point.X, ctrl.Margin.Top, ctrl.Margin.Right, ctrl.Margin.Bottom);
                break;
            }
        }

        async void start()
        {
            while (true)
            {
                await Task.Delay(20);
                foreach (Control ctrl in grid1.Children)
                {
                    ctrl.InvalidateVisual();
                    ctrl.Margin = new Thickness(ctrl.Margin.Left + 1, ctrl.Margin.Top, ctrl.Margin.Right, ctrl.Margin.Bottom);
                }
                _model.TimeString = DateTime.Now.ToString("hh:mm:ss.fff");
            }
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var res = Application.Current.FindResource(Avalonia.Styling.ThemeVariant.Dark, textBox1.Text);

            if(res is SolidColorBrush brush)
            {
                var color = brush.Color;
               textBox2.Text = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            else if (res is Color color)
            {
                textBox2.Text = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            else
            {
                textBox2.Text = res.GetType().FullName;
            }
        }
    }
}