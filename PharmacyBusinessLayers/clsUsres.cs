using PharmacyDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PharmacyBusinessLayers
{
    public class clsUsres
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ID { get; set; }
        public string UserRole { set; get; }
        public string FullName { set; get; }
        public string Email { set; get; }
        public long Phone { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string UserName { set; get; }
        public string PassWord { set; get; }

        public clsUsres()
        {
            this.ID = -1;
            this.FullName = "";
            this.UserRole = "";
            this.Email = "";
            this.Phone = 0;
            this.UserName = "";
            this.PassWord = "";
            this.DateOfBirth = DateTime.Now;
            Mode = enMode.AddNew;
        }

        private clsUsres(int ID, string UserRole, string FullName, string Email,long Phone, string UserName,string Password, DateTime DateOfBirth)
        {

            this.ID = ID;
            this.UserRole = UserRole;
            this.FullName = FullName;
            this.Email = Email;
            this.Phone =Phone;
            this.UserName = UserName;
            this.PassWord = Password;
            this.DateOfBirth = DateOfBirth;
            Mode = enMode.Update;

        }

        private bool _AddNewUser()

        {
            this.ID = clsPharmacyUsersDataAccess.AddNewUser(this.UserRole, this.FullName,
             this.DateOfBirth, this.Phone, this.Email, this.UserName, this.PassWord);
            return (this.ID != -1);

        }

        private bool _UpdateUser()
        {
            //call DataAccess Layer 

            return clsPharmacyUsersDataAccess.UpdateUser(this.ID, this.UserRole, this.FullName,
                this.DateOfBirth, this.Email, this.Phone, this.UserName, this.PassWord);

        }

        public static clsUsres FindUserByID(int ID)
        {

            string UserRole = "", FullName = "", Email = "", UserName = "", Password = "";
            DateTime DateOfBirth = DateTime.Now;
            long Phone = 0;
            if (clsPharmacyUsersDataAccess.GetUsersInfoByID(ID,ref UserRole,ref FullName,ref Email,ref DateOfBirth,ref Phone,ref UserName,ref Password))
            {

                return new clsUsres(ID, UserRole, FullName,Email, Phone, UserName, Password, DateOfBirth);
            }
            else
                return null;

        }

        public static DataTable GetAllUsers()
        {
            return clsPharmacyUsersDataAccess.GetAllUsers();
        }
        public static DataTable SearchForUsersByUserName(string UserName)
        {
            return clsPharmacyUsersDataAccess.SearchForUsersByUserName(UserName);
        }
        public static int GetUserIDByUserName(string UserName)
        {
            int ID = 0;
            DataTable dt = clsUsres.SearchForUsersByUserName(UserName);
            DataRow RecordRow = dt.Rows[0];
            ID =Int32.Parse((RecordRow["ID"].ToString()));
            return ID;
        }
        public static DataTable SearchForUserByID(string ID)
        {
            return clsPharmacyUsersDataAccess.SearchForUsersByID(ID);
        }

        public static bool DeleteUser(int ID)
        {

            return clsPharmacyUsersDataAccess.DeleteUser(ID);
        }

        public static bool IsUserExistByID(int ID)
        {

            return clsPharmacyUsersDataAccess.IsUserExistByID(ID);
        }
        public static bool IsUserExistByUserName(string UserName)
        {

            return clsPharmacyUsersDataAccess.IsUserExistByUserName(UserName);
        }
        public static bool IsUserExistByUserNameAndPassword(string UserName,string Password)
        {

            return clsPharmacyUsersDataAccess.IsUserExistByUserNameAndPassword(UserName,Password);
        }

        public static bool IsUserIsAdministrator(string UserName, string Password)
        {
            
                return clsPharmacyUsersDataAccess.IsUserAdmenistrator(UserName, Password);
        }

        public static int GetTotalPharmasist()
        {
            return clsPharmacyUsersDataAccess.CountTotalPharmasist();
        }
        public static int GetTotalAdministrator()
        {
            return clsPharmacyUsersDataAccess.CountTotalAdministrator();
        }
        public static bool IsUserIsPhrmacist(string UserName, string Password)
        {

            return !clsPharmacyUsersDataAccess.IsUserAdmenistrator(UserName, Password);
        }
        public static bool DeleteAllRecodsUsres()
        {
            return clsPharmacyUsersDataAccess.DeleteAllUsers();
        }
        public static bool IsNotExistAnyUser()
        {
            return clsPharmacyUsersDataAccess.IsEmptyUsersData();
        }
      
        public bool Save()

        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:

                    return _UpdateUser();
            }
            return false;

        }

    }
}
