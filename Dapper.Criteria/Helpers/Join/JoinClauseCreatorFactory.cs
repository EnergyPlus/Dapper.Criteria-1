﻿using System;
using Dapper.Criteria.Metadata;

namespace Dapper.Criteria.Helpers.Join
{
    public class JoinClauseCreatorFactory : IJoinClauseCreatorFactory
    {
        public IJoinClauseCreator Get(Type joinAttributeType)
        {
            if (!typeof (JoinAttribute).IsAssignableFrom(joinAttributeType))
            {
                throw new ArgumentException("joinAttributeType should inherit JoinAttribute");
            }
            if (joinAttributeType == typeof (SimpleJoinAttribute))
            {
                return new SimpleJoinClauseCreator();
            }
            if (joinAttributeType == typeof (ManyToManyJoinAttribute))
            {
                return new ManyToManyClauseCreator();
            }
            throw new ArgumentOutOfRangeException(nameof(joinAttributeType));
        }
    }
}