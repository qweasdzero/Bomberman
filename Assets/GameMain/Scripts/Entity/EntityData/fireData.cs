using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG1
{   
    public class fireData : EntityData
    {
        public float HideTimer=0.5f;
        public fireData(int entityId, int typeId) : base(entityId, typeId)
        {
            
        }
    }
}

