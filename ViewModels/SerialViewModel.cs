using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;

namespace DED_MonitoringSensor.ViewModels
{
    class SerialViewModel : ViewModelBase
    {
        SerialPort serialPort;
        IGetDataService IGetDataService;

        private RelayCommand serialCommand;
        public RelayCommand SerialCommand
        {
            get => serialCommand;
            set => SetProperty(ref serialCommand, value);

        }
        public RelayCommand SerialPortCommand { get; set; }

        private string getData;
        public string GetData
        {
            get => getData;
            set => SetProperty(ref getData, value);
        }

        private string serialContent;
        public string SerialContent
        {
            get => serialContent;
            set => SetProperty(ref serialContent, value);
        }

        private bool serialState;
        public bool SerialState
        {
            get => serialState;
            set => SetProperty(ref serialState, value);
        }

        private string selectedSerialPort;
        public string SelectedSerialPort
        {
            get => selectedSerialPort;
            set => SetProperty(ref selectedSerialPort, value);
        }

        private string selectedSerialBaudRate;
        public string SelectedSerialBaudRate
        {
            get => selectedSerialBaudRate;
            set => SetProperty(ref selectedSerialBaudRate, value);
        }
        private List<string> serialPorts;
        public List<string> SerialPorts
        {
            get => serialPorts;
            set => SetProperty(ref serialPorts, value);

        }
        public List<int> SerialBaudRate { get; set; }

        TimerViewModel timerViewModel;

        public SerialViewModel(TimerViewModel timerViewModel, IGetDataService getDataService)
        {

            this.timerViewModel = timerViewModel;
            this.IGetDataService = getDataService;

            SerialCommand = new RelayCommand(OpenSerial);

            SerialContent = "Open";
            SerialState = true;
            SerialPorts = new List<string>(SerialPort.GetPortNames());
            SerialBaudRate = new List<int> { 9600, 14400, 19200, 38400, 57600, 115200 };
            SerialPortCommand = new RelayCommand(LoadSerial);

        }

        private void LoadSerial()
        {
            SerialPorts = new List<string>(SerialPort.GetPortNames());
        }

        private void OpenSerial()
        {
            try
            {
                serialPort = new SerialPort();
                serialPort.PortName = SelectedSerialPort;
                serialPort.BaudRate = int.Parse(SelectedSerialBaudRate);
                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (serialPort.IsOpen)
            {
                SerialCommand = new RelayCommand(CloseSerial);
                SerialContent = "Close";
                SerialState = false;
                timerViewModel.Start();
            }
        }

        public void CloseSerial()
        {
            serialPort.DiscardInBuffer();
            serialPort.DataReceived -= SerialPort_DataReceived;
            try
            {
                serialPort.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            if (!serialPort.IsOpen)
            {
                SerialCommand = new RelayCommand(OpenSerial);
                SerialContent = "Open";
                SerialState = true;
                timerViewModel.Stop();
            }
        }

        public void SendSerial(string message)
        {
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine(message);
            }
        }


        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                GetData = serialPort.ReadLine();
                IGetDataService.GetData();
            }
            catch
            {
                return;
            }

        }

    }
}