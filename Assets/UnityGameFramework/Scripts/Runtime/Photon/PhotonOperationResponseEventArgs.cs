using ExitGames.Client.Photon;
using GameFramework.Event;
using GameFramework.Photon;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// PhotonOperationResponse 事件。
    /// </summary>
    public sealed class PhotonOperationResponseEventArgs : GameEventArgs
    {
        /// <summary>
        /// PhotonOperationResponse 事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PhotonOperationResponseEventArgs).GetHashCode();

        /// <summary>
        /// 获取 PhotonOperationResponse 事件编号。
        /// </summary>
        public override int Id
        {
            get { return EventId; }
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
        /// 获取 Photon 操作返回数据。
        /// </summary>
        public OperationResponse OperationResponse
        {
            get;
            private set;
        }
        
        public override void Clear()
        {
            PhotonChannel = default(IPhotonChannel);
            OperationResponse = default(OperationResponse);
        }
        
        /// <summary>
        /// 填充 PhotonOperationResponse 事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>PhotonOperationResponse 事件。</returns>
        public PhotonOperationResponseEventArgs Fill(GameFramework.Photon.PhotonOperationResponseEventArgs e)
        {
            PhotonChannel = e.PhotonChannel;
            OperationResponse = e.OperationResponse;
            
            return this;
        }
    }
}