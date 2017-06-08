namespace MyCoreLib.BaseSocket.Base
{
    public interface IWorkItem : IStatusInfoSource, ISystemEndPoint
    {
        /// <summary>
        /// Transfers the system message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageData">The message data.</param>
        void TransferSystemMessage(string messageType, object messageData);
    }
}
