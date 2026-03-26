using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Security.Policy;
using System.Windows.Forms;

namespace merzzy
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Point lastMousePos;
        DateTime lastMoveTime;
        DateTime lastFXTime;

        int idleSeconds = 5;
        int fxCooldown= 10;
        int counter = 0;
        int checkSuccesfullCounter = 0;

        NotifyIcon trayIcon = null!;
        ContextMenuStrip trayMenu = null!;

        private Random random = new Random();

        private Image[] neutralImages = null!;
        private SoundPlayer[] neutralSounds = null!;

        private Image[] sadImages = null!;
        private SoundPlayer[] sadSounds = null!;

        private Image[] angryImages = null!;
        private SoundPlayer[] angrySounds = null!;

        private Image[] happyImages = null!;
        private SoundPlayer[] happySounds = null!;

        public Form1()
        {
            InitializeComponent();
            SetupWindow();
            SetupResources();
            SetupTray();
            SetupTimer();
            StartMonitoring(this, EventArgs.Empty);
        }

        void SetupWindow()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                Screen.PrimaryScreen!.WorkingArea.Width - this.Width,
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
            trayMenu.Items.Add("Reset", null,   ResetCounter);
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

        void StartMonitoring(object? sender, EventArgs e)
        {
            lastMousePos = Cursor.Position;
            lastMoveTime = DateTime.Now;
            lastFXTime = DateTime.Now;

            timer.Start();
        }

        void StopMonitoring(object? sender, EventArgs e)
        {
            timer.Stop();
            counter = 0;
            this.Hide();
        }

        void ResetCounter(object? sender, EventArgs e)
        {
            counter = 0;
            this.Hide();
        }

        void ExitApp(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        void CheckMouse(object? sender, EventArgs e)
        {
            if (Cursor.Position != lastMousePos)
            {
                lastMousePos = Cursor.Position;
                lastMoveTime = DateTime.Now;

                if (this.Visible)
                {
                    counter--;
                    checkSuccesfullCounter++;
                    this.Hide();
                }

            }

            if(checkSuccesfullCounter == 5)
            {
                checkSuccesfullCounter = 0;
                counter = 0;
            }

            if ((DateTime.Now - lastMoveTime).TotalSeconds > idleSeconds)
            {
                if (!this.Visible)
                {
                    counter++;
                    ShowFX();
                }

                if((DateTime.Now - lastFXTime).TotalSeconds > fxCooldown)
                {
                    ShowFX();
                }


            }
        }

        private void ShowFX()
        {
            lastFXTime = DateTime.Now;

            Image[] selectedImages;
            SoundPlayer[] selectedSounds;

            if (counter <= 0)
            {
                selectedImages = happyImages;
                selectedSounds = happySounds;
            }
            else if (counter == 1)
            {
                selectedImages = neutralImages;
                selectedSounds = neutralSounds;
            }
            else if (counter == 2)
            {
                selectedImages = sadImages;
                selectedSounds = sadSounds;
            }
            else if (counter == 3)
            {
                selectedImages = angryImages;
                selectedSounds = angrySounds;
            }
            else if (counter >= 4)
            {
                selectedImages = angryImages;
                selectedSounds = angrySounds;
                Process.Start(new ProcessStartInfo { FileName = "https://karriere.mcdonalds.de/jobangebote/15/?title=Restaurant-Mitarbeiter", UseShellExecute = true });
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
            int x = Screen.PrimaryScreen!.WorkingArea.Width - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

            if (x < 0) x = 0;
            if (y < 0) y = 0;

            this.Location = new Point(x, y);
        }
    }
}