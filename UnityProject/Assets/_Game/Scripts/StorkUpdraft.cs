using UnityEngine;

namespace Storky
{
    public sealed class StorkUpdraft : MonoBehaviour
    {
        public StorkPrototype player;
        public float radius=16, height=55, acceleration=1.4f;
        void Update()
        {
            if(!player)return;
            Vector3 p=player.transform.position-transform.position;
            if(p.y>=0 && p.y<height && p.x*p.x+p.z*p.z<radius*radius)player.AddUpdraft(acceleration);
            else player.ClearUpdraft();
        }
        void OnDisable(){if(player)player.ClearUpdraft();}
    }
}
