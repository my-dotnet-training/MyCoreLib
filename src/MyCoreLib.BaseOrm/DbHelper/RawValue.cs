
namespace MyCoreLib.BaseOrm.DbHelper
{
    public class RawValue
    {
        public RawValue(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }
    }
}
