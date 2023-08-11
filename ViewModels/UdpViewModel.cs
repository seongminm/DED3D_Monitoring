using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using System.ComponentModel;

namespace DED_MonitoringSensor.ViewModels
{

    class UdpViewModel : INotifyPropertyChanged
    {
        private string ip;
        public string Ip
        {
            get { return ip; }
            set
            {
                ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private string udpContent;
        public string UdpContent
        {
            get { return udpContent; }
            set
            {
                udpContent = value;
                OnPropertyChanged(nameof(UdpContent));
            }
        }

        private bool udpState;
        public bool UdpState
        {
            get { return udpState; }
            set
            {
                udpState = value;
                OnPropertyChanged(nameof(UdpState));
            }
        }


        private RelayCommand _udpCommand;
        public RelayCommand UdpCommand
        {
            get { return _udpCommand; }
            set
            {
                _udpCommand = value;
                OnPropertyChanged(nameof(UdpCommand));
            }
        }

        private UdpService udpService;
        private GetDataService getDataService;

        TimerViewModel timerViewModel;

        public UdpViewModel(TimerViewModel timerViewModel, GetDataService getDataService)
        {
            this.getDataService = getDataService;
            this.timerViewModel = timerViewModel;

            udpService = new UdpService(this.getDataService);
            UdpCommand = new RelayCommand(OpenUdp);

            UdpState = true;
            UdpContent = "Open";
        }

        private void OpenUdp()
        {
            udpService.OpenUdp(Ip, Port);
            if (udpService.udpState)
            {
                UdpState = false;
                UdpCommand = new RelayCommand(CloseUdp);
                UdpContent = "Close";
                timerViewModel.Start();
            }
        }

        private void CloseUdp()
        {
            udpService.CloseUdp();
            if (!udpService.udpState)
            {
                UdpState = true;
                UdpCommand = new RelayCommand(OpenUdp);
                UdpContent = "Open";
                timerViewModel.Stop();
            }
        }

        public void SendUdp(string message)
        {
            if(udpService.udpState)
            {
                udpService.SendUdp(message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
