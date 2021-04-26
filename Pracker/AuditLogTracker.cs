using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Pracker
{
    public class AuditLogTracker<T> where T : class
    {
        private readonly AuditLog<T> _auditLog;
        private readonly T _classToTrack;

        public AuditLogTracker(T classToTrack, string onNullValue = null)
        {
            _classToTrack = classToTrack;
            _auditLog = new AuditLog<T>(classToTrack, onNullValue);
        }

        public void UpdateAndTrack<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var memberExpression = property.Body as MemberExpression;
            var member = memberExpression.Member;

            var propertyInfo = member as PropertyInfo;
            propertyInfo.SetValue(_classToTrack, value);

            _auditLog.OnChanged(propertyInfo.Name);
        }

        public void Track<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var func = property.Compile();
            var oldValue = func(_classToTrack);

            if (!Equals(oldValue, value))
            {
                var memberExpression = property.Body as MemberExpression;
                var member = memberExpression.Member;

                var propertyInfo = member as PropertyInfo;
                _auditLog.OnChanged(propertyInfo.Name, value);
            }
        }

        public List<string> DisplayChanges()
            => _auditLog.DisplayChanges();
    }
}