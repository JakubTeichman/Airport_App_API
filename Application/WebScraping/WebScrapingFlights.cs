using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using HtmlAgilityPack;
using static System.Net.WebRequestMethods;

namespace Application.WebScraping
{
    public class WebScrapingFlights
    {
        public static Connection Connections(string iataCode)
        {
            Connection connection = new Connection();
            string from = AirportDetailsRapidAPI.FormatedLocationOfAirport(iataCode).Result;
            string baseUrl = $"https://www.flightconnections.com/flights-from-{from}-{iataCode}";

            var web = new HtmlWeb();
            var document = web.Load(baseUrl);

            var tableRows = document.QuerySelectorAll("div #popular-destinations.popular-destinations-list a");

            connection.Cities = new HashSet<string>();
            foreach (var flightConnection in tableRows )
            {
                string city = "";
                for (int i = 0; i < 5; i++)
                {
                    city += flightConnection.InnerText.Split(' ')[i] + " ";                    
                    if(flightConnection.InnerText.Split(' ')[i].Contains('(')) { break; }
                }
                connection.Cities.Add(city);
            }
            return connection;
        }
    }
}
