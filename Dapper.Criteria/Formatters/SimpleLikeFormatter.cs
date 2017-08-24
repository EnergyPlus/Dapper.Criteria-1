﻿namespace Dapper.Criteria.Formatters
{
    public class SimpleLikeFormatter : IFormatter
    {
        public void Format(ref object input)
        {
            input = string.Format("%{0}%", input);
        }
    }
}