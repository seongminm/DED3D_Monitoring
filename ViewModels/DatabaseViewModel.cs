﻿using DED_MonitoringSensor.Models;
using DED_MonitoringSensor.Services;
using DED_MonitoringSensor.ViewModels.Command;
using DED_MonitoringSensor.Views.PopView;
using MonitoringSensor.Views.PopView;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Windows;

namespace DED_MonitoringSensor.ViewModels
{
    class DatabaseViewModel : ViewModelBase
    {
        #region 1.초기 환경 변수 및 프로퍼티

        private MySqlConnection connection;
        private string tableName;

        DatabasePopView databasePopView;
        DatabasePopViewModel databasePopViewModel;
        DatabaseModel databaseModel;


        private bool mysqlState;
        public bool MysqlState
        {
            get => mysqlState; set => SetProperty(ref mysqlState, value);
        }


        private RelayCommand mysqlCommand;
        public RelayCommand MysqlCommand
        {
            get => mysqlCommand; set => SetProperty(ref mysqlCommand, value);
        }

        #endregion

        #region 2. 생성자
        public DatabaseViewModel()
        {
            MysqlState = false;
            MysqlCommand = new RelayCommand(OpenDatabase);
            databaseModel = new DatabaseModel();
        }
        #endregion

        #region 3. 메서드
        /// <summary>
        /// 데이터베이스 인터페이스 열기
        /// </summary>
        public void OpenDatabase()
        {
            databasePopView = new DatabasePopView();
            databasePopView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            databaseModel.State = false;
            databaseModel.TableName = DateTime.Now.ToString("yyMMdd_HHmm");
            databasePopViewModel = new DatabasePopViewModel(databaseModel, this);
            databasePopView.DataContext = databasePopViewModel;
            databasePopView.ShowDialog();
            if (databaseModel.State == false)
            {
                Close();
                return;
            }

            MysqlState = OpenDatabase(databaseModel.UserName, databaseModel.Password, databaseModel.Server, databaseModel.DatabaseServer, databaseModel.TableName);
            if (MysqlState)
            {
                MysqlCommand = new RelayCommand(CloseDatabase);
            }
        }

        /// <summary>
        /// 데이터베이스 인터페이스 닫기
        /// </summary>
        public void Close()
        {
            databasePopView.Close();
        }

        /// <summary>
        /// 데이터 베이스 연결 해제
        /// </summary>
        public void CloseDatabase()
        {
            MysqlState = CloseDB();
            if (!MysqlState)
            {
                MysqlCommand = new RelayCommand(OpenDatabase);
            }
        }

        /// <summary>
        /// 데이터베이스 데이터 추가
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="dataArray"></param>
        public void AddDatabase(string timer, double[] dataArray)
        {
            string insertDataQuery = "INSERT INTO " + tableName + " (Time, Visible, IRFilter, BlueFilter, Powder980, Sound, LaserPower, Powder780, Powder650)" +
                        "VALUES (@Time, @Visible, @IRFilter, @BlueFilter, @Powder980, @Sound, @LaserPower, @Powder780, @Powder650);";
            MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);

            insertDataCommand.Parameters.AddWithValue("@Time", timer);
            insertDataCommand.Parameters.AddWithValue("@Visible", dataArray[0]);
            insertDataCommand.Parameters.AddWithValue("@IRFilter", dataArray[1]);
            insertDataCommand.Parameters.AddWithValue("@BlueFilter", dataArray[2]);
            insertDataCommand.Parameters.AddWithValue("@Powder980", dataArray[3]);
            insertDataCommand.Parameters.AddWithValue("@Sound", dataArray[4]);
            insertDataCommand.Parameters.AddWithValue("@LaserPower", dataArray[5]);
            insertDataCommand.Parameters.AddWithValue("@Powder780", dataArray[6]);
            insertDataCommand.Parameters.AddWithValue("@Powder650", dataArray[7]);
            insertDataCommand.ExecuteNonQuery();

        }

        /// <summary>
        /// 인터페이스에 작성한 정보를 통해 데이터베이스 연결
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_pw"></param>
        /// <param name="_server"></param>
        /// <param name="_database"></param>
        /// <param name="_tableName"></param>
        /// <returns></returns>
        public bool OpenDatabase(string _userName, string _pw, string _server, string _database, string _tableName)
        {
            try
            {
                string uid = _userName;
                string password = _pw;
                string server = _server;
                string database = _database;
                string defaultTableName = DateTime.Now.ToString("yyMMdd_HHmm");
                tableName = _tableName;
                string connectionString = $"server={server};database={database};uid={uid};password={password};";
                connection = new MySqlConnection(connectionString);
                string createTableQuery = "CREATE TABLE IF NOT EXISTS `" + tableName + "` (`Pk` INT NOT NULL AUTO_INCREMENT, " +
                    "`Time` VARCHAR(45) NULL, " +
                    "`Visible` VARCHAR(45) NULL, " +
                    "`IRFilter` VARCHAR(45) NULL, " +
                    "`BlueFilter` VARCHAR(45) NULL, " +
                    "`Powder980` VARCHAR(45) NULL, " +
                    "`Sound` VARCHAR(45) NULL, " +
                    "`LaserPower` VARCHAR(45) NULL, " +
                    "`Powder780` VARCHAR(45) NULL, " +
                    "`Powder650` VARCHAR(45) NULL, " +
                    "PRIMARY KEY (`Pk`));";

                connection.Open();
                MySqlCommand createTableCommand = new MySqlCommand(createTableQuery, connection);
                createTableCommand.ExecuteNonQuery();

                MessageBox.Show(tableName + " Connect !");
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 데이터베이스 연결 해제
        /// </summary>
        /// <returns></returns>
        public bool CloseDB()
        {
            try
            {
                connection.Close();
                MessageBox.Show(tableName + " Disconnect !");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return true;
            }

        }
        #endregion
    }
}
