using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Student_management_api.Services
{
    public class deleteTask
    {
        private readonly dbServices ds = new dbServices(); // Ensure ds is available

        public async Task<responseData> RemoveTask(requestData req)
        {
            responseData resData = new responseData();
            resData.rData["rCode"] = 0;
            resData.eventID = req.eventID;

            try
            {
                // Check if the task exists by ID
                MySqlParameter[] myParams1 = new MySqlParameter[] {
                    new MySqlParameter("@ID", req.addInfo["ID"])
                };
                var sql1 = @"SELECT * FROM dashboard WHERE ID = @ID";
                var data1 = ds.ExecuteSQLName(sql1, myParams1);

                // If no task is found
                if (data1 == null || data1.Count == 0)
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 100;
                    resData.rData["Error"] = "Task not found";
                    return resData;
                }

                // Delete the task from the dashboard table
                var query = @"DELETE FROM dashboard WHERE ID = @ID";

                var deleteParams = new MySqlParameter[] {
                    new MySqlParameter("@ID", req.addInfo["ID"].ToString())
                };

                // Execute the delete command
                var deleteResult = ds.executeSQL(query, deleteParams);

                // Check if deleteResult is a non-empty list, meaning rows were affected
                if (deleteResult == null || deleteResult.Count == 0) // Check if the result is empty
                {
                    resData.rData["rCode"] = 1;
                    resData.rStatus = 100; 
                    resData.rData["Error"] = "Unable to delete the task. Please try again.";
                }
                else
                {
                    resData.rData["rCode"] = 0;
                    resData.rData["rMessage"] = "Task deleted successfully";
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
