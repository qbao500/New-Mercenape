using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class inputManager : MonoBehaviour
{
    //Written by Ossi Uusitalo
    //This script is put on the UI_Canvas for ease of access.
    public KeyCode[] inputs, defaultInputs;
    public GameObject[] inputButtons;
    public GameObject selectedInput;
    //You can set the then inputs from the editor
    [SerializeField]
    public KeyCode left, right, up, down, jump, climb, attack1, block;


    void Start()
    {
        //You could declare the inputs in editor, but there are so many of them to go through, that it's quicker to do in code.
        left = KeyCode.A;
        right = KeyCode.D;
        up = KeyCode.W;
        down = KeyCode.S;
        jump = KeyCode.Space;
        climb = KeyCode.E;
        attack1 = KeyCode.Mouse0;
        block = KeyCode.Mouse1;
        //VERY IMPORTANT

        //Make an array for the inputs to be compared when a button is pressed. I know this isn't as intuitive, but this is the best I could come up with.
        //Also, make sure that these inputs are in the same top-to-bottom, left-to-right order as they are on the menu panel. We do not want to mix them up. Nothing fatal though.
        inputs = new KeyCode[8];
        inputs[0] = left;
        inputs[1] = right;
        inputs[2] = up;
        inputs[3] = down;
        inputs[4] = jump;
        inputs[5] = climb;
        inputs[6] = attack1;
        inputs[7] = block;

        //Make sure to add the inputbuttons array in the same order as the array above.

        //Then we declare that these are the default setting that can be set on the input menu.
        defaultInputs = new KeyCode[inputs.Length];

        // Here we fill the default array
        if(defaultInputs != null)
        {
            for(int i = 0; i < defaultInputs.Length; i++)
            {
                defaultInputs[i] = inputs[i];
                // Set the text component in the input button to reflect the input it has.
                inputButtons[i].transform.GetComponentInChildren<Text>().text = inputs[i].ToString();

            }
        }
        //I know that with more inputs, I'll have to update this. Until I find a smoother solution, I'll do it manually. 

    }

    public void initiateInputChange()
    {
        selectedInput = EventSystem.current.currentSelectedGameObject;
        selectedInput.GetComponentInChildren<Text>().text = "Add new input";
        Debug.Log(selectedInput + " selected.");
    }

    public void changeInput(KeyCode newKey)
    {
        bool duplicate = false;
        //Here we check if the selected input is already in use.
        for(int i = 0; i < inputs.Length; i++)
        {
            if(newKey == inputs[i])
            {
                duplicate = true;
                break;
            }
        }

        //If no duplicates are found, we add the new input in the old inputs place.
        if(!duplicate)
        {
            Debug.Log("No duplicates found");
            for(int i = 0; i < inputButtons.Length; i++)
            {
                if(selectedInput == inputButtons[i])
                {
                    inputs[i] = newKey;
                    inputButtons[i].GetComponentInChildren<Text>().text = inputs[i].ToString();
                    
                    break;
                }
            }
        } else
        {
            Debug.Log("Duplicate input");

            for(int i = 0; i < inputButtons.Length; i++)
            {
                if(selectedInput == inputButtons[i])
                {
                    inputButtons[i].GetComponentInChildren<Text>().text = inputs[i].ToString();
                    Debug.Log("Reset the text");
                    break;
                }

            }
        }

        //After this, the selected button becomes null until another input button is pressed.
        selectedInput = null;
    }


    public void clearInput()
    {
        selectedInput = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Debug.Log(selectedInput.name + " is to be cleared.");
        for(int i = 0; i <inputButtons.Length; i++)
        {
            if (selectedInput == inputButtons[i])
            {
                inputs[i] = KeyCode.None;
                inputButtons[i].transform.GetComponentInChildren<Text>().text = "Empty";
                selectedInput = null;
                break;
            }
        }
    }

    public void setDefault()
    {
        // Here we put the default inputs to their respective slots, hopefully in the right order.
        for(int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = defaultInputs[i];
            inputButtons[i].transform.GetComponentInChildren<Text>().text = inputs[i].ToString();
            Debug.Log(inputs[i].ToString());
        }

        //for(int i = 0; i < inputs.Length; i++)
        //{
        //    defaul
        //}
    }

    private void OnGUI()
    {

        Event e = Event.current;
        KeyCode currentKey = e.keyCode;

        //If an input UI button's been pressed, this will check if the current input can be used to replace the old input.
        if (selectedInput != null)
        {

            if (e.isKey)
            {
                changeInput(currentKey);
            }
            //For a mouse button input, a switch statement is needed.
            else if (e.isMouse)
            {

                switch (e.button)
                {
                    case 0:
                        {
                            changeInput(KeyCode.Mouse0);
                            break;
                        }
                    case 1:
                        {
                            changeInput(KeyCode.Mouse1);
                            break;
                        }
                    case 2:
                        {
                            changeInput(KeyCode.Mouse2);
                            break;
                        }
                    case 3:
                        {
                            changeInput(KeyCode.Mouse3);
                            break;
                        }
                    case 4:
                        {
                            changeInput(KeyCode.Mouse4);
                            break;
                        }
                    case 5:
                        {
                            changeInput(KeyCode.Mouse5);
                            break;
                        }
                    case 6:
                        {
                            changeInput(KeyCode.Mouse6);
                            break;
                        }              
                    default:
                        {
                            break;
                        }

                }
                Debug.Log("Mouse " + e.button + " pressed for new input");
            }
        }

        else
        //We will check if the current input matches with any of the inputs in the inputs array.
        {
            //If a mouse key us oressed, this will activate and replace currentkey with the mouse button keycode. It's odd, but it works.
            if (e.isMouse)
            {
                switch (e.button)
                {
                    case 0:
                        {
                            currentKey = KeyCode.Mouse0;
                            break;
                        }
                    case 1:
                        {
                            currentKey = KeyCode.Mouse1;
                            break;
                        }
                    case 2:
                        {
                            currentKey = KeyCode.Mouse2;
                            break;
                        }
                    case 3:
                        {
                            currentKey = KeyCode.Mouse3;
                            break;
                        }
                    case 4:
                        {
                            currentKey = KeyCode.Mouse4;
                            break;
                        }
                    case 5:
                        {
                            currentKey = KeyCode.Mouse5;
                            break;
                        }
                    case 6:
                        {
                            currentKey = KeyCode.Mouse6;
                            break;
                        }

                    default:
                        { break; }
                }
            }
                for (int i = 0; i < inputs.Length; i++)
                {
                    //Here we compare the current input with the inputs array to see if it matches.
                    if (currentKey == inputs[i])
                    {
                        if (inputs[i] != KeyCode.None)
                        {
                            switch (i)
                            {
                                case 0:
                                    {
                                        Debug.Log("Left pressed");
                                        break;
                                    }
                                case 1:
                                    {
                                        Debug.Log("Right pressed");
                                        break;
                                    }
                                case 2:
                                    {
                                        Debug.Log("Up pressed");
                                        break;
                                    }
                                case 3:
                                    {
                                        Debug.Log("Down pressed");
                                        break;
                                    }
                                case 4:
                                    {
                                        Debug.Log("Jump pressed");
                                        break;
                                    }
                                case 5:
                                    {
                                        Debug.Log("Climb pressed");
                                        break;
                                    }
                                case 6:
                                    {
                                        Debug.Log("Attack1 pressed");
                                        break;
                                    }
                                case 7:
                                    {
                                        Debug.Log("Block pressed");
                                        break;
                                    }


                            }
                        }
                    }
                }      
        }
    }
}
