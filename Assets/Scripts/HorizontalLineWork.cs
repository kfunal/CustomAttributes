using UnityEngine;

public class HorizontalLineWork : MonoBehaviour
{
    [HorizontalLine(Thickness = 2, Padding = 20)]
    public int Variable;

    [HorizontalLine(Color = ".5,.5,.5,1")]
    public int OtherVariable;

    [HorizontalLine(Color = ".2,.3,.4,1")]
    public int OtherOtherVariable;
}
