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

namespace DesktopClient.View
{
    public partial class OrderForm : Form
    {
        private LoginForm loginForm;
        private string JWTtoken;
        private IDesktopApiClient dataAccess;
        private IDesktopApiClient dataAccessStagesAccess;

        private List<Order> orders;
        private List<OrderStage> orderStages;
        
        
        public OrderForm(LoginForm loginForm, string JWTtoken)
        {
            this.loginForm = loginForm;
            this.JWTtoken = JWTtoken;
            dataAccess = new DesktopApiClient("https://localhost:44394/api/Orders");
            dataAccessStagesAccess = new DesktopApiClient("https://localhost:44394/api/OrderStages");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.MouseClick += new MouseEventHandler(listView1_MouseClick);
            button1.MouseClick += new MouseEventHandler(button1_MouseClick);
            button3.MouseClick += new MouseEventHandler(button3_MouseClick);
            dateTimePicker1.ValueChanged += new EventHandler(dateTimePicker1_ValueChanged);

            LoadFormData();
        }

        private void LoadFormData()
        {
            if(orders != null) { 
                orders.Clear();
            }
            if (orderStages != null)
            {
                orderStages.Clear();
            }
            comboBox1.Items.Clear();
            listView1.Items.Clear();
            string date = dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            orders = dataAccess.GetOrders("763", dateTimePicker1.Text, JWTtoken);
            orderStages = dataAccessStagesAccess.GetOrderStages(JWTtoken);

            List<string> strArr = new List<string>();
            List<ListViewItem> items = new List<ListViewItem>();


            for (int i = 0; i < orders.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(orders[i].id);
                string[] var = { orders[i].id, orders[i].date.ToString(), orders[i].customer.firstName + " " + orders[i].customer.lastName, orders[i].orderLines[0].dish.description + ", ...", orders[i].orderStage.name };
                items.Add(new ListViewItem(var));

                /*foreach (OrderLine orderLine in order.orderLines)
                {
                    this.listView1.Columns.Add(orderLine.dish.name);
                }*/

            }

            // = { "1", "Matej", "2021", "hranolky" };

            foreach (ListViewItem item in items)
            {
                this.listView1.Items.Add(item);
            }

            foreach(OrderStage item in orderStages)
            {
                comboBox1.Items.Add(item.name);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadFormData();
        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                var rectangle = listView1.GetItemRect(i);
                if (rectangle.Contains(e.Location))
                {
                    //Write your code here
                    System.Diagnostics.Debug.WriteLine(orders[i].id);

//                    openOrder(orders[listView1.SelectedItems[0].Index]);
                    return;
                }
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //  orderStages[comboBox1.SelectedIndex]
                bool isCompleted = dataAccess.PatchOrderStage(orders[listView1.SelectedItems[0].Index].id.ToString(), (comboBox1.SelectedIndex + 1).ToString(), JWTtoken);

                if (isCompleted)
                {
                    LoadFormData();
                }
            } catch (Exception) { }
        }

            private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            try
            { 
            if(orders[listView1.SelectedItems[0].Index] != null) {

                openOrder(orders[listView1.SelectedItems[0].Index]);
            }
            } catch (Exception)
            {
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadFormData();
        }

        private void openOrder(Order order)
        {
            new OrderDetailForm(order, this).Show();
            //Hide();
        }
    }
}
