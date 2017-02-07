using System.IO;

namespace MyCoreLib.BaseWeb.Utils
{
    public class HttpObjectInfo
    {
        public int ContentLength { get; internal set; }
        public string ContentType { get; internal set; }
        public Stream Content { get; internal set; }
    }
}
