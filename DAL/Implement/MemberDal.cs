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
        #region SearchCondition 查询参数
        public class SearchCondition
        {
            #region 子表查询字段，当只查询member时也使用
            public string flag { get; set; }
            public string keyword { get; set; }
            public string field { get; set; }
            #endregion
            #region 分页
            public int start { get; set; }
            public int size { get; set; }
            #endregion
            #region member表查询字段
            public string country { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string address { get; set; }
            public string memo { get; set; }
            public string classx { get; set; }
            public string tracktype { get; set; }
            public string aftersales { get; set; }
            #endregion
        }
        #endregion
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
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
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

        public List<Member> GetList(string keyword, string field, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            //QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type};
            //QueryContainer termQuery = new TermQuery { Field = type, Value = keyword };
            if (string.IsNullOrEmpty(field))
            { field = "_all"; }
            else { field = field.ToLower(); }
            //QueryContainer prefixQuery = new PrefixQuery { Field = field, Value = keyword };
            //QueryContainer matchQuery = new MatchQuery() { Field = field, Query = keyword, Type = "phrase" };
            QueryContainer wildcardQuery = new WildcardQuery() {Field = field ,Value = string.Format("*{0}*", keyword) };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(wildcardQuery)
                //.Sort(st => st.OnField(f => f.newsid).Order(SortOrder.Descending))  /*按ID排序，id为数字，排序正常*/
                .From(start)
                .Size(size)
            );
            List<Member> eslist = new List<Member>(searchResults.Documents);
            return eslist;
        }


        public List<Member> GetListByPrefix(string keyword, string field, int start, int size)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            if (string.IsNullOrEmpty(field))
            { field = "_all"; }
            else { field = field.ToLower(); }
            QueryContainer prefixQuery = new PrefixQuery() { Field = field, Value = keyword };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(prefixQuery)
                .Analyzer("standard")
                .From(start)
                .Size(size)
            );
            List<Member> eslist = new List<Member>(searchResults.Documents);
            return eslist;
        }

        

        /// <summary>
        /// getlist
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="field"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="memo"></param>
        /// <param name="classx"></param>
        /// <param name="trackType"></param>
        /// <param name="afterSales"></param>
        /// <param name="em"></param>
        /// <returns></returns>
        public List<Member> GetListByTrack(string keyword, string field, int start, int size, string country, string province, string city, string address, string memo, string classx, string trackType, string afterSales, out int em)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMTrackType"].ToString();
            if (string.IsNullOrEmpty(field))
            { field = "memo"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(classx))
            {
                classx = "";
            }
            else
            {
                classx = "class" + classx.ToString();
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
                    m => m.Bool(bl => bl.Must(
                       mu => mu.Match(mt => mt.OnField("country").Query(Convert.ToString(country)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("province").Query(Convert.ToString(province)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("city").Query(Convert.ToString(city)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("address").Query(Convert.ToString(address)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("memo").Query(Convert.ToString(memo)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(trackType)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField("aftersalesid").Query(Convert.ToString(afterSales)).Operator(Operator.And)),
                       mu => mu.Match(mt => mt.OnField(classx).Query(classVaule).Operator(Operator.And)),
                       mu => mu.Wildcard(wc =>wc.OnField("").Value(""))
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
            if (!string.IsNullOrEmpty(sc.flag))
            {
                switch (sc.flag)
                {
                    case "member":
                        return GetMember(sc, out em);
                    case "member_track":
                        return GetListByTrack(sc,out em);
                    case "member_contact":
                        return GetListByContact(sc, out em);
                    default:
                        em = -1;
                        return "";
                }
            }
            else {
                em = -1;
                return "";
            }
        }

        public string GetMember(SearchCondition sc, out int em)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string field = "";
            if (string.IsNullOrEmpty(sc.field))
            { field = ""; }
            else { field = sc.field.ToLower(); }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = sc.classx.ToLower();
                classVaule = "1";
            }
            //QueryContainer wildcardQuery = new WildcardQuery() { Field = field, Value = string.IsNullOrEmpty(sc.keyword) ? "*" : string.Format("*{0}*", sc.keyword) };
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Fields(f => f.Id, f => f.Enterprise, f => f.Isstar, f => f.Masterlinkman, f => f.Tracktypeid, f => f.Lastcontacttime, f => f.Enddate, f => f.Adminname, f => f.Addtime)
                .Query(q => q
                .Bool(b => b
                .Must(
                       mu => mu.Wildcard(mt => mt.OnField(field).Value(Convert.ToString(string.IsNullOrEmpty(sc.keyword) ? "" : string.Format("*{0}*", sc.keyword)))),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(sc.tracktype))),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule))
                )
                ))
                .Sort(st => st.OnField(f => f.Lastcontacttime).Order(SortOrder.Descending))  /*排序*/
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
                            .Select(FVD => "'" + FVD.Key + "':'" + FVD.Value.ToString().Replace("[", "").Replace("]", "") + "'")
                            .ToArray()
                    )
                );
                ResultJson.Append("},");
            }
            if (ResultJson.Length > 1)
                ResultJson.Length = ResultJson.Length - 1;
            ResultJson.Append("]");
            return ResultJson.ToString();
        }

        /// <summary>
        /// getjson
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="em"></param>
        /// <returns></returns>
        public string GetListByTrack(SearchCondition sc, out int em)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMTrackType"].ToString();
            if (string.IsNullOrEmpty(sc.field))
            { sc.field = "memo"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = sc.classx.ToLower();
                classVaule = "1";
            }
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Fields(f => f.Id, f => f.Enterprise, f => f.Isstar, f => f.Masterlinkman, f => f.Tracktypeid, f => f.Lastcontacttime, f => f.Enddate, f => f.Adminname, f => f.Addtime)
                .Query(q => q
                .Bool(b => b
                .Must(
                    m => m.HasChild<MemberTrack>(h => h.Type(ctype).Query(qu => qu.Prefix(mt => mt.OnField(sc.field.ToLower()).Value(sc.keyword)))),
                    m => m.Bool(bl => bl.Must(
                       mu => mu.Wildcard(mt => mt.OnField("country").Value(Convert.ToString(string.IsNullOrEmpty(sc.country) ? "": string.Format("*{0}*",sc.country)))),
                       mu => mu.Wildcard(mt => mt.OnField("province").Value(Convert.ToString(string.IsNullOrEmpty(sc.province) ? "" : string.Format("*{0}*",sc.province)))),
                       mu => mu.Wildcard(mt => mt.OnField("city").Value(Convert.ToString(string.IsNullOrEmpty(sc.city) ? "" : string.Format("*{0}*",sc.city)))),
                       mu => mu.Wildcard(mt => mt.OnField("address").Value(Convert.ToString(string.IsNullOrEmpty(sc.address) ? "" : string.Format("*{0}*",sc.address)))),
                       mu => mu.Wildcard(mt => mt.OnField("memo").Value(Convert.ToString(string.IsNullOrEmpty(sc.memo) ? "" : string.Format("*{0}*",sc.memo)))),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(sc.tracktype))),
                       mu => mu.Wildcard(mt => mt.OnField("aftersalesid").Value(Convert.ToString(string.IsNullOrEmpty(sc.aftersales) ? "" : string.Format("*{0}*",sc.aftersales)))),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule))
                       )
                    )
                )
                ))
                .Sort(st => st.OnField(f => f.Lastcontacttime).Order(SortOrder.Descending))  /*排序*/
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
                            .Select(FVD => "'" + FVD.Key + "':'" + FVD.Value.ToString().Replace("[", "").Replace("]", "") + "'")
                            .ToArray()
                    )
                );
                ResultJson.Append("},");
            }
            if (ResultJson.Length > 1)
                ResultJson.Length = ResultJson.Length - 1;
            ResultJson.Append("]");
            return ResultJson.ToString();
        }

        public string GetListByContact(SearchCondition sc, out int em)
        {
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMenberContactType"].ToString();
            if (string.IsNullOrEmpty(sc.field))
            { sc.field = "Email"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = sc.classx.ToLower();
                classVaule = "1";
            }
            var searchResults = Connect.GetSearchClient().Search<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Fields(f => f.Id, f => f.Enterprise, f => f.Isstar, f => f.Masterlinkman, f => f.Tracktypeid, f => f.Lastcontacttime, f => f.Enddate, f => f.Adminname, f => f.Addtime)
                .Query(q => q
                .Bool(b => b
                .Must(
                    m => m.HasChild<MemberContact>(h => h.Type(ctype).Query(qu => qu.Prefix(mt => mt.OnField(sc.field.ToLower()).Value(sc.keyword)))),
                    m => m.Bool(bl => bl.Must(
                       mu => mu.Wildcard(mt => mt.OnField("country").Value(Convert.ToString(string.IsNullOrEmpty(sc.country) ? "" : string.Format("*{0}*", sc.country)))),
                       mu => mu.Wildcard(mt => mt.OnField("province").Value(Convert.ToString(string.IsNullOrEmpty(sc.province) ? "" : string.Format("*{0}*", sc.province)))),
                       mu => mu.Wildcard(mt => mt.OnField("city").Value(Convert.ToString(string.IsNullOrEmpty(sc.city) ? "" : string.Format("*{0}*", sc.city)))),
                       mu => mu.Wildcard(mt => mt.OnField("address").Value(Convert.ToString(string.IsNullOrEmpty(sc.address) ? "" : string.Format("*{0}*", sc.address)))),
                       mu => mu.Wildcard(mt => mt.OnField("memo").Value(Convert.ToString(string.IsNullOrEmpty(sc.memo) ? "" : string.Format("*{0}*", sc.memo)))),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(sc.tracktype))),
                       mu => mu.Wildcard(mt => mt.OnField("aftersalesid").Value(Convert.ToString(string.IsNullOrEmpty(sc.aftersales) ? "" : string.Format("*{0}*", sc.aftersales)))),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule))
                       )
                    )
                )
                ))
                .Sort(st => st.OnField(f => f.Lastcontacttime).Order(SortOrder.Descending))  /*排序*/
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
                            .Select(FVD => "'" + FVD.Key + "':'" + FVD.Value.ToString().Replace("[", "").Replace("]", "") + "'")
                            .ToArray()
                    )
                );
                ResultJson.Append("},");
            }
            if (ResultJson.Length > 1)
                ResultJson.Length = ResultJson.Length - 1;
            ResultJson.Append("]");
            return ResultJson.ToString();
        }

        public int GetCountByM(SearchCondition sc)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string field = "";
            if (string.IsNullOrEmpty(sc.field))
            { field = ""; }
            else { field = sc.field.ToLower(); }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = sc.classx.ToLower();
                classVaule = "1";
            }
            //QueryContainer wildcardQuery = new WildcardQuery() { Field = field, Value = string.IsNullOrEmpty(sc.keyword) ? "*" : string.Format("*{0}*", sc.keyword) };
            var searchResults = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q
                .Bool(b => b
                .Must(
                       mu => mu.Wildcard(mt => mt.OnField(field).Value(Convert.ToString(string.IsNullOrEmpty(sc.keyword) ? "" : string.Format("*{0}*", sc.keyword)))),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(sc.tracktype))),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule))
                )
                ))
            );
            count = (int)searchResults.Count;
            return count;
        }
        public int GetCountByTrack(SearchCondition sc)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMTrackType"].ToString();
            if (string.IsNullOrEmpty(sc.field))
            { sc.field = "memo"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = sc.classx.ToLower();
                classVaule = "1";
            }
            var searchResults = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q
                .Bool(b => b
                .Must(
                    m => m.HasChild<MemberTrack>(h => h.Type(ctype).Query(qu => qu.Prefix(mt => mt.OnField(sc.field.ToLower()).Value(sc.keyword)))),
                    m => m.Bool(bl => bl.Must(
                       mu => mu.Wildcard(mt => mt.OnField("country").Value(Convert.ToString(string.IsNullOrEmpty(sc.country) ? "" : string.Format("*{0}*", sc.country)))),
                       mu => mu.Wildcard(mt => mt.OnField("province").Value(Convert.ToString(string.IsNullOrEmpty(sc.province) ? "" : string.Format("*{0}*", sc.province)))),
                       mu => mu.Wildcard(mt => mt.OnField("city").Value(Convert.ToString(string.IsNullOrEmpty(sc.city) ? "" : string.Format("*{0}*", sc.city)))),
                       mu => mu.Wildcard(mt => mt.OnField("address").Value(Convert.ToString(string.IsNullOrEmpty(sc.address) ? "" : string.Format("*{0}*", sc.address)))),
                       mu => mu.Wildcard(mt => mt.OnField("memo").Value(Convert.ToString(string.IsNullOrEmpty(sc.memo) ? "" : string.Format("*{0}*", sc.memo)))),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(sc.tracktype))),
                       mu => mu.Wildcard(mt => mt.OnField("aftersalesid").Value(Convert.ToString(string.IsNullOrEmpty(sc.aftersales) ? "" : string.Format("*{0}*", sc.aftersales)))),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule))
                       )
                    )
                )
                ))
            );
            count = (int)searchResults.Count;
            return count;
        }
        public int GetCountByContact(SearchCondition sc)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            string ctype = ConfigurationManager.AppSettings["CRMMenberContactType"].ToString();
            if (string.IsNullOrEmpty(sc.field))
            { sc.field = "Email"; }
            string classVaule = "";
            if (string.IsNullOrEmpty(sc.classx))
            {
                sc.classx = "";
            }
            else
            {
                sc.classx = "class" + sc.classx.ToString();
                classVaule = "1";
            }
            var searchResults = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q
                .Bool(b => b
                .Must(
                    m => m.HasChild<MemberContact>(h => h.Type(ctype).Query(qu => qu.Prefix(mt => mt.OnField(sc.field.ToLower()).Value(sc.keyword)))),
                    m => m.Bool(bl => bl.Must(
                       mu => mu.Wildcard(mt => mt.OnField("country").Value(Convert.ToString(string.IsNullOrEmpty(sc.country) ? "" : string.Format("*{0}*", sc.country)))),
                       mu => mu.Wildcard(mt => mt.OnField("province").Value(Convert.ToString(string.IsNullOrEmpty(sc.province) ? "" : string.Format("*{0}*", sc.province)))),
                       mu => mu.Wildcard(mt => mt.OnField("city").Value(Convert.ToString(string.IsNullOrEmpty(sc.city) ? "" : string.Format("*{0}*", sc.city)))),
                       mu => mu.Wildcard(mt => mt.OnField("address").Value(Convert.ToString(string.IsNullOrEmpty(sc.address) ? "" : string.Format("*{0}*", sc.address)))),
                       mu => mu.Wildcard(mt => mt.OnField("memo").Value(Convert.ToString(string.IsNullOrEmpty(sc.memo) ? "" : string.Format("*{0}*", sc.memo)))),
                       mu => mu.Match(mt => mt.OnField("tracktypeid").Query(Convert.ToString(sc.tracktype))),
                       mu => mu.Wildcard(mt => mt.OnField("aftersalesid").Value(Convert.ToString(string.IsNullOrEmpty(sc.aftersales) ? "" : string.Format("*{0}*", sc.aftersales)))),
                       mu => mu.Match(mt => mt.OnField(sc.classx).Query(classVaule))
                       )
                    )
                )
                ))
            );
            count = (int)searchResults.Count;
            return count;
        }

        public int GetCount(SearchCondition sc)
        {
            if (!string.IsNullOrEmpty(sc.flag))
            {
                switch (sc.flag)
                {
                    case "member":
                        return GetCountByM(sc);
                    case "member_track":
                        return GetCountByTrack(sc);
                    case "member_contact":
                        return GetCountByContact(sc);
                    default:
                        return -1;
                }
            }
            else
            {
                return -1;
            }
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
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(q => q.QueryString(qs => qs.Query(keyword).DefaultOperator(Operator.Or)))
            );
            count = (int)counts.Count;
            return count;
        }

        public int GetCount(string keyword,string field)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            //QueryContainer query = new QueryStringQuery() { Query = keyword, DefaultOperator = Operator.Or, DefaultField = type };
            //QueryContainer termQuery = new TermQuery { Field = type, Value = keyword };
            if (string.IsNullOrEmpty(field))
            { field = "_all"; }
            else { field = field.ToLower(); }
            //QueryContainer prefixQuery = new PrefixQuery { Field = field, Value = keyword };
            //QueryContainer matchQuery = new MatchQuery() { Field = field, Query = keyword, Operator = Operator.And };
            QueryContainer wildcardQuery = new WildcardQuery() { Field = field, Value = string.Format("*{0}*", keyword) };
            //调用仅取数量方法
            var counts = Connect.GetSearchClient().Count<Member>(s => s
                .Index(indexname)
                .Type(typename)
                .Query(wildcardQuery)
            );
            count = (int)counts.Count;
            return count;
        }

        public int GetCountByPrefix(string keyword, string field)
        {
            int count = 0;
            string indexname = ConfigurationManager.AppSettings["CRMIndex"].ToString();
            string typename = ConfigurationManager.AppSettings["CRMMemberType"].ToString();
            if (string.IsNullOrEmpty(field))
            { field = "_all"; }
            else { field = field.ToLower(); }
            QueryContainer prefixQuery = new PrefixQuery() { Field = field, Value = keyword };
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
