using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class ScenarioMapTests
    {
        private ScenarioMap scenarioMap;

        [SetUp]
        public void Setup()
        {
            scenarioMap = new ScenarioMap();
        }

        [Test]
        public void GetScenarioGrid_EmptyGrid_ReturnEmptyList()
        {
            List<List<ScenarioMazeElement>> _actual = scenarioMap.GetScenarioGrid(10, 10);

            Assert.AreEqual(0, _actual.Count);
        }


        [Test]
        public void GetScenarioGrid_HavingAnOAsScenario_ReturnOFields()
        {
            scenarioMap.SetScenarioString("111" +
                                        "101" +
                                        "111");

            List<List<ScenarioMazeElement>> _actual = scenarioMap.GetScenarioGrid(3, 3);

            Assert.AreEqual(3, _actual.Count);
        }


        [Test]
        public void GetScenarioGrid_HavingAnBigOAsScenario_ReturnBigOFields()
        {
            scenarioMap.SetScenarioString("11111" +
                                        "10001" +
                                        "10101" +
                                        "10001" +
                                        "11111");

            List<List<ScenarioMazeElement>> _actual = scenarioMap.GetScenarioGrid(5, 5);

            Assert.AreEqual(5, _actual.Count);
            Assert.AreEqual(ElementType.wall, _actual[0][0].elementType);
            Assert.AreEqual(ElementType.empty, _actual[1][1].elementType);
            Assert.AreEqual(ElementType.empty, _actual[1][2].elementType);
            Assert.AreEqual(ElementType.wall, _actual[2][2].elementType);
            Assert.AreEqual(ElementType.wall, _actual[4][4].elementType);
        }


        [Test]
        public void GetScenarioGrid_HavingAnIncompleteBigOAsScenario_ReturnBigOFieldsWithZeros()
        {
            scenarioMap.SetScenarioString("11111" +
                                            "10001" +
                                            "10101" +
                                            "10001" +
                                            "1");

            List<List<ScenarioMazeElement>> _actual = scenarioMap.GetScenarioGrid(5, 5);

            Assert.AreEqual(5, _actual.Count);
            Assert.AreEqual(ElementType.wall, _actual[0][0].elementType);
            Assert.AreEqual(ElementType.empty, _actual[1][1].elementType);
            Assert.AreEqual(ElementType.empty, _actual[1][2].elementType);
            Assert.AreEqual(ElementType.wall, _actual[2][2].elementType);
            Assert.AreEqual(ElementType.empty, _actual[4][3].elementType);
        }

        [Test]
        public void GetScenarioString_EmptyString_ReturnEmptyString()
        {
            string _actual = scenarioMap.GetScenarioString();

            Assert.AreEqual(string.Empty, _actual);
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase("12", "12")]
        [TestCase("1001", "1001")]
        [TestCase("", "")]
        [TestCase(null, "")]
        public void SetScenarioString_SetValueToString_ChangeCurrentScenarioString(string _newString, string _expectedString)
        {
            scenarioMap.SetScenarioString(_newString);

            string _actual = scenarioMap.GetScenarioString();
            string _expected = _expectedString;


            Assert.AreEqual(_expected, _actual);
        }


        [Test]
        [TestCase(1, 0, 0f)]
        [TestCase(0, 0, 0f)]
        [TestCase(0, 1, 0f)]
        [TestCase(-2, -6, 0f)]
        [TestCase(3, 0, -1f)]
        [TestCase(3, 1, 0f)]
        [TestCase(3, 2, 1f)]
        [TestCase(2, 0, -0.5f)]
        [TestCase(2, 1, 0.5f)]
        [TestCase(4, 0, -1.5f)]
        [TestCase(4, 2, 0.5f)]
        [TestCase(3, 0, -0.5f, .5f)]
        [TestCase(2, 0, -0.25f, .5f)]
        [TestCase(4, 0, -0.75f, .5f)]
        public void GetCenteredTilePositionByIndex_DistributeTiles_ReturnMirroredPosition(int _length,
                                                                                            int _index,
                                                                                            float _expectedResult,
                                                                                            float _distance=1)
        {

            float _actual = scenarioMap.GetCenteredTilePositionByIndex(_length, _index, _distance);
            float _expected = _expectedResult;


            Assert.AreEqual(_expected, _actual);
        }

    }
}
