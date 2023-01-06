using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; // TwoPaneSplitView class

public class BTSplitView : TwoPaneSplitView
{
    // Allows to see this.class inside of UI Builder's library
    public new class UxmlFactory : UxmlFactory<BTSplitView, UxmlTraits> { }
}
