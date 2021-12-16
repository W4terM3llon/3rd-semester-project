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
    public partial class AddTableForm : Form
    {
        private Restaurant restaurant;
        private TableForm tableForm;
        private IDesktopApiClient dataAccess;
        public AddTableForm(Restaurant restaurant, TableForm tableForm)
        {
            InitializeComponent();
            this.restaurant = restaurant;
            this.tableForm = tableForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Tables");
        }
        
        #region EventHandlers
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddTable();
            this.Close();
        }
        #endregion

        #region Functionality
        private void AddTable() 
        {
            Table table = new Table();
            table.restaurant = restaurant;
            table.seatNumber = seatNumTextBox.Text;
            table.description = descTextBox.Text;
            int responseCode = dataAccess.CreateTable(table, tableForm.restForm.homeForm.JWTtoken);
            if (responseCode == 201)
            {
                MessageBox.Show($"Table added successfully");
            }
            else 
            {
                MessageBox.Show($"There was an error creating table");
            }
        }
        #endregion
    }
}
