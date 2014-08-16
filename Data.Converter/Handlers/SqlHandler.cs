﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Convertion Services          *
*  Namespace : Empiria.Data.Convertion.Handlers                 Assembly : Empiria.Data.Convertion.dll       *
*  Type      : SqlHandler                                       Pattern  : Static Class                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Performs SQL Server data read and write bulk operations.                                      *
*                                                                                                            *
********************************* Copyright (c) 2007-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using System.Data.SqlClient;

namespace Empiria.Data.Convertion.Handlers {

  static internal class SqlHandler {

    #region Internal methods

    static internal int GetIntegerValue(string dataSourceName, string queryString) {
      object value = GetValue(dataSourceName, queryString);
      if (value != DBNull.Value) {
        return Convert.ToInt32(value);
      } else {
        return 0;
      }
    }

    static internal object GetValue(string dataSourceName, string queryString) {
      SqlConnection connection = GetDataSourceConnection(dataSourceName);
      SqlCommand command = new SqlCommand(queryString, connection);

      object returnValue = 0;
      try {
        command.CommandType = CommandType.Text;
        connection.Open();
        returnValue = command.ExecuteScalar();
      } catch (Exception exception) {
        throw new DataConvertionException(DataConvertionException.Msg.MySqlReadProblem, exception);
      } finally {
        connection.Dispose();
      }
      return returnValue;
    }

    static internal int Execute(string dataSourceName, string queryString) {
      SqlConnection connection = GetDataSourceConnection(dataSourceName);
      SqlCommand command = new SqlCommand(queryString, connection);

      int affectedRows = 0;
      try {
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 7200; // waits all needed time
        connection.Open();
        affectedRows = command.ExecuteNonQuery();
      } catch (Exception exception) {
        throw exception;
      } finally {
        connection.Dispose();
      }
      return affectedRows;
    }

    static internal DataTable GetDataTable(string dataSourceName, string queryString, string dataTableName) {
      SqlConnection connection = GetDataSourceConnection(dataSourceName);
      SqlCommand command = new SqlCommand(queryString, connection);

      DataTable dataTable = new DataTable(dataTableName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = CommandType.Text;
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
      } catch (Exception exception) {
        throw exception;
      } finally {
        connection.Dispose();
      }
      return dataTable;
    }

    static internal DataRow GetDataRow(string dataSourceName, string queryString, string tableName) {
      DataTable table = GetDataTable(dataSourceName, queryString, tableName);
      if (table.Rows.Count != 0) {
        return table.Rows[0];
      } else {
        return null;
      }
    }

    static internal string GetTableSchema(string dataSourceName, string queryString, string tableName, string xmlFilePath) {
      try {
        DataTable table = GetDataTable(dataSourceName, queryString, tableName);

        string fileName = xmlFilePath + tableName + ".xml.txt";

        table.WriteXmlSchema(fileName);

        return fileName;
      } catch (Exception exception) {
        throw new DataConvertionException(DataConvertionException.Msg.MySqlReadProblem, exception);
      }
    }

    static internal int UpdateTable(string dataSourceName, DataTable sourceTable, DataTable targetTable,
                                    string targetEmptyQueryString, int startRowIndex, int endRowIndex) {
      int affectedRows = 0;
      using (SqlConnection connection = GetDataSourceConnection(dataSourceName)) {
        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        dataAdapter.ContinueUpdateOnError = true;
        dataAdapter.SelectCommand = new SqlCommand(targetEmptyQueryString, connection, transaction);
        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

        dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
        dataAdapter.InsertCommand.Transaction = transaction;
        dataAdapter.Fill(targetTable);
        DataConvertionEngine.GetInstance().TranslateData(sourceTable, targetTable, startRowIndex, endRowIndex);
        affectedRows = dataAdapter.Update(targetTable);
        transaction.Commit();
      }
      return affectedRows;
    }

    #endregion Internal methods

    #region Private methods

    static private SqlConnection GetDataSourceConnection(string dataSourceName) {
      string connectionString = ConfigurationData.GetString("DS." + dataSourceName);

      return new SqlConnection(connectionString);
    }

    #endregion Private methods

  } // class SqlHandler

} // namespace Empiria.Data.Convertion.Handlers