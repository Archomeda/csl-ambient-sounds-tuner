using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;

namespace CommonShared.Utils
{
    /// <summary>
    /// Contains various utilities regarding atlases.
    /// </summary>
    public static class AtlasUtils
    {
        /// <summary>
        /// Creates a new <see cref="UITextureAtlas"/> from a PNG file.
        /// </summary>
        /// <param name="fileName">The PNG file name of the texture.</param>
        /// <param name="atlasName">The name of the <see cref="UITextureAtlas"/>.</param>
        /// <param name="shaderName">The name of the <see cref="Shader"/>.</param>
        /// <param name="spriteSize">The size of each sprite.</param>
        /// <param name="spriteGrid">The size of the sprite grid.</param>
        /// <param name="spriteNames">The names of the sprites.</param>
        /// <returns>The generated <see cref="UITextureAtlas"/>.</returns>
        public static UITextureAtlas CreateAtlas(string fileName, string atlasName, string shaderName, Vector2 spriteSize, Vector2 spriteGrid, string[][] spriteNames)
        {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
            {
                return null;
            }

            // Setup
            Texture2D texture = new Texture2D((int)(spriteSize.x * spriteGrid.x), (int)(spriteSize.y * spriteGrid.y), TextureFormat.ARGB32, false);
            byte[] textureBytes = File.ReadAllBytes(fileName);
            texture.LoadImage(textureBytes);
            FixTransparency(texture);

            Material material = new Material(shader);
            material.mainTexture = texture;

            UITextureAtlas atlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            atlas.name = atlasName;
            atlas.material = material;

            // Sprites
            for (int y = 0; y < spriteGrid.y; y++)
            {
                for (int x = 0; x < spriteGrid.x; x++)
                {
                    if (y < spriteNames.Length && x < spriteNames[y].Length && !string.IsNullOrEmpty(spriteNames[y][x]))
                    {
                        Rect spriteRect = new Rect(
                            x * spriteSize.x / texture.width,
                            1 - ((y + 1) * spriteSize.y / texture.height),
                            spriteSize.x / texture.width,
                            spriteSize.y / texture.height
                        );

                        UITextureAtlas.SpriteInfo spriteInfo = new UITextureAtlas.SpriteInfo()
                        {
                            name = spriteNames[y][x],
                            texture = texture,
                            region = spriteRect
                        };
                        atlas.AddSprite(spriteInfo);
                    }
                }
            }

            return atlas;
        }

        /// <summary>
        /// Copy the values of adjacent pixels to transparent pixels color info, to remove the white border artifact when importing transparent .PNGs.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <remarks>Source at http://answers.unity3d.com/questions/238922/png-transparency-has-white-borderhalo.html. </remarks>
        private static void FixTransparency(this Texture2D texture)
        {
            Color32[] pixels = texture.GetPixels32();
            int w = texture.width;
            int h = texture.height;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * w + x;
                    Color32 pixel = pixels[idx];
                    if (pixel.a == 0)
                    {
                        bool done = false;
                        if (!done && x > 0) done = TryAdjacent(ref pixel, pixels[idx - 1]);        // Left pixel
                        if (!done && x < w - 1) done = TryAdjacent(ref pixel, pixels[idx + 1]);    // Right pixel
                        if (!done && y > 0) done = TryAdjacent(ref pixel, pixels[idx - w]);        // Top pixel
                        if (!done && y < h - 1) done = TryAdjacent(ref pixel, pixels[idx + w]);    // Bottom pixel
                        pixels[idx] = pixel;
                    }
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply();
        }

        private static bool TryAdjacent(ref Color32 pixel, Color32 adjacent)
        {
            if (adjacent.a == 0) return false;

            pixel.r = adjacent.r;
            pixel.g = adjacent.g;
            pixel.b = adjacent.b;
            return true;
        }
    }
}
