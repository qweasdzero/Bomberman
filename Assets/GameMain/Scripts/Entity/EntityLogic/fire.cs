using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Debug = System.Diagnostics.Debug;

namespace SG1
{
    public class fire : Entity
    {
        public fireData m_data;
        public List<RaycastHit2D> HIT=new List<RaycastHit2D>();
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_data = (fireData) userData;
            CachedTransform.localScale=new Vector2(0.9f,0.9f);
            for (int i = 0; i < 4; i++)
            {
                HIT.Add(new RaycastHit2D());
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_data.HideTimer -= elapseSeconds;
            for (int i = 0; i < HIT.Count; i++)
            {
                HIT[i] = new RaycastHit2D();
            }
            HIT[0]= Physics2D.Raycast(CachedTransform.position, Vector2.up,0.45f);
            HIT[1]= Physics2D.Raycast(CachedTransform.position, Vector2.down,0.45f);
            HIT[2]= Physics2D.Raycast(CachedTransform.position, Vector2.left,0.45f);
            HIT[3]= Physics2D.Raycast(CachedTransform.position, Vector2.right,0.45f);
            for (int i = 0; i < HIT.Count; i++)
            {
                if (HIT[i] && HIT[i].collider.GetComponent<TargetableObject>())
                {
                    HIT[i].collider.GetComponent<TargetableObject>().OnHurt();
                }
            }
            if (m_data.HideTimer <= 0)
            {
                GameEntry.Entity.HideEntity(this);
            }
        }
    }
}

