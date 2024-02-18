using Input;
using UnityEngine;

namespace DefaultNamespace
{
    public class EndingTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform carpenter;
        [SerializeField] private Transform musician;
        [SerializeField] private Transform librarian;
        
        public void Interact()
        {
            Vector3 rotation = new Vector3(0, -108.276f, 0);
            Vector3 librarianRotation = new Vector3(0, -118.032f, 0);

            carpenter.position = new Vector3(-226.821f, 4.27f, 221.62f);
            carpenter.eulerAngles = rotation;

            musician.position = new Vector3(-227.179f, 4.134f, 222.6f);
            musician.eulerAngles = rotation;

            librarian.position = new Vector3(-227.2133f, 4.321f, 223.7123f);
            librarian.eulerAngles = librarianRotation;
        }

    }
}