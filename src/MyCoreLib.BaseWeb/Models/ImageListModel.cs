
namespace MyCoreLib.BaseWeb.Models
{
    public class ImageListModel
    {
        /// <summary>
        /// 指明是否所有的结果都已经返回； “true”表示本次没有返回全部结果；“false”表示本次已经返回了全部结果。
        /// </summary>
        public bool IsTruncated { get; set; }

        /// <summary>
        /// 标明这次Get Bucket（List Object）的起点。
        /// </summary>
        public string Marker { get; set; }

        /// <summary>
        /// Returns a list of images.
        /// </summary>
        public ImageItemModel[] Images { get; set; }
    }
}
