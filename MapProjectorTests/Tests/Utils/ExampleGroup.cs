using System.Collections.Generic;
using System.Text;

namespace MapProjectorCLI.Tests
{
    public class ExampleGroup : IExample
    {
        public static string skipLine = "\n\n";
        public List<IExample> Examples = new List<IExample>();
        //public string Notes = "";
        public string Title = "";

        public StringBuilder ToString(int stackLevel)
        {
            StringBuilder sb = new StringBuilder()
                .Append(skipLine)
                .Append('#', stackLevel)
                .Append(' ')
                .Append(Title)
                .Append(skipLine);

            //if(!string.IsNullOrEmpty(Notes))
            //{
            //    sb.Append(Notes)
            //        .Append(skipLine);
            //}

            foreach(var ex in Examples)
            {
                sb.Append(ex.ToString(stackLevel + 1));
            }

            return sb;
        }
    }
}
