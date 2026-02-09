using DG.Tweening;
using TMPro;
using UnityEngine;

public enum TileLevel
{
    Empty = 0,
    Two = 2,
    Four = 4,
    Eight = 8,
    Sixteen = 16,
    ThirtyTwo = 32,
    SixtyFour = 64,
    OneTwoEight = 128,
    TwoFiveSix = 256,
    FiveOneTwo = 512,
    TenTwentyFour = 1024,
    TwentyFortyEight = 2048
}
public class Tile : MonoBehaviour
{
    [SerializeField] private TileLevel currentTileLevel;
    [SerializeField] private TextMeshProUGUI textDisplay; // Link to the UI text
    [SerializeField] private Renderer cubeRenderer;

    //Expose private variable to other scripts
    public TileLevel CurrentLevel => currentTileLevel;

    public void UpdateVisuals(TileLevel newLevel)
    {
        currentTileLevel = newLevel;

        // Convert Enum to Int then to String
        string textValue = (currentTileLevel == TileLevel.Empty) ? "" : ((int)currentTileLevel).ToString();

        // Check if textDisplay exists before using it
        if (textDisplay != null)
        {
            textDisplay.text = textValue;
            Debug.Log("Cube text set to: " + textValue);
        }
        else
        {
            Debug.LogError("TextDisplay is MISSING on this cube!");
        }

        ApplyColor(currentTileLevel);
    }
    private void ApplyColor(TileLevel level)
    {
        // Set general color for text and background
        textDisplay.color = Color.white;
        Color cubeColor = Color.gray;

        // Handle background color depanding num
        switch (level)
        {
            case TileLevel.Two: 
                cubeColor = new Color(0.93f, 0.89f, 0.85f);
                break;
            case TileLevel.Four: cubeColor = new Color(0.93f, 0.88f, 0.78f); break;
            case TileLevel.Eight: cubeColor = new Color(0.95f, 0.69f, 0.47f); break;
            case TileLevel.Sixteen: cubeColor = new Color(0.96f, 0.58f, 0.39f); break;
            case TileLevel.ThirtyTwo: cubeColor = new Color(0.96f, 0.49f, 0.37f); break;
            case TileLevel.SixtyFour: cubeColor = new Color(0.96f, 0.37f, 0.23f); break;
            case TileLevel.OneTwoEight: cubeColor = new Color(0.93f, 0.81f, 0.45f); break;
            case TileLevel.TwoFiveSix: cubeColor = new Color(0.93f, 0.80f, 0.38f); break;
            case TileLevel.FiveOneTwo: cubeColor = new Color(0.93f, 0.78f, 0.31f); break;
            case TileLevel.TenTwentyFour: cubeColor = new Color(0.93f, 0.77f, 0.25f); break;
            case TileLevel.TwentyFortyEight: cubeColor = new Color(0.93f, 0.76f, 0.18f); break;
        }

        cubeRenderer.material.color = cubeColor;

        // Specific background color for 2 and 4 nums
        if (level == TileLevel.Two || level == TileLevel.Four)
        {
            textDisplay.color = new Color(0.46f, 0.43f, 0.39f); // Dark Grey
        }
    }
    public void MergeWith(Tile incom)
    {
        //Suming by Enum level and update
        int nextValue = (int)currentTileLevel * 2;
        currentTileLevel = (TileLevel)nextValue;

        //Animation
        incom.transform.DOLocalMove(this.transform.localPosition, 0.15f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                //when nimation overlaps, after destory the incom cube
                Destroy(incom.gameObject);
                //Update the cube text
                UpdateVisuals(currentTileLevel);
                //for merging tiles
                transform.DOPunchScale(Vector3.one * 0.2f, 0.15f);
            });
    }
}
