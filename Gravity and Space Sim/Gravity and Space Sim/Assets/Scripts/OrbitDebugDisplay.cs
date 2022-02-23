using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugDisplay : MonoBehaviour
{

    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;

    public bool relativeToBody;
    public PlanetGravity centralBody;
    public float width = 100;
    public bool useThickLines;

    private PlanetGravity planetGravity;
    private Vector3 currentSpeed;
    private bool drawn;

    void Awake()
    {
        planetGravity = GetComponent<PlanetGravity>();
        currentSpeed = planetGravity.initialSpeed;

    }

    void Update()
    {

        if (!Application.isPlaying)
        {
            DrawOrbits();
        }

        if (Application.isPlaying)
        {
            if(planetGravity.initialSpeed != currentSpeed)
            {
                currentSpeed = planetGravity.initialSpeed;
                DrawOrbits();
            }
        }

    }

    void DrawOrbits()
    {
        PlanetGravity[] bodies = FindObjectsOfType<PlanetGravity>();
        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == centralBody && relativeToBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            Color pathColour = Color.white;

            if (useThickLines)
            {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = width;
            }
            else
            {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                }

                // Hide renderer
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                if (lineRenderer)
                {
                    lineRenderer.enabled = false;
                }
            }

        }

        drawn = true;
    }

    Vector3 CalculateAcceleration(int i, VirtualBody[] virtualBodies)
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
            float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
            acceleration += forceDir * Universe.gravitationalConstant * virtualBodies[j].mass / sqrDst;
        }
        return acceleration;
    }

    void HideOrbits()
    {
        PlanetGravity[] bodies = FindObjectsOfType<PlanetGravity>();

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++)
        {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
    }

    void OnValidate()
    {
        if (usePhysicsTimeStep)
        {
            timeStep = Universe.physicsTimeStep;
        }
        

    }

    class VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody(PlanetGravity body)
        {
            position = body.transform.position;
            velocity = body.initialSpeed;
            mass = body.mass;
        }
    }
}