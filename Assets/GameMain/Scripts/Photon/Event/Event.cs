namespace SG1
{
    public abstract class Event : NetworkMessage
    {
        public override NetworkType NetworkType
        {
            get
            {
                return NetworkType.Event;
            }
        }
    }
}