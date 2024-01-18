using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDataAccessLayer
{
    public class clsMedicineDataAcces
    {
        public static bool GetMedicineInfoByID(int ID, ref string MidicineID, ref string MedicineName,

       ref string MedicineNumber, ref DateTime MedFuctoryDate, ref DateTime MedExpiredDate, ref int Quantity, ref double PricePerUnite)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = "SELECT * FROM Medicines WHERE ID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    
                    isFound = true;
                    MidicineID = (string)reader["MedicineID"];
                    MedicineName = (string)reader["MedicineName"];
                    MedicineNumber = (string)reader["MedicineNumber"];
                    MedFuctoryDate = (DateTime)reader["MedFuctoryDate"];
                    MedExpiredDate = (DateTime)reader["MedExpireDate"];
                    Quantity = (int)reader["Quantity"];
                    PricePerUnite = (double)reader["PricePerUnit"];
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

        public static int AddNewMedicale(string MedicaleID, string MedicalName,string MedicalNumber,DateTime MedExpireDate,DateTime MedFuctoryDate,int MedQuantity,double MedPricePerUnit)
        {
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string Query = @"INSERT INTO Medicines (MedicineID,MedicineName,MedicineNumber,MedFuctoryDate,MedExpireDate,Quantity,PricePerUnit)
                             VALUES (@MedicineID, @MedicineName, @MedicineNumber, @MedFuctoryDate, @MedExpireDate,@Quantity,@PricePerUnit);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@MedicineID", MedicaleID);
            command.Parameters.AddWithValue("@MedicineName", MedicalName);
            command.Parameters.AddWithValue("@MedicineNumber", MedicalNumber);
            command.Parameters.AddWithValue("@MedFuctoryDate", MedFuctoryDate);
            command.Parameters.AddWithValue("@MedExpireDate", MedExpireDate);
            command.Parameters.AddWithValue("@Quantity", MedQuantity);
            command.Parameters.AddWithValue("@PricePerUnit", MedPricePerUnit);

            try
            {
               
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
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
        public static int GetQuantityOfMedicineByMedID(string MedID)
        {
            int MedicineQuantity = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string Query = "select Quantity from Medicines where MedicineID=@MedicineID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@MedicineID", MedID);

            try
            {

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    MedicineQuantity = insertedID;
                }
            }
            catch (Exception ex)
            {
                MedicineQuantity = 0;

            }
            finally
            {
                connection.Close();
            }
            return MedicineQuantity;


        }
        public static bool UpdateQuantityOfMedicineByMedicineID(string MedicaleID,int NewQuantity)
        {
            int TotalQuantity = GetQuantityOfMedicineByMedID(MedicaleID) + NewQuantity;
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = @"Update  Medicines  
                            set Quantity = @Quantity
                          
                                where MedicineID = @MedicineID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MedicineID", MedicaleID);
            command.Parameters.AddWithValue("@Quantity", TotalQuantity);
            try
            {
                if (GetQuantityOfMedicineByMedID(MedicaleID)  <NewQuantity)
                {
                    return false;
                }
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

        public static int CountValidMedicine()
        {
            int TotalValidMedicine = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select Count(MedicineName)  from Medicines where MedExpireDate>=getdate()";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int TotalPharamasist))
                {

                    TotalValidMedicine = TotalPharamasist;
                }
            }
            catch (Exception e)
            {
                TotalValidMedicine = 0;
            }
            finally { connection.Close(); }
            return TotalValidMedicine;

        }
        public static DataTable GetAllValidMedicine()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select *  from Medicines where MedExpireDate>=getdate()";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception e)
            {
                
            }
            finally { connection.Close(); }
            return dt;

        }
        public static DataTable GetNamesOfValidMedicineAndQuantityGreatThanZero()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select MedicineName  from Medicines where MedExpireDate>=getdate() and Quantity>'0'";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception e)
            {

            }
            finally { connection.Close(); }
            return dt;

        }
        public static int CountExpiredMedicine()
        {
            int TotalExpiredMedicine = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select Count(MedicineName)  from Medicines where MedExpireDate<getdate()";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int TotalPharamasist))
                {

                    TotalExpiredMedicine = TotalPharamasist;
                }
            }
            catch (Exception e)
            {
                TotalExpiredMedicine = 0;
            }
            finally { connection.Close(); }
            return TotalExpiredMedicine;

        }
        public static DataTable GetAllExpireMedicine()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select *  from Medicines where MedExpireDate<getdate()";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception e)
            {

            }
            finally { connection.Close(); }
            return dt;

        }

        public static DataTable GetAllMedical()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Medicines";
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
        
        public static bool DeleteMedical(int ID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = @"Delete Medicines 
                                where ID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);

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
        public static DataTable SearchForMedicineByName(string medicineName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Medicines where MedicineName like'" + medicineName + "%'";
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
        public static DataTable FindAllValidMedicineNamesAndQtyGreatThanZero(string medicineName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT MedicineName FROM Medicines where MedExpireDate>=getdate() and Quantity>'0' and MedicineName like'" + medicineName + "%'";
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

        public static DataTable SearchForMedicineByID(int ID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Medicines where ID like'" + ID + "'";
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
        public static DataTable SearchForMedicineByNumberMedID(string NumMedID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "SELECT * FROM Medicines where MedicineNumber like'" + NumMedID + "%'";
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

        public static bool IsMedicineExistByID(int ID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = "select found=1 from Medicines where ID=@ID";

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

        public static bool UpdateMedicine(int ID, string MedicaleID, string MedicalName, string MedicalNumber, DateTime MedFuctoryDate, DateTime MedExpireDate, int MedQuantity, double MedPricePerUnit)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);

            string query = @"Update  Medicines  
                            set MedicineID = @MedicineID, 
                                MedicineName = @MedicineName,  
                             MedicineNumber = @MedicineNumber,
                                  MedFuctoryDate = @MedFuctoryDate,
                                  MedExpireDate = @MedExpireDate,
                                   Quantity = @Quantity,
                                PricePerUnit =@PricePerUnit
                                where ID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@MedicineID", MedicaleID);
            command.Parameters.AddWithValue("@MedicineName", MedicalName);
            command.Parameters.AddWithValue("@MedicineNumber", MedicalNumber);
            command.Parameters.AddWithValue("@MedFuctoryDate", MedFuctoryDate);
            command.Parameters.AddWithValue("@MedExpireDate", MedExpireDate);
            command.Parameters.AddWithValue("@Quantity", MedQuantity);
            command.Parameters.AddWithValue("@PricePerUnit", MedPricePerUnit);



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

        public static int TatalOfMedicene(int MedID)
        {
            int TotalMedicine = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSittings.ConnectionContactsDatebaseString);
            string query = @"
SELECT        Quantity
FROM            Medicines
where Medicines.ID=@MedicineID
";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@MedicineID", MedID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int TotalPharamasist))
                {

                    TotalMedicine = TotalPharamasist;
                }
            }
            catch (Exception e)
            {
                TotalMedicine = 0;
            }
            finally { connection.Close(); }
            return TotalMedicine;

        }
    }

}

