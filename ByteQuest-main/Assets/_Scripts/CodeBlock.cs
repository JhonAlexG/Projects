using UnityEngine;

public class CodeBlock : MonoBehaviour
{
    public bool isHolding = false;
    public Vector3 desiredRotation = new Vector3(0f, 0f, 0f); // La rotación deseada
    public Vector3 initialPosition; // Almacena la posición inicial Y
    public GameObject codeBlockObject;
    public PanelController panelController;
    private bool hasBeenAdded = false; // Interruptor para rastrear si el objeto ya ha sido agregado

    private void Start()
    {
        codeBlockObject = this.gameObject;
    }

    private void Update()
    {
        isKinematic();
    }

    void isKinematic()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (isHolding == true)
        {
            rb.isKinematic = false; // Esto evitará que el objeto responda a las fuerzas físicas
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CodePanel") && !isHolding && !hasBeenAdded)
        {
            // Agrega este objeto a la lista del PanelController
            panelController.objectsInPanel.Add(codeBlockObject);

            // Establece la posición y rotación del objeto
            codeBlockObject.transform.position = initialPosition;
            codeBlockObject.transform.rotation = Quaternion.Euler(desiredRotation);

            // Desactiva la física del objeto
            Rigidbody rb = codeBlockObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            // Establece el interruptor a true para que no se agregue nuevamente en este frame
            hasBeenAdded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CodePanel"))
        {
            // Remueve este objeto de la lista del PanelController
            panelController.objectsInPanel.Remove(codeBlockObject);

            // Restablece el interruptor cuando el objeto sale del trigger
            hasBeenAdded = false;
        }
    }
}
