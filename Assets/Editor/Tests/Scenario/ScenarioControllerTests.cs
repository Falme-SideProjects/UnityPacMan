using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScenarioControllerTests
    {
        private ScenarioController scenarioController;

        [SetUp]
        public void Setup()
        {
            scenarioController = new GameObject().AddComponent<ScenarioController>();
        }

        [Test]
        public void GetScenarioGrid_EmptyGrid_ReturnEmptyList()
        {
            List<List<char>> _actual = scenarioController.GetScenarioGrid(10,10);

            Assert.AreEqual(0, _actual.Count);
        }


        [Test]
        public void GetScenarioGrid_HavingAnOAsScenario_ReturnOFields()
        {
            scenarioController.SetScenarioString("111" +
                                                 "101" +
                                                 "111");

            List<List<char>> _actual = scenarioController.GetScenarioGrid(3, 3);

            Assert.AreEqual(3, _actual.Count);
        }


        [Test]
        public void GetScenarioGrid_HavingAnBigOAsScenario_ReturnBigOFields()
        {
            scenarioController.SetScenarioString("11111" +
                                                 "10001" +
                                                 "10101" +
                                                 "10001" +
                                                 "11111");

            List<List<char>> _actual = scenarioController.GetScenarioGrid(5, 5);

            Assert.AreEqual(5, _actual.Count);
            Assert.AreEqual('1', _actual[0][0]);
            Assert.AreEqual('0', _actual[1][1]);
            Assert.AreEqual('0', _actual[1][2]);
            Assert.AreEqual('1', _actual[2][2]);
            Assert.AreEqual('1', _actual[4][4]);
        }


        [Test]
        public void GetScenarioGrid_HavingAnIncompleteBigOAsScenario_ReturnBigOFieldsWithZeros()
        {
            scenarioController.SetScenarioString("11111" +
                                                 "10001" +
                                                 "10101" +
                                                 "10001" +
                                                 "1");

            List<List<char>> _actual = scenarioController.GetScenarioGrid(5, 5);

            Assert.AreEqual(5, _actual.Count);
            Assert.AreEqual('1', _actual[0][0]);
            Assert.AreEqual('0', _actual[1][1]);
            Assert.AreEqual('0', _actual[1][2]);
            Assert.AreEqual('1', _actual[2][2]);
            Assert.AreEqual('0', _actual[4][3]);
        }

        [Test]
        public void GetScenarioString_EmptyString_ReturnEmptyString()
        {
            string _actual = scenarioController.GetScenarioString();

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
            scenarioController.SetScenarioString(_newString);

            string _actual = scenarioController.GetScenarioString();
            string _expected = _expectedString;


            Assert.AreEqual(_expected, _actual);
        }
    }
}
