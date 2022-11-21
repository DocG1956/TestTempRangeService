namespace TempRangeService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public class MinTempRangeService
    {
        private int iDay { get; set; }
        private int iMaxTemp { get; set; }
        private int iMinTemp { get; set; }
        private string sFilePath { get; set; }
        private int iMinTempRange { get; set; }
        private bool fileExist { get; set; }
        private bool dataCollectionValid { get; set; }

        public string sResult = string.Empty;
        public List<MinTempRangeService> Temps { get; set; } = new List<MinTempRangeService>();

        public MinTempRangeService()
        {

        }

        public MinTempRangeService(string path)
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

        public int GetSmallestTempRange()
        {
            if (FileExist())
            {
                if (CreateTempCollection())
                {
                    var result = GetLowestTempRange();
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
                return iMinTempRange;
            }
            else
            {
                sResult = "File not found;";
                return 0;
            }
        }

        private bool GetLowestTempRange()
        {
            try
            {
                foreach (var temp in Temps)
                {
                    if ((temp.iMaxTemp - temp.iMinTemp) < iMinTempRange || iMinTempRange == 0)
                        iMinTempRange = (temp.iMaxTemp - temp.iMinTemp);
                }

                return true;
            }
            catch ()
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

            ln = ln.Trim().Replace(" F", "F").Replace("*", ""); //data with text to denote Float is back practice

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
                    if (counter > 1)
                    {

                        var data = CleanData(ln);
                        var rec = new MinTempRangeService();

                        int iDayConv = 0;
                        if (int.TryParse(data[0].ToString(), out iDayConv))
                            rec.iDay = iDayConv;

                        var iMaxConvTemp = 0;
                        if (int.TryParse(data[1], out iMaxConvTemp))
                            rec.iMaxTemp = iMaxConvTemp;

                        var iMinConvTemp = 0;
                        if (int.TryParse(data[2], out iMinConvTemp))
                            rec.iMinTemp = iMinConvTemp;

                        if (rec.iDay > 0 && rec.iMaxTemp > 0 && rec.iMinTemp > 0)
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






