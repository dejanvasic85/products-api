using System.Collections.Generic;
using System.Linq;

namespace Xero.Products.BusinessLayer
{
    public class ListResponse<T> where T : class
    {
        public T[] Items { get; private set; }

        public ListResponse(IEnumerable<T> items)
        {
            Items = items.ToArray();
        }
    }
}

