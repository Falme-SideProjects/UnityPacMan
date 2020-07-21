using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CharacterMovimentationTests
    {
        CharacterMovimentation characterMovimentation;

        [SetUp]
        public void Setup()
        {
            characterMovimentation = new CharacterMovimentation();
        }

        [Test]
        public void GetPosition_DidNotModified_VectorZero()
        {
            Vector2 _actual = characterMovimentation.GetPosition();
            Vector2 _expected = new Vector2(0f, 0f);

            Assert.AreEqual(_expected, _actual);
        }



        [Test]
        [TestCase(10f, 10f, 10f, 10f)]
        [TestCase(-2f, 10f, -2f, 10f)]
        [TestCase(-2f, 0f, -2f, 0f)]
        [TestCase(-2f, -123.5f, -2f, -123.5f)]
        public void SetPosition_ModifiedPosition_NewVectorPosition(float _positionX,
                                                                    float _positionY,
                                                                    float _expectedX,
                                                                    float _expectedY)
        {
            characterMovimentation.SetPosition(new Vector2(_positionX, _positionY));

            Vector2 _actual = characterMovimentation.GetPosition();
            Vector2 _expected = new Vector2(_expectedX, _expectedY);

            Assert.AreEqual(_expected, _actual);
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
            characterMovimentation.SetWarpLimit(new Vector2(1000f, 1000f));
            characterMovimentation.SetPosition(new Vector2(_positionX, _positionY));

            characterMovimentation.Move(direction, _delta);

            Vector2 _actual = characterMovimentation.GetPosition();
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
                                                                                        float _limitX = 4f,
                                                                                        float _limitY = 4f)
        {
            characterMovimentation.SetPosition(new Vector2(_positionX, _positionY));
            characterMovimentation.CheckWarp(new Vector2(_limitX, _limitY));

            Vector2 _result = characterMovimentation.GetPosition();
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
        [TestCase(-10f, 10f, 30f, -40f, -20f, -10f, 5, 6, 0, 2)]
        [TestCase(-10f, 10f, 30f, -40f, -200f, -10f, 5, 6, 0, 2)]
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

            characterMovimentation.SetPosition(new Vector2(_playerPositionX, _playerPositionY));

            Vector2 _result = characterMovimentation.GetPositionInGrid(_initialPosition, _endPosition, _length);

            Vector2 _expected = new Vector2(_expectedX, _expectedY);
            Assert.AreEqual(_expected, _result);
        }

        [Test]
        [TestCase(4, 6, 0f, 0)]
        [TestCase(4, 6, 2f, 1)]
        [TestCase(4, 6, 4f, 2)]
        [TestCase(4, 6, 6f, 3)]
        [TestCase(4, 6, 3f, 1)]
        public void GetPositionBetweenTwoNumbers_RandomNumberAddedTo_ReturnClosestNumberInLength(int _arrayLength,
                                                                                                 float _distance,
                                                                                                 float _compareNumber,
                                                                                                 int _expectedPosition)
        {

            int _result = characterMovimentation.GetPositionBetweenTwoNumbers(_arrayLength, _distance, _compareNumber);

            Assert.AreEqual(_expectedPosition, _result);
        }

        [Test]
        public void GetInitialPosition_DidNotChanged_ReturnZero()
        {
            Vector2 _actual = characterMovimentation.GetInitialPosition();
            Assert.AreEqual(Vector2.zero, _actual);
        }

        [Test]
        public void SetInitialPosition_SetNewValue_ReturnNewInitialPosition()
        {
            characterMovimentation.SetInitialPosition(new Vector2(1f, 10f));

            Vector2 _actual = characterMovimentation.GetInitialPosition();
            Assert.AreEqual(new Vector2(1f, 10f), _actual);
        }



        [Test]
        public void SetInitialPosition_SetNewValueDontChangePlayerPosition_ReturnOldPlayerPosition()
        {
            characterMovimentation.SetPosition(new Vector2(12f, 333f));
            characterMovimentation.SetInitialPosition(new Vector2(1f, 10f));

            Vector2 _actual = characterMovimentation.GetPosition();
            Assert.AreNotEqual(new Vector2(1f, 10f), _actual);
        }


        [Test]
        public void ResetPosition_ResetPlayerPosition_MovePlayerPositionToInitialPosition()
        {
            characterMovimentation.SetInitialPosition(new Vector2(1f, 10f));
            characterMovimentation.SetPosition(new Vector2(12f, 333f));

            characterMovimentation.ResetPosition();

            Vector2 _actual = characterMovimentation.GetPosition();
            Assert.AreEqual(new Vector2(1f, 10f), _actual);
        }

        [Test]
        [TestCase(Direction.right, 1, 1, 1, 1, true)]
        [TestCase(Direction.right, 0.99f, 1, 1, 1, false)]
        [TestCase(Direction.left, 1, 1, 1, 1, true)]
        [TestCase(Direction.left, 1.01f, 1, 1, 1, false)]
        [TestCase(Direction.up, 1, 1, 1, 1, true)]
        [TestCase(Direction.up, 1, 0.99f, 1, 1, false)]
        [TestCase(Direction.down, 1, 1, 1, 1, true)]
        [TestCase(Direction.down, 1, 1.01f, 1, 1, false)]
        public void ReachedOffsetToChangeDirection_IfPassedThreshold_ReturnTrue(Direction _currentDirection,
                                                                                float _currentPositionX,
                                                                                float _currentPositionY,
                                                                                float _tilePositionX,
                                                                                float _tilePositionY,
                                                                                bool _expected)
        {
            Vector2 currentPosition = new Vector2(_currentPositionX, _currentPositionY);
            Vector2 tilePosition = new Vector2(_tilePositionX, _tilePositionY);

            bool _actual = characterMovimentation.ReachedOffsetToChangeDirection(_currentDirection,
                                                                                currentPosition,
                                                                                tilePosition);

            Assert.AreEqual(_expected, _actual);
        }

    }
}
