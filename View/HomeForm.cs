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
    public partial class HomeForm : Form
    {
        private LoginForm loginForm;
        private string JWTtoken;
        public HomeForm(LoginForm loginForm, string JWTtoken)
        {
            this.loginForm = loginForm;
            this.JWTtoken = JWTtoken;
            InitializeComponent();
        }
    }
}
