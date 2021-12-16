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
    public partial class BookingForm : Form
    {
        private RestForm restForm;
        private IDesktopApiClient dataAccess;

        public BookingForm(RestForm restForm)
        {
            InitializeComponent();
            this.restForm = restForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Booking");
        }

        #region EventHandlers
        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }
        private void refreshButton_Click(object sender, EventArgs e)
        {
            ShowBookings();
        }
        #endregion

        #region Functionality
        private void Back()
        {
            restForm.Show();
            this.Close();
        }

        private void ShowBookings() 
        {
            string date = dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            List<Booking> bookingList = dataAccess.GetBookings(restForm.homeForm.JWTtoken, restForm.restaurant.id, dateTimePicker1.Text);
            foreach (Booking b in bookingList) 
            {
                
                string[] row = { b.id, b.date.ToString("yyyy-MM-dd"), b.table.seatNumber, b.diningPeriod.name, 
                    b.user.firstName + " " + b.user.lastName};
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
            }
        }
        #endregion

    }
}
