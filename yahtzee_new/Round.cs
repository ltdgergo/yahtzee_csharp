using yahtzee_new;

public class Round
{
    private FullGameMng gameManager;
    private DiceSet diceSet;
    public bool isFirstRoll;
    private dgvGamePlay dataGridView;
    public int rollCount;
    public Button btnRoll;

    public Round(DiceSet diceSet, dgvGamePlay dgvGamePlay, Button btnRoll, FullGameMng gameManager)
    {
        this.gameManager = gameManager;
        this.diceSet = diceSet;
        this.dataGridView = dgvGamePlay;
        this.btnRoll = btnRoll;
    }

    public void ResetRound()
    {
        btnRoll.Click -= btnRoll_Click;
        diceSet.ClearDiceSelection();
        diceSet.ResetDice();
        btnRoll.Enabled = true;
        isFirstRoll = true;
        rollCount = 1;
        UpdateRollButtonText();
        btnRoll.Click += btnRoll_Click;
    }

    #region btnRoll
    public void btnRoll_Click(object? sender, EventArgs? e)
    {
        if (sender == null || e == null)
        {
            throw new ArgumentNullException("Sender or event arguments cannot be null.");
        }

        if (isFirstRoll)
        {
            diceSet.FirstRoll();
            isFirstRoll = false;
        }
        else
        {
            diceSet.NotFirstRoll();
        }

        int[] diceResults = diceSet.diceResults;
        int currentPlayerIndex = gameManager.GetCurrentPlayerIndex();

        UpdatePossibleScores(diceResults, currentPlayerIndex);
        diceSet.ClearDiceSelection();

        if (rollCount >= 3)
        {
            DisableRollButton();
            return;
        }

        rollCount++;
        UpdateRollButtonText();
    }

    public void UpdateRollButtonText()
    {
        btnRoll.Font = new Font(btnRoll.Font, FontStyle.Bold);
        btnRoll.Text = $"{gameManager.GetCurrentPlayerName()} {rollCount}. roll";
    }

    private void DisableRollButton()
    {
        btnRoll.Enabled = false;
        btnRoll.Text = $"{gameManager.GetCurrentPlayerName()},\nchoose a score!";
    }

    #endregion

    public bool CheckIfHasMoreRolls()
    {
        if (rollCount < 3) // If less than 3 rolls have occurred
        {
            DialogResult result = MessageBox.Show
            (
                "Are you sure you want to fix the selected score? You still have more rolls left!",
                "Fix Score",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            return result == DialogResult.Yes;
        }

        return true;
    }

    public void UpdatePossibleScores(int[] diceResults, int currentPlayerIndex)
    {
        dataGridView.UpdatePossibleScores(diceResults, currentPlayerIndex);
    }
}
