using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class UI_InteractionController : MonoBehaviour
{
    [SerializeField]
    GameObject UIController;

    [SerializeField]
    private GameObject BaseController;

    [SerializeField]
    InputActionReference inputActionReference_UISwitcher;

    bool isUICanvasActive = false;

    [SerializeField]
    GameObject UICanvasGameobject;

    [SerializeField]
    Vector3 positionOffsetForUICanvasGameobject;

    [SerializeField]
    GameObject lineVisual;
   

    private void OnEnable()
    {
        inputActionReference_UISwitcher.action.performed += ActivateUIMode;
        Debug.Log("press action added");
    }
    private void OnDisable()
    {
        // inputActionReference_UISwitcher.action.performed -= ActivateUIMode;
        // Debug.Log("press action removed");
    }

    private void Start()
    {
        //Deactivating UI Canvas Gameobject by default
        UICanvasGameobject.SetActive(false);

        //Deactivating UI Controller by default
        GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor>().blockUIOnInteractableSelection = false;
        lineVisual.SetActive(false);
    }

    /// <summary>
    /// This method is called when the player presses UI Switcher Button which is the input action defined in Default Input Actions.
    /// When it is called, UI interaction mode is switched on and off according to the previous state of the UI Canvas.
    /// </summary>
    /// <param name="obj"></param>
    private void ActivateUIMode(InputAction.CallbackContext obj)
    {
        Debug.Log("press succesfull");
        if (!isUICanvasActive)
        {
            isUICanvasActive = true;

            lineVisual.SetActive(true);
            GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor>().blockUIOnInteractableSelection = true;

            //Deactivating Base Controller by disabling its XR Direct Interactor
            //BaseController.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>().enabled = false;\
            BaseController.SetActive(false);

            //Adjusting the transform of the UI Canvas Gameobject according to the VR Player transform
            Vector3 positionVec = new Vector3(UIController.transform.position.x, positionOffsetForUICanvasGameobject.y, UIController.transform.position.z);
            Vector3 directionVec = UIController.transform.forward;
            directionVec.y = 0f;
            UICanvasGameobject.transform.position = positionVec + positionOffsetForUICanvasGameobject.magnitude * directionVec;
            UICanvasGameobject.transform.rotation = Quaternion.LookRotation(directionVec);

            //Activating the UI Canvas Gameobject
            UICanvasGameobject.SetActive(true);
        }
        else
        {
            BaseController.SetActive(true);
            isUICanvasActive = false;
            lineVisual.SetActive(false);
            UICanvasGameobject.SetActive(false);
        }

    }
}
