using System;
using System.Data;
using Dapper;

namespace MetricsManager.MappingSettings
{
    // задаем хэндлер для парсинга значений в DateTimeOffset если таковые попадутся в наших классах моделей
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
            => DateTimeOffset.FromUnixTimeSeconds((long)value);

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
            => parameter.Value = value;
    }
}