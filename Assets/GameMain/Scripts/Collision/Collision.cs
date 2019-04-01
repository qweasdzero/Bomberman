using System.Collections.Generic;
using UnityEngine;

namespace SG1
{
	 abstract class Collision
	{
		protected static readonly Vector2 Southeast = new Vector2(1, -1);
		protected static readonly Vector2 Northeast = new Vector2(1, 1);
		protected static readonly Vector2 Southwest = new Vector2(-1, -1);
		protected static readonly Vector2 Northwest = new Vector2(-1, 1);		
		protected static readonly Vector2 West = new Vector2(-1, 0);
		protected static readonly Vector2 East = new Vector2(1, 0);
		protected static readonly Vector2 South = new Vector2(0, -1);
		protected static readonly Vector2 North = new Vector2(0, 1);
		protected static readonly Vector2[] list = {Southeast, Northeast, Southwest, Northwest, West, East, South, North};

		protected abstract List<GameObject> OnCollision(Vector3 position, float Length, LayerMask Mask);
		
		public List<GameObject> OnCollision(Vector3 position,float length)
		{
			return OnCollision(position, length, -5);
		}
		/// <summary>
		/// 圆形物体是否碰到物体
		/// </summary>
		public bool IsOnCollision(Vector3 position,float length,LayerMask mask)
		{
			if (OnCollision(position, length, mask) == null)
			{
				return false;
			}
			return true;
		}
		public bool IsOnCollision(Vector3 position,float length)
		{
			return IsOnCollision(position, length, -5);
		}
	}
}
