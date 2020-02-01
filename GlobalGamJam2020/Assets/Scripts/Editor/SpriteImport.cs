using UnityEngine;
using UnityEditor;

class SpriteImport : AssetPostprocessor
{
    //void OnPreprocessTexture()
    //{
    //    TextureImporter textureImporter = (TextureImporter)assetImporter; // create a TextureImporter object, this gives us tools to let us tweak the import settings on the current asset

    //    if(textureImporter.textureType == TextureImporterType.Sprite)
    //    {
    //        textureImporter.spriteImportMode = SpriteImportMode.Single; // .. lets assume that each sprite is singular. if its a sprite sheet you have to set that up yourself the normal way
    //        textureImporter.filterMode = FilterMode.Point;
    //        textureImporter.spritePixelsPerUnit = 40; // Turn off mip maps because they are *gross* 
    //    }
    //}
}