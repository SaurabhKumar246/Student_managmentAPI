using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Student_management_api.Services
{
    public class GetTasks
    {
        dbServices ds = new dbServices();
        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<responseData> getTasksForUser(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.eventID = req.eventID;

            try
            {
                // Check if the user exists by ID
                MySqlParameter[] myParams1 = new MySqlParameter[] {
                    new MySqlParameter("@ID", req.addInfo["ID"])
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

                // Retrieve tasks for the user
                MySqlParameter[] myParams = new MySqlParameter[] {
                    new MySqlParameter("@user_id", req.addInfo["ID"])
                };

                var query = @"SELECT task_name, description, priority, due_date, status 
                              FROM dashboard 
                              WHERE user_id = @user_id";

                var taskData = ds.executeSQL(query, myParams);

                if (taskData == null || taskData.Count == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 101; // no tasks found
                    resData.rData["Message"] = "No tasks found for this user.";
                }
                else
                {
                    resData.rData["rCode"] = 0;
                    resData.rStatus = 200;
                    resData.rData["tasks"] = taskData;
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
