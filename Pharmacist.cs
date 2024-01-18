using PharmacyBusinessLayers;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Pharmacy_Management.DGVPrinterHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Pharmacy_Management
{
    public partial class Pharmacist : Form
    {
        int IDSelectedInDataGridView = 0;
        int NumberOfRowsInDGVSellMed = 0;
        double TotalBellInSellPanel = 0;
        string MedicineIDSelectedInDGVSellPanel = "";
        int TotalOfUnitsSelectedInSellPanel = 0;
        int NumberOfRowSelectedInSellPanel = -1;
      
        string CurrentUserName;
        int CurrentUserID;
        public Pharmacist()
        {
            InitializeComponent();
        }
        public Pharmacist(string UserName)
        {
            InitializeComponent();
            HideAllPanles();
            CurrentUserName = UserName;
            lblPharmasistUserName.Text = CurrentUserName;
            CurrentUserID = clsUsres.GetUserIDByUserName(UserName);
        }
        void FindAllMedicineNamesInSellPanelByNames(string MedNames)
        {
            DataTable dt = clsMedicine.FindAllValidMedicineNamesAndQtyGreatThanZero(MedNames);
            foreach (DataRow dr in dt.Rows)
            {
                LBSellMedNames.Items.Add(dr[0].ToString());
            }


        }
        void FillListBoxInSellPanelWithNamesOfValidMedicine()
        {
            DataTable dt=clsMedicine.GetNamesOfValidMedicineAndQuantityGreatThanZero();
            foreach (DataRow dr in dt.Rows)
            {
                LBSellMedNames.Items.Add(dr[0].ToString());
            }
        }
        void FillDGVInSellPanel()
        {
           NumberOfRowsInDGVSellMed =DGVInSellPanel.Rows.Add();
            DGVInSellPanel.Rows[NumberOfRowsInDGVSellMed].Cells[0].Value = txtBMedIdInSellPanel.Text;
            DGVInSellPanel.Rows[NumberOfRowsInDGVSellMed].Cells[1].Value = txtBMedNameInSellPanel.Text;
            DGVInSellPanel.Rows[NumberOfRowsInDGVSellMed].Cells[2].Value = DTPExpiredDateInSellPanel.Text;
            DGVInSellPanel.Rows[NumberOfRowsInDGVSellMed].Cells[3].Value = txtBPricePerUnitInSellPanell.Text;
            DGVInSellPanel.Rows[NumberOfRowsInDGVSellMed].Cells[4].Value = txtBNumberOfUnitsInSellPanel.Text;
            DGVInSellPanel.Rows[NumberOfRowsInDGVSellMed].Cells[5].Value = txtBTotalPriceInSellPanel.Text;
        }
        int GetNewTotalQuantityToUpdate()
        {
            int TotalQty = 0,AvaliableQty,NewQty;
            if (txtBAddQty.Text == "")
                txtBAddQty.Text = "0";
            AvaliableQty =int.Parse (txtBAvailableQty.Text);
            NewQty = Int32.Parse(txtBAddQty.Text);
            TotalQty = AvaliableQty + NewQty;
            return TotalQty;

        }
        bool IsAnyTextBoxeInPanelUpdateEmpty()
        {
        return(txtBSearchMedIDUpdate.Text==""||txtBMedNumberUpdate.Text==""||txtBMedNameUpdate.Text==""||txtBAvailableQty.Text==""||txtBPricePerUnitUpdate.Text=="");
           
        }
        void ShowAllMedicinesInDataGridView()
        {
            if(PanelViewMedical.Visible==true)
            DGVMedicales.DataSource = clsMedicine.GetAllMedical();
            if (panelCheckMedValidity.Visible == true)
                DGVCheckValidMedicine.DataSource = clsMedicine.GetAllMedical();



        }
        void FullChatInDashbord()
        {
            int ValidMedicine = clsMedicine.CountValidMedicine();
            int ExpiredMedicine = clsMedicine.CountExpiredMedicine();
            this.chartPharmacistDashbord.Series["Valid Medicine"].Points.AddXY("Medicine Validity Chart", ValidMedicine);

            this.chartPharmacistDashbord.Series["Expired Medicine"].Points.AddXY("Medicine Validity Chart",ExpiredMedicine);
        }
        void GetAllValidMedicine()
        {
            DGVCheckValidMedicine.DataSource = clsMedicine.GetAllValidMedicine();
        
        }
        void GetAllExpiredMedicine()
        {
            DGVCheckValidMedicine.DataSource = clsMedicine.GetAllExpiredMedicine();

        }
        int CalculateTotalUnitsInSellMedicine(string MedId)
        {
            int TotalUnitsInStock =clsMedicine.GetQuantityOfMedicineByMedID(MedId);
           
                if (txtBNumberOfUnitsInSellPanel.Text == "")
                {
                    TotalUnitsInStock -=0;

                }
                else
                {
                    TotalUnitsInStock -=  Int32.Parse(txtBNumberOfUnitsInSellPanel.Text);

                }

            
            return TotalUnitsInStock;

        }
        double CalculateTotalPriceInSellMedicine()
        {
            double TotalPrice = 0;
            if (txtBPricePerUnitInSellPanell.Text != "")
            {
                if (txtBNumberOfUnitsInSellPanel.Text == "")
                {
                    TotalPrice = Double.Parse(txtBPricePerUnitInSellPanell.Text) * 1;

                }
                else
                {
                    TotalPrice = Double.Parse(txtBPricePerUnitInSellPanell.Text) * Int32.Parse(txtBNumberOfUnitsInSellPanel.Text);

                }
               
            }
            return TotalPrice;

        }
        clsMedicine ReadMedicalInformation()
         {
            clsMedicine Medicine = new clsMedicine();
            if (!(txtBMedicineID.Text == "" || txtBMedicineName.Text == "" || txtBNumber.Text == ""
               || DTPExpireDate.Value == null || DTPManufactringDate == null || txtBQuantity.Text == "" || txtBPricePerUnit.Text == "")) 

            {
                Medicine.MedicenID = txtBMedicineID.Text;
                Medicine.MedicineName = txtBMedicineName.Text;
                Medicine.MedicineNumber = txtBNumber.Text;
                Medicine.MedcineExpireDate = DTPExpireDate.Value;
                Medicine.MedicineFuctoryDate = DTPManufactringDate.Value;
                Medicine.Quantity = Int32.Parse((txtBQuantity.Text));
                Medicine.PricePerUnite = Double.Parse(txtBPricePerUnit.Text);
                return Medicine;
            }
            else 
              return null;
         
        }
        void ChangeAllButtonsBackgroundAndSizeInControlPanel()
        { 
            btnLogout.BackColor = Color.BlueViolet;    
            btnDashbord.BackColor = Color.BlueViolet;
            btnDashbord.Size = new Size(260, 41);
            btnAddMedicine.BackColor = Color.BlueViolet;
            btnAddMedicine.Size = new Size(260, 41);
            btnViewMedicin.BackColor = Color.BlueViolet;
            btnViewMedicin.Size = new Size(260, 41);
            btnModifieMedicine.BackColor = Color.BlueViolet;
            btnModifieMedicine.Size = new Size(260, 41);
            btnMedicinValidityCheck.BackColor = Color.BlueViolet;
            btnMedicinValidityCheck.Size = new Size(260, 41);
            btnSellMedicine.BackColor = Color.BlueViolet;
            btnSellMedicine.Size = new Size(260, 41);
        }
        void HideAllPanles()
        {
            ClearTextBoxesAddMedcine();
            ClearTextBoxesInUpdatePanel();
            panelPharmacistDashbord.Visible = false;
            panelAddMedicine.Visible = false;
            PanelViewMedical.Visible = false;
            panelUpdateMedicine.Visible = false;
            panelCheckMedValidity.Visible = false;
            panelSellMedicine.Visible = false;

        
        }
        void ClearTextBoxesAddMedcine()
        {
            txtBMedicineID.Clear();
            txtBMedicineName.Clear();
            txtBNumber.Clear();
            txtBQuantity.Clear();
            txtBPricePerUnit.Clear();

        }
        void ClearTextBoxesInUpdatePanel()
        {
            txtBMedNumberUpdate.Clear();
            txtBSearchMedIDUpdate.Clear();
            txtBMedNameUpdate.Clear();
            DTPExpireDateUpdate.Value=DateTime.Now;
            DTPManfDateUpdate.Value = DateTime.Now; ;
            txtBAvailableQty.Clear();
            txtBAddQty.Clear();
            txtBPricePerUnitUpdate.Clear();
            txtBMedIDUpdate.Clear();
            TBTotalMedicens.Clear();

        }
        void ClearTextBoxesInSellPanel()
        {
            txtBMedIdInSellPanel.Clear();
            txtBMedNameInSellPanel.Clear();
            txtBNumberOfUnitsInSellPanel.Clear();
            txtBPricePerUnitInSellPanell.Clear();
            txtBTotalPriceInSellPanel.Clear();
        }
        void FillTextBoxesInSellPanel(string MedName)
        {
            if (MedName != "")
            {
                DataTable dt = clsMedicine.SearchMedicineByMedicineName(MedName);
                
                DataRow dr = dt.Rows[0];
                txtBMedIdInSellPanel.Text = dr[1].ToString();
                txtBMedNameInSellPanel.Text = dr[2].ToString();
                DTPExpiredDateInSellPanel.Text = dr[5].ToString();
                txtBPricePerUnitInSellPanell.Text = dr[7].ToString();
                TBTotalMedicens.Text = dr[6].ToString();

            }
            else 
            {
                txtBMedIdInSellPanel.Text = "";
                txtBMedNameInSellPanel.Text = "";
                DTPExpiredDateInSellPanel.Value =DateTime.Now;
                txtBPricePerUnitInSellPanell.Text = "";
                txtBNumberOfUnitsInSellPanel.Text = "";

            }
            }
        bool IsQuantityFound(string MedID,int NewQuantity)
        {
            int TotalQuantity = clsMedicine.GetQuantityOfMedicineByMedID(MedID);
            return TotalQuantity > NewQuantity;
        }
        clsMedicine GetMedicineInfoByID(int ID)
        {
            clsMedicine Medicine = clsMedicine.FindMedicineByID(ID);
           
            return Medicine;
        }
        private void btnDashbord_Click(object sender, EventArgs e)
        {     
            FullChatInDashbord();
            ChangeAllButtonsBackgroundAndSizeInControlPanel();
            HideAllPanles();
            panelPharmacistDashbord.Location = new Point(275, 0);
            panelPharmacistDashbord.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            bt.Size = new Size(264,55);
            panelPharmacistDashbord.Visible = true;
        }

        private void Pharmacist_Load(object sender, EventArgs e)
        {
            btnDashbord.PerformClick();

        }

        private void btnAddMedicine_Click(object sender, EventArgs e)
        {
            HideAllPanles();
            ChangeAllButtonsBackgroundAndSizeInControlPanel();
            panelAddMedicine.Location = new Point(275, 0);
            panelAddMedicine.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            bt.Size = new Size(264, 55);
            panelAddMedicine.Visible = true;
        }

        private void btnAddNewMedicine_Click(object sender, EventArgs e)
        {
            clsMedicine Med = ReadMedicalInformation();
            if (!(Med == null))
            {
                if (Med.Save())
                {

                    MessageBox.Show("Medicine Saved Seccessfully", "ADD Medicine", MessageBoxButtons.OK);
                    ClearTextBoxesAddMedcine();
                }
                else
                {
                    MessageBox.Show("Medicine doesn't  Saved\n Check Medicine Information and try again ", "ADD Medicine", MessageBoxButtons.OK);

                }

            }
            else 
            {
                    MessageBox.Show("Medicine doesn't  Saved\n Check Medicine Information and try again ", "ADD Medicine", MessageBoxButtons.OK);


            }

        }

        private void ReadNumbersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void ReadDoubleNumbersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)&& !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }
        private void ReadIntegerNumberOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnResetNewMedicine_Click(object sender, EventArgs e)
        {
            ClearTextBoxesAddMedcine();   
        }

        private void btnReloadChartDashbordPharmacist_Click(object sender, EventArgs e)
        {
            FullChatInDashbord();
        }

        
        private void btnViewMedicin_Click(object sender, EventArgs e)
        {
            HideAllPanles();
            ChangeAllButtonsBackgroundAndSizeInControlPanel();
            PanelViewMedical.Location = new Point(275, 0);
            PanelViewMedical.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            bt.Size = new Size(264, 55);
            PanelViewMedical.Visible = true;
            ShowAllMedicinesInDataGridView();
        }

        private void btnDeleteMedicine_Click(object sender, EventArgs e)
        {
            if (IDSelectedInDataGridView<1)
            {
                MessageBox.Show("You Don't Selected Any Midicine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else 
            {
                if (MessageBox.Show("Are You Sure You Want Delete This Medicine", "Delete Medcine", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (clsMedicine.DeleteMedicine(IDSelectedInDataGridView))
                        MessageBox.Show("Medicine Deleted Seccefully", "Delete Medicine", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Medicine doesn't Delete Try Again", "Delete Medicine", MessageBoxButtons.OK);

                }

                ShowAllMedicinesInDataGridView();
            }
        }

        private void DGVMedicales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
              // MedicalNameSelectedInDataGridView = DGVMedicales.Rows[e.RowIndex].Cells[2].Value.ToString();
                IDSelectedInDataGridView = Int32.Parse(DGVMedicales.Rows[e.RowIndex].Cells[0].Value.ToString());
               // MessageBox.Show(MedicalNameSelectedInDataGridView + "\n" + IDSelectedInDataGridView.ToString());
            }
            catch
            {

            }
        }

        private void txtBSeachMedByName_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                DGVMedicales.DataSource = clsMedicine.GetAllMedical();
            }
            else
                DGVMedicales.DataSource = clsMedicine.SearchMedicineByMedicineName((sender as TextBox).Text);

        }

        private void txtBSearchMedicineByID_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                DGVMedicales.DataSource = clsMedicine.GetAllMedical();
            }
            else
                DGVMedicales.DataSource = clsMedicine.SearchMedicalByID(Int32.Parse(txtBSearchMedicineByID.Text));

        }

        private void txtBSearchMedByNumID_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                DGVMedicales.DataSource = clsMedicine.GetAllMedical();
            }
            else
                DGVMedicales.DataSource = clsMedicine.SearchMedicalByNumMedID(txtBSearchMedByNumID.Text);

        }

        private void btnModifieMedicine_Click(object sender, EventArgs e)
        {
            HideAllPanles();
            ChangeAllButtonsBackgroundAndSizeInControlPanel();
            panelUpdateMedicine.Location = new Point(275, 0);
            panelUpdateMedicine.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            bt.Size = new Size(264, 55);
            panelUpdateMedicine.Visible = true;
        }

        private void txtBSearchMedIDUpdate_TextChanged(object sender, EventArgs e)
        {

        
            if ((sender as TextBox).Text != "")
            {
                int ID = Int32.Parse((sender as TextBox).Text);
                clsMedicine medicine = GetMedicineInfoByID(ID);
                if (clsMedicine.IsMedicineExistByID(ID))
                {

                    txtBMedNameUpdate.Text = medicine.MedicineName;
                    txtBMedIDUpdate.Text = medicine.MedicenID;
                    txtBMedNumberUpdate.Text = medicine.MedicineNumber;
                    DTPManfDateUpdate.Value = medicine.MedicineFuctoryDate;
                    DTPExpireDateUpdate.Value = medicine.MedcineExpireDate;
                    txtBAvailableQty.Text = medicine.Quantity.ToString();
                    txtBPricePerUnitUpdate.Text = medicine.PricePerUnite.ToString();
                }
                else 
                {
                    txtBMedNameUpdate.Text ="";
                    txtBMedIDUpdate.Text = "";
                    txtBMedNumberUpdate.Text ="";
                    DTPManfDateUpdate.Value = DateTime.Now;
                    DTPExpireDateUpdate.Value = DateTime.Now;
                    txtBAvailableQty.Text = "";
                    txtBPricePerUnitUpdate.Text ="";

                }

            }
            else
                ClearTextBoxesInUpdatePanel();
        }

        private void btnUpdateMed_Click(object sender, EventArgs e)
        {
            if (!IsAnyTextBoxeInPanelUpdateEmpty())

            { 
                if (clsMedicine.UpdateMedicine(Int32.Parse(txtBSearchMedIDUpdate.Text), txtBMedIDUpdate.Text, txtBMedNameUpdate.Text,
                    txtBMedNumberUpdate.Text, DTPExpireDateUpdate.Value, DTPManfDateUpdate.Value,
                    GetNewTotalQuantityToUpdate(), Double.Parse(txtBPricePerUnitUpdate.Text)))
                {
                    MessageBox.Show("Updated Seccussefully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxesInUpdatePanel();

                }
                else
                {
                    MessageBox.Show("Updated Not Seccussefully\n Check Medicine Information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        
            else
            {
            
                MessageBox.Show("Check The Information You Entired \n And Try Agian", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void btnResetInUpdate_Click(object sender, EventArgs e)
        {
            ClearTextBoxesInUpdatePanel();
        }

        private void btnDeleteInUpdate_Click(object sender, EventArgs e)
        {
            if (txtBSearchMedIDUpdate.Text == "")
            {
                MessageBox.Show("Enter ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                int ID= Int32.Parse(txtBSearchMedIDUpdate.Text);
                if (clsMedicine.IsMedicineExistByID(ID))
                {
                    if (MessageBox.Show("Are You Shure You Want\n To Delete This Medicine?", "Delete Medicine", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                     {     
                        if (clsMedicine.DeleteMedicine(ID))
                          {
                            MessageBox.Show("This ID Deleted Seccufully", "Delete Medicine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTextBoxesInUpdatePanel();
                          }
                        else
                            MessageBox.Show("This ID not Deleted Seccufully Try Again", "Delete Medicine", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                else
                 MessageBox.Show("This ID Not Exist Try Another", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnMedicinValidityCheck_Click(object sender, EventArgs e)
        {
            cmbMedValidity.SelectedIndex = 0;
            lblCheckValidMedicine.Text = "Valid Medicine";
            lblCheckValidMedicine.ForeColor = Color.Green;
            GetAllValidMedicine();
            HideAllPanles();
            ChangeAllButtonsBackgroundAndSizeInControlPanel();
            panelCheckMedValidity.Location = new Point(275, 0);
            panelCheckMedValidity.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            bt.Size = new Size(264, 55);
            panelCheckMedValidity.Visible = true;
        }

        private void cmbMedValidity_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbMedValidity.SelectedIndex)
            {
                case 0:
                    GetAllValidMedicine();
                    lblCheckValidMedicine.Text = "Valid Medicine";
                    lblCheckValidMedicine.ForeColor = Color.Green;
                    break;
                case 1:
                    GetAllExpiredMedicine();
                    lblCheckValidMedicine.Text = "Expired Medicine";
                    lblCheckValidMedicine.ForeColor = Color.Red;
                    break;
                case 2:
                    ShowAllMedicinesInDataGridView();
                    lblCheckValidMedicine.Text = "All Medicine";
                    lblCheckValidMedicine.ForeColor = Color.Blue;
                    break;
            
            }

        }

        private void btnSellMedicine_Click(object sender, EventArgs e)
        {
            HideAllPanles();
            ChangeAllButtonsBackgroundAndSizeInControlPanel();
            LBSellMedNames.Items.Clear();
            FillListBoxInSellPanelWithNamesOfValidMedicine();
            ClearTextBoxesInSellPanel();
            panelSellMedicine.Location = new Point(275, 0);
            panelSellMedicine.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            bt.Size = new Size(264, 55);
            panelSellMedicine.Visible = true;
            TotalBellInSellPanel = 0;
            lblRs.Text = "RS: 0.0";
            DGVInSellPanel.Rows.Clear();
            MedicineIDSelectedInDGVSellPanel = "";
            TotalOfUnitsSelectedInSellPanel = 0;
            NumberOfRowSelectedInSellPanel = -1;
        }

        private void txtBSearchNameMedInSellPanel_TextChanged(object sender, EventArgs e)
        {

            string NameOfMedicine = (sender as TextBox).Text;
            if (NameOfMedicine == "")
            {
      
                LBSellMedNames.Items.Clear();
                FillListBoxInSellPanelWithNamesOfValidMedicine();
            
                
            }
            else 
            {
                LBSellMedNames.Items.Clear();
                ClearTextBoxesInSellPanel();

                FindAllMedicineNamesInSellPanelByNames(NameOfMedicine);
            }
        }

        private void LBSellMedNames_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string MedName = LBSellMedNames.GetItemText(LBSellMedNames.SelectedItem);
            FillTextBoxesInSellPanel(MedName);
            txtBTotalPriceInSellPanel.Text = CalculateTotalPriceInSellMedicine().ToString();
            txtBNumberOfUnitsInSellPanel.Text = "1";
        }

        private void txtBSearchNameMedInSellPanel_MouseClick(object sender, MouseEventArgs e)
        {
            LBSellMedNames.ClearSelected();
            FillTextBoxesInSellPanel("");
        }

        private void txtBNumberOfUnitsInSellPanel_TextChanged(object sender, EventArgs e)
        {
            txtBTotalPriceInSellPanel.Text = CalculateTotalPriceInSellMedicine().ToString();
        }

        private void btnAddToCardInSellPanel_Click(object sender, EventArgs e)
        {
           
            if (txtBMedIdInSellPanel.Text == "")
            {
                MessageBox.Show("You Must To Enter The Medicine ID First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                if (CalculateTotalUnitsInSellMedicine(txtBMedIdInSellPanel.Text) >= 0)
                {
                    if (MessageBox.Show("Are You Sure You Want \n To Add This Qty in you Card", "Cofirm", MessageBoxButtons.OKCancel)==DialogResult.OK) 
                    {
                        if (txtBNumberOfUnitsInSellPanel.Text == "")
                            txtBNumberOfUnitsInSellPanel.Text = "1";
                        if (clsMedicine.UpdateQuantityOfMedicineByMedID(txtBMedIdInSellPanel.Text,Int32.Parse(txtBNumberOfUnitsInSellPanel.Text)*-1))
                        {
                         
                            MessageBox.Show("Quantity of this Medicine Chenged In Stock", "Update Quantity", MessageBoxButtons.OK);
                            FillDGVInSellPanel();
                            TotalBellInSellPanel += CalculateTotalPriceInSellMedicine();
                            lblRs.Text = "RS: " + TotalBellInSellPanel;
                            LBSellMedNames.ClearSelected();
                            FillTextBoxesInSellPanel("");

                        }
                        else 
                        { 
                            MessageBox.Show("Quantity of this Medicine Not Chenged In Stock", "Update Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                    

                }
                else
                {
                    MessageBox.Show("This Quantity Great Than \n Quantity In Stock", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
            }
        }

        private void btnRemoveInSellPanel_Click(object sender, EventArgs e)
        {
            if (MedicineIDSelectedInDGVSellPanel != ""&&NumberOfRowSelectedInSellPanel>-1)
            {
                if (MessageBox.Show("Are Sure You Want to Delete This Medicine", "Delete Medicine", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (clsMedicine.UpdateQuantityOfMedicineByMedID(MedicineIDSelectedInDGVSellPanel, TotalOfUnitsSelectedInSellPanel))
                    {
                        DGVInSellPanel.Rows.RemoveAt(NumberOfRowSelectedInSellPanel);
                        MessageBox.Show("Row Deleted Seccussfully", "Delete Medcine", MessageBoxButtons.OK);
                        NumberOfRowSelectedInSellPanel = -1;
                    }
                    else 
                    {
                        MessageBox.Show("Row Not Deleted Seccussfully", "Delete Medcine", MessageBoxButtons.OK,MessageBoxIcon.Error);

                    }

                }
            }
           

            else
            {
                MessageBox.Show("You Don't Selected Any Medicine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void DGVInSellPanel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGVInSellPanel.Rows.Count-1 > 0&& DGVInSellPanel.Rows[e.RowIndex].Cells[0].Value!=null)
            {
                try
                {
                    MedicineIDSelectedInDGVSellPanel = DGVInSellPanel.Rows[e.RowIndex].Cells[0].Value.ToString();
                    TotalOfUnitsSelectedInSellPanel = Int32.Parse(DGVInSellPanel.Rows[e.RowIndex].Cells[4].Value.ToString());
                    NumberOfRowSelectedInSellPanel = e.RowIndex;
                }
                catch
                {
                    MessageBox.Show("You Have An Exeption",
                     "Exeption", MessageBoxButtons.OK);
                    MedicineIDSelectedInDGVSellPanel = "";
                    TotalOfUnitsSelectedInSellPanel = 0;
                    NumberOfRowSelectedInSellPanel = -1;

                }
            }
            else 
            {
                MessageBox.Show("This Row Is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                MedicineIDSelectedInDGVSellPanel ="";
                TotalOfUnitsSelectedInSellPanel = 0;
                NumberOfRowSelectedInSellPanel = -1;
            }
           

        }

        private void btnPruchaseAndPrintInSellPanell_Click(object sender, EventArgs e)
        {
            DGVPrinter Pr = new DGVPrinter();
            Pr.Title = "Bill Of Medicines";
            Pr.SubTitle = String.Format("Date:{0}", DateTime.Now.Date.ToString());
            Pr.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            Pr.PageNumbers = true;
            Pr.PageNumberInHeader = false;
            Pr.PorportionalColumns = true;
            Pr.HeaderCellAlignment = StringAlignment.Near;
            Pr.Footer = "Total Bill: " + TotalBellInSellPanel;
            Pr.FooterSpacing = 8;
            Pr.PrintDataGridView(DGVInSellPanel);

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            FormLogin FL = new FormLogin();
            FL.Show();
            this.Hide();
        }

        private void panelAddMedicine_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelSellMedicine_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
