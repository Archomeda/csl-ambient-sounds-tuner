using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonShared.Utils
{
    /// <summary>
    /// Contains various utilities regarding reflection.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Gets the value of a private field.
        /// </summary>
        /// <typeparam name="T">The object type of the returned value.</typeparam>
        /// <param name="obj">The object to get the value of the field from.</param>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field.</returns>
        public static T GetPrivateField<T>(object obj, string name)
        {
            return (T)obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
        }

        /// <summary>
        /// Sets the value of a private field.
        /// </summary>
        /// <typeparam name="T">The object type of the value to set.</typeparam>
        /// <param name="obj">The object to set the value of the field on.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value to set.</param>
        public static void SetPrivateField<T>(object obj, string name, T value)
        {
            obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
        }

        /// <summary>
        /// Invokes a private method.
        /// </summary>
        /// <param name="obj">The object to invoke the method on.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="args">The arguments to pass to the method.</param>
        public static void InvokePrivateMethod(object obj, string name, params object[] args)
        {
            MethodInfo method = obj.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(obj, args);
        }

        /// <summary>
        /// Invokes a private method with a return value.
        /// </summary>
        /// <typeparam name="T">The object type of the returned value.</typeparam>
        /// <param name="obj">The object to invoke the method on.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="args">The arguments to pass to the method.</param>
        /// <returns>The return value of the invoked method.</returns>
        public static T InvokePrivateMethod<T>(object obj, string name, params object[] args)
        {
            MethodInfo method = obj.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)method.Invoke(obj, args);
        }

        /// <summary>
        /// Invokes a private static method.
        /// </summary>
        /// <param name="type">The object to invoke the method on.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="args">The arguments to pass to the method.</param>
        public static void InvokePrivateStaticMethod(Type type, string name, params object[] args)
        {
            MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
            method.Invoke(null, args);
        }

        /// <summary>
        /// Invokes a private static method with a return value.
        /// </summary>
        /// <typeparam name="T">The object type of the returned value.</typeparam>
        /// <param name="obj">The object to invoke the method on.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="args">The arguments to pass to the method.</param>
        /// <returns>The return value of the invoked method.</returns>
        public static T InvokePrivateStaticMethod<T>(Type type, string name, params object[] args)
        {
            MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
            return (T)method.Invoke(null, args);
        }
    }
}
