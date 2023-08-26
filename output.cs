namespace chronosguessr
{
    public partial class Output
    {
        public static void OutputText(string text, TextBox outputBox)
        {
            string currentText = outputBox.Text;
            if (GetLineCount(currentText) >= 50)
            {
                currentText = RemoveTopmostLine(currentText);
                // using StreamWriter writer = new(@"C:\Users\Jax\Downloads\log.txt", false);
                // writer.WriteLine("This is a message to the log file.");
            }
            outputBox.Text = currentText + "\r\n" + DateTime.Now.ToString() + " - " + text;
        }

        static int GetLineCount(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Length;
        }

        static string RemoveTopmostLine(string input)
        {
            int newIndex = input.IndexOf('\n');
            if (newIndex >= 0)
            {
                return input.Substring(newIndex + 1);
            }
            else
            {
                return input;
            }
        }
    }
}
