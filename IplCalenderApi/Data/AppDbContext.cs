namespace IplCalenderApi.Data
{
    public class MatchRecord
    {
        public int MatchId { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class MatchCsvRecord
    {
        public int MatchNumber { get; set; }
        public int RoundNumber { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string? Result { get; set; }
    }
}
