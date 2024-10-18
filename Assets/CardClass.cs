using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass : MonoBehaviour
{
    // Start is called before the first frame update
    public CardType cardType;
    [TextArea]
    public string title;

    public string GetTitle()
    {
        return title;
    }
}

