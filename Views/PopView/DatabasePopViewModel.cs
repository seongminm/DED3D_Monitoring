using DED_MonitoringSensor.Models;
using DED_MonitoringSensor.ViewModels;
using DED_MonitoringSensor.ViewModels.Command;

namespace DED_MonitoringSensor.Views.PopView
{
    class DatabasePopViewModel
    {
        #region 1.초기 환경 변수 및 프로퍼티

        DatabaseModel _databaseModel;
        DatabaseViewModel _databaseViewModel;

        public string Server { get; set; }
        public string DatabaseServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TableName { get; set; }
        public RelayCommand SetCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        #endregion

        #region 2. 생성자

        public DatabasePopViewModel(DatabaseModel databaseModel, DatabaseViewModel dataBaseViewModel)
        {
            _databaseModel = databaseModel;
            _databaseViewModel = dataBaseViewModel;
            SetCommand = new RelayCommand(Set);
            CancelCommand = new RelayCommand(Close);

            Server = _databaseModel.Server;
            DatabaseServer = _databaseModel.DatabaseServer;
            UserName = _databaseModel.UserName;
            Password = _databaseModel.Password;
            TableName = _databaseModel.TableName;
        }
        #endregion

        #region 3. 메서드
        private void Close()
        {
            _databaseViewModel.Close();
        }

        private void Set()
        {
            _databaseModel.Server = Server;
            _databaseModel.DatabaseServer = DatabaseServer;
            _databaseModel.UserName = UserName;
            _databaseModel.Password = Password;
            _databaseModel.TableName = TableName;
            _databaseModel.State = true;
            _databaseViewModel.Close();
        }
        #endregion

    }
}
