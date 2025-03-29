using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smok_Fire_Yolo11_Live
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void CloseBn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginBn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserNameTb.Text) || string.IsNullOrEmpty(PassTb.Text))
            {
                MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // اتصال بقاعدة البيانات
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-6GAPUSI\MYSQLHAWRAA;Initial Catalog=FireAlarm;Integrated Security=True;TrustServerCertificate=True");

            try
            {
                con.Open();
                string query = "SELECT * FROM Login WHERE username=@username AND password=@password";
                SqlCommand cmd = new SqlCommand(query, con);

                // تمرير القيم باستخدام المعاملات
                cmd.Parameters.AddWithValue("@username", UserNameTb.Text);
                cmd.Parameters.AddWithValue("@password", PassTb.Text);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) // إذا وُجد مستخدم مطابق
                {
                    MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // فتح الواجهة الرئيسية
                    Main mainForm = new Main();
                    this.Hide(); // إخفاء فورم تسجيل الدخول
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                PassTb.UseSystemPasswordChar = false; // إظهار النص الحقيقي
            }
            else
            {
                PassTb.UseSystemPasswordChar = true; // إخفاء النص برموز
            }
        }

        private void Forget_PassLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword forgotPasswordForm = new ForgotPassword();
            this.Hide(); // إخفاء نافذة تسجيل الدخول
            forgotPasswordForm.Show();
        }
    }
}
