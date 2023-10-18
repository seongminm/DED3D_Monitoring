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


        public DatabaseViewModel()
        {
            MysqlState = false;
            MysqlCommand = new RelayCommand(OpenDatabase);
            databaseModel = new DatabaseModel();
        }

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

        public void Close()
        {
            databasePopView.Close();
        }

        public void CloseDatabase()
        {
            MysqlState = CloseDB();
            if (!MysqlState)
            {
                MysqlCommand = new RelayCommand(OpenDatabase);
            }
        }

        public void AddDatabase(string timer, string data)
        {
            AddData(timer, data);
        }


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
                    "`LaserPower_avg` VARCHAR(45) NULL, " +
                    "`LaserPower_std` VARCHAR(45) NULL, " +
                    "`visible1_avg` VARCHAR(45) NULL, " +
                    "`visible1_std` VARCHAR(45) NULL, " +
                    "`visible2_avg` VARCHAR(45) NULL, " +
                    "`visible2_std` VARCHAR(45) NULL, " +
                    "`visible3_avg` VARCHAR(45) NULL, " +
                    "`visible3_std` VARCHAR(45) NULL, " +
                    "`Sound_avg` VARCHAR(45) NULL, " +
                    "`Sound_std` VARCHAR(45) NULL, " +
                    "`Powder_avg` VARCHAR(45) NULL, " +
                    "`Powder_std` VARCHAR(45) NULL, " +
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

        public void AddData(string timer, string data)
        {
            string[] splitData = data.Split('/');

            string insertDataQuery = "INSERT INTO " + tableName + " (Time, LaserPower_avg, LaserPower_std, visible1_avg, visible1_std, visible2_avg, visible2_std, visible3_avg, visible3_std, Sound_avg, Sound_std, Powder_avg, Powder_std)" +
                        "VALUES (@Time, @LaserPower_avg, @LaserPower_std, @visible1_avg, @visible1_std, @visible2_avg, @visible2_std, @visible3_avg, @visible3_std, @Sound_avg, @Sound_std, @Powder_avg, @Powder_std);";
            MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);

            insertDataCommand.Parameters.AddWithValue("@Time", timer);
            insertDataCommand.Parameters.AddWithValue("@LaserPower_avg", splitData[0]);
            insertDataCommand.Parameters.AddWithValue("@LaserPower_std", splitData[1]);
            insertDataCommand.Parameters.AddWithValue("@visible1_avg", splitData[2]);
            insertDataCommand.Parameters.AddWithValue("@visible1_std", splitData[3]);
            insertDataCommand.Parameters.AddWithValue("@visible2_avg", splitData[4]);
            insertDataCommand.Parameters.AddWithValue("@visible2_std", splitData[5]);
            insertDataCommand.Parameters.AddWithValue("@visible3_avg", splitData[6]);
            insertDataCommand.Parameters.AddWithValue("@visible3_std", splitData[7]);
            insertDataCommand.Parameters.AddWithValue("@Sound_avg", splitData[8]);
            insertDataCommand.Parameters.AddWithValue("@Sound_std", splitData[9]);
            insertDataCommand.Parameters.AddWithValue("@Powder_avg", splitData[10]);
            insertDataCommand.Parameters.AddWithValue("@Powder_std", splitData[11]);
            insertDataCommand.ExecuteNonQuery();
        }

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

    }
}
