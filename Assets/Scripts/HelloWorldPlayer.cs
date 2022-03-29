
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<Color> ColorPlayer = new NetworkVariable<Color>();
        public static List<Color> listaColores = new List<Color>();
        Renderer rend;

        void Start()
        {    
            rend = GetComponent<MeshRenderer>();
            Position.OnValueChanged += OnPositionChange;
            ColorPlayer.OnValueChanged += OnColorChange;
            if(IsServer && IsOwner){
                llenarListaColores();
            }                 
            SubmitColorRequestServerRpc(true);
                    
        }

        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Position.Value;
        }

        public void OnColorChange(Color previousValue, Color newValue){
            rend.material.color = ColorPlayer.Value;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }           
        }

        public void llenarListaColores(){
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
            SubmitColorRequestServerRpc();         
        }
        public void Move()
        {       
            SubmitPositionRequestServerRpc();
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
            
        }
        [ServerRpc]
        void SubmitColorRequestServerRpc(bool primeraVez = false, ServerRpcParams rpcParams = default)
        {
            Color old= ColorPlayer.Value;
            Color newColor = listaColores[Random.Range(0, listaColores.Count)];   
            listaColores.Remove(newColor);
            if(!primeraVez){
                listaColores.Add(old);
            }           
            ColorPlayer.Value= newColor;
        }
        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            //transform.position = Position.Value;       
            //rend.material.color = ColorPlayer.Value;
        }   
    }
}