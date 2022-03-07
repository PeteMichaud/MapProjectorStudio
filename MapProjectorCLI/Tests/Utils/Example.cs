using System.Text;

namespace MapProjectorCLI.Tests
{
    public class Example : IExample
    {
        public static string skipLine = "\n\n";

        public string Title;
        public string[] Args;
        public string Notes;
        public string ImageURL;

        public Example(string title, string[] args, string imageUrl, string notes)
        {
            Title = title;
            Args = args;
            ImageURL = imageUrl;
            Notes = notes;
        }

        public StringBuilder ToString(int stackLevel)
        {
            StringBuilder sb = new StringBuilder()
                .Append('#', stackLevel)
                .Append(' ')
                .Append(Title)
                .Append(skipLine);

            if (!string.IsNullOrEmpty(Notes))
            {
                sb.Append(Notes).Append(skipLine);
            }

            sb
                .Append($"`{string.Join(" ", Args)}`")
                .Append(skipLine)
                .Append($"![{Title}]({ImageURL})")
                .Append(skipLine);

            return sb;
        }
    }
}
