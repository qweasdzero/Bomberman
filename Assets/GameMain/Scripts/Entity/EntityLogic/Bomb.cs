using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class Bomb : Entity
    {
        public BoxCollider2D collider;
        public BombData m_data;
        private LayerMask _layerMask;
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_data = (BombData) userData;
            m_data.Master.m_data.bomblist.Add(this);
            CachedTransform.localScale=new Vector2(3.125f,3.125f);
            collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
            _layerMask = 1 << LayerMask.NameToLayer("Wall");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //TODO:如果玩家没有走出去，怪也可以穿过雷
            if (!collider.enabled&&Vector2.Distance(m_data.Master.CachedTransform.position, CachedTransform.transform.position) > 0.8f)
            {
                collider.enabled = true;
            }

            m_data.boomtimer -= elapseSeconds;
            if (m_data.boomtimer < 0)
            {
                m_data.Master.m_data.bomblist.Remove(this);
                GameEntry.Entity.HideEntity(this);
                Boom(Vector3.up);
                Boom(Vector3.down);
                Boom(Vector3.left);
                Boom(Vector3.right);
                //todo：生成爆炸
                GameEntry.Entity.Showfire(new fireData(GameEntry.Entity.GenerateSerialId(),5)
                {
                    Position = CachedTransform.position
                });
            }
        }

        private void Boom(Vector3 vector)
        {
            RaycastHit2D a= Physics2D.Raycast(CachedTransform.position, vector,0.6f,_layerMask);
            if (a)
            {
                if (a.collider.gameObject.CompareTag("WoodWall"))
                {
                    GameEntry.Entity.HideEntity(a.collider.GetComponent<WoodWall>());
                    return;
                }
                if(a.collider.gameObject.CompareTag("IronWall"))
                {
                    return;                    
                }
            }
            GameEntry.Entity.Showfire(new fireData(GameEntry.Entity.GenerateSerialId(),5)
            {
                Position = CachedTransform.position+vector
            });
            

        }
    }
}

