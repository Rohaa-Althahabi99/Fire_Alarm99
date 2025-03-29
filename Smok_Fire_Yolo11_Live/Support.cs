using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Smok_Fire_Yolo11_Live
{
    public partial class Support : Form
    {
        public Support()
        {
            InitializeComponent();
            telegramlk.Click += (sender, e) => OpenUrlWithConfirmation("https://t.me/+OMhhDlW7pws1YjZi", "تيليغرام");
            facebooklk.Click += (sender, e) => OpenUrlWithConfirmation("https://www.facebook.com/yourFacebookPage", "فيسبوك");
            emaillk1.Click += (sender, e) => OpenUrlWithConfirmation("mailto:owl125668@gmail.com", "البريد الإلكتروني");
            emaillk2.Click += (sender, e) => OpenUrlWithConfirmation("https://wa.me/97466321653", "واتساب");
        }

        private void OpenUrlWithConfirmation(string url, string serviceName)
        {
            DialogResult result = MessageBox.Show(
                $"هل تريد فتح {serviceName}؟ سيتم توجيهك الآن.",
                "تأكيد",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    MessageBox.Show($"حدث خطأ أثناء فتح الرابط: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
private void guna2Button1_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main = new Main();
            main.Show();
        }
    }
}
