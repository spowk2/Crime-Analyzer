using System;

namespace CrimeAnalyzer
{
    public class Crimestats
    {
        public int Year;
        public int Population;
        public int ViolentCrime;
        public int Murder;
        public int Rape;
        public int Robbery;
        public int AggAssault;
        public int PropertyCrime;
        public int Burglary;
        public int Theft;
        public int MVT;


        public Crimestats(int year, int population, int violentCrime, int murder,
                         int rape, int robbery, int aggravatedAssault, int propertyCrime,
                         int burglary, int theft, int motorVehicleTheft)
        {
            Year = year;
            Population = population;
            ViolentCrime = violentCrime;
            Murder = murder;
            Rape = rape;
            Robbery = robbery;
            AggAssault = aggravatedAssault;
            PropertyCrime = propertyCrime;
            Burglary = burglary;
            Theft = theft;
            MVT = motorVehicleTheft;
        }
    }
}