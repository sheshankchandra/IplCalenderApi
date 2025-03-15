using CsvHelper.Configuration;
using IplCalenderApi.Data;

namespace IplCalenderApi.Models
{
    public class MatchHeaderMap : ClassMap<MatchCsvRecord>
    {
        public MatchHeaderMap()
        {
            Map(m => m.MatchNumber).Name("Match Number");
            Map(m => m.RoundNumber).Name("Round Number");
            Map(m => m.Date).Name("Date");
            Map(m => m.Location).Name("Location");
            Map(m => m.HomeTeam).Name("Home Team");
            Map(m => m.AwayTeam).Name("Away Team");
        }
    }
}
