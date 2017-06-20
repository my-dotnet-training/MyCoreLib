using System.Collections.Generic;
using System.Net.Http;
using MyCoreLib.WeixinSDK.Apis._01World;
using MyCoreLib.WeixinSDK.Apis.MP;
using MyCoreLib.WeixinSDK.Entiyies;
using MyCoreLib.WeixinSDK.Enums;
using MyCoreLib.WeixinSDK.Helper;
using MyCoreLib.WeixinSDK.Interface;

namespace MyCoreLib.WeixinSDK.Implement
{
    /// <summary>
    /// 只做了文本消息,Event处理
    /// </summary>
    public class WeixinExecutor : IWeixinExecutor
    {
        public WeixinExecutor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns>已经打包成xml的用于回复用户的消息包</returns>
        public string Execute(WeixinMessage message)
        {
            string result = string.Empty;
            string openId = message.Body.FromUserName.Value;
            string myUserName = message.Body.ToUserName.Value;
            string unionId = AdminAPI.GetUnionId(GlobalContext.AccessToken, openId);
            switch (message.Type)
            {
                //文字消息
                case WeixinMessageType.Text:
                    string userMessage = message.Body.Content.Value;
                    result = ReplayPassiveMessageAPI.RepayText(openId, myUserName, string.Format("欢迎使用，[openid:{0};\nunionid:{1}]\n输入了：" + userMessage, openId, unionId));
                    break;
                //事件处理
                case WeixinMessageType.Event:
                    DealWithEvent(message, openId, unionId, myUserName, out result);
                    break;
                default:
                    result = ReplayPassiveMessageAPI.RepayText(openId, myUserName, string.Format("未处理消息类型:{0}", message.Type));
                    break;
            }
            return result;
        }

        private void DealWithEvent(WeixinMessage message, string openId, string unionId, string myUserName, out string result)
        {
            string eventType = message.Body.Event.Value.ToLower();
            string eventKey = string.Empty;
            try
            {
                eventKey = message.Body.EventKey.Value;
            }
            catch { }
            switch (eventType)
            {
                case "subscribe"://用户未关注时，进行关注后的事件推送
                    #region 首次关注

                    //TODO: 获取用户基本信息后，将用户信息存储在本地。
                    //var token = WeixinConfig.TokenHelper.GetToken();
                    //var weixinInfo = UserAdminAPI.GetInfo(token, openId);//注意：订阅号没有此权限

                    if (!string.IsNullOrEmpty(eventKey))
                    {
                        var qrscene = eventKey.Replace("qrscene_", "");//此为场景二维码的场景值
                        result = ReplayPassiveMessageAPI.RepayNews(openId, myUserName,
                            new WeixinNews
                            {
                                title = "欢迎订阅01World，场景值：" + qrscene,
                                description = "欢迎订阅01World，场景值：" + qrscene,
                                picurl = string.Format("{0}/resource/Content/images/usericof.png", GlobalContext.Domain),
                                url = string.Format("{0}?unionId={1}", GlobalContext.Domain, unionId)
                            });
                    }
                    else
                    {
                        result = ReplayPassiveMessageAPI.RepayNews(openId, myUserName,
                         new WeixinNews
                         {
                             title = "欢迎订阅01World",
                             description = "欢迎订阅01World",
                             picurl = string.Format("{0}/resource/Content/images/usericof.png", GlobalContext.Domain),
                             url = string.Format("{0}?unionId={1}", GlobalContext.Domain, unionId)
                         });
                    }
                    #endregion
                    break;
                case "unsubscribe"://取消关注
                    #region 取消关注
                    result = ReplayPassiveMessageAPI.RepayText(openId, myUserName, "欢迎再来");
                    #endregion
                    break;
                case "click"://自定义菜单事件
                    #region 自定义菜单事件
                    {
                        switch (eventKey)
                        {
                            case "V1001_Hello":
                                #region Hello
                                result = ReplayPassiveMessageAPI.RepayNews(openId, myUserName, new List<WeixinNews>()
                                    {
                                        new WeixinNews{
                                            title="欢迎关注本公众号",
                                            url=string.Format("{0}?unionId={1}", GlobalContext.Domain,unionId),
                                            description="点击进入01world",
                                            picurl=string.Format("{0}/resource/Content/images/usericof.png", GlobalContext.Domain)
                                        },
                                    });
                                #endregion
                                break;
                            case "V1001_My01":
                                #region 我的01
                                result = ReplayPassiveMessageAPI.RepayNews(openId, myUserName, new List<WeixinNews>()
                                    {
                                        new WeixinNews{
                                            title="我的01",
                                            url=string.Format("{0}/My01/My01?unionId={1}", GlobalContext.Domain,unionId),
                                            description= "我的余额:"+GetYE()+";\n点击查看帐户详情",
                                            picurl=string.Format("{0}/resource/Content/images/usericof.png", GlobalContext.Domain)
                                        },
                                    });
                                #endregion
                                break;
                            case "V1001_Binding":
                                #region 绑定账号
                                result = ReplayPassiveMessageAPI.RepayNews(openId, myUserName, new List<WeixinNews>()
                                    {
                                        new WeixinNews{
                                            title="绑定01world账号",
                                            url=string.Format("{0}/User/WeixinBind?unionId={1}", GlobalContext.Domain,unionId),
                                            description="点击绑定01world账号",
                                            picurl=string.Format("{0}/resource/Content/images/usericof.png", GlobalContext.Domain)
                                        },
                                    });
                                #endregion
                                break;
                            default:
                                result = ReplayPassiveMessageAPI.RepayText(openId, myUserName, "没有响应菜单事件");
                                break;
                        }
                    }
                    #endregion
                    break;
                case "view"://点击菜单跳转链接时的事件推送
                    #region 点击菜单跳转链接时的事件推送
                    result = ReplayPassiveMessageAPI.RepayText(openId, myUserName, string.Format("您将跳转至：{0}", eventKey));
                    #endregion
                    break;
                default:
                    result = ReplayPassiveMessageAPI.RepayText(openId, myUserName, "没有响应菜单事件");
                    break;
            }
        }

        private double GetYE()
        {
            return 0;
        }
    }
}
