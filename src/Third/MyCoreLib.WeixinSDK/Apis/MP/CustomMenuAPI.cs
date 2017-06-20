using MyCoreLib.WeixinSDK.Helper;

namespace MyCoreLib.WeixinSDK.Apis.MP
{
    public class CustomMenuAPI : BaseAPI
    {
        /// <summary>
        /// 自定义菜单创建接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static bool Create(string accessToekn, string jsonData)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.CustomMenuCreate, accessToekn);
            var result = PostJsonAsync(url, jsonData, Enums.ContentType.String).errcode;
            return result == 0;
        }

        /// <summary>
        /// 自定义菜单查询接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static dynamic Query(string accesstoken)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.CustomMenuQuery, accesstoken);
            return GetJsonAsync(url);
        }

        /// <summary>
        /// 自定义菜单删除接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool Delete(string accesstoken)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.CustomMenuDelete, accesstoken);
            var result = GetJsonAsync(url);
            return result.errmsg == "ok";
        }
    }
}
