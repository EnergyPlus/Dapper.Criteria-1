﻿using System;
using Dapper.Criteria.Metadata;

namespace Dapper.Criteria.Helpers.Join
{
    public abstract class JoinClauseCreator : IJoinClauseCreator
    {
        public abstract JoinClause Create(JoinAttribute joinAttribute);

        public JoinClause CreateNotJoin(JoinAttribute joinAttribute)
        {
            SimpleJoinAttribute simpleJoinAttribute;
            if ((simpleJoinAttribute = joinAttribute as SimpleJoinAttribute) == null)
            {
                throw new ArgumentException("Attribute must be SimpleJoinAttribute");
            }
            var splitter = GetSplitter(simpleJoinAttribute);
            return new JoinClause
                {
                    HasJoin = false,
                    Splitter = splitter,
                    JoinType = simpleJoinAttribute.JoinType,
                    Order = joinAttribute.Order,
                };
        }

        protected string GetSplitter(SimpleJoinAttribute joinAttribute)
        {
            return joinAttribute.NoSplit
                       ? string.Empty
                       : $"SplitOn{joinAttribute.JoinedTable.Replace("[", "").Replace("]", "")}{joinAttribute.JoinedTableField}";
        }

        protected virtual string GetAddOnClauses(JoinAttribute joinAttribute)
        {
            return string.IsNullOrWhiteSpace(joinAttribute.AddOnClause)
                       ? string.Empty
                       : $" AND {joinAttribute.AddOnClause}";
        }
    }
}