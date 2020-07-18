using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GhostMovimentationTests
    {
        private GhostMovimentation ghostMovimentation;

        [SetUp]
        public void Setup()
        {
            ghostMovimentation = new GhostMovimentation();
        }

        [Test]
        public void GetCurrentDirection_DoNotModified_ReturnUp()
        {
            Assert.AreEqual(Direction.up, ghostMovimentation.GetCurrentDirection());
        }

        [Test]
        [TestCase(Direction.down, Direction.down)]
        [TestCase(Direction.up, Direction.up)]
        [TestCase(Direction.right, Direction.right)]
        [TestCase(Direction.left, Direction.left)]
        public void SetCurrentDirection_ModifyDirection_SetNewDirection(Direction _newDirection,
                                                                        Direction _expectedDirection)
        {
            ghostMovimentation.SetCurrentDirection(_newDirection);

            Direction _actual = ghostMovimentation.GetCurrentDirection();
            Assert.AreEqual(_expectedDirection, _actual);
        }

    }
}
