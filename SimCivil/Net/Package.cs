using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net
{
    class Package
    {
        Head head;
        string s;

        public string S
        {
            get { return s; }
            set { s = value; }
        }

        public Head Head
        {
            get { return head; }
            set { head = value; }
        }

        public Package(string s) : this(new Head(0), s) { }

        public Package(Head head, string s)
        {
            this.head = head;
            this.s = s;
        }
    }

    struct Head
    {
        public int packageID;
        public int typeID;
        public int length;

        public Head(int typeID) : this(0, typeID, 0) { }

        public Head(int packageID, int typeID) : this(packageID, typeID, 0) { }

        public Head(int packageID, int typeID, int length)
        {
            this.packageID = packageID;
            this.length = length;
            this.typeID = typeID;
        }
    }
}
