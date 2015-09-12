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
    public class MemberTrackDal : IMemberTrackDal
    {
        public class SearchCondition {
            public string keyword { get; set; }
            public string field { get; set; }
            public int start { get; set; }
            public int size { get; set; }
            public string country { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string address { get; set; }
            public string memo { get; set; }
            public string classx { get; set; }
            public string trackType { get; set; }
            public string afterSales { get; set; }
            
        }

        public List<Member> GetList(SearchCondition sc) {
            return null;
        }

        public List<Member> GetList(string keyword, string field, int start, int size,string country, string province,string city,string address,string memo,string classx,string trackType,string afterSales,out int em)
        {
            string indexname = ConfigurationManager.AppSettings["CRMMTIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMTType"].ToString();
            if (string.IsNullOrEmpty(field))
            { field = "Memo"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(classx))
            {
                classx = "";
            }
            else {
                classx = "Class" + classx.ToString();
                classVaule = "1";
            }
            //QueryContainer matchQuery = new MatchQuery() { Field = field, Query = keyword, Operator = Operator.And };
            //QueryContainer hasChildQuery = new HasChildQuery() { Type = ctype, Query = matchQuery };
            //QueryContainer boolQuery = new BoolQuery() { };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q
                .Bool(b => b
                .Must(
                    m => m.HasChild<MemberTrack>(h => h.Type(ctype).Query(qu => qu.Match(mt => mt.OnField(field).Query(keyword).Operator(Operator.And)))),
                    m => m.Bool( bl => bl.Must(
                        mu => mu.Match(mt => mt.OnField("Country").Query(Convert.ToString(country)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField("Province").Query(Convert.ToString(province)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField("City").Query(Convert.ToString(city)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField("Address").Query(Convert.ToString(address)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField("Memo").Query(Convert.ToString(memo)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField("TrackTypeID").Query(Convert.ToString(trackType)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField("AfterSalesID").Query(Convert.ToString(afterSales)).Operator(Operator.And)),
                        mu => mu.Match(mt => mt.OnField(classx).Query(classVaule).Operator(Operator.And))
                        )
                    )
                )
                ))
                .From(start)
                .Size(size)
            );
            em = searchResults.ElapsedMilliseconds;
            List<Member> eslist = new List<Member>(searchResults.Documents);
            return eslist;
        }

        public string GetList(SearchCondition sc, out int em)
        {
            string indexname = ConfigurationManager.AppSettings["CRMMTIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMTType"].ToString();
            if (string.IsNullOrEmpty(sc.field))
            { sc.field = "Memo"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = "Class" + sc.classx.ToString();
                classVaule = "1";
            }
            //QueryContainer matchQuery = new MatchQuery() { Field = field, Query = keyword, Operator = Operator.And };
            //QueryContainer hasChildQuery = new HasChildQuery() { Type = ctype, Query = matchQuery };
            //QueryContainer boolQuery = new BoolQuery() { };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Fields("ID", "ADMINID", "Memo")
                .Query(q => q
                .Bool(b => b
                .Must(
                    m => m.HasChild<MemberTrack>(h => h.Type(ctype).Query(qu => qu.Match(mt => mt.OnField(sc.field).Query(sc.keyword).Operator(Operator.And)))),
                    m => m.Bool(bl => bl.Must(
                       mu => mu.Match(mt => mt.OnField("Country").Query(Convert.ToString(sc.country)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("Province").Query(Convert.ToString(sc.province)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("City").Query(Convert.ToString(sc.city)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("Address").Query(Convert.ToString(sc.address)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("Memo").Query(Convert.ToString(sc.memo)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("TrackTypeID").Query(Convert.ToString(sc.trackType)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("AfterSalesID").Query(Convert.ToString(sc.afterSales)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule).Operator(Operator.And))
                       )
                    )
                )
                ))
                .From(sc.start)
                .Size(sc.size)
            );
            em = searchResults.ElapsedMilliseconds;
            StringBuilder ResultJson = new StringBuilder("");
            ResultJson.Append("[");
            foreach (var hit in searchResults.Hits)
            {
                ResultJson.Append("{");
                ResultJson.Append(
                    String.Join(",",
                        hit.Fields.FieldValuesDictionary
                            .Select(FVD => "'" + FVD.Key + "':'" + FVD.Value.ToString().Replace("[","").Replace("]","") + "'")
                            .ToArray()
                    )
                );
                ResultJson.Append("},");
            }
            if (ResultJson.Length > 1)
                ResultJson.Length = ResultJson.Length - 1;

            ResultJson.Append("]");
            //foreach (var hit in searchResults.Hits)
            //{
            //    List<string> list = new List<string>();
            //    list.Add(hit.Fields.FieldValuesDictionary);
            //}
            //List<Member> eslist = new List<Member>(searchResults.Documents);
            return ResultJson.ToString();
        }


        public int GetCount(string keyword, string field)
        {
            string indexname = ConfigurationManager.AppSettings["CRMMTIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMTType"].ToString();
            if (string.IsNullOrEmpty(field))
            { field = "Memo"; }
            QueryContainer matchQuery = new MatchQuery() { Field = field, Query = keyword, Operator = Operator.And };
            QueryContainer hasChildQuery = new HasChildQuery() { Type = ctype, Query = matchQuery };
            var results = Connect.GetSearchClient().Count<Member>(c => c
                .Index(indexname)
                .Type(typename)
                .Query(hasChildQuery)
                );
            return (int)results.Count;

        }
    }
}
