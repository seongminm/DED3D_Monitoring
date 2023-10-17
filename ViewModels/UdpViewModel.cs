using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using System.ComponentModel;

namespace DED_MonitoringSensor.ViewModels
{

    class UdpViewModel : ViewModelBase
    {
        private string ip;
        public string Ip
        {
            get => ip; set => SetProperty(ref ip, value);
           
        }

        private string port;
        public string Port
        {
            get => port; set => SetProperty(ref port, value);
        }

        private string udpContent;
        public string UdpContent
        {
            get => udpContent; set => SetProperty(ref udpContent, value);
        }

        private bool udpState;
        public bool UdpState
        {
            get => udpState; set => SetProperty(ref udpState, value);
        }


        private RelayCommand _udpCommand;
        public RelayCommand UdpCommand
        {
            get => _udpCommand; set => SetProperty(ref _udpCommand, value);
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

        
    }
}
