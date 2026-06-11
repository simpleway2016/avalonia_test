using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        start();
    }

    private void btnRun_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       
    }

    async void start()
    {
        while(true)
        {
            await Task.Delay(1);
            txt2.Text =$"FPS:{((TestControl)panel.Children[0]).Fps} {DateTime.Now.ToString("hh:mm:ss.fff")}"; 
        
            if(playPointer.Margin.Left > 400)
            {
                playPointer.Margin = new Thickness(290, playPointer.Margin.Top, 0, 0);
            }
            else
            {
                playPointer.Margin = new Thickness(playPointer.Margin.Left + 1, playPointer.Margin.Top, 0, 0);
            }
            foreach ( TestControl ctrl in panel.Children )
            {
                ctrl.InvalidateVisual();
            }
        }
    }
}