namespace chronosguessr
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();

            this.Size = getScreenSize();
            this.Text = "Chronos Guessr";
            globalData.previousSize = this.Size;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.FormClosed += GameForm_FormClosed;
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

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        public static Size getScreenSize()
        {
            Screen mainScreen = Screen.PrimaryScreen;
            return new Size(mainScreen.Bounds.Width, mainScreen.Bounds.Height);
            //return new Size(1000, 1000);
        }
    }
}
