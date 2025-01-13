using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yahtzee_new
{
    public class NextRoundButton
    {
        private FullGameMng gameManager;
        private dgvGamePlay dataGridView;
        public Button btnNextRound;
        private Round currentRound;
        private ScoreCalculator scoreCalculator;
        private Player currentPlayer;

        public NextRoundButton(FullGameMng gameManager, dgvGamePlay dataGridView, Button btnNextRound, Round currentRound)
        {
            this.gameManager = gameManager;
            this.dataGridView = dataGridView;
            this.btnNextRound = btnNextRound;
            this.currentRound = currentRound;
            this.currentPlayer = gameManager.GetCurrentPlayer();
            this.scoreCalculator = new ScoreCalculator(currentPlayer);

            UpdateNxtRoundButtonText();

            dataGridView.HighlightCurrentPlayerHeader(gameManager.GetCurrentPlayerIndex());

        }
        public void btnNextRound_Click(object? sender, EventArgs? e)
        {
            if (!currentRound.CheckIfHasMoreRolls()) return;

            var diceSet = gameManager.diceSet;

            scoreCalculator.CheckSecondYahtzee(gameManager.GetCurrentPlayerIndex(), diceSet.diceResults, dataGridView);

            if (!dataGridView.FixSelectedScore(gameManager.GetCurrentPlayerIndex()))
            {
                // In case of failed recording, wait for a new selection
                MessageBox.Show("Select a valid score before moving to the next round!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataGridView.ClearAllPossibleScores(gameManager.GetCurrentPlayerIndex());

            dataGridView.UpdateTotals(gameManager.GetCurrentPlayerIndex(), currentPlayer, scoreCalculator);

            dataGridView.ClearSelection();

            gameManager.StartNewRound();

            UpdateNxtRoundButtonText();

            dataGridView.HighlightCurrentPlayerHeader(gameManager.GetCurrentPlayerIndex());

        }

        private void UpdateNxtRoundButtonText()
        {
            string nextPlayerName = gameManager.GetNextPlayerName();
            int roundCount = gameManager.GetCurrentRoundNumber();

            if (roundCount == 13 && gameManager.GetCurrentPlayerIndex() == gameManager.GetPlayersCount() - 1)
            {
                btnNextRound.Text = "Game over! Summarizing results...";
            }
            else if (gameManager.GetCurrentPlayerIndex() < gameManager.GetPlayersCount() - 1)
            {
                btnNextRound.Text = $"Fix selected score\n{nextPlayerName} will play\nround {roundCount}.";
            }
            else
            {
                btnNextRound.Text = $"Fix selected score\n{nextPlayerName} will play\nround {roundCount + 1}.";
            }
        }
    }
}
