using UnityEngine;

namespace SK.PathfindingDemo
{
	public class PlayerCharacter : Unit
	{
		public void MoveTo(GridElement gridElement)
		{
			transform.position = gridElement.GetGlobaPos();
			SetGridPosition(gridElement.GetGridPosition());
		}
	}
}