using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
internal class MySqlConnectionService
{
    private string connectionString;

    //public SlipsDAO()
    //{
    //    connectionString = "datasource=localhost;port=3306;username=root;password=www##2846;database=msbslips;";
    //}

    public List<Order> getAllSlips()
    {
        List<Order> returnThese = new List<Order>();

        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        MySqlCommand command = new MySqlCommand("SELECT * FROM slips", connection);

        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                //Slip slip = new Slip
                //{
                //    SlipId = reader.GetInt32("slipID").ToString(),
                //    StartTime = reader.GetDateTime("startTime"),
                //    EndTime = reader.GetDateTime("endTime"),
                //    DateTime = reader.GetDateTime("date"),
                //    CustomerName = reader.GetString("customerName"),
                //    AgentName = reader.GetString("agentName"),
                //    GuardName = reader.GetString("guardName"),
                //    FlightNumber = reader.GetString("flightNumber"),
                //};

                //returnThese.Add(slip);
            }
        }

        connection.Close();

        return returnThese;
    }

    //internal int AddOneSlip(Slip slip)
    //{
    //    List<Slip> returnThese = new List<Slip>();

    //    MySqlConnection connection = new MySqlConnection(connectionString);

    //    connection.Open();

    //    var dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    //    MySqlCommand command = new MySqlCommand("INSERT INTO `msbslips`.`slips` (`slipId`, `date`, `startTime`, `endTime`, `customerName`, `agentName`, `guardID`, `guardName`, `airline`, `flightNumber`) VALUES (@slipId, @date, @startTime, @endTime, @customerName, @agentName, @guardId, @guardName, @airline, @flightNumber)", connection);
    //    command.Parameters.AddWithValue("@slipId", slip.SlipId);
    //    command.Parameters.AddWithValue("@date", slip.DateTime.ToString("yyyy-MM-dd"));
    //    command.Parameters.AddWithValue("@startTime", slip.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
    //    command.Parameters.AddWithValue("@endTime", slip.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
    //    command.Parameters.AddWithValue("@customerName", slip.CustomerName);
    //    command.Parameters.AddWithValue("@agentName", slip.AgentName);
    //    command.Parameters.AddWithValue("@guardId", 0);
    //    command.Parameters.AddWithValue("@guardName", slip.GuardName);
    //    command.Parameters.AddWithValue("@airline", slip.Airline);
    //    command.Parameters.AddWithValue("@flightNumber", slip.FlightNumber);


    //    int newRows = command.ExecuteNonQuery();
    //    connection.Close();

    //    return newRows;
    //}

    //internal List<Slip> searchSlips(string selectedText, DateTime date)
    //{
    //    List<Slip> returnThese = new List<Slip>();

    //    MySqlConnection connection = new MySqlConnection(connectionString);

    //    connection.Open();

    //    var searchWildPhrase = "%" + selectedText + "%";
    //    var dateWildPhrase = "%" + date.ToString("yyyy-MM-dd") + "%";

    //    MySqlCommand command = new MySqlCommand();

    //    command.CommandText = "SELECT * FROM slips WHERE (airline LIKE @search AND date LIKE @date)";
    //    command.Parameters.AddWithValue("@search", searchWildPhrase);
    //    command.Parameters.AddWithValue("@date", dateWildPhrase);
    //    command.Connection = connection;

    //    using (MySqlDataReader reader = command.ExecuteReader())
    //    {
    //        while (reader.Read())
    //        {
    //            Slip slip = new Slip
    //            {
    //                SlipId = reader.GetInt32("slipID").ToString(),
    //                StartTime = reader.GetDateTime("startTime"),
    //                EndTime = reader.GetDateTime("endTime"),
    //                DateTime = reader.GetDateTime("date"),
    //                CustomerName = reader.GetString("customerName"),
    //                AgentName = reader.GetString("agentName"),
    //                GuardName = reader.GetString("guardName"),
    //                FlightNumber = reader.GetString("flightNumber"),
    //            };

    //            returnThese.Add(slip);
    //        }
    //    }

    //    connection.Close();

    //    return returnThese;
    //}

    internal List<JObject> getSlipsForGuard(int guardID)
    {
        List<JObject> returnThese = new List<JObject>();

        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        var searchWildPhrase = "%" + guardID + "%";
        //var dateWildPhrase = "%" + date.ToString("yyyy-MM-dd") + "%";

        MySqlCommand command = new MySqlCommand();

        command.CommandText = "SELECT * FROM slips WHERE guardId = @guardId)";
        command.Parameters.AddWithValue("@guardId", searchWildPhrase);
        //command.Parameters.AddWithValue("@date", dateWildPhrase);
        command.Connection = connection;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                JObject jObject = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    jObject.Add(reader.GetName(i), reader.GetValue(i).ToString());
                }
                returnThese.Add(jObject);
            }
        }

        connection.Close();

        return returnThese;
    }
}
