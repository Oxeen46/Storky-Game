using UnityEngine;

namespace Storky
{
    [RequireComponent(typeof(Camera))]
    public sealed class IslandViewSettings : MonoBehaviour
    {
        public float viewDistance=3000, smallDetailDistance=330, buildingDistance=1500;
        void OnEnable()
        {
            var camera=GetComponent<Camera>();
            camera.farClipPlane=viewDistance;
            var distances=new float[32];
            int detail=LayerMask.NameToLayer("IslandSmallDetails"), buildings=LayerMask.NameToLayer("IslandBuildings");
            if(detail>=0)distances[detail]=smallDetailDistance;
            if(buildings>=0)distances[buildings]=buildingDistance;
            camera.layerCullDistances=distances;
            QualitySettings.vSyncCount=1;
            Application.targetFrameRate=60;
        }
    }
}
