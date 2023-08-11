﻿using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DED_MonitoringSensor.Views.SecondTabView
{
    class SecondTabViewModel : INotifyPropertyChanged
    {
        public UdpViewModel UdpViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public OxyPlotViewModel LaserPower { get; set; }
        public OxyPlotViewModel Visible1 { get; set; }
        public OxyPlotViewModel Visible2 { get; set; }
        public OxyPlotViewModel Visible3 { get; set; }
        public OxyPlotViewModel Sound { get; set; }
        public OxyPlotViewModel Powder { get; set; }


        public CsvViewModel CsvViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; set; }
        public GetDataService getDataService;




        private bool graphState;
        public bool GraphState
        {
            get { return graphState; }
            set
            {
                graphState = value;
                OnPropertyChanged(nameof(GraphState));
            }
        }

        private string graphContent;
        public string GraphContent
        {
            get { return graphContent; }
            set
            {
                graphContent = value;
                OnPropertyChanged(nameof(GraphContent));
            }
        }

        public RelayCommand GraphCommand { get; set; }
        public RelayCommand GraphClearCommand { get; set; }

        private double dataCount = 1;
        private string line = $"{"Time"},{"Temperature"},{"Humidity"}, {"PM1.0"}, {"PM2.5"}, {"PM10"}, {"PID"}, {"MICS"}, {"CJMCU"}, {"MQ"}, {"HCHO"}";

        private List<double> laserPower = new List<double>();
        private List<double> visible1 = new List<double>();
        private List<double> visible2 = new List<double>();
        private List<double> visible3 = new List<double>();
        private List<double> sound = new List<double>();
        private List<double> powder = new List<double>();

        public SecondTabViewModel()
        {
            TimerViewModel = new TimerViewModel();
            getDataService = new GetDataService();

            LaserPower = new OxyPlotViewModel("Laser Power");
            Visible1 = new OxyPlotViewModel("Visible1");
            Visible2 = new OxyPlotViewModel("Visible2");
            Visible3 = new OxyPlotViewModel("Visible3");
            Sound = new OxyPlotViewModel("Sound");
            Powder = new OxyPlotViewModel("Powder");

            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            UdpViewModel = new UdpViewModel(TimerViewModel, getDataService);
            getDataService.Method += DataReceived;

            GraphState = true;
            GraphContent = "Stop";
            GraphCommand = new RelayCommand(GraphToggle);
            GraphClearCommand = new RelayCommand(ClearGraph);

        }

        private void ClearGraph()
        {
            dataCount = 0;
            LaserPower.GrahpClear();
            Visible1.GrahpClear();
            Visible2.GrahpClear();
            Visible3.GrahpClear();
            Sound.GrahpClear();
            Powder.GrahpClear();
        }

        private void GraphToggle()
        {
            if (GraphState)
            {
                GraphState = !GraphState;
                GraphContent = "Live";
            }
            else
            {
                GraphState = !GraphState;
                GraphContent = "Stop";
            }
        }

        private void DataReceived()
        {
            string data = getDataService.StringData;
            string[] splitData = data.Split('/');
            if (splitData.Length >= 7 && splitData.Length <= 8)
            {
               

                if (splitData[0].Equals("1"))
                {
                    laserPower.Add(double.Parse(splitData[1]));
                    visible1.Add(double.Parse(splitData[2]));
                    visible2.Add(double.Parse(splitData[3]));
                    visible3.Add(double.Parse(splitData[4]));
                    sound.Add(double.Parse(splitData[5]));
                    powder.Add(double.Parse(splitData[6]));

                }
                else if (splitData[0].Equals("0") && laserPower.Count != 0)
                {
                    laserPower.Add(double.Parse(splitData[1]));
                    visible1.Add(double.Parse(splitData[2]));
                    visible2.Add(double.Parse(splitData[3]));
                    visible3.Add(double.Parse(splitData[4]));
                    sound.Add(double.Parse(splitData[5]));
                    powder.Add(double.Parse(splitData[6]));

                    string calculatedData = "";

                    calculatedData += Calculate(LaserPower, laserPower);
                    calculatedData += "/" + Calculate(Visible1, visible1);
                    calculatedData += "/" + Calculate(Visible2, visible2);
                    calculatedData += "/" + Calculate(Visible3, visible3);
                    calculatedData += "/" + Calculate(Sound, sound);
                    calculatedData += "/" + Calculate(Powder, powder);

                    LaserPower.GrpahUpdate(dataCount, GraphState);
                    Visible1.GrpahUpdate(dataCount, GraphState);
                    Visible2.GrpahUpdate(dataCount, GraphState);
                    Visible3.GrpahUpdate(dataCount, GraphState);
                    Sound.GrpahUpdate(dataCount, GraphState);
                    Powder.GrpahUpdate(dataCount, GraphState);

                    dataCount++;

                    laserPower.Clear();
                    visible1.Clear();
                    visible2.Clear();
                    visible3.Clear();
                    sound.Clear();
                    powder.Clear();

                    DateTime currentTime = DateTime.Now;
                    string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");

                    if (CsvViewModel.CsvState)
                    {
                        CsvViewModel.Add(formattedTime, calculatedData);
                    }

                    if (DatabaseViewModel.MysqlState)
                    {
                        DatabaseViewModel.AddDatabase(formattedTime, calculatedData);
                    }
                }
            }

        }
        static string Calculate(OxyPlotViewModel oxyplotViewModel, List<double> numbers)
        {
            double mean = numbers.Average();
            double sumOfSquaredDifferences = numbers.Sum(x => Math.Pow(x - mean, 2));
            double standardDeviation = Math.Sqrt(sumOfSquaredDifferences / (numbers.Count - 1));

            oxyplotViewModel.Output = Math.Round(mean, 2);
            oxyplotViewModel.Std = Math.Round(standardDeviation, 2);

            return oxyplotViewModel.Output.ToString() + "/" + oxyplotViewModel.Std.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}