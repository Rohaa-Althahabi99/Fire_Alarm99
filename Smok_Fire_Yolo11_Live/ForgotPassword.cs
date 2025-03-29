using System;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Security;
//using System.Net.Mail;

namespace Smok_Fire_Yolo11_Live
{
    public partial class ForgotPassword : Form
    {
        private string verificationCode;

        public ForgotPassword()
        {
            InitializeComponent();
        }

        // زر الخروج
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // زر إرسال رمز التحقق عبر البريد الإلكتروني
        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text; 

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("الرجاء إدخال البريد الإلكتروني");
                return;
            }

            Random random = new Random();
            verificationCode = random.Next(100000, 999999).ToString();

            try
            {
                await SendVerificationEmail(email, verificationCode);
                MessageBox.Show($"تم إرسال رمز التحقق إلى {email}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل الإرسال: {ex.Message}");
            }
        }

        // دالة الإرسال عبر Gmail SMTP
        private async Task SendVerificationEmail(string email, string code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("System", "your-email@gmail.com")); // استخدم بريدك هنا
            message.To.Add(new MailboxAddress("User", email));
            message.Subject = "رمز التحقق - استعادة كلمة المرور";

            message.Body = new TextPart("plain")
            {
                Text = $"رمز التحقق الخاص بك هو: {code}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // هنا نستخدم رمز التطبيق المكون من 16 حرفًا
                await client.AuthenticateAsync(
                    userName: "owl125668@gmail.com", // نفس البريد المستخدم في From
                    password: "kzjfrsgmuypzzcpc"   // ⭐️ رمز التطبيق هنا (بدون مسافات إذا لم تكن موجودة)
                );

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        // زر التحقق من الرمز
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string enteredCode = codeTextBox.Text;
            if (enteredCode == verificationCode)
            {
                MessageBox.Show("تم التحقق بنجاح!");
                OpenMainForm();
            }
            else
            {
                MessageBox.Show("رمز التحقق غير صحيح!");
            }
        }

        private void OpenMainForm()
        {
            Main mainForm = new Main();
            this.Hide();
            mainForm.Show();
        }
        
    }
}



