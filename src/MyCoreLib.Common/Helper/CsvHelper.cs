using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyCoreLib.Common.Helper
{
    /// <summary>
    /// csv file opration
    /// </summary>
    public static class CsvHelper
    {
        private static IList<FieldInfo> EntityHeaderToCsv<T>(T data, bool includeHeader = true, TextWriter write = null)
        {
            var _fields = data.GetFieldInfos(BindingFlags.Public | BindingFlags.Instance);
            // write header
            if (includeHeader)
            {
                StringBuilder _headerStr = new StringBuilder();
                bool _first = true;
                foreach (var field in _fields)
                {
                    if (_first)
                        _first = false;
                    else
                        _headerStr.Append(",");
                    _headerStr.Append(field.Name);
                }
                write.WriteLine(_headerStr.ToString());
                write.Flush();
            }
            return _fields;
        }
        public static bool EnumeratorToCsv<T>(IEnumerable<T> data, string strFilePath)
        {
            if (string.IsNullOrWhiteSpace(strFilePath))
                throw new ArgumentNullException("strFilePath");

            var _stream = File.OpenWrite(strFilePath);
            var _write = new StreamWriter(_stream, Encoding.UTF8);

            bool _first = false;
            foreach (var item in data)
            {
                if (!_first)
                {
                    if (!EntityToCsv(item, true, _write))
                        return false;
                    _first = true;
                }
                else if (!EntityToCsv(item, false, _write))
                    return false;
            }
            _write.Flush();
            _write.Dispose();
            _stream.Dispose();
            return true;
        }
        public static bool EntityToCsv<T>(T data, bool includeHeader = true, TextWriter write = null)
        {
            IList<FieldInfo> _header = EntityHeaderToCsv(data, includeHeader, write);
            if (_header.Count == 0)
                return false;
            // write rows
            StringBuilder _rowStr = new StringBuilder();
            bool _first = true;
            foreach (var field in _header)
            {
                if (_first)
                    _first = false;
                else
                    _rowStr.Append(",");
                _rowStr.Append(field.GetValue(data));
            }
            write.WriteLine(_rowStr);
            write.Flush();
            return true;
        }

        public static IEnumerable<T> CsvToEnumerator<T>(string filePath)
        {
            FileStream _stream = File.OpenRead(filePath);
            var _reader = new StreamReader(_stream, Encoding.UTF8);

            T _entity;
            List<T> _list = new List<T>();
            IList<FieldInfo> _header = null;
            bool _first = true;
            string _line = _reader.ReadLine();
            while (!string.IsNullOrWhiteSpace(_line))
            {
                _entity = default(T);
                if (_first)
                {
                    _header = _entity.GetFieldInfos(BindingFlags.Public);
                    _first = false;
                }
                else if (CsvLineToEntity(_line, _header, out _entity))
                    _list.Add(_entity);

                _line = _reader.ReadLine();
            }
            return _list;
        }
        private static bool CsvLineToEntity<T>(string line, IList<FieldInfo> _header, out T entity)
        {
            entity = default(T);
            var _values = line.Split(',');
            try
            {
                for (int i = 0; i < _header.Count; i++)
                {
                    _header[i].SetValue(entity, _values[i]);
                }
                return true;
            }
            catch (Exception ee)
            {

            }
            return false;
        }
    }
}
