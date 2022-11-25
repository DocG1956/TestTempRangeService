namespace DataMap
{
    public class DataFileMap
    {
        public Dictionary<int, int> dataColumns = new Dictionary<int, int>();
        public int dataRowWidth { get; set; }
        public int compareColumnIndex1;
        public int compareColumnIndex2;
        public int compareDataColumnKey;
        public bool ignoreNegativeValues;
        public char dataSeperatorChar;
        public int headerRows;
        public int direction;   //compareColumnIndex1 must be greater than compareColumnIndex2
        public List<string> filters = new List<string>();

        public bool MatchDataWidth()
        {
            var colWidthTotal = 0;

            foreach (var col in dataColumns)
            {
                colWidthTotal += col.Value;
            }

            return true; // colWidthTotal == dataRowWidth;
        }
    }
}