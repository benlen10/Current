using System;
using System.Windows;
using System.Windows.Threading;


/// <summary>
/// Interaction logic for Notification.xaml
/// </summary>
public partial class NotificationWindow : Window
{
    public NotificationWindow(String title, String body)
    {
     

        InitializeComponent();

        Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                textBlock11.Text = title;
                textBlock0.Text = body;
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                this.Left = corner.X - this.ActualWidth - 100;
                this.Top = corner.Y - this.ActualHeight;
            }));
    }

    private void DoubleAnimationCompleted(object sender, EventArgs e)
    {

        this.Close();

    }
}

