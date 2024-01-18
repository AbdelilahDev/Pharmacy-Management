using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDataAccessLayer
{
   public class clsPharmacyUsersDataAccess
    {
        public static bool GetUsersInfoByID(int ID, ref string UserRole, ref string FullName,
                   
           ref string Email ,ref DateTime DateOfBirth, ref long MobilePhone, ref string UserName,ref string Password)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = "SELECT * FROM Users WHERE ID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {


                        isFound = true;
                       UserRole = (string)reader["UserRole"];
                       FullName = (string)reader["FullName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    MobilePhone = (long)reader["Mobile"];
                       UserName = (string)reader["UserName"];
                       Password = (string)reader["Pass"];
                          Email = (string)reader["Email"];

                }
                else
                {

                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewUser(string UserRole, string FullName, DateTime DateOfBirth,long Mobile,string Email ,string UserName,string Password)
        {
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string Query = @"INSERT INTO Users (UserRole,FullName,DateOfBirth,Mobile,UserName,Pass,Email)
                             VALUES (@UserRole, @FullName, @DateOfBirth, @Mobile, @UserName,@Pass,@Email);
                             SELECT SCOPE_IDENTITY();";
            Console.WriteLine("Add New");
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserRole", UserRole);
            command.Parameters.AddWithValue("@FullName", FullName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Mobile",Mobile);
            command.Parameters.AddWithValue("@UserName",UserName);
            command.Parameters.AddWithValue("@Pass",Password);
            command.Parameters.AddWithValue("@Email",Email);

            try
            {
                Console.WriteLine("Inside Try");
                connection.Open();
                object result = command.ExecuteScalar();
                
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    Console.WriteLine("Inside Try");
                    UserID = insertedID;
                }
            }
            catch (Exception ex)
            {
                UserID = -1;

            }
            finally
            {
                connection.Close();
            }
            return UserID;


        }

        public static bool UpdateUser(int ID, string UserRole, string FullName, DateTime DateOfBirth,string Email ,long Mobile, string UserName,
            string Password)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = @"Update  Users  
                            set UserRole = @UserRole, 
                                FullName = @FullName,  
                             DateOfBirth = @DateOfBirth,
                                  Mobile = @Mobile,
                                   Email = @Email,
                                UserName =@UserName,
                                     Pass=@Password
                                where ID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@FullName",FullName);
            command.Parameters.AddWithValue("@UserRole",UserRole);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Mobile",Mobile);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@UserName",UserName);
            command.Parameters.AddWithValue("@Password", Password);



            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteUser(int UserID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = @"Delete Users 
                                where ID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }
        public static bool DeleteAllUsers()
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = @"Delete from Users";

            SqlCommand command = new SqlCommand(query, connection);


            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }
        public static bool IsUserExistByID(int ID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select found=1 from Users where ID=@ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                IsFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception e)
            {


            }
            finally { connection.Close(); }

            return IsFound;

        }
        public static bool IsUserExistByUserName(string UserName)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select found=1 from Users where UserName=@UserName";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                IsFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception e)
            {


            }
            finally { connection.Close(); }

            return IsFound;

        }

        public static bool IsUserExistByUserNameAndPassword(string UserName,string Password)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select found=1 from Users where UserName=@UserName and Pass=@Password";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                IsFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception e)
            {


            }
            finally { connection.Close(); }

            return IsFound;

        }

        public static bool IsUserAdmenistrator(string UserName, string Password)
        {
            
                bool IsAdministrator = false;
                SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
                string query = "select found=1 from Users where UserName=@UserName and Pass=@Password and UserRole='Administrator'";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);


                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    IsAdministrator = reader.HasRows;
                    reader.Close();
                }
                catch (Exception e)
                {


                }
                finally { connection.Close(); }

                return IsAdministrator;

          
        }
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Users";
            SqlCommand command = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows)
                {
                    dt.Load(read);
                }
            }
            catch (Exception e)
            {

            }
            finally { con.Close(); }
            return dt;

        }

        public static DataTable SearchForUsersByUserName(string UserName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Users where UserName like'"+ UserName +"%'";
            SqlCommand command = new SqlCommand(query, con);
     

            try
            {
                con.Open();
                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows)
                {
                    dt.Load(read);
                }
            }
            catch (Exception e)
            {

            }
            finally { con.Close(); }
            return dt;

        }
        public static DataTable SearchForUsersByID(string ID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Users where ID like'" + ID + "'";
            SqlCommand command = new SqlCommand(query, con);


            try
            {
                con.Open();
                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows)
                {
                    dt.Load(read);
                }
            }
            catch (Exception e)
            {

            }
            finally { con.Close(); }
            return dt;

        }
        public static int CountTotalPharmasist()
        {
            int TotalPhramasist = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select Count(UserRole)  from Users where UserRole=@UserRole";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserRole", "Pharmasist");
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int TotalPharamasist))
                {
                  
                    TotalPhramasist = TotalPharamasist;
                }
            }
            catch (Exception e)
            {

            }
            finally { connection.Close(); }
            return TotalPhramasist;

        }
        public static int CountTotalAdministrator()
        {
            int TotalPhramasist = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select Count(UserRole)  from Users where UserRole=@UserRole";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserRole", "Administrator");
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int TotalPharamasist))
                {
                   
                    TotalPhramasist = TotalPharamasist;
                }
            }
            catch (Exception e)
            {

            }
            finally { connection.Close(); }
            return TotalPhramasist;

        }

        public static bool IsEmptyUsersData() 
        {
            return (GetAllUsers().Rows.Count == 0);
        }
    }
}
