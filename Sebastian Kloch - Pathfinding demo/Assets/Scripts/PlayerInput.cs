using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SK.PathfindingDemo
{
	public class PlayerInput : MonoBehaviour
	{
		private const string GRID_ELEMENT = "Grid Element";

		[SerializeField]
		private LayerMask gridLayerMask;
		[SerializeField]
		private Camera cam;
		[SerializeField]
		private PlayerCharacter playerCharacter;
		[SerializeField]
		private UnitManager unitManager;
		[SerializeField]
		private Grid grid;

		private AStarPathfinder pathfinder = new AStarPathfinder();
		private GridElement selectedElement;

		private void Update()
		{
			Mouse mouse = Mouse.current;
			if (mouse != null)
			{
				if (mouse.leftButton.wasPressedThisFrame)
				{
					Ray ray = cam.ScreenPointToRay(mouse.position.value);

					if (Physics.Raycast(ray, out RaycastHit hit, 1000, gridLayerMask))
					{
						Collider col = hit.collider;
						if (col.CompareTag(GRID_ELEMENT))
						{
							GridElement gridElement = col.GetComponent<GridElement>();
							if (gridElement)
							{
								Debug.Log($"Clicked on {gridElement.GetGridPosition()}");
								Unit unit = unitManager.GetUnitAtGridPosition(gridElement.GetGridPosition());

								if (unit)
								{
									if (unit.GetUnitType() == UnitType.Player)
									{

									}
									else if (unit.GetUnitType() == UnitType.Enemy)
									{
										if (gridElement.GetElementType() == GridElementType.Travelsable)
										{
											if (gridElement == selectedElement)
											{
												List<GridPosition> path = pathfinder.FindPath(grid.GetGridAsPathfindingGrid_Attack(), playerCharacter.GetGridPosition(), gridElement.GetGridPosition());
												Debug.Log($"Attack Path:\n{AStarPathfinder.PathToString(path)}");
												if (path.Count > 0)
												{
													unit.Damage();
													grid.ClearHighlight();
													selectedElement = null;
												}
											}
											else
											{
												selectedElement = gridElement;
												List<GridPosition> path = pathfinder.FindPath(grid.GetGridAsPathfindingGrid_Attack(), playerCharacter.GetGridPosition(), gridElement.GetGridPosition());
												Debug.Log($"Attack Path:\n{AStarPathfinder.PathToString(path)}");
												grid.ClearHighlight();
												grid.HighlightAttackPath(path);
											}

										}
									}
								}
								else
								{
									if (gridElement.GetElementType() == GridElementType.Travelsable)
									{
										if (gridElement == selectedElement)
										{
											List<GridPosition> path = pathfinder.FindPath(grid.GetGridAsPathfindingGrid_Move(), playerCharacter.GetGridPosition(), gridElement.GetGridPosition());
											Debug.Log($"Path:\n{AStarPathfinder.PathToString(path)}");
											if (path.Count > 0 && path.Count - 1 <= playerCharacter.GetMoveRange())
											{
												playerCharacter.MoveTo(gridElement);
												grid.ClearHighlight();
												selectedElement = null;
											}
										}
										else
										{
											selectedElement = gridElement;
											List<GridPosition> path = pathfinder.FindPath(grid.GetGridAsPathfindingGrid_Move(), playerCharacter.GetGridPosition(), gridElement.GetGridPosition());
											Debug.Log($"Path:\n{AStarPathfinder.PathToString(path)}");
											grid.ClearHighlight();
											grid.HighlightMovePath(path);
										}

									}
								}
							}
							else
								Debug.LogError($"There is no {typeof(GridElement)}, after raycast check", col);
						}
					}
				}
			}
		}
	}
}