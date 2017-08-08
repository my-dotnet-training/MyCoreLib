using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseCache.Memcached
{
    public class MemcachedService : ICacheService
    {
        private MemcachedClient memcachedClient;
        private string[] _servers;
        public MemcachedService()
        {
        }
        public bool Init(string servers = "192.168.154.142:11211")
        {
            try
            {
                _servers = servers.Split(',');
                var pool = SockIOPool.GetInstance();
                pool.InitConnections = 3;
                pool.MinConnections = 3;
                pool.MaxConnections = 5;
                pool.SocketConnectTimeout = 1000;
                pool.SocketTimeout = 3000;
                pool.MaintenanceSleep = 30;
                pool.Failover = true;
                pool.Nagle = true;
                pool.SetServers(servers.Split(','));

                pool.Initialize();

                memcachedClient = new MemcachedClient { EnableCompression = true };

                return true;
            }
            catch (Exception ee)
            {

                return false;
            }
        }
        public bool Set(string key, object value)
        {
            return memcachedClient.Set(key, value);
        }

        public bool Set(string key, object value, int ttl)
        {
            return memcachedClient.Set(key, value, DateTime.Now.AddMinutes(Convert.ToDouble(ttl)));
        }

        public bool Delete(string key)
        {
            return memcachedClient.Delete(key);
        }

        public bool Exists(string key)
        {
            return memcachedClient.KeyExists(key);
        }

        public T Get<T>(string key)
        {
            var t = memcachedClient.Get(key);
            if (t == null)
                return default(T);
            return (T)t;
        }

        public IDictionary<string, T> Gets<T>(string[] keys)
        {
            return (IDictionary<string, T>)memcachedClient.GetMultiple(keys);
        }

        #region 张剑龙 2016-05-12 获得所有缓存 用于清除缓存
        /// <summary>
        /// Memcached commadn 
        /// </summary>
        private const string STATS = "STAT";
        /// <summary>
        /// Memcached commadn 
        /// </summary>
        private const string END = "END";
        /// <summary>
        /// Memcached commadn 
        /// </summary>
        private const string ITEM = "ITEM";
        /// <summary>
        /// 执行 commadn 获得数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Hashtable Stats(string command)
        {
            ArrayList servers = new ArrayList(this._servers);
            SockIOPool pool = SockIOPool.GetInstance(memcachedClient.PoolName);
            if (pool == null)
                return null;

            if (servers == null || servers.Count == 0)
                servers = pool.Servers;

            if (servers == null || servers.Count <= 0)
                return null;

            // array of stats Hashtables
            Hashtable statsMaps = new Hashtable();
            for (int i = 0; i < servers.Count; i++)
            {
                string _cmd = command;
                SockIO sock = pool.GetConnection((string)servers[i]);
                if (sock == null)
                {
                    continue;
                }

                // build _cmd
                if (_cmd == null || _cmd.Length == 0)
                {
                    _cmd = "stats\r\n";
                }
                else
                {
                    _cmd = _cmd + "\r\n";
                }

                try
                {
                    sock.Write(UTF8Encoding.UTF8.GetBytes(_cmd));
                    sock.Flush();
                    Hashtable stats = new Hashtable();
                    while (true)
                    {
                        string line = sock.ReadLine();
                        if (line.StartsWith(STATS))
                        {
                            string[] info = line.Split(' ');
                            string key = info[1];
                            string val = info[2];
                            stats[key] = val;
                        }
                        else if (line.StartsWith(ITEM))
                        {

                            string[] info = line.Split('[');
                            string key = info[0].Split(' ')[1];
                            string val = "[" + info[1];
                            stats[key] = val;
                        }
                        else if (END == line)
                        {
                            break;
                        }
                        statsMaps[servers[i]] = stats;
                    }
                }
                catch (IOException e)
                {
                    try
                    {
                        sock.TrueClose();
                    }
                    catch (IOException)
                    {
                    }
                    sock = null;
                }

                if (sock != null)
                    sock.Close();
            }

            return statsMaps;
        }

        /// <summary>
        /// 获取服务器端缓存的数据信息 
        /// </summary>
        /// <param name="statsCommand"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<string> GetStats(MemcachedStats statsCommand, string param)
        {
            IList<string> statsArray = new List<string>();
            if (param == null)
                param = "";
            else
                param = param.Trim().ToLower();

            string commandstr = "stats";
            //转换stats命令参数
            switch (statsCommand)
            {
                case MemcachedStats.Reset: { commandstr = "stats reset"; break; }
                case MemcachedStats.Malloc: { commandstr = "stats malloc"; break; }
                case MemcachedStats.Maps: { commandstr = "stats maps"; break; }
                case MemcachedStats.Sizes: { commandstr = "stats sizes"; break; }
                case MemcachedStats.Slabs: { commandstr = "stats slabs"; break; }
                case MemcachedStats.Items: { commandstr = "stats items"; break; }//此处原先是返回stats
                case MemcachedStats.CachedDump:
                    {
                        string[] statsparams = param.Split(' ');
                        if (statsparams.Length == 2)
                            //if (param.IsIntArr(' '))
                            commandstr = "stats cachedump  " + param;
                        break;
                    }
                case MemcachedStats.Detail:
                    {
                        if (string.Equals(param, "on") || string.Equals(param, "off") || string.Equals(param, "dump"))
                            commandstr = "stats detail " + param.Trim();

                        break;
                    }
                default: { commandstr = "stats"; break; }
            }

            Hashtable stats = Stats(commandstr);

            foreach (string key in stats.Keys)
            {
                statsArray.Add("server:__:" + key);//此处也改了
                Hashtable values = (Hashtable)stats[key];
                foreach (string key2 in values.Keys)
                {
                    statsArray.Add(key2 + ":" + values[key2]);
                }
            }
            return statsArray;
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<CacheEntity> GetAllKeys()
        {
            IList<int> idList = new List<int>();
            IList<string> list = GetStats(MemcachedStats.Items, null);
            foreach (var item in list)
            {
                string[] tmpArr = item.Split(':');
                if (tmpArr.Length > 1)
                {
                    int itemID = 0;
                    if (tmpArr[1] == "__") continue;

                    int.TryParse(tmpArr[1], out itemID);
                    if (itemID <= 0) continue;

                    bool find = false;
                    foreach (int item1 in idList)
                    {
                        if (item1 == itemID)
                        {
                            find = true;
                            break;
                        }
                    }

                    if (!find)
                    {
                        idList.Add(itemID);
                    }
                }
            }

            List<CacheEntity> keys = new List<CacheEntity>();
            foreach (int item in idList)
            {
                IList<string> cachearr = GetStats(MemcachedStats.CachedDump, item + " 0");
                string _service = string.Empty;
                foreach (string itemCache in cachearr)
                {
                    string[] tmpArr = itemCache.Split(':');
                    if (tmpArr.Length > 1)
                    {
                        if (tmpArr[1] == "__")
                        {
                            _service = tmpArr[2];
                            continue;
                        }

                        keys.Add(new CacheEntity() { Key = tmpArr[0], Value = tmpArr[1], Service = _service });
                    }
                }
            }

            return keys;
        }
        /// <summary>
        /// 关闭service
        /// </summary>
        public void CloseService()
        {
            memcachedClient.FlushAll();
            SockIOPool.GetInstance(memcachedClient.PoolName).Shutdown();
        }

        #endregion
    }
}
