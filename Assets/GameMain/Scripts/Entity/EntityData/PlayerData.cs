using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG1
{
    public class PlayerData : TargetableObjectData
    {
        public List<Bomb> bomblist=new List<Bomb>();
        public int MaxBomb=1;
        public PlayerData(int entityId, int typeId) : base(entityId, typeId)
        {
            
        }
    }
}

