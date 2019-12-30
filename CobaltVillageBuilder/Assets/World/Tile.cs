using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile : MonoBehaviour
{

    public string seedID { get; set; }

    #region Assets
    public Material grass;
    public Material flowers;
    public Material water;

    public Texture water_single;
    public Texture water_horz;
    public Texture water_vert;
    public Texture water_upLeft;
    public Texture water_upRight;
    public Texture water_downRight;
    public Texture water_downLeft;
    public Texture water_rightT;
    public Texture water_downT;
    public Texture water_leftT;
    public Texture water_upT;
    public Texture water_all;

    public Tree tree1;
    public Tree tree2;
    public Tree tree3;

    public House house1;
    public House house2;
    #endregion

    public enum TreeType
    {
        tree1 = 0,
        tree2,
        tree3
    }

    public enum HouseType
    {
        house1 = 0,
        house2
    }

    private static System.Random numGen = new System.Random();

    public int xPos { get; set; }
    public int zPos { get; set; }

    private TileType _type { get; set; }
    public TileType Type
    {
        get
        {
            return _type;
        }
        set
        {
            if (displayDebug)
            {
                Debug.Log($"Setting type to: {value}");
            }
            _type = value;
        }
    }

    public bool IsShapeable = false;

    public enum TileType
    {
        grass = 0,
        flowers,
        water,
        trees,
        house
    }

    public bool displayDebug { get; set; }
    public DebugTileMode currentDebugMode { get; set; }

    public Tile TileAbove { get; set; }
    public Tile TileRight { get; set; }
    public Tile TileBelow { get; set; }
    public Tile TileLeft { get; set; }

    public List<Tree> MyTrees = new List<Tree>();
    public List<House> MyHouses = new List<House>();

    public void UpdateTileType(TileType type)
    {
        Type = type;

        switch (type)
        {
            case TileType.grass:
                this.GetComponent<MeshRenderer>().material = grass;
                IsShapeable = false;
                break;

            case TileType.flowers:
                this.GetComponent<MeshRenderer>().material = flowers;
                IsShapeable = false;
                break;

            case TileType.water:
                this.GetComponent<MeshRenderer>().material = water;
                IsShapeable = true;
                break;

            case TileType.trees:
                this.GetComponent<MeshRenderer>().material = grass;
                IsShapeable = true;
                break;

            case TileType.house:
                this.GetComponent<MeshRenderer>().material = grass;
                IsShapeable = false;
                break;

            default:
                this.GetComponent<MeshRenderer>().material = grass;
                IsShapeable = false;
                break;
        }

    }

    private bool CheckAbove()
    {
        if (TileAbove == null)
            return true;
        else if (TileAbove.Type == Type)
            return true;
        else
            return false;
    }

    private bool CheckRight()
    {
        if (TileRight == null)
            return true;
        else if (TileRight.Type == Type)
            return true;
        else
            return false;
    }
    private bool CheckBelow()
    {
        if (TileBelow == null)
            return true;
        else if (TileBelow.Type == Type)
            return true;
        else
            return false;
    }
    private bool CheckLeft()
    {
        if (TileLeft == null)
            return true;
        else if (TileLeft.Type == Type)
            return true;
        else
            return false;
    }

    private int RetrieveShapeId()
    {

        if (displayDebug == true && TileAbove != null && TileRight != null && TileBelow != null && TileLeft != null)
        {
            Debug.Log($"{xPos}{zPos} Has all neighbors");
        }

        int shapeId = 0;
        if (CheckAbove()) { shapeId |= 4; }
        if (CheckRight()) { shapeId |= 1; }
        if (CheckBelow()) { shapeId |= 2; }
        if (CheckLeft()) { shapeId |= 8; }

        return shapeId;
    }

    private string ShapeToString(int shapeId)
    {
        string ret = "";
        switch (shapeId)
        {
            // SINGLE
            case 0:
                ret = ".";
                break;

            // HORZ LINE
            case 1:
            case 8:
            case 9:
                ret = "━";
                break;

            // VERT LINE
            case 2:
            case 4:
            case 6:
                ret = "│";
                break;

            // UPLEFT CORNER
            case 12:
                ret = "┛";
                break;

            // UPRIGHT CORNER
            case 5:
                ret = "┗";
                break;

            // DOWNRIGHT CORNER
            case 3:
                ret = "┏";
                break;

            // DOWNLEFT CORNER
            case 10:
                ret = "┓";
                break;

            // RIGHT T
            case 7:
                ret = "┣";
                break;

            // DOWN T
            case 11:
                ret = "┯";
                break;

            // LEFT T
            case 14:
                ret = "┨";
                break;

            // UP T
            case 13:
                ret = "┷";
                break;

            // ALL SIDES
            case 15:
                ret = "┿";
                break;
        }
        return ret;
    }

    public void SetTileDebug(DebugTileMode mode, bool showDebugMessages = true)
    {
        displayDebug = showDebugMessages;
        if (mode == DebugTileMode.none)
        {
            SetDebugText("");
        }
        else if (mode == DebugTileMode.tileShape)
        {
            if (IsShapeable)
            {
                SetDebugText(ShapeToString(RetrieveShapeId()));
            }
            else
            {
                SetDebugText(" ");
            }
        }
        else if (mode == DebugTileMode.tileType)
        {
            if (Type == TileType.grass) { SetDebugText("G"); }
            else if (Type == TileType.flowers) { SetDebugText("F"); }
            else if (Type == TileType.trees) { SetDebugText("T"); }
            else if (Type == TileType.water) { SetDebugText("W"); }
        }
        else if (mode == DebugTileMode.tilePosition)
        {
            SetDebugText($"({xPos},{zPos})");
        }
    }


    public void ResetTileShape()
    {
        if (IsShapeable)
        {
            if (Type == TileType.trees)
            {
                ClearTrees();
            }

            int shape = RetrieveShapeId();
            switch (shape)
            {
                // SINGLE
                case 0:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - single"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_single);
                    }
                    else if (Type == TileType.trees)
                    {
                        float radius = 0f;
                        GenerateTreesAtPos(1, -radius, radius, -radius, radius);
                    }
                    break;

                // HORZ LINE
                case 1:
                case 8:
                case 9:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Horz"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_horz);
                    }
                    else if (Type == TileType.trees)
                    {
                        float radiusX = 0.5f;
                        float radiusZ = 0.1f;
                        //GenerateTreesAtPos(1, 0, 0, 0, 0); // test
                        GenerateTreesAtPos(8, -radiusX, radiusX, -radiusZ, radiusZ);
                    }
                    break;

                // VERT LINE
                case 2:
                case 4:
                case 6:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Vert"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_vert);
                    }
                    else if (Type == TileType.trees)
                    {
                        float radiusX = 0.1f;
                        float radiusZ = 0.5f;
                        //GenerateTreesAtPos(1, 0, 0, 0, 0); // test
                        GenerateTreesAtPos(8, -radiusX, radiusX, -radiusZ, radiusZ);
                    }
                    break;

                // UPLEFT CORNER
                case 12:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Up Left"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_upLeft);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateCorner(CornerType.upLeft, 5);
                    }
                    break;

                // UPRIGHT CORNER
                case 5:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Up Right"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_upRight);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateCorner(CornerType.upRight, 5);
                    }
                    break;

                // DOWNRIGHT CORNER
                case 3:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Down Right"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_downRight);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateCorner(CornerType.downRight, 5);
                    }
                    break;

                // DOWNLEFT CORNER
                case 10:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Down Left"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_downLeft);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateCorner(CornerType.downLeft, 5);
                    }
                    break;

                // RIGHT T
                case 7:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Right T"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_rightT);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateTreesAtPos(6, -0.5f, 0.5f, 0.2f, 0.2f); // horizontal
                        GenerateTreesAtPos(6, -0.5f, 0.0f, -0.5f, 0.5f); // vertical
                    }
                    break;

                // DOWN T
                case 11:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Down T"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_downT);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateTreesAtPos(6, -0.5f, 0.5f, 0.0f, 0.5f); // horizontal
                        GenerateTreesAtPos(6, -0.2f, 0.2f, -0.5f, 0.5f); // vertical
                    }
                    break;

                // LEFT T
                case 14:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Left T"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_leftT);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateTreesAtPos(6, -0.5f, 0.5f, 0.2f, 0.2f); // horizontal
                        GenerateTreesAtPos(6, 0.0f, 0.5f, -0.5f, 0.5f); // vertical
                    }
                    break;

                // UP T
                case 13:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - Up T"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_upT);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateTreesAtPos(6, -0.5f, 0.5f, -0.5f, 0.0f); // horizontal
                        GenerateTreesAtPos(6, -0.2f, 0.2f, -0.5f, 0.5f); // vertical
                    }
                    break;

                // ALL SIDES
                case 15:
                    if (Type == TileType.water)
                    {
                        if (displayDebug) { Debug.Log($"{xPos}, {zPos}: setting water texture - all"); }
                        this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", water_all);
                    }
                    else if (Type == TileType.trees)
                    {
                        GenerateTreesAtPos(12, -0.5f, 0.5f, -0.5f, 0.5f);
                    }
                    break;
            }
        }
    }

    public void SetDebugText(string text)
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = text;
    }



    void Awake()
    {
        seedID = "all";
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (displayDebug)
        {
            transform.Find("Debug").gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            transform.Find("Debug").gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }


    private enum CornerType
    {
        upLeft = 0,
        upRight,
        downLeft,
        downRight
    }
    private void GenerateCorner(CornerType corner, int amountPerLine)
    {
        if (corner == CornerType.downLeft)
        {
            GenerateTreesAtPos(amountPerLine, -0.5f, 0.5f, 0.0f, 0.5f); // horizontal
            GenerateTreesAtPos(amountPerLine, 0.0f, 0.5f, -0.5f, 0.5f); // vertical
        }
        else if (corner == CornerType.downRight)
        {
            GenerateTreesAtPos(amountPerLine, -0.5f, 0.5f, 0.0f, 0.5f); // horizontal
            GenerateTreesAtPos(amountPerLine, -0.5f, 0.0f, -0.5f, 0.5f); // vertical
        }
        else if (corner == CornerType.upLeft)
        {
            GenerateTreesAtPos(amountPerLine, -0.5f, 0.5f, -0.5f, 0.0f); // horizontal
            GenerateTreesAtPos(amountPerLine, 0.0f, 0.5f, -0.5f, 0.5f); // vertical
        }
        else if (corner == CornerType.upRight)
        {
            GenerateTreesAtPos(amountPerLine, -0.5f, 0.5f, -0.5f, 0.0f); // horizontal
            GenerateTreesAtPos(amountPerLine, -0.5f, 0.0f, -0.5f, 0.5f); // vertical
        }

    }


    public void GenerateTreesAtPos(int amount, float minX = 0, float maxX = 0, float minZ = 0, float maxZ = 0)
    {

        if (MyTrees.Count > 0)
        {
            ClearTrees();
        }

        Tree curTree;
        TreeType typeRoll;
        for (int i = 0; i < amount; i++)
        {
            typeRoll = (TreeType)numGen.Next((int)TreeType.tree1, (int)TreeType.tree3 + 1);
            switch (typeRoll)
            {
                case TreeType.tree1:
                    curTree = GameObject.Instantiate(tree1);
                    break;

                case TreeType.tree2:
                    curTree = GameObject.Instantiate(tree2);
                    break;

                case TreeType.tree3:
                    curTree = GameObject.Instantiate(tree3);
                    break;

                default:
                    curTree = GameObject.Instantiate(tree1);
                    break;
            }
            float ranX = ((float)numGen.NextDouble() * (maxX - minX)) + minX;
            float ranZ = ((float)numGen.NextDouble() * (maxZ - minZ)) + minZ;
            Vector3 newPos = new Vector3(transform.position.x + ranX, 1, transform.position.z + ranZ);
            if (displayDebug)
            {
                Debug.Log($"Setting tree for tile ({transform.position.x},{transform.position.z}) with ranX={ranX} and ranZ={ranZ}");
            }

            curTree.transform.position = newPos;
            MyTrees.Add(curTree);
        }
    }

    public void ClearTrees()
    {
        foreach (Tree tree in MyTrees)
        {
            Destroy(tree.gameObject);
        }
        MyTrees.Clear();
    }


    public void GenerateHouseOnTile()
    {

        if (MyHouses.Count > 0)
        {
            ClearHouses();
        }

        House curHouse = null;
        HouseType typeRoll;
        typeRoll = (HouseType)numGen.Next((int)HouseType.house1, (int)HouseType.house2 + 1);
        switch (typeRoll)
        {
            case HouseType.house1:
                curHouse = GameObject.Instantiate(house1);
                curHouse.transform.position = new Vector3(this.transform.position.x, 1.3f, this.transform.position.z);
                break;

            case HouseType.house2:
                curHouse = GameObject.Instantiate(house2);
                curHouse.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                break;

            default:
                break;
        }

        if (curHouse != null)
        {
            // Rotate house randomly:
            int rotDir = numGen.Next(0, 4);
            curHouse.transform.Rotate(new Vector3(0.0f, (float)(90 * rotDir), 0.0f), Space.Self);
            MyHouses.Add(curHouse);
        }
    }

    public void ClearHouses()
    {
        foreach (House house in MyHouses)
        {
            Destroy(house.gameObject);
        }
        MyHouses.Clear();
    }


}
