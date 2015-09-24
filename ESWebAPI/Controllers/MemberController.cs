using DAL.Implement;
using DAL.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ESWebAPI.Controllers
{
    [RoutePrefix("api/member")]
    public class MemberController : ApiController
    {
        private IMemberDal _dal = new MemberDal();
        #region SearchCondition 查询参数
        [ModelBinder]
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
        [HttpGet]
        [Route("getm/{a}/{b}/{c}" )]
        public SearchCondition getm(string a, string b, string c)
        {
            return new SearchCondition { flag = a, keyword = b, field = c };
        }

        [HttpGet]
        public HttpResponseMessage getme(SearchCondition model)
        {
            int em = 0;
            DAL.Implement.MemberDal.SearchCondition sc = new MemberDal.SearchCondition();
            sc.address = model.address;
            sc.aftersales = model.aftersales;
            sc.city = model.city;
            sc.classx = model.classx;
            sc.country = model.country;
            sc.field = model.field;
            sc.flag = model.flag;
            sc.keyword = model.keyword;
            sc.memo = model.memo;
            sc.province = model.province;
            sc.size = model.size;
            sc.start = model.start;
            sc.tracktype = model.tracktype;
            string str = _dal.GetList(sc, out em);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;

            //return _dal.GetList(model, out em);
        }
        [HttpGet]
        public HttpResponseMessage getCount(SearchCondition model)
        {
            DAL.Implement.MemberDal.SearchCondition sc = new MemberDal.SearchCondition();
            sc.address = model.address;
            sc.aftersales = model.aftersales;
            sc.city = model.city;
            sc.classx = model.classx;
            sc.country = model.country;
            sc.field = model.field;
            sc.flag = model.flag;
            sc.keyword = model.keyword;
            sc.memo = model.memo;
            sc.province = model.province;
            sc.size = model.size;
            sc.start = model.start;
            sc.tracktype = model.tracktype;
            int str = _dal.GetCount(sc);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str.ToString(), Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;

            //return _dal.GetList(model, out em);
        }

    }

}
