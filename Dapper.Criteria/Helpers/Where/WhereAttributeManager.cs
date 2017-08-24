﻿using System;
using System.ComponentModel;
using System.Linq;
using Dapper.Criteria.Models.Enumerations;

namespace Dapper.Criteria.Helpers.Where
{
    public class WhereAttributeManager : IWhereAttributeManager
    {
        public bool IsWithoutValue(WhereType whereType)
        {
            switch (whereType)
            {
                case WhereType.Eq:
                case WhereType.NotEq:
                case WhereType.Gt:
                case WhereType.Lt:
                case WhereType.GtEq:
                case WhereType.LtEq:
                case WhereType.Like:
                case WhereType.In:
                case WhereType.NotIn:
                    return false;
                case WhereType.IsNull:
                case WhereType.IsNotNull:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("whereType");
            }
        }

        public string GetExpression(WhereType whereType, string paramName)
        {
            switch (whereType)
            {
                case WhereType.IsNull:
                case WhereType.IsNotNull:
                    return GetSelector(whereType);
                case WhereType.Eq:
                case WhereType.NotEq:
                case WhereType.Gt:
                case WhereType.Lt:
                case WhereType.GtEq:
                case WhereType.LtEq:
                case WhereType.In:
                case WhereType.Like:
                case WhereType.NotIn:
                    return string.Format("{0} {1}", GetSelector(whereType), paramName);
                default:
                    throw new ArgumentOutOfRangeException("whereType");
            }
        }

        public string GetSelector(WhereType whereType)
        {
            if (!Enum.GetValues(typeof (WhereType)).Cast<WhereType>().Contains(whereType))
            {
                throw new ArgumentOutOfRangeException("whereType");
            }
            return whereType.GetAttribute<DescriptionAttribute>().Description;
        }
    }
}