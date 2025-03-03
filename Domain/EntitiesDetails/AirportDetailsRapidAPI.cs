using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Domain
{
    public class AirportDetailsRapidAPI
    {
        public static async Task<Dictionary<string, string>> AirportDetailsFinder(string iataCode)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://airport-info.p.rapidapi.com/airport?iata={iataCode}"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "9350d87356mshd80b91a44dfd40cp19c825jsn1e9b74b827f8" },
                        { "X-RapidAPI-Host", "airport-info.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
                return dic;

            }
        }
        public static async Task<string> LocationOfAirport(string iataCode)
        {
            var location = AirportDetailsFinder(iataCode);
            string city = location.Result["location"].Split(',')[0];
            return city;
        }

        public static async Task<string> TimeDiffrenceCounter(string iataCodeFrom, string iataCodeTo)
        {
            var location1 = AirportDetailsFinder(iataCodeFrom).Result["uct"];
            var location2 = AirportDetailsFinder(iataCodeTo).Result["uct"];

            try
            {
                int time1 = Int32.Parse(location1);
                int time2 = Int32.Parse(location2);
                int diffrence = Math.Abs(time1 - time2) / 60;

                return $"{diffrence}h";
            }
            catch { return "undefined"; }

        }

        public static async Task<double> DistanceCounter(string iataCodeFrom, string iataCodeTo)
        {
            try
            {
                const double R = 6371;

                var lat1 = Convert.ToDouble(AirportDetailsFinder(iataCodeFrom).Result["latitude"].Replace('.', ',')); //problem jest najpewniej z konwersja na double
                var lat2 = Convert.ToDouble(AirportDetailsFinder(iataCodeTo).Result["latitude"].Replace('.', ','));
                var lon1 = Convert.ToDouble(AirportDetailsFinder(iataCodeFrom).Result["longitude"].Replace('.', ','));
                var lon2 = Convert.ToDouble(AirportDetailsFinder(iataCodeTo).Result["longitude"].Replace('.', ','));

                
                lat1 = ToRadians(lat1);
                lon1 = ToRadians(lon1);
                lat2 = ToRadians(lat2);
                lon2 = ToRadians(lon2);

                double dlat = lat2 - lat1;
                double dlon = lon2 - lon1;

                double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                           Math.Cos(lat1) * Math.Cos(lat2) *
                           Math.Sin(dlon / 2) * Math.Sin(dlon / 2);

                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                double distance = R * c;

                

                return (double)Math.Round(distance);
               
            }
            catch { return 0; }
            
        }

        public static async Task<string> FormatedDistanceCounter(string iataCodeFrom, string iataCodeTo)
        {
            double distance = DistanceCounter(iataCodeFrom, iataCodeTo).Result;
            return $"{distance} km";
        }

        public static async Task<string> FormatedLocationOfAirport(string iataCode)
        {
            string city = LocationOfAirport(iataCode).Result;

            if (city.Contains('('))
            {
                city = city.Split("(")[0];
                if (city.Contains(' ')) 
                { 
                    city = city.Remove(city.IndexOf(' '), 1); 
                }
            }
            if (city.Contains('/'))
            {
                city = city.Replace('/', '-');
                while (city.Contains(' '))
                {
                    city = city.Remove(city.IndexOf(' '), 1);
                }
            }
            if (city.Contains(' '))
            {
                city = city.Replace(' ', '-');
            }

            return city.ToLower();
        }

        public static async Task<string> DurationCounter(string iataCodeFrom, string iataCodeTo)
        {
            double duration = Math.Round((double)(DistanceCounter(iataCodeFrom, iataCodeTo).Result / 700), 2);
            var durationInMin = Math.Round(duration * 60, 0);
            return $"{durationInMin}min";

        }

        private static double ToRadians(double degree)
        {
            return degree * (Math.PI / 180.0);
        }

    }
}
