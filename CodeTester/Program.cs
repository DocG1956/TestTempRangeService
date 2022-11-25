using DataMap;
using UniversalScoreDiffService;

string path = "c:\\users\\gbales\\downloads\\weather.dat";

var dataMap = new DataFileMap();

dataMap.compareDataColumnKey = 0; //column # in data file 
dataMap.compareColumnIndex1 = 4;
dataMap.compareColumnIndex2 = 8;
dataMap.ignoreNegativeValues = true;
dataMap.dataRowWidth = 89;
dataMap.headerRows = 2;

dataMap.filters.Add("*");

dataMap.direction = 1;

Dictionary<int, int> dataColumns = new Dictionary<int, int>();

dataColumns.Add(0, 4);  //col 0 for length of 4 
dataColumns.Add(4, 4);  //col 4 for length of 4 
dataColumns.Add(8, 6);  //col 8 for length of 6 

dataMap.dataColumns = dataColumns;

var diffService = new UniversalDataDiffService(path, dataMap);
var result = diffService.GetSmallestDataRange();

Console.WriteLine("weather min diff " + result);

Console.WriteLine();

path = "c:\\users\\gbales\\downloads\\football.dat";

dataMap = new DataFileMap();

dataMap.compareDataColumnKey = 7;
dataMap.compareColumnIndex1 = 43;
dataMap.compareColumnIndex2 = 50;

dataMap.ignoreNegativeValues = true;
dataMap.dataRowWidth = 58;
dataMap.headerRows = 1;

dataMap.filters.Add("-");

dataMap.direction = 1;

dataColumns.Clear();

dataColumns.Add(7, 16); //column 1 (0 based)
dataColumns.Add(43, 4);  
dataColumns.Add(50, 6);

dataMap.dataColumns = dataColumns;

var diffService1 = new UniversalDataDiffService(path, dataMap);
result = diffService1.GetSmallestDataRange();

Console.WriteLine("weather min diff " + result);

Console.WriteLine();

