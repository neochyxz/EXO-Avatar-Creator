using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
    public class OutfitFollowBlendShapes : MonoBehaviour, IOutfitExtension
    {
        OutfitSystem system;
        SkinnedMeshRenderer mesh;
        SkinnedMeshRenderer followTarget;

        [SerializeField] OutfitType follow;
        [SerializeField] List<Vector2> shapes = new List<Vector2>();

        private bool isAlive = true;

        private void OnDestroy()
        {
            isAlive = false;

            if (system != null)
            {
                system.OnOutfitChanged -= OnNewSetUpHead;
            }

            mesh = null;
            followTarget = null;
            shapes.Clear();
        }

        private void OnNewSetUpHead(Outfit outfit)
        {
            if (!isAlive) return;
            if (outfit == null) return;
            if (outfit.Type != follow) return;
            if (!outfit.skinnedRenderer) return;

            followTarget = outfit.skinnedRenderer;
            SetUp();
        }

        private void SetUp()
        {
            if (!isAlive) return;

            mesh = GetComponentInChildren<SkinnedMeshRenderer>();

            if (!mesh || !mesh.sharedMesh) return;
            if (!followTarget || !followTarget.sharedMesh) return;
            if (followTarget.sharedMesh.blendShapeCount == 0) return;

            var characterShapeTitle = followTarget.sharedMesh.GetBlendShapeName(0);
            var sort = characterShapeTitle.Split(".");
            characterShapeTitle = sort.Length > 1 ? sort[0] + "." : "";

            shapes.Clear();

            var meshShared = mesh.sharedMesh;
            var followShared = followTarget.sharedMesh;

            for (int i = 0; i < meshShared.blendShapeCount; i++)
            {
                var shapeName = meshShared.GetBlendShapeName(i);
                sort = shapeName.Split(".");
                if (sort.Length > 1) shapeName = sort[1];

                int shapeIndex = followShared.GetBlendShapeIndex(characterShapeTitle + shapeName);
                if (shapeIndex != -1)
                {
                    shapes.Add(new Vector2(i, shapeIndex));
                }
            }
        }

        private void Update()
        {
            if (!isAlive) return;
            if (!mesh || !followTarget) return;
            if (!mesh.sharedMesh || !followTarget.sharedMesh) return;

            for (int i = 0; i < shapes.Count; i++)
            {
                if (!mesh || !followTarget) return;

                mesh.SetBlendShapeWeight(
                    (int)shapes[i].x,
                    followTarget.GetBlendShapeWeight((int)shapes[i].y)
                );
            }
        }

        public string GetID()
        {
            return "BlendShapeFollow";
        }

        public void Initalize(OutfitSystem outfitSystem, Outfit outfit)
        {
            system = outfitSystem;
            if (system == null || outfit == null) return;

            system.OnOutfitChanged += OnNewSetUpHead;

            mesh = outfit.skinnedRenderer;
            if (!mesh) return;

            var followOutfit = system.GetOutfit(follow);
            if (followOutfit == null || !followOutfit.skinnedRenderer) return;

            followTarget = followOutfit.skinnedRenderer;
            SetUp();
        }

        public void Execute(OutfitSystem outfitSystem, Outfit outfit) { }

        public object GetValue() => null;
        public Type GetValueType() => null;
    }
}
