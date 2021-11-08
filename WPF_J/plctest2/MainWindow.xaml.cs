using Modbus.Device;
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
using System.Windows.Threading;

namespace TestPT
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowsModel model;
        DispatcherTimer timer;

        
        public MainWindow()
        {
            InitializeComponent();

            model = this.Resources["model"] as MainWindowsModel;
        }

        public void Timer()
        {
            timer = new DispatcherTimer(); // 타이머 설정
            timer.Interval = TimeSpan.FromTicks(10000000); // 1/10 sec
            timer.Tick += Timer_Ticks;
            timer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            model.Max = 100;
            model.Min = 0;

            model.Maximum = 100;
            model.Minimum = 0;

            Timer();
        }

        public void Timer_Ticks(object sender, EventArgs e)
        {
            Random ran_value = new Random();
            int temp = ran_value.Next(0, 1000);
            model.Value = temp * 0.1;


            Random ran_setting = new Random();
            int temp2 = ran_setting.Next(0, 1000);
            model.SettingValue = temp2 * 0.1;

            Random Tvalues = new Random();
            int temp3 = Tvalues.Next(0, 100);
            model.Values = temp3;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {

            TestPT.Login TestWindow = new TestPT.Login();

            TestWindow.Show(); // 모달리스: 해당 창으로 이동 가능
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
