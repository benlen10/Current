using System;
using System.Windows;
using System.Windows.Threading;

/// <summary>
/// Interaction logic for Notification.xaml
/// </summary>
public partial class NotificationWindow
{
    #region Constructors

    /// <summary>
    /// Public constructor for the NotificationWindow 
    /// </summary>
    /// <param name="titleText">Heading text for the nofication</param>
    /// <param name="bodyText"> Body text for the notification</param>
    public NotificationWindow(String titleText, String bodyText)
    {
        InitializeComponent();

        Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                textBlock11.Text = titleText;
                textBlock0.Text = bodyText;
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                this.Left = corner.X - this.ActualWidth - 100;
                this.Top = corner.Y - this.ActualHeight;
            }));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called once the notification animation is completed and closes the window
    /// </summary>
    private void DoubleAnimationCompleted(object sender, EventArgs e)
    {
        this.Close();
    }

    #endregion

}

