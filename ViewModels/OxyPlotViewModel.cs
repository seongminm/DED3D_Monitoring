using OxyPlot.Series;
using OxyPlot;
using System.ComponentModel;
using DED_MonitoringSensor.ViewModels;

namespace DED_MonitoringSensor.Views
{
    class OxyPlotViewModel : ViewModelBase
    {
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


        public OxyPlotViewModel(string title)
        {
            PlotModel = new PlotModel { Title = title, TitleFontSize = 11};
            linePlotModel = new LineSeries();

            PlotModel.Series.Add(linePlotModel);

        }

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

       

    }
}
