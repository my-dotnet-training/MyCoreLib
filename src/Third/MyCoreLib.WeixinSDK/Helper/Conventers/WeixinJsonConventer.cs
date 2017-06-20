using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web.Script.Serialization;
using MyCoreLib.WeixinSDK.Annotations;
using MyCoreLib.WeixinSDK.Entiyies;

namespace MyCoreLib.WeixinSDK.Helper.Conventers
{
    public class WeixinJsonConventer : JavaScriptConverter
    {
        private readonly JsonSetting _jsonSetting;
        private readonly Type _type;
        public WeixinJsonConventer(Type type, JsonSetting jsonSetting = null)
        {
            this._jsonSetting = jsonSetting ?? new JsonSetting();
            this._type = type;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                var typeList = new List<Type>(new[] { typeof(IJsonIgnoreNull)/*,typeof(JsonIgnoreNull)*/ });

                if (_jsonSetting.TypesToIgnore.Count > 0)
                {
                    typeList.AddRange(_jsonSetting.TypesToIgnore);
                }

                if (_jsonSetting.IgnoreNulls)
                {
                    typeList.Add(_type);
                }

                return new ReadOnlyCollection<Type>(typeList);
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            if (obj == null)
            {
                return result;
            }

            var properties = obj.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                //排除的属性
                bool excludedProp = propertyInfo.IsDefined(typeof(ExcludedAttribute), true);
                if (excludedProp)
                    result.Add(propertyInfo.Name, propertyInfo.GetValue(obj, null));
                else
                {
                    if (!this._jsonSetting.PropertiesToIgnore.Contains(propertyInfo.Name))
                    {
                        bool ignoreProp = propertyInfo.IsDefined(typeof(ScriptIgnoreAttribute), true);
                        if ((this._jsonSetting.IgnoreNulls || ignoreProp) && propertyInfo.GetValue(obj, null) == null)
                        {
                            continue;
                        }

                        //当值匹配时需要忽略的属性
                        IgnoreValueAttribute attri = propertyInfo.GetCustomAttribute<IgnoreValueAttribute>();
                        if (attri != null && attri.Value.Equals(propertyInfo.GetValue(obj)))
                        {
                            continue;
                        }

                        EnumStringAttribute enumStringAttri = propertyInfo.GetCustomAttribute<EnumStringAttribute>();
                        if (enumStringAttri != null)
                        {   //枚举类型显示字符串
                            result.Add(propertyInfo.Name, propertyInfo.GetValue(obj).ToString());
                        }
                        else
                            result.Add(propertyInfo.Name, propertyInfo.GetValue(obj, null));
                    }
                }
            }
            return result;
        }

    }
}
