using System.Collections.Generic;
using System.Linq;
using TsSoft.Dapper.QueryBuilder.Helpers.Select;
using TsSoft.Dapper.QueryBuilder.Models.Enumerations;

namespace TsSoft.Dapper.QueryBuilder.Models
{
    public class Criteria
    {
        public Criteria()
        {
            Select = new SelectClause
            {
                IsExpression = false,
                Select = "*",
            };
        }
        public int Take { get; set; }

        public int Skip { get; set; }

        public QueryType QueryType { get; set; }

        public IDictionary<string, OrderType> Order { get; set; }

        public SelectClause Select { get; set; }

        public bool HasOrder
        {
            get { return Order != null && Order.Any(); }
        }
    }
}