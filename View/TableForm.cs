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
    public partial class TableForm : Form
    {
        public RestForm restForm { get; set; }
        private IDesktopApiClient dataAccess;
        public TableForm(RestForm restForm)
        {
            InitializeComponent();
            this.restForm = restForm;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Tables");
        }

        #region EventHandlers
        private void backButton_Click(object sender, EventArgs e)
        {
            Back();
        }
        private void addButton_Click(object sender, EventArgs e)
        {
            AddTable();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteTable();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            CleanList();
            ShowTables();
        }
        private void TableForm_Load(object sender, EventArgs e)
        {
            ShowTables();
        }
        #endregion

        #region Functionality
        private void Back() 
        {
            restForm.Show();
            this.Close();
        }
        
        private void ShowTables() 
        {
            List<Table> tableList = dataAccess.GetTables(restForm.homeForm.JWTtoken, restForm.restaurant.id);
            foreach (Table t in tableList)
            {
                string[] row = { t.id, t.seatNumber, t.description };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
            }
        }

        private void CleanList()
        {
            listView1.Items.Clear();
        }

        private void AddTable()
        {
            new AddTableForm(restForm.restaurant, this).Show();
        }

        private void UpdateTable() 
        {
            var selectedItem = listView1.SelectedItems[0];
            Table table = dataAccess.GetTables(restForm.homeForm.JWTtoken, restForm.restaurant.id)[selectedItem.Index];
            new UpdateTableForm(table, this).Show();
        }

        private void DeleteTable() 
        {
            var selectedItem = listView1.SelectedItems[0];
            Table table = dataAccess.GetTables(restForm.homeForm.JWTtoken, restForm.restaurant.id)[selectedItem.Index];
            int responseCode = dataAccess.DeleteTable(table.id, restForm.homeForm.JWTtoken);
            MessageBox.Show($"Table deleted successfully");           
        }
        #endregion

    }
}
