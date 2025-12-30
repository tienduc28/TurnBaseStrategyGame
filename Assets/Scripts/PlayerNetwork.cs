using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int = 0, 
            _bool = false 
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }
    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log($"Random number changed from {previousValue._int} to {newValue._int} " +
                $"and {previousValue._bool} to {previousValue._bool} with {newValue.message} on client {OwnerClientId}");
        };
    }


    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            TestServerRpc(new ServerRpcParams());
            //randomNumber.Value = new MyCustomData
            //{
            //    _int = Random.Range(0, 100),
            //    _bool = false,
            //    message = "All your base are belong to us"
            //};
        }

        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector3.right;
        }

        float moveSpeed = 5f;
        transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
    }
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        //Debug.Log($"TestServerRpc called on the server from client {OwnerClientId} with {message}");
        Debug.Log($"TestServerRpc called on the server from client {serverRpcParams.Receive.SenderClientId}");
    }
}
