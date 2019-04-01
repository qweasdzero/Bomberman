using ExitGames.Client.Photon;
using GameFramework.Event;
using GameFramework.Photon;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// PhotonEvent 事件。
    /// </summary>
    public sealed class PhotonEventEventArgs : GameEventArgs
    {
        /// <summary>
        /// PhotonEvent 事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PhotonEventEventArgs).GetHashCode();

        /// <summary>
        /// 获取 PhotonEvent 事件编号。
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
        /// 获取Photon 事件数据。
        /// </summary>
        public EventData EventData
        {
            get;
            private set;
        }
        
        
        public override void Clear()
        {
            PhotonChannel = default(IPhotonChannel);
            EventData = default(EventData);
        }
        
        /// <summary>
        /// 填充 PhotonEvent 事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>PhotonEvent 事件。</returns>
        public PhotonEventEventArgs Fill(GameFramework.Photon.PhotonEventEventArgs e)
        {
            PhotonChannel = e.PhotonChannel;
            EventData = e.EventData;
            
            return this;
        }
    }
}