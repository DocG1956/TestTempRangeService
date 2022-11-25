namespace ScoreDiffService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public class ScoreDiffService
    {
        private string sTeam { get; set; }
        private int iScoreFor { get; set; }
        private int iScoredAgainst { get; set; }
        private string sFilePath { get; set; }
        private int iScoreDiff { get; set; }
        private bool fileExist { get; set; }
        private bool dataCollectionValid { get; set; }
        private int colCount = 10;


        public string sResult = string.Empty;
        public List<ScoreDiffService> Temps { get; set; } = new List<ScoreDiffService>();

        public ScoreDiffService()
        {

        }

        public ScoreDiffService(string path)
        {
            sFilePath = path;
        }

        private bool FileExist()
        {
            fileExist = File.Exists(sFilePath);

            if (!fileExist)
            {
                sResult = "File Not Found";
                return false;
            }

            return fileExist;
        }

        public int GetSmallestScoresRange()
        {
            if (FileExist())
            {
                if (CreateTempCollection())
                {
                    var result = GetScoresDiffRange();
                    if (!result)
                    {
                        return 0;
                    }
                }
                else
                {
                    sResult = "Temp collection issue;";
                    return 0;
                }
                return iScoreDiff;
            }
            else
            {
                sResult = "File not found;";
                return 0;
            }
        }

        private bool GetScoresDiffRange()
        {
            try
            {
                foreach (var temp in Temps)
                {
                    if ((temp.iScoreFor - temp.iScoredAgainst) > 0 && (temp.iScoreFor - temp.iScoredAgainst) < iScoreDiff || iScoreDiff == 0)
                        iScoreDiff = (temp.iScoreFor - temp.iScoredAgainst);
                }

                return true;
            }
            catch (Exception)
            {
                sResult = "Calculation of Lowest Range threw an exception";
                return false;
            }
        }

        private string[] CleanData(string ln)
        {
            if (ln.Length == 0)
            {
                return ln.Split(",");
            }

            //Console.WriteLine(ln);

            //ln = ln.Trim().Replace(" F", "F").Replace("*", ""); //data with text to denote Float is bad practice so the data was cleaned up

            //Console.WriteLine(ln);

            while (ln.Contains("  "))
            {
                ln = ln.Trim().Replace("  ", " ");
                //Console.WriteLine(ln);
            }

            ln = ln.Replace(" ", ",");

            //Console.WriteLine(ln);

            return ln.Split(',');

        }

        private bool CreateTempCollection()
        {
            using (StreamReader file = new StreamReader(sFilePath))
            {

                int counter = 0;
                string ln = string.Empty;

                while ((ln = file.ReadLine()) != null)
                {
                    if (counter > 0)
                    {

                        var data = CleanData(ln);

                        if (data.Length != colCount)
                        {
                            sResult = "At least one data row bypassed";
                            continue;
                        }

                        var rec = new ScoreDiffService();

                        rec.sTeam = data[1];

                        var iScoreFor = 0;
                        if (int.TryParse(data[6], out iScoreFor))
                            rec.iScoreFor = iScoreFor;

                        var iScoredAgainst = 0;
                        if (int.TryParse(data[8], out iScoredAgainst))
                            rec.iScoredAgainst = iScoredAgainst;

                        if (rec.sTeam != null && rec.iScoreFor > 0 && rec.iScoredAgainst > 0)
                        {
                            Temps.Add(rec);
                        }
                        else
                        {
                            file.Close();
                            dataCollectionValid = false;
                            sResult = "CreateTempCollection failed";
                            return dataCollectionValid;
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






