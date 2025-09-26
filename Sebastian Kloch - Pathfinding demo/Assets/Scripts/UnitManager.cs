using UnityEngine;
using System.Collections.Generic;

namespace SK.PathfindingDemo
{
	public class UnitManager : MonoBehaviour
	{
		[SerializeField]
		private List<Unit> units;

		private void Start()
		{
			foreach (Unit unit in units)
			{
				if (unit.GetUnitType() == UnitType.Player)
				{
					unit.SetGridPosition(new GridPosition(0, 0));
				}
				else if (unit.GetUnitType() == UnitType.Enemy)
					unit.SetGridPosition(new GridPosition(3, 2));
			}
		}

		public Unit GetUnitAtGridPosition(GridPosition gridPosition)
		{
			foreach (Unit unit in units)
			{
				if (unit && unit.GetGridPosition() == gridPosition)
				{
					return unit;
				}
			}

			return null;
		}
	}
}