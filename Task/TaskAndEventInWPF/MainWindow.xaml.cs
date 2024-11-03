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

namespace TaskAndEventInWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: MainWindow()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: Window_Activated()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
    }

    /*
     * NOTE:
     * 1. "await TestAsync()" directly in Button4_Click and "Task.Run(() => await TestAsync())" in Button3_Click is different 
     *    in SynchronizationContext.Current for TestAsync.
     * 2. WPF application has System.Windows.Threading.DispatcherSynchronizationContext as SynchronizationContext.Current 
     *    while console application has null for SynchronizationContext.Current. Compare the outputs of this and TaskAndEvent project.
     */
    private async Task TestAsync()
    {
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: + TestAsync()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
        await Task.Delay(3000);
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: - TestAsync()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
        return;
    }

    private async void Button3_Click(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: + Button3_Click()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
        await Task.Run(async () =>
        {
            Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: + Task.Run()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
            await TestAsync();
            Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: - Task.Run()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
        });
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: - Button3_Click()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
    }

    private async void Button4_Click(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: + Button4_Click()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
        await TestAsync();
        Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: - Button4_Click()\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
    }
}