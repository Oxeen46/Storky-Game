using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Storky.Editor
{
    public static class StorkMotionCalibration
    {
        const string WalkPath="Assets/_Game/Animations/Stork_Walk_GroundMatched.anim";
        static void SolveLeg(Transform upper,Transform lower,Transform foot,Vector3 target,Vector3 pole)
        {
            Quaternion footRotation=foot.rotation;
            Vector3 a=upper.position,b=lower.position,c=foot.position;
            float ab=Vector3.Distance(a,b),bc=Vector3.Distance(b,c);
            Vector3 direction=(target-a).normalized;
            float distance=Mathf.Clamp(Vector3.Distance(a,target),Mathf.Abs(ab-bc)+.001f,(ab+bc)*.999f);
            Vector3 bend=Vector3.ProjectOnPlane(pole,direction).normalized;
            float adjacent=(ab*ab-bc*bc+distance*distance)/(2*distance);
            float altitude=Mathf.Sqrt(Mathf.Max(0,ab*ab-adjacent*adjacent));
            Vector3 knee=a+direction*adjacent+bend*altitude;
            upper.rotation=Quaternion.FromToRotation(b-a,knee-a)*upper.rotation;
            lower.rotation=Quaternion.FromToRotation(foot.position-lower.position,a+direction*distance-lower.position)*lower.rotation;
            foot.rotation=footRotation;
        }
        static void Tune(StorkPrototype p,float referenceSpeed)
        {
            p.walkSpeed=1.4f;p.walkCycleSpeed=referenceSpeed;
            p.rollRate=220;p.pitchRate=68;p.maxAirSpeed=62;
            p.flapSpeedImpulse=5.5f;p.flapLiftImpulse=3.2f;
            p.recoverySpeedImpulse=11;p.recoveryLift=9;p.recoveryDuration=2;
            EditorUtility.SetDirty(p);
        }
        public static string Bake()
        {
            if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play before calibration.");
            var player=UnityEngine.Object.FindFirstObjectByType<StorkPrototype>();
            if(!player)throw new InvalidOperationException("Stork Player not found.");
            var source=AssetDatabase.LoadAllAssetsAtPath("Assets/_Game/Art/Stork/Stork_Hero.fbx").OfType<AnimationClip>().First(c=>c.name=="Walk");
            var copy=UnityEngine.Object.Instantiate(source);copy.name="Walk";copy.frameRate=60;
            var clone=UnityEngine.Object.Instantiate(player.animator.gameObject);
            clone.hideFlags=HideFlags.HideAndDontSave;clone.transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);
            clone.GetComponent<Animator>().enabled=false;
            var all=clone.GetComponentsInChildren<Transform>();
            Transform body=all.First(t=>t.name=="Body");
            Transform[] upper={all.First(t=>t.name=="LegUpper.L"),all.First(t=>t.name=="LegUpper.R")};
            Transform[] lower={all.First(t=>t.name=="LegLower.L"),all.First(t=>t.name=="LegLower.R")};
            Transform[] feet={all.First(t=>t.name=="Foot.L"),all.First(t=>t.name=="Foot.R")};
            var animated=upper.Concat(lower).Concat(feet).ToArray();
            var curves=new Dictionary<string,AnimationCurve>();
            var paths=animated.ToDictionary(t=>t, t=>AnimationUtility.CalculateTransformPath(t,clone.transform));
            string bodyPath=AnimationUtility.CalculateTransformPath(body,clone.transform);
            foreach(string axis in new[]{"x","y","z"})curves[bodyPath+"|m_LocalPosition."+axis]=new AnimationCurve();
            foreach(var t in animated)
                foreach(string axis in new[]{"x","y","z","w"})curves[paths[t]+"|m_LocalRotation."+axis]=new AnimationCurve();
            var positions=new List<Vector3[]>();
            int samples=Mathf.CeilToInt(source.length*60);
            try
            {
                for(int i=0;i<=samples;i++)
                {
                    float time=source.length*i/samples;source.SampleAnimation(clone,time);
                    var targets=feet.Select(f=>clone.transform.InverseTransformPoint(f.position)).ToArray();
                    for(int side=0;side<2;side++)targets[side].z*=2.3f;
                    body.position-=clone.transform.up*.13f;
                    for(int side=0;side<2;side++)SolveLeg(upper[side],lower[side],feet[side],clone.transform.TransformPoint(targets[side]),-clone.transform.forward);
                    positions.Add(feet.Select(f=>clone.transform.InverseTransformPoint(f.position)).ToArray());
                    Vector3 p=body.localPosition;
                    curves[bodyPath+"|m_LocalPosition.x"].AddKey(time,p.x);
                    curves[bodyPath+"|m_LocalPosition.y"].AddKey(time,p.y);
                    curves[bodyPath+"|m_LocalPosition.z"].AddKey(time,p.z);
                    foreach(var t in animated)
                    {
                        Quaternion q=t.localRotation;
                        curves[paths[t]+"|m_LocalRotation.x"].AddKey(time,q.x);
                        curves[paths[t]+"|m_LocalRotation.y"].AddKey(time,q.y);
                        curves[paths[t]+"|m_LocalRotation.z"].AddKey(time,q.z);
                        curves[paths[t]+"|m_LocalRotation.w"].AddKey(time,q.w);
                    }
                }
                foreach(var item in curves)
                {
                    string[] parts=item.Key.Split('|');var curve=item.Value;
                    for(int i=0;i<curve.length;i++)
                    {AnimationUtility.SetKeyLeftTangentMode(curve,i,AnimationUtility.TangentMode.Linear);AnimationUtility.SetKeyRightTangentMode(curve,i,AnimationUtility.TangentMode.Linear);}
                    AnimationUtility.SetEditorCurve(copy,EditorCurveBinding.FloatCurve(parts[0],typeof(Transform),parts[1]),curve);
                }
                copy.EnsureQuaternionContinuity();
                var settings=AnimationUtility.GetAnimationClipSettings(copy);settings.loopTime=true;settings.loopBlend=true;AnimationUtility.SetAnimationClipSettings(copy,settings);
                var plantedSpeeds=new List<float>();
                for(int side=0;side<2;side++)
                {
                    float minY=positions.Min(p=>p[side].y);
                    for(int i=1;i<positions.Count;i++)
                    {
                        Vector3 a=positions[i-1][side],b=positions[i][side];
                        float backward=(a.z-b.z)/(source.length/samples);
                        if(a.y<minY+.006f && b.y<minY+.006f && backward>.05f)plantedSpeeds.Add(backward);
                    }
                }
                if(plantedSpeeds.Count<10)throw new InvalidOperationException("Not enough planted-foot samples to calibrate stride.");
                plantedSpeeds.Sort();float reference=plantedSpeeds[plantedSpeeds.Count/2];
                var existing=AssetDatabase.LoadAssetAtPath<AnimationClip>(WalkPath);
                if(existing){EditorUtility.CopySerialized(copy,existing);UnityEngine.Object.DestroyImmediate(copy);}
                else {AssetDatabase.CreateAsset(copy,WalkPath);existing=copy;}
                var controller=player.animator.runtimeAnimatorController as AnimatorController;
                var walkState=controller.layers[0].stateMachine.states.First(s=>s.state.name=="Walk").state;
                walkState.motion=existing;EditorUtility.SetDirty(walkState);EditorUtility.SetDirty(controller);
                Tune(player,reference);
                string prefabPath="Assets/_Game/Prefabs/StorkPlayer.prefab";
                var prefab=PrefabUtility.LoadPrefabContents(prefabPath);
                try {Tune(prefab.GetComponent<StorkPrototype>(),reference);PrefabUtility.SaveAsPrefabAsset(prefab,prefabPath);}
                finally{PrefabUtility.UnloadPrefabContents(prefab);}
                AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(player.gameObject.scene);EditorSceneManager.SaveScene(player.gameObject.scene);
                return $"Walk calibrated: foot-ground speed at 1x={reference:F4}m/s; ground speed={player.walkSpeed:F2}m/s; playback at full walk={player.walkSpeed/reference:F3}x; planted samples={plantedSpeeds.Count}. Original FBX untouched.";
            }
            finally {UnityEngine.Object.DestroyImmediate(clone);}
        }
    }
}
