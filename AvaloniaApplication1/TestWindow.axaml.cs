using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Controls;
using System;
using System.Threading.Tasks;

namespace AvaloniaApplication1;

public partial class TestWindow : Window
{
    public TestWindow()
    {
        InitializeComponent();
    }

    private void btnRun_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        start();
    }

    async void start()
    {
        while(true)
        {
            await Task.Delay(20);
            txtTime.Text = DateTime.Now.ToString("hh:mm:ss.fff");
            txt2.Text = DateTime.Now.ToString("hh:mm:ss.fff");
            playPointer.Margin = new Thickness(playPointer.Margin.Left + 1, playPointer.Margin.Top, 0, 0);
            foreach ( TestControl ctrl in panel.Children )
            {
                ctrl.InvalidateVisual();
            }
        }
    }
}