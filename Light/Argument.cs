using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Light {
    public static class Argument {
        [DebuggerHidden]
        public static void RequireNotNull(string name, object value) {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        [DebuggerHidden]
        public static void RequireNotEmpty(string name, string value) {
            if (value.Length == 0)
                throw new ArgumentException("Value can not be empty.", name);
        }

        [DebuggerHidden]
        public static void RequireNotEmpty<T>(string name, ICollection<T> collection) {
            if (!collection.Any())
                throw new ArgumentException("Value can not be empty.", name);
        }

        [DebuggerHidden]
        public static void RequireNotNullAndNotEmpty(string name, string value) {
            Argument.RequireNotNull(name, value);
            Argument.RequireNotEmpty(name, value);
        }

        [DebuggerHidden]
        public static void RequireNotNullAndNotEmpty<T>(string name, ICollection<T> collection) {
            Argument.RequireNotNull(name, collection);
            Argument.RequireNotEmpty(name, collection);
        }

        [DebuggerHidden]
        public static void RequireNotContainsNull<T>(string name, ICollection<T> collection) 
            where T : class
        {
            if (collection.Contains(null))
                throw new ArgumentException("Collection can not contain null values.", name);
        }

        [DebuggerHidden]
        public static void RequireNotNullAndNotContainsNull<T>(string name, ICollection<T> collection)
            where T : class
        {
            RequireNotNull(name, collection);
            RequireNotContainsNull(name, collection);
        }

        [DebuggerHidden]
        public static void RequireNotNullNotEmptyAndNotContainsNull<T>(string name, ICollection<T> collection)
            where T : class 
        {
            RequireNotNullAndNotContainsNull(name, collection);
            RequireNotEmpty(name, collection);
        }
    }
}
