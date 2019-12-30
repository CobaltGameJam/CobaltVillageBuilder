using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TileGenerator : MonoBehaviour
    {
        #region CONSTANTS
        public const int SHORT_DELAY = 10;
        public const int MEDIUM_DELAY = 30;
        public const int LONG_DELAY = 60;
        #endregion

        public System.Collections.IEnumerator PaintTiles()
        {
            // Your world here
            // Example:
            SetTileType(2, 2, Tile.TileType.house);
            SetTileType(1, 3, Tile.TileType.trees);
            SetTileType(1, 2, Tile.TileType.trees);
            SetTileType(1, 3, Tile.TileType.trees);
            SetTileType(2, 3, Tile.TileType.water);
            SetTileType(1, 2, Tile.TileType.flowers);

            yield return Wait(0); // don't delete me
        }





        #region Custom Shapes
        public void PaintCross(int xPos, int zPos, Tile.TileType type)
        {
            SetTileType(xPos - 1, zPos, type);
            SetTileType(xPos, zPos, type);
            SetTileType(xPos + 1, zPos, type);
            SetTileType(xPos, zPos - 1, type);
            SetTileType(xPos, zPos + 1, type);
        }

        public void PaintSquare(int xPos, int zPos, Tile.TileType type)
        {
            SetTileType(xPos, zPos, type);
            SetTileType(xPos + 1, zPos, type);
            SetTileType(xPos, zPos + 1, type);
            SetTileType(xPos + 1, zPos + 1, type);
        }
        #endregion



        #region Interface
        private void SetTileType(int xPos, int zPos, Tile.TileType type)
        {
            Tile curTile = WorldTiles[zPos][xPos];
            curTile.UpdateTileType(type);
        }

        public void SetRandomTile(Tile.TileType type)
        {
            int xPos = numGen.Next(0, WorldWidth);
            int yPos = numGen.Next(0, WorldHeight);
            SetTileType(xPos, yPos, type);
        }


        private System.Collections.IEnumerator Wait(int milliseconds)
        {
            if (milliseconds > 0)
            {
                yield return new WaitForSeconds(((float)milliseconds)/1000);
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }

        public bool IsEven(int i)
        {
            return (i % 2 == 0);
        }
        public bool IsOdd(int i)
        {
            return (i % 2 == 1);
        }
        #endregion



        #region DEMO
        public System.Collections.IEnumerator PaintDemo()
        {
            // Forest in four corners:
            SeedTilePos(0, 0, Tile.TileType.trees, "BL");
            SeedTilePos(WorldWidth - 1, WorldHeight - 1, Tile.TileType.trees, "TR");
            SeedTilePos(0, WorldHeight - 1, Tile.TileType.trees, "TL");
            SeedTilePos(WorldWidth - 1, 0, Tile.TileType.trees, "BR");
            for (int i = 0; i < 10; i++)
            {
                GrowSeeds(Tile.TileType.trees, "TL", 0, 20, 80, 20, 20);
                GrowSeeds(Tile.TileType.trees, "TR", 0, 20, 80, 20, 20);
                GrowSeeds(Tile.TileType.trees, "BL", 0, 80, 20, 40, 10);
                GrowSeeds(Tile.TileType.trees, "BR", 0, 80, 20, 10, 40);
                yield return Wait(SHORT_DELAY);
            }

            // River down middle
            SeedTilePos(10, 5, Tile.TileType.water, "river1");
            SeedTilePos(10, 10, Tile.TileType.water, "river1");
            SeedTilePos(10, 15, Tile.TileType.water, "river1");
            for (int i = 0; i < 11; i++)
            {
                GrowSeeds(Tile.TileType.water, "river1", 0, 85, 85, 5, 5);
                yield return Wait(MEDIUM_DELAY);
            }

            // Housing settlements
            for (int i = 0; i < 4; i++)
            {
                SeedPosRandom(Tile.TileType.house, $"all houses");
            }
            for (int j = 0; j < 8; j++)
            {
                yield return Wait(SHORT_DELAY);
                GrowSeeds(Tile.TileType.house, $"all houses", 25);
            }

            // Flowers
            for (int i = 0; i < 12; i++)
            {
                SeedPosRandom(Tile.TileType.flowers, "flowers");
            }
            for (int i = 0; i < 20; i++)
            {
                GrowAllFlowers();
                if (i % 3 == 0) { yield return Wait(SHORT_DELAY); }
            }

            yield return Wait(0);
        }

        public System.Collections.IEnumerator RandomDemo()
        {
            int forestChance = 65;
            if (numGen.Next(0, 100) > forestChance)
            {
                int numSeeds = numGen.Next(1, 5);
                for (int i = 0; i <= numSeeds; i++)
                {
                    SeedPosRandom(Tile.TileType.trees, $"forest{i}");
                    int forestDepth = numGen.Next(1, 6);
                    for (int j = 0; j <= forestDepth; j++)
                    {
                        GrowSeeds(Tile.TileType.trees, $"forest{i}");
                    }

                    yield return Wait(SHORT_DELAY);
                }
            }

            int riverChance = 15;
            if (numGen.Next(0, 100) > riverChance)
            {
                int startX = numGen.Next(1, WorldWidth - 1);
                int startZ = numGen.Next(1, WorldHeight - 1);
                SeedTilePos(startX, startZ, Tile.TileType.water, "river");

                int vertGrowChance = 30;
                int horzGrowChance = 30;
                if (numGen.Next() % 2 == 0) // horizontal
                {
                    int leftX = startX / 2;
                    SeedTilePos(leftX, startZ, Tile.TileType.water, "river");
                    int rightX = (startX + (WorldWidth - 1)) / 2;
                    SeedTilePos(rightX, startZ, Tile.TileType.water, "river");
                    vertGrowChance = 5;
                    horzGrowChance = 70;
                }
                else // Vertical
                {
                    int belowZ = startZ / 2;
                    SeedTilePos(startX, belowZ, Tile.TileType.water, "river");
                    int aboveZ = (startZ + (WorldHeight - 1)) / 2;
                    SeedTilePos(startX, aboveZ, Tile.TileType.water, "river");
                    vertGrowChance = 70;
                    horzGrowChance = 5;
                }

                for (int i = 0; i < WorldWidth/2; i++)
                {
                    GrowSeeds(Tile.TileType.water, "river", 0, vertGrowChance, vertGrowChance, horzGrowChance, horzGrowChance);
                    yield return Wait(SHORT_DELAY);
                }
            }

            int pondChance = 45;
            if (numGen.Next(0, 100) > pondChance)
            {
                int numPonds = numGen.Next(1, 4);
                for (int i = 0; i < numPonds; i++)
                {
                    yield return Wait(SHORT_DELAY);
                    SeedPosRandom(Tile.TileType.water, "pond");
                }
                int pondSize = numGen.Next(2, 8);
                for (int i = 0; i < pondSize; i++)
                {
                    yield return Wait(SHORT_DELAY);
                    GrowSeeds(Tile.TileType.water, "pond");
                }
            }

            // Housing settlements
            int numHouses = numGen.Next(0, 7);
            for (int i = 0; i < numHouses; i++)
            {
                SeedPosRandom(Tile.TileType.house, $"all houses");
            }

            int houseSize = (numGen.Next(1, 10));
            for (int j = 0; j < houseSize; j++)
            {
                yield return Wait(SHORT_DELAY);
                GrowSeeds(Tile.TileType.house, $"all houses", 25);
            }


            // Flowers
            for (int i = 0; i < 12; i++)
            {
                SeedPosRandom(Tile.TileType.flowers, "flowers");
            }
            int flowerSize = numGen.Next(3, 15);
            for (int i = 0; i < flowerSize; i++)
            {
                GrowAllFlowers();
                if (i % 3 == 0) { yield return Wait(SHORT_DELAY); }
            }

            yield return Wait(0);
        }
        #endregion


        #region Unity Configurable Properties
        public int WorldWidth;
        public int WorldHeight;
        public int TileDimension;
        public Tile tile;
        public bool displayDebug;
        #endregion

        #region Internal Properties
        private DebugTileMode debugTileMode { get; set; }
        private Tile[][] WorldTiles;
        private System.Random numGen = new System.Random();
        #endregion




        #region Internal Methods
        private Tile GenerateTileAt(int xPos, int zPos)
        {
            Tile curTile = GameObject.Instantiate(tile);
            float yPos = 1 + ((float)(xPos % 2) / 100) + ((float)(zPos % 2) / 100);
            curTile.transform.position = new Vector3(xPos * TileDimension, yPos, zPos * TileDimension);
            curTile.xPos = xPos;
            curTile.zPos = zPos;
            curTile.displayDebug = displayDebug;
            return curTile;
        }

        public void GridInit()
        {
            WorldTiles = new Tile[WorldHeight][];
            for (int i = 0; i < WorldHeight; i++)
            {
                WorldTiles[i] = new Tile[WorldWidth];
            }
        }

        public void InstantiateGrid(Tile.TileType type = Tile.TileType.grass)
        {

            GridInit();

            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = GenerateTileAt(col, row);
                    curTile.UpdateTileType(type);
                    WorldTiles[row][col] = curTile;

                }
            }

            // Need to do AFTER generating all the tiles
            SetNeighbors();
        }

        private void SetNeighbors()
        {

            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];

                    // below
                    if (row > 0)
                    {
                        curTile.TileBelow = WorldTiles[row - 1][col];
                    }
                    else { curTile.TileBelow = null; }

                    // left
                    if (col > 0)
                    {
                        curTile.TileLeft = WorldTiles[row][col - 1];
                    }
                    else { curTile.TileLeft = null; }

                    // above
                    if (row < WorldHeight - 1)
                    {
                        curTile.TileAbove = WorldTiles[row + 1][col];
                    }
                    else { curTile.TileAbove = null; }

                    // right
                    if (col < WorldWidth - 1)
                    {
                        curTile.TileRight = WorldTiles[row][col + 1];
                    }
                    else { curTile.TileRight = null; }
                }
            }
        }

        public void ClearTiles()
        {
            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];
                    curTile.UpdateTileType(Tile.TileType.grass);
                    curTile.seedID = "all";
                    curTile.ClearTrees();
                }
            }
            ClearAllBuildings();
            SetNeighbors();
        }

        public void ResetAllShapeableTiles()
        {
            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];
                    curTile.ResetTileShape();
                }
            }
        }

        public void GenerateAllBuildings()
        {
            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];
                    if (curTile.Type == Tile.TileType.house)
                    {
                        curTile.GenerateHouseOnTile();
                    }
                }
            }
        }

        public void ClearAllBuildings()
        {
            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];
                    curTile.ClearHouses();
                }
            }
        }

        public void SetTileDebugText(DebugTileMode mode)
        {
            Debug.Log($"Setting all tiles to type: {mode}");
            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];
                    curTile.SetTileDebug(mode, displayDebug);
                    debugTileMode = mode;
                }
            }
        }

        public System.Collections.IEnumerator GenerateWorld()
        {
            yield return StartCoroutine(PaintTiles());
            ResetAllShapeableTiles();
            GenerateAllBuildings();
        }

        public void Awake()
        {
            InstantiateGrid();
        }

        public void ResetWorldAsync()
        {
            StartCoroutine(ResetWorld());
        }

        public void DemoAsync()
        {
            ClearWorld();
            StartCoroutine(PaintDemo());
        }

        public void DemoRandAsync()
        {
            ClearWorld();
            StartCoroutine(RandomDemo());
        }

        private System.Collections.IEnumerator ResetWorld()
        {
            ClearTiles();
            yield return StartCoroutine(GenerateWorld());
            SetTileDebugText(debugTileMode);
        }

        public void ClearWorld()
        {
            ClearTiles();
        }

        public void Update()
        {
            if (Input.GetKey("r"))
            {
                ResetWorld();
            }
        }
        #endregion


        #region Seeding
        public void GrowAllWater() { GrowSeeds(Tile.TileType.water, "all"); }
        public void GrowAllTrees() { GrowSeeds(Tile.TileType.trees, "all"); }
        public void GrowAllFlowers() { GrowSeeds(Tile.TileType.flowers, "all"); }
        public void GrowAllHouses() { GrowSeeds(Tile.TileType.house, "all"); }

        public void SeedRandomWater() { SeedPosRandom(Tile.TileType.water, "all"); }
        public void SeedRandomTree() { SeedPosRandom(Tile.TileType.trees, "all"); }
        public void SeedRandomFlower() { SeedPosRandom(Tile.TileType.flowers, "all"); }
        public void SeedRandomHouse() { SeedPosRandom(Tile.TileType.house, "all"); }

        public void SeedPosRandom(Tile.TileType type, string seedID = "all")
        {
            int posX = numGen.Next(0, WorldWidth);
            int posZ = numGen.Next(0, WorldHeight);

            SeedTilePos(posX, posZ, type, seedID);
        }


        public void SeedTilePos(int posX, int posZ, Tile.TileType type, string seedID = "all")
        {
            Tile curTile = WorldTiles[posZ][posX];
            if (SeedTile(curTile, seedID, type))
            {
                if (type == Tile.TileType.house) { curTile.GenerateHouseOnTile(); }
                if (curTile.IsShapeable) { curTile.ResetTileShape(); }
            }
        }

        /// <summary>
        /// Grows a seed of a certain tile type, and of a certain seed id (optional)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="seedID"></param>
        /// <param name="percentChance"> This is the chance of any direction. This is overridden by the optional next parameters</param>
        /// <param name="percTop">Overrides the percentChance on the top checks only</param>
        /// <param name="percBottom">Overrides the percentChance on the bottom checks only</param>
        /// <param name="percRight">Overrides the percentChance on the right checks only</param>
        /// <param name="percLeft">Overrides the percentChance on the left checks only</param>
        public void GrowSeeds(Tile.TileType type, string seedID = "all", int percentChance = 35, int percTop = -1, int percBottom = -1, int percRight = -1, int percLeft = -1)
        {

            List<Tile> GrownTiles = new List<Tile>();

            // (columns) Top To Bottom:
            for (int row = 0; row < WorldHeight; row++)
            {
                for (int col = 0; col < WorldWidth; col++)
                {
                    Tile curTile = WorldTiles[row][col];
                    if (curTile.Type == type && checkSeedID(curTile, seedID) && !GrownTiles.Contains(curTile))
                    {
                        GrownTiles.AddRange(scanAndGrowTile(curTile, percentChance, percTop, percBottom, percRight, percLeft));
                        GrownTiles.Add(curTile);
                    }
                }
            }

            // (columns) Bottom to Top:
            for (int row = WorldHeight - 1; row >= 0; row--)
            {
                for (int col = WorldWidth - 1; col >= 0; col--)
                {
                    Tile curTile = WorldTiles[row][col];
                    if (curTile.Type == type && checkSeedID(curTile, seedID) && !GrownTiles.Contains(curTile))
                    {
                        GrownTiles.AddRange(scanAndGrowTile(curTile, percentChance, percTop, percBottom, percRight, percLeft));
                        GrownTiles.Add(curTile);
                    }
                }
            }

            // (rows) Left to Right:
            for (int col = 0; col < WorldWidth; col++)
            {
                for (int row = 0; row < WorldHeight; row++)
                {
                    Tile curTile = WorldTiles[row][col];
                    if (curTile.Type == type && checkSeedID(curTile, seedID) && !GrownTiles.Contains(curTile))
                    {
                        GrownTiles.AddRange(scanAndGrowTile(curTile, percentChance, percTop, percBottom, percRight, percLeft));
                        GrownTiles.Add(curTile);
                    }
                }
            }

            // (rows) Right to Left:
            for (int col = WorldWidth - 1; col >= 0; col--)
            {
                for (int row = WorldHeight - 1; row >= 0; row--)
                {
                    Tile curTile = WorldTiles[row][col];
                    if (curTile.Type == type && checkSeedID(curTile, seedID) && !GrownTiles.Contains(curTile))
                    {
                        GrownTiles.AddRange(scanAndGrowTile(curTile, percentChance, percTop, percBottom, percRight, percLeft));
                        GrownTiles.Add(curTile);
                    }
                }
            }
            GrownTiles.Clear();
            ResetAllShapeableTiles();
            GenerateAllBuildings();
            SetTileDebugText(debugTileMode);
        }

        private bool SeedTile(Tile target, string seedID, Tile.TileType type)
        {

            if (target.Type == Tile.TileType.house) { return false; } // never overwrite houses
            if (target.Type == Tile.TileType.water) { return false; } // never overwrite water

            // Flowers don't override, they grow nicely:
            if (type == Tile.TileType.flowers)
            {
                if (target.Type == Tile.TileType.grass)
                {
                    target.UpdateTileType(type);
                    target.seedID = seedID;
                }
                else return false;
            }
            else
            {
                if (target.Type == Tile.TileType.trees) { target.ClearTrees(); }
                else if (target.Type == Tile.TileType.house) { target.ClearHouses(); }

                target.UpdateTileType(type);
                target.seedID = seedID;
            }
            return true;
        }

        public bool checkSeedID(Tile target, string checkId)
        {
            if (checkId == "all" || target.seedID == checkId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        ///  Returns tiles that were generated
        /// </summary>
        /// <param name="originTile"></param>
        /// <param name="percentChance"></param>
        /// <param name="percAbove"></param>
        /// <param name="percBelow"></param>
        /// <param name="percRight"></param>
        /// <param name="percLeft"></param>
        /// <returns></returns>
        public List<Tile> scanAndGrowTile(Tile originTile, int percentChance = 25, int percAbove = -1, int percBelow = -1, int percRight = -1, int percLeft = -1)
        {
            List<Tile> tilesSeeded = new List<Tile>();

            // Above
            if (originTile.TileAbove != null)
            {
                int roll = numGen.Next(0, 101);
                if (percAbove > -1 && roll >= (100 - percAbove))
                {
                    SeedTile(originTile.TileAbove, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileAbove);
                }
                else if (roll >= 100 - percentChance)
                {
                    SeedTile(originTile.TileAbove, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileAbove);
                }
            }

            // Right
            if (originTile.TileRight != null)
            {
                int roll = numGen.Next(0, 101);
                if (percRight > -1 && roll >= (100 - percRight))
                {
                    SeedTile(originTile.TileRight, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileRight);
                }
                else if (roll >= 100 - percentChance)
                {
                    SeedTile(originTile.TileRight, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileRight);
                }
            }

            // Below
            if (originTile.TileBelow != null)
            {
                int roll = numGen.Next(0, 101);
                if (percBelow > -1 && roll >= (100 - percBelow))
                {
                    SeedTile(originTile.TileBelow, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileBelow);
                }
                else if (roll >= 100 - percentChance)
                {
                    SeedTile(originTile.TileBelow, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileBelow);
                }
            }

            // Left
            if (originTile.TileLeft != null)
            {
                int roll = numGen.Next(0, 101);
                if (percLeft > -1 && roll >= (100 - percLeft))
                {
                    SeedTile(originTile.TileLeft, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileLeft);
                }
                else if (roll >= 100 - percentChance)
                {
                    SeedTile(originTile.TileLeft, originTile.seedID, originTile.Type);
                    tilesSeeded.Add(originTile.TileLeft);
                }
            }

            return tilesSeeded;
        }

        #endregion




    }
}
