using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using HtmlAgilityPack;
using static System.Net.WebRequestMethods;

namespace Application.WebScraping
{
    public static class WebScrapingConnections
    {
        public static bool IsConnectionDirect(string iataCodeFrom, string iataCodeTo)
        {
            string baseUrl = $"https://www.flightconnections.com/flights-from-{iataCodeFrom}-to-{iataCodeTo}";

            var web = new HtmlWeb();
            var document = web.Load(baseUrl);

            var directConnection = document.QuerySelector("div.book-row.book-row-fixed.non-stop > div.book-row-fixed-container > div.book-row-fixed-title > h2");
            if (directConnection != null)
            {
                if (directConnection.InnerText.Contains(iataCodeFrom.ToUpper()))
                {
                    return true;
                }
                else return false;
            }
            return false;
        } 

        public static Dictionary<string, string> ShowAirlineDetails(string iataCodeFrom)
        {
            Dictionary<string, string> AirlinesCodes = new Dictionary<string, string>();

            string from = AirportDetailsRapidAPI.FormatedLocationOfAirport(iataCodeFrom).Result;
            string baseUrl = $"https://www.flightconnections.com/flights-from-{from}-{iataCodeFrom}";

            var web = new HtmlWeb();
            var document = web.Load(baseUrl);

            var airlineRow = document.QuerySelectorAll("#listjs > div.airport-page-info.show > div.airport-page-content > div.airline-list > a");

            foreach (var airline in airlineRow)
            {
                var airlineInfo = airline.InnerText.Remove(airline.InnerText.IndexOf(')') + 1, airline.InnerText.Length - (airline.InnerText.IndexOf(')') + 1));
                var airlineCode = airlineInfo.Split('(')[1].Remove(airline.InnerText.Split('(')[1].IndexOf(')'));
                var airlineName = airlineInfo.Split('(')[0];
                AirlinesCodes.Add(airlineCode, airlineName);
            }
            

            return AirlinesCodes;
        }

        public static Dictionary<string, string> ShowAirlineDetails(string iataCodeFrom, string iataCodeTo)
        {
            Dictionary<string, string> AirlineCodes = new Dictionary<string, string>();
            var web = new HtmlWeb();
            var baseUrl = $"https://www.flightconnections.com/flights-from-{iataCodeFrom}-to-{iataCodeTo}";
            var document = web.Load(baseUrl);

            var airlinesList = document.QuerySelectorAll("body > div.site-content > div.route-page > div.route-page-segment.route-page-airlines > div > a");
            
            foreach(var airline in airlinesList)
            {
                string airlineName = airline.InnerText.Split(' ')[0];
                string airlineCode = airline.InnerText.Split(' ')[1];
                AirlineCodes.Add(airlineCode, airlineName);
            }
            return AirlineCodes;
        }

        public static string ShowAirlineName(string iataCodeFrom, string airlineId)
        {
            var dic = ShowAirlineDetails(iataCodeFrom);
            return dic[airlineId];
        }

        public static HashSet<string> ShowTransfers(string iataCodeFrom, string iataCodeTo)
        {
            HashSet<string> connection = new HashSet<string>();
            string baseUrl = $"https://www.flightconnections.com/flights-from-{iataCodeFrom}-to-{iataCodeTo}";

            var web = new HtmlWeb();
            var document = web.Load(baseUrl);

            var transfers = document.QuerySelectorAll("#route-info-popup > div.popup-content > div > div.route-page > div.route-page-schedule-info > div.flight-path > ul > li");

            foreach (var t in transfers)
            {
                var city = t.InnerText.Remove(t.InnerText.IndexOf('&'), 5);
                connection.Add(city);
            }
            return connection;
        }
    }
}
