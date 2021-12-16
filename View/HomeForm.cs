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
    public partial class HomeForm : Form
    {
        private LoginForm loginForm;
        public string JWTtoken { get; set; }
        private IDesktopApiClient dataAccess;
        public HomeForm(LoginForm loginForm, string JWTtoken)
        {
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Restaurant");
            this.loginForm = loginForm;
            this.JWTtoken = JWTtoken;
            InitializeComponent();
        }

        #region EventHandlers
        private void button1_Click(object sender, EventArgs e)
        {
            AddRest();
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            CleanList();
            ShowRestaurants();
        }

        private void listView1_Click(object sender, EventArgs e) 
        {
            var firstSelectedItem = listView1.SelectedItems[0];
            Restaurant restaurant = dataAccess.GetRestaurants(JWTtoken)[firstSelectedItem.Index];
            OpenRestaurant(restaurant);
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            CleanList();
            ShowRestaurants();
        }
        #endregion

        #region Functionality
        private void Logout() 
        {
            loginForm.Show();
            Close();
        }

        private void AddRest() 
        {
            new AddRestForm(this).Show();
            Hide();
        }

        private void ShowRestaurants() 
        {
            List<Restaurant> restList = dataAccess.GetRestaurants(JWTtoken);
            if (restList?.Any() != true)
            {
                return;
            }
            else
            {
                foreach (Restaurant r in restList)
                {
                    string address = r.address.street +" "+ r.address.appartment; 
                    string[] row = {r.name, address};
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                }
            }

        }

        private void CleanList() 
        {
            listView1.Items.Clear();
        }

        private void OpenRestaurant(Restaurant restaurant) 
        {
            new RestForm(restaurant, this).Show();
            this.Hide();
        }
        #endregion

    }
}
