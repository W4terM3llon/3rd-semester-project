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
    public partial class DinPerForm : Form
    {
        public RestForm restForm { get; set; }
        private IDesktopApiClient dataAccess;
        public DinPerForm(RestForm restForm)
        {
            InitializeComponent();
            this.restForm = restForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/DiningPeriods");

        }

        #region EventHandlers
        private void refreshButton_Click(object sender, EventArgs e)
        {
            CleanList();
            ShowPeriods();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddPeriod();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdatePeriod();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeletePeriod();
        }

        private void DinPerForm_Load(object sender, EventArgs e)
        {
            ShowPeriods();
        }
        #endregion

        #region Functionality
        private void Back()
        {
            restForm.Show();
            this.Close();
        }

        private void CleanList()
        {
            listView1.Items.Clear();
        }

        private void DeletePeriod()
        {
            var selectedItem = listView1.SelectedItems[0];
            DiningPeriod dinPeriod = dataAccess.GetPeriods(restForm.homeForm.JWTtoken, restForm.restaurant.id)[selectedItem.Index];
            int responseCode = dataAccess.DeletePeriod(dinPeriod.id, restForm.homeForm.JWTtoken);
            MessageBox.Show($"Dining period deleted successfully");
        }

        private void ShowPeriods()
        {
            List<DiningPeriod> periodList = dataAccess.GetPeriods(restForm.homeForm.JWTtoken, restForm.restaurant.id);
            foreach (DiningPeriod dp in periodList)
            {
                string time = DateTime.MinValue.AddMinutes(dp.timeStartMinutes).ToString("T");
                string duration = dp.durationMinutes.ToString();
                string[] row = { dp.name, time, duration };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
            }
        }

        private void UpdatePeriod() 
        {
            var selectedItem = listView1.SelectedItems[0];
            DiningPeriod period = dataAccess.GetPeriods(restForm.homeForm.JWTtoken, restForm.restaurant.id)[selectedItem.Index];
            new UpdatePeriodForm(period, this).Show();
        }

        private void AddPeriod() 
        {
            new AddPeriodForm(restForm.restaurant, this).Show();
        }
        #endregion
    }
}
