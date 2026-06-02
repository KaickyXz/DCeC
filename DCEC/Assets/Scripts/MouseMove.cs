using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Vector2 spawnPoint;

    Rigidbody2D rb;

    public bool podeMovimentar = false;

    public GameObject mouseObj;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Cursor.lockState = CursorLockMode.Confined;


    }

    void FixedUpdate()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseObj.transform.position = new Vector2(mousePos.x, mousePos.y);

        if (podeMovimentar)
        {

            Vector2 targetPos = new Vector2(mousePos.x, mousePos.y);

            rb.MovePosition(targetPos);

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Olá");

            podeMovimentar = false;
            transform.position = new Vector2(-8, -2.8f);
        }

       
        if (col.gameObject.CompareTag("Finish"))
        {
            Debug.Log("VOCÊ VENCEU!");
        }

        if (col.gameObject.CompareTag("Semente"))
        {
            Debug.Log("sementado"); 
            podeMovimentar = true;
        }
    }
}