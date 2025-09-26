using System.Collections.Generic;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public class AStarPathfinder
	{
		List<Node> open = new List<Node>();
		HashSet<Node> closed = new HashSet<Node>();
		List<Node> neighbours = new List<Node>();

		public class Node
		{
			public GridPosition gridPosition;
			public int g_costFromStart;
			public int heuristic;
			public Node parent;

			public int GetF_TotalCost()
			{
				return g_costFromStart + heuristic;
			}

			public Node(GridPosition gridPosition)
			{
				this.gridPosition = gridPosition;
			}

			public override bool Equals(object obj)
			{
				return
					obj is Node other &&
					other.gridPosition == gridPosition;
			}

			public override int GetHashCode()
			{
				return gridPosition.GetHashCode();
			}

			public static int ReverseTotalCostComparision(Node first, Node second)
			{
				return first.GetF_TotalCost().CompareTo(second.GetF_TotalCost()) * -1;
			}
		}

		private static int Manhattan(GridPosition pos, GridPosition posOther)
		{
			return
				Mathf.Abs(pos.x - posOther.x) +
				Mathf.Abs(pos.z - posOther.z);
		}

		public List<GridPosition> FindPath(int[,] grid, GridPosition start, GridPosition goal)
		{
			if (grid.GetLength(0) > 0 &&
				grid.GetLength(1) > 0)
			{
				open.Clear();
				closed.Clear();

				Node startNode = new Node(start)
				{
					g_costFromStart = 0,
					heuristic = Manhattan(start, goal)
				};
				open.Add(startNode);

				while (open.Count > 0)
				{
					Node current = SortAndGetNodeWithSmallestTotalCost();

					if (current.gridPosition == goal)
						return ReconstructPath(current);

					open.RemoveAt(0);
					closed.Add(current);

					neighbours.Clear();
					GetNeighbours(grid, current, neighbours);

					foreach (Node neighbour in neighbours)
					{
						if (closed.Contains(neighbour)) continue;

						int g_costFromStart = current.g_costFromStart + 1;

						Node openNode = GetFirstOpenNodeEqualTo(neighbour);
						if (openNode == null)
						{
							neighbour.g_costFromStart = g_costFromStart;
							neighbour.heuristic = Manhattan(neighbour.gridPosition, goal);
							neighbour.parent = current;
							open.Add(neighbour);
						}
						else if (g_costFromStart < openNode.g_costFromStart)
						{
							openNode.g_costFromStart = g_costFromStart;
							openNode.parent = current;
						}
					}
				}
			}

			return new List<GridPosition>(); // No path found
		}

		private Node GetFirstOpenNodeEqualTo(Node node)
		{
			for (int id = 0; id < open.Count; id++)
			{
				if (open[id].Equals(node))
					return open[id];
			}

			return null;
		}

		private Node SortAndGetNodeWithSmallestTotalCost()
		{
			open.Sort(Node.ReverseTotalCostComparision);
			return open[0];
		}

		private static void GetNeighbours(int[,] grid, Node node, List<Node> neighbours)
		{
			int xCount = grid.GetLength(0);
			int zCount = grid.GetLength(1);

			// 4-way movement
			int[,] directions =
			{
				{ 0, 1 },
				{ 1, 0 },
				{ 0, -1 },
				{ -1, 0 }
			};

			for (int i = 0; i < directions.GetLength(0); i++)
			{
				int neighbourX = node.gridPosition.x + directions[i, 0];
				int neighbourZ = node.gridPosition.z + directions[i, 1];

				if (neighbourX >= 0 &&
					neighbourZ >= 0 &&
					neighbourX < xCount &&
					neighbourZ < zCount &&
					grid[neighbourX, neighbourZ] == 0)
				{
					neighbours.Add(new Node(new GridPosition(neighbourX, neighbourZ)));
				}
			}
		}

		private static List<GridPosition> ReconstructPath(Node node)
		{
			List<GridPosition> path = new List<GridPosition>();
			while (node != null)
			{
				path.Add(node.gridPosition);
				node = node.parent;
			}
			path.Reverse();
			return path;
		}

		public static string PathToString(List<GridPosition> path)
		{
			string text = "";

			for (int id = 0; id < path.Count; id++)
			{
				GridPosition pos = path[id];
				text += $"id: {id}, pos: {pos}\n";
			}

			return text;
		}
	}
}