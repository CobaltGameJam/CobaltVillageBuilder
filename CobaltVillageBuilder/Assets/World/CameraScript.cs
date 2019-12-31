using Assets.Scripts;
using UnityEngine;


public class GlobalMethods
{
    public static float Ease(float curVal, float targetVal, float speed)
    {
        return curVal + (targetVal - curVal) * speed;
    }

    public static int Ease(int curVal, int targetVal, float speed)
    {
        return (int)(curVal + (targetVal - curVal) * speed);
    }

    public static double Ease(double curVal, double targetVal, float speed)
    {
        return (double)(curVal + (targetVal - curVal) * speed);
    }


    public static Vector3 Ease(Vector3 curVal, Vector3 targetVal, float speed)
    {
        return (Vector3)(curVal + (targetVal - curVal) * speed);
    }

}

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 TargetPos;




    public int MapHeight
    {
        get
        {
            return GetComponent<TileGenerator>().WorldHeight;
        }
    }

    public int MapWidth
    {
        get
        {
            return GetComponent<TileGenerator>().WorldWidth;
        }
    }

    public int tileDimension;
    public float SmoothingSpeed;

    public float minZoom = 3.5f;
    public float maxZoom;

    public float panSpeed;
    public float panBorderThickness;
    public float scrollSpeed;

    public bool isTopDown { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        TargetPos = transform.position;
        maxZoom = (float)MapWidth * 1.5f;
        panSpeed += (float)MapWidth * 0.15f;
        scrollSpeed += (float)MapWidth * 0.25f;
        isTopDown = false;

        float centerX = (float)MapWidth / 2.0f;
        float zoomY = 0;
        float centerZ = (float)MapHeight / 2.0f;
        Vector3 CenterPos = new Vector3(centerX, zoomY, centerZ);
        this.transform.position = CenterPos;
        CenterCamera();
    }


    // Update is called once per frame
    void Update()
    {
        System.Console.WriteLine($"Time deltatime = {Time.deltaTime}");

        //if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        if (Input.GetKey("w"))
        {
            TargetPos.z += panSpeed * Time.deltaTime;
        }

        //if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        if (Input.GetKey("s"))
        {
            TargetPos.z -= panSpeed * Time.deltaTime;
        }

        //if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        if (Input.GetKey("d"))
        {
            TargetPos.x += panSpeed * Time.deltaTime;
        }

        //if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        if (Input.GetKey("a"))
        {
            TargetPos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        TargetPos.y -= scroll * scrollSpeed * 10f * Time.deltaTime;

        //TargetPos.x = Mathf.Clamp(TargetPos.x, 0, MapHeight*tileDimension);
        //TargetPos.z = Mathf.Clamp(TargetPos.z, 0, MapWidth*tileDimension);
        //TargetPos.y = Mathf.Clamp(TargetPos.y, minZoom, maxZoom);

        transform.position = GlobalMethods.Ease(transform.position, this.TargetPos, SmoothingSpeed);
    }

    public void SetCameraTopdown()
    {
        this.transform.rotation = Quaternion.Euler(90f, this.transform.rotation.y, this.transform.rotation.z);
        this.isTopDown = true;
        CenterCamera();
    }

    public void SetCameraPerspective()
    {
        this.transform.rotation = Quaternion.Euler(50f, this.transform.rotation.y, this.transform.rotation.z);
        this.isTopDown = false;
        CenterCamera();
    }

    public void CenterCamera()
    {
        float centerX = MapWidth / 2;
        float centerY = (float)MapWidth * 0.8f;
        float centerZ = MapHeight - (1.75f * centerY);
        if (isTopDown)
        {
            centerX = MapWidth / 2;
            centerY = MapHeight * 1.25f;
            centerZ = MapHeight / 2;
        }

        this.TargetPos = new Vector3(centerX, centerY, centerZ);
    }


}
