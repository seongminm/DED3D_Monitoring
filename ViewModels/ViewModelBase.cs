using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DED_MonitoringSensor.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// UI 업데이트 메서드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        // 파생 클래스의 속성 setter에서 사용할 메서드
        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value)) return false;

            member = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
    }
}
