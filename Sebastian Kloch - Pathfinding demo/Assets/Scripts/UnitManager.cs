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
		[SerializeField]
		private PlayerCharacter player;
		[SerializeField]
		private GameObject enemyPrefab;

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

		public PlayerCharacter GetPlayer()
		{
			return player;
		}

		public void CreateEnemyUnitAt(GridElement gridElement)
		{
			if (!GetUnitAtGridPosition(gridElement.GetGridPosition()))
			{
				GameObject enemyGO = Instantiate(enemyPrefab, gridElement.GetGlobaPos(), Quaternion.identity);
				EnemyCharacter enemy = enemyGO.GetComponent<EnemyCharacter>();
				enemy.SetGridPosition(gridElement.GetGridPosition());
				units.Add(enemy);
			}
		}

		public void ClearEnemyUnitAt(GridPosition gridPosition)
		{
			for (int id = 0; id < units.Count; id++)
			{
				if (units[id] && units[id].GetGridPosition() == gridPosition && units[id].GetUnitType() == UnitType.Enemy)
				{
					Destroy(units[id].gameObject);
					units.RemoveAt(id);
					return;
				}
			}
		}
	}
}