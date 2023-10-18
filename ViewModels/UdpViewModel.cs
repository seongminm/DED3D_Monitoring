using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace DED_MonitoringSensor.ViewModels
{
    class UdpViewModel : ViewModelBase
    {
        UdpClient udpClient;

        IGetDataService IGetDataService;

        private string getData;
        public string GetData
        {
            get => getData;
            set => SetProperty(ref getData, value);
        }

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

        private bool udpOpenState;
        public bool UdpOpenState
        {
            get => udpOpenState; set => SetProperty(ref udpOpenState, value);
        }

        private RelayCommand _udpCommand;
        public RelayCommand UdpCommand
        {
            get => _udpCommand; set => SetProperty(ref _udpCommand, value);
        }

        TimerViewModel timerViewModel;

        public UdpViewModel(TimerViewModel timerViewModel, IGetDataService getDataService)
        {
            this.timerViewModel = timerViewModel;
            this.IGetDataService = getDataService;

            UdpCommand = new RelayCommand(OpenUdp);

            UdpState = true;
            UdpOpenState = false;
            UdpContent = "Open";
        }

        private void OpenUdp()
        {
            OpenUdp(Ip, Port);
            if (UdpOpenState)
            {
                UdpState = false;
                UdpCommand = new RelayCommand(CloseUdp);
                UdpContent = "Close";
                timerViewModel.Start();
            }
        }

        private void CloseUdp()
        {
            try
            {
                udpClient.Close();
                udpOpenState = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                udpOpenState = true;
            }
            
            if (!UdpOpenState)
            {
                UdpState = true;
                UdpCommand = new RelayCommand(OpenUdp);
                UdpContent = "Open";
                timerViewModel.Stop();
            }
        }

        public void SendUdp(string message)
        {
            if(UdpOpenState)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, ip, int.Parse(port));
            }
        }


        public void OpenUdp(string ip, string port)
        {
            try
            {
                this.ip = ip;
                this.port = port;

                udpClient = new UdpClient(int.Parse(port));

                byte[] data = Encoding.UTF8.GetBytes("open");
                udpClient.Send(data, data.Length, ip, int.Parse(port));

                udpClient.BeginReceive(ReceiveCallback, null);
                UdpOpenState = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                UdpOpenState = false;
            }

        }



        private void ReceiveCallback(IAsyncResult ar)
        {
            if (UdpOpenState)
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = udpClient.EndReceive(ar, ref ipEndPoint);
                GetData = Encoding.UTF8.GetString(receivedBytes); // 바이트 배열을 문자열로 변환               
                IGetDataService.GetData();
                udpClient.BeginReceive(ReceiveCallback, null); // 계속해서 데이터 수신 대기
            }
        }
    }

}

