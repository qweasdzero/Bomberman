using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Debug = System.Diagnostics.Debug;

namespace SG1
{
    public class Wall : Entity
    {
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            CachedTransform.localScale=new Vector2(3.125f,3.125f);
        }
        
        
    }

}

