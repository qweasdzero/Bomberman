using GameFramework.Event;
using GameFramework.Photon;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// PhotonConnected 事件。
    /// </summary>
    public sealed class PhotonConnectedEventArgs : GameEventArgs
    {
        /// <summary>
        /// PhotonConnected 事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PhotonConnectedEventArgs).GetHashCode();
        
        /// <summary>
        /// 获取 PhotonConnected 事件编号。
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
        /// 清理 PhotonConnected 事件。
        /// </summary>
        public override void Clear()
        {
            PhotonChannel = default(IPhotonChannel);
        }
        
        /// <summary>
        /// 填充 PhotonConnected 事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>PhotonConnected 事件。</returns>
        public PhotonConnectedEventArgs Fill(GameFramework.Photon.PhotonStatusChangedEventArgs e)
        {
            PhotonChannel = e.PhotonChannel;

            return this;
        }
    }
}
