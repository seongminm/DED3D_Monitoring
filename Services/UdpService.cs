using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;

namespace DED_MonitoringSensor.Services
{
    class UdpService
    {
        UdpClient udpClient;
        GetDataService getDataService;

        public bool udpState;

        private string ip;
        private string port;

        public UdpService(GetDataService getDataService)
        {
            this.getDataService = getDataService;
            udpState = false;
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
                udpState = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        public void CloseUdp()
        {
            udpState = false;
            udpClient.Close();
        }

        public void SendUdp(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, ip, int.Parse(port));
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (udpState)
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = udpClient.EndReceive(ar, ref ipEndPoint);
                getDataService.StringData = Encoding.UTF8.GetString(receivedBytes); // 바이트 배열을 문자열로 변환               
                udpClient.BeginReceive(ReceiveCallback, null); // 계속해서 데이터 수신 대기
            }
        }
    }
}
