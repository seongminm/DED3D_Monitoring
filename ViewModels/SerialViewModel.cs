using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using System.Collections.Generic;
using System.ComponentModel;

namespace DED_MonitoringSensor.ViewModels
{
    class SerialViewModel : ViewModelBase
    {
        private RelayCommand serialCommand;
        public RelayCommand SerialCommand
        {
            get => serialCommand; set => SetProperty(ref serialCommand, value);
        }
        public RelayCommand SerialPortCommand { get; set; }
        private string _serialContent;
        public string SerialContent
        {
            get => _serialContent; set => SetProperty(ref _serialContent, value);
        }
        private bool _serialState;
        public bool SerialState
        {
            get => _serialState; set => SetProperty(ref _serialState, value);
        }
        private string _selectedSerialPort;
        public string SelectedSerialPort
        {
            get => _selectedSerialPort; set => SetProperty(ref _selectedSerialPort, value);
        }

        private string _selectedSerialBaudRate;
        public string SelectedSerialBaudRate
        {
            get => _selectedSerialBaudRate; set => SetProperty(ref _selectedSerialBaudRate, value);
        }
        private List<string> serialPorts;
        public List<string> SerialPorts
        {
            get => serialPorts; set => SetProperty(ref serialPorts, value);
            
        }
        public List<int> SerialBaudRate { get; set; }


        private SerialService serialService;
        private GetDataService getDataService;

        TimerViewModel timerViewModel;

        public SerialViewModel(TimerViewModel timerViewModel, GetDataService getDataService)
        {

            this.getDataService = getDataService;
            this.timerViewModel = timerViewModel;

            serialService = new SerialService(this.getDataService);
            SerialCommand = new RelayCommand(OpenSerial);
            
            SerialContent = "Open";
            SerialState = true;
            SerialPorts = serialService.SerialPorts;
            SerialBaudRate = serialService.SerialBaudRate;
            SerialPortCommand = new RelayCommand(LoadSerial);

        }

        private void LoadSerial()
        {
            serialService.LoadSerialPorts();
            SerialPorts = serialService.SerialPorts;
        }

        private void OpenSerial()
        {

            serialService.OpenSerial(SelectedSerialPort, SelectedSerialBaudRate);
            if (serialService.isOpen())
            {
                SerialCommand = new RelayCommand(CloseSerial);
                SerialContent = "Close";
                SerialState = false;
                timerViewModel.Start();
            }
        }

        public void CloseSerial()
        {
            serialService.CloseSerial();
            if (!serialService.isOpen())
            {
                SerialCommand = new RelayCommand(OpenSerial);
                SerialContent = "Open";
                SerialState = true;
                timerViewModel.Stop();
            }
        }

        public void SendSerial(string message)
        {
            if(serialService.isOpen())
            {
                serialService.SendSerial(message);
            }
        }

    }
}
