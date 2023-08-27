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
                SpawnPin(new Vector2(Map.Location.X + relativeClickPoint.X, Map.Location.Y + relativeClickPoint.Y));

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
            Map.Size = new Size(screen.Width / 4, screen.Width / 4);
            Map.BorderStyle = BorderStyle.FixedSingle;
            Map.Location = new Point(screen.Width - Map.Width - 10, screen.Height - Map.Height - screen.Height / 8);
        }

        private void GuessButtonInit()
        {
            Size screen = getScreenSize();
            GuessButton.Text = "Guess";
            GuessButton.Font = new Font("Minecraft Ten", (float)Math.Round(screen.Width / 100.0), FontStyle.Regular);
            GuessButton.Size = new Size(Map.Width, screen.Height / 20);
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

        private void OutputBoxInit()
        {
            Size screen = getScreenSize();
            outputLabel.Size = new Size(screen.Width / 4, screen.Height);
            outputLabel.Location = new Point(0, -15);
        }
        public static Size getScreenSize()
        {
            Screen mainScreen = Screen.PrimaryScreen;
            return new Size(mainScreen.Bounds.Width, mainScreen.Bounds.Height);
            //return new Size(1000, 1000);
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

            previousSelections.Add(randomIndex);
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
            }
            Output.OutputText($"Spawned pin at: {position}", outputLabel);
            Output.OutputText($"pin is at {globalData.currPin.Location.X - Map.Location.X - globalData.currPin.Width / 2}, {globalData.currPin.Location.Y - Map.Location.Y - globalData.currPin.Height / 2}", outputLabel);
        }

        private void GuessButton_Click(object sender, EventArgs e)
        {
            if (globalData.currPin != null)
            {
                CalcPoints(new Point(globalData.currPin.Location.X - Map.Location.X, globalData.currPin.Location.Y - Map.Location.Y));
            }
        }

        private void CalcPoints(Point pos)
        {
            int scaledX = (int)((float)(pos.X - globalData.currPin.Width / 2) / Map.Width * 1024) - 512;
            int scaledY = (int)((float)(pos.Y - globalData.currPin.Height / 2) / Map.Height * 1024) - 512;
            Vector2 imagePos = globalData.imagePos;
            int dx = (int)(imagePos.X - scaledX);
            int dy = (int)(imagePos.Y - scaledY);
            int distance = (int)Math.Sqrt(dx * dx + dy * dy);
            int score = (int)Math.Round(1000.0 / (distance + 1));

            overallScore += score;
            Output.OutputText($"Distance: {distance}", outputLabel);
            Output.OutputText($"new score: {overallScore}", outputLabel);
            Output.OutputText($"added score: {score}", outputLabel);
        }
    }
}
