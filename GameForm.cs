using System.Drawing.Drawing2D;
using System.Numerics;
using System.Text.RegularExpressions;

namespace chronosguessr
{
    public partial class GameForm : Form
    {
        private int maxRounds = 5;
        private int currentPlay = 0;
        private int overallScore = 0;

        public GameForm()
        {
            InitializeComponent();

            this.Size = getScreenSize();
            this.Text = "Chronos Guessr";
            globalData.previousSize = this.Size;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            OutputBoxInit();
            Output.OutputText("Welcome to Chronos Guessr!", outputLabel);

            MapInit();
            GuessButtonInit();
            PositionLabelInit();

            RoundUiInit();
            ScoreUiInit();
            this.FormClosed += GameForm_FormClosed;
            Map.MouseClick += Map_MouseClick;

            SetNewImage();
        }

        private void Map_MouseClick(object? sender, MouseEventArgs e)
        {
            Point relativeClickPoint = e.Location;

            // check if click is inside picture
            if (relativeClickPoint.X >= 0 && relativeClickPoint.X < Map.Width &&
                relativeClickPoint.Y >= 0 && relativeClickPoint.Y < Map.Height)
            {
                int scaledX = (int)((float)relativeClickPoint.X / Map.Width * 1024);
                int scaledY = (int)((float)relativeClickPoint.Y / Map.Height * 1024);
                Output.OutputText($"CLICKED AT: {scaledX - 512}, {scaledY - 512}", outputLabel);
                globalData.currGuess = new Vector2(scaledX - 512, scaledY - 512);
                SpawnPin(new Vector2(Map.Location.X + relativeClickPoint.X, Map.Location.Y + relativeClickPoint.Y));
                PositionLabel.Text = $"{scaledX - 512}, {scaledY - 512}";
                PositionLabel.Location = new Point(Map.Left + (Map.Width / 2) - (PositionLabel.Width / 2), Map.Top - PositionLabel.Height - 5);
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
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

        private void MapInit()
        {
            Size screen = getScreenSize();

            Size buttonSize = new Size(0, (int)(screen.Height / 20 * globalData.UiScale));
            Map.Size = new Size((int)(screen.Width / 4 * globalData.UiScale), (int)(screen.Width / 4 * globalData.UiScale));
            Map.Image = Properties.Resources.ChronosMapAlpha;
            Map.BorderStyle = BorderStyle.None;
            Map.Location = new Point(screen.Width - Map.Width - 10, (int)(screen.Height - Map.Height - GetTaskbarSize() - (buttonSize.Height * 1.5) - (screen.Height / 108)));

            Map.MouseEnter += Map_MouseEnter;
            Map.MouseLeave += Map_MouseLeave;
        }

        private void Map_MouseLeave(object? sender, EventArgs e)
        {
            Map.Image = Properties.Resources.ChronosMapAlpha;
        }

        private void Map_MouseEnter(object? sender, EventArgs e)
        {
            Map.Image = Properties.Resources.ChronosMap;
        }

        private void GuessButtonInit()
        {
            Size screen = getScreenSize();
            GuessButton.Text = "Guess";
            GuessButton.Font = new Font("Minecraft Ten", (float)Math.Round(screen.Width / 100.0) * globalData.UiScale, FontStyle.Regular);
            GuessButton.Size = new Size(Map.Width, (int)(screen.Height / 20 * globalData.UiScale));
            GuessButton.FlatStyle = FlatStyle.Flat;
            GuessButton.FlatAppearance.BorderSize = 0;
            GuessButton.BackColor = Color.FromArgb(100, ColorTranslator.FromHtml("#007FE0"));
            GuessButton.ForeColor = ColorTranslator.FromHtml("#F7FAFD");
            GuessButton.FlatAppearance.MouseOverBackColor = GuessButton.BackColor;
            GuessButton.FlatAppearance.CheckedBackColor = GuessButton.BackColor;
            GuessButton.Location = new Point(Map.Left, Map.Bottom + 5);

            GuessButton.Paint += GuessButton_Paint;
        }

        private void GuessButton_Paint(object? sender, PaintEventArgs e)
        {
            Button button = (Button)sender;
            int borderRadius = 10;

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

        private Panel RoundRight;
        private Panel RoundCentre;
        private Panel RoundLeft;
        private Label RoundCount;

        private Panel ScoreRight;
        private Panel ScoreCentre;
        private Panel ScoreLeft;
        private Label ScoreCount;

        private void RoundUiInit()
        {
            Size screen = getScreenSize();

            RoundRight = new Panel();
            RoundRight.BackColor = Color.FromArgb(100, Color.Black);
            RoundRight.BorderStyle = BorderStyle.None;
            RoundRight.Size = new Size(screen.Width / 120, screen.Height / 60);
            RoundRight.Location = new Point(screen.Width - RoundRight.Width - 5, RoundRight.Height / 2 + 5);

            RoundCentre = new Panel();
            RoundCentre.BackColor = Color.FromArgb(100, Color.Black);
            RoundCentre.BorderStyle = BorderStyle.None;
            RoundCentre.Size = new Size(screen.Width / 15, screen.Height / 30);
            RoundCentre.Location = new Point(RoundRight.Left - RoundCentre.Width, 0 + 5);

            RoundLeft = new Panel();
            RoundLeft.BackColor = Color.FromArgb(100, Color.Black);
            RoundLeft.BorderStyle = BorderStyle.None;
            RoundLeft.Size = RoundRight.Size;
            RoundLeft.Location = new Point(RoundCentre.Left - RoundLeft.Width, RoundLeft.Height / 2 + 5);

            RoundCount = new Label();
            RoundCount.BackColor = Color.FromArgb(100, Color.Black);
            RoundCount.ForeColor = ColorTranslator.FromHtml("#F7FAFD");
            RoundCount.BorderStyle = BorderStyle.None;
            RoundCount.Text = $"Round {currentPlay}/{maxRounds}";
            RoundCount.Font = new Font("Minecraft Ten", getScreenSize().Height / 75, FontStyle.Regular);
            RoundCount.Size = RoundCentre.Size;
            RoundCount.TextAlign = ContentAlignment.MiddleCenter;
            RoundCount.Location = new Point(RoundCentre.Location.X + (RoundCentre.Width / 2) - (RoundCount.Width / 2), RoundCentre.Location.Y + (RoundCentre.Height / 2) - (RoundCount.Height / 2));

            this.Controls.Add(RoundCentre);
            this.Controls.Add(RoundRight);
            this.Controls.Add(RoundLeft);
            this.Controls.Add(RoundCount);
            RoundCount.BringToFront();
        }

        private void ScoreUiInit()
        {
            Size screen = getScreenSize();

            ScoreRight = new Panel();
            ScoreRight.BackColor = Color.FromArgb(100, Color.Black);
            ScoreRight.BorderStyle = BorderStyle.None;
            ScoreRight.Size = RoundRight.Size;
            ScoreRight.Location = new Point(RoundLeft.Left - ScoreRight.Width - 10, RoundRight.Location.Y);

            ScoreCentre = new Panel();
            ScoreCentre.BackColor = Color.FromArgb(100, Color.Black);
            ScoreCentre.BorderStyle = BorderStyle.None;
            ScoreCentre.Size = RoundCentre.Size;
            ScoreCentre.Location = new Point(ScoreRight.Left - ScoreCentre.Width, RoundCentre.Location.Y);

            ScoreLeft = new Panel();
            ScoreLeft.BackColor = Color.FromArgb(100, Color.Black);
            ScoreLeft.BorderStyle = BorderStyle.None;
            ScoreLeft.Size = RoundLeft.Size;
            ScoreLeft.Location = new Point(ScoreCentre.Left - ScoreLeft.Width, RoundLeft.Location.Y);

            ScoreCount = new Label();
            ScoreCount.BackColor = Color.FromArgb(100, Color.Black);
            ScoreCount.ForeColor = ColorTranslator.FromHtml("#F7FAFD");
            ScoreCount.Font = new Font("Minecraft Ten", getScreenSize().Height / 75, FontStyle.Regular);
            ScoreCount.TextAlign = ContentAlignment.MiddleCenter;
            ScoreCount.Text = $"{globalData.score} Points";
            ScoreCount.BorderStyle = BorderStyle.None;
            ScoreCount.Size = ScoreCentre.Size;
            ScoreCount.Location = new Point(ScoreCentre.Location.X + (ScoreCentre.Width / 2) - (ScoreCount.Width / 2), ScoreCentre.Location.Y + (ScoreCentre.Height / 2) - (ScoreCount.Height / 2));

            this.Controls.Add(ScoreLeft);
            this.Controls.Add(ScoreRight);
            this.Controls.Add(ScoreCentre);
            this.Controls.Add(ScoreCount);
            ScoreCount.BringToFront();
        }
        private void OutputBoxInit()
        {
            Size screen = getScreenSize();
            outputLabel.Size = new Size(screen.Width / 4, screen.Height);
            outputLabel.Location = new Point(0, -15);
        }
        private void PositionLabelInit()
        {
            PositionLabel.TextAlign = ContentAlignment.MiddleCenter;
            PositionLabel.Text = "0, 0";
            PositionLabel.Font = new Font("Minecraft Ten", getScreenSize().Height / 50, FontStyle.Regular);
            PositionLabel.BackColor = Color.Transparent;
            PositionLabel.ForeColor = ColorTranslator.FromHtml("#F7FAFD");
            PositionLabel.Size = new Size(Map.Width, TextRenderer.MeasureText(PositionLabel.Text, PositionLabel.Font).Height);
            PositionLabel.Location = new Point(Map.Left + (Map.Width / 2) - (PositionLabel.Width / 2), Map.Top - PositionLabel.Height - 5);
        }
        public static Size getScreenSize()
        {
            Screen mainScreen = Screen.PrimaryScreen;
            //return Screen.PrimaryScreen.WorkingArea.Size;
            return new Size(mainScreen.Bounds.Width, mainScreen.Bounds.Height);
            //return new Size(1000, 1000);
        }

        public static int GetTaskbarSize()
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int workingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;

            return height - workingAreaHeight;
        }
        private Vector2 GetImageLocation(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            Match match = Regex.Match(fileName, @"(-?\d+),(-?\d+)");
            if (match.Success && match.Groups.Count == 3)
            {
                int x = int.Parse(match.Groups[1].Value);
                int y = int.Parse(match.Groups[2].Value);

                Vector2 vector = new Vector2(x, y);
                return vector;
            }
            else
            {
                return new Vector2(0, 0);
            }
        }

        private List<int> previousSelections = new List<int>();

        private void SetNewImage()
        {
            string[] images = Directory.GetFiles("./images/images");
            int totalImages = images.Length;

            if (previousSelections.Count >= totalImages)
            {
                previousSelections.Clear();
            }

            int randomIndex;

            do
            {
                randomIndex = new Random().Next(0, totalImages);
            } while (previousSelections.Contains(randomIndex));

            this.BackgroundImage = new System.Drawing.Bitmap(images[randomIndex]);
            Output.OutputText($"Loaded image: {images[randomIndex]}", outputLabel);
            Output.OutputText($"Image location: {GetImageLocation(images[randomIndex])}", outputLabel);
            globalData.imagePos = GetImageLocation(images[randomIndex]);
            currentPlay += 1;
            UpdateUi();



            previousSelections.Add(randomIndex);
            PositionLabel.Text = "0, 0";
            PositionLabel.Location = new Point(Map.Left + (Map.Width / 2) - (PositionLabel.Width / 2), Map.Top - PositionLabel.Height - 5);
        }

        private void UpdateUi()
        {
            RoundCount.Text = $"Round {currentPlay}/{maxRounds}";
            ScoreCount.Text = $"{globalData.score} Points";
        }
        private void PinInit()
        {
            Panel pin = globalData.currPin;
            pin.Cursor = Cursors.Hand;
            pin.MouseEnter += Pin_MouseEnter;
        }

        private void Pin_MouseEnter(object? sender, EventArgs e)
        {
            Map.Image = Properties.Resources.ChronosMap;
        }

        private void SpawnPin(Vector2 position)
        {
            if (globalData.currPin != null)
            {
                globalData.currPin.Location = new Point((int)position.X - globalData.currPin.Width / 2, (int)position.Y - globalData.currPin.Height / 2);
            }
            else
            {
                Panel pin2 = new Panel();
                pin2.BorderStyle = BorderStyle.None;
                pin2.Size = new Size(14, 14);
                pin2.BackColor = Color.Transparent;
                pin2.Location = new Point((int)position.X - pin2.Width / 2, (int)position.Y - pin2.Height / 2);
                pin2.BackgroundImage = Properties.Resources.ChronosPin;

                this.Controls.Add(pin2);
                pin2.BringToFront();
                globalData.currPin = pin2;
                PinInit();
            }

            int scaledX = (int)((float)(globalData.currPin.Location.X - globalData.currPin.Width / 2) / Map.Width * 1024);
            int scaledY = (int)((float)(globalData.currPin.Location.X - globalData.currPin.Height / 2) / Map.Height * 1024);
            Output.OutputText($"Spawned pin at: {position}", outputLabel);
            Output.OutputText($"pin is at {scaledX - Map.Location.X - 512}, {scaledY - Map.Location.Y - 512}", outputLabel);
        }

        private void GuessButton_Click(object sender, EventArgs e)
        {
            if (globalData.currPin != null)
            {
                CalcPoints(new Point(globalData.currPin.Location.X - Map.Location.X, globalData.currPin.Location.Y - Map.Location.Y));
                this.Controls.Remove(globalData.currPin);
                globalData.currPin = null;
                SetNewImage();
            }
        }

        private void CalcPoints(Point pos)
        {
            Size screen = getScreenSize();
            Vector2 imagePos = globalData.imagePos;

            int dx = (int)(imagePos.X - globalData.currGuess.X);
            int dy = (int)(imagePos.Y - globalData.currGuess.Y);
            double distance = Math.Sqrt(dx * dx + dy * dy);
            int score = (int)Math.Max(500 - distance, 0);

            overallScore += score;
            globalData.score = overallScore;
            Output.OutputText($"Distance: {distance}", outputLabel);
            Output.OutputText($"new score: {overallScore}", outputLabel);
            Output.OutputText($"added score: {score}", outputLabel);
            NotificationUtils.SpawnNotif($"+{score}", 1000, screen.Width / 2 - TextRenderer.MeasureText($"+{score}", new Font("Minecraft Ten", screen.Width / 100, FontStyle.Regular)).Width / 2, 50, this, outputLabel);
        }

        private void PositionLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
