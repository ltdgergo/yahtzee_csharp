using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace yahtzee_new
{
    public class dgvGamePlay : DataGridView
    {
        private Player currentPlayer;
        public dgvGamePlay()
        {
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.ReadOnly = true;
            this.AllowUserToAddRows = false;
            this.RowHeadersVisible = false;
            this.ColumnAdded += (s, e) => e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #region initializadgv
        public void InitializeDataGridView(List<string> playerNames, Size formSize)
        {
            this.Rows.Clear();
            this.Columns.Clear();

            var scoreColumn = new DataGridViewTextBoxColumn
            {
                Name = "ScoreName",
                HeaderText = "Score Name",
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                HeaderCell = new DataGridViewColumnHeaderCell
                {
                    Style = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                }
            };
            this.Columns.Add(scoreColumn);

            InitializeScoreNames();

            InitializePlayerNames(playerNames);

            this.Height = this.Rows.Count * 33;
            this.Width = this.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width);

            ApplyTotalRowStyles();

            CenterDataGridView(formSize);
        }
        private void InitializeScoreNames()
        {
            string[] scoreNames = new string[]
            {
                "1", "2", "3", "4", "5", "6",
                "Bonus", "Total Upper",
                "Three of a Kind", "Four of a Kind", "Full House", "Small Straight", "Large Straight", "Yahtzee", "Chance",
                "Total Lower",
                "Grand Total"
            };
            foreach (var name in scoreNames)
            {
                this.Rows.Add(name);
            }
        }
        private void InitializePlayerNames(List<string> playerNames)
        {
            foreach (var playerName in playerNames)
            {
                var playerColumn = new DataGridViewTextBoxColumn
                {
                    Name = playerName,
                    HeaderText = playerName,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                    HeaderCell = new DataGridViewColumnHeaderCell
                    {
                        Style = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                    }
                };
                this.Columns.Add(playerColumn);
            }
        }

        private void ApplyTotalRowStyles()
        {
            foreach (DataGridViewRow row in this.Rows)
            {
                var cellValue = row.Cells[0].Value?.ToString() ?? string.Empty;

                // If "Total" is found in the cell text
                if (cellValue.Contains("Total"))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.Font = new Font(this.Font, FontStyle.Bold); // Bold font
                    }
                }
            }
        }


        private void CenterDataGridView(Size formSize)
        {
            // Center the form based on its size
            int centerX = (formSize.Width - this.Width) / 2;
            int centerY = (formSize.Height - this.Height) / 2;
            centerY += 110;
            this.Location = new Point(centerX, centerY);
        }

        #endregion

        public void UpdatePossibleScores(int[] diceResults, int currentPlayerIndex)
        {
            ScoreCalculator calculator = new ScoreCalculator(currentPlayer);

            var possibleScores = new Dictionary<string, int>
            {
                { "1", calculator.Facenumbers(diceResults, 1) },
                { "2", calculator.Facenumbers(diceResults, 2) },
                { "3", calculator.Facenumbers(diceResults, 3) },
                { "4", calculator.Facenumbers(diceResults, 4) },
                { "5", calculator.Facenumbers(diceResults, 5) },
                { "6", calculator.Facenumbers(diceResults, 6) },
                { "Three of a Kind", calculator.ThreeOfAKind(diceResults) },
                { "Four of a Kind", calculator.FourOfAKind(diceResults) },
                { "Full House", calculator.FullHouse(diceResults) },
                { "Small Straight", calculator.SmallStraight(diceResults) },
                { "Large Straight", calculator.LargelStraight(diceResults) },
                { "Yahtzee", calculator.Yahtzee(diceResults) },
                { "Chance", calculator.Chance(diceResults) }
            };

            int targetColumnIndex = currentPlayerIndex + 1;// The first column is the score category name, followed by the player columns

            foreach (DataGridViewRow row in this.Rows)
            {
                string scoreName = row.Cells[0].Value?.ToString() ?? string.Empty;  // Default to empty string if null

                if (!string.IsNullOrEmpty(scoreName) && possibleScores.ContainsKey(scoreName))
                {
                    var cell = row.Cells[targetColumnIndex];

                    // Check if the cell is "Fixed" status
                    if (cell.Tag != null && cell.Tag.ToString() == "Fixed")
                    {
                        continue; // If it's fixed, skip it from recalculation
                    }

                    // Update the score
                    cell.Value = possibleScores[scoreName];
                    cell.Style.ForeColor = Color.Lime;
                }

            }
        }
        public bool FixSelectedScore(int currentPlayerIndex)
        {
            if (this.CurrentCell == null)
            {
                MessageBox.Show("No score selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if the selected cell is in the current player's column
            int targetColumnIndex = currentPlayerIndex + 1; // Category column is at index 0
            if (this.CurrentCell.ColumnIndex != targetColumnIndex)
            {
                MessageBox.Show("The selected cell is not in the player's column!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if the cell is already fixed
            if (this.CurrentCell.Tag != null && this.CurrentCell.Tag.ToString() == "Fixed")
            {
                MessageBox.Show(
                    "The score has already been fixed! Please select another one.",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return false;
            }

            // Check if the cell has a value
            if (this.CurrentCell.Value == null || string.IsNullOrWhiteSpace(this.CurrentCell.Value.ToString()))
            {
                MessageBox.Show("There is no score in the selected cell!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if the score is valid
            if (!int.TryParse(this.CurrentCell.Value.ToString(), out int selectedScore))
            {
                MessageBox.Show("The score in the selected cell is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // If the score is 0, ask the user for confirmation
            if (selectedScore == 0)
            {
                DialogResult result = MessageBox.Show(
                    "The selected score is 0. Are you sure you want to fix it?",
                    "Fix Score",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                {
                    return false; // The user does not want to fix the 0 score
                }
            }

            // Fix the score
            this.CurrentCell.Style.ForeColor = Color.Gray;
            this.CurrentCell.Tag = "Fixed"; // Mark as fixed

            return true;
        }

        public void ClearAllPossibleScores(int currentPlayerIndex)
        {
            int targetColumnIndex = currentPlayerIndex + 1; // Player's column

            foreach (DataGridViewRow row in this.Rows)
            {
                if (row.Cells.Count > targetColumnIndex)
                {
                    var cell = row.Cells[targetColumnIndex];

                    if (cell.Tag == null || !cell.Tag.Equals("Fixed"))
                    {
                        cell.Value = null;
                    }
                }
            }
        }

        public void UpdateTotals(int currentPlayerIndex, Player currentPlayer, ScoreCalculator calculator)
        {
            calculator.SetPlayer(currentPlayer);

            int targetColumnIndex = currentPlayerIndex + 1; // Player's column (1-based index)

            // Clear existing scores to ensure they are recalculated
            calculator.faceScores.Clear();
            calculator.lowerScores.Clear();

            // Loop through the rows to update scores
            foreach (DataGridViewRow row in this.Rows)
            {
                if (row.Cells[targetColumnIndex].Tag?.ToString() == "Fixed" &&
                    int.TryParse(row.Cells[targetColumnIndex].Value?.ToString(), out int score))
                {
                    string scoreName = row.Cells[0].Value?.ToString() ?? string.Empty;

                    // If it's an upper section score (1-6), add it to faceScores
                    if (int.TryParse(scoreName, out _))
                    {
                        calculator.faceScores.Add(score);
                    }
                    // If it's a lower section score (e.g., Yahtzee, Three of a Kind), add to lowerScores
                    else if (scoreName == "Three of a Kind" || scoreName == "Four of a Kind" ||
                             scoreName == "Full House" || scoreName == "Small Straight" ||
                             scoreName == "Large Straight" || scoreName == "Yahtzee" || scoreName == "Chance")
                    {
                        calculator.lowerScores.Add(score);
                    }
                }
            }

            // Update the totals based on the current scores
            calculator.UpdateTotals();

            // Update the DataGridView with the new totals
            foreach (DataGridViewRow row in this.Rows)
            {
                string scoreName = row.Cells[0].Value?.ToString() ?? string.Empty;

                // Update the Bonus row
                if (scoreName == "Bonus")
                {
                    row.Cells[targetColumnIndex].Value = calculator.CalculateBonus();
                }
                // Update the Total Upper row
                else if (scoreName == "Total Upper")
                {
                    row.Cells[targetColumnIndex].Value = calculator.CalculateUpperTotal();
                }
                // Update the Total Lower row
                else if (scoreName == "Total Lower")
                {
                    row.Cells[targetColumnIndex].Value = calculator.CalculateLowerTotal();
                }
                // Update the Grand Total row
                else if (scoreName == "Grand Total")
                {
                    row.Cells[targetColumnIndex].Value = calculator.CalculateGrandTotal();
                }
            }
        }

        public void HighlightCurrentPlayerHeader(int currentPlayerIndex)
        {
            // The current player's column
            int targetColumnIndex = currentPlayerIndex + 1;

            this.EnableHeadersVisualStyles = false; // Disable default styles

            foreach (DataGridViewColumn column in this.Columns)
            {
                column.HeaderCell.Style.BackColor = Color.White; // Default background color
                column.HeaderCell.Style.ForeColor = Color.Black; // Default text color
                column.HeaderCell.Style.Font = new Font(this.DefaultCellStyle.Font, FontStyle.Regular); // Regular font
            }

            // Highlight the current player's column
            if (targetColumnIndex < this.Columns.Count)
            {
                this.Columns[targetColumnIndex].HeaderCell.Style.Font = new Font(this.DefaultCellStyle.Font, FontStyle.Bold); // Bold font
                this.Columns[targetColumnIndex].HeaderCell.Style.ForeColor = Color.RoyalBlue; // Blue text
            }
        }
    }
}
