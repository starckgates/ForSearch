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
    public class VCommonProductDal : IVCommonProductDal
    {
        public int GetCount(string keyword)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VCProductType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<VCommonProduct>(s => s
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
            string typename = ConfigurationManager.AppSettings["VCProductType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<VCommonProduct>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
            );
            count = (int)counts.Count;
            return count;
        }

        public List<VCommonProduct> GetList(string keyword, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VCProductType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or };
            var searchResults = Connect.GetSearchClient().Search<VCommonProduct>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
                .From(start)
                .Size(size)
            );
            List<VCommonProduct> eslist = new List<VCommonProduct>(searchResults.Documents);
            return eslist;
        }

        public List<VCommonProduct> GetList(string keyword, string type, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["VCProductType"].ToString();
            QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type };
            var searchResults = Connect.GetSearchClient().Search<VCommonProduct>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(query)
                .From(start)
                .Size(size)
            );
            List<VCommonProduct> eslist = new List<VCommonProduct>(searchResults.Documents);
            return eslist;
        }
    }
}
