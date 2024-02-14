﻿using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Util
{
    /// <summary>
    /// Contains utility methods for requiring data.
    /// </summary>
    public static class RequireUtil
    {
        /// <summary>
        /// Gets the asset of type <typeparamref name="T"/> at the path specified by <paramref name="assetPath"/> or
        /// throws an exception if the asset is missing.
        /// </summary>
        /// <param name="assetPath">the required asset's path</param>
        /// <typeparam name="T">the type of the required asset</typeparam>
        /// <returns>the required asset</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="assetPath"/> is <c>null</c></exception>
        /// <exception cref="InvalidOperationException">
        /// if there is no asset of type <typeparamref name="T"/> at the path specified by <paramref name="assetPath"/>
        /// </exception>
        /// <seealso cref="AssetDatabase.LoadAssetAtPath{T}"/>
        [return: NotNull]
        public static T RequireAsset<T>([DisallowNull] string assetPath)
            where T : Object
        {
            if (assetPath == null)
                throw new ArgumentNullException(nameof(assetPath));

            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (asset == null)
                throw new InvalidOperationException($"Missing required {typeof(T).Name} asset at {assetPath}!");

            return asset;
        }
        
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

        /// <summary>
        /// Gets the element of type <typeparamref name="T"/> with the name provided as <paramref name="name"/>
        /// contained by <paramref name="visualElement"/> or throws an exception if the element is missing.
        /// </summary>
        /// <param name="visualElement">ancestor element of the required element</param>
        /// <param name="name">name of the required element</param>
        /// <typeparam name="T">type of the required element</typeparam>
        /// <returns>the required descendant element of <paramref name="visualElement"/></returns>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="visualElement"/> or <paramref name="name"/> is <c>null</c>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// if there is no element of type <typeparamref name="T"/> with the name <paramref name="name"/> in
        /// <paramref name="visualElement"/>
        /// </exception>
        [return: NotNull]
        public static T RequireElement<T>([DisallowNull] this VisualElement visualElement, [DisallowNull] string name)
            where T : VisualElement
        {
            if (visualElement == null)
                throw new ArgumentNullException(nameof(visualElement));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var element = visualElement.Q<T>(name);

            if (element == null)
                throw new InvalidOperationException(
                    $"Missing {typeof(T).Name} element called {name} on {visualElement.GetType().Name} {visualElement.name}!");

            return element;
        }
    }
}