using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Configuration;
using Nest;
using common;

namespace DAL.Implement
{
    public class VSmsClassDal : IVSmsClassDal
    {
        public int GetCount(string keyword)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VSmsClassType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<VSmsClass>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
            );
            count = (int)counts.Count;
            return count;
        }

        public int GetCount(string keyword, string type)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VSmsClassType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<VSmsClass>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
            );
            count = (int)counts.Count;
            return count;
        }

        public List<VSmsClass> GetList(string keyword, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VSmsClassType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or };
            var searchResults = Connect.GetSearchClient().Search<VSmsClass>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
                //.Sort(st => st.OnField(f => f.newsid).Order(SortOrder.Descending))  /*按ID排序，id为数字，排序正常*/
                .From(start)
                .Size(size)
            );
            List<VSmsClass> eslist = new List<VSmsClass>(searchResults.Documents);
            return eslist;
        }

        public List<VSmsClass> GetList(string keyword, string type, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VSmsClassType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type };
            var searchResults = Connect.GetSearchClient().Search<VSmsClass>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
                //.Sort(st => st.OnField(f => f.newsid).Order(SortOrder.Descending))  /*按ID排序，id为数字，排序正常*/
                .From(start)
                .Size(size)
            );
            List<VSmsClass> eslist = new List<VSmsClass>(searchResults.Documents);
            return eslist;
        }
    }
}
