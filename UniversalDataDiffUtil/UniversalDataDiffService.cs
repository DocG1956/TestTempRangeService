namespace UniversalScoreDiffService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using DataMap;
    using FileExist;

    public class UniversalDataDiffService
    {
        public int compareDataColumnIndex1 { get; set; }
        public int compareDataColumnIndex2 { get; set; }
        public string compareDataColumnKey { get; set; }

        private string sFilePath { get; set; }
        private int iDataDiff { get; set; }
        private bool fileExist { get; set; }
        private bool dataCollectionValid { get; set; }
        private int colCount = 10;
        private DataFileMap dataFileMap { get; set; }

        public string sResult = string.Empty;
        public List<UniversalDataDiffService> FileData { get; set; } = new List<UniversalDataDiffService>();

        private UniversalDataDiffService()
        {

        }

        public UniversalDataDiffService(string path, DataFileMap dataFileMap)
        {
            sFilePath = path;
            this.dataFileMap = dataFileMap;
            this.colCount = dataFileMap.dataColumns.Count;
        }

        public int GetSmallestDataRange()
        {
            if (dataFileMap.MatchDataWidth())
            {
                if (new FileExists(sFilePath).FileExist())
                {
                    if (CreateTempCollection(dataFileMap))
                    {
                        var result = GetScoresDiffRange();
                        if (!result)
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        sResult = "Temp collection issue";
                        return 0;
                    }
                    return iDataDiff;
                }
                else
                {
                    sResult = "File not found";
                    return 0;
                }
            }
            else
            {
                sResult = "Data Map Invalid";
                return 0;
            }
        }

        private bool GetScoresDiffRange()
        {
            try
            {
                foreach (var temp in FileData)
                {
                    if ((temp.compareDataColumnIndex1 - temp.compareDataColumnIndex2) > 0 && (temp.compareDataColumnIndex1 - temp.compareDataColumnIndex2) < iDataDiff || iDataDiff == 0)
                        iDataDiff = (temp.compareDataColumnIndex1 - temp.compareDataColumnIndex2);
                }

                return true;
            }
            catch (Exception)
            {
                sResult = "Calculation of Lowest Range threw an exception";
                return false;
            }
        }

        private string CleanData(string data, List<string> filters)
        {
            if (data.Length == 0)
            {
                return data;
            }

            foreach (var f in filters)
            {
                data = data.Replace(f, " ");
            }

            return data;
        }

        private string CleanData1(string ln, List<string> filters)
        {
            if (ln.Length == 0)
            {
                return ln;
            }

            foreach (var f in filters)
            {
                ln = ln.Trim().Replace(f, "");
            }

            while (ln.Contains("  "))
            {
                ln = ln.Trim().Replace("  ", " ");
              }

            ln = ln.Replace(" ", ",");

            return ln;
        }

        private bool CreateTempCollection(DataFileMap dataFileMap)
        {
            using (StreamReader file = new StreamReader(sFilePath))
            {
                int counter = 0;
                int firstDataRow = dataFileMap.headerRows;
                string ln = string.Empty;

                while ((ln = file.ReadLine()) != null)
                {
                    if (counter >= dataFileMap.headerRows)
                    {
                        if (ln.Length == dataFileMap.dataRowWidth)
                        {
                            var rec = new UniversalDataDiffService();

                            foreach (var d in dataFileMap.dataColumns)
                            {
                                if (dataFileMap.compareDataColumnKey == d.Key)
                                    rec.compareDataColumnKey = ln.Substring(d.Key, d.Value).Trim();
                                else if (dataFileMap.compareColumnIndex1 == d.Key)
                                    rec.compareDataColumnIndex1 = int.Parse(CleanData(ln.Substring(d.Key, d.Value), dataFileMap.filters));
                                else if (dataFileMap.compareColumnIndex2 == d.Key)
                                    rec.compareDataColumnIndex2 = int.Parse(CleanData(ln.Substring(d.Key, d.Value), dataFileMap.filters));
                            }

                            FileData.Add(rec);
                        }
                    }
                    else
                        counter++;
                }

                file.Close();

                dataCollectionValid = true;
                return dataCollectionValid;
            }

        }
    }
}







