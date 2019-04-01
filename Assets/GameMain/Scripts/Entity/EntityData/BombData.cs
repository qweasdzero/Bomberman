using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG1
{   
    public class BombData : EntityData
    {
        public Player Master;
        public float boomtimer=3f;
        public BombData(int entityId, int typeId,Player master) : base(entityId, typeId)
        {
            Master = master;
        }
    }
}

