
namespace MyCoreLib.BaseWeb.Models
{
    /// <summary>
    /// The generic json response.
    /// </summary>
    public class FullPagingResponseModel : PagingResponseModel
    {
        /// <summary>
        /// Get or set the total number of records.
        /// </summary>
        public int total_count { get; set; }
    }
}
