
namespace MyCoreLib.BaseWeb.Models
{
    using MyCoreLib.Common.Model;

    /// <summary>
    /// The generic json response.
    /// </summary>
    public class PagingResponseModel : JsonResponseModel
    {
        /// <summary>
        /// The requested data offset.
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        /// The requested page size.
        /// </summary>
        public int page_size { get; set; }
    }
}
