using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace DED_MonitoringSensor.ViewModels
{
    class TimerViewModel : ViewModelBase
    {
        #region 1.초기 환경 변수 및 프로퍼티
        private DispatcherTimer _timer;
        private int _seconds; // 초 단위 시간 변수 선언


        private string _timerContent;
        public string TimerContent
        {
            get => _timerContent; set => SetProperty(ref _timerContent, value);
        }
        #endregion

        #region 2. 생성자
        public TimerViewModel()
        {
            TimerContent = "00:00:00";

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _seconds = 0;
        }
        #endregion

        #region 3. 메서드
        /// <summary>
        /// 타이머 시간 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            _seconds++;
            TimerContent = TimeSpan.FromSeconds(_seconds).ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// 타이머 시작
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// 타이머 정지
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _seconds = 0;
            TimerContent = "00:00:00";
        }
        #endregion

    }
}
