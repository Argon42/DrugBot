namespace Memes;

public class MemesData
{
    public byte[] Meme { get; }
    public string MemeUrl { get; }
    public int PageNumber { get; }

    public MemesData(byte[] byteMeme, int pageNumber, string memeUrl)
    {
        Meme = byteMeme;
        PageNumber = pageNumber;
        MemeUrl = memeUrl;
    }
}