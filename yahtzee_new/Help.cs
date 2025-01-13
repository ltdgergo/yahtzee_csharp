using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace yahtzee_new
{
    internal class Help
    {
        public void ShowScoringHelpDialog()
        {
            var helpForm = new Form
            {
                Text = "Score Rules",
                Size = new Size(800, 950),
                StartPosition = FormStartPosition.CenterParent,
                AutoScroll = true
            };

            var flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown
            };

            #region Rules

            void AddRuleWithExample(string title, int[] exampleDice, string scoringDescription)
            {
                var rulePanel = new TableLayoutPanel
                {
                    AutoSize = true,
                    ColumnCount = 2,
                    RowCount = 2,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Dock = DockStyle.Top
                };

                rulePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                rulePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

                rulePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                rulePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                var titleLabel = new Label
                {
                    Text = title,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    AutoSize = true,
                    Dock = DockStyle.Top
                };

                var examplePanel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(5)
                };

                foreach (var diceValue in exampleDice)
                {
                    var pictureBox = new PictureBox
                    {
                        Image = ResizeImageFromResource($"yahtzee_new.pictures.dice{diceValue}.png", 40, 40),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Size = new Size(40, 40),
                        Margin = new Padding(5)
                    };
                    examplePanel.Controls.Add(pictureBox);
                }

                var scorePanel = new Label
                {
                    Text = scoringDescription,
                    Font = new Font("Arial", 8, FontStyle.Italic),
                    AutoSize = true,
                    Margin = new Padding(5)
                };

                rulePanel.Controls.Add(titleLabel, 0, 0);
                rulePanel.SetColumnSpan(titleLabel, 2);

                rulePanel.Controls.Add(examplePanel, 0, 1);
                rulePanel.Controls.Add(scorePanel, 1, 1);

                rulePanel.SizeChanged += (sender, e) =>
                {
                    int maxWidth = Math.Max(examplePanel.Width, scorePanel.Width);
                    rulePanel.ColumnStyles[0].Width = maxWidth;
                    rulePanel.ColumnStyles[1].Width = maxWidth;
                };

                flowLayoutPanel.Controls.Add(rulePanel);
            }

            AddRuleWithExample(
                "Facenumbers (Pl. Ones, Twos, etc.)"
                ,new[] { 6, 6 }
                ,"The total value of dice\nwith the specified number."
            );

            AddRuleWithExample(
                "Three of a Kind"
                ,new[] { 4, 4, 4, 5, 1 }
                , "The total value of all dice\nif there are three of the same number."
            );

            AddRuleWithExample(
                "Four of a Kind"
                ,new[] { 5, 5, 5, 5, 1 }
                , "The total value of all dice\nif there are four of the same number."
            );

            AddRuleWithExample(
                "Full House"
                ,new[] { 3, 3, 3, 5, 5 }
                , "25 points\nif there are\nthree of the same number and two of another."
            );

            AddRuleWithExample(
                "Small Straight"
                ,new[] { 1, 2, 3, 4 }
                ,"30 points\nif there are four consecutive numbers."

            );

            AddRuleWithExample(
                "Large Straight"
                ,new[] { 2, 3, 4, 5, 6 }
                , "40 points\nif there are five consecutive numbers."
            );

            AddRuleWithExample(
                "Yahtzee"
                ,new[] { 5, 5, 5, 5, 5 }
                , "50 points\nif all dice show the same number."
            );

            AddRuleWithExample(
            "Bonus"
            , new[] { 1, 2, 3, 4, 5, 6 }
            , "35 points\nif the sum of facenumber scores\nare more than 63."
             );
            #endregion

            helpForm.Controls.Add(flowLayoutPanel);
            helpForm.ShowDialog();
        }

        private Image ResizeImageFromResource(string resourcePath, int width, int height)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"The embedded resource was not found: {resourcePath}");
                }

                using (var originalImage = Image.FromStream(stream))
                {
                    return new Bitmap(originalImage, new Size(width, height));
                }
            }
        }

    }
}
