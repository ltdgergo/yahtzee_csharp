using System.Text.Json;
using System.IO;
using yahtzee_new;
using System.Diagnostics;
using System.ComponentModel;

public class EndGame
{
    private List<Player> players;
    private List<GameResult> gameResults = new List<GameResult>();
    private const string FilePath = @"C:\Users\reveszg\source\repos\yahtzee\form\yahtzee_new\yahtzee_new\GameResults.json";

    public EndGame(List<Player> players)
    {
        this.players = players;
    }

    public List<(string PlayerName, int GrandTotal)> GetGrandTotalsFromDataGridView(DataGridView dgvGamePlay)
    {
        var grandTotals = new List<(string PlayerName, int GrandTotal)>();

        // 1. Locate the "Grand Total" row
        DataGridViewRow grandTotalRow = null;
        foreach (DataGridViewRow row in dgvGamePlay.Rows)
        {
            if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "Grand Total")
            {
                grandTotalRow = row;
                break;
            }
        }

        if (grandTotalRow == null)
        {
            throw new Exception("The 'Grand Total' row was not found in the DataGridView.");
        }

        // 2. Read the values from each player's column
        foreach (DataGridViewColumn column in dgvGamePlay.Columns)
        {
            if (column.Index == 0) continue; // The first column is "Score Name", we will skip this

            string playerName = column.HeaderText;
            if (grandTotalRow.Cells[column.Index].Value != null &&
                int.TryParse(grandTotalRow.Cells[column.Index].Value.ToString(), out int grandTotal))
            {
                grandTotals.Add((playerName, grandTotal));
            }
            else
            {
                // If there is no value or it's not a number, consider it as 0
                grandTotals.Add((playerName, 0));
            }
        }
        return grandTotals;
    }

    public void EndGameMethod(List<(string PlayerName, int GrandTotal)> currentGameResults, DataGridView dgvGamePlay)
    {
        // 1. Load previous results from JSON
        LoadResultsFromJson();

        // 2. Set the IsCurrentGame value to false for old records
        foreach (var result in gameResults)
        {
            result.IsCurrentGame = false;
        }

        // 3. Read the Grand Total directly from the DataGridView
        var updatedGameResults = GetGrandTotalsFromDataGridView(dgvGamePlay);

        // 4. List of the current players' names
        var currentPlayerNames = currentGameResults.Select(r => r.PlayerName).ToList();

        // 5. Add new results as separate rows
        foreach (var result in updatedGameResults)
        {
            gameResults.Add(new GameResult
            {
                PlayerName = result.PlayerName,
                GrandTotal = result.GrandTotal,
                GameEndDate = DateTime.Now,
                IsCurrentGame = true
            });
        }

        // 6. Recalculate the rankings
        UpdateRankings();

        // 7. Save the results to JSON
        SaveResultsToJson();

        // 8. Display the results
        ShowGameResultsForm(gameResults, currentPlayerNames); // Pass the currentPlayerNames list
    }

    private void LoadResultsFromJson()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            var loadedResults = JsonSerializer.Deserialize<List<GameResult>>(json);
            if (loadedResults != null)
            {
                gameResults = loadedResults;
            }
        }
    }

    private void SaveResultsToJson()
    {
        var json = JsonSerializer.Serialize(gameResults, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    private void UpdateRankings()
    {
        // Calculate the rankings for all players (in descending order of GrandTotal)
        var allTimeSorted = gameResults
            .OrderByDescending(gr => gr.GrandTotal)
            .ThenBy(gr => gr.GameEndDate)// In case of a tie, rank by time
            .ToList();

        int overallRank = 1;
        for (int i = 0; i < allTimeSorted.Count; i++)
        {
            // If the previous player's score is the same, they receive the same rank
            if (i > 0 && allTimeSorted[i].GrandTotal == allTimeSorted[i - 1].GrandTotal)
            {
                allTimeSorted[i].OverallRanking = allTimeSorted[i - 1].OverallRanking;
            }
            else
            {
                allTimeSorted[i].OverallRanking = overallRank;
            }
            overallRank++;
        }

        // Calculate the current rankings only for the active players
        var currentPlayerSorted = gameResults
            .Where(gr => gr.IsCurrentGame)
            .OrderByDescending(gr => gr.GrandTotal)
            .ThenBy(gr => gr.GameEndDate)
            .ToList();

        int currentRank = 1;
        for (int i = 0; i < currentPlayerSorted.Count; i++)
        {
            // If the previous player's score is the same, they receive the same rank
            if (i > 0 && currentPlayerSorted[i].GrandTotal == currentPlayerSorted[i - 1].GrandTotal)
            {
                currentPlayerSorted[i].CurrentRanking = currentPlayerSorted[i - 1].CurrentRanking;
            }
            else
            {
                currentPlayerSorted[i].CurrentRanking = currentRank;
            }
            currentRank++;
        }

        // Set the updated values in the original `gameResults` list
        foreach (var result in gameResults)
        {
            var overallRankResult = allTimeSorted.FirstOrDefault(r => r == result);
            if (overallRankResult != null)
            {
                result.OverallRanking = overallRankResult.OverallRanking;
            }

            if (result.IsCurrentGame)
            {
                var currentRankResult = currentPlayerSorted.FirstOrDefault(r => r == result);
                if (currentRankResult != null)
                {
                    result.CurrentRanking = currentRankResult.CurrentRanking;
                }
            }
            else
            {
                result.CurrentRanking = null; // Did not play this time
            }
        }
    }


    private void ShowGameResultsForm(List<GameResult> gameResults, List<string> currentPlayerNames)
    {
        // Default filtered list: top 3 players + current players
        var filteredResults = gameResults
            .Where(gr => gr.IsCurrentGame || gr.OverallRanking <= 3) // Filter: current players or top 3
            .OrderBy(gr => gr.OverallRanking)
            .ToList();

        var resultsForm = new Form
        {
            Text = "Game Results",
            AutoSize = true,
            MinimumSize = new Size(600, 400), // Set the minimum size
            Size = new Size(800, 600) // Fix size
        };

        DataGridView resultsGridView = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = false,
            DataSource = filteredResults,  // Directly use the sorted list as the data source
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells // Auto column width
        };

        resultsForm.Controls.Add(resultsGridView);

        // Add Columns
        resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Overall Ranking",
            DataPropertyName = "OverallRanking",  // Directly bind to the property
            Name = "AllTimePosition",
            SortMode = DataGridViewColumnSortMode.Automatic // Enable column sorting
        });

        resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Current Ranking",
            DataPropertyName = "CurrentRanking",  // Bind to the CurrentRanking property
            Name = "CurrentPosition"
        });

        resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Player Name",
            DataPropertyName = "PlayerName",  // Bind to PlayerName
            Name = "PlayerName"
        });

        resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "GrandTotal",
            DataPropertyName = "GrandTotal",  // Bind to GrandTotal
            Name = "GrandTotal"
        });

        resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Date of Game",
            DataPropertyName = "GameEndDate",  // Bind to GameEndDate
            Name = "GameEndDate"
        });

        resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Current Gamr",
            DataPropertyName = "IsCurrentGame", // Bind to IsCurrentGame
            Name = "IsCurrentGame",
            Visible = false
        });

        resultsGridView.CellFormatting += (sender, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= resultsGridView.Rows.Count) return;

            // Check the IsCurrentGame field
            var isCurrentGameCell = resultsGridView.Rows[e.RowIndex].Cells["IsCurrentGame"].Value;
            if (isCurrentGameCell != null && bool.TryParse(isCurrentGameCell.ToString(), out bool isCurrentGame) && isCurrentGame)
            {
                // If it's the current player, apply BOLD formatting
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
        };

        // Button to toggle between filtered and full view
        Button toggleViewButton = new Button
        {
            Text = "All Results",
            Dock = DockStyle.Bottom
        };

        bool isFilteredView = true; // Default filtered view

        toggleViewButton.Click += (sender, e) =>
        {
            if (isFilteredView)
            {
                // Full view
                resultsGridView.DataSource = gameResults.OrderBy(gr => gr.OverallRanking).ToList();
                toggleViewButton.Text = "Filtered Results"; // Change the button text
            }
            else
            {
                // Filtered view
                resultsGridView.DataSource = gameResults
                    .Where(gr => gr.IsCurrentGame || gr.OverallRanking <= 3)
                    .OrderBy(gr => gr.OverallRanking)
                    .ToList();
                toggleViewButton.Text = "All Results"; // Change the button text
            }
            isFilteredView = !isFilteredView; // Toggle between the two views
        };

        resultsForm.Controls.Add(toggleViewButton);

        resultsForm.ShowDialog();
    }

}
