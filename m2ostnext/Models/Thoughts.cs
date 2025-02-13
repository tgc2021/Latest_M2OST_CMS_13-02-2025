using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnext.Models
{
    public class Thoughts
    {
        public int IdThoughts { get; set; } // id_thoughts
        public string ThoughtsName { get; set; } // thoughts_name
        public DateTime? StartDateTime { get; set; }
        public DateTime? ExpiredDate { get; set; }// expired_date
        public DateTime? CreatedDateTime { get; set; } // created_date_time
        public DateTime? UpdatedDateTime { get; set; } // updated_date_time
        public int IdOrganization { get; set; } // id_organization
        public int IdCmsUser { get; set; } // id_cms_user
        public string Status { get; set; } // status
    }

    public class Greeting
    {
        public int IdGreetings { get; set; } // id_greetings
        public string ImageAndGif { get; set; } // image_and_gif
        public DateTime CreatedDateTime { get; set; } // created_date_time
        public DateTime UpdatedDateTime { get; set; } // updated_date_time
        public string IdOrganization { get; set; } // id_organization
        public string IdCmsUser { get; set; } // id_cms_user
        public string Status { get; set; } // Status
    }

    public class ThoughtsModel

    {

        private MySqlConnection conn;



        public ThoughtsModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);



        public string AddThoughts(Thoughts model)
        {
            string result = "FALSE";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    conn.Open();

                    if (model.IdThoughts != 0)
                    {
                        // Update Query
                        string updateQuery = @"
                UPDATE tbl_thoughts
                SET thoughts_name = @ThoughtsName, 
                    start_date_time = @StartDateTime, 
                    expired_data = @ExpiredDate, 
                    id_organization = @IdOrganization, 
                    id_cms_user = @IdCmsUser, 
                    status = @Status
                   
                WHERE id_thoughts = @IdThoughts";

                        using (MySqlCommand command = new MySqlCommand(updateQuery, conn))
                        {
                            AddParameters(command, model);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert Query
                        string insertQuery = @"
                INSERT INTO tbl_thoughts 
                (thoughts_name, start_date_time, expired_data, id_organization, id_cms_user, status)
                VALUES 
                (@ThoughtsName, @StartDateTime, @ExpiredDate, @IdOrganization, @IdCmsUser, @Status)";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, conn))
                        {
                            AddParameters(command, model);
                            command.ExecuteNonQuery();
                        }
                    }

                    result = "TRUE";
                }
            }
            catch (Exception ex)
            {
                // Log the exception (replace with Serilog, etc.)
                Console.WriteLine($"Error in AddThoughts: {ex.Message}");
            }

            return result;
        }

        private void AddParameters(MySqlCommand command, Thoughts model)
        {
            command.Parameters.AddWithValue("@ThoughtsName", model.ThoughtsName);
            command.Parameters.AddWithValue("@StartDateTime", model.StartDateTime);
            command.Parameters.AddWithValue("@ExpiredDate", model.ExpiredDate);
            command.Parameters.AddWithValue("@IdOrganization", model.IdOrganization);
            command.Parameters.AddWithValue("@IdCmsUser", model.IdCmsUser);
            command.Parameters.AddWithValue("@Status", "A"); // Assuming 'A' is active
          

            if (model.IdThoughts != 0)
            {
                command.Parameters.AddWithValue("@IdThoughts", model.IdThoughts);
            }
        }




        public List<Thoughts> ThoughtList(int organizationId)
        {
            List<Thoughts> thoughtList = new List<Thoughts>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"
        SELECT * 
        FROM tbl_thoughts
        WHERE id_organization = @Id_organization
          AND status = 'A'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_organization", organizationId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Thoughts thoughts = new Thoughts
                            {
                                IdThoughts = reader["id_thoughts"] != DBNull.Value ? Convert.ToInt32(reader["id_thoughts"]) : 0,
                                IdOrganization = reader["id_organization"] != DBNull.Value ? Convert.ToInt32(reader["id_organization"]) : 0,
                                IdCmsUser = reader["id_cms_user"] != DBNull.Value ? Convert.ToInt32(reader["id_cms_user"]) : 0,
                                ThoughtsName = reader["thoughts_name"] != DBNull.Value ? reader["thoughts_name"].ToString() : string.Empty,
                                StartDateTime = reader["start_date_time"] != DBNull.Value ? Convert.ToDateTime(reader["start_date_time"]) : (DateTime?)null,
                                ExpiredDate = reader["expired_data"] != DBNull.Value ? Convert.ToDateTime(reader["expired_data"]) : (DateTime?)null, // Updated column name here
                                CreatedDateTime = reader["created_date_time"] != DBNull.Value ? Convert.ToDateTime(reader["created_date_time"]) : DateTime.MinValue,
                                UpdatedDateTime = reader["updated_date_time"] != DBNull.Value ? Convert.ToDateTime(reader["updated_date_time"]) : DateTime.MinValue,
                                Status = reader["status"] != DBNull.Value ? reader["status"].ToString() : string.Empty

                            };

                            thoughtList.Add(thoughts);
                        }
                    }
                }
            }

            return thoughtList;
        }



        public bool DeleteThough(int temp)
        {
            try
            {
                using (MySqlCommand command = this.conn.CreateCommand())
                {
                    // Open the connection
                    conn.Open();

                    // SQL query to update the status of the record
                    command.CommandText = "UPDATE tbl_thoughts SET status = 'D' WHERE id_thoughts = @idThought";
                    command.Parameters.AddWithValue("@idThought", temp);

                    // Execute the query
                    int rowsAffectedCategory = command.ExecuteNonQuery();

                    // Return true if the update was successful
                    return rowsAffectedCategory > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                // For example, using a logging framework or simply writing to a log file
                // Log.Error("Error deleting thought: " + ex.Message);
                return false;
            }
            finally
            {
                // Close the connection if it's open
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


        public List<Greeting> GreetingList(int organizationId)
        {
            List<Greeting> greetingList = new List<Greeting>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"
            SELECT * 
            FROM tbl_greetings
            WHERE id_organization = @Id_organization
              AND status = 'A'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_organization", organizationId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Greeting greeting = new Greeting();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);

                                // Map database column names to class property names
                                switch (columnName)
                                {
                                    case "id_greetings":
                                        if (!reader.IsDBNull(i)) greeting.IdGreetings = reader.GetInt32(i);
                                        break;
                                    case "image_and_gif":
                                        if (!reader.IsDBNull(i)) greeting.ImageAndGif = reader.GetString(i);
                                        break;
                                    case "created_date_time":
                                        if (!reader.IsDBNull(i)) greeting.CreatedDateTime = reader.GetDateTime(i);
                                        break;
                                    case "updated_date_time":
                                        if (!reader.IsDBNull(i)) greeting.UpdatedDateTime = reader.GetDateTime(i);
                                        break;
                                    case "id_organization":
                                        if (!reader.IsDBNull(i)) greeting.IdOrganization = reader.GetString(i);
                                        break;
                                    case "id_cms_user":
                                        if (!reader.IsDBNull(i)) greeting.IdCmsUser = reader.GetString(i);
                                        break;
                                    case "status":
                                        if (!reader.IsDBNull(i)) greeting.Status = reader.GetString(i);
                                        break;
                                }
                            }

                            greetingList.Add(greeting);
                        }
                    }
                }
            }

            return greetingList;
        
        
        }



    }

}