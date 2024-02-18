using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue
{
    public class MoveNPC : MonoBehaviour
    {
        [SerializeField] private Transform npc;
        
        public void MoveLibrarian()
        {
            npc.position = new Vector3(-225.27f, 4.173f, 218.91f);
        }
        
        public void MoveMusician()
        {
            npc.position = new Vector3(-223.055f, 4.235f, 219.878f);
        }
        
    }
}