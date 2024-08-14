using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using jp.lilxyzw.ndmfmeshsimplifier.NDMF;
using jp.lilxyzw.ndmfmeshsimplifier.runtime;
using jp.lilxyzw.ndmfmeshsimplifier.UnityMeshSimplifier;
using nadena.dev.ndmf;
using nadena.dev.ndmf.preview;
using UnityEditor;
using UnityEngine;

[assembly: ExportsPlugin(typeof(NDMFPlugin))]

namespace jp.lilxyzw.ndmfmeshsimplifier.NDMF
{
    internal class NDMFPlugin : Plugin<NDMFPlugin>
    {
        public override string DisplayName => "lilNDMFMeshSimplifier";
        protected override void Configure()
        {
            InPhase(BuildPhase.Optimizing).BeforePlugin("com.anatawa12.avatar-optimizer").Run("Simplify meshes", ctx =>
            {
                var components = ctx.AvatarRootObject.GetComponentsInChildren<NDMFMeshSimplifier>();
                foreach (var component in components)
                {
                    var renderer = component.GetComponent<SkinnedMeshRenderer>();
                    var filter = component.GetComponent<MeshFilter>();
                    if (renderer)
                    {
                        var mesh = Simplify(component, renderer.sharedMesh);
                        AssetDatabase.AddObjectToAsset(mesh, ctx.AssetContainer);
                        renderer.sharedMesh = mesh;
                    }
                    if (filter)
                    {
                        var mesh = Simplify(component, filter.sharedMesh);
                        AssetDatabase.AddObjectToAsset(mesh, ctx.AssetContainer);
                        filter.sharedMesh = mesh;
                    }
                    Object.DestroyImmediate(component);
                }
            }).PreviewingWith(new PreviewNDMFMeshSimplifier())
            ;
        }

        internal static Mesh Simplify(NDMFMeshSimplifier component, Mesh source)
        {
            var meshSimplifier = new MeshSimplifier(source);
            meshSimplifier.SimplificationOptions = component.options;
            meshSimplifier.SimplifyMesh(component.quality);
            return meshSimplifier.ToMesh();
        }
    }

    internal class PreviewNDMFMeshSimplifier : IRenderFilter
    {
        public ImmutableList<RenderGroup> GetTargetGroups(ComputeContext context)
        {
            return context.GetComponentsByType<NDMFMeshSimplifier>()
            .Select(c => (c, context.GetComponent<Renderer>(c.gameObject)))
            .Where(p => p.Item2 is SkinnedMeshRenderer || p.Item2 is MeshRenderer)
            .Select(p => RenderGroup.For(p.Item2))
            .ToImmutableList();
        }

        public Task<IRenderFilterNode> Instantiate(RenderGroup group, IEnumerable<(Renderer, Renderer)> proxyPairs, ComputeContext context)
        {
            var ndmfMeshSimplifier = group.Renderers.First().GetComponent<NDMFMeshSimplifier>();
            var targetRenderer = proxyPairs.First().Item2;
            var mesh = default(Mesh);

            switch (targetRenderer)
            {
                case SkinnedMeshRenderer smr: { mesh = smr.sharedMesh; break; }
                case MeshRenderer mr:
                    {
                        var mf = mr.GetComponent<MeshFilter>();
                        if (mf == null) { break; }
                        mesh = mf.sharedMesh;
                        break;
                    }
            }
            if (mesh == null) { return null; }
            context.Observe(ndmfMeshSimplifier);
            context.Observe(mesh);
            var simplifiedMesh = NDMFPlugin.Simplify(ndmfMeshSimplifier, mesh);
            return Task.FromResult<IRenderFilterNode>(new MeshSimplifierNode(simplifiedMesh));
        }

        internal class MeshSimplifierNode : IRenderFilterNode
        {
            public RenderAspects WhatChanged => RenderAspects.Mesh;

            Mesh _simplifiedMesh;

            public MeshSimplifierNode(Mesh mesh)
            {
                _simplifiedMesh = mesh;
            }

            public void OnFrame(Renderer original, Renderer proxy)
            {
                switch (proxy)
                {
                    case SkinnedMeshRenderer smr: { smr.sharedMesh = _simplifiedMesh; return; }
                    case MeshRenderer mr:
                        {
                            var mf = mr.GetComponent<MeshFilter>();
                            if (mf == null) { return; }
                            mf.sharedMesh = _simplifiedMesh; return;
                        }
                }
            }

        }
    }
}
