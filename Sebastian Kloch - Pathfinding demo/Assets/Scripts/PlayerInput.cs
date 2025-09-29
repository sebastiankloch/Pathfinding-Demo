using System.Collections.Generic;
using Unity.Cinemachine;
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
		[SerializeField]
		private CameraController cameraController;
		[SerializeField]
		private CinemachineInputAxisController cinemaInputCont;
		[SerializeField]
		private GridEditor gridEditor;

		private AStarPathfinder pathfinder = new AStarPathfinder();
		private GridElement selectedElement;

		private void Update()
		{
			Mouse mouse = Mouse.current;
			if (mouse != null)
			{
				if(GameplayManager.GetState() == GameplayState.EditMode)
				{
					if (mouse.leftButton.isPressed)
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

									gridEditor.OnInteraction(gridElement);
								}
								else
									Debug.LogError($"There is no {typeof(GridElement)}, after raycast check", col);
							}
						}
					}	
				}
				else if (GameplayManager.GetState() == GameplayState.PlayMode)
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
												List<GridPosition> path = FindPath(grid.GetGridAsPathfindingGrid_Attack(gridElement.GetGridPosition()), gridElement);
												Debug.Log($"Attack Path:\n{AStarPathfinder.PathToString(path)}");

												if (gridElement == selectedElement)
												{
													if (path.Count > 0 && path.Count - 1 <= playerCharacter.GetAttackRange())
													{
														unit.Damage();
														grid.ClearHighlight();
														selectedElement = null;
													}
												}
												else
												{
													selectedElement = gridElement;
													grid.ClearHighlight();
													grid.HighlightAttackPath(path, playerCharacter.GetAttackRange());
												}

											}
										}
									}
									else
									{
										if (gridElement.GetElementType() == GridElementType.Travelsable)
										{
											List<GridPosition> path = FindPath(grid.GetGridAsPathfindingGrid_Move(), gridElement);
											Debug.Log($"Path:\n{AStarPathfinder.PathToString(path)}");

											if (gridElement == selectedElement)
											{
												if (path.Count > 0 && path.Count - 1 <= playerCharacter.GetMoveRange())
												{
													playerCharacter.TeleportTo(gridElement);
													grid.ClearHighlight();
													selectedElement = null;
												}
											}
											else
											{
												selectedElement = gridElement;
												grid.ClearHighlight();
												grid.HighlightMovePath(path, playerCharacter.GetMoveRange());
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
					

				for (int i = 0; i < cinemaInputCont.Controllers.Count; i++)
				{
					InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller controller = cinemaInputCont.Controllers[i];

					if (controller.Name == "Look Orbit X" || controller.Name == "Look Orbit Y")
					{
						controller.Enabled = mouse.middleButton.isPressed;
					}
				}
			}

			Keyboard keyboard = Keyboard.current;

			if (keyboard != null)
			{
				if (keyboard.anyKey.isPressed)
				{
					Vector2 moveVector = Vector2.zero;

					if (keyboard[Key.W].isPressed)
					{
						moveVector += Vector2.up;
					}

					if (keyboard[Key.S].isPressed)
					{
						moveVector += Vector2.down;
					}

					if (keyboard[Key.A].isPressed)
					{
						moveVector += Vector2.left;
					}

					if (keyboard[Key.D].isPressed)
					{
						moveVector += Vector2.right;
					}

					cameraController.Move(moveVector, keyboard[Key.LeftShift].isPressed);
				}
			}
		}

		private List<GridPosition> FindPath(int[,] grid, GridElement gridElement)
		{
			return pathfinder.FindPath(grid, playerCharacter.GetGridPosition(), gridElement.GetGridPosition());
		}
	}
}