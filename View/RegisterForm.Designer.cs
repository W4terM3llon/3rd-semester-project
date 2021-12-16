
namespace DesktopClient
{
    partial class RegisterForm
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
            System.Windows.Forms.Label emailLabel;
            System.Windows.Forms.Label pswdLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label pnLabel;
            System.Windows.Forms.Label addressLabel;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label fnLabel;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            this.regButton = new System.Windows.Forms.Button();
            this.emailText = new System.Windows.Forms.TextBox();
            this.pswdText = new System.Windows.Forms.TextBox();
            this.pswdRepeatText = new System.Windows.Forms.TextBox();
            this.pnText = new System.Windows.Forms.TextBox();
            this.streetText = new System.Windows.Forms.TextBox();
            this.appartText = new System.Windows.Forms.TextBox();
            this.lnText = new System.Windows.Forms.TextBox();
            this.fnText = new System.Windows.Forms.TextBox();
            this.cityText = new System.Windows.Forms.TextBox();
            this.zipText = new System.Windows.Forms.TextBox();
            this.backButton = new System.Windows.Forms.Button();
            emailLabel = new System.Windows.Forms.Label();
            pswdLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            pnLabel = new System.Windows.Forms.Label();
            addressLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            fnLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // emailLabel
            // 
            emailLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            emailLabel.AutoSize = true;
            emailLabel.Location = new System.Drawing.Point(90, 85);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new System.Drawing.Size(36, 15);
            emailLabel.TabIndex = 2;
            emailLabel.Text = "Email";
            // 
            // pswdLabel
            // 
            pswdLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            pswdLabel.AutoSize = true;
            pswdLabel.Location = new System.Drawing.Point(90, 132);
            pswdLabel.Name = "pswdLabel";
            pswdLabel.Size = new System.Drawing.Size(57, 15);
            pswdLabel.TabIndex = 4;
            pswdLabel.Text = "Password";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(90, 176);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(89, 15);
            label1.TabIndex = 6;
            label1.Text = "Password again";
            // 
            // pnLabel
            // 
            pnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            pnLabel.AutoSize = true;
            pnLabel.Location = new System.Drawing.Point(90, 223);
            pnLabel.Name = "pnLabel";
            pnLabel.Size = new System.Drawing.Size(86, 15);
            pnLabel.TabIndex = 8;
            pnLabel.Text = "Phone number";
            // 
            // addressLabel
            // 
            addressLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            addressLabel.AutoSize = true;
            addressLabel.Location = new System.Drawing.Point(485, 134);
            addressLabel.Name = "addressLabel";
            addressLabel.Size = new System.Drawing.Size(49, 15);
            addressLabel.TabIndex = 10;
            addressLabel.Text = "Address";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(485, 158);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(37, 15);
            label2.TabIndex = 11;
            label2.Text = "Street";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(485, 204);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(71, 15);
            label3.TabIndex = 13;
            label3.Text = "Appartment";
            // 
            // label11
            // 
            label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(485, 71);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(63, 15);
            label11.TabIndex = 30;
            label11.Text = "Last Name";
            // 
            // fnLabel
            // 
            fnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            fnLabel.AutoSize = true;
            fnLabel.Location = new System.Drawing.Point(485, 24);
            fnLabel.Name = "fnLabel";
            fnLabel.Size = new System.Drawing.Size(64, 15);
            fnLabel.TabIndex = 28;
            fnLabel.Text = "First Name";
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(485, 250);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(28, 15);
            label4.TabIndex = 15;
            label4.Text = "City";
            // 
            // label5
            // 
            label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(485, 300);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(53, 15);
            label5.TabIndex = 17;
            label5.Text = "Zip code";
            // 
            // label6
            // 
            label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label6.Location = new System.Drawing.Point(12, 9);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(199, 46);
            label6.TabIndex = 31;
            label6.Text = "Registration";
            // 
            // regButton
            // 
            this.regButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.regButton.Location = new System.Drawing.Point(349, 384);
            this.regButton.Name = "regButton";
            this.regButton.Size = new System.Drawing.Size(75, 23);
            this.regButton.TabIndex = 0;
            this.regButton.Text = "Register";
            this.regButton.UseVisualStyleBackColor = true;
            this.regButton.Click += new System.EventHandler(this.regButton_Click);
            // 
            // emailText
            // 
            this.emailText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.emailText.Location = new System.Drawing.Point(90, 103);
            this.emailText.Name = "emailText";
            this.emailText.Size = new System.Drawing.Size(171, 23);
            this.emailText.TabIndex = 1;
            // 
            // pswdText
            // 
            this.pswdText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pswdText.Location = new System.Drawing.Point(90, 150);
            this.pswdText.Name = "pswdText";
            this.pswdText.Size = new System.Drawing.Size(171, 23);
            this.pswdText.TabIndex = 3;
            this.pswdText.UseSystemPasswordChar = true;
            // 
            // pswdRepeatText
            // 
            this.pswdRepeatText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pswdRepeatText.Location = new System.Drawing.Point(90, 194);
            this.pswdRepeatText.Name = "pswdRepeatText";
            this.pswdRepeatText.Size = new System.Drawing.Size(171, 23);
            this.pswdRepeatText.TabIndex = 5;
            this.pswdRepeatText.UseSystemPasswordChar = true;
            // 
            // pnText
            // 
            this.pnText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnText.Location = new System.Drawing.Point(90, 241);
            this.pnText.Name = "pnText";
            this.pnText.Size = new System.Drawing.Size(171, 23);
            this.pnText.TabIndex = 7;
            // 
            // streetText
            // 
            this.streetText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.streetText.Location = new System.Drawing.Point(485, 176);
            this.streetText.Multiline = true;
            this.streetText.Name = "streetText";
            this.streetText.Size = new System.Drawing.Size(171, 23);
            this.streetText.TabIndex = 9;
            // 
            // appartText
            // 
            this.appartText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.appartText.Location = new System.Drawing.Point(485, 222);
            this.appartText.Multiline = true;
            this.appartText.Name = "appartText";
            this.appartText.Size = new System.Drawing.Size(171, 23);
            this.appartText.TabIndex = 12;
            // 
            // lnText
            // 
            this.lnText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnText.Location = new System.Drawing.Point(485, 89);
            this.lnText.Name = "lnText";
            this.lnText.Size = new System.Drawing.Size(171, 23);
            this.lnText.TabIndex = 29;
            // 
            // fnText
            // 
            this.fnText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.fnText.Location = new System.Drawing.Point(485, 42);
            this.fnText.Name = "fnText";
            this.fnText.Size = new System.Drawing.Size(171, 23);
            this.fnText.TabIndex = 27;
            // 
            // cityText
            // 
            this.cityText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cityText.Location = new System.Drawing.Point(485, 268);
            this.cityText.Multiline = true;
            this.cityText.Name = "cityText";
            this.cityText.Size = new System.Drawing.Size(171, 23);
            this.cityText.TabIndex = 14;
            // 
            // zipText
            // 
            this.zipText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zipText.Location = new System.Drawing.Point(485, 318);
            this.zipText.Multiline = true;
            this.zipText.Name = "zipText";
            this.zipText.Size = new System.Drawing.Size(171, 23);
            this.zipText.TabIndex = 16;
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Location = new System.Drawing.Point(713, 12);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 32;
            this.backButton.Text = "Go Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.backButton);
            this.Controls.Add(label6);
            this.Controls.Add(label11);
            this.Controls.Add(this.lnText);
            this.Controls.Add(fnLabel);
            this.Controls.Add(this.fnText);
            this.Controls.Add(label5);
            this.Controls.Add(this.zipText);
            this.Controls.Add(label4);
            this.Controls.Add(this.cityText);
            this.Controls.Add(label3);
            this.Controls.Add(this.appartText);
            this.Controls.Add(label2);
            this.Controls.Add(addressLabel);
            this.Controls.Add(this.streetText);
            this.Controls.Add(pnLabel);
            this.Controls.Add(this.pnText);
            this.Controls.Add(label1);
            this.Controls.Add(this.pswdRepeatText);
            this.Controls.Add(pswdLabel);
            this.Controls.Add(this.pswdText);
            this.Controls.Add(emailLabel);
            this.Controls.Add(this.emailText);
            this.Controls.Add(this.regButton);
            this.Name = "RegisterForm";
            this.Text = "Register";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button regButton;
        private System.Windows.Forms.TextBox emailText;
        private System.Windows.Forms.TextBox pswdText;
        private System.Windows.Forms.TextBox pswdRepeatText;
        private System.Windows.Forms.TextBox pnText;
        private System.Windows.Forms.TextBox streetText;
        private System.Windows.Forms.TextBox appartText;
        private System.Windows.Forms.TextBox lnText;
        private System.Windows.Forms.TextBox fnText;
        private System.Windows.Forms.TextBox cityText;
        private System.Windows.Forms.TextBox zipText;
        private System.Windows.Forms.Button backButton;
    }
}