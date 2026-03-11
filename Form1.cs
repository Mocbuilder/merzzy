using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace merzzy
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Point lastMousePos;
        DateTime lastMoveTime;

        int idleSeconds = 5;
        int counter = 0;

        NotifyIcon trayIcon;
        ContextMenuStrip trayMenu;

        private Random random = new Random();

        private Image[] neutralImages;
        private SoundPlayer[] neutralSounds;

        private Image[] sadImages;
        private SoundPlayer[] sadSounds;

        private Image[] angryImages;
        private SoundPlayer[] angrySounds;

        private Image[] happyImages;
        private SoundPlayer[] happySounds;

        public Form1()
        {
            InitializeComponent();
            SetupWindow();
            SetupResources();
            SetupTray();
            SetupTimer();
        }

        void SetupWindow()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            this.TransparencyKey = this.BackColor;
            this.Hide();
        }

        private void SetupResources()
        {
            //Hardcoded because I don't want to deal with somehow passing strings as property names to get the resources
            neutralImages = new Image[] { Properties.Resources.img_neutral_1, Properties.Resources.img_neutral_2 };
            neutralSounds = new SoundPlayer[] { new SoundPlayer(Properties.Resources.sfx_neutral_1), new SoundPlayer(Properties.Resources.sfx_neutral_2) };

            sadImages = new Image[] { Properties.Resources.img_sad_1, Properties.Resources.img_sad_2 };
            sadSounds = new SoundPlayer[] { new SoundPlayer(Properties.Resources.sfx_sad_1), new SoundPlayer(Properties.Resources.sfx_sad_2) };

            angryImages = new Image[] { Properties.Resources.img_angry_1, Properties.Resources.img_angry_2 };
            angrySounds = new SoundPlayer[] { new SoundPlayer(Properties.Resources.sfx_angry_1), new SoundPlayer(Properties.Resources.sfx_angry_2) };

            happyImages = new Image[] { Properties.Resources.img_happy_1, Properties.Resources.img_happy_2 };
            happySounds = new SoundPlayer[] { new SoundPlayer(Properties.Resources.sfx_happy_1), new SoundPlayer(Properties.Resources.sfx_happy_2) };
        }

        void SetupTray()
        {
            trayMenu = new ContextMenuStrip();

            trayMenu.Items.Add("Start", null, StartMonitoring);
            trayMenu.Items.Add("Stop", null, StopMonitoring);
            trayMenu.Items.Add("Exit", null, ExitApp);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Merzzy";
            trayIcon.Icon = Properties.Resources.bundesschild;
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;
        }

        void SetupTimer()
        {
            lastMousePos = Cursor.Position;
            lastMoveTime = DateTime.Now;

            timer.Interval = 200;
            timer.Tick += CheckMouse;
        }

        void StartMonitoring(object sender, EventArgs e)
        {
            lastMousePos = Cursor.Position;
            lastMoveTime = DateTime.Now;

            timer.Start();
        }

        void StopMonitoring(object sender, EventArgs e)
        {
            timer.Stop();
            counter = 0;
            this.Hide();
        }

        void ExitApp(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        void CheckMouse(object sender, EventArgs e)
        {
            if (Cursor.Position != lastMousePos)
            {
                lastMousePos = Cursor.Position;
                lastMoveTime = DateTime.Now;

                if (this.Visible)
                    this.Hide();
            }

            if ((DateTime.Now - lastMoveTime).TotalSeconds > idleSeconds)
            {
                if (!this.Visible)
                {
                    counter++;
                    ShowFX();
                }
            }
        }

        private void ShowFX()
        {
            Image[] selectedImages;
            SoundPlayer[] selectedSounds;

            if (counter == 1)
            {
                selectedImages = neutralImages;
                selectedSounds = neutralSounds;
            }
            else if (counter == 2)
            {
                selectedImages = sadImages;
                selectedSounds = sadSounds;
            }
            else if (counter >= 3)
            {
                selectedImages = angryImages;
                selectedSounds = angrySounds;
            }
            else
            {
                selectedImages = happyImages;
                selectedSounds = happySounds;
            }

            pbx_mainImage.Image = selectedImages[random.Next(selectedImages.Length)];
            SoundPlayer player = selectedSounds[random.Next(selectedSounds.Length)];

            pbx_mainImage.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Size = pbx_mainImage.Image.Size;

            PositionForm();

            this.Show();
            this.BringToFront();
            this.Refresh();
            player?.Play();
        }

        private void PositionForm()
        {
            int x = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

            if (x < 0) x = 0;
            if (y < 0) y = 0;

            this.Location = new Point(x, y);
        }
    }
}