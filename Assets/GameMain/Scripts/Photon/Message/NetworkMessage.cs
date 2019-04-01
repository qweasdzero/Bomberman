using GameFramework;

namespace SG1
{
    public abstract class NetworkMessage : IReference
    {
        public abstract NetworkType NetworkType
        {
            get;
        }

        public abstract byte Id
        {
            get;
        }

        public abstract void Clear();
    }
}