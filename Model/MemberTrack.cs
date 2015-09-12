using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MemberTrack
    {
        private string _id;
        private string _memberID;
        private string _nextContactDate;
        private string _nextAdminID;
        private string _memo;

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string MemberID
        {
            get
            {
                return _memberID;
            }

            set
            {
                _memberID = value;
            }
        }

        public string NextContactDate
        {
            get
            {
                return _nextContactDate;
            }

            set
            {
                _nextContactDate = value;
            }
        }

        public string NextAdminID
        {
            get
            {
                return _nextAdminID;
            }

            set
            {
                _nextAdminID = value;
            }
        }

        public string Memo
        {
            get
            {
                return _memo;
            }

            set
            {
                _memo = value;
            }
        }
    }
}
