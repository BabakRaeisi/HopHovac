using UnityEngine;

public class ColorManager : MonoBehaviour, IColorManager
{
    [SerializeField] private Color[] playerColors;

    public Color GetPlayerColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerColors.Length)
        {
            return playerColors[playerIndex];
        }
        else
        {
            Debug.LogError("Player index out of range!");
            return Color.white;
        }
    }
}
public interface IColorManager
{
    Color GetPlayerColor(int playerIndex);
}