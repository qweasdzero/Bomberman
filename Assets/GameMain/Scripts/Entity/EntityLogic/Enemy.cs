using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

namespace SG1
{
	public class Enemy : TargetableObject
	{
		public EnemyData m_data;
		public override TargetableObjectData Data
		{
			get { return m_data; }
		}
		private int h=-1;
		private int v=0;
		public List<int> list =new List<int>();
		public float Randomtimer;
		protected override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_data = (EnemyData) userData;
			CachedTransform.localScale=new Vector2(1.9f,1.9f);
			list.Add(1);
			list.Add(-1);
			Randomtimer=Random.Range(3,5);
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
			if (m_data.HP <= 0)
			{
				GameEntry.Entity.HideEntity(this);
			}
			Move(elapseSeconds);
			Randomtimer -= elapseSeconds;
			if (Randomtimer <= 0)
			{
				random();
			}
		}

		private void Move(float elapseSeconds)
		{            
			if (Math.Abs(h) > Math.Abs(v))
			{
				collider(Vector2.right,Mathf.Sign(h), elapseSeconds);
			}
			else if(Math.Abs(v) > Math.Abs(h))
			{
				collider(Vector2.up, Mathf.Sign(v), elapseSeconds);
			}       

		}

		private void collider(Vector2 vector,float a,float elapseSeconds)
		{
			RaycastHit2D result1=Physics2D.Raycast(CachedTransform.position, a*vector,0.5f,1<< LayerMask.NameToLayer("Wall")|1 << LayerMask.NameToLayer("Player"));
//			RaycastHit2D result2=Physics2D.Raycast(CachedTransform.position, new Vector2(vector.x*a==0 ?a:vector.x*a,vector.y*a==0 ?a:vector.y*a),0.5f,1 << LayerMask.NameToLayer("Wall"));
//			RaycastHit2D result3=Physics2D.Raycast(CachedTransform.position, new Vector2(vector.x*a==0 ?-a:vector.x*a,vector.y*a==0 ?-a:vector.y*a),0.5f,1 << LayerMask.NameToLayer("Wall"));
			if (result1&&result1.collider.CompareTag("Player"))
			{
				v = -v;
				result1.collider.GetComponent<TargetableObject>().OnHurt();
			}
//			else if (result2&&result2.collider.CompareTag("Player"))
//			{
//				v = -v;
//				result2.collider.GetComponent<TargetableObject>().OnHurt();
//			}		
//			else if (result3&&result3.collider.CompareTag("Player"))
//			{
//				v = -v;
//				result3.collider.GetComponent<TargetableObject>().OnHurt();
//			}
			else if (!result1)
			{
				CachedTransform.Translate(elapseSeconds * m_data.Speed*vector*a);
			}
			else
			{
				random();
			}
		}
		private void random()
		{

				h = Random.Range(-1, 2);
				if (h == 0)
				{
					v = list[Random.Range(0, 2)];
				}
				else
				{
					v = 0;
				}
				Randomtimer = Random.Range(3, 5);

		}
	}
}
