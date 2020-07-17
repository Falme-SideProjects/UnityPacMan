using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MovementPermissionTests
    {
        private MovementPermission movementPermission;

        [SetUp]
        public void Setup()
        {
            movementPermission = new MovementPermission();
        }

        [Test]
        [TestCase(Direction.up)]
        [TestCase(Direction.left)]
        [TestCase(Direction.right)]
        [TestCase(Direction.down)]
        public void CanMoveAt_WitoutModifyingParameters_True(Direction _direction)
        {
            bool _actual = movementPermission.CanMoveAt(_direction);
            Assert.AreEqual(true, _actual);
        }

        [Test]
        [TestCase(Direction.up, false)]
        [TestCase(Direction.left, false)]
        [TestCase(Direction.right, false)]
        [TestCase(Direction.down, false)]
        public void SetMovePermission_ModifyPermissionsToFalse_ReturnFalse(Direction _direction,
                                                                                     bool _expectValue)
        {
            movementPermission.SetMovePermission(false, false, false, false);

            bool _actual = movementPermission.CanMoveAt(_direction);
            Assert.AreEqual(_expectValue, _actual);
        }

        [Test]
        [TestCase(Direction.up, true)]
        [TestCase(Direction.left, true)]
        [TestCase(Direction.right, false)]
        [TestCase(Direction.down, false)]
        public void SetMovePermission_ModifyPermissions_ChangeCanMoveDirectionReturn(Direction _direction,
                                                                                     bool _expectValue)
        {
            movementPermission.SetMovePermission(true, true, false, false);

            bool _actual = movementPermission.CanMoveAt(_direction);
            Assert.AreEqual(_expectValue, _actual);
        }

    }
}
