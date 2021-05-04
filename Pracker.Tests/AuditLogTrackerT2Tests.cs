using NUnit.Framework;
using Pracker.Tests.Models;
using System.Linq;

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
            userWithTracker.Track(u => u.FirstName);

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
            userWithTracker.Track(u => u.FirstName);

            var allChanges = userWithTracker.DisplayChanges();

            Assert.IsFalse(allChanges.Any());
        }

        [Test]
        public void Track_PropertyNotPresent_DoesNotStoreAnyValues()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userViewModel = new UserWithoutFirstNameViewModel();
            var userWithTracker = new AuditLogTracker<User, UserWithoutFirstNameViewModel>(user, userViewModel);
            userWithTracker.Track(u => u.FirstName);

            var allChanges = userWithTracker.DisplayChanges();

            Assert.IsFalse(allChanges.Any());
        }

        [Test]
        public void Track_NullOnClassToTrack_StorePreviousAndNewValue()
        {
            var user = new User
            {
                FirstName = null
            };
            var userViewModel = new UserViewModel
            {
                FirstName = "After"
            };
            var userWithTracker = new AuditLogTracker<User, UserViewModel>(user, userViewModel, onNullValue: "null");
            userWithTracker.Track(u => u.FirstName);

            var allChanges = userWithTracker.DisplayChanges();
            Assert.AreEqual(1, allChanges.Count);

            var change = allChanges.First();
            StringAssert.Contains("null", change);
            StringAssert.Contains("After", change);
        }

        [Test]
        public void Track_NullOnClassWithChanges_StorePreviousAndNewValue()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userViewModel = new UserViewModel
            {
                FirstName = null
            };
            var userWithTracker = new AuditLogTracker<User, UserViewModel>(user, userViewModel, onNullValue: "null");
            userWithTracker.Track(u => u.FirstName);

            var allChanges = userWithTracker.DisplayChanges();
            Assert.AreEqual(1, allChanges.Count);

            var change = allChanges.First();
            StringAssert.Contains("Before", change);
            StringAssert.Contains("null", change);
        }

        // TrackAll: More than one property changed
    }
}