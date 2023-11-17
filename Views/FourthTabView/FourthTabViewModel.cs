using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.ComponentModel;
using System.Text;

namespace DED_MonitoringSensor.Views.FourthTabView
{
    class FourthTabViewModel : ViewModelBase, IGetDataService
    {
        public UdpViewModel UdpViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand Send1Command { get; set; }
        public RelayCommand Send2Command { get; set; }

        

        private StringBuilder stringBuilder;

        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private string sentText;
        public string SentText
        {
            get => sentText; set => SetProperty(ref sentText, value);
        }
        private string textbox1;
        public string TextBox1
        {
            get => textbox1; set => SetProperty(ref textbox1, value);
        }
        private string textBox2;
        public string TextBox2
        {
            get => textBox2; set => SetProperty(ref textBox2, value);
        }
        public FourthTabViewModel()
        {
            TimerViewModel = new TimerViewModel();
            

            UdpViewModel = new UdpViewModel(TimerViewModel, this);
            

            ClearCommand = new RelayCommand(Clear);
            Send1Command = new RelayCommand(SendUdp1);
            Send2Command = new RelayCommand(SendUdp2);

            stringBuilder = new StringBuilder();

            TextBox1 = "";
            TextBox2 = "";
        }

        private void SendUdp2()
        {
            if (!UdpViewModel.UdpState)
            {
                UdpViewModel.SendUdp(TextBox2);
                SentText += TextBox2 + Environment.NewLine;
            }
        }

        private void SendUdp1()
        {
            if (!UdpViewModel.UdpState)
            {
                UdpViewModel.SendUdp(TextBox1);
                SentText += TextBox1 + Environment.NewLine;
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

        public void GetData(string getData)
        {
            stringBuilder.Append(getData);
            if (stringBuilder.Length >= 10000)
            {
                int a = stringBuilder.Length - 10000;

                stringBuilder.Remove(0, a);

                Text = "The oldest data was removed... \n" + stringBuilder.ToString();
                return;
            }
            Text = stringBuilder.ToString();
        }
    }
}
