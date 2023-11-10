# define DEBUG

using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventAndThreadInWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Debug.WriteLine($"MainWindow() on thread {Thread.CurrentThread.ManagedThreadId}");
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        Debug.WriteLine($"Window_Activated() on thread {Thread.CurrentThread.ManagedThreadId}");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine($"Button_Click() on thread {Thread.CurrentThread.ManagedThreadId}");
    }

    private void Button2_Click(object sender, RoutedEventArgs e)
    {
        //NOTE: Note the order of outputs of the following invoke calls
        this.Dispatcher.Invoke(() =>
        {
            Debug.WriteLine($"Dispatcher.Invoke() on thread {Thread.CurrentThread.ManagedThreadId}");
        });

        //NOTE: Though async, this call will still block the UI thread for 3 second,
        //since the Dispatcher sends the action to the UI thread to process.
        this.Dispatcher.InvokeAsync(() =>
        {
            Thread.Sleep(3000);
            Debug.WriteLine($"Dispatcher.InvokeAsync() on thread {Thread.CurrentThread.ManagedThreadId}");
        });

        this.Dispatcher.Invoke(async ()  =>
        {
            await Task.Delay(3000);
            Debug.WriteLine($"Dispatcher.Invoke(async action) on thread {Thread.CurrentThread.ManagedThreadId}");
        });
    }

    private async void Button3_Click(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine($"Start Button3_Click() on thread {Thread.CurrentThread.ManagedThreadId}");
        await Task.Run(async () =>
        {
            await Task.Delay(5000);
            Debug.WriteLine($"Task.Run() on thread {Thread.CurrentThread.ManagedThreadId}");
        });
        Debug.WriteLine($"End Button3_Click() on thread {Thread.CurrentThread.ManagedThreadId}");
    }

    private async Task TestAsync()
    {
        Debug.WriteLine($"Start TestAsync() on thread {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(5000);
        Debug.WriteLine($"End TestAsync() on thread {Thread.CurrentThread.ManagedThreadId}");
        return;
    }

    private async void Button4_Click(object sender, RoutedEventArgs e)
    {
        await TestAsync();
    }
}