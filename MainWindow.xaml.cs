using System;
using System.Windows;
using System.Windows.Threading;

namespace TimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private DateTime lastUpdateTime;
        private TimeSpan elapsedTime;
        private bool isRunning = false;
        private const double targetIntervalMs = 1; // 1 millisecond target
        //private const double refreshIntervalMs = 1000.0 / 60.0; // ~16.67ms for 60Hz
        //private const double refreshIntervalMs = 1000.0 / 120.0; // 120hz refresh rate 
        private const double refreshIntervalMs = 1000.0 / 165.0; // 165z refresh rate 

        public MainWindow()
        {
            InitializeComponent();

            // Initialize high-resolution timer
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(refreshIntervalMs);
            timer.Tick += Timer_Tick;

            elapsedTime = TimeSpan.Zero;
            lastUpdateTime = DateTime.Now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isRunning) return;

            DateTime now = DateTime.Now;
            TimeSpan delta = now - lastUpdateTime;
            lastUpdateTime = now;

            // Accumulate time with actual elapsed milliseconds
            elapsedTime = elapsedTime.Add(delta);

            // Update display (will be limited to screen refresh rate)
            timerDisplay.Text = elapsedTime.ToString(@"hh\:mm\:ss\.fff"); // Now shows milliseconds
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                lastUpdateTime = DateTime.Now;
                timer.Start();
                isRunning = true;
                startButton.IsEnabled = false;
                pauseButton.IsEnabled = true;
            }
        }

        /*private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
                startButton.IsEnabled = true;
                pauseButton.IsEnabled = false;
            }
            MessageBox.Show("Click on \"start\" button to resume.");
        }*/

        private void ShowStatusMessage(string message, double seconds = 2)
        {
            statusText.Text = message;
            statusText.Visibility = Visibility.Visible;

            var hideTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(seconds) };
            hideTimer.Tick += (s, args) =>
            {
                statusText.Visibility = Visibility.Collapsed;
                hideTimer.Stop();
            };
            hideTimer.Start();
        }

        // Then just call ShowStatusMessage() in your button handlers
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
                startButton.IsEnabled = true;
                pauseButton.IsEnabled = false;
                ShowStatusMessage("Timer paused. Click 'Start' to resume.", 3);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            elapsedTime = TimeSpan.Zero;
            timerDisplay.Text = "00:00:00.000"; // Updated to show milliseconds
            isRunning = false;
            startButton.IsEnabled = true;
            pauseButton.IsEnabled = false;
        }
    }
}