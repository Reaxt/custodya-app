using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Custodya.Models;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;

namespace Custodya.Repos
{
    public static class ChartRepo<T>
    {
        public static ObservableCollection<ISeries> GetSeries(ObservableCollection<T> models, string value)
        {
            ObservableCollection<DateTimePoint> oc = new();
            foreach (var item in models)
            {
                oc.Add(new((DateTime)item.GetType().GetProperty("Timestamp").GetValue(item), (dynamic)item.GetType().GetProperty(value).GetValue(item)));
            }
            try {

                return new ObservableCollection<ISeries>
                {
                    new StepLineSeries<DateTimePoint>
                    {
                        TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long) chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue}",
                        Values = oc
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
    }
}
