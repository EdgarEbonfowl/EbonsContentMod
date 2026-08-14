using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EbonsContentMod.Utilities
{
    internal class SpriteHelperators
    {
        public static void ExportSprite(Sprite sprite, string fileName)
        {
            if (sprite == null)
                return;

            Texture2D texture = sprite.texture;

            if (texture == null)
                return;

            Rect rect = sprite.textureRect;

            Texture2D readableTexture =
                new Texture2D(
                    (int)rect.width,
                    (int)rect.height,
                    TextureFormat.RGBA32,
                    false);

            RenderTexture renderTexture =
                RenderTexture.GetTemporary(
                    texture.width,
                    texture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, renderTexture);

            RenderTexture previous =
                RenderTexture.active;

            RenderTexture.active =
                renderTexture;

            Texture2D fullReadableTexture =
                new Texture2D(
                    texture.width,
                    texture.height,
                    TextureFormat.RGBA32,
                    false);

            fullReadableTexture.ReadPixels(
                new Rect(
                    0,
                    0,
                    texture.width,
                    texture.height),
                0,
                0);

            fullReadableTexture.Apply();

            RenderTexture.active =
                previous;

            RenderTexture.ReleaseTemporary(
                renderTexture);

            Color[] pixels =
                fullReadableTexture.GetPixels(
                    (int)rect.x,
                    (int)rect.y,
                    (int)rect.width,
                    (int)rect.height);

            readableTexture.SetPixels(pixels);
            readableTexture.Apply();

            byte[] png =
                readableTexture.EncodeToPNG();

            string directory =
                Path.Combine(
                    Main.ModPath,
                    "ToolOutput",
                    "Sprites");

            Directory.CreateDirectory(directory);

            string path =
                Path.Combine(
                    directory,
                    fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                        ? fileName
                        : fileName + ".png");

            File.WriteAllBytes(
                path,
                png);

            UnityEngine.Object.Destroy(
                readableTexture);

            UnityEngine.Object.Destroy(
                fullReadableTexture);

            Main.log.Log(
                $"Exported icon to: {path}");
        }

        public static void ExportTexture(Texture2D texture, string fileName)
        {
            if (texture == null)
                return;

            RenderTexture renderTexture =
                RenderTexture.GetTemporary(
                    texture.width,
                    texture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

            RenderTexture previous =
                RenderTexture.active;

            try
            {
                Graphics.Blit(texture, renderTexture);

                RenderTexture.active = renderTexture;

                Texture2D readableTexture =
                    new Texture2D(
                        texture.width,
                        texture.height,
                        TextureFormat.RGBA32,
                        false);

                readableTexture.ReadPixels(
                    new Rect(
                        0,
                        0,
                        texture.width,
                        texture.height),
                    0,
                    0);

                readableTexture.Apply();

                byte[] png =
                    readableTexture.EncodeToPNG();

                string directory =
                    Path.Combine(
                        Main.ModPath,
                        "ToolOutput",
                        "Textures");

                Directory.CreateDirectory(directory);

                string path =
                    Path.Combine(
                        directory,
                        fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                            ? fileName
                            : fileName + ".png");

                File.WriteAllBytes(
                    path,
                    png);

                UnityEngine.Object.Destroy(readableTexture);

                Main.log.Log(
                    $"Exported texture to: {path}");
            }
            finally
            {
                RenderTexture.active = previous;

                RenderTexture.ReleaseTemporary(renderTexture);
            }
        }

        public static Sprite LoadSprite(string relativePath)
        {
            string path =
                Path.Combine(
                    Main.ModPath,
                    relativePath);

            if (!File.Exists(path))
            {
                Main.log.Error(
                    $"Could not find sprite file: {path}");

                return null;
            }

            byte[] data =
                File.ReadAllBytes(path);

            Texture2D texture =
                new Texture2D(
                    2,
                    2,
                    TextureFormat.RGBA32,
                    false);

            if (!texture.LoadImage(data))
            {
                Main.log.Error(
                    $"Could not load sprite texture: {path}");

                UnityEngine.Object.Destroy(texture);

                return null;
            }

            texture.name =
                Path.GetFileNameWithoutExtension(path);

            texture.wrapMode =
                TextureWrapMode.Clamp;

            texture.filterMode =
                FilterMode.Bilinear;

            Sprite sprite =
                Sprite.Create(
                    texture,
                    new Rect(
                        0,
                        0,
                        texture.width,
                        texture.height),
                    new Vector2(
                        0.5f,
                        0.5f),
                    100f);

            sprite.name =
                Path.GetFileNameWithoutExtension(path);

            return sprite;
        }
    }
}
