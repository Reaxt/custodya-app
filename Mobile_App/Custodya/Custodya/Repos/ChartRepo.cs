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
<<<<<<< Updated upstream
        public static ObservableCollection<ISeries> GetSeries(ObservableCollection<T> models, string value)
=======
        public ObservableCollection<ISeries> DataSeries { get; private set; }
        private ObservableCollection<DateTimePoint> data;
        private ObservableCollection<T> _modelCollection;
        private string _propName;
        public ChartRepo(ObservableCollection<T> models, string value, int datapoints)
        {
            _modelCollection = models;
            _propName = value;
            data = new();
            List<DateTimePoint> tempData = new();
            foreach (var item in models)
            {
                tempData.Add(new((DateTime)item.GetType().GetProperty("Timestamp").GetValue(item), (dynamic)item.GetType().GetProperty(_propName).GetValue(item)));
            }
            try
            {
                var startingData = tempData.OrderBy(x => x.DateTime).TakeLast(datapoints).ToList();
                data = new ObservableCollection<DateTimePoint>(startingData);
                DataSeries = new ObservableCollection<ISeries>
                {
                    new LineSeries<DateTimePoint>
                    {
                        TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue}",
                        Values = data
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            models.CollectionChanged += ModelDataChange;
        }

        private void ModelDataChange(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            data.Remove(data.First());
            var item = _modelCollection.Last();
            data.Add(new((DateTime)item.GetType().GetProperty("Timestamp").GetValue(item), (dynamic)item.GetType().GetProperty(_propName).GetValue(item)));
        }
        private static ObservableCollection<ISeries> GetSeries(ObservableCollection<T> models, string value, int datapoints = 20)
>>>>>>> Stashed changes
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
