namespace SG1
{
    public abstract class Response : NetworkMessage
    {
        public override NetworkType NetworkType
        {
            get
            {
                return NetworkType.Response;
            }
        }
    }
}