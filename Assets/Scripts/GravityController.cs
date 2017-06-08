using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private readonly Vector3 _startGravityVector = new Vector3(0, 0, 0);
    private Vector3 _endGravityVector;
    private Vector3 _startMouseVector;
    private Vector3 _endMouseVector;

    private float _startTime;
    private readonly float animationLength = 1.0F;
    
    private bool _animating = false;

    private readonly Dictionary<Vector3,Vector3> _cameraToGravity = new Dictionary<Vector3, Vector3>()
    {
        { new Vector3(0, 0, 0), new Vector3(0, -9.8F, 0)},
        { new Vector3(0, 0, 90), new Vector3(9.8F, 0, 0)},
        { new Vector3(0, 0, 180), new Vector3(0, 9.8F, 0)},
        { new Vector3(0, 0, 270), new Vector3(-9.8F, 0, 0)},
        { new Vector3(90, 0, 0), new Vector3(0, 0, -9.8f)},
        { new Vector3(270, 0, 0), new Vector3(0, 0, 9.8f)},
    };
   

    // Use this for initialization
    private void Start ()
    {
    }
	
	// Update is called once per frame
    private void Update ()
    {
        if (_animating)
        {
            Animate();
        }
	    else
	    {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                SetStartEndValues(new Vector3(0, 0, 180));
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                SetStartEndValues(new Vector3(0, 0, -180));
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                SetStartEndValues(new Vector3(0, 0, -90));
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                SetStartEndValues(new Vector3(0, 0, 90));
            else if (Input.GetKeyDown(KeyCode.PageUp))
                SetStartEndValues(new Vector3(90, 0, 0));
            else if (Input.GetKeyDown(KeyCode.PageDown))
                SetStartEndValues(new Vector3(-90, 0, 0));
	    }
    }

    private void SetStartEndValues(Vector3 addMouseVector)
    {
        _startTime = Time.time;
        
        _startMouseVector = GetComponent<MouseLook>().originalRotation.eulerAngles;
        _endMouseVector = _startMouseVector + addMouseVector;

        foreach (var pair in _cameraToGravity)
            if (CompareVectors(pair.Key, _endMouseVector, 2F))
                _endGravityVector = pair.Value;
                
        _animating = true;
    }

    private void Animate()
    {
        var timePassed = Time.time - _startTime;
        var fracTimePassed = timePassed / animationLength;

        Physics.gravity = Vector3.Lerp(_startGravityVector, _endGravityVector, fracTimePassed);
        GetComponent<MouseLook>().originalRotation = Quaternion.Euler(Vector3.Lerp(_startMouseVector, _endMouseVector, fracTimePassed));
        
        if (fracTimePassed > 0.999F)
        {
            Physics.gravity = _endGravityVector;
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(_endMouseVector);
            _animating = false;
        }
    }

    private static bool CompareVectors(Vector3 a, Vector3 b, float epsilon)
    {
        if (NearlyEqual(a.x, b.x, epsilon) && NearlyEqual(a.y, b.y, epsilon) && NearlyEqual(a.z, b.z, epsilon))
            return true;
        return false;
    }

    private static bool NearlyEqual(float a, float b, float epsilon)
    {
        var diff = Mathf.Abs((a+360) % 360 - (b+360) % 360);

        if (diff < epsilon)
            return true;
        return false;
    }
}
