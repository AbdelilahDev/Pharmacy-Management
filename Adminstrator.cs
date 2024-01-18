using Pharmacy_Management.Properties;
using PharmacyBusinessLayers;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace Pharmacy_Management
{
    public partial class Adminstrator : Form
    {

        
        string UserNameSelectedInDataGridView="";
        int UserIDSelectedInDataGridView=0;
        string CurrentUserName;
        int CurrentUserID;
        public Adminstrator()
        {
            InitializeComponent();
            HideAllPanels();
            cmbUserRole.SelectedIndex = 0;
        }
        public Adminstrator(string UserName)
        {
            InitializeComponent();
            HideAllPanels();
            cmbUserRole.SelectedIndex = 0;
            lblSystemUser.Text = UserName;
            CurrentUserName = UserName;
           CurrentUserID = clsUsres.GetUserIDByUserName(UserName);
            
        }
        clsUsres _AddNewUser()
        {
            clsUsres User = new clsUsres();
            User.UserRole = cmbUserRole.GetItemText(cmbUserRole.SelectedItem);
            User.FullName = txtName.Text;
            User.DateOfBirth = DTPDateOfBirth.Value;
            User.Email = txtBEmailAddress.Text;
            User.Phone =long.Parse( txtBMobileNumber.Text);
            User.UserName = txtBUserName.Text;
            User.PassWord = txtBPassword.Text;
            return User;
 
        }
        void ChangeAllButtonsBackground() 
        {

            btnAddUser.BackColor = Color.DimGray;
            btnViewUser.BackColor = Color.DimGray;
            btnProfil.BackColor = Color.DimGray;
            btnLogout.BackColor = Color.DimGray;
            btnDashbord.BackColor = Color.DimGray;

        }
        void ClearControls()
        {
            cmbUserRole.SelectedIndex = -1;
            DTPDateOfBirth.Value = DateTime.Now;
            txtBEmailAddress.Clear();
            txtBMobileNumber.Clear();
            txtBPassword.Clear();
            txtBUserName.Clear();
            txtName.Clear();
        }
        void HideAllPanels() 
        {

            panelDashbord.Visible = false;
            panelAddUser.Visible = false;
            panelViewUsers.Visible = false;
            panelProfile.Visible = false;
        }
        void RefreshDGVOfUsers()
        {
            DGVUsers.DataSource = clsUsres.GetAllUsers();
        }
        bool CheckTexesBoxInAddUser()
        {
            return !(cmbUserRole.Text==""||txtName.Text==""||txtBEmailAddress.Text==""||txtBUserName.Text==""||txtBPassword.Text=="");
        
        }
        bool CheckTexesBoxInProfileUser()
        {
            return !(cmbUserRolesProfile.Text == "" || txtBFullNameProfile.Text == "" 
                || txtBEmailProfile.Text == "" || txtBUserIDProfile.Text == "" || txtBPasswordProfile.Text == ""
               ||txtBUserIDProfile.Text==""||txtBMobileProfile.Text==""||txtBUserNameProfile.Text=="");

        }

        void ClearAllTextesBoxFromAddUserPanel()
        {
            txtBEmailAddress.Clear();
        }
        private void _FillUserRoleInComboBoxInUserProfile()
        {
            DataTable dtUsers = clsUsres.GetAllUsers();
            foreach (DataRow row in dtUsers.Rows)
            {
                if (!cmbUserRolesProfile.Items.Contains(row["UserRole"]) )
                 {
                    cmbUserRolesProfile.Items.Add(row["UserRole"]);

                }
              
            }

        }
        void showUserInformationInProfile(clsUsres User)
        {
            txtBEmailProfile.Text = User.Email;
            cmbUserRolesProfile.Text = User.UserRole;
            txtBFullNameProfile .Text = User.FullName ;
            DTPProfile.Value= User.DateOfBirth ;
           txtBMobileProfile.Text= User.Phone.ToString();
             txtBUserNameProfile.Text= User.UserName ;
            txtBPasswordProfile.Text = User.PassWord ;
            txtBEmailAddress.Text = User.Email ;
            txtBUserIDProfile.Text = User.ID.ToString();
            lblUserNameProfile.Text = User.FullName;
            lblUserRoleProfile.Text = User.UserRole;
           

        }
        private void btnDashbord_Click(object sender, EventArgs e)
        {
            ChangeAllButtonsBackground();
            HideAllPanels();
            panelDashbord.Location = new Point(0, 0);
            panelDashbord.Size = new Size(724, 694);
            Button bt = (sender as Button);
            bt.BackColor = Color.DeepPink;
            panelDashbord.Visible = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form frm = new FormLogin();
            frm.Show();
            this.Hide();
        }

        private void Adminstrator_Load(object sender, EventArgs e)
        {
            btnDashbord.PerformClick();
            lblTotalPharmacist.Text = clsUsres.GetTotalPharmasist().ToString();
            lblTotalAdmin.Text = clsUsres.GetTotalAdministrator().ToString();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            ClearAllTextesBoxFromAddUserPanel();
            ChangeAllButtonsBackground();
            HideAllPanels();
            panelAddUser.Location = new Point(0, 0);
            panelAddUser.Size = new Size(724, 694);
            Button bt = (sender as Button);
           
            bt.BackColor = Color.DeepPink;
            panelAddUser.Visible = true;
        }

        private void txtBMobileNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void btnSingUp_Click(object sender, EventArgs e)
        {
            if (CheckTexesBoxInAddUser())
            {
                clsUsres User = _AddNewUser();
                if (User.Save())
                {
                    MessageBox.Show("User Added Seccessfully", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.None);
                    ClearControls();
                }
                else
                {
                    MessageBox.Show("User deosn't addes try Again", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }


            }
            else 
            {
                    MessageBox.Show("You Have Missing information about this User\n Chack User Information and try Again", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void txtBUserName_TextChanged(object sender, EventArgs e)
        {
            if (clsUsres.IsUserExistByUserName((sender as TextBox).Text.ToString()))
            {
                pcUserName.ImageLocation = @"C:\Users\arab\Downloads\yes.png";
            }
            else
                pcUserName.ImageLocation = @"C:\Users\arab\Downloads\No.png";

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Adminstrator_Load(this, null);
        }

        private void btnViewUser_Click(object sender, EventArgs e)
        {
            ChangeAllButtonsBackground();
            HideAllPanels();
            
            panelViewUsers.Location = new Point(0, 0);
            panelViewUsers.Size = new Size(724, 694);
            Button bt = (sender as Button);

            bt.BackColor = Color.DeepPink;
            panelViewUsers.Visible = true;
            txtBSearchUserByID.Focus();
            DGVUsers.DataSource = clsUsres.GetAllUsers();
        }

        private void txtBSearchUserByUserName_TextChanged(object sender, EventArgs e)
        {
           
            DGVUsers.DataSource = clsUsres.SearchForUsersByUserName((sender as TextBox).Text);
        }

        private void txtBSearchUserByID_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                DGVUsers.DataSource = clsUsres.GetAllUsers();
            }
            else
            DGVUsers.DataSource = clsUsres.SearchForUserByID((sender as TextBox).Text);

        }

        private void DGVUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            try
            {
                UserNameSelectedInDataGridView = DGVUsers.Rows[e.RowIndex].Cells[5].Value.ToString();
                UserIDSelectedInDataGridView =Int32.Parse (DGVUsers.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            catch 
            {
            
            }
       }

        private void btnDelete_Click(object sender, EventArgs e)
        {
                  if (UserNameSelectedInDataGridView == "")
            {
                MessageBox.Show("You Don't Selected Any user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           else if (CurrentUserName != UserNameSelectedInDataGridView)
            {
                // MessageBox.Show("You Can't delete this UserName", "Error", MessageBoxButtons.OK);
                if (MessageBox.Show("Are You Sure You Want Delete This User", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    clsUsres.DeleteUser(UserIDSelectedInDataGridView);
                    MessageBox.Show("User Deleted Seccefully", "Delete User", MessageBoxButtons.OK);
                    RefreshDGVOfUsers();

                }
            }
       
            else
            {
                MessageBox.Show("You Can't delete this UserName", "Error", MessageBoxButtons.OK);

            }
        }

        private void btnProfil_Click(object sender, EventArgs e)
        {
            _FillUserRoleInComboBoxInUserProfile();
            ChangeAllButtonsBackground();
            HideAllPanels();
            showUserInformationInProfile(clsUsres.FindUserByID(CurrentUserID));

            panelProfile.Location = new Point(0, 0);
            panelProfile.Size = new Size(724, 694);
            Button bt = (sender as Button);

            bt.BackColor = Color.DeepPink;
            panelProfile.Visible = true;
        }

        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            clsUsres User = clsUsres.FindUserByID(CurrentUserID);
            User.Mode = clsUsres.enMode.Update;
            if (CheckTexesBoxInProfileUser())
            {

                if (MessageBox.Show("Are You Show You Want To\n Update this informating", "Update User Ibformation", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {

                    User.UserRole = cmbUserRole.Text;
                    User.FullName = txtBUserNameProfile.Text;
                    User.DateOfBirth = DTPProfile.Value;
                    User.Phone = Int32.Parse(txtBMobileProfile.Text);
                    User.UserName = txtBUserNameProfile.Text;
                    User.PassWord = txtBPasswordProfile.Text;
                    if (User.Save())
                    {
                        MessageBox.Show("The User Information Updated Secceffully", "Update", MessageBoxButtons.OK);
                        btnLogout.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("The User Information is not Updated ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
            }
            else
            {
                MessageBox.Show("You Have Missing information about this User\n Chack User Information and try Again", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }



            
        }

        private void btResetProfile_Click(object sender, EventArgs e)
        {
            showUserInformationInProfile(clsUsres.FindUserByID(CurrentUserID));
        }

        private void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
