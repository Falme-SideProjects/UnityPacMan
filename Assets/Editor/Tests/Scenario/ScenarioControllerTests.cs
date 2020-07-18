using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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

        /*private List<List<ScenarioMazeElement>> GetDummyGrid()
        {
            List<List<ScenarioMazeElement>> _list = new List<List<ScenarioMazeElement>>();

            _list.Add(new List<ScenarioMazeElement>());
            _list.Add(new List<ScenarioMazeElement>());


            _list[0].Add(new ScenarioMazeElement() { elementSpriteRenderer = new Spr });

        }

        [Test]
        public void */

    }
}
