using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace yahtzee_new
{
    public class Player
    {
        public string PlayerName { get; set; }
        public ScoreCalculator ScoreCalculator { get; set; }

        public Player(string playerName)
        {
            PlayerName = playerName;
            ScoreCalculator = new ScoreCalculator(this); // Associated ScoreCalculator
        }

        public static List<string> ShowDialog(string caption = "Players")
        {
            List<string> playerNames;

            do
            {
                playerNames = new List<string>();

                Form prompt = new Form()
                {
                    Width = 350,
                    Height = 340,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };

                Label instructionLabel = new Label()
                {
                    Text = "Who is Playing?",
                    Left = 20,
                    Top = 10,
                    Width = 300
                };
                prompt.Controls.Add(instructionLabel);

                TextBox[] inputBoxes = new TextBox[5];
                for (int i = 0; i < 5; i++)
                {
                    Label nameLabel = new Label()
                    {
                        Text = $"Player {i + 1}:",
                        Left = 20,
                        Top = 40 + (i * 40),
                        Width = 100
                    };

                    inputBoxes[i] = new TextBox()
                    {
                        Left = 120,
                        Top = 40 + (i * 40),
                        Width = 180
                    };

                    prompt.Controls.Add(nameLabel);
                    prompt.Controls.Add(inputBoxes[i]);
                }

                Button confirmation = new Button()
                {
                    Text = "Start!",
                    Left = 250,
                    Width = 80,
                    Top = 240,
                    DialogResult = DialogResult.OK
                };

                confirmation.Click += (sender, e) =>
                {
                    string firstPlayerName = inputBoxes[0].Text.Trim();
                    if (string.IsNullOrWhiteSpace(firstPlayerName))
                    {
                        MessageBox.Show("The first player's name is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    playerNames.AddRange(inputBoxes
                        .Select(tb => tb.Text.Trim())
                        .Where(name => !string.IsNullOrWhiteSpace(name)));

                    prompt.DialogResult = DialogResult.OK;
                };

                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                if (prompt.ShowDialog() == DialogResult.Cancel)
                {
                    return new List<string>();
                }

            } while (!playerNames.Any());

            return playerNames;
        }
    }
}
