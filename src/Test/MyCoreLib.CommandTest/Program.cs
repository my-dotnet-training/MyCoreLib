using MyCoreLib.BaseLog;
using MyCoreLib.BaseMail;
using MyCoreLib.Common.Helper;
using MyCoreLib.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCoreLib.CommandTest
{
    public class Teach
    {
        public int Id;
        public string Name;
        public int Age;

    }

    public class TeachCollection : BaseClassIndexable<Teach>
    {
        public List<Teach> Teachs;
        public override Teach CallByIndex(int index)
        {
            return Teachs[index];
        }

        public override Teach CallByKey(string key)
        {
            return Teachs.Find(t => t.Name.Equals(key));
        }

        public override void SetByIndex(int index, Teach value)
        {
            Teachs[index] = value;
        }

        public override void SetByKey(string key, Teach value)
        {
            var _t = Teachs.Find(t => t.Name.Equals(key));
            var _index = -1;
            if (_t != null && (_index = Teachs.IndexOf(_t)) >= 0)
                Teachs[_index] = value;
        }

        public void Demo()
        {
            IMailProvider _mail = new MailKitProviderDemo().MailKitProvider();
            MailEntity _mailEntity = new MailEntity()
            {
                No = "0001",
                Title = "Demo",
                Subtype = "Demo sub",
                FromName = "gmail",
                FromEmail = "zjlbyron@gmail.com",
                ToName = "163",
                ToEmail = "zjl9494@163.com",
                Body = "Hello Mail Kit"
            };
            MailSetting _setting = new MailSetting()
            {
                Host = "smtp.gmail.com",
                Port = 25,
                UseSsl = false,
                User = "zjlbyron@gmail.com",
                Password = "1987629"
            };
            _mail.Init(_setting);
            _mail.Send(_mailEntity);

            ILoggerProvider _logf = new LogProviderDemo().SystemDBLog();
            ILog _log = _logf.GetLog("Demo");
            _log.Debug("Hello");

            Teach _teach1 = new Teach { Id = 1, Name = "a", Age = 25 };
            Teach _teach2 = new Teach { Id = 2, Name = "b", Age = 35 };
            Tuple<int, string, int> teach = new Tuple<int, string, int>(3, "c", 45);

            //if (CsvHelper.EnumeratorToCsv<Teach>(new Teach[] { _teach1, _teach2 }, @"G:\share\csv.text"))
            //    Console.WriteLine("Entity to csv file success");
            //else
            //    Console.WriteLine("Entity to csv file faild");

            TeachCollection _col = new TeachCollection();
            _col.Teachs = new List<Teach>();
            _col.Teachs.Add(_teach1);
            _col.Teachs.Add(_teach2);
            Console.WriteLine(_col[0].Name + "--" + _col["b"].Age);

            Console.ReadKey();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //new AcrossRiver<int>().Across();
            //Console.WriteLine(MathHelper.Sum<int>(array =>
            //{
            //    int _result = 0;
            //    foreach (var p in array)
            //        _result += p;
            //    return _result;
            //}, new int[] { 1, 2 }));
            Console.ReadKey();
        }
    }
}