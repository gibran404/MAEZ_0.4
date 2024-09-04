#if UNITY_EDITOR
// An asset postprocessor that sets the material setting of a model to "Use Embedded Materials" 
// and places the materials in a "Materials" folder within the model's directory.
//
// It only processes an asset if it's a new one, that didn't exist in the project yet.
// Duplicating an asset inside Unity does not count as a new asset in this case.
// It counts as a new asset if the .meta file is missing.
//
// Save as: Assets/Editor/ForceEmbeddedMaterialProcessor.cs
using UnityEngine;
using UnityEditor;

public class ForceEmbeddedMaterialProcessor : AssetPostprocessor
{
    void OnPreprocessModel()
    {
#if UNITY_2018_1_OR_NEWER
        var importSettingsMissing = assetImporter.importSettingsMissing;
#else
        var importSettingsMissing = !System.IO.File.Exists(AssetDatabase.GetTextMetaFilePathFromAssetPath(assetPath));
#endif
        if (!importSettingsMissing)
            return; // Asset imported already, do not process.

        var modelImporter = assetImporter as ModelImporter;
        modelImporter.materialLocation = ModelImporterMaterialLocation.InPrefab;

        // Specify the materials folder relative to the model file
        modelImporter.materialSearch = ModelImporterMaterialSearch.Local;
    }
}
#endif
