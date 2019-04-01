using GameFramework;
using GameFramework.Photon;

namespace SG1
{
    public static class PhotonExtension
    {
        public static bool Login(this IPhotonChannel photonChannel,string userName,string password)
        {
            return ReferencePool.Acquire<LoginRequest>().Fill(userName, password).Send(photonChannel);
        }
        
        public static bool Registered(this IPhotonChannel photonChannel,string userName,string password)
        {
            return ReferencePool.Acquire<RegisteredRequest>().Fill(userName, password).Send(photonChannel);
        }
    }
}