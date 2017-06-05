
namespace MyCoreLib.BaseLog.FileLog
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Support rollover log files.
    /// </summary>
    public class RolloverTraceListener : TextWriterTraceListener
    {
        private string _directory;
        private string _prefix;
        private FileStream _stream;
        private int _maxSizeInBytes;
        private bool _autoFlush;
        private readonly object _locker = new object();

        /// <summary>
        /// ctor.
        /// </summary>
        public RolloverTraceListener(
            string directory,
            string prefix,
            int maxSizeInBytes = 4 * 1024 * 1024,  // 4M by default
            bool autoFlush = false)
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentNullException("directory");
            if (string.IsNullOrWhiteSpace(prefix)) throw new ArgumentNullException("prefix");
            if (maxSizeInBytes <= 0) throw new ArgumentOutOfRangeException("maxSizeInBytes");
            if (prefix.IndexOfAny(new char[] { '/', '\\' }) >= 0)
                throw new ArgumentException(string.Format("Slashes are not allowed: {0}", prefix), "prefix");

            this._directory = directory;
            this._prefix = prefix;
            this._maxSizeInBytes = maxSizeInBytes;
            this._autoFlush = autoFlush;
        }

        public override void Write(string message)
        {
            EnsureWriter();
            base.Write(message);

            if (_autoFlush)
            {
                Flush();
            }
        }

        public override void WriteLine(string message)
        {
            EnsureWriter();
            base.WriteLine(message);

            if (_autoFlush)
            {
                Flush();
            }
        }

        /// <summary>
        /// Ensures that the log file is created, and decides if we need to create a new log file.
        /// </summary>
        private void EnsureWriter()
        {
            if (_stream == null || _stream.Position >= _maxSizeInBytes)
            {
                lock (_locker)
                {
                    if (_stream != null)
                    {
                        Writer.Dispose();
                        _stream = null;
                    }

                    // Need to create a new log file.
                    DateTime date = DateTime.Now.Date;
                    int max = GetMaxSeriaNo(date);
                    string logFileName = string.Format("{0}_{1:yyyyMMdd}_{2:00}.log", _prefix, date, max);
                    string logFullPath = Path.Combine(_directory, logFileName);

                    _stream = new FileStream(logFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    Writer = new StreamWriter(_stream, Encoding.UTF8);
                }
            }
        }

        private int GetMaxSeriaNo(DateTime date)
        {
            // Log file name will use the format: prefix_yyMMdd_01.log
            string pattern = string.Concat("^", Regex.Escape(_prefix), string.Format("_{0:yyyyMMdd}", date), @"_(\d+)\.log$");
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            DirectoryInfo dirInfo = new DirectoryInfo(_directory);
            int max = 0;
            if (dirInfo.Exists)
            {
                foreach (FileInfo f in dirInfo.GetFiles("*.log", SearchOption.TopDirectoryOnly))
                {
                    Match m = regex.Match(f.Name);
                    if (m.Success)
                    {
                        max = Math.Max(max, int.Parse(m.Groups[1].Value));
                    }
                }
            }

            return max + 1;
        }
    }
}
