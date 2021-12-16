
namespace DesktopClient
{
    partial class DishForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            this.backButton = new System.Windows.Forms.Button();
            this.addDishButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.dishUpdateButton = new System.Windows.Forms.Button();
            this.deleteDishButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.name = new System.Windows.Forms.ColumnHeader();
            this.price = new System.Windows.Forms.ColumnHeader();
            this.dishCategory = new System.Windows.Forms.ColumnHeader();
            this.description = new System.Windows.Forms.ColumnHeader();
            this.refreshButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 28);
            label1.TabIndex = 0;
            label1.Text = "Dishes";
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Location = new System.Drawing.Point(713, 5);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 1;
            this.backButton.Text = "Go Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // addDishButton
            // 
            this.addDishButton.Location = new System.Drawing.Point(3, 3);
            this.addDishButton.Name = "addDishButton";
            this.addDishButton.Size = new System.Drawing.Size(75, 23);
            this.addDishButton.TabIndex = 2;
            this.addDishButton.Text = "Add Dish";
            this.addDishButton.UseVisualStyleBackColor = true;
            this.addDishButton.Click += new System.EventHandler(this.addDishButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.addDishButton);
            this.flowLayoutPanel1.Controls.Add(this.dishUpdateButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteDishButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 420);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 30);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // dishUpdateButton
            // 
            this.dishUpdateButton.Location = new System.Drawing.Point(84, 3);
            this.dishUpdateButton.Name = "dishUpdateButton";
            this.dishUpdateButton.Size = new System.Drawing.Size(92, 23);
            this.dishUpdateButton.TabIndex = 3;
            this.dishUpdateButton.Text = "Update Dish";
            this.dishUpdateButton.UseVisualStyleBackColor = true;
            this.dishUpdateButton.Click += new System.EventHandler(this.dishUpdateButton_Click);
            // 
            // deleteDishButton
            // 
            this.deleteDishButton.Location = new System.Drawing.Point(182, 3);
            this.deleteDishButton.Name = "deleteDishButton";
            this.deleteDishButton.Size = new System.Drawing.Size(92, 23);
            this.deleteDishButton.TabIndex = 4;
            this.deleteDishButton.Text = "Delete Dish";
            this.deleteDishButton.UseVisualStyleBackColor = true;
            this.deleteDishButton.Click += new System.EventHandler(this.deleteDishButton_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.price,
            this.dishCategory,
            this.description});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 28);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(800, 392);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // name
            // 
            this.name.Text = "Name";
            this.name.Width = 150;
            // 
            // price
            // 
            this.price.Text = "Price";
            this.price.Width = 100;
            // 
            // dishCategory
            // 
            this.dishCategory.Text = "Dish Category";
            this.dishCategory.Width = 200;
            // 
            // description
            // 
            this.description.Text = "Description";
            this.description.Width = 300;
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Location = new System.Drawing.Point(632, 7);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // DishForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.backButton);
            this.Controls.Add(label1);
            this.Name = "DishForm";
            this.Text = "Dishes";
            this.Load += new System.EventHandler(this.DishForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button addDishButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button dishUpdateButton;
        private System.Windows.Forms.Button deleteDishButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader price;
        private System.Windows.Forms.ColumnHeader dishCategory;
        private System.Windows.Forms.ColumnHeader description;
        private System.Windows.Forms.Button refreshButton;
    }
}