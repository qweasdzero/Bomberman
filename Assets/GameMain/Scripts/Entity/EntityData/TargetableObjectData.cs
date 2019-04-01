using System;
using UnityEngine;

namespace SG1
{
    [Serializable]
    public abstract class TargetableObjectData : EntityData
    {
        [SerializeField]
        private int m_HP ;
        public float Speed=3;
        public TargetableObjectData(int entityId, int typeId)
            : base(entityId, typeId)
        {
            m_HP = 3;
        }
        /// <summary>
        /// 当前生命。
        /// </summary>
        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = value;
            }
        }
    }
}
