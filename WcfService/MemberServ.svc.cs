using DAL.Implement;
using DAL.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MemberServ”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MemberServ.svc 或 MemberServ.svc.cs，然后开始调试。
    public class MemberServ : IMemberServ
    {
        private IMemberDal _dal = new MemberDal();
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type">类型：Enterprise,Products,Memo,Address,AdminName,WebName</param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<Member> GetList(string keyword, string type, int start, int size)
        {
            return _dal.GetList(keyword, type, start, size);
        }

        public List<Member> GetListByPrefix(string keyword, string type, int start, int size)
        {
            return _dal.GetList(keyword, type, start, size);
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="type">类型：Enterprise,Products,Memo,Address,AdminName,WebName</param>
        /// <returns></returns>
        public int GetCount(string keyword, string type)
        {
            return _dal.GetCount(keyword, type);
        }

        public int GetCountByPrefix(string keyword, string type)
        {
            return _dal.GetCountByPrefix(keyword, type);
        }
    }
}
