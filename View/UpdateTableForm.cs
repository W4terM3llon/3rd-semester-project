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
    public partial class UpdateTableForm : Form
    {
        private Table table;
        private TableForm tableForm;
        private IDesktopApiClient dataAccess;
        public UpdateTableForm(Table table, TableForm tableForm)
        {
            InitializeComponent();
            this.table = table;
            this.tableForm = tableForm;
            dataAccess = new DesktopApiClient("/Tables");
        }

        #region EventHandlers
        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateTable();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Functionality
        private void UpdateTable() 
        {
            Table updateTable = new Table();
            if (String.IsNullOrEmpty(seatNumTextBox.Text))
            {
                updateTable.seatNumber = table.seatNumber;
            }
            else
            {
                updateTable.seatNumber = seatNumTextBox.Text;
            }

            if (String.IsNullOrEmpty(descTextBox.Text))
            {
                updateTable.description = table.description;
            }
            else
            {
                updateTable.description = descTextBox.Text;
            }
            updateTable.id = table.id;
            updateTable.restaurant = table.restaurant;

            int responseCode = dataAccess.UpdateTable(updateTable, tableForm.restForm.homeForm.JWTtoken);

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
