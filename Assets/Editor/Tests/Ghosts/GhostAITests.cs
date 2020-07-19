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
                    new ScenarioMazeElement(ElementType.empty),
                    new ScenarioMazeElement(ElementType.empty),
                    new ScenarioMazeElement(ElementType.empty),
                    new ScenarioMazeElement(ElementType.empty)
                });
            }

            return dummyGrid;
        }

    }
}
