using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG1
{
    public class WoodWall : Wall
    {
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            tag = "WoodWall";
        }
    }

}

