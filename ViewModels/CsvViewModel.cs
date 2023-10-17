using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using System.ComponentModel;

namespace DED_MonitoringSensor.ViewModels
{
    class CsvViewModel : ViewModelBase
    {
        private CsvService csvService;



        private bool csvState;
        public bool CsvState
        {
            get => csvState; set => SetProperty(ref csvState, value);
        }


        private string line;

        private RelayCommand _csvCommand;
        public RelayCommand CsvCommand
        {
            get => _csvCommand; set => SetProperty(ref _csvCommand, value);
        }

        public CsvViewModel(string line)
        {
            this.line = line;
            csvService = new CsvService();
            CsvCommand = new RelayCommand(Open);
            CsvState = false;
        }

        public void Open()
        {
            if (CsvState = csvService.CreateCsv(line))
            {
                CsvCommand = new RelayCommand(Close);
            }

        }
        public void Close()
        {
            CsvState = csvService.CloseCsv();
            CsvCommand = new RelayCommand(Open);
        }
        public void Add(string timer, string data)
        {
            csvService.AddCsv(timer, data);
        }


       


    }
}
