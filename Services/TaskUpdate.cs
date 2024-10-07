using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Student_management_api.Services
{
    public class TaskUpdate
    {
        private readonly dbServices ds = new dbServices(); // Declare ds here

        public async Task<responseData> updateTask(requestData req)
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
                if (data1 == null || data1.Count == 0) // Ensure that data1 returns a collection
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 100;
                    resData.rData["Error"] = "User not found";
                    return resData;
                }

                // Update task in the dashboard table
                MySqlParameter[] myParams = new MySqlParameter[] {
                    new MySqlParameter("@ID", req.addInfo["ID"].ToString()),
                    new MySqlParameter("@task_name", req.addInfo["task_name"].ToString()),
                    new MySqlParameter("@description", req.addInfo["description"].ToString()),
                    new MySqlParameter("@due_date", req.addInfo["due_date"].ToString()),
                    new MySqlParameter("@priority", req.addInfo["priority"].ToString()),
                };

                var query = @"UPDATE dashboard 
                              SET task_name = @task_name, description = @description, priority = @priority, 
                                  due_date = @due_date 
                              WHERE ID = @ID";


                var taskdata = ds.executeSQL(query, myParams);


                if (taskdata == null || !taskdata.Any())
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 100; 
                    resData.rData["Error"] = "Unable to update the task. Check all fields.";
                }
                else
                {
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Task updated successfully";
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
