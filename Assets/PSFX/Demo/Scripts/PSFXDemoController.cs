using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PSFXDemoController : MonoBehaviour
{
    public float MovementSpeed = 3f;
    public float MouseSensitivity = 1.61f;
     
    float yaw = 0;

    Transform cameraTransform;
    Transform windowGUI;
    PSFXCamera psfx;
    GUIStyle cornerTextStyle;

    Slider resolutionScaleSlider;
    Slider cameraPositionPrecisionSlider;
    Slider fadeSlider;
    Dropdown fadeMethod;
    Slider vertexPrecisionSlider;
    Slider affineDistortionSlider;
    InputField triangleCullingDistance;
    Toggle triangleNearClipping;
    InputField colorDepth;
    Slider ditheringAmountSlider;

    void Start()
    {
        cameraTransform = transform.GetChild(0);
        psfx = cameraTransform.GetComponent<PSFXCamera>();

        cornerTextStyle = new GUIStyle() {
            fontSize = 18,
            fontStyle = FontStyle.Normal
        };

        // Capture mouse
        AcquireMouse();

        // Get objects
        windowGUI = GameObject.Find("GUI").transform.Find("Window").transform;

        resolutionScaleSlider = windowGUI.Find("Slider_ResolutionScale").GetComponent<Slider>();
        cameraPositionPrecisionSlider = windowGUI.Find("Slider_CameraPrecision").GetComponent<Slider>();
        fadeSlider = windowGUI.Find("Slider_Fade").GetComponent<Slider>();
        fadeMethod = windowGUI.Find("Dropdown_Fade").GetComponent<Dropdown>();
        vertexPrecisionSlider = windowGUI.Find("Slider_VertexPrecision").GetComponent<Slider>();
        affineDistortionSlider = windowGUI.Find("Slider_AffineDistortion").GetComponent<Slider>();
        triangleCullingDistance = windowGUI.Find("InputField_TriangleCullingDistance").GetComponent<InputField>();
        triangleNearClipping = windowGUI.Find("Toggle_TriangleNearClipping").GetComponent<Toggle>();
        colorDepth = windowGUI.Find("InputField_ColorDepth").GetComponent<InputField>();
        ditheringAmountSlider = windowGUI.Find("Slider_Dithering").GetComponent<Slider>();

        // Load values into GUI elements
        resolutionScaleSlider.value = psfx.Resolution;
        cameraPositionPrecisionSlider.value = psfx.CameraPositionPrecision;
        fadeSlider.value = psfx.Fade;
        fadeMethod.value = (int)psfx.FadeMethod;
        vertexPrecisionSlider.value = psfx.VertexPrecision;
        affineDistortionSlider.value = psfx.AffineStrength;
        triangleCullingDistance.text = psfx.TriangleCullDistance.ToString();
        triangleNearClipping.isOn = psfx.TriangleNearClipping;
        colorDepth.text = psfx.ColorDepth.ToString();
        ditheringAmountSlider.value = psfx.DitheringStrength;

        // Add listeners
        resolutionScaleSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(UpdatePSFXCamera));
        cameraPositionPrecisionSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(UpdatePSFXCamera));
        fadeSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(UpdatePSFXCamera));
        fadeMethod.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(UpdatePSFXCamera));
        vertexPrecisionSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(UpdatePSFXCamera));
        affineDistortionSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(UpdatePSFXCamera));
        triangleCullingDistance.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<string>(UpdatePSFXCamera));
        triangleNearClipping.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>(UpdatePSFXCamera));
        colorDepth.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<string>(UpdatePSFXCamera));
        ditheringAmountSlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(UpdatePSFXCamera));

        Canvas.ForceUpdateCanvases();
    }

    #region "Input"
    void Update()
    {
        if(Cursor.lockState == CursorLockMode.None)
            return;

        float horizontalInput = Input.GetAxis("Mouse X") * MouseSensitivity;
        float verticalInput = Input.GetAxis("Mouse Y") * MouseSensitivity;

        yaw -= verticalInput * MouseSensitivity;
        yaw = Mathf.Clamp(yaw, -89.9f, 89.9f);

        transform.localEulerAngles += Vector3.up * horizontalInput * MouseSensitivity;
        transform.GetChild(0).localEulerAngles = Vector3.right * yaw;
    }

    void FixedUpdate()
    {
        // Menu behavior
        if(Cursor.lockState == CursorLockMode.None)
        {
            if(Input.GetMouseButtonDown(0) && !windowGUI.gameObject.activeSelf)
            {
                AcquireMouse();
            }
            else
            {
                return;
            }
        }
        else
        {
            if(Input.GetKey(KeyCode.Escape) && !windowGUI.gameObject.activeSelf)
            {
                windowGUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Camera.main.pixelWidth * 0.25f, 0);
                windowGUI.gameObject.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // Movement
        float forwardSpeed = 0;
        float strafeSpeed = 0;
        float upSpeed = 0;

        if(Input.GetKey(KeyCode.W))
            forwardSpeed = 1;
        else if(Input.GetKey(KeyCode.S))
            forwardSpeed = -1;

        if(Input.GetKey(KeyCode.A))
            strafeSpeed = -1;
        else if(Input.GetKey(KeyCode.D))
            strafeSpeed = 1;

        if(Input.GetKey(KeyCode.E))
            upSpeed = 1;
        else if(Input.GetKey(KeyCode.Q))
            upSpeed = -1;

        Vector3 movementDirection = new Vector3(strafeSpeed, 0, forwardSpeed);
        movementDirection.Normalize();
        movementDirection *= MovementSpeed;
        upSpeed *= MovementSpeed;

        transform.position += (cameraTransform.forward * movementDirection.z + cameraTransform.right * movementDirection.x) * Time.fixedDeltaTime;
        transform.position += Vector3.up * upSpeed * Time.fixedDeltaTime;
    }
    #endregion

    void AcquireMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region "UI Behavior"
    void DrawText(Rect area, string text, GUIStyle style)
    {
        area.x += 2;
        area.y += 2;
        style.normal.textColor = Color.black;
        GUI.Label(area, text, style);

        area.x -= 2;
        area.y -= 2;
        style.normal.textColor = Color.white;
        GUI.Label(area, text, style);
    }

    private void OnGUI()
    {
        DrawText(new Rect(4, 4, 300, 300), "Press ESC to open up the Demo Menu as well as release the cursor.\nMovement: W, A, S, D, Q, E", cornerTextStyle);
    }

    public void CloseMenuWindow()
    {
        windowGUI.gameObject.SetActive(false);

        AcquireMouse();
    }

    public void UpdatePSFXCamera<T>(T obj) // Passing a type just so Unity will accept our listeners
    {
        psfx.Resolution = (int)resolutionScaleSlider.value;
        psfx.CameraPositionPrecision = (int)cameraPositionPrecisionSlider.value;
        psfx.Fade = fadeSlider.value;
        psfx.FadeMethod = (PSFXFadeMethod)fadeMethod.value;
        psfx.VertexPrecision = (int)vertexPrecisionSlider.value;
        psfx.AffineStrength = affineDistortionSlider.value;

        float cullDistance = psfx.TriangleCullDistance;
        float.TryParse(triangleCullingDistance.text, out cullDistance);
        psfx.TriangleCullDistance = cullDistance;

        psfx.TriangleNearClipping = triangleNearClipping.isOn;

        int colorDepth = psfx.ColorDepth;
        int.TryParse(this.colorDepth.text, out colorDepth);
        psfx.ColorDepth = colorDepth;

        psfx.DitheringStrength = ditheringAmountSlider.value;
    }
    #endregion
}