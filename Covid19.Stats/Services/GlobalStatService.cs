﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Covid19.Stats.Data;
using Covid19.Stats.Models;


namespace Covid19.Stats.Services
{
    public class GlobalStatService : BaseStatService
    {
        public GlobalStatService(AppDbContext context) : base(context) { }

        public GlobalSummaryViewModel GetGlobalStat()
        {
            var lastData = getLastData();
            var penultData = getPenultData();
            var Cases = lastData.Sum(x => x.Confirmed);
            var Deaths = lastData.Sum(x => x.Death);
            return new() {
                Cases = Cases,
                Deaths = Deaths,
                LastUpdate = lastData.Max(x => x.Last_Update),
                CasesDelta = Cases - penultData.Sum(x => x.Confirmed),
                DeathsDelta = Deaths - penultData.Sum(x => x.Death),
                DataPoints =
                _context.Stats
                .GroupBy(   
                x => x.Date,
                x => new { x.Confirmed, x.Death })
                .Select(x => new DataPoint
                {
                    Date = x.Key,
                    Cases = x.Sum(y => y.Confirmed),
                    Deaths = x.Sum(y => y.Death),
                }
                ).OrderBy(x => x.Date)   
        };   
        }
        public IEnumerable<GlobalCountrySummaryViewModel> GetCountriesStat()
        {
            #region PrepareData
            var lastData = getLastData()
                    .GroupBy(
                    x => x.Country_Region,
                    x => new { x.Confirmed, x.Death })
                    .Select(x => new
                    {
                        Country = x.Key,
                        Cases = x.Sum(y => y.Confirmed),
                        Deaths = x.Sum(y => y.Death),
                    });
            var penultData = getPenultData()
                    .GroupBy(
                    x => x.Country_Region,
                    x => new { x.Confirmed, x.Death })
                    .Select(x => new
                    {
                        Country = x.Key,
                        Cases = x.Sum(y => y.Confirmed),
                        Deaths = x.Sum(y => y.Death),
                    });
            #endregion
            var joinedData = lastData.Join(penultData,
                    f => f.Country,
                    s => s.Country,
                    (f, s) => new GlobalCountrySummaryViewModel
                    {
                        Country = f.Country,
                        Cases = f.Cases,
                        CasesDelta = f.Cases - s.Cases,
                        Deaths = f.Deaths,
                        DeathsDelta = f.Deaths - s.Deaths
                    }
                    ).OrderByDescending(x => x.Cases);
           
            return joinedData;
            //return getLastData()
            //    .GroupBy(
            //    x => x.Country_Region,
            //    x => new { x.Confirmed, x.Death})
            //    .Select(x => new CountrySummaryViewModel
            //    {
            //        Country = x.Key,
            //        Cases = x.Sum(y => y.Confirmed),
            //        Deaths = x.Sum(y => y.Death),
            //    }).
            //    OrderByDescending(x => x.Cases);
        }

    }
}
