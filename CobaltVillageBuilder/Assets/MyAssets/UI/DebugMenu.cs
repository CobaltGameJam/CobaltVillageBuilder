using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DebugTileMode
{
    none = 0,
    tileType,
    tileShape,
    tilePosition
}

public class DebugMenu : MonoBehaviour
{
    public Text tileText;
    public Text treeText;
    public Text buildingText;
    public Text tileModeText;
    public Text showHideText;

    public DebugTileMode currentMode { get; set; }
    public bool showGui { get; set; }
    private List<GameObject> disabledGuiItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentMode = DebugTileMode.none;
        showGui = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (tileText != null)
        {
            tileText.text = GameObject.FindGameObjectsWithTag("Tile").Length.ToString();
        }
        if (treeText != null)
        {
            treeText.text = GameObject.FindGameObjectsWithTag("Tree").Length.ToString();
        }

        if (buildingText != null)
        {
            buildingText.text = GameObject.FindGameObjectsWithTag("Building").Length.ToString();
        }
    }

    public void DebugMenuHideShow()
    {
        // Reveal Menu:
        if (!this.showGui)
        {
            Debug.Log("Revealing GUI...");
            foreach (var item in disabledGuiItems)
            {
                item.SetActive(true);
            }
            this.showHideText.text = "Hide";
            this.showGui = true;
        }

        // Hide menu:
        else
        {
            Debug.Log("Hidding GUI...");
            var GuiItems = GameObject.FindGameObjectsWithTag("gui");
            foreach (var item in GuiItems)
            {
                item.SetActive(false);
                disabledGuiItems.Add(item);
            }
            this.showHideText.text = "Show";
            this.showGui = false;
        }
    }

    //private Text[] Retrieve

    public void ToggleDebugMode()
    {
        Assets.Scripts.TileGenerator generator = Camera.main.GetComponent<Assets.Scripts.TileGenerator>();
        if (generator != null)
        {
            Debug.Log($"Setting debug tile mode");

            DebugTileMode newMode = DebugTileMode.none;
            switch (currentMode)
            {
                case DebugTileMode.none:
                    newMode = DebugTileMode.tileType;
                    break;

                case DebugTileMode.tileType:
                    newMode = DebugTileMode.tileShape;
                    break;

                case DebugTileMode.tileShape:
                    newMode = DebugTileMode.tilePosition;
                    break;

                case DebugTileMode.tilePosition:
                    newMode = DebugTileMode.none;
                    break;

                default:
                    this.tileModeText.text = "error";
                    break;
            }

            if (newMode == DebugTileMode.none) { generator.displayDebug = false; }
            else { generator.displayDebug = true; }

            SetDebugMode(newMode);
            generator.SetTileDebugText(newMode);
        }
        else
        {
            this.tileModeText.text = "error";
        }
    }

    private void SetDebugMode(DebugTileMode mode)
    {
        currentMode = mode;
        if (currentMode == DebugTileMode.none)
        {
            this.tileModeText.text = " TileMode: None";
        }
        else if (currentMode == DebugTileMode.tileType)
        {
            this.tileModeText.text = " TileMode: Type";
        }
        else if (currentMode == DebugTileMode.tileShape)
        {
            this.tileModeText.text = " TileMode: Shape";
        }
        else if (currentMode == DebugTileMode.tilePosition)
        {
            this.tileModeText.text = " TileMode: Position";
        }
    }

}
