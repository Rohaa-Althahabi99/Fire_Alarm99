using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Guna.UI2.WinForms;

namespace Smok_Fire_Yolo11_Live
{
    public partial class Main : Form
    {
        private VideoCaptureDevice videoSource;
        private TcpListener tcpListener;
        private Thread listenerThread;

        public Main()
        {
            InitializeComponent();

        }

        // بدء استقبال الإشعارات عند تحميل النموذج
        private void Main_Load(object sender, EventArgs e)
        {
            StartListening();
        }

        // بدء الاستماع لإشعارات الحريق
        private void StartListening()
        {
            tcpListener = new TcpListener(IPAddress.Any, 12345);
            tcpListener.Start();

            listenerThread = new Thread(ListenForMessages)
            {
                IsBackground = true
            };
            listenerThread.Start();
        }
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
                videoBox.Image = frame;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying video frame: " + ex.Message);
            }
        }
        // الاستماع للرسائل الواردة
        private void ListenForMessages()
        {
            while (true)
            {
                try
                {
                    using (var client = tcpListener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    {
                        byte[] data = new byte[256];
                        int bytes = stream.Read(data, 0, data.Length);
                        string message = Encoding.ASCII.GetString(data, 0, bytes);

                        if (message.Contains("Fire Alert"))
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show("تم الكشف عن حريق!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                PlayAlarmSound();
                                DisplayFireImage();
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // تشغيل صوت الإنذار
        private void PlayAlarmSound()
        {
            try
            {
                using (var player = new SoundPlayer(@"E:\Python_Project1\Smok_Fire_Yolo11_Live\Smok_Fire_Yolo11_Live\Resources\383254__klankbeeld__fire-alarm.wav"))
                {
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تشغيل الصوت: " + ex.Message);
            }
        }

        // عرض صورة الحريق
        private void DisplayFireImage()
        {
            try
            {
                string fireImagePath = @"C:\path_to_fire_image.png";
                if (System.IO.File.Exists(fireImagePath))
                {
                    pictureBox1.Image = Image.FromFile(fireImagePath);
                }
                else
                {
                    MessageBox.Show("لم يتم العثور على صورة الحريق.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في عرض الصورة: " + ex.Message);
            }
        }
        
        
        // تشغيل الكاميرا
        private void btnStartCamera_Click_1(object sender, EventArgs e)
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                lblCameraStatus.Text = "الكاميرا متصلة";
            }
            else
            {
                MessageBox.Show("لم يتم العثور على كاميرا متصلة.");
            }
        }
        // إيقاف الكاميرا
        private void btnStopCamera_Click_1(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                lblCameraStatus.Text = "الكاميرا غير متصلة";
            }
        }
        // زر للانتقال إلى نموذج الدعم
        private void btnSupport_Click_1(object sender, EventArgs e)
        {
            Support support = new Support();
            this.Hide();
            support.Show();
        }
        // زر للإشعارات المستقبلية
        private void btnReporte_Click(object sender, EventArgs e)
        {
            MessageBox.Show("سيتم إضافة هذا قريباً...");
        }
        // تحديث إطار الفيديو
        private void videoBox_Click(object sender, AForge.Video.NewFrameEventArgs e)
        {
            videoBox.Image = (Bitmap)e.Frame.Clone();
        }

        private void Main_Load_1(object sender, EventArgs e)
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping camera: " + ex.Message);
            }

            try
            {
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping TCP listener: " + ex.Message);
            }

            try
            {
                if (listenerThread != null && listenerThread.IsAlive)
                {
                    listenerThread.Abort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping listener thread: " + ex.Message);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSupport_Click(object sender, EventArgs e)
        {
            PlayAlarmSound();
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            if (this.BackColor == Color.White)
            {
                // تفعيل الوضع الليلي
                this.BackColor = Color.Black;
                btnMode.Text = "الوضع النهاري";
                guna2HtmlLabel3.ForeColor = Color.White;
                pictureBox2.Image = Properties.Resources.dark_mode;
                pictureBox2.Tag = "night";
            }
            else
            {
                // تفعيل الوضع النهاري
                this.BackColor = Color.White;
                btnMode.Text = "الوضع الليلي";
                guna2HtmlLabel3.ForeColor = Color.Black;
                pictureBox2.Image = Properties.Resources.sunlight;
                pictureBox2.Tag = "day";
            }
            pictureBox2.Refresh();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login=new LoginForm();
            login.Show();
        }
    }
}
