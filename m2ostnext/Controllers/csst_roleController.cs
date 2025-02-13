// Decompiled with JetBrains decompiler
// Type: m2ostnext.Controllers.csst_roleController
// Assembly: m2ostnext, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AB5479F-6947-434C-859E-D38C2141B485
// Assembly location: E:\Vidit\Personal\Carl Ambrose\M2OST Code\m2ostproduction_cms\bin\m2ostnext.dll

using m2ostnext.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using static Mysqlx.Notice.Warning.Types;

namespace m2ostnext.Controllers
{
  [UserFilter]
  public class csst_roleController : Controller
  {
    private db_m2ostEntities db = new db_m2ostEntities();

    public ActionResult Index()
    {
      int oid = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);
      tbl_organization tblOrganization = this.db.tbl_organization.Find(new object[1]
      {
        (object) oid
      });
      List<tbl_csst_role> list = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>) (t => t.id_organization == (int?) oid)).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>) (t => t.csst_role)).ToList<tbl_csst_role>();
      if (list.Count == 0)
      {
        this.db.tbl_csst_role.Add(new tbl_csst_role()
        {
          id_organization = new int?(oid),
          csst_role = "User",
          status = "A",
          updated_dated_time = new DateTime?(DateTime.Now)
        });
        this.db.SaveChanges();
        list = this.db.tbl_csst_role.Where<tbl_csst_role>((Expression<Func<tbl_csst_role, bool>>) (t => t.id_organization == (int?) oid)).OrderBy<tbl_csst_role, string>((Expression<Func<tbl_csst_role, string>>) (t => t.csst_role)).ToList<tbl_csst_role>();
      }
            AddDepaetment_Model departmentModel = new AddDepaetment_Model();
            var model = departmentModel.GetDepartment(oid);
            this.ViewData["department_list"] = model;

          
            var GetLevel = departmentModel.GetLevel();

            this.ViewData["Level_list"] = GetLevel;

            this.ViewData["csst_role"] = (object) list;
      this.ViewData["cscc_org"] = (object) tblOrganization;
      return (ActionResult) this.View();
    }

    public string editCsstRole(string id, string role,string departmentId, string levelId)
    {
            //tbl_csst_role tblCsstRole = this.db.tbl_csst_role.Find(new object[1]
            //{
            //  (object) Convert.ToInt32(id)
            //});
            //if (tblCsstRole != null)
            //{
            //  tblCsstRole.csst_role = role;
            //  tblCsstRole.updated_dated_time = new DateTime?(DateTime.Now);
            //  this.db.SaveChanges();
            //}
            //return "1";
            int int32 = Convert.ToInt32(((UserSession)this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

            try
            {
                string query = @"
        UPDATE tbl_csst_role 
        SET 
            csst_role = @csst_role,
            status = @status,
            updated_dated_time = @updated_dated_time,
            Id_department = @Id_department,
            Id_Level = @Id_Level,
            id_organization = @id_organization
        WHERE 
            ID_KPI = @id";

                // Use a MySQL connection
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@id", Convert.ToInt32(id)); // Fixed parameter for WHERE clause
                        command.Parameters.AddWithValue("@csst_role", role);
                        command.Parameters.AddWithValue("@status", "A");
                        command.Parameters.AddWithValue("@updated_dated_time", DateTime.Now);
                        command.Parameters.AddWithValue("@Id_department", departmentId);
                        command.Parameters.AddWithValue("@Id_Level", levelId);
                        command.Parameters.AddWithValue("@id_organization", int32); // Correctly assign organization ID

                        // Open the connection
                        connection.Open();

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the operation was successful
                        if (rowsAffected > 0)
                        {
                            return "1"; // Success
                        }
                        else
                        {
                            return "0"; // No rows updated
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                Console.WriteLine($"Error: {ex.Message}");
                return "-1"; // Indicate an error
            }
        }

    public string insertCsstRole(string role,string departmentId,string levelId)
    {
      int int32 = Convert.ToInt32(((UserSession) this.HttpContext.Session.Contents["UserSession"]).id_ORGANIZATION);

      //this.db.tbl_csst_role.Add(new tbl_csst_role()
      //{
      //  id_organization = new int?(int32),
      //  csst_role = role,
      //  status = "A",

      //  updated_dated_time = new DateTime?(DateTime.Now)
      //});
      //this.db.SaveChanges();
      //return "1";
            try
            {
     
                string query = @"INSERT INTO tbl_csst_role 
                         (id_organization, csst_role, status, updated_dated_time,Id_department,Id_Level) 
                         VALUES (@id_organization, @csst_role, @status, @updated_dated_time,@Id_department,@Id_Level)";

                // Use a MySQL connection
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@id_organization", int32);
                        command.Parameters.AddWithValue("@csst_role", role);
                        command.Parameters.AddWithValue("@status", "A");
                        command.Parameters.AddWithValue("@Id_department", departmentId);
                        command.Parameters.AddWithValue("@Id_Level", levelId);
                    
                        command.Parameters.AddWithValue("@updated_dated_time", DateTime.Now);

                        // Open the connection
                        connection.Open();

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the operation was successful
                        if (rowsAffected > 0)
                        {
                            return "1"; // Success
                        }
                        else
                        {
                            return "0"; // No rows inserted
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                Console.WriteLine($"Error: {ex.Message}");
                return "-1"; // Indicate an error
            }


        }

    public string deleteCsstRole(string id)
    {
      int ids = Convert.ToInt32(id);
      DbSet<tbl_content_role_mapping> contentRoleMapping = this.db.tbl_content_role_mapping;
      Expression<Func<tbl_content_role_mapping, bool>> predicate = (Expression<Func<tbl_content_role_mapping, bool>>) (t => t.id_csst_role == (int?) ids);
      foreach (tbl_content_role_mapping entity in contentRoleMapping.Where<tbl_content_role_mapping>(predicate).ToList<tbl_content_role_mapping>())
      {
        this.db.tbl_content_role_mapping.Remove(entity);
        this.db.SaveChanges();
      }
      this.db.tbl_csst_role.Remove(this.db.tbl_csst_role.Find(new object[1]
      {
        (object) ids
      }));
      this.db.SaveChanges();
      return "1";
    }
  }
}
