using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace DED_MonitoringSensor.Views.SecondTabView
{
    class SecondTabViewModel : ViewModelBase, IGetDataService
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




        private bool graphState;
        public bool GraphState
        {
            get => graphState; set => SetProperty(ref graphState, value);
        }

        private string graphContent;
        public string GraphContent
        {
            get => graphContent; set => SetProperty(ref graphContent, value);
        }

        public RelayCommand GraphCommand { get; set; }
        public RelayCommand GraphClearCommand { get; set; }

        private double dataCount = 1;

        private List<double> laserPower = new List<double>();
        private List<double> visible1 = new List<double>();
        private List<double> visible2 = new List<double>();
        private List<double> visible3 = new List<double>();
        private List<double> sound = new List<double>();
        private List<double> powder = new List<double>();

        public SecondTabViewModel()
        {
            TimerViewModel = new TimerViewModel();

            LaserPower = new OxyPlotViewModel("Laser Power");
            Visible1 = new OxyPlotViewModel("Visible1");
            Visible2 = new OxyPlotViewModel("Visible2");
            Visible3 = new OxyPlotViewModel("Visible3");
            Sound = new OxyPlotViewModel("Sound");
            Powder = new OxyPlotViewModel("Powder");

            string line = $"{"Time"},{"LaserPower_avg"},{"LaserPower_std"},{"visible1_avg"},{"visible1_std"},{"visible2_avg"},{"visible2_std"},{"visible3_avg"},{"visible3_std"},{"Sound_avg"},{"Sound_std"},{"Powder_avg"},{"Powder_std"}";
            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            UdpViewModel = new UdpViewModel(TimerViewModel, this);

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

       
        static string Calculate(OxyPlotViewModel oxyplotViewModel, List<double> numbers)
        {
            double mean = numbers.Average();
            double sumOfSquaredDifferences = numbers.Sum(x => Math.Pow(x - mean, 2));
            double standardDeviation = Math.Sqrt(sumOfSquaredDifferences / (numbers.Count - 1));

            oxyplotViewModel.Output = Math.Round(mean, 2);
            oxyplotViewModel.Std = Math.Round(standardDeviation, 2);

            return oxyplotViewModel.Output.ToString() + "/" + oxyplotViewModel.Std.ToString() + "/";
        }

        public void GetData()
        {
            //string data = getDataService.StringData;
            string[] splitData = UdpViewModel.GetData.Split('/');
            if (splitData.Length >= 7 && splitData.Length <= 8)
            {


                if (splitData[0].Equals("1"))
                {
                    visible1.Add(double.Parse(splitData[1]));
                    visible2.Add(double.Parse(splitData[2]));
                    visible3.Add(double.Parse(splitData[3]));
                    powder.Add(double.Parse(splitData[4]));
                    sound.Add(double.Parse(splitData[5]));
                    laserPower.Add(double.Parse(splitData[6]));

                }
                else if (splitData[0].Equals("0") && laserPower.Count != 0)
                {
                    visible1.Add(double.Parse(splitData[1]));
                    visible2.Add(double.Parse(splitData[2]));
                    visible3.Add(double.Parse(splitData[3]));
                    powder.Add(double.Parse(splitData[4]));
                    sound.Add(double.Parse(splitData[5]));
                    laserPower.Add(double.Parse(splitData[6]));

                    StringBuilder calculatedData = new StringBuilder();
                    calculatedData.Append(Calculate(LaserPower, laserPower));
                    calculatedData.Append(Calculate(Visible1, visible1));
                    calculatedData.Append(Calculate(Visible2, visible2));
                    calculatedData.Append(Calculate(Visible3, visible3));
                    calculatedData.Append(Calculate(Sound, sound));
                    calculatedData.Append(Calculate(Powder, powder));

                    string calculatedDataString = calculatedData.ToString();

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
                        CsvViewModel.Add(formattedTime, calculatedDataString);
                    }

                    if (DatabaseViewModel.MysqlState)
                    {
                        DatabaseViewModel.AddDatabase(formattedTime, calculatedDataString);
                    }
                }
            }
        }
    }
}