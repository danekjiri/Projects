using UnityEngine;

public class MapPreview : MonoBehaviour
{
    //map image representation
    public GameObject MapImage;

    //enable when mous on button
    public void CursonOnButton()
    {
        MapImage.SetActive(true);
    }

    //disable when mous out of button
    public void CursorExitButton()
    {
        MapImage.SetActive(false);
    }
}

