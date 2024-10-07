using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Student_managment_api.Services
{
    public class Registration
    {
         dbServices ds = new dbServices();

        IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<responseData> registration(requestData req)
        {
            responseData resData= new responseData();
            resData.rData["rCode"]=0;
        
        try 
            {
               string Email = req.addInfo["Email"].ToString();
               bool asEmail = IsValidEmail(Email);

               string aEmail;
               if(asEmail)
               {
                aEmail = "Email";

                MySqlParameter[] myParam = new MySqlParameter[] {
                new MySqlParameter("@Name", req.addInfo["Name"].ToString()),
                new MySqlParameter("@Email", Email),
                new MySqlParameter("@Password", req.addInfo["Password"].ToString()),
                new MySqlParameter("@Status", 1)
                };

             var exist=$"SELECT * FROM saurabh_database.user_data where Email=@Email";
             var exists = ds.executeSQL(exist, myParam);
            if(exists[0].Count()!=0)
            {
                 resData.rData["rMessage"] = "Already Exists data";
            }

            else {
                var query = $"insert into saurabh_database.user_data(Name,Email,Password,Status)values(@Name,@Email,@Password,@Status);";
                var data = ds.ExecuteInsertAndGetLastId(query, myParam);
                
            if (data!=null)
                {

                resData.rData["rCode"]=0;
                resData.rData["rMessage"]="Registration Successfully";

                }
                else
                {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Error in data Inserting... ";
                }

            }
               }
               else
               {
                    resData.rData["rCode"] = 1;
                    resData.rData["rMessage"] = "Enter valid Email!";
               }
            }
                 catch (Exception ex)
                 {
                resData.rData["rCode"]=1;
                resData.rData["rMessage"]=ex.Message;
            }
           return resData;
            
    }

     public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}