using Dapper;
using System;
using System.Data;

namespace Xero.Products.Repository
{
    // Dapper has an issue with SqlLite text to Guid mapping. This fixes it
    public class DapperDecimalTypeHandler : SqlMapper.TypeHandler<decimal>
    {
        public override void SetValue(IDbDataParameter parameter, decimal value)
        {
            parameter.Value = value;
        }

        public override decimal Parse(object value)
        {
            return Convert.ToDecimal(value);
        }
    }
}
