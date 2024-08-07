﻿using System.Collections.Generic;
using System.Reflection;

namespace Pracker
{
    internal class AuditLog<T>
    {
        private readonly Dictionary<string, object> _currentState;
        private readonly List<string> _changes;
        private readonly T _entity;
        private readonly string _onNullValue;

        public AuditLog(T entity, string onNullValue = null)
        {
            _entity = entity;
            _currentState = new Dictionary<string, object>();
            _changes = new List<string>();
            _onNullValue = onNullValue;

            Initialize();
        }

        private void Initialize()
        {
            var thisType = _entity.GetType();
            PropertyInfo[] properties = thisType.GetProperties();

            // Save the current value of the properties to our dictionary.
            foreach (PropertyInfo property in properties)
            {
                var value = thisType.GetProperty(property.Name)?.GetValue(_entity);

                _currentState.Add(property.Name, value);
            }
        }

        public void OnChanged(string propertyName)
        {
            var currentValue = _entity.GetType().GetProperty(propertyName)?.GetValue(_entity);
            if (_currentState.ContainsKey(propertyName))
            {
                var prevValue = _currentState[propertyName];
                _currentState[propertyName] = currentValue;

                _changes.Add($"Field {propertyName}, original value: {prevValue ?? _onNullValue}, new value: {currentValue ?? _onNullValue}");
            }
            else
            {
                _changes.Add($"Field {propertyName}, original value: , new value: {currentValue}");
            }
        }

        public void OnChanged(string propertyName, object newValue)
        {
            if (_currentState.ContainsKey(propertyName))
            {
                var prevValue = _currentState[propertyName];
                _currentState[propertyName] = newValue;

                _changes.Add($"Field {propertyName}, original value: {prevValue ?? _onNullValue}, new value: {newValue ?? _onNullValue}");
            }
            else
            {
                _changes.Add($"Field {propertyName}, original value: , new value: {newValue}");
            }
        }

        public List<string> DisplayChanges()
            => _changes;
    }
}