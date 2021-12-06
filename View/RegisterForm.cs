using DesktopClient.Controller;
using DesktopClient.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient
{
    public partial class RegisterForm : Form
    {
        private IDesktopApiClient dataAccess;
        private LoginForm loginForm;
        public RegisterForm(LoginForm loginForm)
        {
            this.loginForm = loginForm;
            InitializeComponent();
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Login/");
        }

        #region EventHandlers
        private void emailLabel_Click_1(object sender, EventArgs e)
        {

        }

        private void emailText_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void pswdLabel_Click_1(object sender, EventArgs e)
        {

        }

        private void pswdText_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void pswdRepeatText_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void pnText_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void pnLabel_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Register_Load_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void regButton_Click(object sender, EventArgs e)
        {
            Register();
        }

        private void zipText_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Functionality
        private void Register() 
        {

            if (pswdText.Text.Equals(pswdRepeatText.Text))
            {
               
                AccountingAddress address = new AccountingAddress();
                address.Appartment = appartText.Text;
                address.Street = streetText.Text;
                User user = new User();
                user.Email = emailText.Text;
                user.Password = pswdText.Text;
                user.PhoneNumber = pnText.Text;
                user.FirstName = fnText.Text;
                user.LastName = lnText.Text;
                user.AccountingAddress = address;
                int responseCode = dataAccess.Register(user);
                if (responseCode == 201)
                {
                    MessageBox.Show($"Successful registration");
                    loginForm.Show();
                    Close();
                    
                }
                else if (responseCode == 400)
                {
                    MessageBox.Show($"Registration failed! \n" +
                        "Some fields are missing.");
                }
            }
            else
            {
                MessageBox.Show("Passwords don't match");
            }
        }
        
        #endregion
    }
}
