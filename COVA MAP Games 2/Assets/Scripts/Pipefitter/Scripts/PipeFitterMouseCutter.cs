using DynamicMeshCutter;
using FuzzPhyte.Tools.Connections;
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.AI;
using JetBrains.Annotations;
public class PipeFitterMouseCutter : CutterBehaviour
{
    public LineRenderer LR => GetComponent<LineRenderer>();
    public float BladeLength = 2;
    public float BladeWidth = 0.002f;
    public float ZForwardDistanceLineRendererOffCam;
    [Header("Cut Parameters")]
    public bool MoveLeftPieceOnCut = true;
    public float SeparationDistance = 0.007f;
    [Space]
    [Header("Colors")]
    public Color ToolOnColor;
    public Color ToolAboutToCutColor;

    private Vector3 _endBladePt;
    private Vector3 _topOfBladePt;
    private Vector3 _from;
    private Vector3 _to;
    private bool _isDragging;
    private Plane cachedCuttingPlane;
    [Tooltip("Make sure to drop in the length pipe part")]
    public GameObject PipePrefab;
    public Camera CutCam;
    public Transform CutpartspawnTransform;
    // delegate attempt one for getting children to be moved
    public delegate void CutStateEvents();
    public event CutStateEvents OnFininshedCutting;
    public delegate void GetCutPart(GameObject CutpartLeft, GameObject GetCutPartaRight);
    public static event GetCutPart RetrievePartsFromCut;
    public delegate void CutPipeLength();
   

    public void Start()
    {
      
    }
    protected override void Update()
    {
        //START OF OLD BASE CODE
        base.Update();
        //END OF OLD BASE
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }
            /*
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;

                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 0.05f);
                _from = Camera.main.ScreenToWorldPoint(mousePos);
            }
            */
        if (_isDragging)
        {
            DrawCutTool(ToolAboutToCutColor);
        }
        else
        {
            DrawCutTool(ToolOnColor);
            //VisualizeLine(false,Vector3.zero,Vector3.zero);
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;
            Cut();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _isDragging = false;
    }
    protected override void CreateGameObjects(Info info)
    {

        //base.CreateGameObjects(info);
        //get the plane from info
        

        MeshCreationData creationInfo = MeshCreation.CreateObjects(info, DefaultMaterial, VertexCreationThreshold);
        Vector3 worldScale = info.MeshTarget.transform.lossyScale;
        if (info.MeshTarget != null)
        {
            Debug.LogWarning($"Mesh Target Found? {info.MeshTarget.gameObject.name}");
            if (info.MeshTarget.gameObject.GetComponent<MeshCollider>())
            {
                Destroy(info.MeshTarget.gameObject.GetComponent<MeshCollider>());
            }
        }
        //do we have our custom target information?
        Vector3 WorldLeftStartPos = Vector3.zero;
        Vector3 WorldRightEndPos = Vector3.zero;
        Vector3 WorldPivotMeshBeforeCut = Vector3.zero;
        Transform OldMesh = null;
        GameObject CutPartLeft = creationInfo.CreatedObjects[0];    
        GameObject CutPartRight = creationInfo.CreatedObjects[1];

        if (info.MeshTarget.gameObject.GetComponent<PipeFitterPipeTargetDetails>())
        {
            WorldLeftStartPos = info.MeshTarget.gameObject.GetComponent<PipeFitterPipeTargetDetails>().LeftEndPoint.transform.position;
            WorldRightEndPos = info.MeshTarget.gameObject.GetComponent<PipeFitterPipeTargetDetails>().RightEndPoint.transform.position;
            WorldPivotMeshBeforeCut = info.MeshTarget.gameObject.GetComponent<PipeFitterPipeTargetDetails>().ParentPivot.transform.position;
            OldMesh = info.MeshTarget.gameObject.GetComponent<PipeFitterPipeTargetDetails>().ParentPivot.transform;
        }
        if (DestroyTargets)
        {
            if (info.MeshTarget)
            {
                info.MeshTarget.transform.position = new Vector3(0, -10000, 0);
                if (info.MeshTarget.GameobjectRoot != null)
                    Destroy(info.MeshTarget.GameobjectRoot, 0);
                else
                    Destroy(info.MeshTarget.gameObject, 0);
            }
        }

        for (int i = 0; i < creationInfo.CreatedObjects.Length; i++)
        {
            if (creationInfo.CreatedObjects[i] == null)
            {
                Debug.Log("Dynamic Mesh Cutter: Cut supressed creation of object due to VertexCreationThreshold. Make sure you handle NullReferenceExceptions!");
            }
        }
        // ALL BE IN ONE COROUTINE
        StartCoroutine(MainPartCoroutine(info,creationInfo,OldMesh,CutPartLeft,CutPartRight,worldScale,WorldPivotMeshBeforeCut,WorldLeftStartPos,WorldRightEndPos));
        /*
        for (int i = 0; i < creationInfo.CreatedObjects.Length; i++)
        {
            GameObject anObject = creationInfo.CreatedObjects[i];
            if (anObject.transform.childCount > 0)
            {
                anObject.transform.GetChild(0).transform.localScale = worldScale;
                StartCoroutine(GeneratePipeFromPart(anObject.transform,WorldPivotMeshBeforeCut, WorldLeftStartPos, WorldRightEndPos,i));
            }
            //update scale
            //anObject.transform.localScale = worldScale;
        }
        //destroy actual pipe
        Destroy(OldMesh.gameObject);
        info.OnCreatedCallback?.Invoke(info, creationInfo);
        //using here isnt getting all part components unslash if need be
        // seeing if a delayed coroutine works, originally was just post cut etc.
        StartCoroutine(DelayedPostCut( CutPartLeft, CutPartRight));
        Debug.Log("Should have ran and invked post cut parts should be assigned");
        */
        //
    }
    
    private IEnumerator MainPartCoroutine(Info info,MeshCreationData creationInfo, Transform OldMesh, GameObject PartLeft, GameObject PartRight, Vector3 worldScale, Vector3 WorldPivotMeshBeforeCut,Vector3 WorldLeftStartPos,Vector3 WorldRightEndPos)
    {
        for (int i = 0; i < creationInfo.CreatedObjects.Length; i++)
        {
            GameObject anObject = creationInfo.CreatedObjects[i];
            if (anObject.transform.childCount > 0)
            {
                anObject.transform.GetChild(0).transform.localScale = worldScale;
               
                if (anObject.transform.GetChild(0).GetComponent<MeshCollider>())
                {
                    //remove the collider
                    Destroy(anObject.transform.GetChild(0).GetComponent<MeshCollider>());
                }
                yield return StartCoroutine(GeneratePipeFromPart(anObject.transform, WorldPivotMeshBeforeCut, WorldLeftStartPos, WorldRightEndPos, i));

            }
            //update scale
            //anObject.transform.localScale = worldScale;
        }
        //destroy actual pipe
        Destroy(OldMesh.gameObject);
        info.OnCreatedCallback?.Invoke(info, creationInfo);
        //using here isnt getting all part components unslash if need be
        // seeing if a delayed coroutine works, originally was just post cut etc.
        //yield return StartCoroutine(DelayedPostCut(PartLeft, PartRight));

        // Now safely fire the event
        RetrievePartsFromCut?.Invoke(PartLeft.transform.parent.gameObject, PartRight.transform.parent.gameObject);
        if (RetrievePartsFromCut != null)
        {
            Debug.Log(" Parts should be retrieved and ready to be passed");
        }
        //LAST THING THAT OCCURS
        OnFininshedCutting?.Invoke();
        Debug.Log("Should have ran and invked post cut parts should be assigned");
    }
    private IEnumerator GeneratePipeFromPart(Transform newMesh, Vector3 originalPivot, Vector3 leftEdgeBeforeCut, Vector3 rightEdgeBeforeCut, int childIndex)
    {
        var PipePart = GameObject.Instantiate(PipePrefab, originalPivot, Quaternion.identity);
        //first child is the visual

        GameObject oldVisual = PipePart.transform.GetChild(0).gameObject;
        Destroy(oldVisual);
        newMesh.transform.SetParent(PipePart.transform);
        yield return new WaitForEndOfFrame();
        var pipeCode = PipePart.GetComponent<ConnectionPart>();

        Vector3 pipeDirection = rightEdgeBeforeCut - leftEdgeBeforeCut;
        Vector3 planeNormal = cachedCuttingPlane.normal;
        float planeDistance = cachedCuttingPlane.distance;
        float numerator = -(Vector3.Dot(planeNormal, leftEdgeBeforeCut) + planeDistance);
        float denominator = Vector3.Dot(planeNormal, pipeDirection);
        float t = numerator / denominator;
        Vector3 intersection = leftEdgeBeforeCut + (t * pipeDirection);
       
        //Debug.LogError($"I MADE IT THIS FAR:{pipeCode.gameObject.name} has # {pipeCode.ConnectionPointParent.childCount} children");
        if (pipeCode)
        {
           if(pipeCode.ConnectionPointParent.transform.childCount == 2)
           {
                //fix on two but this won't work for anything else other than a straight pipe
                var leftEndPoint = pipeCode.ConnectionPointParent.GetChild(0);
                var rightEndPoint = pipeCode.ConnectionPointParent.GetChild(1);
                if (rightEndPoint.GetComponent<BoxCollider>() && leftEndPoint.GetComponent<BoxCollider>())
                {
                    leftEndPoint.gameObject.GetComponent<BoxCollider>().enabled = false;
                    rightEndPoint.gameObject.GetComponent<BoxCollider>().enabled = false;
                    
                }
                var CCollider = pipeCode.ColliderParent.GetChild(0).gameObject.GetComponent<CapsuleCollider>();
               
                float newPipeLength = 0;
                if (childIndex == 0)
                {
                    //update rightEndPoint
                    //cachedCuttingPlane
                    rightEndPoint.transform.position = intersection;
                    
                    Vector3 worldMid = (leftEndPoint.transform.position + intersection) / 2f;
                    Vector3 localMid = CCollider.gameObject.transform.InverseTransformPoint(worldMid);
                    newPipeLength = Vector3.Distance(rightEndPoint.position, leftEndPoint.position);
                    
                    
                    if (CCollider != null)
                    {
                        CCollider.center = localMid;
                        //JOHN: need to incorporate radius and remove it from the height/length of our pipe
                        //if distance is greater than 6" or roughly 0.2142 we should reduce by 3F
                        if (newPipeLength > 0.2142f)
                        {
                            CCollider.height = newPipeLength - (CCollider.radius * 4f);
                        }
                        else
                        {
                            CCollider.height = newPipeLength - (CCollider.radius * 2f);
                        }
                    }
                    if (MoveLeftPieceOnCut)
                    {
                        PipePart.transform.position += new Vector3(-SeparationDistance, 0, 0);
                    }
                }
                    
                if (childIndex == 1)
                {
                    //update leftEndpoint
                    leftEndPoint.transform.position = intersection;
                    Vector3 worldMid = (rightEndPoint.transform.position + intersection) / 2f;
                    Vector3 localMid = CCollider.gameObject.transform.InverseTransformPoint(worldMid);
                    newPipeLength = Vector3.Distance(rightEndPoint.position, leftEndPoint.position);
                    if (CCollider != null)
                    {
                        CCollider.center = localMid;
                        if (newPipeLength > 0.2142f)
                        {
                            CCollider.height = newPipeLength - (CCollider.radius * 4f);
                        }
                        else
                        {
                            CCollider.height = newPipeLength - (CCollider.radius * 2f);
                        }
                    }
                    if (MoveLeftPieceOnCut)
                    {
                        PipePart.transform.position += new Vector3(SeparationDistance, 0, 0);
                    }
                    else
                    {
                        //only right is moving
                        PipePart.transform.position += new Vector3(SeparationDistance*2f, 0, 0);
                    }
                    
                }
                //Debug.LogError($"BEFORE Wait for end of frame");

                yield return new WaitForEndOfFrame();
                //Debug.LogError($"After Wait for end of frame");
                if (rightEndPoint.GetComponent<BoxCollider>() && leftEndPoint.GetComponent<BoxCollider>())
                {
                    leftEndPoint.gameObject.GetComponent<BoxCollider>().enabled = true;
                    rightEndPoint.gameObject.GetComponent<BoxCollider>().enabled = true;

                }
                //update the visual information
                pipeCode.MyVisualItem = newMesh.gameObject;
                newMesh.transform.SetAsFirstSibling();
                var meshTargetFound = newMesh.GetChild(0).gameObject.GetComponent<MeshTarget>();
                //Debug.LogError($"Wtf am I {meshTargetFound}");
                var rootParentTransform = pipeCode.gameObject.transform;

                //add our component
                var details = rootParentTransform.gameObject.AddComponent<PipeFitterPipeTargetDetails>();
                if (meshTargetFound != null)
                {
                    details.ReferenceToMeshTarget = meshTargetFound;
                } 
                details.ConnectionData = pipeCode;
                details.ParentPivot = PipePart;
                details.PipeMesh = newMesh.gameObject;
                details.LeftEndPoint = leftEndPoint.gameObject;
                details.RightEndPoint = rightEndPoint.gameObject;
                details.UpdateLength(newPipeLength);
                Debug.Log("cut pipe length asserted"+ newPipeLength.ToString());
                //manual move about of resetting our pivot point
                List<Transform> mfChildren = new List<Transform>();
                //update details to PartChecker
                var partCheckerRef = pipeCode.ColliderParent.GetChild(0).GetComponent<PartChecker>();
                if (partCheckerRef!=null)
                {
                    partCheckerRef.PassData(details);
                }
                else
                {
                    Debug.LogError("PartChecker reference is null, please check your setup.");
                }
                (bool success, Vector3 newWorldPivot) = details.ReturnWorldMidPoint();
                if (success)
                {
                    var newParent = GameObject.Instantiate(new GameObject(), newWorldPivot, Quaternion.identity);
                    for (int i = 0; i < pipeCode.transform.childCount; i++)
                    {
                        var aChild = pipeCode.transform.GetChild(i);
                        mfChildren.Add(aChild);
                        aChild.SetParent(null);
                    }
                    pipeCode.gameObject.transform.SetParent(newParent.transform);
                    pipeCode.gameObject.transform.localPosition = Vector3.zero;
                    pipeCode.gameObject.transform.SetParent(null);
                    for (int i = 0; i<mfChildren.Count; i++) 
                    {
                        var aChild = mfChildren[i];
                        aChild.SetParent(pipeCode.transform);
                    }
                    foreach(GameObject aCollider in pipeCode.MyBodyColliders)
                    {
                        if (aCollider.GetComponent<CapsuleCollider>())
                        {
                            aCollider.GetComponent<CapsuleCollider>().center = Vector3.zero;
                        }
                    }
                    Destroy(newParent);
                }
                
               

                //end of manual move
            }
        }
    }
    private void DrawCutTool(Color currentColor)
    {
        var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, CutCam.nearClipPlane + ZForwardDistanceLineRendererOffCam);
        _to = CutCam.ScreenToWorldPoint(mousePos);
        _endBladePt = _to - new Vector3(0, BladeLength, 0);
        //assuming we are facing Z here with this one
        _topOfBladePt = _to + new Vector3(0, 0, -1f);

        VisualizeLine(true, _to, _endBladePt,currentColor);
    }

    private void Cut()
    {
        Plane plane = new Plane(_endBladePt, _to, _topOfBladePt);
        cachedCuttingPlane = plane;
        var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            if (!root.activeInHierarchy)
                continue;
            var targets = root.GetComponentsInChildren<MeshTarget>();
            foreach (var target in targets)
            {
                Cut(target, _to, plane.normal, null, OnCreated);
            }
        }
    }

    void OnCreated(Info info, MeshCreationData cData)
    {
        //see if it works here delete if need be, postcut tht is.
        //post cut does work here but same issue as before, collider and connection do not move on cut. unslash if need be
      //  GameObject CutPartLeft = cData.CreatedObjects[0];
      //  GameObject CutPartRight = cData.CreatedObjects[1];
        MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
      //  PostCut(CutPartLeft, CutPartRight);
      //  Debug.Log("Should have ran and invked post cut parts should be assigned");
    }
    private void VisualizeLine(bool value, Vector3 startPt,Vector3 endPt, Color lineColor )
    {
        if (LR == null)
            return;

        LR.enabled = value;
        LR.material.color = lineColor;
        if (value)
        {
            LR.positionCount = 2;
            LR.startWidth = BladeWidth;
            LR.endWidth = BladeWidth;
            LR.SetPosition(0, startPt);
            LR.SetPosition(1, endPt);
        }
    }

    private IEnumerator DelayedPostCut(GameObject cutLeft, GameObject cutRight)
    {
        // Wait one frame (or you can use WaitForSeconds(0.1f))
        yield return new WaitForEndOfFrame();

        // Now safely fire the event
        RetrievePartsFromCut?.Invoke(cutLeft, cutRight);
        if (RetrievePartsFromCut != null)
        {
            Debug.Log(" Parts should be retrieved and ready to be passed");
        }
        //LAST THING THAT OCCURS
        OnFininshedCutting?.Invoke();
    }

   
    //delete right side of cut pipe then transform to assembly area 
    public void PostCutMove(Info info, int childIndex)
    {
        //cData.CreatedObjects.GetChild(0).transform , need to know left side cut code reference
        // childIndex           ;
        // GeneratePipeFromPart
        // StartCoroutine(GeneratePipeFromPart())
      //  var cutpipepart = GetComponent < childIndex(0) >;
    }


}
