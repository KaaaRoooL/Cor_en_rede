
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;




namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

       //public NetworkList<Material> materiales = new NetworkList<Material>();

        

        public List<Material> materiales = new List<Material>();
        

        Renderer rend;

        void Start()
        {
            rend = GetComponent<MeshRenderer>();

        }


        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                ChangeColor();
                Move();
            }
        }



        public void ChangeColor()
        {

            rend.material = materiales[Random.Range(0, 10)];

          
        }







        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;

            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();

        }




        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;

        }
    }
}