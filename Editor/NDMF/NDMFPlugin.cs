using jp.lilxyzw.ndmfmeshsimplifier.NDMF;
using jp.lilxyzw.ndmfmeshsimplifier.runtime;
using jp.lilxyzw.ndmfmeshsimplifier.UnityMeshSimplifier;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;

[assembly: ExportsPlugin(typeof(NDMFPlugin))]

namespace jp.lilxyzw.ndmfmeshsimplifier.NDMF
{
    internal class NDMFPlugin : Plugin<NDMFPlugin>
    {
        protected override void Configure()
        {
            InPhase(BuildPhase.Optimizing).BeforePlugin("com.anatawa12.avatar-optimizer").Run("Simplify meshes", ctx => {
                var components = ctx.AvatarRootObject.GetComponentsInChildren<NDMFMeshSimplifier>();
                foreach(var component in components)
                {
                    var renderer = component.GetComponent<SkinnedMeshRenderer>();
                    var filter = component.GetComponent<MeshFilter>();
                    if(renderer)
                    {
                        var mesh = Simplify(component, renderer.sharedMesh);
                        AssetDatabase.AddObjectToAsset(mesh, ctx.AssetContainer);
                        renderer.sharedMesh = mesh;
                    }
                    if(filter)
                    {
                        var mesh = Simplify(component, filter.sharedMesh);
                        AssetDatabase.AddObjectToAsset(mesh, ctx.AssetContainer);
                        filter.sharedMesh = mesh;
                    }
                    Object.DestroyImmediate(component);
                }
            });
        }

        private Mesh Simplify(NDMFMeshSimplifier component, Mesh source)
        {
            var meshSimplifier = new MeshSimplifier(source);
            meshSimplifier.SimplificationOptions = component.options;
            meshSimplifier.SimplifyMesh(component.quality);
            return meshSimplifier.ToMesh();
        }
    }
}
