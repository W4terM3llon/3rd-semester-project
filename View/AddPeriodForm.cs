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
    public partial class AddPeriodForm : Form
    {
        private Restaurant restaurant;
        private DinPerForm perForm;
        private IDesktopApiClient dataAccess;
        public AddPeriodForm(Restaurant restaurant, DinPerForm perForm)
        {
            InitializeComponent();
            this.restaurant = restaurant;
            this.perForm = perForm;
            this.dataAccess = new DesktopApiClient("https://localhost:44394/api/DiningPeriods");
        }

        #region EventHandlers
        private void addButton_Click(object sender, EventArgs e)
        {
            AddPeriod();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Functionality
        private void AddPeriod() 
        {
            DiningPeriod period = new DiningPeriod();
            period.restaurant = restaurant;
            period.name = nameTextBox.Text;
            period.timeStartMinutes = Int32.Parse(startTimeTextBox.Text);
            period.durationMinutes = Int32.Parse(durationTextBox.Text);
            int responseCode = dataAccess.CreatePeriod(period, perForm.restForm.homeForm.JWTtoken);
            if(responseCode == 201)
            {
                MessageBox.Show($"Dining period added successfully");
            }
            else
            {
                MessageBox.Show($"There was an error creating dining period");
            }
        }
        #endregion
    }
}
