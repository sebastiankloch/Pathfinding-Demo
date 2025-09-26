using UnityEngine;

namespace SK.PathfindingDemo
{
	public class PlayerCharacter : Unit
	{
		[SerializeField]
		private int moveRange;
		[SerializeField]
		private int attackRange;

		public int GetMoveRange()
		{
			return moveRange;
		}

		public void SetMoveRange(int value)
		{
			moveRange = value;
		}

		public int GetAttackRange()
		{
			return attackRange;
		}

		public void SetAttackRange(int value)
		{
			attackRange = value;
		}

		public void MoveTo(GridElement gridElement)
		{
			transform.position = gridElement.GetGlobaPos();
			SetGridPosition(gridElement.GetGridPosition());
		}
	}
}