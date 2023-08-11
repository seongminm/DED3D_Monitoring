using System.Windows.Controls;

namespace DED_MonitoringSensor.Views.FourthTabView
{
    /// <summary>
    /// FourthTabView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FourthTabView : UserControl
    {
        public FourthTabView()
        {
            InitializeComponent();
        }

        private void TBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBox.ScrollToEnd();
        }

        private void TBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBox2.ScrollToEnd();
        }
    }
}
