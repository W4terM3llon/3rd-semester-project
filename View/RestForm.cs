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
    public partial class RestForm : Form
    {
        public Restaurant restaurant { get; set; }
        public HomeForm homeForm { get; set; }

        public RestForm(Restaurant restaurant, HomeForm homeForm)
        {
            InitializeComponent();
            this.restaurant = restaurant;
            this.homeForm = homeForm;
            this.restName.Text = restaurant.name;
        }

        #region EventHandlers
        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }
        private void bookingsButton_Click(object sender, EventArgs e)
        {
            Booking();
        }
        private void tableButton_Click(object sender, EventArgs e)
        {
            Tables();
        }
        private void dishButton_Click(object sender, EventArgs e)
        {
            Dish();
        }
        private void dinPerButton_Click(object sender, EventArgs e)
        {
            DiningPeriod();
        }
        private void orderButton_Click(object sender, EventArgs e)
        {
            Order();
        }
        #endregion

        #region Functionality
        private void Back() 
        {
            homeForm.Show();
            this.Close();
        }

        private void Booking() 
        {
            new BookingForm(this).Show();
            this.Hide();
        }

        private void Tables()
        {
            new TableForm(this).Show();
            this.Hide();
        }

        private void Dish()
        {
            new DishForm(this).Show();
            this.Hide();
        }

        private void DiningPeriod() 
        {
            new DinPerForm(this).Show();
            this.Hide();
        }

        private void Order() 
        {
            new OrderForm(this).Show();
            this.Hide();
        }
        #endregion

    }
}
