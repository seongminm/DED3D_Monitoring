using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace DED_MonitoringSensor.Services
{
    class CsvService
    {
        private string csvFilePath; // CSV 파일 경로
        private StreamWriter writer; // CSV 파일 작성자

        public bool CreateCsv(string line)
        {
            string currentDate = DateTime.Now.ToString("yyMMdd_HHmm");

            var dialog = new SaveFileDialog
            {
                FileName = currentDate,
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv",
                AddExtension = true
            };

            if (dialog.ShowDialog() == true)
            {
                // 선택한 경로로 CSV 파일을 저장합니다.
                csvFilePath = dialog.FileName;

                writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
                writer.WriteLine(line);
                writer.Close();
                MessageBox.Show("CSV file saved successfully.");
                return true;
            }

            return false;
        }

        public void AddCsv(string timer, string data)
        {
            try
            {
                writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
                string[] splitData = data.Split('/');
                string result = timer;
                for (int i = 0; i < splitData.Length; i++)
                {
                    result += "," + splitData[i];
                }
                writer.WriteLine(result);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }finally
            {
                writer.Close();
            }
           
        }

        public bool CloseCsv()
        {
            MessageBox.Show(csvFilePath + " Disconnect !");
            return false;
        }
    }
}
