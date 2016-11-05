using CqrsSpirit.Objects;

namespace CqrsSpirit.Tests.Stubs
{
    internal interface IQuery1 : IQuery
    {
    }

    internal class Query1 : ObjectQueryBase<object>, IQuery1
    {
        public Query1(object dbContext)
            : base(dbContext)
        {
        }
    }

    internal class Query11 : ObjectQueryBase<object>, IQuery1
    {
        public Query11(object dbContext)
            : base(dbContext)
        {
        }
    }

    internal interface IQuery2 : IQuery
    {
    }

    internal interface IQuery3 : IQuery
    {
    }

    internal class Query2 : ObjectQueryBase<object>, IQuery2
    {
        public Query2(object dbContext)
            : base(dbContext)
        {
        }
    }
}