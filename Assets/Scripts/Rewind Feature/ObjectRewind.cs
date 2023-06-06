using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to each game object that is rewinded.
/// Controls the rewind for one game object
/// </summary>
public class ObjectRewind : MonoBehaviour
{
    private bool isRewinding;

    // A list of data points storing the object's position variables.
    private List<RewindVariables> dataPoints;
    private Rigidbody2D rb;

    private void Awake()
    {
        dataPoints = new List<RewindVariables>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RewindController.Instance.AddRewindObject(this);
    }

    private void OnDestroy()
    {
        RewindController.Instance.RemoveRewindObject(this);
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            RecordPositions();
        }
    }

    public void BeginRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void EndRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }

    /// <summary>
    /// Records the object's position variables in a list which is continuously updated.
    /// </summary>
    private void RecordPositions()
    {
        if (dataPoints.Count > Mathf.RoundToInt(RewindController.SecondsToRewind / Time.fixedDeltaTime))
        {
            dataPoints.RemoveAt(dataPoints.Count - 1);
        }

        RewindVariables point =
            new RewindVariables(transform.position, transform.rotation,
                gameObject.activeInHierarchy, rb.velocity, rb.angularVelocity);
        dataPoints.Insert(0, point);
    }

    /// <summary>
    /// Reverses through the list of points and sets the object's transform to its previous position.
    /// </summary>
    private void Rewind()
    {
        if (dataPoints.Count > 0)
        {
            RewindVariables point = dataPoints[0];
            transform.position = point.Position;
            transform.rotation = point.Rotation;
            rb.velocity = point.Velocity;
            rb.angularVelocity = point.AngularVelocity;

            Asteroid asteroid = GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.SetVisible(point.IsActive);
            }
            dataPoints.RemoveAt(0);
        }
        else
        {
            EndRewind();
        }
    }
}