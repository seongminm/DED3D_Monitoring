﻿using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.ComponentModel;
using System.Text;

namespace DED_MonitoringSensor.Views.ThirdTabView
{
    class ThirdTabViewModel : ViewModelBase, IGetDataService
    {
        #region 1.초기 환경 변수 및 프로퍼티

        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand Send1Command { get; set; }
        public RelayCommand Send2Command { get; set; }

        private StringBuilder stringBuilder;

        private string text;
        public string Text
        {
            get => text; set => SetProperty(ref text, value);
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

        #endregion

        #region 2. 생성자
        public ThirdTabViewModel()
        {
            TimerViewModel = new TimerViewModel();

            SerialViewModel = new SerialViewModel(TimerViewModel, this);
            ClearCommand = new RelayCommand(Clear);
            Send1Command = new RelayCommand(SendSerial1);
            Send2Command = new RelayCommand(SendSerial2);

            stringBuilder = new StringBuilder();

            TextBox1 = "";
            TextBox2 = "";

        }
        #endregion

        #region 3. 메서드
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

        public void GetData(string getData)
        {
            stringBuilder.Append(getData);
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
        #endregion
    }
}
