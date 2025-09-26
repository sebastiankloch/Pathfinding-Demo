using UnityEngine;

namespace SK.PathfindingDemo
{
	public class Unit : MonoBehaviour
	{
		[SerializeField]
		protected UnitType unitType;
		[SerializeField]
		protected GridPosition gridPosition;

		public UnitType GetUnitType()
		{
			return unitType;
		}

		public GridPosition GetGridPosition()
		{
			return gridPosition;
		}

		public void SetGridPosition(GridPosition value)
		{
			gridPosition = value;
		}

		public void Damage()
		{
			Destroy(gameObject);
		}
	}
}