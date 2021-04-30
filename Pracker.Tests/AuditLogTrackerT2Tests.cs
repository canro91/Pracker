using NUnit.Framework;
using Pracker.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pracker.Tests
{
    [TestFixture]
    public class AuditLogTrackerT2Tests
    {
        [Test]
        public void Track_SinglePropertyChanged_StorePreviousAndNewValue()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userViewModel = new UserViewModel
            {
                FirstName = "After"
            };
            var userWithTracker = new AuditLogTracker<User, UserViewModel>(user, userViewModel);
            userWithTracker.TrackIfChanged(u => u.FirstName);

            var allChanges = userWithTracker.DisplayChanges();

            var change = allChanges.First();
            StringAssert.Contains("Before", change);
            StringAssert.Contains("After", change);
        }

        [Test]
        public void Track_SinglePropertyNotChanged_DoesNotStoreAnyValues()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userViewModel = new UserViewModel
            {
                FirstName = user.FirstName
            };
            var userWithTracker = new AuditLogTracker<User, UserViewModel>(user, userViewModel);
            userWithTracker.TrackIfChanged(u => u.FirstName);

            var allChanges = userWithTracker.DisplayChanges();

            Assert.IsFalse(allChanges.Any());
        }

        // Class2 doesn't have property
        // Nulls on either side
        // TrackAll: More than one property changed
    }
}
