using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Storky.Editor
{
    public static class StorkIslandSetup
    {
        const string Root="Assets/_Game/";
        const string Fbx=Root+"Art/Environment/BirdIsland/Storky_BirdIsland.fbx";
        static Material Material(string name, Color color, string shader="Universal Render Pipeline/Lit")
        {
            string path=Root+"Materials/"+name+".mat";
            var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!mat){mat=new Material(Shader.Find(shader));AssetDatabase.CreateAsset(mat,path);}
            mat.SetColor("_BaseColor",color);mat.SetFloat("_Smoothness",0);mat.enableInstancing=true;
            return mat;
        }
        static int Layer(string name)
        {
            int found=LayerMask.NameToLayer(name);if(found>=0)return found;
            var tags=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layers=tags.FindProperty("layers");
            for(int i=8;i<32;i++)if(string.IsNullOrEmpty(layers.GetArrayElementAtIndex(i).stringValue))
            {layers.GetArrayElementAtIndex(i).stringValue=name;tags.ApplyModifiedPropertiesWithoutUndo();return i;}
            throw new InvalidOperationException("No free environment culling layer.");
        }
        public static string Build()
        {
            if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play before authoring.");
            var scene=SceneManager.GetActiveScene();
            if(scene.path!=Root+"Scenes/FlightTest.unity")throw new InvalidOperationException("Expected FlightTest scene.");
            if(scene.isDirty)throw new InvalidOperationException("Scene has unsaved user edits. Refusing to replace them.");
            var player=UnityEngine.Object.FindFirstObjectByType<StorkPrototype>();
            if(!player || player.gameObject.name!="Stork Player")throw new InvalidOperationException("Expected original Stork Player.");
            var importer=AssetImporter.GetAtPath(Fbx) as ModelImporter;
            if(!importer)throw new InvalidOperationException("Island FBX not imported.");
            var stone=Material("Island_Grey_Stone",new Color(.56f,.56f,.56f));
            var roofs=Material("Island_Grey_Roofs",new Color(.43f,.43f,.43f));
            var roads=Material("Island_Grey_Roads",new Color(.34f,.34f,.34f));
            var recess=Material("Island_Grey_Recesses",new Color(.24f,.24f,.24f));
            importer.globalScale=1;importer.useFileScale=true;importer.importAnimation=false;
            importer.importCameras=false;importer.importLights=false;importer.isReadable=false;
            importer.meshCompression=ModelImporterMeshCompression.Off;
            importer.importNormals=ModelImporterNormals.Import;importer.importTangents=ModelImporterTangents.None;
            importer.materialImportMode=ModelImporterMaterialImportMode.ImportStandard;
            importer.AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(Material),"Grey_Stone"),stone);
            importer.AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(Material),"Grey_Roofs"),roofs);
            importer.AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(Material),"Grey_Roads"),roads);
            importer.AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(Material),"Grey_Recesses"),recess);
            importer.SaveAndReimport();
            var model=AssetDatabase.LoadAssetAtPath<GameObject>(Fbx);
            if(!model)throw new InvalidOperationException("Island model load failed.");
            // Preserve the entire original bird hierarchy; remove only other scene roots.
            Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();Undo.SetCurrentGroupName("Replace training field with Bird Island");
            foreach(var go in scene.GetRootGameObjects())if(go!=player.gameObject)Undo.DestroyObjectImmediate(go);
            var island=(GameObject)PrefabUtility.InstantiatePrefab(model,scene);island.name="Bird Island - 600m";
            Undo.RegisterCreatedObjectUndo(island,"Add Bird Island");
            PrefabUtility.UnpackPrefabInstance(island,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            int detailLayer=Layer("IslandSmallDetails"),buildingLayer=Layer("IslandBuildings");
            var env= new GameObject("Environment");Undo.RegisterCreatedObjectUndo(env,"Add environment");island.transform.SetParent(env.transform,false);
            int colliders=0,lods=0;
            var renderers=island.GetComponentsInChildren<MeshRenderer>();
            foreach(var renderer in renderers)
            {
                var go=renderer.gameObject;var mf=go.GetComponent<MeshFilter>();
                string n=go.name;bool tree=n.StartsWith("Tree_");
                bool house=n.StartsWith("House_")||n.StartsWith("Ridge_House_")||n.StartsWith("Fisher_Cottage_");
                bool detail=tree||n.StartsWith("Market_Four")||n.StartsWith("Flat_Roof_Landing");
                bool surface=n.Contains("Street")||n.Contains("Lane")||n.Contains("Road")||n.Contains("Avenue")||n.Contains("Loop")||n.Contains("Access")||n.Contains("Promenade")||n.Contains("Link")||n.Contains("Courtyard")||n.Contains("Square")||n.Contains("Allotment");
                go.layer=detail?detailLayer:house?buildingLayer:0;
                GameObjectUtility.SetStaticEditorFlags(go,StaticEditorFlags.BatchingStatic|StaticEditorFlags.OccludeeStatic);
                renderer.shadowCastingMode=(surface||detail)?ShadowCastingMode.Off:ShadowCastingMode.On;
                renderer.receiveShadows=true;
                renderer.lightProbeUsage=LightProbeUsage.Off;renderer.reflectionProbeUsage=ReflectionProbeUsage.Off;
                if(tree)
                {
                    var c=go.AddComponent<MeshCollider>();c.sharedMesh=mf.sharedMesh;colliders++;
                }
                else if(!detail && mf && mf.sharedMesh)
                {var c=go.AddComponent<MeshCollider>();c.sharedMesh=mf.sharedMesh;colliders++;}
                if(detail||house)
                {
                    var lod=go.AddComponent<LODGroup>();lod.SetLODs(new[]{new LOD(detail?.022f:.0075f,new Renderer[]{renderer})});
                    lod.RecalculateBounds();lods++;
                }
            }
            var sun=new GameObject("Sun").AddComponent<Light>();sun.type=LightType.Directional;sun.intensity=1.45f;
            sun.color=new Color(1,.955f,.87f);sun.shadows=LightShadows.Soft;sun.shadowStrength=.8f;
            sun.transform.rotation=Quaternion.Euler(46,-32,0);sun.transform.SetParent(env.transform);
            RenderSettings.sun=sun;
            RenderSettings.ambientMode=AmbientMode.Trilight;
            RenderSettings.ambientSkyColor=new Color(.56f,.65f,.72f);RenderSettings.ambientEquatorColor=new Color(.46f,.48f,.49f);RenderSettings.ambientGroundColor=new Color(.29f,.30f,.31f);
            string skyPath=Root+"Materials/Island_Sky.mat";
            var sky=AssetDatabase.LoadAssetAtPath<Material>(skyPath);
            if(!sky){sky=new Material(Shader.Find("Storky/Island Sky"));AssetDatabase.CreateAsset(sky,skyPath);}
            sky.shader=Shader.Find("Storky/Island Sky");
            sky.SetColor("_HorizonColor",new Color(.50f,.66f,.73f));sky.SetColor("_ZenithColor",new Color(.22f,.48f,.72f));RenderSettings.skybox=sky;
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;
            RenderSettings.fogColor=new Color(.50f,.66f,.73f);RenderSettings.fogDensity=.0009f;
            var camera=new GameObject("Main Camera",typeof(Camera),typeof(AudioListener)).GetComponent<Camera>();camera.tag="MainCamera";
            camera.nearClipPlane=.15f;camera.farClipPlane=3000;camera.fieldOfView=65;camera.clearFlags=CameraClearFlags.Skybox;
            camera.allowHDR=false;camera.allowMSAA=true;camera.useOcclusionCulling=true;
            var cameraData=camera.GetUniversalAdditionalCameraData();cameraData.renderPostProcessing=false;
            cameraData.requiresColorTexture=false;cameraData.requiresDepthTexture=false;
            float[] distances=new float[32];distances[detailLayer]=330;distances[buildingLayer]=1500;
            camera.layerCullDistances=distances;camera.gameObject.AddComponent<IslandViewSettings>();
            var pipeline=GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if(pipeline)
            {
                // Scene-specific copy leaves the project's original quality assets intact.
                const string path=Root+"Island_RenderPipeline.asset";
                var tuned=AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);
                if(!tuned){AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(pipeline),path);tuned=AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);}
                tuned.supportsHDR=false;tuned.msaaSampleCount=2;tuned.renderScale=1;
                tuned.shadowDistance=110;tuned.shadowCascadeCount=2;tuned.mainLightShadowmapResolution=2048;
                tuned.supportsCameraDepthTexture=false;tuned.supportsCameraOpaqueTexture=false;
                tuned.useSRPBatcher=true;EditorUtility.SetDirty(tuned);
                QualitySettings.renderPipeline=tuned;
            }
            QualitySettings.vSyncCount=1;Application.targetFrameRate=60;
            QualitySettings.lodBias=1;QualitySettings.maximumLODLevel=0;
            // A two-triangle opaque surface covers the far clip even at altitude.
            const string seaMeshPath=Root+"Art/Environment/BirdIsland/OceanQuad.asset";
            var seaMesh=AssetDatabase.LoadAssetAtPath<Mesh>(seaMeshPath);
            if(!seaMesh)
            {
                seaMesh=new Mesh{name="Ocean_2_Triangles"};
                seaMesh.vertices=new[]{new Vector3(-4096,0,-4096),new Vector3(-4096,0,4096),new Vector3(4096,0,4096),new Vector3(4096,0,-4096)};
                seaMesh.triangles=new[]{0,1,2,0,2,3};seaMesh.RecalculateNormals();seaMesh.RecalculateBounds();AssetDatabase.CreateAsset(seaMesh,seaMeshPath);
            }
            var seaMat=Material("Island_Ocean",new Color(.07f,.30f,.40f),"Storky/Stylized Ocean");
            seaMat.SetColor("_CrestColor",new Color(.17f,.43f,.50f));
            var ocean=new GameObject("Ocean - camera centred - 2 triangles",typeof(MeshFilter),typeof(MeshRenderer),typeof(InfiniteOcean));
            ocean.transform.SetParent(env.transform);ocean.layer=4;ocean.GetComponent<MeshFilter>().sharedMesh=seaMesh;
            var seaRenderer=ocean.GetComponent<MeshRenderer>();seaRenderer.sharedMaterial=seaMat;seaRenderer.shadowCastingMode=ShadowCastingMode.Off;seaRenderer.receiveShadows=false;
            ocean.GetComponent<InfiniteOcean>().follow=camera.transform;
            // Spawn in the open forecourt, with the bird facing away from production.
            var court=renderers.First(r=>r.name=="Factory_Courtyard");
            var hall=renderers.First(r=>r.name=="Factory_01_Production_Hall");
            Vector3 forward=Vector3.ProjectOnPlane(court.bounds.center-hall.bounds.center,Vector3.up).normalized;
            Vector3 spawn=court.bounds.center+forward*18;
            Physics.SyncTransforms();
            if(!Physics.Raycast(spawn+Vector3.up*100,Vector3.down,out RaycastHit ground,200,1,QueryTriggerInteraction.Ignore))throw new InvalidOperationException("Factory spawn surface missing.");
            spawn.y=ground.point.y+.06f;
            var body=player.GetComponent<CharacterController>();body.enabled=false;player.transform.SetPositionAndRotation(spawn,Quaternion.LookRotation(forward));body.enabled=true;
            player.gameObject.layer=2;
            player.followCamera=camera;player.walkSpeed=1.4f;player.flightSpeed=14;player.maxAirSpeed=62;
            player.environmentMask=(1<<0)|(1<<detailLayer)|(1<<buildingLayer);
            camera.transform.position=spawn-forward*5.5f+Vector3.up*2.5f;camera.transform.LookAt(spawn+Vector3.up*.85f);
            // One light updraft, with no extra stamina regeneration.
            var thermal=new GameObject("Coastal Updraft");thermal.transform.SetParent(env.transform);
            var lightHouse=renderers.First(r=>r.name=="East_Headland_Lighthouse");thermal.transform.position=lightHouse.bounds.center+Vector3.up*18;
            var draft=thermal.AddComponent<StorkUpdraft>();draft.player=player;draft.radius=20;draft.height=60;draft.acceleration=1.4f;
            EditorUtility.SetDirty(player);EditorUtility.SetDirty(camera);EditorUtility.SetDirty(scene.GetRootGameObjects().First());
            if(SceneView.lastActiveSceneView)SceneView.lastActiveSceneView.LookAt(new Vector3(0,20,0),Quaternion.Euler(48,-35,0),550);
            Undo.CollapseUndoOperations(undo);
            PrefabUtility.SaveAsPrefabAssetAndConnect(island,Root+"Prefabs/BirdIsland.prefab",InteractionMode.AutomatedAction);
            AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            return $"Island ready. Renderers={renderers.Length}, Colliders={colliders}, screen-size culling groups={lods}, spawn={spawn}, facing={forward}, ocean triangles=2.";
        }
    }
}
