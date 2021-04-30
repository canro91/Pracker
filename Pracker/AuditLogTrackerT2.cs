using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pracker
{
    public class AuditLogTracker<T1, T2> where T1 : class where T2 : class
    {
        private readonly T1 _classToTrack;
        private readonly T2 _classWithChanges;
        private readonly AuditLog<T1> _auditLog;

        public AuditLogTracker(T1 classToTrack, T2 classWithChanges)
        {
            _classToTrack = classToTrack;
            _classWithChanges = classWithChanges;
            _auditLog = new AuditLog<T1>(classToTrack);
        }

        public void TrackIfChanged<TValue>(Expression<Func<T1, TValue>> propertyToTrack)
        {
            var expression = propertyToTrack.Body as MemberExpression;
            var propertyName = expression.Member.Name;

            var changed = _classWithChanges.GetType().GetProperty(propertyName);
            if (changed == null)
            {
                return;
            }

            var func = propertyToTrack.Compile();
            var oldValue = func(_classToTrack);
            var newValue = changed.GetValue(_classWithChanges);
            if (!Equals(oldValue, newValue))
            {
                _auditLog.OnChanged(propertyName, newValue);
            }
        }

        public List<string> DisplayChanges()
            => _auditLog.DisplayChanges();
    }
}