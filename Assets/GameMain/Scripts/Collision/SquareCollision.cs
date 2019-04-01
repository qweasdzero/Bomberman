using System.Collections.Generic;
using UnityEngine;

namespace SG1
{
	class SquareCollision:Collision
	{		
		/// <summary>
		/// 圆形物体所碰到的物体
		/// </summary>
		/// <param name="position"></param>碰撞物体位置
		/// <param name="Length"></param>碰撞物大小
		/// <param name="Mask"></param>碰撞物层级
		/// <returns></returns>
		protected override List<GameObject> OnCollision(Vector3 position,float Length,LayerMask Mask)
		{
			List<GameObject> GameObjectList=new List<GameObject>();
			for (int i = 0; i < list.Length; i++)
			{
				
			}
			return GameObjectList;
		}
	}
}
