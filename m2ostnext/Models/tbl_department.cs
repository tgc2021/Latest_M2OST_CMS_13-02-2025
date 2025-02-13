using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnext.Models
{
    public class tbl_department
    {
        public int Id_department { get; set; }
        public string Department_name { get; set; }
        public int Id_org { get; set; }
        public int Id_cms_user { get; set; }
        public string Status { get; set; }
        
        
    }

    public class tbl_level_master
    {
        public string Id_level { get; set; }
        public string Level_name { get; set; }
        public string Status { get; set; }
        public string Update_date { get; set; }
    }


    public class AddDepaetment_Model

    {

        private MySqlConnection conn;



        public AddDepaetment_Model() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);



        public string AddDepaetment(tbl_department temp)

        {

            string str = (string)null;

            try

            {

                if (temp.Id_department != 0)

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        // Open the connection
                        conn.Open();

                        command.CommandText = "UPDATE tbl_department  SET Department_name = @Department_name, Id_org = @Id_org,  Status = @Status  WHERE Id_department = " + temp.Id_department + "";

                        command.Parameters.AddWithValue("@Department_name", temp.Department_name);

                        command.Parameters.AddWithValue("@Id_org", temp.Id_org);

                       

                        command.Parameters.AddWithValue("@Status", temp.Status);


                        str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";
                    }

                }

                else

                {

                    using (MySqlCommand command = this.conn.CreateCommand())
                    {
                        // Open the connection
                        conn.Open();

                        command.CommandText = @"
            INSERT INTO tbl_department 
            (Department_name, Id_org, Status) 
            VALUES (@Department_name, @Id_org, @Status)";

                        command.Parameters.AddWithValue("@Department_name", temp.Department_name);
                        command.Parameters.AddWithValue("@Id_org", temp.Id_org);
                   
                        command.Parameters.AddWithValue("@Status", temp.Status);



                        str = command.ExecuteNonQuery() != 1 ? "FALSE" : "TRUE";

                    }
                }

            }

            catch (Exception ex)

            {


            }

            finally
            {
                // Ensure connection is closed
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return str;

        }



        public List<tbl_department> GetDepartmentData(int num)
        {

            List<tbl_department> DepartementList = new List<tbl_department>();



            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))

            {


                string query = @"SELECT * FROM tbl_department where Id_org = @Id_org and status ='A' order by Id_department asc";





                using (MySqlCommand command = new MySqlCommand(query, connection))

                {



                    command.Parameters.AddWithValue("@Id_org", num);



                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())
                    {

                        tbl_department department = new tbl_department();

                        department.Id_department = Convert.ToInt32(reader["Id_department"]);

                        department.Department_name = reader["Department_name"].ToString();

                        department.Id_org = Convert.ToInt32(reader["Id_org"]);

                        department.Status = reader["Status"].ToString();

                        DepartementList.Add(department);

                    }

                    reader.Close();

                }

            }

            return DepartementList;

        }


        public bool DeleteDepartment(int temp)
        {
            bool isDeleted = false;

            try
            {
                using (MySqlCommand command = this.conn.CreateCommand())
                {
                    conn.Open();

                    command.CommandText = "UPDATE tbl_department SET status = 'D' WHERE Id_department = " + temp;
                    int rowsAffectedCategory = command.ExecuteNonQuery();


                    int rowsAffected = command.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0; // True if at least one row was updated
                }



            }

            catch (Exception ex)

            {


            }

            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return isDeleted;

        }



        public IEnumerable<SelectListItem> GetDepartment(int Orgid)
        {
            var department = new List<SelectListItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(conn.ConnectionString))
                {
                    string query = "SELECT Id_department, Department_name FROM tbl_department WHERE Id_org = @OrgId AND status = 'A'";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrgId", Orgid);

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                department.Add(new SelectListItem
                                {
                                    Value = reader["Id_department"].ToString(),
                                    Text = reader["Department_name"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return department;
        }

        public IEnumerable<SelectListItem> GetLevel()
        {
            var department = new List<SelectListItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(conn.ConnectionString))
                {
                    string query = "SELECT Id_level, Level_name FROM tbl_level_master WHERE status = 'A'";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                      

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                department.Add(new SelectListItem
                                {
                                    Value = reader["Id_level"].ToString(),
                                    Text = reader["Level_name"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return department;
        }
    }

}