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


    // Frame rate properties:
    public Text frameRateText;
    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;
    private float curFPS = 0.0f;
    private float updateRate = 2.5f;

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

        if (frameRateText != null)
        {
            frameCount++;
            dt += Time.unscaledDeltaTime;

            if (dt > 1.0f / updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= (1.0f / updateRate);
            }
            curFPS = GlobalMethods.Ease(curFPS, fps, 0.25f);
            frameRateText.text = curFPS.ToString("0.0");
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
            this.tileModeText.text = " DebugMode: None";
        }
        else if (currentMode == DebugTileMode.tileType)
        {
            this.tileModeText.text = " DebugMode: Type";
        }
        else if (currentMode == DebugTileMode.tileShape)
        {
            this.tileModeText.text = " DebugMode: Shape";
        }
        else if (currentMode == DebugTileMode.tilePosition)
        {
            this.tileModeText.text = " DebugMode: Position";
        }
    }

}
