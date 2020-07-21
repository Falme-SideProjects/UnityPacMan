using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class GhostStateChaseTests
    {
        private GhostStateChase ghostStateChase;

        [SetUp]
        public void Setup()
        {
            ghostStateChase = new GhostStateChase();
        }

        [Test]
        [TestCase(0, 0, 0, 0, 0, 0)]
        [TestCase(10, 10, 12, 12, 8, 8)]
        [TestCase(10, 8, 12, 12, 8, 4)]
        [TestCase(10, 10, 20, 20, 0, 0)]
        [TestCase(10, 10, 22, 22, -2, -2)]
        [TestCase(-10, -10, -22, -22, 2, 2)]
        public void GetMirroredPositionBetweenBlinkyAndPlayer_BetweenTwoPoints_Return180DegreesPosition(int _playerPositionX,
                                                                                                        int _playerPositionY,
                                                                                                        int _blinkyPositionX,
                                                                                                        int _blinkyPositionY,
                                                                                                        int _expectedX,
                                                                                                        int _expectedY)
        {
            Vector2 playerPosition = new Vector2(_playerPositionX, _playerPositionY);
            Vector2 blinkyPositon = new Vector2(_blinkyPositionX, _blinkyPositionY);

            Vector2 _actual = ghostStateChase.GetMirroredPositionBetweenBlinkyAndPlayer(playerPosition, blinkyPositon);

            Assert.AreEqual(new Vector2(_expectedX, _expectedY) ,_actual);
        }

        [Test]
        [TestCase(0, 0, 0, 0, 1, false)]
        [TestCase(0, 0, 0, 0, 0, true)]
        [TestCase(0, 0, 0, 0, 8, false)]
        [TestCase(0, 0, 7, 0, 8, false)]
        [TestCase(0, 0, 0, 7, 8, false)]
        [TestCase(0, 0, 7, 7, 8, false)]
        [TestCase(7, 0, 0, 0, 8, false)]
        [TestCase(0, 7, 0, 0, 8, false)]
        [TestCase(7, 7, 0, 0, 8, false)]
        [TestCase(0, 0, 8, 0, 8, true)]
        [TestCase(0, 0, 0, 8, 8, true)]
        [TestCase(0, 0, 8, 8, 8, true)]
        [TestCase(8, 0, 0, 0, 8, true)]
        [TestCase(0, 8, 0, 0, 8, true)]
        [TestCase(8, 8, 0, 0, 8, true)]
        [TestCase(0, 0, 9, 0, 8, true)]
        [TestCase(0, 0, 0, 9, 8, true)]
        [TestCase(0, 0, 9, 9, 8, true)]
        [TestCase(9, 0, 0, 0, 8, true)]
        [TestCase(0, 9, 0, 0, 8, true)]
        [TestCase(9, 9, 0, 0, 8, true)]
        public void IsTooFarFromOtherTile_IsClydeTooFarFromPlayer_ReturnTrue(int _playerPositionX,
                                                                            int _playerPositionY,
                                                                            int _ghostPositionX,
                                                                            int _ghostPositionY,
                                                                            int _minDistance,
                                                                            bool _expected)
        {
            Vector2 playerPosition = new Vector2(_playerPositionX, _playerPositionY);
            Vector2 ghostPositon = new Vector2(_ghostPositionX, _ghostPositionY);


            bool _actual = ghostStateChase.IsTooFarFromOtherTile(playerPosition, ghostPositon, _minDistance);

            Assert.AreEqual(_expected, _actual);
        }

    }
}
