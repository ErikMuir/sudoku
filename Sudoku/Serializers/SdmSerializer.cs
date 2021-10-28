using System;

namespace Sudoku.Serializers
{
    public class SdmSerializer : ISerializer
    {
        public string FileExtension => "sdm";

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
