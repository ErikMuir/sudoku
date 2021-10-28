using System;

namespace Sudoku.Serializers
{
    public class SdxSerializer : ISerializer
    {
        public string FileExtension => "sdx";

        public Puzzle Deserialize(string input)
        {
            throw new NotImplementedException();
        }

        public string Serialize(Puzzle puzzle)
        {
            throw new NotImplementedException();
        }
    }
}
