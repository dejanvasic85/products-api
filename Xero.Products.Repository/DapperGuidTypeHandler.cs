using Dapper;
using System;
using System.Data;

namespace Xero.Products.Repository
{
    // Dapper has an issue with SqlLite double to decimal mapping. This fixes it.
    public class DapperGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }

        public override Guid Parse(object value)
        {
            return new Guid(value.ToString());
        }
    }
}
