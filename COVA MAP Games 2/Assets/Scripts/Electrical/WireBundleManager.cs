using System.Collections.Generic;
using UnityEngine;

public class WireBundleManager : MonoBehaviour
{
   public enum WireMaterialType
   {
    white,
    black,
    blue,
    red,
   }
   //wire data structure
   [System.Serializable]
   public class Wire
   {
    public WireMaterialType wireType;
    public Color wireColor; //visual color representation
    public Sprite wireSprite; //determine unqiue object id for sprites cause multiple white and black wires
   }
   // wire bundle data structure
   [System.Serializable]
   public class WireBundle
   {
    public List<Wire> wires; //list of wires in bundles

    public WireBundle(int size)
    {
        wires = new List<Wire>(size);
    }
   }

   // List of all wire types and their properties
   public List<Wire> availableWires;

   // Function to create wire bundle of size and specific wires
   public WireBundle CreateWireBundle(int bundleSize)
   {
        if (bundleSize < 2 || bundleSize > 4)
        {
            Debug.LogError("Invalid bundle size. Size must be 2, 3, or 4.");
            return null;
        }
        List<Wire> usedWires = new List<Wire>();
        WireBundle newBundle = new WireBundle(bundleSize);

        for (int i = 0; i < bundleSize; i++)
        {
            Wire randomWire;
            do
            {
                randomWire = availableWires[Random.Range(0, availableWires.Count)];
            } while (usedWires.Contains(randomWire)); // Ensure no duplicate wire types

            usedWires.Add(randomWire);
            newBundle.wires.Add(randomWire);
        }

        return newBundle;
        }

    // Function to generate a random set of bundles (for testing or gameplay setup)
    public List<WireBundle> GenerateRandomBundles(int numberOfBundles)
    {
        List<WireBundle> bundles = new List<WireBundle>();
        int[] bundleSizes = { 2, 3, 4 };

        for (int i = 0; i < numberOfBundles; i++)
        {
            int randomSize = bundleSizes[Random.Range(0, bundleSizes.Length)];
            bundles.Add(CreateWireBundle(randomSize));
        }

        return bundles;
    }
     // Debugging: Visualize generated bundles in the console
    public void PrintBundle(WireBundle bundle)
    {
        string bundleInfo = "Wire Bundle: ";
        foreach (Wire wire in bundle.wires)
        {
            bundleInfo += $"[{wire.wireType} ({wire.wireColor})] ";
        }
        Debug.Log(bundleInfo);
    }
     // Example usage
    //private void Start()
    //{
        // Create and display 5 random bundles at startup
       // var bundles = GenerateRandomBundles(5);
        //foreach (var bundle in bundles)
        //{
           // PrintBundle(bundle);
       // }
    //}
}
