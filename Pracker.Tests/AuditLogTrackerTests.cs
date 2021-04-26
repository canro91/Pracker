using NUnit.Framework;
using Pracker.Tests.Models;
using System.Linq;

namespace Pracker.Tests
{
    [TestFixture]
    public class AuditLogTrackerTests
    {
        [Test]
        public void Track_ByDefault_UpdatesProperty()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userWithTracker = new AuditLogTracker<User>(user);

            userWithTracker.UpdateAndTrack(u => u.FirstName, "After");

            Assert.AreEqual("After", user.FirstName);
        }

        [Test]
        public void Track_ByDefault_StorePreviousAndNewValue()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userWithTracker = new AuditLogTracker<User>(user);
            userWithTracker.UpdateAndTrack(u => u.FirstName, "After");

            var allChanges = userWithTracker.DisplayChanges();

            var change = allChanges.First();
            StringAssert.Contains("Before", change);
            StringAssert.Contains("After", change);
        }

        [Test]
        public void Track_NullAsNewValue_StoresKeywordInsteadOfNull()
        {
            var user = new User
            {
                FirstName = "Before"
            };
            var userWithTracker = new AuditLogTracker<User>(user, onNullValue: "null");
            userWithTracker.UpdateAndTrack(u => u.FirstName, null);

            var allChanges = userWithTracker.DisplayChanges();

            var change = allChanges.First();
            StringAssert.Contains("Before", change);
            StringAssert.Contains("null", change);
        }

        [Test]
        public void Track_NullAsPreviousValue_StoresKeywordInsteadOfNull()
        {
            var user = new User
            {
                FirstName = null
            };
            var userWithTracker = new AuditLogTracker<User>(user, onNullValue: "null");
            userWithTracker.UpdateAndTrack(u => u.FirstName, "After");

            var allChanges = userWithTracker.DisplayChanges();

            var change = allChanges.First();
            StringAssert.Contains("null", change);
            StringAssert.Contains("After", change);
        }

        // Only track changes
        // Track single property change
    }
}