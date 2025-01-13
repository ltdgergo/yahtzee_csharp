using System.Diagnostics;
using System.Windows.Forms;
using yahtzee_new;

public class FullGameMng
{
    private dgvGamePlay dgvgameplay;
    public DiceSet diceSet;
    private List<Player> players;
    private int currentPlayerIndex;
    private int currentRoundNumber;
    private ScoreCalculator scoreCalculator;
    private Button btnRoll;
    private Round? currentRound;
    private NextRoundButton? nextRoundButton;
    private Button btnNextRound;
    private EndGame endGame;

    public FullGameMng(dgvGamePlay dgv, Button rollbutton, Button btnNextRound, PictureBox[] dicePictureBoxes)
    {

        diceSet = new DiceSet(dicePictureBoxes);
        dgvgameplay = dgv;
        players = new List<Player>();
        currentPlayerIndex = 0;
        currentRoundNumber = 1;
        btnRoll = rollbutton;
        this.btnNextRound = btnNextRound;

    }

    public void StartGame(Size formSize)
    {
        var playerNames = Player.ShowDialog(); // Prompt to enter player names

        players.Clear();

        if (playerNames == null || playerNames.Count == 0)
        {
            throw new InvalidOperationException("No players were provided!");
        }

        foreach (var name in playerNames)
        {
            players.Add(new Player(name));
        }

        if (players.Count > 0)
        {
            dgvgameplay.InitializeDataGridView(playerNames, formSize);
            currentRound = new Round(diceSet, dgvgameplay, btnRoll, this);
            scoreCalculator = new ScoreCalculator(players[currentPlayerIndex]);
            endGame = new EndGame(players);
            currentRound.ResetRound();
            InitializeNextRoundButtonClass(btnNextRound);
        }
        else
        {
            MessageBox.Show("No players were provided!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void InitializeNextRoundButtonClass(Button btnNextRound)
    {
        nextRoundButton = new NextRoundButton(this, dgvgameplay, btnNextRound, currentRound);
        btnNextRound.Click += nextRoundButton.btnNextRound_Click;
    }

    #region playername
    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public string GetCurrentPlayerName()
    {
        if (players == null || players.Count == 0)
        {
            throw new InvalidOperationException("No players available.");
        }
        return players[currentPlayerIndex].PlayerName;
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public string GetNextPlayerName()
    {
        if (players == null || players.Count == 0)
        {
            throw new InvalidOperationException("No players available.");
        }
        if (players.Count == 1)
        {
            return players[currentPlayerIndex].PlayerName;
        }
        int nextPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        return players[nextPlayerIndex].PlayerName;
    }

    public int GetPlayersCount()
    {
        return players.Count;
    }

    #endregion


    public int GetCurrentRoundNumber()
    {
        return currentRoundNumber;
    }

    public void StartNewRound()
    {
        if (currentRoundNumber == 13 && currentPlayerIndex == players.Count - 1)
        {
            // Update the scores for all players
            foreach (var player in players)
            {
                dgvgameplay.UpdateTotals(players.IndexOf(player), player, player.ScoreCalculator);
            }

            // Query GrandTotal based on the updated ScoreCalculator
            var currentGameResults = players.Select(player => (
                PlayerName: player.PlayerName,
                GrandTotal: player.ScoreCalculator.GetGrandTotal()
            )).ToList();

            // End the game
            endGame.EndGameMethod(currentGameResults, dgvgameplay);
            return; // Exit as the game has ended
        }

        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
            currentRoundNumber++;
        }
        currentRound.ResetRound();
    }
}
