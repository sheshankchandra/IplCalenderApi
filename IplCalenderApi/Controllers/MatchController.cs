using IplCalenderApi.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using IplCalenderApi.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace IplCalenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : Controller
    {
        private readonly string _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ipl-2025-UTC.csv");

        private List<MatchRecord> LoadMatches()
        {
            if (!System.IO.File.Exists(_csvFilePath))
            {
                throw new FileNotFoundException("CSV file not found: " + _csvFilePath);
            }

            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            });

            csv.Context.RegisterClassMap<MatchHeaderMap>();
            return csv.GetRecords<MatchCsvRecord>().Select(r =>
            {
                var matchDate = DateTime.ParseExact(r.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                return new MatchRecord
                {
                    MatchId = r.MatchNumber,
                    MatchDateTime = matchDate,
                    DayOfWeek = matchDate.DayOfWeek.ToString(),
                    Location = r.Location,
                    HomeTeam = r.HomeTeam,
                    AwayTeam = r.AwayTeam
                };
            }).ToList();
        }

        [HttpGet]
        public IActionResult GetMatches(string? teams = null, string? day = null, bool? weekend = null, string? venue = null, string? format = "json")
        {
            var matches = LoadMatches().AsQueryable(); // Load matches from CSV

            if (!string.IsNullOrWhiteSpace(teams))
            {
                var teamList = teams.Split(',').Select(t => t.Trim()).ToList();
                matches = matches.Where(m => teamList.Contains(m.HomeTeam) || teamList.Contains(m.AwayTeam));
            }

            if (!string.IsNullOrWhiteSpace(day))
            {
                matches = matches.Where(m => m.DayOfWeek == day);
            }

            if (weekend.HasValue && weekend.Value)
            {
                matches = matches.Where(m => m.DayOfWeek == "Saturday" || m.DayOfWeek == "Sunday");
            }

            if (!string.IsNullOrWhiteSpace(venue))
            {
                matches = matches.Where(m => m.Location.Contains(venue));
            }

            var filteredMatches = matches.ToList();

            // Check if ICS format is requested
            if (format?.ToLower() == "ics")
            {
                var sb = new StringBuilder();
                sb.AppendLine("BEGIN:VCALENDAR");
                sb.AppendLine("VERSION:2.0");
                sb.AppendLine("PRODID:-//IPL Schedule//EN");

                foreach (var match in filteredMatches)
                {
                    sb.AppendLine("BEGIN:VEVENT");
                    sb.AppendLine($"UID:{match.MatchId}@iplcalendar.com");
                    sb.AppendLine($"DTSTAMP:{match.MatchDateTime:yyyyMMddTHHmmssZ}");
                    sb.AppendLine($"DTSTART:{match.MatchDateTime:yyyyMMddTHHmmssZ}");
                    sb.AppendLine($"SUMMARY:{match.HomeTeam} vs {match.AwayTeam}");
                    sb.AppendLine($"LOCATION:{match.Location}");
                    sb.AppendLine($"DESCRIPTION:IPL 2025 - {match.HomeTeam} vs {match.AwayTeam} at {match.Location}");
                    sb.AppendLine("END:VEVENT");
                }

                sb.AppendLine("END:VCALENDAR");

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/calendar", "IPL2025_Schedule.ics");
            }

            // Default: Return JSON response
            return Ok(filteredMatches);
        }
    }
}