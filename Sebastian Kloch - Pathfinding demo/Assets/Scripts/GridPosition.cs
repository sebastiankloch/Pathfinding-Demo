using System;
using System.Drawing;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public struct GridPosition
	{
		public int x;
		public int z;

		public GridPosition(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is GridPosition position &&
				   x == position.x &&
				   z == position.z;
		}

		public override readonly int GetHashCode()
		{
			return HashCode.Combine(x, z);
		}

		public static bool operator ==(GridPosition pos, GridPosition posOther)
		{
			return
				pos.x == posOther.x &&
				pos.z == posOther.z;
		}

		public static bool operator !=(GridPosition pos, GridPosition posOther)
		{
			return !(pos == posOther);
		}

		public static GridPosition operator -(GridPosition pos, GridPosition posOther)
		{
			pos.x -= posOther.x;
			pos.z -= posOther.z;
			
			return pos;
		}

		public override string ToString()
		{
			return $"x: {x}, z: {z}";
		}
	}
}