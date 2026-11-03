using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Storky.Editor
{
    public static class StorkPrototypeSetup
    {
        const string Root = "Assets/_Game/";
        public static string Build()
        {
            if (SceneManager.GetActiveScene().isDirty) throw new System.InvalidOperationException("Save the current scene before building the test area.");
            string scenePath = Root + "Scenes/FlightTest.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath)) throw new System.InvalidOperationException("FlightTest already exists; refusing to overwrite.");
            foreach (var folder in new[] {"Scenes", "Materials", "Animations", "Prefabs", "Textures"})
                if (!AssetDatabase.IsValidFolder(Root + folder)) AssetDatabase.CreateFolder("Assets/_Game", folder);
            string modelPath = Root + "Art/Stork/Stork_Hero.fbx";
            var importer = (ModelImporter)AssetImporter.GetAtPath(modelPath);
            importer.animationType = ModelImporterAnimationType.Generic;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.motionNodeName = "Root";
            importer.importAnimation = true;
            importer.animationCompression = ModelImporterAnimationCompression.Off;
            var clips = importer.defaultClipAnimations;
            foreach (var c in clips)
            {
                string name = c.name.Replace("Stork_", "");
                c.name = name;
                c.loopTime = name == "Idle" || name == "Walk" || name == "Fly" || name == "Glide";
                c.loopPose = c.loopTime;
                c.lockRootRotation = true; c.lockRootHeightY = true; c.lockRootPositionXZ = true;
                c.keepOriginalOrientation = true; c.keepOriginalPositionY = true; c.keepOriginalPositionXZ = true;
            }
            importer.clipAnimations = clips;
            importer.SaveAndReimport();
            var texImporter = (TextureImporter)AssetImporter.GetAtPath(Root + "Art/Stork/Stork_Palette.png");
            texImporter.filterMode = FilterMode.Point; texImporter.mipmapEnabled = false;
            texImporter.textureCompression = TextureImporterCompression.Uncompressed; texImporter.SaveAndReimport();
            var palette = AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "Art/Stork/Stork_Palette.png");
            var birdMaterial = Material("Stork_Palette", new Color(.95f,.95f,.95f));
            birdMaterial.SetTexture("_BaseMap", palette); birdMaterial.SetFloat("_Smoothness", 0);
            // One repeating texture supplies metre lines and ten-metre landmarks in one floor draw.
            const int resolution = 512;
            var grid = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            var pixels = new Color[resolution * resolution];
            for (int y=0;y<resolution;y++) for(int x=0;x<resolution;x++)
            {
                bool major = x < 5 || y < 5;
                bool minor = x % 51.2f < 1.8f || y % 51.2f < 1.8f;
                bool checker = ((int)(x/51.2f) + (int)(y/51.2f)) % 2 == 0;
                pixels[y*resolution+x] = major ? new Color(.16f,.23f,.27f) : minor ? new Color(.36f,.43f,.46f) : checker ? new Color(.62f,.67f,.68f) : new Color(.58f,.63f,.64f);
            }
            grid.SetPixels(pixels); grid.Apply();
            System.IO.File.WriteAllBytes(Root + "Textures/TrainingGrid.png", grid.EncodeToPNG());
            Object.DestroyImmediate(grid); AssetDatabase.ImportAsset(Root + "Textures/TrainingGrid.png");
            var gridImporter = (TextureImporter)AssetImporter.GetAtPath(Root + "Textures/TrainingGrid.png");
            gridImporter.wrapMode = TextureWrapMode.Repeat; gridImporter.anisoLevel = 8; gridImporter.SaveAndReimport();
            var floorMaterial = Material("TrainingGrid", Color.white);
            floorMaterial.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "Textures/TrainingGrid.png"));
            floorMaterial.SetTextureScale("_BaseMap", new Vector2(30,30)); floorMaterial.SetFloat("_Smoothness", 0);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(.65f,.70f,.76f);
            RenderSettings.fog = true; RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = new Color(.61f,.72f,.80f); RenderSettings.fogStartDistance = 100; RenderSettings.fogEndDistance = 230;
            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube); ground.name = "Training Ground - 300m - 1m grid";
            ground.transform.position = new Vector3(0,-.15f,0); ground.transform.localScale = new Vector3(300,.3f,300);
            ground.GetComponent<Renderer>().sharedMaterial = floorMaterial;
            var sun = new GameObject("Sun").AddComponent<Light>(); sun.type = LightType.Directional;
            sun.intensity = 1.4f; sun.shadows = LightShadows.Soft; sun.transform.rotation = Quaternion.Euler(48,-35,0);
            var camera = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener)).GetComponent<Camera>();
            camera.tag = "MainCamera"; camera.fieldOfView = 60; camera.farClipPlane = 350;
            camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = RenderSettings.fogColor;
            var player = new GameObject("Stork Player");
            var body = player.AddComponent<CharacterController>(); body.height = 1.35f; body.radius = .24f; body.center = new Vector3(0,.7f,0); body.stepOffset = .2f; body.skinWidth = .025f;
            var visualPivot = new GameObject("Visual").transform; visualPivot.SetParent(player.transform, false);
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(modelPath));
            model.transform.SetParent(visualPivot, false);
            // Export converts Blender -Y-facing bird to Unity +Z. Place the feet on the floor.
            var renderer = model.GetComponentInChildren<SkinnedMeshRenderer>(); renderer.sharedMaterial = birdMaterial;
            float bottom = renderer.bounds.min.y;
            visualPivot.localPosition = new Vector3(0,-bottom+.025f,0);
            var animator = model.GetComponent<Animator>(); if (!animator) animator = model.AddComponent<Animator>();
            var controller = AnimatorController.CreateAnimatorControllerAtPath(Root + "Animations/Stork.controller");
            var machine = controller.layers[0].stateMachine;
            foreach (var clip in AssetDatabase.LoadAllAssetsAtPath(modelPath).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")))
            { var state = machine.AddState(clip.name); state.motion = clip; if(clip.name=="Idle") machine.defaultState = state; }
            animator.runtimeAnimatorController = controller; animator.applyRootMotion = false; animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            var movement = player.AddComponent<StorkPrototype>(); movement.visual = visualPivot; movement.animator = animator; movement.followCamera = camera;
            player.transform.position = new Vector3(0,.05f,0);
            camera.transform.position = new Vector3(0,3,-5); camera.transform.LookAt(player.transform.position+Vector3.up*.9f);
            // Axis lanes and numbered beacons make distance and direction readable during flight.
            var blue = Material("North Blue", new Color(.08f,.45f,.85f));
            var orange = Material("East Orange", new Color(.95f,.39f,.1f));
            Lane("North lane +Z", new Vector3(-3,.008f,0), new Vector3(.15f,.012f,280), blue);
            Lane("East lane +X", new Vector3(0,.009f,-3), new Vector3(280,.012f,.15f), orange);
            for(int i=-100;i<=100;i+=25) if(i!=0)
            { Beacon(new Vector3(-5,0,i), blue, "Z " + i + " m"); Beacon(new Vector3(i,0,-5), orange, "X " + i + " m"); }
            PrefabUtility.SaveAsPrefabAsset(player, Root + "Prefabs/StorkPlayer.prefab");
            EditorSceneManager.SaveScene(scene, scenePath); AssetDatabase.SaveAssets();
            Selection.activeGameObject = player;
            return "Created FlightTest, six-clip Generic rig, prefab, grid and controller. Model renderer bounds: " + renderer.bounds;
        }
        static Material Material(string name, Color color)
        {
            var m = new Material(Shader.Find("Universal Render Pipeline/Lit")); m.name = name;
            m.SetColor("_BaseColor",color); m.SetFloat("_Smoothness",.05f);
            AssetDatabase.CreateAsset(m, Root + "Materials/" + name + ".mat"); return m;
        }
        static void Lane(string name, Vector3 p, Vector3 scale, Material material)
        {
            var o=GameObject.CreatePrimitive(PrimitiveType.Cube); o.name=name; o.transform.position=p; o.transform.localScale=scale;
            o.GetComponent<Renderer>().sharedMaterial=material; Object.DestroyImmediate(o.GetComponent<Collider>());
        }
        static void Beacon(Vector3 p, Material material, string label)
        {
            var o=GameObject.CreatePrimitive(PrimitiveType.Cube); o.name="Beacon " + label; o.transform.position=p+Vector3.up; o.transform.localScale=new Vector3(.4f,2,.4f); o.GetComponent<Renderer>().sharedMaterial=material;
            var t=new GameObject(label).AddComponent<TextMesh>(); t.transform.position=p+new Vector3(0,2.6f,0); t.text=label; t.characterSize=.23f; t.fontSize=48; t.anchor=TextAnchor.MiddleCenter;
        }
    }
}
