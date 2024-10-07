using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Student_managment_api.Services
{
    public class AddTask
    {
        dbServices ds = new dbServices();

        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<responseData> addTask(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.eventID = req.eventID;
            try
            {
                // Check if the user exists by ID
                MySqlParameter[] myParams1 = new MySqlParameter[] {
                    new MySqlParameter("@ID",req.addInfo["ID"])
                };
                var sql1 = @"SELECT * FROM saurabh_database.user_data WHERE ID = @ID";
                var data1 = ds.ExecuteSQLName(sql1, myParams1);

                // If no user is found
                if (data1 == null || data1.Count == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 100;
                    resData.rData["Error"] = "User not found";
                    return resData;
                }

                // Insert task into the dashboard table
                MySqlParameter[] myParams = new MySqlParameter[]
                {
                    new MySqlParameter("@task_name", req.addInfo["task_name"].ToString()),
                    new MySqlParameter("@description", req.addInfo["description"].ToString()),
                    new MySqlParameter("@due_date",req.addInfo["due_date"].ToString()),
                    new MySqlParameter("@priority", req.addInfo["priority"].ToString()),
                    new MySqlParameter("@status", 1),
                    new MySqlParameter("@user_id", req.addInfo["ID"])
                };

                var query = @"INSERT INTO dashboard (task_name, description, priority, due_date, status, user_id) 
                              VALUES (@task_name, @description, @priority, @due_date, @status, @user_id);";
                var dbData = ds.executeSQL(query, myParams);

                if (dbData == null)
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 100; // database error, caught at the app level
                    resData.rData["Error"] = "Payload Is Incorrect. Check all fields.";
                }
                else
                {
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Task Added Successfully";
                }
            }
            catch (Exception ex)
            {
                resData.rData["rCode"] = 105;
                resData.rData["rData"] = errors.err[105] + Environment.NewLine + ex.Message;
            }
            return resData;
        }
    }
}
