using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Util
{
    public static class UxmlUtil
    {
        public static void LoadUxml([DisallowNull] VisualElement visualElement)
        {
            if (visualElement == null)
                throw new ArgumentNullException(nameof(visualElement));
            
            var elementTypeName = visualElement.GetType().Name;
            var assetPath = AssetDatabase
                .FindAssets($"t:Script {elementTypeName}")!
                .Select(AssetDatabase.GUIDToAssetPath)
                .FirstOrDefault();
            
            if (assetPath == null)
                throw new InvalidOperationException($"There is no script asset for {elementTypeName}!");
            
            var uxmlAssetPath = Path.Join(
                Path.GetDirectoryName(assetPath),
                $"{elementTypeName}.uxml");
            RequireUtil
                .RequireAsset<VisualTreeAsset>(uxmlAssetPath)
                .CloneTree(visualElement);
        }
    }
}