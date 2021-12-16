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
    public partial class UpdatePeriodForm : Form
    {
        private DiningPeriod period;
        private DinPerForm perForm;
        private IDesktopApiClient dataAccess;
        public UpdatePeriodForm(DiningPeriod period, DinPerForm perForm)
        {
            InitializeComponent();
            this.period = period;
            this.perForm = perForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/DiningPeriods");
        }

        #region EventHandlers
        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdatePeriod();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Functionality
        private void UpdatePeriod() 
        {
            DiningPeriod updatePeriod = new DiningPeriod();
            if (String.IsNullOrEmpty(nameTextBox.Text))
            {
                updatePeriod.name = period.name;
            }
            else
            {
                updatePeriod.name = nameTextBox.Text;
            }

            if (String.IsNullOrEmpty(startTimeTextBox.Text))
            {
                updatePeriod.timeStartMinutes = period.timeStartMinutes;
            }
            else
            {
                updatePeriod.timeStartMinutes = Int32.Parse(startTimeTextBox.Text);
            }

            if (String.IsNullOrEmpty(durationTextBox.Text))
            {
                updatePeriod.durationMinutes = period.durationMinutes;
            }
            else
            {
                updatePeriod.durationMinutes = Int32.Parse(durationTextBox.Text);
            }
            updatePeriod.id = period.id;
            updatePeriod.restaurant = period.restaurant;

            int responseCode = dataAccess.UpdatePeriod(updatePeriod, perForm.restForm.homeForm.JWTtoken);

            if (responseCode == 200)
            {
                MessageBox.Show($"Updated successfully!");
            }
            else
            {
                MessageBox.Show($"Update failed!");
            }
        }
        #endregion
    }
}
