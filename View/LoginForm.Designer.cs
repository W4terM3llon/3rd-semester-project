
namespace DesktopClient
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label pwdLabel;
            System.Windows.Forms.Label emailLabel;
            System.Windows.Forms.Label regTextLabel;
            System.Windows.Forms.Label label1;
            this.loginButton = new System.Windows.Forms.Button();
            this.emailText = new System.Windows.Forms.TextBox();
            this.pwdText = new System.Windows.Forms.TextBox();
            this.registerButton = new System.Windows.Forms.Button();
            pwdLabel = new System.Windows.Forms.Label();
            emailLabel = new System.Windows.Forms.Label();
            regTextLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pwdLabel
            // 
            pwdLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            pwdLabel.AutoSize = true;
            pwdLabel.Location = new System.Drawing.Point(332, 156);
            pwdLabel.Name = "pwdLabel";
            pwdLabel.Size = new System.Drawing.Size(57, 15);
            pwdLabel.TabIndex = 1;
            pwdLabel.Text = "Password";
            // 
            // emailLabel
            // 
            emailLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            emailLabel.AutoSize = true;
            emailLabel.Location = new System.Drawing.Point(332, 112);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new System.Drawing.Size(36, 15);
            emailLabel.TabIndex = 2;
            emailLabel.Text = "Email";
            // 
            // regTextLabel
            // 
            regTextLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            regTextLabel.AutoSize = true;
            regTextLabel.Location = new System.Drawing.Point(293, 248);
            regTextLabel.Name = "regTextLabel";
            regTextLabel.Size = new System.Drawing.Size(171, 15);
            regTextLabel.TabIndex = 5;
            regTextLabel.Text = "Don\'t have an account register:";
            // 
            // loginButton
            // 
            this.loginButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loginButton.Location = new System.Drawing.Point(342, 201);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // emailText
            // 
            this.emailText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.emailText.Location = new System.Drawing.Point(332, 130);
            this.emailText.Name = "emailText";
            this.emailText.Size = new System.Drawing.Size(100, 23);
            this.emailText.TabIndex = 3;
            // 
            // pwdText
            // 
            this.pwdText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pwdText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pwdText.Location = new System.Drawing.Point(332, 172);
            this.pwdText.Name = "pwdText";
            this.pwdText.Size = new System.Drawing.Size(100, 23);
            this.pwdText.TabIndex = 4;
            this.pwdText.UseSystemPasswordChar = true;
            // 
            // registerButton
            // 
            this.registerButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.registerButton.Location = new System.Drawing.Point(342, 266);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 6;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(329, 28);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(103, 46);
            label1.TabIndex = 7;
            label1.Text = "Login";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(label1);
            this.Controls.Add(this.emailText);
            this.Controls.Add(regTextLabel);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(emailLabel);
            this.Controls.Add(this.pwdText);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(pwdLabel);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox emailText;
        private System.Windows.Forms.TextBox pwdText;
        private System.Windows.Forms.Button registerButton;
    }
}

