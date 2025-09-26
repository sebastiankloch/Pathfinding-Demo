using System.Collections.Generic;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public class Grid : MonoBehaviour
	{
		[SerializeField]
		private int startXSize;
		[SerializeField]
		private int startZSize;
		[SerializeField]
		private GameObject traversablePrefab;
		[SerializeField]
		private GameObject obstaclePrefab;
		[SerializeField]
		private GameObject coverPrefab;
		[SerializeField]
		private UnitManager unitManager;

		private Transform trans;
		private List<List<GridElement>> grid = new List<List<GridElement>>();

		private void Awake()
		{
			trans = transform;
		}

		private void Start()
		{
			CreateGrid();
		}

		private void CreateGrid()
		{
			Quaternion rotZero = Quaternion.identity;
			for (int x = 0; x < startXSize; x++)
			{
				grid.Add(new List<GridElement>());
				for (int z = 0; z < startZSize; z++)
				{
					GameObject elementPrefab;
					if (x == 0 && z == 2)
						elementPrefab = obstaclePrefab;
					else if (x == 2 && z == 1)
						elementPrefab = obstaclePrefab;
					else if (x == 4 && z == 1)
						elementPrefab = coverPrefab;
					else if (x == 3 && z == 1)
						elementPrefab = coverPrefab;
					else
						elementPrefab = traversablePrefab;

						GameObject gameObject = Instantiate(elementPrefab, new Vector3(x, 0, z), rotZero, trans);
					gameObject.name += $" {x}, {z}";

					GridElement gridElement = gameObject.GetComponent<GridElement>();

					if (gridElement)
					{
						grid[x].Add(gridElement);
						gridElement.SetGridPosition(new GridPosition(x, z));
					}
					else
					{
						Debug.LogError($"There is no {typeof(GridElement)} component when creating a grid", gameObject);
						grid[x].Add(null);
					}
				}
			}
		}

		public int[,] GetGridAsPathfindingGrid_Move()
		{
			if (grid.Count > 0)
			{
				int[,] pathfindingGrid = new int[grid.Count, grid[0].Count];

				for (int x = 0; x < grid.Count; x++)
				{
					for (int z = 0; z < grid[0].Count; z++)
					{
						pathfindingGrid[x, z] = (int)grid[x][z].GetElementType() > 0 ? 1 : 0;

						if (pathfindingGrid[x, z] == 0)
						{
							Unit unit = unitManager.GetUnitAtGridPosition(grid[x][z].GetGridPosition());
							if (unit && unit.GetUnitType() != UnitType.None)
							{
								pathfindingGrid[x, z] = 1;
							}
						}
					}
				}

				return pathfindingGrid;
			}
			else
				return new int[0, 0];
		}

		public int[,] GetGridAsPathfindingGrid_Attack()
		{
			if (grid.Count > 0)
			{
				int[,] pathfindingGrid = new int[grid.Count, grid[0].Count];

				for (int x = 0; x < grid.Count; x++)
				{
					for (int z = 0; z < grid[0].Count; z++)
					{
						pathfindingGrid[x, z] = grid[x][z].GetElementType() == GridElementType.Obstacle ? 1 : 0;

						/*if (pathfindingGrid[x, z] == 0)
						{
							Unit unit = unitManager.GetUnitAtGridPosition(grid[x][z].GetGridPosition());
							if (unit && unit.GetUnitType() != UnitType.None)
							{
								pathfindingGrid[x, z] = 1;
							}
						}*/
					}
				}

				return pathfindingGrid;
			}
			else
				return new int[0, 0];
		}

		public void HighlightMovePath(List<GridPosition> gridPositions)
		{
			foreach (GridPosition gridPos in gridPositions)
			{
				grid[gridPos.x][gridPos.z].GridHighlightMove();
			}
		}

		public void HighlightAttackPath(List<GridPosition> gridPositions)
		{
			foreach (GridPosition gridPos in gridPositions)
			{
				grid[gridPos.x][gridPos.z].GridHighlightAttack();
			}
		}

		public void ClearHighlight()
		{
			for (int x = 0; x < grid.Count; x++)
			{
				for (int z = 0; z < grid[x].Count; z++)
				{
					grid[x][z].ClearHighlight();
				}
			}
		}
	}
}