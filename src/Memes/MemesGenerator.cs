using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Jpeg;


namespace Memes
{
    public class MemesGenerator
    {
        private const string Url = "https://joyreactor.cc/tag/%23Приколы+для+даунов/{0}";
        public const int maxAttempts = 100;
        private const string MemePatternText = "//img2.joyreactor.cc/pics/post/(?!full).{0,300}-[0-9]{7}.jpeg";
        private readonly Random _random;

        public MemesGenerator()
        {
            _random = new Random();
        }

        public MemesGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public MemesData GetMeme()
        {
            int pageNumber;
            string htmlText;
            string[]? memesArray;
            int attempts = 0;

            do
            {
                pageNumber = GetRandomPage();
                htmlText = GetHtmlPage(string.Format(Url, pageNumber));
                memesArray = ParsHtmlEhd(htmlText);
                if (attempts++ > maxAttempts)
                    throw new Exception($"Meme not founded for {maxAttempts} attempts");
            } while (memesArray == null || memesArray.Length == 0);
            string? memeUrl = memesArray[RandomImg(memesArray)];
            Image rawMeme = Image.Load(GetImgStream(memeUrl));
            Image meme = CropImage(rawMeme);
            byte[] byteMeme = ImageToByte(meme);
            return new MemesData(byteMeme, pageNumber, memeUrl);
        }

        public string GetHtmlPage(string url)
        {
            string HtmlText = string.Empty;
            HttpWebRequest myHttwebrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebresponse = (HttpWebResponse)myHttwebrequest.GetResponse();
            StreamReader strm = new StreamReader(myHttpWebresponse.GetResponseStream());
            return strm.ReadToEnd();
        }

        public Stream GetImgStream(string url)
        {
            HttpWebRequest myHttwebrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebresponse = (HttpWebResponse)myHttwebrequest.GetResponse();
            Stream strm = myHttpWebresponse.GetResponseStream();
            return strm;
        }

        public static string[]? ParsHtmlEhd(string htmlText)
        {
            string htmlImg = htmlText;
            MatchCollection matchesImg = Regex.Matches(htmlImg, MemePatternText, RegexOptions.IgnoreCase);
            return matchesImg.Count == 0 ? null : matchesImg.Select(matchesImg => $"https:{matchesImg.Groups[0].Value}").ToArray();
        }

        public int GetRandomPage()
        {
            Random randomPage = new Random();
            int page = randomPage.Next(1388);
            return page;
        }

        public int RandomImg(string[] memesArray)
        {
            return _random.Next(memesArray.Length);
        }

        private Image CropImage(Image img)
        {
            var imageHeight = img.Size().Height;
            var imageWidth = img.Size().Width;
            img.Mutate(
        i => i.Crop(new Rectangle(0, 0, imageWidth, imageHeight - 14)));
            return img;
        }

        public byte[] ImageToByte(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(JpegFormat.Instance);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }
    }
}
