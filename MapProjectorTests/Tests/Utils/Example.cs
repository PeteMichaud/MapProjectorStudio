using System.Text;
using System.Collections.Generic;

namespace MapProjectorCLI.Tests
{
    public class Example : IExample
    {
        public static string skipLine = "\n\n";

        public string Title;
        public string[] Args;
        public string Notes;
        public List<string> ImageURLs;

        public Example(string title, string[] args, List<string> imageUrls, string notes)
        {
            Title = title;
            Args = args;
            ImageURLs = imageUrls;
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
                .Append(skipLine);

            for(var i = 0; i < ImageURLs.Count; i++)
            {
                var num = ImageURLs.Count > 1 ? $" {i + 1}" : "";
                sb
                    .Append($"![{Title}{num}]({ImageURLs[i]})")
                    .Append(skipLine);
            }

            return sb;
        }
    }
}
