using UnityEngine;

[CreateAssetMenu(fileName = "GridElementData", menuName = "Scriptable Objects/GridElementData")]
public class GridElementData : ScriptableObject
{
	public Color normalColor;
	public Color moveColor;
	public Color moveOutOfRangeColor;
	public Color attackColor;
	public Color attackOutOfRangeColor;
}
