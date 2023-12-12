using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DED_MonitoringSensor.Views.FirstTabView
{
    class FirstTabViewModel : ViewModelBase, IGetDataService
    {
        #region 1.초기 환경 변수 및 프로퍼티
        public SerialViewModel SerialViewModel { get; set; }
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
            get => graphState;
            set => SetProperty(ref graphState, value);
        }

        private string graphContent;
        public string GraphContent
        {
            get => graphContent;
            set => SetProperty(ref graphContent, value);
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

        #endregion

        #region 2. 생성자
        public FirstTabViewModel()
        {
            TimerViewModel = new TimerViewModel();

            LaserPower = new OxyPlotViewModel("Laser Power", 0, 3.4);
            Visible = new OxyPlotViewModel("Visible Light", 0, 3.4);
            IrFilter = new OxyPlotViewModel("IR_Filter", 0, 3.4);
            BlueFilter = new OxyPlotViewModel("Blue_Filter", 0, 3.4);
            Sound = new OxyPlotViewModel("Sound", 0, 3.4);
            Powder980 = new OxyPlotViewModel("980nm_Powder", 0, 3.4);
            Powder780 = new OxyPlotViewModel("780nm_Powder", 0, 3.4);
            Powder650 = new OxyPlotViewModel("650nm_Powder", 0, 3.4);

            string line = $"{"Time"},{"Visible"},{"IRFilter"}," +
                $"{"BlueFilter"},{"Powder980"}," +
                $"{"Sound"}," +
                $"{"LaserPower"},{"Powder780"},{"Powder650"}";
            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            SerialViewModel = new SerialViewModel(TimerViewModel, this);

            GraphState = true;
            GraphContent = "Stop";
            GraphCommand = new RelayCommand(GraphToggle);
            GraphClearCommand = new RelayCommand(ClearGraph);

        }
        #endregion

        #region 3. 메서드

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

        static void Calculate(OxyPlotViewModel oxyplotViewModel, List<double> numbers, double dataCount, bool graphState)
        {
            double mean = numbers.Average();
            double sumOfSquaredDifferences = numbers.Sum(x => Math.Pow(x - mean, 2));
            double standardDeviation = Math.Sqrt(sumOfSquaredDifferences / (numbers.Count - 1));

            oxyplotViewModel.Output = Math.Round(mean, 2);
            oxyplotViewModel.Std = Math.Round(standardDeviation, 2);

            oxyplotViewModel.GrpahUpdate(dataCount, graphState);
            numbers.Clear();
        }

        public void GetData(string getData)
        {

            string[] stringSplitData = getData.Split('/');
            

            if (stringSplitData.Length >= 9 && stringSplitData.Length <= 10)
            {
                double[] splitdata = new double[8];
                for (int i = 0; i < 8; i++)
                {
                    splitdata[i] = double.Parse(stringSplitData[i + 1]) * 0.0001;
                }

                if (stringSplitData[0].Equals("1"))
                {
                    visible.Add(splitdata[0]);
                    irFilter.Add(splitdata[1]);
                    blueFilter.Add(splitdata[2]);
                    powder980.Add(splitdata[3]);
                    sound.Add(splitdata[4]);
                    laserPower.Add(splitdata[5]);
                    powder780.Add(splitdata[6]);
                    powder650.Add(splitdata[7]);

                } else if (stringSplitData[0].Equals("0") && laserPower.Count != 0)
                {
                    visible.Add(splitdata[0]);
                    irFilter.Add(splitdata[1]);
                    blueFilter.Add(splitdata[2]);
                    powder980.Add(splitdata[3]);
                    sound.Add(splitdata[4]);
                    laserPower.Add(splitdata[5]);
                    powder780.Add(splitdata[6]);
                    powder650.Add(splitdata[7]);

                    Calculate(LaserPower, laserPower, dataCount, GraphState);
                    Calculate(Visible, visible, dataCount, GraphState);
                    Calculate(IrFilter, irFilter, dataCount, GraphState);
                    Calculate(BlueFilter, blueFilter, dataCount, GraphState);
                    Calculate(Sound, sound, dataCount, GraphState);
                    Calculate(Powder980, powder980, dataCount, GraphState);
                    Calculate(Powder780, powder780, dataCount, GraphState);
                    Calculate(Powder650, powder650, dataCount, GraphState);

                    dataCount++;
                }

                if (CsvViewModel.CsvState)
                {
                    DateTime currentTime = DateTime.Now;
                    string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");
                    CsvViewModel.Add(formattedTime, splitdata);
                }

                if (DatabaseViewModel.MysqlState)
                {
                    DateTime currentTime = DateTime.Now;
                    string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");
                    DatabaseViewModel.AddDatabase(formattedTime, splitdata);
                }

            }
        }
        #endregion
    }
}
