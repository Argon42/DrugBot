using System.Net;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Memes;

public class MemesGenerator
{
    private const string Url = "https://joyreactor.cc/tag/%23Приколы+для+даунов/{0}";
    private const int MaxAttempts = 100;
    private const string MemePatternText = "//img2.joyreactor.cc/pics/post/(?!full).{0,300}-[0-9]{7}.jpeg";
    private readonly Random _random;

    public MemesGenerator() => _random = new Random();

    public MemesGenerator(int seed) => _random = new Random(seed);

    public MemesData GetMeme()
    {
        int pageNumber;
        string[]? memesArray;
        int attempts = 0;

        do
        {
            pageNumber = GetRandomPage();
            string htmlText = GetHtmlPage(string.Format(Url, pageNumber));
            memesArray = ParsHtmlEhd(htmlText);
            if (attempts++ > MaxAttempts)
                throw new Exception($"Meme not founded for {MaxAttempts} attempts");
        } while (memesArray == null || memesArray.Length == 0);

        string? memeUrl = memesArray[RandomImg(memesArray)];
        Image rawMeme = Image.Load(GetImgStream(memeUrl));
        Image meme = CropImage(rawMeme);
        byte[] byteMeme = ImageToByte(meme);
        return new MemesData(byteMeme, pageNumber, memeUrl);
    }

    private Image CropImage(Image img)
    {
        int imageHeight = img.Size().Height;
        int imageWidth = img.Size().Width;
        img.Mutate(
            i => i.Crop(new Rectangle(0, 0, imageWidth, imageHeight - 14)));
        return img;
    }

    private static string GetHtmlPage(string url)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        return new StreamReader(response.GetResponseStream()).ReadToEnd();
    }

    private static Stream GetImgStream(string url)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        return response.GetResponseStream();
    }

    private int GetRandomPage()
    {
        int page = _random.Next(1388);
        return page;
    }

    private byte[] ImageToByte(Image image)
    {
        using MemoryStream memoryStream = new();
        IImageEncoder? imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(JpegFormat.Instance);
        image.Save(memoryStream, imageEncoder);
        return memoryStream.ToArray();
    }

    private static string[]? ParsHtmlEhd(string htmlText)
    {
        MatchCollection matchesImg = Regex.Matches(htmlText, MemePatternText, RegexOptions.IgnoreCase);
        return matchesImg.Count == 0 ? null : matchesImg.Select(match => $"https:{match.Groups[0].Value}").ToArray();
    }

    private int RandomImg(string[] memesArray) => _random.Next(memesArray.Length);
}