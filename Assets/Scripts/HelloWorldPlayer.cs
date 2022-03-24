
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        //public NetworkVariable<Color> color = new NetworkVariable<Color>();

       //public NetworkList<Material> materiales = new NetworkList<Material>();

        public static List<Color> listaColores = new List<Color>();
        /*
        {

            Color.blue, 
            Color.green,  
            Color.white,  
            Color.yellow, 
            Color.grey, 
            Color.magenta, 
            Color.red, 
            Color.black
        
        };*/

        //public List<Material> materiales = new List<Material>();
        
        

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




        public static void llenarListaColores(){
            listaColores.Add(Color.blue);
            listaColores.Add(Color.green);
            listaColores.Add(Color.white);
            listaColores.Add(Color.yellow); 
            listaColores.Add(Color.grey); 
            listaColores.Add(Color.magenta); 
            listaColores.Add(Color.red);
            listaColores.Add(Color.black);

        }
        public void ChangeColor()
        {

             if (NetworkManager.Singleton.IsServer)
            {
                llenarListaColores();

            }
            //rend.material = materiales[Random.Range(0, materiales.Count)];
            rend.material.color = listaColores[Random.Range(0, listaColores.Count)];
            
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