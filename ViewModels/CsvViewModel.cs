using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace DED_MonitoringSensor.ViewModels
{
    class CsvViewModel : ViewModelBase
    {
        #region 1.초기 환경 변수 및 프로퍼티

        private string csvFilePath; // CSV 파일 경로
        private StreamWriter writer; // CSV 파일 작성자

        private bool csvState;
        public bool CsvState
        {
            get => csvState; set => SetProperty(ref csvState, value);
        }

        private string line;

        private RelayCommand _csvCommand;
        public RelayCommand CsvCommand
        {
            get => _csvCommand; set => SetProperty(ref _csvCommand, value);
        }
        #endregion

        #region 2. 생성자
        public CsvViewModel(string line)
        {
            this.line = line;

            CsvCommand = new RelayCommand(Open);
            CsvState = false;
        }
        #endregion

        #region 3. 메서드
        /// <summary>
        /// CSV 파일 버튼 스위치 
        /// </summary>
        public void Open()
        {
            if (CsvState = CreateCsv(line))
            {
                CsvCommand = new RelayCommand(Close);
            }

        } 

        /// <summary>
        /// CSV 파일 버튼 스위치
        /// </summary>
        public void Close()
        {
            CsvState = CloseCsv();
            CsvCommand = new RelayCommand(Open);
        }

        /// <summary>
        /// CSV 파일 데이터 추가
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="dataArray"></param>
        public void Add(string timer, double[] dataArray)
        {
            try
            {
                writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
                StringBuilder stb = new StringBuilder();
                stb.Append(timer);
                foreach (double num in dataArray)
                {
                    stb.Append(",");
                    stb.Append(num);
                }
                writer.WriteLine(stb.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                
            }
            finally
            {
                writer.Close();
            }

        }

        /// <summary>
        /// CSV파일 생성
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool CreateCsv(string line)
        {
            try
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
                return true;
               

            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            } 
           
        }

        /// <summary>
        /// CSV 파일 연결해제
        /// </summary>
        /// <returns></returns>
        public bool CloseCsv()
        {
            MessageBox.Show(csvFilePath + " Disconnect !");
            return false;
        }

        #endregion
    }



}

