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
    public partial class AddDishForm : Form
    {
        private Restaurant restaurant;
        private IDesktopApiClient dataAccess;
        private IDesktopApiClient dataAccessDCategory;
        private DishForm dishForm;
        public AddDishForm(Restaurant restaurant, DishForm dishForm)
        {
            InitializeComponent();
            this.restaurant = restaurant;
            this.dishForm = dishForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Dishes");
            dataAccessDCategory = new DesktopApiClient("https://localhost:44394/api/DishCategories");
        }

        #region EventHandlers
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void createButton_Click(object sender, EventArgs e)
        {
            AddDish();
            this.Close();
        }
        #endregion

        #region Functionality
     
        private void AddDish()
        {
            DishCategory dishCategory = new DishCategory();
            List<DishCategory> dishCatList = dataAccessDCategory.GetDishCategories(dishForm.restForm.homeForm.JWTtoken);
            foreach (DishCategory d in dishCatList) 
            {
                if (d.name.Equals(dishCatTextBox.Text)) 
                {
                    dishCategory.id = d.id;
                    dishCategory.name = d.name;
                }
            }
            Dish dish = new Dish();
            dish.name = nameTextBox.Text;
            dish.price = float.Parse(priceTextBox.Text);
            dish.description = descTextBox.Text;
            dish.likes = 0;
            dish.dishCategory = dishCategory;
            dish.restaurant = restaurant;
            int resultCode = dataAccess.CreateDish(dish, dishForm.restForm.homeForm.JWTtoken);
            if (resultCode == 201)
            {
                MessageBox.Show($"Dish was successfully created.");
            }
            else 
            {
                MessageBox.Show($"There was an error creating dish.");
            }
        }
        #endregion

    }
}
