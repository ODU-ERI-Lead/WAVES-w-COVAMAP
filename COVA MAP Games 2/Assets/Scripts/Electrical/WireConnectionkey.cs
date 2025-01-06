using System;
using System.Collections.Generic;
using UnityEngine;
using static IWireGroup;




public class WireConnectionkey : MonoBehaviour
{
    [Serializable]
    public class WireConnection
    {
        public int wireType;
        public List<int> connections;

    }
    

    public List<WireConnection> wireConnections = new List<WireConnection>();

    void Start()
    {
        // add specific wire connections here based on answer key
        AddWireConnections();
        PrintWireConnections();
    }

    void AddWireConnections()
    {
        wireConnections.Clear();

        // Example of specific wire connections need to change numbers to match key!
        wireConnections.Add(new WireConnection
        {
            wireType = 1,
            connections = new List<int> { 1, 2, 3 }
        });

        wireConnections.Add(new WireConnection
        {
            wireType = 2,
            connections = new List<int> { 4, 5 }
        });

        wireConnections.Add(new WireConnection
        {
            wireType = 3,
            connections = new List<int> { 6 }
        });

        wireConnections.Add(new WireConnection
        {
            wireType = 4,
            connections = new List<int> { 1, 6 }
        });
    }

    void PrintWireConnections()
    {
        Debug.Log("Wire Connections:");
        foreach (WireConnection wire in wireConnections)
        {
            string connectionString = string.Join(",", wire.connections);
            Debug.Log($"Wire Type {wire.wireType}: {connectionString}");
        }
    }
}
