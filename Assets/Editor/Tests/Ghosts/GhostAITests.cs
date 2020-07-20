using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GhostAITests
    {
        private GhostAI ghostAI;

        [SetUp]
        public void Setup()
        {
            ghostAI = new GhostAI();
        }

        [Test]
        public void GetGhostCurrentState_DidNotChangedState_ReturnChaseState()
        {
            GhostState _actual = ghostAI.GetGhostCurrentState();
            Assert.AreEqual(GhostState.chase, _actual);
        }

        [Test]
        [TestCase(GhostState.chase, GhostState.chase)]
        [TestCase(GhostState.eaten, GhostState.eaten)]
        [TestCase(GhostState.frightened, GhostState.frightened)]
        [TestCase(GhostState.scatter, GhostState.scatter)]
        public void SetGhostCurrentState_ChangeState_ReturnSettedState(GhostState _newState,
                                                                        GhostState _expected)
        {
            ghostAI.SetGhostCurrentState(_newState);

            GhostState _actual = ghostAI.GetGhostCurrentState();
            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        public void GetGhostType_DidNotChangeGhost_ReturnBlinky()
        {
            Assert.AreEqual(GhostType.blinky, ghostAI.GetGhostType());
        }

        [Test]
        [TestCase(GhostType.blinky, GhostType.blinky)]
        [TestCase(GhostType.clyde, GhostType.clyde)]
        [TestCase(GhostType.inky, GhostType.inky)]
        [TestCase(GhostType.pinky, GhostType.pinky)]
        public void SetGhostType_ChangedGhostType_ReturnSettedGhostType(GhostType _newType,
                                                                        GhostType _expected)
        {
            ghostAI.SetGhostType(_newType);

            GhostType _actual = ghostAI.GetGhostType();
            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        public void GetScenarioGrid_DidNotChangedTable_ReturnEmptyTable()
        {
            List<List<ScenarioMazeElement>> _scenarioGrid = ghostAI.GetScenarioGrid();

            Assert.AreEqual(0, _scenarioGrid.Count);
        }


        [Test]
        public void SetScenarioGrid_SetGameScenarioGrid_ReturnSettedGrid()
        {
            List<List<ScenarioMazeElement>> dummyGrid = CreateDummyGrid();

            ghostAI.SetScenarioGrid(dummyGrid);
            List<List<ScenarioMazeElement>> _scenarioGrid = ghostAI.GetScenarioGrid();

            Assert.AreEqual(3, _scenarioGrid.Count);
        }

        [Test]
        public void GetCurrentTarget_DidNotChangeTarget_ReturnTargetForSpecificGhost()
        {
            Vector2 _actual = ghostAI.GetCurrentTarget();

            Assert.AreEqual(new Vector2(0, 0), _actual);
        }

        [Test]
        [TestCase(GhostType.pinky, 0, 0)]
        [TestCase(GhostType.blinky, 3, 0)]
        [TestCase(GhostType.clyde, 0, 2)]
        [TestCase(GhostType.inky, 3, 2)]
        public void SetCurrentTarget_ScatterState_ReturnTargetForSpecificGhost(GhostType _ghostType,
                                                                                int _expectedX,
                                                                                int _expectedY)
        {

            List<List<ScenarioMazeElement>> dummyGrid = CreateDummyGrid();

            ghostAI.SetScenarioGrid(dummyGrid);
            ghostAI.SetGhostType(_ghostType);
            ghostAI.SetGhostCurrentState(GhostState.scatter);

            ghostAI.SetCurrentTarget();

            Vector2 _actual = ghostAI.GetCurrentTarget();

            Assert.AreEqual(new Vector2(_expectedX, _expectedY), _actual);
        }

        [Test]
        [TestCase(0,0,0,0, 0f)]
        [TestCase(0, 0, 1, 0, 1f)]
        [TestCase(0, 0, 0, 1, 1f)]
        [TestCase(0, 0, 1, 1, 1.4142f)]
        public void GetDistanceBetweenTiles_SetGridWithPositions_ReturnsDistance(int _originX,
                                                                                 int _originY,
                                                                                 int _destinyX,
                                                                                 int _destinyY,
                                                                                 float _expected)
        {
            List<List<ScenarioMazeElement>> dummyGrid = CreateDummyGrid();

            ghostAI.SetScenarioGrid(dummyGrid);

            float _actual = ghostAI.GetDistanceBetweenTiles(new Vector2(_originX, _originY), new Vector2(_destinyX, _destinyY));

            Assert.AreEqual(_expected, _actual, 0.01f);

        }

        [Test]
        [TestCase(0, 0, 0f)]
        [TestCase(1, 0, 1f)]
        [TestCase(0, 1, 1f)]
        [TestCase(1, 1, 1.4142f)]
        public void GetDistanceBetweenTileToTarget_SetGridWithPositions_ReturnsDistance(int _originX,
                                                                                 int _originY,
                                                                                 float _expected)
        {
            List<List<ScenarioMazeElement>> dummyGrid = CreateDummyGrid();

            ghostAI.SetScenarioGrid(dummyGrid);
            ghostAI.SetGhostType(GhostType.pinky);
            ghostAI.SetGhostCurrentState(GhostState.scatter);

            float _actual = ghostAI.GetDistanceBetweenTileToTarget(new Vector2(_originX, _originY));

            Assert.AreEqual(_expected, _actual, 0.01f);
        }

        [Test]
        [TestCase(1,1, Direction.up, GhostState.scatter, 1)]
        [TestCase(6, 1, Direction.right, GhostState.scatter, 2)]
        [TestCase(6, 5, Direction.down, GhostState.scatter, 3)]
        [TestCase(6, 5, Direction.down, GhostState.scatter, 3)]
        [TestCase(0, 14, Direction.left, GhostState.scatter, 1)]
        [TestCase(0, 14, Direction.left, GhostState.chase, 1)]
        [TestCase(1, 14, Direction.right, GhostState.scatter, 1)]
        [TestCase(26, 14, Direction.left, GhostState.chase, 1)]
        [TestCase(27, 14, Direction.right, GhostState.chase, 1)]
        public void GetPermittedDirections_SetGridWithPositions_ReturnArrayDirections(int _posX,
                                                                                      int _posY,
                                                                                      Direction _currentDirection,
                                                                                      GhostState _ghostState,
                                                                                      int _expectedLength)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetScenarioGrid(dummyGrid);

            List<Direction> directions = ghostAI.GetPermittedDirections(new Vector2(_posX, _posY), _currentDirection);

            for (int a = 0; a < directions.Count; a++) Debug.Log(directions[a].ToString());

            Assert.AreEqual(_expectedLength, directions.Count);
        }

        [Test]
        [TestCase(1, 0, Direction.left, false)]
        [TestCase(1, 0, Direction.right, false)]
        [TestCase(1, 14, Direction.left, false)]
        [TestCase(1, 14, Direction.right, false)]
        [TestCase(0, 14, Direction.left, true)]
        [TestCase(0, 14, Direction.right, false)]
        [TestCase(27, 14, Direction.left, false)]
        [TestCase(28, 14, Direction.right, false)]
        public void IsOnEdge_WhenGhostInExtremeMiddleLeftRight_ReturnTrue(int _x,
                                                                          int _y,
                                                                          Direction _direction,
                                                                          bool _expected)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetScenarioGrid(dummyGrid);

            bool _actual = ghostAI.IsOnEdge(_x, _y, _direction);

            Assert.AreEqual(_expected, _actual);
        }


        [Test]
        [TestCase(13, 11, Direction.up, GhostState.scatter, 2)]
        [TestCase(13, 11, Direction.right, GhostState.scatter, 1)]
        [TestCase(14, 11, Direction.left, GhostState.scatter, 1)]
        [TestCase(14, 11, Direction.up, GhostState.scatter, 2)]
        public void GetPermittedDirections_DoNotReturnToBox_ReturnArrayDirections(int _posX,
                                                                                      int _posY,
                                                                                      Direction _currentDirection,
                                                                                      GhostState _ghostState,
                                                                                      int _expectedLength)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetScenarioGrid(dummyGrid);

            List<Direction> directions = ghostAI.GetPermittedDirections(new Vector2(_posX, _posY), _currentDirection);

            Assert.AreEqual(_expectedLength, directions.Count);
        }

        [Test]
        [TestCase(13, 11, false)]
        [TestCase(13, 13, true)]
        [TestCase(16, 15, true)]
        [TestCase(11, 13, true)]
        [TestCase(11, 12, true)]
        public void IsGhostInBox_CheckGhostInBoxByPosition_ReturnBoolean(int _posX,
                                                                         int _posY,
                                                                         bool _expected)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetScenarioGrid(dummyGrid);

            bool _actual = ghostAI.IsGhostInBox(new Vector2(_posX, _posY));

            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        [TestCase(1, 2, GhostType.pinky, Direction.up, Direction.up)]
        [TestCase(1, 1, GhostType.pinky, Direction.up, Direction.right)]
        [TestCase(2, 1, GhostType.pinky, Direction.right, Direction.right)]
        [TestCase(3, 1, GhostType.pinky, Direction.right, Direction.right)]
        [TestCase(9, 11, GhostType.pinky, Direction.left, Direction.down)]
        [TestCase(10, 11, GhostType.pinky, Direction.left, Direction.left)]
        [TestCase(15, 15, GhostType.inky, Direction.right, Direction.right)]
        [TestCase(16, 15, GhostType.inky, Direction.right, Direction.up)]
        [TestCase(6, 1, GhostType.pinky, Direction.left, Direction.left)]
        [TestCase(6, 1, GhostType.inky, Direction.left, Direction.down)]
        public void GetNextNearestMove_FollowTargetScatter_ReturnDirection(int _posX,
                                                                            int _posY,
                                                                            GhostType _ghostType,
                                                                            Direction _currentDirection,
                                                                            Direction _expected)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetScenarioGrid(dummyGrid);
            ghostAI.SetGhostType(_ghostType);
            ghostAI.SetGhostCurrentState(GhostState.scatter);
            ghostAI.SetCurrentTarget();

            Direction _actual = ghostAI.GetNextNearestMove(new Vector2(_posX, _posY), _currentDirection);

            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        [TestCase(14, 14, GhostType.inky, Direction.up, Direction.up)]
        [TestCase(13, 14, GhostType.inky, Direction.left, Direction.up)]
        [TestCase(13, 13, GhostType.inky, Direction.up, Direction.up)]
        [TestCase(13, 12, GhostType.inky, Direction.up, Direction.up)]
        public void GetNextNearestMove_GetOutOfBox_ReturnDirection(int _posX,
                                                                            int _posY,
                                                                            GhostType _ghostType,
                                                                            Direction _currentDirection,
                                                                            Direction _expected)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetGhostPosition(new Vector2(_posX, _posY));
            ghostAI.SetScenarioGrid(dummyGrid);
            ghostAI.SetGhostType(_ghostType);
            ghostAI.SetGhostCurrentState(GhostState.scatter);
            ghostAI.SetCurrentTarget();

            Debug.Log(ghostAI.GetCurrentTarget());

            Direction _actual = ghostAI.GetNextNearestMove(new Vector2(_posX, _posY), _currentDirection);

            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        [TestCase(1, 2, Direction.up, 1, 1)]
        [TestCase(1, 1, Direction.right, 2, 1)]
        [TestCase(1, 1, Direction.left, 0, 1)]
        [TestCase(1, 1, Direction.down, 1, 2)]
        public void GetTilePositionBasedOnDirection_SettedDirectionAndGrid_ReturnVector2(int _posX,
                                                                                         int _posY,
                                                                                         Direction direction,
                                                                                         float _expectedX,
                                                                                         float _expectedY)
        {
            List<List<ScenarioMazeElement>> dummyGrid = CreateDummyGrid();

            ghostAI.SetScenarioGrid(dummyGrid);

            Vector2 _actual = ghostAI.GetTilePositionBasedOnDirection(new Vector2(_posX,_posY), direction);

            Assert.AreEqual(new Vector2(_expectedX, _expectedY), _actual);
        }

        [Test]
        [TestCase(0, 14, Direction.left, 27, 14)]
        [TestCase(27, 14, Direction.right, 0, 14)]
        public void GetTilePositionBasedOnDirection_SettedDirectionAndFacadeGrid_ReturnVector2(int _posX,
                                                                                         int _posY,
                                                                                         Direction direction,
                                                                                         float _expectedX,
                                                                                         float _expectedY)
        {
            List<List<ScenarioMazeElement>> dummyGrid = GetFacadeMap();

            ghostAI.SetScenarioGrid(dummyGrid);

            Debug.Log(dummyGrid[0].Count - 1);

            Vector2 _actual = ghostAI.GetTilePositionBasedOnDirection(new Vector2(_posX, _posY), direction);

            Assert.AreEqual(new Vector2(_expectedX, _expectedY), _actual);
        }



        /// --------------
        /// Local Functions
        /// --------------

        private List<List<ScenarioMazeElement>> CreateDummyGrid()
        {
            List<List<ScenarioMazeElement>> dummyGrid = new List<List<ScenarioMazeElement>>();

            for(int a=0; a<3;a++)
            {
                dummyGrid.Add(new List<ScenarioMazeElement>()
                {
                    new ScenarioMazeElement(ElementType.empty) { elementPositionInWorld = new Vector2(0, a) },
                    new ScenarioMazeElement(ElementType.empty) { elementPositionInWorld = new Vector2(1, a) },
                    new ScenarioMazeElement(ElementType.empty) { elementPositionInWorld = new Vector2(2, a) },
                    new ScenarioMazeElement(ElementType.empty) { elementPositionInWorld = new Vector2(3, a) }
                });
            }

            return dummyGrid;
        }

        private List<List<ScenarioMazeElement>> GetFacadeMap()
        {
            ScenarioMap scenarioMap = new ScenarioMap();
            TextAsset mapText = (TextAsset)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Scripts/Data/LevelMap/Map.txt", typeof(TextAsset));
            string inlineMap = System.Text.RegularExpressions.Regex.Replace(mapText.text, @"\t|\n|\r", "");
            scenarioMap.SetScenarioString(inlineMap);
            return scenarioMap.GetScenarioGrid(28, 31);
        }


    }
}
