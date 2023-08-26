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

            playButton.Location = new Point(screen.Width / 2 - playButton.Width / 2, screen.Height / 2 - playButton.Height / 2 - screen.Height / 10);
            Output.OutputText($"RESIZED BUTTON: {playButton.Size}", outputLabel);
        }

        private async Task leaderboardLabelInitAsync()
        {
            Size screen = getScreenSize();

            leaderboardLabel.TabStop = true;
            leaderboardLabel.TabStop = false;
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

            Size textSize = TextRenderer.MeasureText(leaderboardLabel.Text, leaderboardLabel.Font);
            leaderboardLabel.Size = new Size(textSize.Width, textSize.Height);

            leaderboardLabel.Location = new Point(screen.Width / 2 - textSize.Width / 2, playButton.Location.Y + playButton.Size.Height + screen.Height / 20);
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
        }
    }

    static class globalData
    {
        public static Size previousSize;
        public static int hi = 0;
    }
}