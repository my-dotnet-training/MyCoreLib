using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace MyCoreLib.BaseOrm.DbHelper
{
    public class DataReaderConverter<T> where T : new()
    {
        private class Mapping
        {
            public int Index;
            public PropertyInfo Property;
        }

        private Mapping[] _mappings;
        private DbDataReader _lastReader;

        public T Convert(DbDataReader reader)
        {
            if (_mappings == null || reader != _lastReader)
                _mappings = MapProperties(reader);

            var _obj = new T();
            foreach (var _mapping in _mappings)
            {
                var _prop = _mapping.Property;
                var _rawValue = reader.GetValue(_mapping.Index);
                var _value = DBConvert.To(_prop.PropertyType, _rawValue);
                _prop.SetValue(_obj, _value, null);
            }
            _lastReader = reader;
            return _obj;
        }

        private Mapping[] MapProperties(DbDataReader reader)
        {
            var _fieldCount = reader.FieldCount;
            var _fields = new Dictionary<string, int>(_fieldCount);
            for (var i = 0; i < _fieldCount; i++)
                _fields.Add(reader.GetName(i).ToLowerInvariant(), i);
            var _type = typeof(T);
            var _mappingList = new List<Mapping>(_fieldCount);
            foreach (var _prop in _type.GetProperties())
            {
                var _name = _prop.Name;
                int _index;
                if (_fields.TryGetValue(_name, out _index))
                    _mappingList.Add(new Mapping() { Index = _index, Property = _prop });
            }
            return _mappingList.ToArray();
        }
    }
}
