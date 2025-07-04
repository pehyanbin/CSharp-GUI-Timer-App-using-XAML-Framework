using System;
using System.Windows;
using System.Windows.Threading;

namespace TimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan elapsedTime;
        private bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;

            elapsedTime = TimeSpan.Zero;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromMilliseconds(10));
            timerDisplay.Text = elapsedTime.ToString(@"hh\:mm\:ss\.ff");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                timer.Start();
                isRunning = true;
                startButton.IsEnabled = false;
                pauseButton.IsEnabled = true;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
                startButton.IsEnabled = true;
                pauseButton.IsEnabled = false;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            elapsedTime = TimeSpan.Zero;
            timerDisplay.Text = "00:00:00.00";
            isRunning = false;
            startButton.IsEnabled = true;
            pauseButton.IsEnabled = false;
        }
    }
}