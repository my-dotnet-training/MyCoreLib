using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BasePush
{
    /// <summary>
    /// Indicates the type of a message item.
    /// </summary>
    public enum MessageType
    {
        UserRechargeOrder,
        UserActivity
    }

    public class MessageItem<T>
    {
        /// <summary>
        /// Gets or sets the type of the message item.
        /// </summary>
        public MessageType Type { get; set; }

        public T OrigMessage { get; set; }
        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public string Body { get; set; }
    }
}
