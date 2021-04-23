using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricsManager.Model
{
    public class TemperatureModel
    {
        private Dictionary<DateTime, int> _data;
        public TemperatureModel()
        {
            _data = new Dictionary<DateTime, int>()
            {
                { new DateTime(2021,04,19), 9},
                { new DateTime(2021,04,20), 9},
                { new DateTime(2021,04,21), 10},
                { new DateTime(2021,04,22), 10},
            };
        }
        public void UpdateValue(DateTime date, int temperature) => UpdateValue(date, temperature, out _);
        public void AddValue(DateTime date, int temperature)
        {
            UpdateValue(date, temperature, out bool isUpdated);
            
            if (isUpdated)
                return;

            _data.Add(date, temperature);
        }
        public void DeleteValue(DateTime date)
        {
            if (_data.ContainsKey(date))
            {
                _data.Remove(date);
            }
        }
        public void DeleteRange(DateTime from, DateTime to)
        {
            _data = _data.Where(data => data.Key < from || data.Key > to).ToDictionary(
                key => key.Key,
                value => value.Value, EqualityComparer<DateTime>.Default);
        }
        public IEnumerable<int> GetTemperatureValues(DateTime from, DateTime to)
        {
            return _data
                .Where(data => data.Key >= from && data.Key <= to)
                .Select(data => data.Value);
        }
        public IEnumerable<int> GetTemperatureValues()
        {
            return _data.Select(data => data.Value);
        }
        private void UpdateValue(DateTime date, int temperature, out bool success)
        {
            success = false;
            if (_data.ContainsKey(date))
            {
                _data[date] = temperature;
                success = true;
            }
        }
        
    }
}