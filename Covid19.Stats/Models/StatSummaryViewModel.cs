﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19.Stats.Data;

namespace Covid19.Stats.Models
{
    public class StatSummaryViewModel
    {
        public int Id { get; set; }
        public string ProvinceState { get; set; }
        public string CountryRegion { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public string Date { get; set; }
        public static StatSummaryViewModel FromStat(CovidStat stat)
        {
            return new StatSummaryViewModel
            {
                Id = stat.Id,
                ProvinceState = stat.Province_State,
                CountryRegion = stat.Country_Region,
                Confirmed = stat.Confirmed,
                Deaths = stat.Death,
                Date = DateTime.Now.ToString()
            };
        }
    }
}
