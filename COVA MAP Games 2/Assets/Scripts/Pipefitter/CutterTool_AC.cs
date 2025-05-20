using UnityEngine;
using DynamicMeshCutter;


[System.Serializable]
public class CutterTool_AC : CutterBehaviour

{
    // PlaneBehaviour  maybe unslash
    public Transform LeftRootTransform; //changed this from gameobject to transform
    public Transform RightRootTransform;
    public GameObject CutObjectParent;
    public GameObject targetMeshObject;
    public Transform spawnParent;
    public LineRenderer LR => GetComponent<LineRenderer>();


    protected override void Update()
    {
        base.Update();
        if (LeftRootTransform != null)
        {
            //Debug.LogWarning($"no left root transform found");
        }
        if (RightRootTransform != null)
        {
            //Debug.LogWarning($"no right root transform found");
        }
    }
    //attempt 1
   // void PositionRootsAtMeshEnds()
   // {
   //     MeshFilter meshFilter = targetMeshObject.GetComponent<MeshFilter>();
    //    if (meshFilter == null) return;

     //   Mesh mesh = meshFilter.sharedMesh;
     //   if (mesh == null) return;

        // Get mesh bounds in local space
     //   Bounds bounds = mesh.bounds;

     //   Vector3 leftPoint = new Vector3(bounds.min.x, 0, 0);
     //   Vector3 rightPoint = new Vector3(bounds.max.x, 0, 0);

        // Convert local mesh space to world space
     //   LeftRootTransform.position = targetMeshObject.transform.TransformPoint(leftPoint);
    //    RightRootTransform.position = targetMeshObject.transform.TransformPoint(rightPoint);
  //  }

    void CreateMeshAndReparent(GameObject originalObject)
    {
        Transform originalParent = originalObject.transform.parent;

        // Example: create a new mesh object (clone, modified, etc.)
        GameObject newMeshObj = Instantiate(originalObject);
        newMeshObj.name = "GeneratedMesh";
        newMeshObj.transform.SetParent(null); // Temporarily remove from hierarchy

        // ... Modify mesh on newMeshObj ...

        // Re-parent to original parent
        newMeshObj.transform.SetParent(originalParent);
    }
    //attempt 2?
    public void MyOnCreated(GameObject original, GameObject upperHull, GameObject lowerHull)
    {
        // Example: using the upper hull as the new mesh
        GameObject resultMesh = upperHull;

        // Parent the result to original’s parent
        resultMesh.transform.SetParent(original.transform.parent);
        resultMesh.transform.position = original.transform.position;
        resultMesh.transform.rotation = original.transform.rotation;

        // Find MeshFilter
        MeshFilter meshFilter = resultMesh.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning("MeshFilter not found on cut result.");
            return;
        }

        // Get bounds
        Bounds bounds = meshFilter.sharedMesh.bounds;
        Vector3 leftPos = resultMesh.transform.TransformPoint(new Vector3(bounds.min.x, 0, 0));
        Vector3 rightPos = resultMesh.transform.TransformPoint(new Vector3(bounds.max.x, 0, 0));

        // Position existing root markers
        if (LeftRootTransform != null) LeftRootTransform.position = leftPos;
        if (RightRootTransform != null) RightRootTransform.position = rightPos;

        // Optional: deactivate or destroy original
        original.SetActive(false);
    }
    public void SpawnMeshWithRoots()
    {
        // Instantiate the mesh
        GameObject meshInstance = Instantiate(targetMeshObject);
        meshInstance.name = "GeneratedMesh";

        // Parent it if needed
        if (spawnParent != null)
            meshInstance.transform.SetParent(spawnParent);

        // Get MeshFilter from instance
        MeshFilter meshFilter = meshInstance.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

        Mesh mesh = meshFilter.sharedMesh;
        Bounds bounds = mesh.bounds;

        // Create left/right root objects
        Transform leftRoot = spawnParent.Find("LeftRoot");
        Transform rightRoot = spawnParent.Find("RightRoot");
        leftRoot.name = "LeftRoot";
        rightRoot.name = "RightRoot";

        // Parent them to the mesh (optional)
        leftRoot.transform.SetParent(meshInstance.transform);
        rightRoot.transform.SetParent(meshInstance.transform);

        // Convert mesh bounds to world positions
        Vector3 leftPos = meshInstance.transform.TransformPoint(new Vector3(bounds.min.x, 0, 0));
        Vector3 rightPos = meshInstance.transform.TransformPoint(new Vector3(bounds.max.x, 0, 0));

        // Set positions
        leftRoot.transform.position = leftPos;
        rightRoot.transform.position = rightPos;
    }
    //attempt 3 but 2 maybe right?
    public void OnCreated(GameObject upperHull, GameObject lowerHull)
    {
        // Parent both the upper and lower hulls to the same parent as the original object
        upperHull.transform.SetParent(this.transform.parent);
        lowerHull.transform.SetParent(this.transform.parent);

        // Position the new mesh objects (upperHull and lowerHull) based on the original mesh
        upperHull.transform.position = this.transform.position;
        lowerHull.transform.position = this.transform.position;

        // Position root markers based on upper and lower hull bounds

        // Upper Hull marker positioning
        MeshFilter upperMeshFilter = upperHull.GetComponent<MeshFilter>();
        if (upperMeshFilter != null)
        {
            Bounds upperBounds = upperMeshFilter.sharedMesh.bounds;
            Vector3 upperLeftPos = upperHull.transform.TransformPoint(new Vector3(upperBounds.min.x, 0, 0));
            Vector3 upperRightPos = upperHull.transform.TransformPoint(new Vector3(upperBounds.max.x, 0, 0));

            if (LeftRootTransform != null) LeftRootTransform.position = upperLeftPos;
            if (RightRootTransform != null) RightRootTransform.position = upperRightPos;
        }

        // Lower Hull marker positioning
        MeshFilter lowerMeshFilter = lowerHull.GetComponent<MeshFilter>();
        if (lowerMeshFilter != null)
        {
            Bounds lowerBounds = lowerMeshFilter.sharedMesh.bounds;
            Vector3 lowerLeftPos = lowerHull.transform.TransformPoint(new Vector3(lowerBounds.min.x, 0, 0));
            Vector3 lowerRightPos = lowerHull.transform.TransformPoint(new Vector3(lowerBounds.max.x, 0, 0));

            if (LeftRootTransform != null) LeftRootTransform.position = lowerLeftPos;
            if (RightRootTransform != null) RightRootTransform.position = lowerRightPos;
        }
    }

    // Call the cut method when the cut needs to happen
    public void Cut(Mesh targetMesh, Vector3 worldposition, Vector3 worldNormal)
    {
        // Assuming this is how the cut method is called
        // The method cuts the target mesh at the given points and triggers the OnCreated callback
      //  Cut(targetMesh, worldposition, worldNormal, OnCut, MyOnCreated);
    }

    // Callback triggered after the cut happens
    public void MyOnCutCallback(GameObject upperHull, GameObject lowerHull)
    {
        // Handle anything else that should happen immediately after the cut, if necessary
    }
    

    

}







public class CutterTool_Data: PipefitterpartData
{


}