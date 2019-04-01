using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SG1
{
    public enum PlayerState
    {
        Normal,//普通
        Invincible,//无敌
    }
    public class Player : TargetableObject
    {
        public PlayerData m_data;
        private readonly float VertexX=0.4f;
        private readonly float VertexY=0.35f;
        private readonly float RayLength = 0.5f;

        public PlayerState state=PlayerState.Normal;
        public int InvincibleTimeMax = 3;
        public float InvincibleTime;
        public DOTweenAnimation DoTweenAnimation;
        private LayerMask _layerMask;

        public override TargetableObjectData Data
        {
            get { return m_data; }
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_data = (PlayerData) userData;
            CachedTransform.localScale=new Vector2(1.6f,1.6f);
            InvincibleTime = InvincibleTimeMax;
            DoTweenAnimation=GetComponent<DOTweenAnimation>();           
            _layerMask = 1 << LayerMask.NameToLayer("Wall");
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (state == PlayerState.Invincible)
            {
                InvincibleTime -= elapseSeconds;
                if (InvincibleTime <= 0)
                {
                    DoTweenAnimation.DOPause();
                    CachedTransform.localScale=new Vector2(1.6f,1.6f);
                    state = PlayerState.Normal;
                    InvincibleTime = InvincibleTimeMax;
                    DoTweenAnimation.loops = InvincibleTimeMax;
                }
            }
            Move(elapseSeconds);
            if (Input.GetKey(KeyCode.Space))
            {
                if (m_data.bomblist.Count < m_data.MaxBomb)
                {
                    Vector2 pos = new Vector2(Mathf.RoundToInt(CachedTransform.position.x),
                        Mathf.RoundToInt(CachedTransform.position.y));
                    GameEntry.Entity.ShowBomb(new BombData(GameEntry.Entity.GenerateSerialId(),4,this)
                    {
                        Position = pos
                    });
                }               
            }
        }

        private void Move(float elapseSeconds)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            
            CachedAnimation.SetFloat("H",h);
            CachedAnimation.SetFloat("V",v);
            if (Math.Abs(h) > Math.Abs(v))
            {
                collider(Vector2.right, Mathf.Sign(h),CachedTransform.position.y%1,elapseSeconds);
            }
            else if(Math.Abs(v) > Math.Abs(h))
            {
                collider(Vector2.up, Mathf.Sign(v),CachedTransform.position.x%1,elapseSeconds);
            }        
        }

        private void collider(Vector2 vector, float a,float b,float elapseSeconds)
        {
            bool result1=Physics2D.Raycast(CachedTransform.position, a*vector,RayLength,_layerMask);
            bool result2=Physics2D.Raycast(CachedTransform.position, new Vector2(vector.x*a==0 ?a:vector.x*a,vector.y*a==0 ?a:vector.y*a),RayLength,_layerMask);
            bool result3=Physics2D.Raycast(CachedTransform.position, new Vector2(vector.x*a==0 ?-a:vector.x*a,vector.y*a==0 ?-a:vector.y*a),RayLength,_layerMask);
            Vector2 vector2=Vector2.zero;
            if (!result1&&!result2&&!result3)
            {
                vector2 = elapseSeconds * m_data.Speed * vector*a;
            }
//            else if (Math.Abs(b) > 0.7f)
//            {
//                vector2 = elapseSeconds * b * b * new Vector2(Mathf.Sign(b), Mathf.Sign(b))*(Vector2.one-vector);
//            }  
//            else if (Math.Abs(b)<0.3f)
//            {
//                vector2 = elapseSeconds * (1 - Math.Abs(b)) * (1 - Math.Abs(b)) * new Vector2(-Mathf.Sign(b), -Mathf.Sign(b))*(Vector2.one-vector);
//            }
            CachedTransform.Translate(vector2);
        }
        public override void OnHurt()
        {
            if (state == PlayerState.Invincible)
            {
                return;
            }
            Data.HP -= 1;
            DoTweenAnimation.DOPlay();
            state = PlayerState.Invincible;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
//            Gizmos.DrawRay(CachedTransform.position+new Vector3(0.45f,0.35f),new Vector2(1,0));
//            Gizmos.DrawRay(CachedTransform.position+new Vector3(0.45f,-0.35f),new Vector2(1,0));
            Gizmos.DrawRay(CachedTransform.position,new Vector2(0.5f,0.5f));
            Gizmos.DrawRay(CachedTransform.position,new Vector2(0.5f,-0.5f));
        }
#endif
    }
}

