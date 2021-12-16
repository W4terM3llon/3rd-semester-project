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
        private void regButton_Click(object sender, EventArgs e)
        {
            Register();
        }
        
        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }
        #endregion

        #region Functionality
        private void Register() 
        {

            if (pswdText.Text.Equals(pswdRepeatText.Text))
            {
               
                Address address = new Address();
                address.appartment = appartText.Text;
                address.street = streetText.Text;
                User user = new User();
                user.email = emailText.Text;
                user.password = pswdText.Text;
                user.phoneNumber = pnText.Text;
                user.firstName = fnText.Text;
                user.lastName = lnText.Text;
                user.address = address;
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

        private void Back() 
        {
            loginForm.Show();
            Close();
        }

        #endregion

    }
}
