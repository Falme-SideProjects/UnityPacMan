using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class PlayerMovimentationTests
    {

        PlayerMovimentation playerMovimentation;

        [SetUp]
        public void Setup()
        {
            playerMovimentation = new PlayerMovimentation();
        }

        [Test]
        [TestCase(10f, 10f, 10f, 11f, 1f, Direction.up)]
        [TestCase(10f, 10f, 11f, 10f, 1f, Direction.right)]
        [TestCase(0f, 0f, 0f, -1f, 1f, Direction.down)]
        [TestCase(-1f, -1f, -2f, -1f, 1f, Direction.left)]
        [TestCase(10f, 10f, 10f, 10.5f, .5f, Direction.up)]
        public void Move_OnMoved_NewPositionToDirection(float _positionX,
                                                        float _positionY,
                                                        float _expectedX,
                                                        float _expectedY,
                                                        float _delta,
                                                        Direction direction)
        {
            playerMovimentation.SetWarpLimit(new Vector2(1000f, 1000f));
            playerMovimentation.SetPosition(new Vector2(_positionX, _positionY));

            playerMovimentation.SetNextDirection(direction);
            playerMovimentation.Move(Direction.up, _delta);

            Vector2 _actual = playerMovimentation.GetPosition();
            Vector2 _expected = new Vector2(_expectedX, _expectedY);

            Assert.AreEqual(_expected, _actual);
        }

    }
}
