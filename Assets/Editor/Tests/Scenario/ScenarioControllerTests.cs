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

    }
}
