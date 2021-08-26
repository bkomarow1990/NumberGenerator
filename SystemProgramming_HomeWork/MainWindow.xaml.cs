using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemProgramming_HomeWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;
        Thread thread2;
        private EventWaitHandle wh = new ManualResetEvent(true);
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool fibonachiState = false;
        private bool simpleState = false;
        private bool simpleStatePaused = false;
        private bool fibonachiStatePaused = false;
        private void GenerateNumbers(object obj)
        {
            long[] nums = obj as long[];
            long tmp = 0;
            long[] nums_for_generate = { nums[0] - 1, nums[0] };
            Dispatcher.Invoke(() =>
            {
                numsListBox.Items.Add(nums[0] - 1);
                numsListBox.Items.Add(nums[0]);
            });
            while (nums_for_generate[1] < nums[1])
            {
                Dispatcher.Invoke(() =>
                    numsListBox.Items.Add(nums_for_generate[0] + nums_for_generate[1])
                );
                tmp = nums_for_generate[0];
                nums_for_generate[0] = nums_for_generate[1];
                nums_for_generate[1] += tmp;
            }
        }
        private void GenerateSimpleNumbers(object obj)
        {
            int[] nums = obj as int[];
            for (int i = nums[0]; i < nums[1]; i++)
            {
                wh.WaitOne();
                Dispatcher.Invoke(() =>

                    numsListBox2.Items.Add(i)
                );
            }
        }
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread.Yield();
            fibonachiState = !fibonachiState;
            if (!fibonachiState)
            {
                this.startBtn.Content = "Start Fibonachi";
                thread2.Abort();
                return;
            }
            //while (fibonachiStatePaused)
            //{
            //    Thread.Sleep(1000);
            //    return;
            //}
            this.startBtn.Content = "Stop Fibonachi";
            long left, right;
            if (String.IsNullOrWhiteSpace(this.leftNumTxtBox.Text))
            {
                left = 2;
            }
            else
            {
                left = Int64.Parse(this.leftNumTxtBox.Text);
            }
            if (String.IsNullOrWhiteSpace(this.rightNumTxtBox.Text))
            {
                right = 10000;
            }
            else
            {
                right = Int64.Parse(this.rightNumTxtBox.Text);
            }
            numsListBox.Items.Clear();
            ParameterizedThreadStart start = new ParameterizedThreadStart(GenerateNumbers);
            thread = new Thread(start);
            long[] nums = { left, right };
            thread.Start(nums);
        }

        private void startBtn2_Click(object sender, RoutedEventArgs e)
        {
            simpleState = !simpleState;
            if (!simpleState)
            {
                this.startBtn2.Content = "Start Simple";
                thread2.Abort();
                return;
            }

            this.startBtn2.Content = "Stop Simple";
            int left, right;
            if (String.IsNullOrWhiteSpace(this.leftNumTxtBox2.Text))
            {
                //right = 33;
                left = 2;
            }
            else
            {
                left = Int32.Parse(this.leftNumTxtBox2.Text);
            }
            if (String.IsNullOrWhiteSpace(this.rightNumTxtBox2.Text))
            {
                right = 10000;
            }
            else
            {
                right = Int32.Parse(this.rightNumTxtBox2.Text);
            }
            numsListBox2.Items.Clear();
            ParameterizedThreadStart start = new ParameterizedThreadStart(GenerateSimpleNumbers);
            thread2 = new Thread(start);
            int[] nums = { left, right };
            thread2.Start(nums);
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (thread == null)
            {
                return;
            }
            fibonachiStatePaused = !fibonachiStatePaused;
            if (fibonachiStatePaused)
            {
                wh.Reset();
                return;
            }
            else
            {
                wh.Set();
            }
        }

        private void pauseBtn2_Click(object sender, RoutedEventArgs e)
        {
            //ManualResetEvent run = new ManualResetEvent(false);
            if (thread2 == null)
            {
                return;
            }
            simpleStatePaused = !simpleStatePaused;
            if (simpleStatePaused)
            {
                wh.Reset();
                return;
            }
            else {
                wh.Set();
            }

           
            //if (thread2 == null)
            //{
            //    return;
            //}
            //fibonachiStatePaused = !fibonachiStatePaused;
            //if (fibonachiStatePaused)
            //{
            //    thread2.Resume();
            //    return;
            //}
            //thread2.Suspend();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wh.Set();
        }
    }
}
