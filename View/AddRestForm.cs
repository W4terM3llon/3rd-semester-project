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
    public partial class AddRestForm : Form
    {
        private HomeForm homeForm;
        private IDesktopApiClient dataAccess;
        public AddRestForm(HomeForm homeForm)
        {
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Restaurant/");
            this.homeForm = homeForm;
            InitializeComponent();
        }
        #region Event Handlers
        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void addRestButton_Click(object sender, EventArgs e)
        {
            AddRest();
        }
        #endregion

        #region Functionality
        private void Back() 
        {
            homeForm.Show();
            Close();
        }

        private void AddRest() 
        {
            Address address = new Address();
            address.appartment = appartmentText.Text;
            address.street = streetText.Text;
            Restaurant restaurant = new Restaurant();
            restaurant.name = restNameText.Text;
            restaurant.isTableBookingEnabled = bookingBox.Checked;
            restaurant.isDeliveryAvailable = deliveryBox.Checked;
            restaurant.address = address;
            restaurant.everyDayUseAccountEmail = emailText.Text;
            restaurant.everyDayUseAccountPassword = pswdText.Text;
            int responseCode = dataAccess.AddRest(restaurant, homeForm.JWTtoken);
            if (responseCode == 201)
            {
                MessageBox.Show($"You have successfully added a new restaurant");
                homeForm.Show();
                Close();
            }
            else if (responseCode == 403)
            {
                MessageBox.Show($"You are not authorized to add new restaurant");
            }
            else if (responseCode == 400) 
            {
                MessageBox.Show($"Operation failed fields are missing");
            }
        }
        #endregion
    }
}
