using UnityEngine;
using System.Collections.Generic;

namespace SK.PathfindingDemo
{
	public class UnitManager : MonoBehaviour
	{
		[SerializeField]
		private List<Unit> units;
		[SerializeField]
		private Grid grid;

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

		public void ClearUnitsOutsideOfBounds(int xSize, int zSize)
		{
			for (int id = units.Count - 1; id >= 0; id--)
			{
				if (!units[id])
				{
					units.RemoveAt(id);
					continue;
				}

				GridPosition gridPosition = units[id].GetGridPosition();
				if (gridPosition.x >= xSize ||
					gridPosition.z >= zSize)
				{
					if (units[id].GetUnitType() == UnitType.Player)
					{
						GridPosition playerPos = new GridPosition(0, 0);
						grid.SetElementAt(GridElementType.Travelsable, playerPos);
						GridElement element = grid.GetElementAt(playerPos);
						if (element)
						{
							((PlayerCharacter)units[id]).TeleportTo(element);
						}

					}
					else
					{
						Destroy(units[id].gameObject);
						units.RemoveAt(id);
					}
				}
			}
		}
	}
}