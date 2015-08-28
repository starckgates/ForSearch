using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class Member
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
       
        private string _adminName;
        public string AdminName
        {
            get { return _adminName; }
            set { _adminName = value; }
        }
        
        private string _version;
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        
        private string _webName;
        public string WebName
        {
            get { return _webName; }
            set { _webName = value; }
        }
        
        private string _enterprise;
        public string Enterprise
        {
            get { return _enterprise; }
            set { _enterprise = value; }
        }
       
        private string _products;
        public string Products
        {
            get { return _products; }
            set { _products = value; }
        }
        
        private string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }
}
