using System.Drawing.Drawing2D;
using System.Numerics;

namespace chronosguessr
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            //set fullscreen
            this.Size = getScreenSize();
            this.Text = "Chronos Guessr";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // init
            PlayButtonInit();
            OutputBoxInit();
            NameLabelInit();
            NameBoxInit();
            SettingsGearInit();

            playButton.Paint += playButton_Paint;

            this.FormClosed += Form1_FormClosed;

            storeLog.logs = LogOrigSizes();

        }

        public static class storeLog
        {
            public static Log[] logs;
        }

        private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
        {
            List<Form> formsToModify = new List<Form>();

            foreach (Form openForm in Application.OpenForms)
            {
                formsToModify.Add(openForm);
            }

            foreach (Form form in formsToModify)
            {
                form.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size screen = getScreenSize();

            if (!Directory.Exists("./images"))
            {
                NotificationUtils.SpawnNotif("Downloading Images", 2000, screen.Width / 2 - TextRenderer.MeasureText("Downloading Images", new Font("Minecraft Ten", screen.Width / 100, FontStyle.Regular)).Width / 2, 50, this, outputLabel);
                GitUtils.CloneRepository("https://github.com/x4caa/chronos-guessr-images/", "./images");
                if (Directory.Exists("./images/.git"))
                {
                    GitUtils.DeleteDirectory("./images/.git");
                }
                globalData.doneDownloading = true;
            }
            else globalData.doneDownloading = true;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Size screen = getScreenSize();
            if (nameBox.Text.Length <= 0)
            {
                NotificationUtils.SpawnNotif("Please Add a Name", 1000, screen.Width / 2 - TextRenderer.MeasureText("Please Add a Name", new Font("Minecraft Ten", screen.Width / 100, FontStyle.Regular)).Width / 2, 50, this, outputLabel);
            }
            else if (nameBox.Text.Length <= 3)
            {
                NotificationUtils.SpawnNotif("Name Must Be Longer Than 3 Characters", 1000, screen.Width / 2 - TextRenderer.MeasureText("Name Must Be Longer Than 3 Characters", new Font("Minecraft Ten", screen.Width / 100, FontStyle.Regular)).Width / 2, 50, this, outputLabel);
            }
            else if (nameBox.Text.Length <= 16 && nameBox.Text.Length > 0 && globalData.doneDownloading)
            {
                Form gameform = new GameForm();
                gameform.Show();
                this.Hide();
            }
        }

        private void PlayButtonInit()
        {
            Size screen = getScreenSize();
            playButton.Text = "Play";

            //font size is 50 on 1080p, 33 on 720p
            playButton.Font = new Font("Minecraft Ten", (float)Math.Round(screen.Width / 38.4), FontStyle.Bold);
            playButton.Size = new Size(screen.Width / 3, screen.Height / 8);
            playButton.FlatAppearance.BorderSize = 0;
            playButton.BackColor = ColorTranslator.FromHtml("#007FE0");
            playButton.ForeColor = ColorTranslator.FromHtml("#F7FAFD");

            playButton.Location = new Point(screen.Width / 2 - playButton.Width / 2, screen.Height / 2 - playButton.Height / 2 - screen.Height / 10);
        }

        private void playButton_Paint(object sender, PaintEventArgs e)
        {
            Button button = (Button)sender;
            int borderRadius = 15;

            Rectangle bounds = new Rectangle(0, 0, button.Width, button.Height);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(bounds.Left, bounds.Top, borderRadius * 2, borderRadius * 2, 180, 90);
                path.AddArc(bounds.Right - borderRadius * 2, bounds.Top, borderRadius * 2, borderRadius * 2, 270, 90);
                path.AddArc(bounds.Right - borderRadius * 2, bounds.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
                path.CloseFigure();

                button.Region = new Region(path);
            }
        }

        private bool SettingsOpen = false;
        private bool SettingsSpawned = false;
        private void SettingsGearInit()
        {
            Size screen = getScreenSize();
            SettingsGear.Image = Properties.Resources.SettingsGear;
            SettingsGear.BorderStyle = BorderStyle.None;
            SettingsGear.BackColor = Color.Transparent;
            SettingsGear.SizeMode = PictureBoxSizeMode.StretchImage;
            SettingsGear.Size = new Size(screen.Width / 40, screen.Width / 40);
            SettingsGear.Location = new Point(screen.Width - SettingsGear.Width - 5, 5);

            SettingsGear.Click += SettingsGear_Click;
        }

        private void SettingsGear_Click(object? sender, EventArgs e)
        {
            if (!SettingsOpen && !SettingsSpawned)
            {
                SettingsOpen = true;
                SettingsSpawned = true;
                Size screen = getScreenSize();
                Panel SettingBack = new Panel();
                SettingBack.Name = "SMenuBack";
                SettingBack.BorderStyle = BorderStyle.None;
                SettingBack.BackColor = ColorTranslator.FromHtml("#102638");
                SettingBack.Size = new Size(screen.Width / 6, screen.Height / 4);
                SettingBack.Location = new Point(screen.Width - SettingBack.Width - 5, SettingsGear.Bottom + 5);
                SettingBack.Paint += SettingBack_Paint;

                Label SettingLabel = new Label();
                SettingLabel.Name = "SMenuLabel";
                SettingLabel.BorderStyle = BorderStyle.None;
                SettingLabel.BackColor = SettingBack.BackColor;
                SettingLabel.ForeColor = ColorTranslator.FromHtml("#F7FAFD");
                SettingLabel.Text = "SETTINGS";
                SettingLabel.Font = new Font("Minecraft Ten", screen.Width / 100, FontStyle.Bold);
                Size SettingLabelSize = TextRenderer.MeasureText(SettingLabel.Text, SettingLabel.Font);
                SettingLabel.Size = SettingLabelSize;
                SettingLabel.Location = new Point(screen.Width - SettingBack.Width / 2 - SettingLabel.Width / 2 - 5, SettingBack.Top + 2);

                Label SettingScale = new Label();
                SettingScale.Name = "SMenuScale";
                SettingScale.BorderStyle = BorderStyle.None;
                SettingScale.BackColor = SettingBack.BackColor;
                SettingScale.ForeColor = SettingLabel.ForeColor;
                SettingScale.Text = "UI Scale";
                SettingScale.Font = new Font("Mojangles", screen.Width / 125, FontStyle.Regular);
                Size ScaleLabelSize = TextRenderer.MeasureText(SettingScale.Text, SettingScale.Font);
                SettingScale.Size = ScaleLabelSize;
                SettingScale.Location = new Point(SettingBack.Left + 2, SettingLabel.Bottom + 2);

                TrackBar SettingScaleBar = new TrackBar();
                SettingScaleBar.Name = "SMenuScaleBar";
                SettingScaleBar.BackColor = SettingBack.BackColor;
                SettingScaleBar.Maximum = 3;
                SettingScaleBar.Minimum = 1;
                SettingScaleBar.Value = 2;
                SettingScaleBar.Size = new Size(SettingBack.Width - SettingScale.Width - 2, SettingScale.Height);
                SettingScaleBar.Location = new Point(SettingScale.Right, SettingScale.Location.Y);
                SettingScaleBar.ValueChanged += SettingScaleBar_ValueChanged;

                this.Controls.Add(SettingBack);
                this.Controls.Add(SettingLabel);
                this.Controls.Add(SettingScale);
                this.Controls.Add(SettingScaleBar);
                SettingLabel.BringToFront();
                SettingScale.BringToFront();
                SettingScaleBar.BringToFront();
            }
            else if (!SettingsOpen && SettingsSpawned)
            {
                SettingsOpen = true;
                foreach (Control control in this.Controls)
                {
                    if (control.Name.Contains("SMenu"))
                    {
                        control.Show();
                    }
                }
            }
            else
            {
                SettingsOpen = false;
                foreach (Control control in this.Controls)
                {
                    if (control.Name.Contains("SMenu"))
                    {
                        control.Hide();
                    }
                }
            }

        }

        private void SettingScaleBar_ValueChanged(object? sender, EventArgs e)
        {
            TrackBar bar = (TrackBar)sender;
            ResizeUi(bar.Value);
        }

        private void SettingBack_Paint(object? sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            int borderRadius = 15;

            Rectangle bounds = new Rectangle(0, 0, panel.Width, panel.Height);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(bounds.Left, bounds.Top, borderRadius * 2, borderRadius * 2, 180, 90);
                path.AddArc(bounds.Right - borderRadius * 2, bounds.Top, borderRadius * 2, borderRadius * 2, 270, 90);
                path.AddArc(bounds.Right - borderRadius * 2, bounds.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
                path.CloseFigure();

                panel.Region = new Region(path);
            }
        }

        private void NameLabelInit()
        {
            Size screen = getScreenSize();

            nameLabel.Text = "NAME";
            nameLabel.Font = new Font("Minecraft Ten", (float)Math.Round(screen.Width / 100.0), FontStyle.Bold);
            nameLabel.ForeColor = ColorTranslator.FromHtml("#f7fafd");
            nameLabel.BackColor = Color.Transparent;
            nameLabel.Size = TextRenderer.MeasureText(nameLabel.Text, nameLabel.Font);
            nameLabel.Location = new Point(screen.Width / 2 - nameLabel.Width / 2, playButton.Bottom + 5);

        }
        private void NameBoxInit()
        {
            Size screen = getScreenSize();

            nameBox.Multiline = false;
            nameBox.MaxLength = 16;
            nameBox.Font = new Font("Mojangles", (float)Math.Round(screen.Width / 120.0), FontStyle.Underline);
            nameBox.Size = new Size((int)(playButton.Width / 1.5), TextRenderer.MeasureText("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", nameBox.Font).Height);
            nameBox.BackColor = ColorTranslator.FromHtml("#102638");
            nameBox.ForeColor = ColorTranslator.FromHtml("#f7fafd");
            nameBox.BorderStyle = BorderStyle.None;
            nameBox.TextAlign = HorizontalAlignment.Center;
            nameBox.Location = new Point(screen.Width / 2 - nameBox.Width / 2, nameLabel.Bottom + 2);
        }

        private void OutputBoxInit()
        {
            Size screen = getScreenSize();
            outputLabel.Size = new Size(screen.Width / 4, screen.Height);
            outputLabel.Location = new Point(0, -15);
        }

        public class Log
        {
            public string name { get; set; }
            public Size size { get; set; }
            public Font? font { get; set; }

        }
        private Log[] LogOrigSizes()
        {
            Log[] log = new Log[20];

            Log OutputLog = new Log();
            OutputLog.name = "Output"; OutputLog.size = outputLabel.Size;
            log[0] = OutputLog;

            Log PlayLog = new Log();
            PlayLog.name = "Play"; PlayLog.size = playButton.Size; PlayLog.font = playButton.Font;
            log[1] = PlayLog;

            Log NameLabelLog = new Log();
            NameLabelLog.name = "NameLabel"; NameLabelLog.size = nameLabel.Size; NameLabelLog.font = nameLabel.Font;
            log[2] = NameLabelLog;

            Log NameBoxLog = new Log();
            NameBoxLog.name = "NameBox"; NameBoxLog.size = nameBox.Size; NameBoxLog.font = nameBox.Font;
            log[3] = NameBoxLog;

            return log;
        }

        private void ResizeUi(int scaleInt)
        {
            Size screen = getScreenSize();

            switch (scaleInt)
            {
                case 1:
                    globalData.UiScale = 0.75f;
                    break;
                case 2:
                    globalData.UiScale = 1.0f;
                    break;
                default:
                    globalData.UiScale = 1.25f;
                    break;
            }

            float scale = globalData.UiScale;

            outputLabel.Size = new Size((int)(storeLog.logs[0].size.Width * scale), (int)(storeLog.logs[0].size.Height * scale));

            playButton.Size = new Size((int)(storeLog.logs[1].size.Width * scale), (int)(storeLog.logs[1].size.Height * scale));
            playButton.Location = new Point(screen.Width / 2 - playButton.Width / 2, screen.Height / 2 - playButton.Height / 2 - screen.Height / 10);
            playButton.Font = new Font(storeLog.logs[1].font.Name, storeLog.logs[1].font.Size * scale, FontStyle.Bold);

            nameLabel.Font = new Font(storeLog.logs[2].font.Name, storeLog.logs[2].font.Size * scale, FontStyle.Bold);
            nameLabel.Size = new Size((int)(storeLog.logs[2].size.Width * scale), (int)(storeLog.logs[2].size.Height * scale));
            nameLabel.Location = new Point(screen.Width / 2 - nameLabel.Width / 2, playButton.Bottom + 5);

            nameBox.Font = new Font(storeLog.logs[3].font.Name, storeLog.logs[3].font.Size * scale, FontStyle.Underline);
            nameBox.Size = new Size((int)(storeLog.logs[3].size.Width * scale), (int)(storeLog.logs[3].size.Height * scale));
            nameBox.Location = new Point(screen.Width / 2 - nameBox.Width / 2, nameLabel.Bottom + 2);
        }
        public static Size getScreenSize()
        {
            return Screen.PrimaryScreen.Bounds.Size;
        }
    }

    public class RoundedTextBox : TextBox
    {
        private int borderRadius = 10;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, borderRadius * 2, borderRadius * 2, 180, 90);
                path.AddArc(Width - borderRadius * 2, 0, borderRadius * 2, borderRadius * 2, 270, 90);
                path.AddArc(Width - borderRadius * 2, Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
                path.AddArc(0, Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
                path.CloseFigure();

                Region = new Region(path);

                using (Pen pen = new Pen(BorderColor))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private Color borderColor = Color.Gray;

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }
    }

    public static class globalData
    {
        public static Size previousSize;
        public static bool notifPlaying;
        public static bool doneDownloading = false;
        public static Panel currPin;
        public static Vector2 imagePos;
        public static Vector2 currGuess;
        public static float UiScale = 1.0f;
        public static int score = 0;
    }
}