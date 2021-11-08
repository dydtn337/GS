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

namespace OneFormWPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        ModbusIpMaster master;

        System.Windows.Threading.DispatcherTimer gaugeTimer = new System.Windows.Threading.DispatcherTimer();

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        

        public MainWindow()
        {
            InitializeComponent(); 
            
            gaugeTimer.Interval = TimeSpan.FromSeconds(0.03f);
            gaugeTimer.Tick += new EventHandler(GaugeTimer_Tick);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                
                if(gauge.Value > 0)
                    gauge.Value = 60f;
                else gauge.Value = 60f; gaugeTimer.Start(); 

                var value_Register = ReadHoldingRegisters(Convert.ToUInt16("0", 16), Convert.ToUInt16("4"));

                imgTank0.Opacity = ToDecimal(value_Register[0]) / 100f;
                tbM1000.Text = (imgTank0.Opacity * 100f).ToString();
                imgTank1.Opacity = ToDecimal(value_Register[1]) / 100f;
                tbM1001.Text = (imgTank1.Opacity * 100f).ToString();
                imgTank2.Opacity = ToDecimal(value_Register[2]) / 100f;
                tbM1002.Text = (imgTank2.Opacity * 100f).ToString();
                imgTank3.Opacity = ToDecimal(value_Register[3]) / 100f;
                tbM1003.Text = (imgTank3.Opacity * 100f).ToString();

                bool[] value_Bool = ReadCoils(Convert.ToUInt16("0", 16), Convert.ToUInt16("6"));

                tbButtonM1500.Text = value_Bool[0] == false ? "OFF" : "ON" ;
                Button0.Fill = value_Bool[0] == false ? Brushes.WhiteSmoke : Brushes.Red;
                tbButtonM1501.Text = value_Bool[1] == false ? "OFF" : "ON";
                Button1.Fill = value_Bool[1] == false ? Brushes.WhiteSmoke : Brushes.Red;
                tbButtonM1502.Text = value_Bool[2] == false ? "OFF" : "ON";
                Button2.Fill = value_Bool[2] == false ? Brushes.WhiteSmoke : Brushes.Red;
                tbButtonM1503.Text = value_Bool[3] == false ? "OFF" : "ON";
                Button3.Fill = value_Bool[3] == false ? Brushes.WhiteSmoke : Brushes.Red;
                tbButtonM1504.Text = value_Bool[4] == false ? "OFF" : "ON";
                Button4.Fill = value_Bool[4] == false ? Brushes.WhiteSmoke : Brushes.Red;
                tbButtonM1505.Text = value_Bool[5] == false ? "OFF" : "ON";
                Button5.Fill = value_Bool[5] == false ? Brushes.WhiteSmoke : Brushes.Red;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GaugeTimer_Tick(object sender, EventArgs e)
        {
            if (gauge.Value > 30) gauge.Value -= 1f;
            else gaugeTimer.Stop();
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
        private bool[] ReadCoils(ushort CoilAddress, ushort length)
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

        private float ToDecimal(ushort value)
        {
            decimal val = Convert.ToDecimal(value.ToString());
            return (float)val;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(master == null)
            {
                master = ModbusIpMaster.CreateIp(new System.Net.Sockets.TcpClient(tbIPAddress.Text, Convert.ToInt32("502")));

                timer.Start();
            }
            else
            {
                timer.Stop();
                master = null;
                gauge.Value = 0;
            }
        }

        private void Button0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(master != null)
            {
                bool bl = tbButtonM1500.Text == "OFF" ? true : false;
                SendBoolData(0, bl);
            }
            
        }

        private void Button1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (master != null)
            {
                bool bl = tbButtonM1501.Text == "OFF" ? true : false;
                SendBoolData(1, bl);
            }
        }

        private void Button2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (master != null)
            {
                bool bl = tbButtonM1502.Text == "OFF" ? true : false;
                SendBoolData(2, bl);
            }
        }

        private void Button3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (master != null)
            {
                bool bl = tbButtonM1503.Text == "OFF" ? true : false;
                SendBoolData(3, bl);
            }
        }

        private void Button4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (master != null)
            {
                bool bl = tbButtonM1504.Text == "OFF" ? true : false;
                SendBoolData(4, bl);
            } 
        }

        private void Button5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (master != null)
            {
                bool bl = tbButtonM1505.Text == "OFF" ? true : false;
                SendBoolData(5, bl);
            } 
        }

        private void SendBoolData(int Address, bool bl)
        {
            if (master != null) master.WriteSingleCoil(Convert.ToByte(Address.ToString()), bl);

        }
    }
}
