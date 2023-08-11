using System.Windows.Controls;

namespace DED_MonitoringSensor.Views.ThirdTabView
{
    /// <summary>
    /// ThirdTabView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ThirdTabView : UserControl
    {
        public ThirdTabView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBox.ScrollToEnd();
        }

        private void TBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBox2.ScrollToEnd();
        }
    }
}
