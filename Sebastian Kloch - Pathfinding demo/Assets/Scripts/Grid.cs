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
		private GameObject[] elementPrefabs;
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
						elementPrefab = elementPrefabs[1];
					else if (x == 2 && z == 1)
						elementPrefab = elementPrefabs[1];
					else if (x == 4 && z == 1)
						elementPrefab = elementPrefabs[2];
					else if (x == 3 && z == 1)
						elementPrefab = elementPrefabs[2];
					else
						elementPrefab = elementPrefabs[0];

					grid[x].Add(null);
					InstantiateElement(elementPrefab, rotZero, x, z);
				}
			}
		}

		private void InstantiateElement(GameObject elementPrefab, Quaternion rotZero, int x, int z)
		{
			GameObject newGO = Instantiate(elementPrefab, new Vector3(x, 0, z), rotZero, trans);
			newGO.name += $" {x}, {z}";

			GridElement gridElement = newGO.GetComponent<GridElement>();

			if (gridElement)
			{
				grid[x][z] = gridElement;
				gridElement.SetGridPosition(new GridPosition(x, z));
			}
			else
			{
				Debug.LogError($"There is no {typeof(GridElement)} component when creating a grid", newGO);
				grid[x][z] = null;
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

		public int[,] GetGridAsPathfindingGrid_Attack(GridPosition targetPosition)
		{
			if (grid.Count > 0)
			{
				int[,] pathfindingGrid = new int[grid.Count, grid[0].Count];

				for (int x = 0; x < grid.Count; x++)
				{
					for (int z = 0; z < grid[0].Count; z++)
					{
						pathfindingGrid[x, z] = grid[x][z].GetElementType() == GridElementType.Obstacle ? 1 : 0;

						if (pathfindingGrid[x, z] == 0 && x != targetPosition.x && z != targetPosition.z)
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

		public void HighlightMovePath(List<GridPosition> gridPositions, int range)
		{
			for (int id = 0; id < gridPositions.Count; id++)
			{
				GridPosition gridPos = gridPositions[id];
				if (id <= range)
					grid[gridPos.x][gridPos.z].HighlightMove();
				else
					grid[gridPos.x][gridPos.z].HighlightMoveOutOfRange();
			}
		}

		public void HighlightAttackPath(List<GridPosition> gridPositions, int range)
		{
			for (int id = 0; id < gridPositions.Count; id++)
			{
				GridPosition gridPos = gridPositions[id];
				if (id <= range)
					grid[gridPos.x][gridPos.z].HighlightAttack();
				else
					grid[gridPos.x][gridPos.z].HighlightAttackOutOfRange();
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

		public void ChangeGridSize(int xSize, int zSize)
		{
			if (xSize == grid.Count && zSize == grid[0].Count)
				return;

			if (xSize < 2) xSize = 2;
			if (zSize < 2) zSize = 2;

			if (xSize > 100) xSize = 100;
			if (zSize > 100) zSize = 100;

			Quaternion rotZero = Quaternion.identity;

			while (grid.Count < xSize)
				grid.Add(new List<GridElement>());
			while (grid.Count > xSize)
			{
				List<GridElement> lastCol = grid[^1];
				foreach (GridElement elem in lastCol)
				{
					if (elem != null) 
						Destroy(elem.gameObject);
				}

				grid.RemoveAt(grid.Count - 1);
			}

			for (int x = 0; x < grid.Count; x++)
			{
				while (grid[x].Count < zSize)
				{
					int z = grid[x].Count;
					grid[x].Add(null);
					InstantiateElement(elementPrefabs[(int)GridElementType.Travelsable], rotZero, x, z);
				}

				while (grid[x].Count > zSize)
				{
					GridElement elem = grid[x][^1];
					if (elem != null) 
						Destroy(elem.gameObject);
					grid[x].RemoveAt(grid[x].Count - 1);
				}
			}
		}

		public void SetElementAt(GridElementType elementType, GridPosition gridPosition)
		{
			if (gridPosition.x < 0 || gridPosition.x >= grid.Count || gridPosition.z < 0 || gridPosition.z >= grid[gridPosition.x].Count)
			{
				Debug.LogError($"SetElementAt: Position ({gridPosition}) is out of bounds!");
				return;
			}

			GridElement element = grid[gridPosition.x][gridPosition.z];

			bool needsNew = (element == null) || (element.GetElementType() != elementType);

			if (needsNew)
			{
				if (element)
					Destroy(element.gameObject);

				InstantiateElement(elementPrefabs[(int)elementType], Quaternion.identity, gridPosition.x, gridPosition.z);
			}
		}

		public GridElement GetElementAt(GridPosition gridPosition)
		{
			if (gridPosition.x < 0 || gridPosition.x >= grid.Count || gridPosition.z < 0 || gridPosition.z >= grid[gridPosition.x].Count)
			{
				Debug.LogError($"GetGridElementAt: Position ({gridPosition}) is out of bounds!");
				return null;
			}

			return grid[gridPosition.x][gridPosition.z];
		}

		public int GetStartXSize()
		{
			return startXSize;
		}

		public int GetStartZSize()
		{
			return startZSize;
		}

		public int GetMapXSize()
		{
			return grid.Count;
		}

		public int GetMapZSize()
		{
			return grid[0].Count;
		}
	}
}