
namespace DesktopClient
{
    partial class RestForm
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
            this.restName = new System.Windows.Forms.Label();
            this.backButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.bookingsButton = new System.Windows.Forms.Button();
            this.orderButton = new System.Windows.Forms.Button();
            this.dinPerButton = new System.Windows.Forms.Button();
            this.tableButton = new System.Windows.Forms.Button();
            this.dishButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // restName
            // 
            this.restName.Dock = System.Windows.Forms.DockStyle.Top;
            this.restName.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.restName.Location = new System.Drawing.Point(0, 0);
            this.restName.Name = "restName";
            this.restName.Size = new System.Drawing.Size(794, 28);
            this.restName.TabIndex = 0;
            this.restName.Text = "Restaurant name";
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(713, 5);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 1;
            this.backButton.Text = "Go Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.bookingsButton);
            this.flowLayoutPanel1.Controls.Add(this.orderButton);
            this.flowLayoutPanel1.Controls.Add(this.dinPerButton);
            this.flowLayoutPanel1.Controls.Add(this.tableButton);
            this.flowLayoutPanel1.Controls.Add(this.dishButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(794, 377);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // bookingsButton
            // 
            this.bookingsButton.Location = new System.Drawing.Point(3, 3);
            this.bookingsButton.Name = "bookingsButton";
            this.bookingsButton.Size = new System.Drawing.Size(100, 100);
            this.bookingsButton.TabIndex = 0;
            this.bookingsButton.Text = "Bookings";
            this.bookingsButton.UseVisualStyleBackColor = true;
            this.bookingsButton.Click += new System.EventHandler(this.bookingsButton_Click);
            // 
            // orderButton
            // 
            this.orderButton.Location = new System.Drawing.Point(109, 3);
            this.orderButton.Name = "orderButton";
            this.orderButton.Size = new System.Drawing.Size(100, 100);
            this.orderButton.TabIndex = 1;
            this.orderButton.Text = "Orders";
            this.orderButton.UseVisualStyleBackColor = true;
            this.orderButton.Click += new System.EventHandler(this.orderButton_Click);
            // 
            // dinPerButton
            // 
            this.dinPerButton.Location = new System.Drawing.Point(215, 3);
            this.dinPerButton.Name = "dinPerButton";
            this.dinPerButton.Size = new System.Drawing.Size(100, 100);
            this.dinPerButton.TabIndex = 4;
            this.dinPerButton.Text = "Dining Periods";
            this.dinPerButton.UseVisualStyleBackColor = true;
            this.dinPerButton.Click += new System.EventHandler(this.dinPerButton_Click);
            // 
            // tableButton
            // 
            this.tableButton.Location = new System.Drawing.Point(321, 3);
            this.tableButton.Name = "tableButton";
            this.tableButton.Size = new System.Drawing.Size(100, 100);
            this.tableButton.TabIndex = 2;
            this.tableButton.Text = "Tables";
            this.tableButton.UseVisualStyleBackColor = true;
            this.tableButton.Click += new System.EventHandler(this.tableButton_Click);
            // 
            // dishButton
            // 
            this.dishButton.Location = new System.Drawing.Point(427, 3);
            this.dishButton.Name = "dishButton";
            this.dishButton.Size = new System.Drawing.Size(100, 100);
            this.dishButton.TabIndex = 3;
            this.dishButton.Text = "Dishes";
            this.dishButton.UseVisualStyleBackColor = true;
            this.dishButton.Click += new System.EventHandler(this.dishButton_Click);
            // 
            // RestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 405);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.restName);
            this.Name = "RestForm";
            this.Text = "Restaurant";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label restName;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button bookingsButton;
        private System.Windows.Forms.Button orderButton;
        private System.Windows.Forms.Button tableButton;
        private System.Windows.Forms.Button dishButton;
        private System.Windows.Forms.Button dinPerButton;
    }
}