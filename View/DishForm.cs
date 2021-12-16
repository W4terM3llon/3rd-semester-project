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
    public partial class DishForm : Form
    {
        public RestForm restForm { get; set; }
        private IDesktopApiClient dataAccess;
        public DishForm(RestForm restForm)
        {
            InitializeComponent();
            this.restForm = restForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Dishes");
        }

        #region EventHandlers
        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }
        private void DishForm_Load(object sender, EventArgs e)
        {
            ShowDishes();
        }
        private void addDishButton_Click(object sender, EventArgs e)
        {
            AddDish();
        }
        private void dishUpdateButton_Click(object sender, EventArgs e)
        {
            UpdateDish();
        }
        private void deleteDishButton_Click(object sender, EventArgs e)
        {
            DeleteDish();
        }
        private void refreshButton_Click(object sender, EventArgs e)
        {
            CleanList();
            ShowDishes();
        }
        #endregion

        #region Functionality
        private void Back()
        {
            restForm.Show();
            this.Close();
        }

        private void AddDish()
        {
            new AddDishForm(restForm.restaurant, this).Show();
        }

        private void ShowDishes() 
        {
            List<Dish> dishList = dataAccess.GetDishes(restForm.homeForm.JWTtoken, restForm.restaurant.id);
            foreach (Dish d in dishList)
            {
                string price = d.price.ToString();
                string[] row = { d.name, price, d.dishCategory.name, d.description };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
            }
        }

        private void CleanList()
        {
            listView1.Items.Clear();
        }

        private void UpdateDish() 
        {
            var selectedItem = listView1.SelectedItems[0];
            Dish dish = dataAccess.GetDishes(restForm.homeForm.JWTtoken, restForm.restaurant.id)[selectedItem.Index];
            new UpdateDishForm(dish, this).Show();
        }

        private void DeleteDish() 
        {
            var selectedItem = listView1.SelectedItems[0];
            Dish dish = dataAccess.GetDishes(restForm.homeForm.JWTtoken, restForm.restaurant.id)[selectedItem.Index];
            int responseCode = dataAccess.DeleteDish(dish.id, restForm.homeForm.JWTtoken);
            if (responseCode == 200)
            {
                MessageBox.Show($"Dish deleted successfully");
            }
            else 
            {
                MessageBox.Show($"Deletion failed");
            }
        }
        #endregion

    }
}
