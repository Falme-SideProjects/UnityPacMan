using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScoreTests
    {
        private Score score;

        [SetUp]
        public void Setup()
        {
            score = new Score();
        }

        [Test]
        public void GetScore_DidNotChangedScore_returnsZero()
        {
            int _actual = score.GetScore();
            Assert.AreEqual(0, _actual);
        }

        [Test]
        public void AddScore_AddScoreCalledOnce_returnSetValue()
        {
            score.AddScore(100);

            int _actual = score.GetScore();
            Assert.AreEqual(100, _actual);
        }

        [Test]
        public void AddScore_AddScoreCalledTwice_returnSetValues()
        {
            score.AddScore(100);
            score.AddScore(60);

            int _actual = score.GetScore();
            Assert.AreEqual(160, _actual);
        }

        [Test]
        public void ResetScore_AddScoreAndReset_returnZero()
        {
            score.AddScore(100);
            score.AddScore(60);

            score.ResetScore();

            int _actual = score.GetScore();
            Assert.AreEqual(0, _actual);
        }

        [Test]
        [TestCase(Items.pacdot, 10)]
        [TestCase(Items.power, 50)]
        [TestCase(Items.strawberry, 300)]
        [TestCase(Items.orange, 500)]
        [TestCase(Items.key, 5000)]
        public void AddScoreBasedOnItem_GivenItemName_AddSetItemValue(Items _item, int _expected)
        {
            score.AddScoreBasedOnItem(_item);

            int _actual = score.GetScore();
            Assert.AreEqual(_expected, _actual);
        }

        [Test]
        [TestCase(1,2,false)]
        [TestCase(100, 200, false)]
        [TestCase(200, 200, false)]
        [TestCase(201, 200, true)]
        [TestCase(1200, 200, true)]
        public void IsNewHighScore_IfHigherThanHighScore_ReturnTrue(int _score, 
                                                                    int _highscore,
                                                                    bool _expected)
        {
            bool _actual = score.IsNewHighScore(_score, _highscore);

            Assert.AreEqual(_expected, _actual);
        }

    }
}
