namespace chronosguessr
{
    public class NotificationUtils
    {
        public static void SpawnNotif(string message, int duration, int x, int y, Form form, Label output)
        {
            Label notificationLabel = new Label();
            notificationLabel.Text = message;
            notificationLabel.Font = new Font("Minecraft Ten", Form1.getScreenSize().Width / 100, FontStyle.Regular);
            notificationLabel.ForeColor = ColorTranslator.FromHtml("#F7FAFD");
            notificationLabel.BackColor = Color.Transparent; // Start with a transparent background
            notificationLabel.Size = TextRenderer.MeasureText(message, notificationLabel.Font);
            notificationLabel.Location = new System.Drawing.Point(x, y);
            form.Controls.Add(notificationLabel);

            int fadeDuration = 500; // Duration for fade-in and fade-out (in milliseconds)
            int fadeStep = 10;      // Opacity change step for each tick

            bool fadingIn = true;
            bool fadingOut = false;
            bool holding = false;
            int holdingCount = duration / 25;
            int holdCounter = 0;

            globalData.notifPlaying = true;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 25; // Adjust the interval to achieve the desired fade effect

            int currentOpacity = 0;
            timer.Tick += (s, e) =>
            {
                if (currentOpacity <= 100 && fadingIn) // Fade-in
                {
                    currentOpacity += fadeStep;
                    Output.OutputText($"FADING IN: {currentOpacity}", output);
                    if (currentOpacity >= 100)
                    {
                        currentOpacity = 100;
                        fadingIn = false;
                        holding = true;
                        Output.OutputText("SET HOLDING TO TRUE", output);
                    }
                }
                else if (holdCounter < holdingCount && holding) // Hold for the specified duration
                {
                    holdCounter++;
                    Output.OutputText($"HOLDING: {holdCounter}", output);
                }
                else if (holdCounter >= holdingCount && holding) // Transition to fading out
                {
                    holdCounter = duration / 25;
                    holding = false;
                    fadingOut = true;
                }
                else if (currentOpacity > 0 && fadingOut) // Fade-out
                {
                    currentOpacity -= fadeStep;
                    Output.OutputText($"FADING OUT: {currentOpacity}", output);
                    if (currentOpacity < 0)
                        currentOpacity = 0;
                }
                else
                {
                    timer.Stop();
                    form.Controls.Remove(notificationLabel);
                    notificationLabel.Dispose();
                    Output.OutputText($"REMOVED NOTIFICATION: {message}", output);
                    globalData.notifPlaying = false;
                }

                notificationLabel.BackColor = Color.FromArgb(currentOpacity, ColorTranslator.FromHtml("#4892cb"));
            };

            timer.Start();
        }
    }
}
