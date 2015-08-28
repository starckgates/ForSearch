using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL.Interface;
using System.Configuration;
using Nest;
using common;

namespace DAL.Implement
{
    public class MemberDal : IMemberDal
    {
        /// <summary>
        /// 获取member
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<Member> GetList(string keyword, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
                //.Sort(st => st.OnField(f => f.newsid).Order(SortOrder.Descending))  /*按ID排序，id为数字，排序正常*/
                .From(start)
                .Size(size)
            );
            List<Member> eslist = new List<Member>(searchResults.Documents);
            return eslist;
        }

        public List<Member> GetList(string keyword, string type, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMType"].ToString();
            //QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type};
            //QueryContainer termQuery = new TermQuery { Field = type, Value = keyword };
            QueryContainer prefixQuery = new PrefixQuery { Field = type, Value = keyword };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
                //.Sort(st => st.OnField(f => f.newsid).Order(SortOrder.Descending))  /*按ID排序，id为数字，排序正常*/
                .From(start)
                .Size(size)
            );
            List<Member> eslist = new List<Member>(searchResults.Documents);
            return eslist;
        }

        public List<Member> GetListByPrefix(string keyword, string type, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMType"].ToString();
            QueryContainer prefixQuery = new PrefixQuery() { Field = type, Value = keyword };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
                .From(start)
                .Size(size)
            );
            List<Member> eslist = new List<Member>(searchResults.Documents);
            return eslist;
        }


        /// <summary>
        /// 获取查询条件的总数量
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public int GetCount(string keyword)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMType"].ToString();
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q.QueryString(qs => qs.Query(keyword).DefaultOperator(Operator.Or)))
            );
            count = (int)counts.Count;
            return count;
        }

        public int GetCount(string keyword,string type)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMType"].ToString();
            //QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type };
            //QueryContainer termQuery = new TermQuery { Field = type, Value = keyword };
            QueryContainer prefixQuery = new PrefixQuery { Field = type, Value = keyword };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
            );
            count = (int)counts.Count;
            return count;
        }

        public int GetCountByPrefix(string keyword, string type)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMType"].ToString();
            QueryContainer prefixQuery = new PrefixQuery { Field = type, Value = keyword };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
            );
            count = (int)counts.Count;
            return count;
        }
    }
}
