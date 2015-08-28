using DAL.Interface;
using Model;
using Nest;
using Elasticsearch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;

namespace DAL.Implement
{
    public class MemberContactDal : IMemberContactDal
    {
        public List<MemberContact> GetList(string keyword, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMCType"].ToString();
            var searchResults = Connect.GetSearchClient().Search<MemberContact>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q.QueryString(qs => qs.Query(keyword).DefaultOperator(Operator.And)))
                //.Sort(st => st.OnField(f => f.newsid).Order(SortOrder.Descending))  /*按ID排序，id为数字，排序正常*/
                .From(start)
                .Size(size)
            );
            List<MemberContact> eslist = new List<MemberContact>(searchResults.Documents);
            return eslist;
        }

        public List<MemberContact> GetList(string keyword, string type, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMCType"].ToString();
            QueryContainer prefixQuery = new PrefixQuery() { Field = type, Value = keyword };
            var searchResults = Connect.GetSearchClient().Search<MemberContact>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
                .From(start)
                .Size(size)
            );
            List<MemberContact> eslist = new List<MemberContact>(searchResults.Documents);
            return eslist;
        }

        public int GetCount(string keyword)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMCType"].ToString();
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<MemberContact>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q.QueryString(qs => qs.Query(keyword).DefaultOperator(Operator.And)))
            );
            count = (int)counts.Count;
            return count;
        }

        public int GetCount(string keyword, string type)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMCType"].ToString();
            QueryContainer prefixQuery = new PrefixQuery() { Field = type, Value = keyword };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<MemberContact>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
            );
            count = (int)counts.Count;
            return count;
        }
    }
}
