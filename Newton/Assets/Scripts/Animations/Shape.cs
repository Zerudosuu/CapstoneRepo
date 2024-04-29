using DG.Tweening;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField]
    RectTransform canvasRectTransform; // Assuming you want to move a UI canvas element

    [SerializeField]
    float duration = 2f;

    private bool isOpen = false;

    void Start()
    {
        canvasRectTransform.DOAnchorPosY(0f, duration);
    }

    public void ExitQuestUI()
    {
        canvasRectTransform
            .DOAnchorPosY(-1090.81f, duration)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
