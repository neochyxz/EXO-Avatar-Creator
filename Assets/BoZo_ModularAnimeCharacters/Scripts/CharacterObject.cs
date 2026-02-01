using UnityEngine;

namespace Bozo.AnimeCharacters
{

    [CreateAssetMenu(fileName = "BMAC_CharacterObject", menuName = "BoZo/BMAC_CharacterObject")]
    public class CharacterObject : ScriptableObject
    {
        public Texture2D icon;
        public CharacterData data;
    }
}
