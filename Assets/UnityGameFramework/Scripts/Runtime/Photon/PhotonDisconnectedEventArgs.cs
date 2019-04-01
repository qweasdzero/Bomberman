using GameFramework.Event;
using GameFramework.Photon;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// PhotonDisconnected 事件。
    /// </summary>
    public sealed class PhotonDisconnectedEventArgs : GameEventArgs
    {
        /// <summary>
        /// PhotonDisconnected 事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PhotonDisconnectedEventArgs).GetHashCode();
        
        /// <summary>
        /// 获取 PhotonDisconnected 事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        
        /// <summary>
        /// 获取Photon频道。
        /// </summary>
        public IPhotonChannel PhotonChannel
        {
            get; 
            private set;
        }
        
        /// <summary>
        /// 清理 PhotonDisconnected 事件。
        /// </summary>
        public override void Clear()
        {
            PhotonChannel = default(IPhotonChannel);
        }
        
        /// <summary>
        /// 填充 PhotonDisconnected 事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>PhotonDisconnected 事件。</returns>
        public PhotonDisconnectedEventArgs Fill(GameFramework.Photon.PhotonStatusChangedEventArgs e)
        {
            PhotonChannel = e.PhotonChannel;

            return this;
        }
    }
}
