using System.IO;
using UnityEditor;
 
using UnityEngine;

/*
This script serves as the tool to be able to import tsv files into the project. It is currently not used because .txt files have "replaced"
the need for .tsv files in the project (because they both have the same functionality), which means it does not need any script that allows 
the importing of .tsv files and can work natively without the need of custom asset import scripts.

This script showcases a use to make a custom asset importer if needed. This script is kept for the sake of preserving old systems for future needs,
should the opportunity arise. To future developers, if this script has no use to you, please do whatever you want with it, such as to delete it.
*/
 
[UnityEditor.AssetImporters.ScriptedImporter(1, "tsv")]
public class TSVImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        TextAsset textAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), textAsset);
        ctx.SetMainObject(textAsset);
        AssetDatabase.SaveAssets();
    }
}
 
