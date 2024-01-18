using PharmacyDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBusinessLayers
{
    public class clsMedicine
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ID { get; set; }
        public string MedicenID { set; get; }
        public string MedicineName { set; get; }
        public string MedicineNumber { set; get; }
        public DateTime MedcineExpireDate { set; get; }
        public DateTime MedicineFuctoryDate { set; get; }
        public int Quantity { set; get; }
        public Double PricePerUnite { set; get; }

        public clsMedicine()
        {
            this.ID = -1;
           // this.MedicenID = "";
            this.MedicineName = "";
            this.MedicineNumber = "";
            this.MedcineExpireDate = DateTime.Now;
            this.MedicineFuctoryDate = DateTime.Now;
            this.Quantity =0;
            this.PricePerUnite =0;
            Mode = enMode.AddNew;
        }

        private clsMedicine(int ID, string MedicineID, string MedicineName,
            string MedicineNumber,
            DateTime MedicineExpireDate,DateTime MedicineFuctoryDate,
            int MedQuantity,double MedPriceUnit)
        {

            this.ID = ID;
            this.MedicenID = MedicineID;
            this.MedicineName = MedicineName;
            this.MedicineNumber = MedicineNumber;
            this.MedcineExpireDate = MedicineExpireDate;
            this.MedicineFuctoryDate = MedicineFuctoryDate;
            this.Quantity = MedQuantity;
            this.PricePerUnite = MedPriceUnit;
            Mode = enMode.Update;

        }

        private bool _AddNewMedcine()

        {
            this.ID = clsMedicineDataAcces.AddNewMedicale(this.MedicenID, this.MedicineName,
            this.MedicineNumber,this.MedcineExpireDate,this.MedicineFuctoryDate,
            this.Quantity,this.PricePerUnite);
            return (this.ID != -1);

        }

   
        public static clsMedicine FindMedicineByID(int ID)
        {

            string MedicineID = "", MedicineName = "", MedicineNumber = "";
            int MedQuantity = 0;
            double MedPricePerUnite = 0;
            DateTime MedFuctoryDate = DateTime.Now, MedExpireDate=DateTime.Now;
            if (clsMedicineDataAcces.GetMedicineInfoByID(ID,ref MedicineID,ref MedicineName,ref MedicineNumber,ref MedFuctoryDate,ref MedExpireDate,ref MedQuantity,ref MedPricePerUnite))
            {
          

                return new clsMedicine(ID, MedicineID, MedicineName, MedicineNumber, MedExpireDate, MedFuctoryDate, MedQuantity, MedPricePerUnite); ;
            }
            else
                return null;

        }
        public bool Save()

        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewMedcine())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                    //case enMode.Update:

                    //    return _UpdateUser();
            }
            return false;

        }

        public static int CountValidMedicine()
        {
            return clsMedicineDataAcces.CountValidMedicine();
        }
        public static DataTable GetAllValidMedicine()
        {
            return clsMedicineDataAcces.GetAllValidMedicine();
        
        }
        public static DataTable  GetNamesOfValidMedicineAndQuantityGreatThanZero() 
        {
            return clsMedicineDataAcces.GetNamesOfValidMedicineAndQuantityGreatThanZero();
        }

        public static int CountExpiredMedicine()
        {
            return clsMedicineDataAcces.CountExpiredMedicine();
        }
        public static int GetQuantityOfMedicineByMedID(string MedID)
        {
            return clsMedicineDataAcces.GetQuantityOfMedicineByMedID(MedID);
        }
        public static bool UpdateQuantityOfMedicineByMedID(string MedID, int quantity)
        {
            return clsMedicineDataAcces.UpdateQuantityOfMedicineByMedicineID(MedID, quantity);
        }
        public static DataTable GetAllExpiredMedicine() 
        {
            return clsMedicineDataAcces.GetAllExpireMedicine();
        }
         public static DataTable GetAllMedical()
        {
           return clsMedicineDataAcces.GetAllMedical();

        }
        public static bool DeleteMedicine(int ID)
        {
            return clsMedicineDataAcces.DeleteMedical(ID);
        
        }
        public static DataTable SearchMedicineByMedicineName(string MedicineName)
        {
            return clsMedicineDataAcces.SearchForMedicineByName(MedicineName);
        }
        public static DataTable   FindAllValidMedicineNamesAndQtyGreatThanZero(string MedName)
        {
            return clsMedicineDataAcces.FindAllValidMedicineNamesAndQtyGreatThanZero(MedName);
        }

        public static DataTable SearchMedicalByID(int ID)
        {
            return clsMedicineDataAcces.SearchForMedicineByID(ID);
        }
        public static DataTable SearchMedicalByNumMedID(string NumMedID)
        {
            return clsMedicineDataAcces.SearchForMedicineByNumberMedID(NumMedID);
        }
        public static bool IsMedicineExistByID(int ID)
        {
            return clsMedicineDataAcces.IsMedicineExistByID(ID);
         

        }
        public static bool UpdateMedicine(int ID, string MedicaleID, string MedicalName, string MedicalNumber, DateTime MedExpireDate, DateTime MedFuctoryDate, int MedQuantity, double MedPricePerUnit)
        {
            return clsMedicineDataAcces.UpdateMedicine(ID, MedicaleID, MedicalName, MedicalNumber,
                MedExpireDate, MedFuctoryDate, MedQuantity, MedPricePerUnit);
        }

        public static int TatalOfMedicene(int MedID)
        {
            return clsMedicineDataAcces.TatalOfMedicene(MedID);
        }

    }
}
