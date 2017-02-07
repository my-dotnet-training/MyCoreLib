
namespace MyCoreLib.BaseWeb.Models
{
    public class ImageItemModel
    {
        private string _imageName;

        /// <summary>
        /// Get or set the size of the image in bytes.
        /// </summary>
        public long Size { get; set; }

        public string Key { get; set; }

        /// <summary>
        /// Get the size of the image.
        /// </summary>
        public string ImageSize
        {
            get
            {
                const int KB = 1024;
                const double MB = KB * KB;
                if (Size < MB)
                {
                    return string.Format("{0} KB", (Size + KB - 1) / KB);
                }

                return string.Format("{0:0.0} MB", Size / MB);
            }
        }

        public string Path { get; set; }
        public string ImageName
        {
            get
            {
                if (string.IsNullOrEmpty(_imageName) && Path != null)
                {
                    return System.IO.Path.GetFileName(Path);
                }
                return _imageName;
            }
            set { _imageName = value; }
        }
    }
}
