using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraWithGyro : MonoBehaviour
{
    private bool isBoseConnected;
    private Bose.Wearable.WearableControl _instance;
    private float initialOrientationY;

    private void OnEnable()
    {
        Bose.Wearable.WearableControl.Instance.ConnectionStatusChanged += OnBoseConnectStatusChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        Bose.Wearable.WearableControl.Instance.ConnectionStatusChanged -= OnBoseConnectStatusChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoseConnected)
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y + _instance.LastSensorFrame.angularVelocity.value.y * Time.deltaTime * Mathf.Rad2Deg, 0.0f);
    }

    public void OnBoseConnectStatusChange(Bose.Wearable.ConnectionStatus status, Bose.Wearable.Device? device)
    {
        isBoseConnected = (status == Bose.Wearable.ConnectionStatus.Connected);
        _instance = Bose.Wearable.WearableControl.Instance;
        initialOrientationY = _instance.LastSensorFrame.angularVelocity.value.y;
    }

}
