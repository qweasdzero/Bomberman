using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetableObject : Entity
    {
        public virtual TargetableObjectData Data { get; }

        public virtual void OnHurt()
        {
            Data.HP -= 1;
        }
    }
}
