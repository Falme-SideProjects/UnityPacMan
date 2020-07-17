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
        public void GetPosition_DidNotModified_VectorZero()
        {
            Vector2 _actual = playerMovimentation.GetPosition();
            Vector2 _expected = new Vector2(0f, 0f);

            Assert.AreEqual(_expected, _actual);
        }
    


        [Test]
        [TestCase(10f,10f, 10f, 10f)]
        [TestCase(-2f, 10f, -2f, 10f)]
        [TestCase(-2f, 0f, -2f, 0f)]
        [TestCase(-2f, -123.5f, -2f, -123.5f)]
        public void SetPosition_ModifiedPosition_NewVectorPosition(float _positionX,
                                                                    float _positionY, 
                                                                    float _expectedX,
                                                                    float _expectedY)
        {
            playerMovimentation.SetPosition(new Vector2(_positionX, _positionY));

            Vector2 _actual = playerMovimentation.GetPosition();
            Vector2 _expected = new Vector2(_expectedX, _expectedY);

            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        [TestCase(10f,10f,10f,11f, 1f, Direction.up)]
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
            playerMovimentation.SetWarpLimit(new Vector2(1000f,1000f));
            playerMovimentation.SetPosition(new Vector2(_positionX, _positionY));

            playerMovimentation.Move(direction, _delta);

            Vector2 _actual = playerMovimentation.GetPosition();

            Vector2 _expected = new Vector2(_expectedX, _expectedY);

            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        [TestCase(-4.1f, 0f, 4.0f, 0f)]
        [TestCase(4.1f, 0f, -4.0f, 0f)]
        [TestCase(4.01f, 0f, -4.0f, 0f)]
        [TestCase(-4.01f, 0f, 4.0f, 0f)]
        [TestCase(-4.0f, 0f, -4.0f, 0f)]
        [TestCase(4.0f, 0f, -2.0f, 0f, 2f)]
        [TestCase(-4.0f, 3f, 2.0f, 3f, 2f, 4f)]
        [TestCase(-4.0f, 3f, 2.0f, -2f, 2f, 2f)]
        public void CheckWarp_PlayerWentTooFarInHorizontalDirection_WarpPlayerToOtherSide(float _positionX,
                                                                                        float _positionY,
                                                                                        float _expectedX,
                                                                                        float _expectedY,
                                                                                        float _limitX=4f,
                                                                                        float _limitY=4f)
        {
            playerMovimentation.SetPosition(new Vector2(_positionX, _positionY));

            playerMovimentation.CheckWarp(new Vector2(_limitX, _limitY));

            Vector2 _result = playerMovimentation.GetPosition();

            Vector2 _expected = new Vector2(_expectedX, _expectedY);

            Assert.AreEqual(_expected, _result);
        }

        [Test] //InitialGrid|EndGrid|PlayerPos|Length|Expected 
        [TestCase(10f, 10f, 30f, 40f, 10f, 10f, 3, 4, 0, 0)]
        [TestCase(10f, 10f, 30f, 40f, 20f, 10f, 3, 4, 1, 0)]
        [TestCase(10f, 10f, 30f, 40f, 30f, 10f, 3, 4, 2, 0)]
        [TestCase(10f, 10f, 30f, 40f, 40f, 10f, 3, 4, 2, 0)]
        [TestCase(10f, 10f, 30f, 40f, 10f, 20f, 3, 4, 0, 1)]
        [TestCase(10f, 10f, 30f, 40f, 10f, 30f, 3, 4, 0, 2)]
        [TestCase(10f, 10f, 30f, 40f, 15f, 30f, 3, 4, 0, 2)]
        [TestCase(10f, 10f, 30f, 40f, 10f, 40f, 3, 4, 0, 3)]
        [TestCase(10f, 10f, 30f, 40f, 10f, 50f, 3, 4, 0, 3)]
        [TestCase(-10f, -10f, 30f, 40f, -10f, -10f, 3, 4, 0, 0)]
        [TestCase(-10f, -10f, 30f, 40f, 0f, -10f, 5, 6, 1, 0)]
        [TestCase(-10f, 10f, 30f, -40f, 10f, -10f, 5, 6, 2, 2)]
        public void GetPositionInGrid_GivenGameGrid_ReturnPlayerVector2GridPosition(float _initialGridX,
                                                                                    float _initialGridY,
                                                                                    float _endGridX,
                                                                                    float _endGridY,
                                                                                    float _playerPositionX,
                                                                                    float _playerPositionY,
                                                                                    int _lengthX,
                                                                                    int _lengthY,
                                                                                    int _expectedX,
                                                                                    int _expectedY)
        {

            Vector2 _initialPosition = new Vector2(_initialGridX, _initialGridY);
            Vector2 _endPosition = new Vector2(_endGridX, _endGridY);
            Vector2 _length = new Vector2(_lengthX, _lengthY);

            playerMovimentation.SetPosition(new Vector2(_playerPositionX, _playerPositionY));

            Vector2 _result = playerMovimentation.GetPositionInGrid(_initialPosition, _endPosition, _length);

            Vector2 _expected = new Vector2(_expectedX, _expectedY);
            Assert.AreEqual(_expected, _result);
        }

        [Test]
        [TestCase(4,6,0f,0)]
        [TestCase(4, 6, 2f, 1)]
        [TestCase(4, 6, 4f, 2)]
        [TestCase(4, 6, 6f, 3)]
        [TestCase(4, 6, 3f, 1)]
        public void GetPositionBetweenTwoNumbers_RandomNumberAddedTo_ReturnClosestNumberInLength(int _arrayLength, 
                                                                                                 float _distance, 
                                                                                                 float _compareNumber,
                                                                                                 int _expectedPosition)
        {

            int _result = playerMovimentation.GetPositionBetweenTwoNumbers(_arrayLength, _distance, _compareNumber);

            int _expected = _expectedPosition;
            Assert.AreEqual(_expectedPosition, _result);
        }

    }
}
