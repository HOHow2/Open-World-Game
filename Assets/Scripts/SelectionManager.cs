using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public bool Ontarget;
    public static SelectionManager Instance { get; set; }
    public Camera playerCamera;

    public GameObject interaction_Info_UI;
    public GameObject SelectedObject;
    TextMeshProUGUI interaction_text;

    public Image HandIcon;
    public Image CenterDot;

    // Cutting down tree
    public GameObject selectedTree;
    public GameObject chopHolder;

    public GameObject selectedBot;
    public GameObject BotState;





    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            interaction interactable = selectionTransform.GetComponent<interaction>();


           
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            HumanisHit humanisHit = selectionTransform.GetComponent<HumanisHit>();

            // Tree
            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBechopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBechopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            // Bot
            if (humanisHit && humanisHit.playerInRange)
            {
                humanisHit.canBeHit = true;
                selectedBot = humanisHit.gameObject;
                BotState.gameObject.SetActive(true);
            }
            else
            {
                if (selectedBot != null)
                {
                    selectedBot.gameObject.GetComponent<HumanisHit>().canBeHit = false;
                    selectedBot = null;
                    BotState.gameObject.SetActive(false);
                }
            }



            if (interactable && interactable.Rangedetect)
            {
                Ontarget = true;
                SelectedObject = interactable.gameObject;
                interaction_Info_UI.SetActive(true);
                interaction_text.text = interactable.GetItemName();

                if (interactable.CompareTag("Pickable") && Ontarget == true)
                {

                    CenterDot.gameObject.SetActive(false);
                    HandIcon.gameObject.SetActive(true);

                }

            }
            else
            {
                Ontarget = false;
                interaction_Info_UI.SetActive(false);
                CenterDot.gameObject.SetActive(true);
                HandIcon.gameObject.SetActive(false);



            }

        }
        else
        {
            Ontarget = false;
            interaction_Info_UI.SetActive( false);
            CenterDot.gameObject.SetActive(true);
            HandIcon.gameObject.SetActive(false);
        }
    }

    public void DisableSelection()
    {
        HandIcon.enabled = false;
        CenterDot.enabled = false;
        interaction_Info_UI.SetActive(false);

        SelectedObject = null;



    }
    public void EnableSelection()
    {
        HandIcon.enabled = true;
        CenterDot.enabled = true;
        interaction_Info_UI.SetActive(true);

    }
}