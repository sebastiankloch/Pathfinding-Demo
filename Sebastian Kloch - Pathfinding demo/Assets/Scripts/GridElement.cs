using TMPro;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public class GridElement : MonoBehaviour
	{
		[SerializeField]
		private GridElementType type;
		[SerializeField]
		private Color normalColor;
		[SerializeField]
		private Color moveColor;
		[SerializeField]
		private Color attackColor;
		[SerializeField]
		private TMP_Text text;

		private GridPosition gridPosition;

		public void SetGridPosition(GridPosition value)
		{
			gridPosition = value;
		}

		public GridPosition GetGridPosition()
		{
			return gridPosition;
		}

		public Vector3 GetGlobaPos()
		{
			return transform.position;
		}

		public GridElementType GetElementType()
		{
			return type;
		}

		public void GridHighlightMove()
		{
			text.color = moveColor;
		}

		public void GridHighlightAttack()
		{
			text.color = attackColor;
		}

		public void ClearHighlight()
		{
			text.color = normalColor;
		}
	}
}