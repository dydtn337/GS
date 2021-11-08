using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TestPT
{
    class MainWindowsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        
        private void OnPropertyChanged(string info)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private string _gaugeTitle;
        private string _gaugeUnit;
        private double _max;
        private double _min;
        private double _value;
        private double _settingValue;
        private int _tankProperty;
        private double _maximum;
        private double _minimum;
        private double _values;
        public string GaugeTitle
        {
            get { return _gaugeTitle; }
            set
            {
                _gaugeTitle = value;
                OnPropertyChanged("GaugeTitle");
            }
        }

        public string GaugeUnit
        {
            get { return _gaugeUnit; }
            set
            {
                _gaugeUnit = value;
                OnPropertyChanged("GaugeUnit");
            }
        }

        public double Max
        {
            get { return _max; }
            set
            {
                _max = value;
                OnPropertyChanged("Max");
            }
        }

        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                OnPropertyChanged("Min");
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public double SettingValue
        {
            get { return _settingValue; }
            set
            {
                _settingValue = value;
                OnPropertyChanged("SettingValue");
            }
        }

        public int TankProperty
        {
            get { return _tankProperty; }
            set
            {
                _tankProperty = value;
                OnPropertyChanged("TankProperty");
            }
        }

        public double Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                OnPropertyChanged("Maximum");
            }
        }

        public double Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                OnPropertyChanged("Minimum");
            }
        }

        public double Values
        {
            get { return _values; }
            set
            {
                _values = value;
                OnPropertyChanged("Values");
            }
        }

        public bool? IsChecked
        {
            get; set;
        }

        public void NotifyPropertyChanged(String propertyName = "")
        {
            if(PropertyChanged != null)
            { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
