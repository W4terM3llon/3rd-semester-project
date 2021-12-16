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
    public partial class UpdateDishForm : Form
    {
        private Dish dish;
        private DishForm dishForm;
        private IDesktopApiClient dataAccessDCategory;
        private IDesktopApiClient dataAccess;

        public UpdateDishForm(Dish dish, DishForm dishForm)
        {
            InitializeComponent();
            this.dish = dish;
            this.dishForm = dishForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Dishes");
            dataAccessDCategory = new DesktopApiClient("https://localhost:44394/api/DishCategories");
        }

        #region EventHandlers
        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateDish();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Functionality
        private void UpdateDish() 
        {
            Dish updateDish = new Dish();
            if (String.IsNullOrEmpty(nameTextBox.Text))
            {
                updateDish.name = dish.name;
            }
            else 
            {
                updateDish.name = nameTextBox.Text;
            }

            if (String.IsNullOrEmpty(priceTextBox.Text))
            {
                updateDish.price = dish.price;
            }
            else
            {
                updateDish.price = float.Parse(priceTextBox.Text);
            }

            if (String.IsNullOrEmpty(descTextBox.Text))
            {
                updateDish.description = dish.description;
            }
            else
            {
                updateDish.description = descTextBox.Text;
            }

            if (String.IsNullOrEmpty(dishCatTextBox.Text))
            {
                updateDish.dishCategory = dish.dishCategory;
            }
            else
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
                updateDish.dishCategory = dishCategory;
            }
            updateDish.id = dish.id;
            updateDish.likes = dish.likes;
            updateDish.restaurant = dish.restaurant;
            int responseCode = dataAccess.UpdateDish(updateDish, dishForm.restForm.homeForm.JWTtoken);

            if (responseCode == 200)
            {
                MessageBox.Show($"Updated successfully!");
            }
            else
            {
                MessageBox.Show($"Update failed!");
            }
            this.Close();
        }
        #endregion
    }
}
