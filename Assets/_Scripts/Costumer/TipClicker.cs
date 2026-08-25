using UnityEngine;

public class TipClicker : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponentInParent<Costumer>().TipClick();
        Destroy(gameObject);
    }
}
