using Common;
using GameFramework.Photon;

namespace SG1
{
    public sealed class RegisteredRequest : Request
    {
        private string m_UserName = string.Empty;
        
        private string m_Password = string.Empty;

        public string UserName
        {
            get
            {
                return m_UserName;
            }
        }

        public string Password
        {
            get
            {
                return m_Password;
            }
        }

        public override byte Id
        {
            get
            {
                return (byte) OperationCode.Register;
            }
        }
        
        public override void Clear()
        {
            m_UserName = default(string);
            m_Password = default(string);
            m_CustomOpParameters.Clear();
        }

        public RegisteredRequest Fill(string userName,string password)
        {
            m_UserName = userName;
            m_Password = password;
            m_CustomOpParameters.Add((byte)ParameterCode.Username,m_UserName);
            m_CustomOpParameters.Add((byte) ParameterCode.Password, m_Password);
            return this;
        }
    }
}