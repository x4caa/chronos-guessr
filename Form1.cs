namespace chronosguessr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //set fullscreen
            this.Size = getScreenSize();
            globalData.previousSize = this.Size;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            PlayButtonInit();
            LeaderboardTextInit();
            OutputBoxInit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void playButton_Click(object sender, EventArgs e)
        {

        }
        private void leaderboardText_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlayButtonInit()
        {
            Size screen = getScreenSize();
            playButton.Text = "Play";
            //font size is 50 on 1080p, 33 on 720p
            playButton.Font = new Font("Minecraft Ten", (float)Math.Round(screen.Width / 38.4), FontStyle.Bold);
            playButton.Size = new Size(screen.Width / 3, screen.Height / 8);

            playButton.Location = new Point(screen.Width / 2 - playButton.Width / 2, screen.Height / 2 - playButton.Height / 2 - screen.Height / 10);
            Output.OutputText($"RESIZED BUTTON: {playButton.Size}", outputBox);
        }

        private void LeaderboardTextInit()
        {
            Size screen = getScreenSize();
            leaderboardText.Multiline = true;
            leaderboardText.ReadOnly = true;
            leaderboardText.TabStop = true;
            leaderboardText.Size = new Size(playButton.Width, screen.Height / 5);

            leaderboardText.Location = new Point(playButton.Location.X, playButton.Location.Y + playButton.Size.Height + screen.Height / 20);
            Output.OutputText("RESIZED LEADERBOARD TEXT", outputBox);
        }

        private void OutputBoxInit()
        {
            Size screen = getScreenSize();
            outputBox.Multiline = true;
            outputBox.ReadOnly = true;
            outputBox.Size = new Size(screen.Width / 4, screen.Height);
            outputBox.Location = new Point(0, 0);
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