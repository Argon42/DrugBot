using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Anecdotes;

public class AnecdoteGenerator
{
    private const string Url = "https://anekdotov.net/arc/{0}.html";
    private const int MaxAttempts = 100;
    private const string AnecdotePatternText = "<div class=anekdot>([.\\W\\w])*?</div>";
    private const string PatternErrorText = "<div id=.*>\n.*</a>";
    private readonly Random _random;

    public AnecdoteGenerator()
    {
        _random = new Random();
    }

    public AnecdoteGenerator(int seed)
    {
        _random = new Random(seed);
    }
    public AnecdoteData GenerateAnecdote()
    {
        string randomPage;
        string[]? array;
        int attempts = 0;

        do
        {
            randomPage = RandomDayPage();
            string htmlText = GetHtmlPage(string.Format(Url, randomPage));
            array = ParsHtmlEhd(htmlText);

            if (attempts++ > MaxAttempts)
                throw new Exception($"Anecdote not founded for {MaxAttempts} attempts");
        } while (array == null || array.Length == 0);

        int numberPost = RandomNumberPost(array);
        string anecdote = ParsText(array[numberPost]);
        return new AnecdoteData(randomPage, numberPost, anecdote);
    }

    private string GetHtmlPage(string url)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
        StreamReader stream = new(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
        return stream.ReadToEnd();
    }

    private string[]? ParsHtmlEhd(string htmlText)
    {
        List<Match> matches = Regex.Matches(htmlText, AnecdotePatternText, RegexOptions.IgnoreCase).ToList();
        MatchCollection matchesErrorText = Regex.Matches(htmlText, PatternErrorText, RegexOptions.IgnoreCase);
        RemoveErroneousMatches(matchesErrorText, matches);

        return matches.Count == 0 ? null : matches.Select(match => match.Groups[0].Value).ToArray();
    }

    private string ParsText(string rawAnecdote)
    {
        return rawAnecdote.Remove(rawAnecdote.Length - 6)
            .Replace("<div class=anekdot>", "")
            .Replace("<p> ", "")
            .Replace("<p>", "")
            .Replace("&mdash;", "-")
            .Replace("</p>", "")
            .Replace("<BR> ", "\r")
            .Replace("<BR>", "\r")
            .Replace("<br> ", "")
            .Replace("<br>", "\r")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("  ", " ")
            .Replace("&#133;", "...")
            .TrimStart();
    }

    private string RandomDayPage()
    {
        DateTime start = new(2020, 8, 01);
        int range = (DateTime.Today - start).Days;
        DateTime finalDate = start.AddDays(_random.Next(range));
        string stringDate = finalDate.ToString("yyMMdd");
        return stringDate;
    }

    private int RandomNumberPost(string[] array)
    {
        return _random.Next(array.Length);
    }

    private void RemoveErroneousMatches(MatchCollection matchesErrorText, ICollection<Match> matches)
    {
        foreach (Match match in matchesErrorText)
        {
            Match? element = matches.FirstOrDefault(s => s.Groups[0].Value.Contains(match.Value));
            if (element != null) matches.Remove(element);
        }
    }
}