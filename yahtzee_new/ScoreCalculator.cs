using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace yahtzee_new
{
    public class ScoreCalculator
    {

        private Player currentPlayer;

        // These are the lists where the scores are stored
        public List<int> faceScores = new List<int>();
        public List<int> lowerScores = new List<int>();

        // Totals and bonus
        private int upperTotal = 0;
        private int lowerTotal = 0;
        private int bonus = 0;
        private int grandTotal = 0;
        public ScoreCalculator(Player player)
        {
            currentPlayer = player;
        }

        public void SetPlayer(Player player)
        {
            currentPlayer = player;
        }

        #region CellPoints

        // Upper Scores "1, 2..."
        public int Facenumbers(int[] diceResults, int diceValue)
        {
            return diceResults.Where(x => x == diceValue).Sum();
        }

        // Three of a kind
        public int ThreeOfAKind(int[] diceResults)
        {
            return diceResults.GroupBy(x => x).Any(g => g.Count() >= 3) ? diceResults.Sum() : 0;
        }

        // Four of a kind
        public int FourOfAKind(int[] diceResults)
        {
            return diceResults.GroupBy(x => x).Any(g => g.Count() >= 4) ? diceResults.Sum() : 0;
        }

        // Full House (25 points)
        public int FullHouse(int[] diceResults)
        {
            var groupedResults = diceResults.GroupBy(x => x).OrderByDescending(g => g.Count());
            return (groupedResults.First().Count() == 3 && groupedResults.Last().Count() == 2) ? 25 : 0;
        }

        // Small Straight (30 points)
        public int SmallStraight(int[] diceResults)
        {
            int[][] smallStraights =
            {
                new int[] { 1, 2, 3, 4 },
                new int[] { 2, 3, 4, 5 },
                new int[] { 3, 4, 5, 6 }
            };
            return smallStraights.Any(s => s.All(diceResults.Contains)) ? 30 : 0;
        }

        // Large Straight (40 points)
        public int LargelStraight(int[] diceResults)
        {
            int[][] largeStraights =
            {
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 2, 3, 4, 5, 6 }
            };
            return largeStraights.Any(s => s.All(diceResults.Contains)) ? 40 : 0;
        }

        // Yahtzee (five of a kind, 50 points)
        public int Yahtzee(int[] diceResults)
        {
            return diceResults.Distinct().Count() == 1 ? 50 : 0;
        }
        public void CheckSecondYahtzee(int currentPlayerIndex, int[] diceResults, dgvGamePlay dataGridView)
        {
            int targetColumnIndex = currentPlayerIndex + 1; // Player's column index

            // Check: Are all dice values the same?
            if (diceResults.Distinct().Count() == 1)
            {
                bool hasYahtzee = false;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    string scoreName = row.Cells[0].Value?.ToString() ?? string.Empty;

                    // If there is already a fixed Yahtzee score
                    if (scoreName == "Yahtzee" &&
                        row.Cells[targetColumnIndex].Tag?.ToString() == "Fixed" &&
                        int.TryParse(row.Cells[targetColumnIndex].Value?.ToString(), out int yahtzeeScore) &&
                        yahtzeeScore > 0)
                    {
                        hasYahtzee = true;
                        break;
                    }
                }

                // If there is no Yahtzee score fixed, do not add +100
                if (hasYahtzee)
                {
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        string scoreName = row.Cells[0].Value?.ToString() ?? string.Empty;

                        // If a Yahtzee score is already fixed
                        if (scoreName == "Yahtzee" &&
                            row.Cells[targetColumnIndex].Tag?.ToString() == "Fixed" &&
                            int.TryParse(row.Cells[targetColumnIndex].Value?.ToString(), out int yahtzeeScore) &&
                            yahtzeeScore > 0)
                        {
                            // Add +100 points to the Yahtzee score
                            yahtzeeScore += 100;
                            row.Cells[targetColumnIndex].Value = yahtzeeScore;

                            MessageBox.Show($"Second Yahtzee! +100 points added to the player!",
                                "Yahtzee", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return;
                        }
                    }
                }
            }
        }


        // Chance (sum of all dice)
        public int Chance(int[] diceResults)
        {
            return diceResults.Sum();
        }

        #endregion

        #region Total Calculations


        public int CalculateBonus()
        {
            int upperScore = faceScores.Sum();
            return upperScore >= 63 ? 35 : 0;
        }

        public int CalculateUpperTotal()
        {
            return faceScores.Sum() + CalculateBonus();
        }

        public int CalculateLowerTotal()
        {
            return lowerScores.Sum();
        }

        public int CalculateGrandTotal()
        {
            return CalculateUpperTotal() + CalculateLowerTotal();
        }

        public int GetGrandTotal() => grandTotal;

        public void UpdateTotals()
        {
            bonus = CalculateBonus();
            upperTotal = CalculateUpperTotal();
            lowerTotal = CalculateLowerTotal();
            grandTotal = CalculateGrandTotal();
        }

        #endregion


    }
}
