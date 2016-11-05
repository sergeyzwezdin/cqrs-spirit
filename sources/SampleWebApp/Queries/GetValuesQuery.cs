using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CqrsSpirit;
using CqrsSpirit.Objects;
using SampleWebApp.Datasource;

namespace SampleWebApp.Queries
{
    public interface IGetValuesQuery : IQuery
    {
        Task<IEnumerable<Item>> ExecuteAsync(string filter = null);
    }

    public class GetValuesQuery : ObjectQueryBase<Datasource.Datasource>, IGetValuesQuery
    {
        public GetValuesQuery(Datasource.Datasource dbContext)
            : base(dbContext)
        {
        }

        public Task<IEnumerable<Item>> ExecuteAsync(string filter = null)
        {
            IEnumerable<Item> result = DbContext.Items;

            if (string.IsNullOrWhiteSpace(filter) == false)
            {
                result = result.Where(item => item.Value.Contains(filter));
            }

            return Task.FromResult(result);
        }
    }
}