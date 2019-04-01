using ExitGames.Client.Photon;
using GameFramework.Event;
using GameFramework.Photon;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// PhotonDebugReturn 事件。
    /// </summary>
    public sealed class PhotonDebugReturnEventArgs : GameEventArgs
    {
        /// <summary>
        /// PhotonDebugReturn 事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PhotonDebugReturnEventArgs).GetHashCode();
        
        /// <summary>
        /// 获取 PhotonDebugReturn 事件编号。
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
        /// 获取Photon DebugLevel。
        /// </summary>
        public DebugLevel DebugLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取Photon Debug 消息。
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// 清理 PhotonDebugReturn 事件。
        /// </summary>
        public override void Clear()
        {
            PhotonChannel = default(IPhotonChannel);
            DebugLevel = default(DebugLevel);
            Message = default(string);
        }

        /// <summary>
        /// 填充 PhotonDebugReturn 事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>PhotonDebugReturn 事件。</returns>
        public PhotonDebugReturnEventArgs Fill(GameFramework.Photon.PhotonDebugReturnEventArgs e)
        {
            PhotonChannel = e.PhotonChannel;
            DebugLevel = e.DebugLevel;
            Message = e.Message;
            
            return this;
        }
    }
}