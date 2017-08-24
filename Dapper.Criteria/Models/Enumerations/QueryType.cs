﻿namespace Dapper.Criteria.Models.Enumerations
{
    public enum QueryType
    {
        Simple = 0,
        Paginate = 1,
        OnlyCount = 2,
        Exists = 3,
        Sum = 4,
    };
}