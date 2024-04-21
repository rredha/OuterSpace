using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour {

    public Transform currentCube;
    public Transform selectedCube;

    [SerializeField] private Transform playerOrigin;
    [SerializeField] private bool debugMode = true;
    [SerializeField] private float raycastDistance = 1.0f;

    public List<Transform> finalPath = new List<Transform>();

    RaycastHit playerHit;
    RaycastHit selectedHit;
    
    Ray ray;
    
    private void Start() {
       DebugRaycast();
       GetCurrentCube();
    }

    private void Update() {
       GetSelectedCube();
    }

    private void GenerateNextCubeToExplore() {
        List<Transform> nextCube = new List<Transform>();
        List<Transform> pastCube = new List<Transform>();

        foreach (var path in currentCube.GetComponent<IsWalkable>().GetPossiblePaths()) {
            if (path.isActive) {
                nextCube.Add(path.target);
                path.target.GetComponent<IsWalkable>().previousBlock = currentCube;
            }
        }
        pastCube.Add(currentCube);

        ExploreCube(nextCube, pastCube);
        BuildFinalPath();

    }

    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes) {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        if (current == selectedCube) {
            return;
        }

        foreach (var path in current.GetComponent<IsWalkable>().GetPossiblePaths()) {
            if (path.isActive) {
                nextCubes.Add(path.target);
                path.target.GetComponent<IsWalkable>().previousBlock = current;
            }
        }
        visitedCubes.Add(current);

        if (nextCubes.Any()) {
            ExploreCube(nextCubes, visitedCubes);
        }
    }


    private void BuildFinalPath() {
        Transform cube = selectedCube;

        while (cube != currentCube) {
            finalPath.Add(cube);
            if (cube.GetComponent<IsWalkable>().previousBlock != null) {
                cube = cube.GetComponent<IsWalkable>().previousBlock;
            } else {
                return;
            }
        }

        finalPath.Insert(0, selectedCube);
    }

    private void GetSelectedCube() {
        if (Input.GetMouseButtonDown(0)) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out selectedHit)) { 
                    if (selectedHit.transform.GetComponent<IsWalkable>() != null) {
                        selectedCube = selectedHit.transform;
                        GenerateNextCubeToExplore();
                    }
            }
        }
    }


    private void GetCurrentCube(){
        if (Physics.Raycast(playerOrigin.position, -playerOrigin.up, out playerHit, raycastDistance)) {
            if (playerHit.transform.GetComponent<IsWalkable>() != null) {
                currentCube = playerHit.transform;
            }
        }
    }

    private void DebugRaycast(){
        if (debugMode) {
            Debug.DrawRay(playerOrigin.position, -playerOrigin.transform.up*raycastDistance, Color.yellow);
        }
    }
}

