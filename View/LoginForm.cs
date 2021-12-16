﻿using DesktopClient.Controller;
using DesktopClient.DTO;
using DesktopClient.View;
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
    public partial class LoginForm : Form
    {
        private IDesktopApiClient dataAccess;
        public LoginForm()
        {
            InitializeComponent();
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Login/");
        }

        #region EventHandlers
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            new RegisterForm(this).Show();
            Hide();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Login();
        }
        #endregion

        #region Functionality
        private void Login() 
        {
            string email = emailText.Text;
            string password = pwdText.Text;
            JWTToken content = dataAccess.Login(email, password);
            //MessageBox.Show($"Your token is: " + content + "\n" +
            //    "With mail: " + email + "\n" +
            //    "With password: " + password);
            if (content.error != null)
            {
                MessageBox.Show($"There has been an error while logging in: \n" + content.error);
            }
            else
            {
//                new HomeForm(this, content.token).Show();

                new OrderForm(this, content.token).Show();
                Hide();
            }
        }
        #endregion

        private void emailText_TextChanged(object sender, EventArgs e)
        {

        }

        private void pwdText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}