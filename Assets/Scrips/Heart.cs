using UnityEngine;

public class Heart : MonoBehaviour
{
    public Sprite onHeart;
    public Sprite offHeart;
    public SpriteRenderer sr;

    public int liveNum;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent(out sr);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.lives >= liveNum)
        {
            sr.sprite = onHeart;
        }
        else
        {
            sr.sprite = offHeart;
        }
    }
}
