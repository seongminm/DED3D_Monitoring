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
        public OxyPlotViewModel Visible { get; set; }
        public OxyPlotViewModel IrFilter { get; set; }
        public OxyPlotViewModel BlueFilter { get; set; }
        public OxyPlotViewModel Sound { get; set; }
        public OxyPlotViewModel Powder980 { get; set; }
        public OxyPlotViewModel Powder780 { get; set; }
        public OxyPlotViewModel Powder650 { get; set; }

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
        private List<double> visible = new List<double>();
        private List<double> irFilter = new List<double>();
        private List<double> blueFilter = new List<double>();
        private List<double> sound = new List<double>();
        private List<double> powder980 = new List<double>();
        private List<double> powder780 = new List<double>();
        private List<double> powder650 = new List<double>();

        public SecondTabViewModel()
        {
            TimerViewModel = new TimerViewModel();

            LaserPower = new OxyPlotViewModel("Laser Power");
            Visible = new OxyPlotViewModel("Visible", 0, 3.3);
            IrFilter = new OxyPlotViewModel("IR_Filter", 0, 3.3);
            BlueFilter = new OxyPlotViewModel("Blue_Filter", 0, 3.3);
            Sound = new OxyPlotViewModel("Sound", 0, 3.3);
            Powder980 = new OxyPlotViewModel("980nm-Powder", 0, 3.3);
            Powder780 = new OxyPlotViewModel("780nm-Powder", 0, 3.3);
            Powder650 = new OxyPlotViewModel("650nm-Powder", 0, 3.3);

            string line = $"{"Time"},{"LaserPower_avg"},{"LaserPower_std"},{"Visible_avg"},{"Visible_std"}," +
                            $"{"IRFilter_avg"},{"IRFilter_std"},{"BlueFilter_avg"},{"BuleFilter_std"}," +
                            $"{"Sound_avg"},{"Sound_std"}," +
                            $"{"Powder980_avg"},{"Powder980_std"},{"Powder780_avg"},{"Powder780_std"},{"Powder650_avg"},{"Powder650_std"}"; CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            UdpViewModel = new UdpViewModel(TimerViewModel, this);

            GraphState = true;
            GraphContent = "Stop";
            GraphCommand = new RelayCommand(GraphToggle);
            GraphClearCommand = new RelayCommand(ClearGraph);

        }

        private void ClearGraph()
        {
            dataCount = 1;
            LaserPower.GrahpClear();
            Visible.GrahpClear();
            IrFilter.GrahpClear();
            BlueFilter.GrahpClear();
            Sound.GrahpClear();
            Powder980.GrahpClear();
            Powder780.GrahpClear();
            Powder650.GrahpClear();
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
            if (splitData.Length >= 9 && splitData.Length <= 10)
            {


                if (splitData[0].Equals("1"))
                {
                    visible.Add(double.Parse(splitData[1]));
                    irFilter.Add(double.Parse(splitData[2]));
                    blueFilter.Add(double.Parse(splitData[3]));
                    powder980.Add(double.Parse(splitData[4]));
                    sound.Add(double.Parse(splitData[5]));
                    laserPower.Add(double.Parse(splitData[6]));
                    powder780.Add(double.Parse(splitData[7]));
                    powder650.Add(double.Parse(splitData[8]));

                }
                else if (splitData[0].Equals("0") && laserPower.Count != 0)
                {
                    visible.Add(double.Parse(splitData[1]));
                    irFilter.Add(double.Parse(splitData[2]));
                    blueFilter.Add(double.Parse(splitData[3]));
                    powder980.Add(double.Parse(splitData[4]));
                    sound.Add(double.Parse(splitData[5]));
                    laserPower.Add(double.Parse(splitData[6]));
                    powder780.Add(double.Parse(splitData[7]));
                    powder650.Add(double.Parse(splitData[8]));

                    StringBuilder calculatedData = new StringBuilder();
                    calculatedData.Append(Calculate(LaserPower, laserPower));
                    calculatedData.Append(Calculate(Visible, visible));
                    calculatedData.Append(Calculate(IrFilter, irFilter));
                    calculatedData.Append(Calculate(BlueFilter, blueFilter));
                    calculatedData.Append(Calculate(Sound, sound));
                    calculatedData.Append(Calculate(Powder980, powder980));
                    calculatedData.Append(Calculate(Powder780, powder780));
                    calculatedData.Append(Calculate(Powder650, powder650));

                    string calculatedDataString = calculatedData.ToString();

                    LaserPower.GrpahUpdate(dataCount, GraphState);
                    Visible.GrpahUpdate(dataCount, GraphState);
                    IrFilter.GrpahUpdate(dataCount, GraphState);
                    BlueFilter.GrpahUpdate(dataCount, GraphState);
                    Sound.GrpahUpdate(dataCount, GraphState);
                    Powder980.GrpahUpdate(dataCount, GraphState);
                    Powder780.GrpahUpdate(dataCount, GraphState);
                    Powder650.GrpahUpdate(dataCount, GraphState);

                    dataCount++;

                    laserPower.Clear();
                    visible.Clear();
                    irFilter.Clear();
                    blueFilter.Clear();
                    sound.Clear();
                    powder980.Clear();
                    powder780.Clear();
                    powder650.Clear();

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