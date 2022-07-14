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
        private DataPointsSelector _dataPointsSelector;
        public GlobalStatService(AppDbContext context, DataPointsSelector dataPointsSelector) : base(context) 
        {
            _dataPointsSelector = dataPointsSelector;
        }

        public SummaryViewModel GetGlobalStat()
       {
            return new() {
                TableData = GetTableData(),
                DataPoints = new()
                {
                    DataPointsDaily = _dataPointsSelector.GetAll(),
                    DataPointsMonthly = _dataPointsSelector.GetMonthly(),
                    DataPointsWeekly = _dataPointsSelector.GetWeekly()
                }
            };   
        }
        private IEnumerable<TableRowSummary> _getCountriesStat()
        {
            #region PrepareData
            var lastData = getLastData()
                    .GroupBy(
                    x => x.Country_Region,
                    x => new { x.Confirmed, x.Death, x.Case_Fatality_Ratio })
                    .Select(x => new
                    {
                        Country = x.Key,
                        Cases = x.Sum(y => y.Confirmed),
                        Deaths = x.Sum(y => y.Death),
                        FatalityRatio = (float)Math.Round(x.Average(y => y.Case_Fatality_Ratio), 2)
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
                    (f, s) => new TableRowSummary
                    {
                        CountryRegion = f.Country,
                        Cases = f.Cases,
                        CasesDelta = f.Cases - s.Cases,
                        Deaths = f.Deaths,
                        DeathsDelta = f.Deaths - s.Deaths,
                        FatalityRatio = f.FatalityRatio
                    }
                    ).OrderByDescending(x => x.Cases);

            return joinedData;
        }

        public TableData GetTableData()
        {
            var lastData = getLastData().ToList();
            var penultData = getPenultData().ToList();
            var Cases = getLastData().Sum(x => x.Confirmed);
            var Deaths = getLastData().Sum(x => x.Death);
            TableData tabledata = new()
            {
                IsGlobalTable = true,
                CountryRegion = "Total",
                Cases = Cases,
                Deaths = Deaths,
                LastUpdate = lastData.Max(x => x.Last_Update),
                CasesDelta = Cases - penultData.Sum(x => x.Confirmed),
                DeathsDelta = Deaths - penultData.Sum(x => x.Death),
                FatalityRatio = (float)Math.Round(lastData.Average(x => x.Case_Fatality_Ratio), 2),
                RowSummary = _getCountriesStat()
            };
            return tabledata;
        }
        public async Task<TableData> GetTableDataAsync()
        {
            return await Task.Run(() => GetTableData());
        }

        public IEnumerable<TableRowSummary> GetCountriesWithoutDelta()
        {
            var lastData = getLastData()
                      .GroupBy(
                      x => x.Country_Region,
                      x => new { x.Confirmed, x.Death })
                      .Select(x => new TableRowSummary
                      {
                          CountryRegion = x.Key,
                          Cases = x.Sum(y => y.Confirmed),
                          Deaths = x.Sum(y => y.Death),
                      });
            return lastData.AsEnumerable();
        }

    }
}
