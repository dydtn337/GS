using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Modbus.Device;
namespace WpfHi
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    
    public partial class MainWindow : Window
    {
        int address;
        ModbusIpMaster master;
        Queue<Rectangle> queData = new Queue<Rectangle>();
        Queue<Rectangle> playData = new Queue<Rectangle>();
        System.Windows.Threading.DispatcherTimer timerMove;


       

        public MainWindow()
        {
            InitializeComponent();
            
            master = ModbusIpMaster.CreateIp(new System.Net.Sockets.TcpClient("192.168.0.22", Convert.ToInt32("502")));

            for(int i = 0; i <20; i++)
            {
                ///" Fill="Blue" Height="13" Canvas.Left="700" Stroke="Black" Canvas.Top="346" Width="13"/>
                Rectangle rectangle1 = new Rectangle
                {
                    Height = 13,
                    Width = 13,
                    Fill = Brushes.Blue,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                myCanvas.Children.Add(rectangle1);
                EnQueData(rectangle1);
            }

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(10000000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            timerMove = new System.Windows.Threading.DispatcherTimer();
            timerMove.Interval = TimeSpan.FromSeconds(0.02);
            timerMove.Tick += new EventHandler(timerMove_Tick);
            timerMove.Start();
            
        }

        private void EnQueData(Rectangle r)
        {
            Canvas.SetLeft(r, 700);
            Canvas.SetTop(r, 346);
            queData.Enqueue(r);
        }

        private void DeQueData()
        {
            playData.Enqueue(queData.Dequeue());
            if(playData.Count <= 1) timerMove.Start();
        }

        private void timerMove_Tick(object sender, EventArgs e)
        {
            if (queData.Count == 10) timerMove.Stop();
            
            for(int i = 0; i < playData.Count; i++)
            {
                Rectangle temp = playData.Dequeue();

                if (Canvas.GetLeft(temp) > Canvas.GetLeft(DataPos))
                {
                    Canvas.SetLeft(temp, Canvas.GetLeft(temp) - 10f);
                    playData.Enqueue(temp);
                }
                else if (Canvas.GetTop(temp) > Canvas.GetTop(DataPos3))
                {
                    Canvas.SetTop(temp, Canvas.GetTop(temp) - 10f);
                    playData.Enqueue(temp);
                }
                else if (Canvas.GetLeft(temp) > Canvas.GetLeft(DataPos1003))
                {
                    Canvas.SetLeft(temp, Canvas.GetLeft(temp) - 10f);
                    playData.Enqueue(temp);
                }
                else EnQueData(temp);
            }

        }

        private void timer_Tick(object sender,EventArgs e)
        {
            try
            {
                var value_Register = ReadHoldingRegisters(Convert.ToUInt16("0", 16), Convert.ToUInt16("4"));

                tbM1000.Text = ToDecimal(value_Register[0]);
                tbM1001.Text = ToDecimal(value_Register[1]);
                tbM1002.Text = ToDecimal(value_Register[2]);
                tbM1003.Text = ToDecimal(value_Register[3]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ushort[] ReadHoldingRegisters(ushort registerAddress, ushort length)
        {
            try
            {
                return master.ReadHoldingRegisters(Convert.ToByte("1"), registerAddress, length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ToDecimal(ushort value)
        {
            decimal val = Convert.ToDecimal(value.ToString());
            return (val).ToString();
        }



        private void SendValue(int value)
        {
            if(tbSendValue.Text == "0")
            {
                tbSendValue.Text = value.ToString();
            }
            else
            {
                tbSendValue.Text += value.ToString();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendValue(1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SendValue(2);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SendValue(3);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SendValue(4);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SendValue(5);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SendValue(6);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SendValue(7);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            SendValue(8);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            SendValue(9);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            SendValue(0);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            tbSendValue.Text = "0";
        }

        private void Button_Click_M1000(object sender, RoutedEventArgs e)
        {
            SendDataIP.Content = "M1000 데이터 수정";
            tbSendValue.Text = "0";
            address = 1000;
        }
        private void Button_Click_M1001(object sender, RoutedEventArgs e)
        {
            SendDataIP.Content = "M1001 데이터 수정";
            tbSendValue.Text = "0";
            address = 1001;
        }
        private void Button_Click_M1002(object sender, RoutedEventArgs e)
        {
            SendDataIP.Content = "M1002 데이터 수정";
            tbSendValue.Text = "0";
            address = 1002;
        }
        private void Button_Click_M1003(object sender, RoutedEventArgs e)
        {
            SendDataIP.Content = "M1003 데이터 수정";
            tbSendValue.Text = "0";
            address = 1003;
        }

        private void Button_Click_Send (object sender, RoutedEventArgs e)
        {
            try
            {
                DeQueData();
                master.WriteSingleRegister(Convert.ToByte((address - 1000).ToString()), Convert.ToUInt16(tbSendValue.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}