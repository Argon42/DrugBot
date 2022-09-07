using SixLabors.ImageSharp;

namespace Memes
{
    public class MemesData
    {
        public byte[] Meme { get; }
        public int PageNumber { get; }
        public string MemeUrl { get; }

        public MemesData(byte[] byteMeme, int pageNumber, string memeUrl)
        {
            Meme = byteMeme;
            PageNumber = pageNumber;
            MemeUrl = memeUrl;
        }
    }
}