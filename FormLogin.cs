using PharmacyBusinessLayers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy_Management
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRestate_Click(object sender, EventArgs e)
        {
            txtBUserName.Clear();
            txtBPassword.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (clsUsres.IsNotExistAnyUser())
            {
                if (txtBUserName.Text == "root" && txtBPassword.Text == "root")
                {

                    Adminstrator admin = new Adminstrator(txtBUserName.Text);
                    admin.Show();
                    this.Hide();
                }
                else
                {

                    MessageBox.Show("Your User Name Or Password Wrong Try Again", "Wrong Enter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }




            }

            else
            {
                if (clsUsres.IsUserExistByUserNameAndPassword(txtBUserName.Text.Trim(), txtBPassword.Text.Trim()))
                {
                    if (clsUsres.IsUserIsAdministrator(txtBUserName.Text.Trim(), txtBPassword.Text.Trim()))
                    {

                        Adminstrator admin = new Adminstrator(txtBUserName.Text);
                        admin.Show();
                        this.Hide();
                    }
                    else
                    {
                        Pharmacist PH = new Pharmacist(txtBUserName.Text);
                        PH.Show();
                        this.Hide();
                    }


                }
                else
                {
                    MessageBox.Show("Your User Name Or Password Wrong Try Again", "Wrong Enter", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
         
        }
    }
}
