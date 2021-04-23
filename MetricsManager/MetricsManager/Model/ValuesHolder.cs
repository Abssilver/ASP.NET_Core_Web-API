using System.Collections.Generic;

namespace MetricsManager.Model
{
    public class ValuesHolder
    {
        public List<string> Values { get; set; }

        public ValuesHolder()
        {
            Values = new List<string>
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
        }
    }
}