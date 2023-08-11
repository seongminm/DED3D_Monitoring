namespace DED_MonitoringSensor.Services
{
    class GetDataService
    {
        public delegate void DelegateType();
        public DelegateType Method;

        private string stringData;
        public string StringData
        {
            get { return stringData; }
            set
            {
                stringData = value;
                if (Method != null)
                {
                    Method.Invoke();
                }
            }
        }
    }
}
