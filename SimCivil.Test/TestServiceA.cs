using System;
using System.Text;

using SimCivil.Rpc.Session;

namespace SimCivil.Test {
    public class TestServiceA : ITestServiceA
    {
        public IRpcSession Session { get; }
        public string Name { get; set; }

        public TestServiceA(IRpcSession session)
        {
            Session = session;
        }

        public string GetName()
        {
            return Name;
        }

        public string HelloWorld(string name)
        {
            Name = name;

            return $"Hello {name}!";
        }

        public int NotImplementedFuc(int i)
        {
            throw new NotImplementedException();
        }

        public string GetSession(string key)
        {
            return Session[key].ToString();
        }

        public void Echo(string str, Action<string> callback)
        {
            callback(str);
        }
    }
}