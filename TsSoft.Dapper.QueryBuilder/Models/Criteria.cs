using System.Collections.Generic;
using System.Linq;
using TsSoft.Dapper.QueryBuilder.Metadata;
using TsSoft.Dapper.QueryBuilder.Models.Enumerations;

namespace TsSoft.Dapper.QueryBuilder.Models
{
    public class Criteria
    {
        /// <summary>
        /// ��� �������� � ����������
        /// ������� ������� �������
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// ��� �������� � ����������
        /// ������� ������� ����������
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// ��� ������� (�������, � ����������, ������ ���������� � �.�.)
        /// </summary>
        public QueryType QueryType { get; set; }

        /// <summary>
        /// ����������
        /// � ����� ���������� ������� ���������� ��� ������� (��������, Tasks.Deadline)
        /// � �������� �����������
        /// </summary>
        public IDictionary<string, OrderType> Order { get; set; }

        /// <summary>
        /// ������� ����������
        /// </summary>
        public bool HasOrder
        {
            get { return Order != null && Order.Any(); }
        }
    }
}