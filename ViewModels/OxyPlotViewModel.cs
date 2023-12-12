using OxyPlot.Series;
using OxyPlot;
using System.ComponentModel;
using DED_MonitoringSensor.ViewModels;
using OxyPlot.Axes;

namespace DED_MonitoringSensor.Views
{
    class OxyPlotViewModel : ViewModelBase
    {
        #region 1.초기 환경 변수 및 프로퍼티

        private double output;
        public double Output
        {
            get => output; set => SetProperty(ref output, value);
        }

        private double std;
        public double Std
        {
            get => std; set => SetProperty(ref std, value);
        }

        public PlotModel PlotModel { get; set; }

        private LineSeries linePlotModel;

        #endregion  

        #region 2. 생성자
        public OxyPlotViewModel(string title)
        {
            PlotModel = new PlotModel { Title = title, TitleFontSize = 11};
            linePlotModel = new LineSeries();

            PlotModel.Series.Add(linePlotModel);

        }

        public OxyPlotViewModel(string title, double min, double max)
        {
            PlotModel = new PlotModel { Title = title, TitleFontSize = 11 };
            linePlotModel = new LineSeries();

            PlotModel.Series.Add(linePlotModel);

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = min,
                Maximum = max
            };

            PlotModel.Axes.Add(yAxis);

        }

        #endregion

        #region 3. 메서드
        public void GrpahUpdate(double x, bool state)
        {
            linePlotModel.Points.Add(new DataPoint(x, output));
            PlotModel.InvalidatePlot(state);
        }


        public void GrahpClear()
        {
            linePlotModel.Points.Clear();
            PlotModel.InvalidatePlot(true);
            Output = 0;
            Std = 0;
        }

        #endregion

    }
}
