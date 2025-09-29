using UnityEngine;

namespace SK.PathfindingDemo
{
	public class GridEditor : MonoBehaviour
	{
		[SerializeField]
		private Grid grid;
		[SerializeField]
		private UnitManager unitManager;

		private bool placeUnit;
		private UnitType unitType;
		private GridElementType gridElementType;

		public void SetBrushToUnit(UnitType unitType)
		{
			placeUnit = true;
			this.unitType = unitType;
		}

		public void SetBrushToGridElement(GridElementType gridElementType)
		{
			placeUnit = false;
			this.gridElementType = gridElementType;
		}

		public void OnInteraction(GridElement gridElement)
		{
			if (placeUnit)
			{
				if (unitType == UnitType.Player)
				{
					PlayerCharacter player = unitManager.GetPlayer();
					grid.SetElementAt(GridElementType.Travelsable, gridElement.GetGridPosition());
					player.TeleportTo(gridElement);
				}
				else if (unitType == UnitType.Enemy)
				{
					grid.SetElementAt(GridElementType.Travelsable, gridElement.GetGridPosition());
					unitManager.CreateEnemyUnitAt(gridElement);
				}
				else if (unitType == UnitType.None)
				{
					unitManager.ClearEnemyUnitAt(gridElement.GetGridPosition());
				}
			}
			else
			{
				if (!unitManager.GetUnitAtGridPosition(gridElement.GetGridPosition()))
					grid.SetElementAt(gridElementType, gridElement.GetGridPosition());
			}
		}
	}
}