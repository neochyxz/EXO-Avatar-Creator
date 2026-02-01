using UnityEngine;

namespace Bozo.AnimeCharacters
{
public enum TextureType {Base, Decal, Pattern}
    public class TexturePackage : MonoBehaviour
    {
        public Texture texture;
        public Texture normalTexture;
        public Sprite icon;
        public TextureType type;
        public string catagory;
    }
}
