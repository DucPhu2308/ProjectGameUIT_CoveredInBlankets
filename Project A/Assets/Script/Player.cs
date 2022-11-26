using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    List<GameObject> collected;
    public float velo = 0.05f;
    public Animator animator;
    public float trashOffset = 2f;
    public Rigidbody2D rb;

    public TextMeshProUGUI organicCountText;
    public int organicCount;
    public GameObject organicPrefab;

    public TextMeshProUGUI plasticCountText;
    public int plasticCount;
    public GameObject plasticPrefab;

    public GameObject gameWonCanvas;
    TrashType trashHolding = TrashType.None;
    // Start is called before the first frame update
    void Start()
    {
        collected = new List<GameObject>();
        organicCount = 3;
        plasticCount = 3;
        organicCountText.text = organicCount.ToString() + " x";
        plasticCountText.text = plasticCount.ToString() + " x";

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CollectedRender();
        CheckWin();
    }
    void CheckWin()
    {
        if (organicCount == 0 && plasticCount == 0)
        {
            //win
            gameWonCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed",Mathf.Sqrt(horizontal * horizontal + vertical * vertical));
        rb.velocity = new Vector2(horizontal,vertical)*velo;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<OrganicTrash>())
        {
            //organicCount--;
            //organicCountText.text = organicCount.ToString() + " x";
            //collision.gameObject.SetActive(false);
            collected.Add(collision.gameObject);
        }
        if (collision.gameObject.GetComponent<PlasticTrash>())
        {
            //plasticCount--;
            //plasticCountText.text = plasticCount.ToString() + " x";
            //collision.gameObject.SetActive(false);
            collected.Add(collision.gameObject);
        }
    }
    void CollectedRender()
    {

        for (int i =0; i<collected.Count; i++)
        {
            if (i==0)
                collected[0].transform.position = new Vector2(transform.position.x, transform.position.y + 2);
            else
                collected[i].transform.position = new Vector2(transform.position.x, transform.position.y + trashOffset * (i+1));
        }
    }

    protected void remove(PlasticBin bin)
    {
        int _flashticCount = 0;
        for (int i = 0; i < collected.Count; i++)
        {
            if (collected[i].GetComponent<PlasticTrash>())
            {
                _flashticCount++;
                Destroy(collected[i].gameObject);
                collected.RemoveAt(i);
                i--;
            }
        }
        plasticCount-= _flashticCount;
        plasticCountText.text = plasticCount.ToString() + " x";

    }
    protected void remove(OrganicBin bin)
    {
        int _flashticCount = 0;
        for (int i = 0; i < collected.Count; i++)
        {
            if (collected[i].GetComponent<OrganicTrash>())
            {
                _flashticCount++;
                Destroy(collected[i].gameObject);
                collected.RemoveAt(i);
                i--;
            }
        }
        organicCount -= _flashticCount;
        organicCountText.text = organicCount.ToString() + " x";

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collected.Count > 0)
        {
            var orgBin = collision.gameObject.GetComponent<OrganicBin>();
            if (orgBin)
            {
                remove(orgBin);
            }
            var flsBin = collision.gameObject.GetComponent<PlasticBin>();
            if (flsBin)
            {
                remove(flsBin);
            }
        }
    }
}
