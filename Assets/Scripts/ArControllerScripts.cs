using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class ArControllerScripts : MonoBehaviour
{
    public GameObject pulsatrixGO, eternusGO;
    public GameObject pulsatrixPrefab, eternusPrefab;
    public Toggle whatCreature; //off -> pulsatix //on -> Eternus
    public ARRaycastManager raycastManager;
    public GameObject arCamera;
    public UnityEngine.Touch touch;
    public Canvas canvas;
    public TMP_Text nameOfCreature, typeOfCreature, descriptionOfCreature;
    Vector2 canvasPos;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            GetScreenPosition();
            if (canvasPos.y > (-468))
            {
                List<ARRaycastHit> touches = new List<ARRaycastHit>();
                raycastManager.Raycast(Input.GetTouch(0).position, touches, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                var hitpose = touches[0].pose;

                if (!whatCreature.isOn) //Pulsatrix
                {
                    if (touches.Count > 0 && !pulsatrixGO)
                    {
                        SpawnAndFaceCamera(hitpose.position, pulsatrixPrefab,"Pulsatrix", true);
                        GetValuesOnUi("Pulsatrix");
                    }
                    else
                    {
                        pulsatrixGO.transform.position = hitpose.position;
                        SpawnAndFaceCamera(hitpose.position, pulsatrixPrefab, "Pulsatrix", false);
                        GetValuesOnUi("Pulsatrix");
                    }
                }
                else //Eternus
                {
                    if (touches.Count > 0 && !eternusGO)
                    {
                        SpawnAndFaceCamera(hitpose.position, eternusPrefab,"Eternus",true);
                        GetValuesOnUi("Eternus");
                    }
                    else
                    {
                        eternusGO.transform.position = hitpose.position;
                        SpawnAndFaceCamera(hitpose.position, eternusPrefab, "Eternus", false);
                        GetValuesOnUi("Eternus");
                    }
                }
            }
        }

    }
    public void GetScreenPosition()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 screenPos = touch.position; // Screen coordinates
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, canvas.worldCamera, out canvasPos);
            }
        }
    }
    public void SpawnAndFaceCamera(Vector3 spawnPosition, GameObject prefab, string creature, bool isFirstTime)
    {
        if (creature == "Pulsatrix")
        {
            if (isFirstTime == true)
            {
                pulsatrixGO = Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
            Vector3 lookPosition = new Vector3(arCamera.transform.position.x, pulsatrixGO.transform.position.y, arCamera.transform.position.z);
            pulsatrixGO.transform.LookAt(lookPosition);
            ZoomPinch.instance.spawned_object = pulsatrixGO;
            ZoomPinch.instance.object_spawned = true;
        }
        if(creature == "Eternus")
        {
            if (isFirstTime == true)
            {
                eternusGO = Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
            Vector3 lookPosition = new Vector3(arCamera.transform.position.x, eternusGO.transform.position.y, arCamera.transform.position.z);
            eternusGO.transform.LookAt(lookPosition);
            ZoomPinch.instance.spawned_object = eternusGO;
            ZoomPinch.instance.object_spawned = true;
        }
    }

    public void SpawnAtPosition(Vector3 position, GameObject modelToLookAt)
    {
        GameObject spawnedObject = Instantiate(modelToLookAt, position, Quaternion.identity);
        Vector3 targetPosition = new Vector3(arCamera.transform.position.x, spawnedObject.transform.position.y, arCamera.transform.position.z);
        spawnedObject.transform.LookAt(targetPosition);

    }

    public void GetValuesOnUi(string name)
    { 
        for (int i = 0;i< CreatureManager.instance.creaturesContainer.creature.Count; i++)
        {
            if (CreatureManager.instance.creaturesContainer.creature[i].name == name)
            {
                nameOfCreature.text = CreatureManager.instance.creaturesContainer.creature[i].name;
                typeOfCreature.text = "Tipo: " + CreatureManager.instance.creaturesContainer.creature[i].tipo;
                descriptionOfCreature.text = CreatureManager.instance.creaturesContainer.creature[i].descricao;
            }
        }

    }
}

