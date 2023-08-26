using System.Drawing.Drawing2D;

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
            globalData.previousSize = this.Size;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            PlayButtonInit();
            _ = leaderboardLabelInitAsync();
            OutputBoxInit();
            NameLabelInit();
            NameBoxInit();


            playButton.Paint += playButton_Paint;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void playButton_Click(object sender, EventArgs e)
        {

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
            Output.OutputText($"RESIZED BUTTON: {playButton.Size}", outputLabel);
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
        private async Task leaderboardLabelInitAsync()
        {
            Size screen = getScreenSize();

            leaderboardLabel.TextAlign = ContentAlignment.TopCenter;
            Task<string> leaderboard = GitUtils.GetFile("https://raw.githubusercontent.com/x4caa/chronos-guessr/master/leaderboard.txt");
            string[] leaderboardLines = await GitUtils.GetLines(leaderboard);
            leaderboardLabel.Text =
                "Leaderboard\r\n" +
                $"1. {leaderboardLines[0]}\r\n" +
                $"2. {leaderboardLines[1]}\r\n" +
                $"3. {leaderboardLines[2]}\r\n" +
                $"4. {leaderboardLines[3]}\r\n" +
                $"5. {leaderboardLines[4]}";
            leaderboardLabel.Font = new Font("Minecraft Ten", (float)Math.Round(screen.Width / 100.0), FontStyle.Regular);
            leaderboardLabel.ForeColor = ColorTranslator.FromHtml("#F7FAFD");

            Size textSize = TextRenderer.MeasureText(leaderboardLabel.Text, leaderboardLabel.Font);
            leaderboardLabel.Size = new Size(textSize.Width, textSize.Height);

            leaderboardLabel.Location = new Point(screen.Width - textSize.Width - screen.Width / 50, screen.Height / 2 - textSize.Height / 2);
            Output.OutputText("RESIZED LEADERBOARD TEXT", outputLabel);
        }

        private void OutputBoxInit()
        {
            Size screen = getScreenSize();
            outputLabel.Size = new Size(screen.Width / 4, screen.Height);
            outputLabel.Location = new Point(0, -15);
        }

        private Size getScreenSize()
        {
            Screen mainScreen = Screen.PrimaryScreen;
            return new Size(mainScreen.Bounds.Width, mainScreen.Bounds.Height);
            //return new Size(1000, 1000);
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

    static class globalData
    {
        public static Size previousSize;
        public static int targetY;
        public static int TargetY;
        public static int initialY;
    }
}