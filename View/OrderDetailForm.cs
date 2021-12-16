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
    public partial class OrderDetailForm : Form
    {
        private Form previousForm;
        private Order order;
        public OrderDetailForm(Order order, Form form)
        {
            this.order = order;
            this.previousForm = form;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            float price = 0;
            this.button1.MouseClick += new MouseEventHandler(buttonClose_MouseClick);
            this.label5.Text = order.customer.firstName + " " + order.customer.lastName;
            this.label6.Text = order.customer.address.street + ", " + order.customer.address.appartment;
            this.label4.Text = order.restaurant.name;
            this.label9.Text = order.customer.phoneNumber;
            
            foreach(OrderLine dish in order.orderLines)
            {
                string[] var = { dish.dish.name, dish.quantity.ToString(), dish.dish.price.ToString() };
                items.Add(new ListViewItem(var));
                price += dish.dish.price;
            }
            foreach (ListViewItem item in items)
            {
                this.listView1.Items.Add(item);
            }

            this.label7.Text = price.ToString();

        }

        private void buttonClose_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
            
        }

    }
}
