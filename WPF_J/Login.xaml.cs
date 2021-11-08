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
using System.Windows.Shapes;
using TestPT;
using System.Net;
using System.Net.Sockets;
using Modbus.Device;
using System.Windows.Threading;

namespace TestPT
{
    /// <summary>
    /// login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        ModbusIpMaster Master;
        DispatcherTimer timer;
        MainWindowsModel model;


        public Login()
        {
            InitializeComponent();

            model = this.Resources["model"] as MainWindowsModel;
        }

        private void Timer()
        {
            timer = new DispatcherTimer(); // 타이머 설정
            timer.Interval = TimeSpan.FromTicks(100000); // 1/1000000 sec
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if ((string)BtnConnect.Content == "Connect") // Conect 버튼에 문자가 Connect일때 DisConnect로 변경
            {
                Master = ModbusIpMaster.CreateIp(new System.Net.Sockets.TcpClient(TxtIP.Text, 502)); // 서버 통신을 위한 소켓 생성  Convert.ToInt32(TxtPort.Text)
                //GrpConnect.Enabled = false;
                //GrpReadWrite.Visible = true;
                BtnConnect.Content = "DisConnect";
                Timer();

            }
            else // Connect 버튼에 문자가 Connect가 아닐때{(DisConnect)일때} Connect로 변경
            {
                Master = null;
                //GrpConnect.Enabled = true;
                //GrpReadWrite.Visible = false;
                BtnConnect.Content = "Connect";
            }
        }

        private ushort[] ReadHoldingRegisters(ushort registerAddress, ushort length) // 워드를 이용한 ReadHoldingRegisters 선언
        {
            try
            {
                return Master.ReadHoldingRegisters(1, registerAddress, length); // Convert.ToByte(TxtSlaveAddress.Text)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool[] ReadCoils(ushort registerAddress, ushort length) // bool 값을 이용한 ReadCoils 선언
        {
            try
            {
                return Master.ReadCoils(1, registerAddress, length); // Convert.ToByte(TxtSlaveAddress.Text)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteSingleCoil(ushort registerAddress, bool length) // bool 값을 이용한 WriteSingleCoil 선언
        {
            Master.WriteSingleCoil(1, registerAddress, length); // Convert.ToByte(TxtSlaveAddress.Text)
        }

        public void WriteSingleRegister(ushort registerAddress, ushort length) // 워드를 이용한 WriteSingleRegister 선언
        {
            try
            {
                Master.WriteSingleRegister(1, registerAddress, length); // Convert.ToByte(TxtSlaveAddress.Text)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            try // try 시도시 catch 나 try-catch-finllay로만 실행 종료 가능
            {
                var values = ReadHoldingRegisters(0, 16); // Values 설정 Convert.ToUInt16(TxtRegisterAddress.Text), Convert.ToUInt16(TxtRegisterCount.Text));
                                                          //var input = ReadInputRegisters(Convert.ToUInt16(TxtRegisterAddress.Text, 16), Convert.ToUInt16(TxtRegisterCount.Text)); // Input 값 표시
                                                          //TBVal1.Text = ToDecimal(values[0]); // 1000번 값
                TxtBox.Text = ToDecimal(values[0]);
                model.Values = (double)values[0];
                TxtBox1.Text = ToDecimal(values[1]);
                
                //TxtBox.Text = values[1]; // 1001번 값
                //TxtBox.Text = ToDecimal(values[2]); // 1002번 값
                //TBVal4.Text = ToDecimal(values[3]); // 1003번 값

                //bool[] coil = Master.ReadCoils(0,5);
                //TBB1.Text = coil[0].ToString();
                var coils = ReadCoils(0, 16); // ON/OFF 값 표시 Convert.ToUInt16(TxtCoilsAddress.Text), Convert.ToUInt16(TxtCoilsCount.Text));
                BoBox.Text = ToBoolean(coils[0]); // 1500번 값
                BoBox1.Text = ToBoolean(coils[1]); // 1501번 값
                BoBox2.Text = ToBoolean(coils[2]); // 1502번 값
                //TBB4.Text = ToBoolean(coils[3]); // 1503번 값
                //TBB5.Text = ToBoolean(coils[4]); // 1504번 값
                //TBB6.Text = ToBoolean(coils[5]); // 1505번 값

                //Master.WriteSingleRegister(1, Convert.ToUInt16(TxtBox.Text));// Convert.ToByte(TxtSendIP.Text), Convert.ToUInt16(TxtSendValue.Text)

            }
            catch (Exception ex) // 예외 설정
            {
                timer.Stop();
                BtnConnect_Click(this, new RoutedEventArgs());
                MessageBox.Show(ex.Message);
            }
        }

        //private double ToDouble(string text)
        //{
        //    throw new NotImplementedException();
        //}

        //private double ToDouble(string text)
        //{
        //    Double vale = Convert.ToDouble(text.ToDouble());
        //    return vale.ToDouble("0");
        //}

        private string ToDecimal(ushort value) // 
        {
            decimal val = Convert.ToDecimal(value.ToString());
            return val.ToString("0");
        }


        private string ToBoolean(bool coil)
        {
            //Boolean Bool = Convert.ToBoolean(coil.ToString());
            if (coil)
            {
                return "true"; // Coil이 맞으면(On이면) true
            }
            else
            {
                return "False"; // Coil이 아니면(OFF면) False
            }

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    bool ONF = new bool();
            //    if (BoBox.Text == "1")
            //    {
            //        ONF = true;
            //    }
            //    else
            //    {
            //        ONF = false;
            //    }
            //    Master.WriteSingleCoil(Convert.ToByte(BoBox.Text), ONF);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void Sendbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Switch.IsChecked == true)
                //{
                //    MessageBox.Show("true"); // Check 되어 있으면(On이면) true
                //}
                //else if
                //{
                //    MessageBox.Show("False"); // Check 되어 있지 않으면(OFF면) False
                //}

                Master.WriteSingleRegister(Convert.ToByte(0), Convert.ToUInt16(TxtBox.Text));

                //bool ONF = Switch.IsChecked == "1" ?  true : false; // 불 표현식 
                //Master.WriteSingleCoil(Convert.ToByte(0), Switch.IsChecked == true);
                //
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Switch_Checked(object sender, RoutedEventArgs e)
        {
            Master.WriteSingleCoil(Convert.ToByte(0), Switch.IsChecked == true);
            //if (Switch.IsChecked == true)
            //{
            //    Master.WriteSingleCoil(Convert.ToByte(0), Switch.IsChecked == true); // Check 되어 있으면(On이면) true
            //}
            //else
            //{
            //    Master.WriteSingleCoil(Convert.ToByte(0), Switch.IsChecked == false); // Check 되어 있지 않으면(OFF면) False
            //}

        }

        private void Switch_Unchecked(object sender, RoutedEventArgs e)
        {
            Master.WriteSingleCoil(Convert.ToByte(0), Switch.IsChecked != false);
        }
    }
}
