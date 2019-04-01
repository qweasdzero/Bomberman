using System.Collections.Generic;
using GameFramework;
using GameFramework.Photon;

namespace SG1
{
    public abstract class Request : NetworkMessage
    {
        protected Dictionary<byte,object> m_CustomOpParameters = new Dictionary<byte, object>();

        public Dictionary<byte, object> CustomOpParameters
        {
            get
            {
                return m_CustomOpParameters;
            }
        }
        
        public override NetworkType NetworkType
        {
            get
            {
                return NetworkType.Request;
            }
        }

        /// <summary>
        /// 开始发送
        /// </summary>
        /// <param name="photonChannel"></param>
        /// <returns></returns>
        public virtual bool Send(IPhotonChannel photonChannel)
        {
            return photonChannel.SendRequset(Id, m_CustomOpParameters, true);
        }
    }
}