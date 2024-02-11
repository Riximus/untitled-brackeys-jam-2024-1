using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// Contains utility methods for requiring data.
    /// </summary>
    public static class RequireExtensions
    {
        /// <summary>
        /// Gets the component of type <typeparamref name="T"/> on <paramref name="monoBehaviour"/>'s
        /// <see cref="GameObject"/> or throws an exception if the component is missing.
        /// </summary>
        /// <param name="monoBehaviour">the calling component on this game object</param>
        /// <typeparam name="T">type of the component to get</typeparam>
        /// <returns>the <typeparamref name="T"/> component on this game object</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="monoBehaviour"/> is <c>null</c></exception>
        /// <exception cref="InvalidOperationException">
        /// if no <typeparamref name="T"/> component exists on this game object
        /// </exception>
        [return: NotNull]
        public static T RequireComponent<T>([DisallowNull] this MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour == null)
                throw new ArgumentNullException(nameof(monoBehaviour));

            var component = monoBehaviour.GetComponent<T>();

            if (component == null)
                throw new InvalidOperationException(
                    $"Missing {typeof(T).Name} component on {monoBehaviour.gameObject.name}!");

            return component;
        }
    }
}