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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Modbus.Device;
using System.Windows.Threading;
using System.Windows.Media;

namespace GS_WPF_App
{
    public partial class MainWindow : Window
    {
        ModbusIpMaster master;

        DispatcherTimer readTimer = new DispatcherTimer();

        int[] timer = new int[4];
        int[] maxProduction = new int[4];

        
        public MainWindow()
        {
            InitializeComponent();

            DisConnect.Height = 553;
            for (int i = 0; i < timer.Length; i++) timer[i] = 0;

            SolidColorBrush redColor = new SolidColorBrush();
            redColor.Color = Color.FromRgb(0, 0, 0);
        }
        
        // IP 접속할 때 발생하는
        private void Button_Click_Connect(object sender, RoutedEventArgs e)
        {
            if (master == null)
            {
                master = ModbusIpMaster.CreateIp(new System.Net.Sockets.TcpClient(tbIPAddress.Text, Convert.ToInt32("502")));

                readTimer.Interval = TimeSpan.FromSeconds(1);
                readTimer.Tick += new EventHandler(readTimer_Tick);
                readTimer.Start();

                btConnect.Content = "DisConnect";
                DisConnect.Height = 0;
            }

            else
            {
                readTimer.Stop();
                master = null;

                btConnect.Content = "Connect";
                DisConnect.Height = 553;
            }
        }

        // word 값을 읽어내는 
        private ushort[] readHoldingRegisters(ushort registerAddress, ushort length)
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

        // Bool 값을 읽어내는 
        private bool[] readCoils(ushort CoilAddress, ushort length)
        {
            try
            {
                return master.ReadCoils(CoilAddress, length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // word 값을 int로 변환하는
        private int ToDecimal(ushort value)
        {
            decimal val = Convert.ToDecimal(value.ToString());
            return (int)val;
        }

        // 1초마다 이벤트 발생 - 데이터 읽어오는
        private void readTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var statusArray = readCoils(Convert.ToUInt16("0", 16), Convert.ToUInt16("50"));
                var productArray = readHoldingRegisters(Convert.ToUInt16("0", 16), Convert.ToUInt16("50"));

                int[] productIntValue = new int[4] { ToDecimal(productArray[2]), ToDecimal(productArray[12]), ToDecimal(productArray[22]), ToDecimal(productArray[32]) };

                tbProduction1000.Text = productIntValue[0].ToString();
                tbProduction1001.Text = productIntValue[1].ToString();
                tbProduction1002.Text = productIntValue[2].ToString();
                tbProduction1003.Text = productIntValue[3].ToString();
                 
                // 생산률(%) = (현재생산량 * 100) / 목표생산량  단,목표생산량이 0일경우 생산량은 100임
                tbPercentage1000.Text = maxProduction[0] == 0 ? "100" : (productIntValue[0] * 100 / maxProduction[0]).ToString();
                tbPercentage1001.Text = maxProduction[1] == 0 ? "100" : (productIntValue[1] * 100 / maxProduction[1]).ToString();
                tbPercentage1002.Text = maxProduction[2] == 0 ? "100" : (productIntValue[2] * 100 / maxProduction[2]).ToString();
                tbPercentage1003.Text = maxProduction[3] == 0 ? "100" : (productIntValue[3] * 100 / maxProduction[3]).ToString();

                if (statusArray[2])
                {
                    timer[0]++;
                    tbTime1000.Text = timer[0].ToString();
                }
               //else Ellipse1000.Fill = so;
 
                if (statusArray[12])
                {
                    timer[1]++;
                    tbTime1001.Text = timer[1].ToString();
                }
 //               else Ellipse1001.Fill = Brushes.Black;

                if (statusArray[22])
                {
                    timer[2]++;
                    tbTime1002.Text = timer[2].ToString();
                }
 //               else Ellipse1002.Fill = Brushes.Black;

                if (statusArray[23])
                {
                    timer[3]++;
                    tbTime1003.Text = timer[3].ToString();
                }
 //               else Ellipse1003.Fill = Brushes.Black;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            tbSendIP.Text = "1";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tbSendIP.Text = "2";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tbSendIP.Text = "3";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            tbSendIP.Text = "4";
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            master.WriteSingleCoil(Convert.ToByte("0"), true);
            master.WriteSingleRegister(Convert.ToByte("0"), Convert.ToUInt16(tbSendIP.Text));
            master.WriteSingleRegister(Convert.ToByte("1"), Convert.ToUInt16(tbSendValue.Text));

            int temp = (int)Convert.ToDecimal(tbSendIP.Text) - 1;
            maxProduction[temp] = (int)Convert.ToDecimal(tbSendValue.Text);

            switch (temp)
            {
                case 0: tbTargetAmount1000.Text = tbSendValue.Text; Ellipse1000.Fill = Brushes.Blue; break;
                case 1: tbTargetAmount1001.Text = tbSendValue.Text; Ellipse1001.Fill = Brushes.Blue; break;
                case 2: tbTargetAmount1002.Text = tbSendValue.Text; Ellipse1002.Fill = Brushes.Blue; break;
                case 3: tbTargetAmount1003.Text = tbSendValue.Text; Ellipse1003.Fill = Brushes.Blue; break;

                default: break;
            }
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            master.WriteSingleCoil(Convert.ToByte("1"), false);
        }
    }
}
