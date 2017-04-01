
namespace MyCore.Model
{
    public class GenericResponseModel
    {
        public const int OK = 0;
        public const int SystemBusy = 1;
        public const int SystemMaintain = 2;
        public const int EntityNotExist = 3;
        public const int AccessDenied = 4;
        public const int UnknownError = 9999;

        /// <summary>
        /// 0 for a successful operation.
        /// </summary>
        public int errcode { get; set; }
        public string errmsg { get; set; }

        public void SetError(int errorCode, string errorMessage)
        {
            this.errcode = errorCode;
            this.errmsg = errorMessage;
        }
    }
}
