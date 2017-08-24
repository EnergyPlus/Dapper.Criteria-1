﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dapper.Criteria.Dapper
{
    public class SqlBuilder
    {
        private readonly Dictionary<string, Clauses> _data = new Dictionary<string, Clauses>();
        private int _seq;


        public Template AddTemplate(string sql, dynamic parameters = null)
        {
            return new Template(this, sql, parameters);
        }

        private void AddClause(string name, string sql, object parameters, string joiner, string prefix = "",
            string postfix = "")
        {
            Clauses clauses;
            if (!_data.TryGetValue(name, out clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                _data[name] = clauses;
            }
            clauses.Add(new Clause {Sql = sql, Parameters = parameters});
            _seq++;
        }

        public SqlBuilder InnerJoin(string sql, dynamic parameters = null)
        {
            AddClause("innerjoin", sql, parameters, joiner: "\nINNER JOIN ", prefix: "\nINNER JOIN ", postfix: "\n");
            return this;
        }

        public SqlBuilder LeftJoin(string sql, dynamic parameters = null)
        {
            AddClause("leftjoin", sql, parameters, joiner: "\nLEFT JOIN ", prefix: "\nLEFT JOIN ", postfix: "\n");
            return this;
        }

        public SqlBuilder RightJoin(string sql, dynamic parameters = null)
        {
            AddClause("rightjoin", sql, parameters, joiner: "\nRIGHT JOIN ", prefix: "\nRIGHT JOIN ", postfix: "\n");
            return this;
        }

        public SqlBuilder SimpleSql(string sql, dynamic parameters = null)
        {
            AddClause("simplesql", sql, parameters, joiner: "\n ", prefix: "\n ", postfix: "\n");
            return this;
        }

        public SqlBuilder Where(string sql, dynamic parameters = null)
        {
            AddClause("where", sql, parameters, " AND ", prefix: "WHERE ", postfix: "\n");
            return this;
        }

        public SqlBuilder OrderBy(string sql, dynamic parameters = null)
        {
            AddClause("orderby", sql, parameters, " , ", prefix: "ORDER BY ", postfix: "\n");
            return this;
        }

        public SqlBuilder Select(string sql, dynamic parameters = null)
        {
            AddClause("select", sql, parameters, " , ", prefix: "", postfix: "\n");
            return this;
        }

        public SqlBuilder AddParameters(dynamic parameters)
        {
            AddClause("--parameters", "", parameters, "");
            return this;
        }

        public SqlBuilder Join(string sql, dynamic parameters = null)
        {
            AddClause("join", sql, parameters, joiner: "\nJOIN ", prefix: "\nJOIN ", postfix: "\n");
            return this;
        }

        public SqlBuilder GroupBy(string sql, dynamic parameters = null)
        {
            AddClause("groupby", sql, parameters, joiner: " , ", prefix: "\nGROUP BY ", postfix: "\n");
            return this;
        }

        public SqlBuilder Having(string sql, dynamic parameters = null)
        {
            AddClause("having", sql, parameters, joiner: "\nAND ", prefix: "HAVING ", postfix: "\n");
            return this;
        }

        public void Clear()
        {
            _data.Clear();
        }

        private class Clause
        {
            public string Sql { get; set; }
            public object Parameters { get; set; }
        }

        private class Clauses : List<Clause>
        {
            private readonly string _joiner;
            private readonly string _postfix;
            private readonly string _prefix;

            public Clauses(string joiner, string prefix = "", string postfix = "")
            {
                _joiner = joiner;
                _prefix = prefix;
                _postfix = postfix;
            }

            public string ResolveClauses(DynamicParameters p)
            {
                foreach (var item in this)
                {
                    p.AddDynamicParams(item.Parameters);
                }
                return _prefix + string.Join(_joiner, this.Select(c => c.Sql)) + _postfix;
            }
        }

        public class Template
        {
            private static readonly Regex Regex =
                new Regex(@"\/\*\*.+\*\*\/", RegexOptions.Compiled | RegexOptions.Multiline);

            private readonly SqlBuilder _builder;
            private readonly object _initParams;
            private readonly string _sql;
            private int _dataSeq = -1; // Unresolved

            private object _parameters;
            private string _rawSql;

            public Template(SqlBuilder builder, string sql, dynamic parameters)
            {
                _initParams = parameters;
                _sql = sql;
                _builder = builder;
            }

            public string RawSql
            {
                get
                {
                    ResolveSql();
                    return _rawSql;
                }
            }

            public object Parameters
            {
                get
                {
                    ResolveSql();
                    return _parameters;
                }
            }

            private void ResolveSql()
            {
                if (_dataSeq != _builder._seq)
                {
                    var p = new DynamicParameters(_initParams);

                    _rawSql = _sql;

                    foreach (var pair in _builder._data)
                    {
                        _rawSql = _rawSql.Replace("/**" + pair.Key + "**/", pair.Value.ResolveClauses(p));
                    }
                    _parameters = p;

                    // replace all that is left with empty
                    _rawSql = Regex.Replace(_rawSql, "");

                    _dataSeq = _builder._seq;
                }
            }
        }
    }
}