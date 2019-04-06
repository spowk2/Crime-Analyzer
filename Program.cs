using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace CrimeAnalyzer
{
    class MainClass
    {
        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("CrimeAnalyzer <crime_csv_file_path> <report_file_path>");
                Environment.Exit(1);
            }

            string csvDataPath = args[0];
            string reportPath = args[1];

            List<Crimestats> csList = null;

            try
            {
                csList = Load(csvDataPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(2);
            }

            var report = CreateCrimeReportText(csList);

            try
            {
                File.WriteAllText(reportPath, report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(3);
            }

        }

        static int ItemsInRow = 11;

        public static List<Crimestats> Load(string csvDataFilePath)
        {
            List<Crimestats> crimeStatsList = new List<Crimestats>();

            try
            {
                using (var reader = new StreamReader(csvDataFilePath))
                {
                    int lineNum = 0;
                    while (!reader.EndOfStream)
                    {
                        var info = reader.ReadLine();
                        lineNum++;
                        if (lineNum == 1) continue;
                        var values = info.Split(',');

                        if (values.Length != ItemsInRow)
                        {
                            throw new Exception($"Row {lineNum} contains {values.Length} values. It should contain {ItemsInRow}.");
                        }
                        try
                        {
                            int year = int.Parse(values[0]);
                            int population = int.Parse(values[1]);
                            int violentCrime = int.Parse(values[2]);
                            int murder = int.Parse(values[3]);
                            int rape = int.Parse(values[4]);
                            int robbery = int.Parse(values[5]);
                            int aggravatedAssault = int.Parse(values[6]);
                            int propertyCrime = int.Parse(values[7]);
                            int burglary = int.Parse(values[8]);
                            int theft = int.Parse(values[9]);
                            int motorVehicleTheft = int.Parse(values[10]);



                            Crimestats crimeStats = new Crimestats(year, population, violentCrime, murder, rape, robbery, aggravatedAssault, propertyCrime, burglary, theft, motorVehicleTheft);
                            crimeStatsList.Add(crimeStats);
                        }
                        catch (FormatException e)
                        {
                            throw new Exception($"Row {lineNum} contains invalid data. ({e.Message})");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to open {csvDataFilePath} ({e.Message}).");
            }

            return crimeStatsList;
        }



        public static string CreateCrimeReportText(List<Crimestats> csList)
        {
            string report = "Crime Analyzer Report\n\n";

            if (csList.Count() < 1)
            {
                report += "not available\n";

                return report;
            }

            var start = (from crimeStats in csList select crimeStats.Year).Min();
            var end = (from crimeStats in csList select crimeStats.Year).Max();

            report += $"Period: " + start + "-" + end + " (" + csList.Count + " years)\n";


            report += "Years murders per year < 15000: ";
            var years = from crimeStats in csList where crimeStats.Murder < 15000 select crimeStats.Year;
            if (years.Count() > 0)
            {
                foreach (var year in years)
                {
                    report += year + ",";
                }
                report.TrimEnd(',');
                report += "\n";
            }
            else
            {
                report += "not available\n";
            }

            report += "Robberies per year > 500000: ";
            var records = from crimeStats in csList where crimeStats.Robbery > 500000 select crimeStats;
            if (records.Count() > 0)
            {
                foreach (var record in records)
                {
                    report += record.Year + " = " + record.Robbery + ",";
                }
                report.TrimEnd(',');
                report += "\n";
            }
            else
            {
                report += "not available\n";
            }

            report += "Violent crime per capita rate (2010): ";
            var violentCrimeRates = from crimeStats in csList where crimeStats.Year == 2010 select ((double)(crimeStats.ViolentCrime) / (double)(crimeStats.Population));
            if (violentCrimeRates.Count() > 0)
            {
                report += violentCrimeRates.First();
                report += "\n";
            }
            else
            {
                report += "not available\n";
            }

            var avgMurders = (from crimeStats in csList select crimeStats.Murder).Average();
            var avgMurders1994_1997 = (from crimeStats in csList where crimeStats.Year >= 1994 && crimeStats.Year <= 1997 select crimeStats.Murder).Average();
            var avgMurders2010_2014 = (from crimeStats in csList where crimeStats.Year >= 2010 && crimeStats.Year <= 2013 select crimeStats.Murder).Average();
            var minThefts = (from crimeStats in csList where crimeStats.Year >= 1999 && crimeStats.Year <= 2004 select crimeStats.Theft).Min();
            var maxThefts = (from crimeStats in csList where crimeStats.Year >= 1999 && crimeStats.Year <= 2004 select crimeStats.Theft).Max();
            var MaxMVT = from crimeStats in csList where crimeStats.MVT == ((from stats in csList select stats.MVT).Max()) select crimeStats.Year;


            report += $"Average murder per year (all years): " + avgMurders + "\n";
            report += $"Average murder per year (1994 - 1997): " + avgMurders1994_1997 + "\n";
            report += $"Average murder per year (2010 - 2014): " + avgMurders2010_2014 + "\n";
            report += $"Minimum thefts per year (1999 - 2004): " + minThefts + "\n";
            report += $"Maximum thefts per year (1999 - 2004): " + maxThefts + "\n";
            report += "Year of highest number of motor vehicle thefts: ";
            if (MaxMVT.Count() > 0)
            {
                report += MaxMVT.First();
                report += "\n";
            }
            else
            {
                report += "not available\n";
            }

            return report;
        }



    }

}