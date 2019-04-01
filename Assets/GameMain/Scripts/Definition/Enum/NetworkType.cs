namespace SG1
{
    public enum NetworkType
    {
        /// <summary>
        /// 客户端向服务器的请求
        /// </summary>
        Request,
        /// <summary>
        /// 服务端向客户端的响应
        /// </summary>
        Response,
        /// <summary>
        /// 服务器向客户端下发的事件
        /// </summary>
        Event,
    }
}