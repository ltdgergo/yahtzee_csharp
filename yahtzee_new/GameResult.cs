public class GameResult
{
    public string PlayerName { get; set; }
    public int GrandTotal { get; set; }
    public DateTime GameEndDate { get; set; }

    public int OverallRanking { get; set; } 

    public int? CurrentRanking { get; set; }
    public bool IsCurrentGame { get; set; }

}
