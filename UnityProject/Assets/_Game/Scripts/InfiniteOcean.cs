using UnityEngine;

namespace Storky
{
    public sealed class InfiniteOcean : MonoBehaviour
    {
        public Transform follow;
        public float seaLevel;
        void LateUpdate()
        {
            if(!follow)return;
            // A camera-centred four-vertex surface. World-space shading prevents swimming.
            transform.position=new Vector3(Mathf.Floor(follow.position.x/128)*128,seaLevel,Mathf.Floor(follow.position.z/128)*128);
        }
    }
}
