namespace MyCoreLib.BaseWeb.Models
{
    using MyCoreLib.Common.Model;

    /// <summary>
    /// The generic json response.
    /// </summary>
    public class GenericUIResponseModel : GenericResponseModel
    {
        public bool AllowDismissAlert { get; set; }

        public GenericUIResponseModel()
        {
            AllowDismissAlert = true;
        }
    }
}
