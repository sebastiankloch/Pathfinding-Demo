using UnityEngine;

namespace SK.PathfindingDemo
{
	public class GridElement : MonoBehaviour
	{
		[SerializeField]
		private GridElementType type;

		private GridPosition gridPosition;

		

		public void SetGridPosition(GridPosition value)
		{
			gridPosition = value;
		}

		public GridPosition GetGridPosition()
		{
			return gridPosition;
		}

		public Vector3 GetGlobaPos()
		{
			return transform.position;
		}

		public GridElementType GetElementType()
		{
			return type;
		}
	}
}