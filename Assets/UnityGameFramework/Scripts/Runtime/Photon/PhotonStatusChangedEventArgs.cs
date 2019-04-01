using ExitGames.Client.Photon;
using GameFramework.Event;
using GameFramework.Photon;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// PhotonStatusChanged 事件。
    /// </summary>
    public sealed class PhotonStatusChangedEventArgs : GameEventArgs
    {
        /// <summary>
        /// PhotonStatusChanged 事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PhotonStatusChangedEventArgs).GetHashCode();

        /// <summary>
        /// 获取 PhotonStatusChanged 事件编号。
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
        /// 获取 Photon 连接状态。
        /// </summary>
        public StatusCode StatusCode
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 清理 PhotonStatusChanged 事件。
        /// </summary>
        public override void Clear()
        {
            PhotonChannel = default(IPhotonChannel);
            StatusCode = default(StatusCode);
        }
        
        /// <summary>
        /// 填充 PhotonStatusChanged 事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>PhotonStatusChanged 事件。</returns>
        public PhotonStatusChangedEventArgs Fill(GameFramework.Photon.PhotonStatusChangedEventArgs e)
        {
            PhotonChannel = e.PhotonChannel;
            StatusCode = e.StatusCode;
            
            return this;
        }
    }
}