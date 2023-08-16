using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.ComponentModel;
using System.Text;

namespace DED_MonitoringSensor.Views.ThirdTabView
{
    class ThirdTabViewModel : INotifyPropertyChanged
    {
        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public GetDataService getDataService;
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand Send1Command { get; set; }
        public RelayCommand Send2Command { get; set; }

        private StringBuilder stringBuilder;

        private string text;
        public string Text
        {
            get { return text; }
            set
            { 
                    text = value;
                    OnPropertyChanged(nameof(Text));
            }
        }
        private string sentText;
        public string SentText
        {
            get { return sentText; }
            set
            {
                sentText = value;
                OnPropertyChanged(nameof(SentText));
            }
        }
        private string textbox1;
        public string TextBox1
        {
            get { return textbox1; }
            set
            {
                textbox1 = value;
                OnPropertyChanged(nameof(TextBox1));
            }
        }
        private string textBox2;
        public string TextBox2
        {
            get { return textBox2; }
            set
            {
                textBox2 = value;
                OnPropertyChanged(nameof(TextBox2));
            }
        }

        public ThirdTabViewModel()
        {
            TimerViewModel = new TimerViewModel();
            getDataService = new GetDataService();

            SerialViewModel = new SerialViewModel(TimerViewModel, getDataService);
            getDataService.Method += DataReceived;
            ClearCommand = new RelayCommand(Clear);
            Send1Command = new RelayCommand(SendSerial1);
            Send2Command = new RelayCommand(SendSerial2);

            stringBuilder = new StringBuilder();

            TextBox1 = "";
            TextBox2 = "";

        }

        private void DataReceived()
        {
            stringBuilder.Append(getDataService.StringData);
            stringBuilder.Append("\n");
            if (stringBuilder.Length >= 10000)
            {
                int a = stringBuilder.Length - 10000;
                stringBuilder.Remove(0, a);

                Text = "The oldest data was removed... \n" + stringBuilder.ToString();
                return;
            }
            Text = stringBuilder.ToString();
        }

        private void SendSerial1()
        {
            if(!SerialViewModel.SerialState)
            {
                SerialViewModel.SendSerial(TextBox1);
                SentText += TextBox1 + Environment.NewLine;
            }
        }

        private void SendSerial2()
        {
            if (!SerialViewModel.SerialState)
            {
                SerialViewModel.SendSerial(TextBox2);
                SentText += TextBox2 + Environment.NewLine;
            }
             
        }

        private void Clear()
        {
            stringBuilder.Clear();
            Text = "";
            SentText = "";
            TextBox1 = "";
            TextBox2 = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
