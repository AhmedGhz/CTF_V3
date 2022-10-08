using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

namespace RopeMinikit
{
    public class Rope : MonoBehaviour
    {
        protected const int MaxCollisionPlanesPerParticle = 3;
        protected const int InitialParticleTargets = 3;
        protected const int MaxRigidbodyConnections = 24;

        public struct Measurements
        {
            public float spawnCurveLength;
            public float realCurveLength;
            public int segmentCount;
            public int particleCount;
            public float particleSpacing;

            public int GetParticleIndexAt(float distance)
            {
                return math.clamp((int)(distance / particleSpacing + 0.5f), 0, particleCount - 1);
            }
        }

        public struct OnSplitParams
        {
            public int minParticleIndex;
            public int maxParticleIndex;
            public Measurements preSplitMeasurements;
        }

        public struct EditorColors
        {
            public Color ropeSegments;
            public Color simulationParticle;
            public Color collisionParticle;
            public Color spawnPointHandle;
        }

        public static readonly EditorColors Colors = new EditorColors()
        {
            ropeSegments = Color.black,
            simulationParticle = new Color(0.2f, 0.8f, 0.2f, 0.5f),
            collisionParticle = new Color(1.0f, 0.92f, 0.016f, 0.5f),
            spawnPointHandle = new Color(0.1f, 0.5f, 0.8f),
        };

        [Tooltip("The radius of the rope. This value is used both for constructing the visual mesh and handling collisions.")]
        [Range(0.01f, 1.0f)]
        public float radius = 0.05f;

        [Tooltip("The number of vertices to use for each segment of the rope's visual mesh. More vertices results in a rounder looking rope but increases the overall vertex and triangle count of the visual mesh. This value does not influence the simulation of the rope at all.")]
        [DisableInPlayMode]
        [Range(3, 32)]
        public int radialVertices = 6;

        [Tooltip("Whether or not the rope is a circular loop. If enabled, the last spawn point of the rope will be connected to the first spawn point.")]
        [DisableInPlayMode]
        public bool isLoop = false;

        [Tooltip("The material used to render the rope. This can be any material that uses vertex positions and optionally normals.")]
        public Material material;

        [Tooltip("If specified, this mesh is rendered instead of the default rope cylinder at each simulation particle. The z-axis of the mesh will align with the rope tangent and the mesh will be scaled so that z=0 is the current simulation particle and z=1 is the next simulation particle. The material specified for the rope must support instancing.")]
        public Mesh customMesh;

        [Tooltip("When using a custom mesh, this property specifies how much to rotate the mesh around the z-axis for every link in the chain of simulation particles.")]
        [Range(0.0f, 360.0f)]
        public float customMeshRotation = 90.0f;
        
        [Tooltip("The spawn points used to initially place the rope in the world. Currently, pairs of consequtive spawn points are considered linear line segments.")]
        [DisableInPlayMode]
        public List<float3> spawnPoints = new List<float3>();

        [System.Serializable]
        public struct SimulationSettings
        {
            [Tooltip("Turns on or off the simulation independently of the rendering of the rope. A use case could be to programmatically disable ropes that are too far away from the camera or ropes that are not visible.")]
            public bool enabled;

            [Tooltip("The number of simulation particles per meter. A higher resolution results in a smoother looking rope but requires more compute.")]
            [DisableInPlayMode]
            public float resolution;

            [Tooltip("The number of solver iterations to run for this rope. High resolution ropes need more iterations to become stiff. More iterations requires more compute.")]
            [Range(1, 32)]
            public int solverIterations;

            [Tooltip("The mass per meter of the rope. This value is used when interacting with rigidbodies via RopeRigidbodyConnection components.")]
            [Delayed]
            public float massPerMeter;

            [Tooltip("A measure of the stiffness of the rope. Note that the actual stiffness is heavily dependent on the number of solver iterations and the size of the physics time step used, if you change one value you problably need to re-tweak the other(s). This particular value does not influence performance.")]
            [Range(0.01f, 1.0f)]
            public float stiffness;

            [Tooltip("A value that dynamically shortens or lengthens the rope by a multiplicative factor. This can be used to create a retractable grappling hook for example.")]
            [Range(0.0f, 2.0f)]
            public float lengthMultiplier;

            [Tooltip("The percentage of energy to remove from the simulation each fixed update. Useful to model air resistance. Does not influence performance.")]
            [Range(0.0f, 1.0f)]
            public float energyLoss;

            [Tooltip("The percentage of the gravity force to apply to the rope. A low gravity multiplier might be useful to straighten out ropes that otherwise sack but should be considered a 'hack' as the rope will behave as if it is in space.")]
            [Range(0.0f, 1.0f)]
            public float gravityMultiplier;
        }

        [Space]
        public SimulationSettings simulation = new SimulationSettings()
        {
            enabled = true,
            resolution = 10.0f,
            solverIterations = 4,
            massPerMeter = 0.2f,
            stiffness = 1.0f,
            lengthMultiplier = 1.0f,
            energyLoss = 0.01f,
            gravityMultiplier = 1.0f,
        };

        [System.Serializable]
        public struct CollisionSettings
        {
            [Tooltip("Enables collision handling for the rope so that it reacts to colliders other than the ones it is connected to via RopePins or RopeRigidbodyConnection components. Performance intensive on the main thread.")]
            public bool enabled;

            [Tooltip(
                "Check and respond to collisions on every n:th simulation particle. A value of one will make every simulated particle react to collisions, a value " +
                "of two will make every other particle react to collisions and so on. As one sphere-overlap test is performed per particle, a low value is very " +
                "performance intensive. Collision particles are visualized by yellow spheres when the rope is selected.")]
            [Range(1, 20)]
            public int stride;

            [Tooltip("The dynamic friction coefficient of the rope. Used to slow the rope down if it is dragged along the ground for example.")]
            [Range(0.0f, 20.0f)]
            public float friction;

            [Tooltip("An extra distance (added ontop of the rope radius) that prevents small radius ropes from falling through geometry easily")]
            [Range(0.0f, 1.0f)]
            public float collisionMargin;

            public LayerMask ignoreLayers;
        }

        [Space]
        public CollisionSettings collisions = new CollisionSettings()
        {
            enabled = false,
            stride = 2,
            friction = 0.1f,
            collisionMargin = 0.025f,
            ignoreLayers = 0,
        };

        protected struct CollisionPlane
        {
            public float3 point;
            public float3 normal;
            public float3 velocityChange;
        }

        protected struct ParticleTarget
        {
            public int particleIndex;
            public float3 position;
            public float stiffness;
        }

        protected struct RigidbodyConnection
        {
            public Rigidbody rigidbody;
            public float rigidbodyDamping;
            public ParticleTarget target;
        }

        protected bool initialized;
        protected bool computingSimulationFrame;
        protected bool firstParticleExplicitlyMovedFrame;
        protected bool simulationDisabledPrevFrame;
        protected bool wasSplit;
        protected JobHandle simulationFrameHandle;

        // State
        protected NativeArray<float3> positions;
        protected NativeArray<float3> prevPositions;
        protected NativeArray<float3> bitangents;
        protected NativeArray<float> massMultipliers;

        // Collision handling
        protected NativeArray<int> activeCollisionPlanes;
        protected NativeArray<CollisionPlane> collisionPlanes;

        // Rigidbody connections
        protected List<RigidbodyConnection> queuedRigidbodyConnections;
        protected List<RigidbodyConnection> liveRigidbodyConnections;
        protected NativeArray<ParticleTarget> particleTargets;
        protected NativeArray<float3> particleTargetFeedbacks;

        // Rendering
        protected NativeArray<Vector3> vertices;
        protected NativeArray<Vector3> normals;
        protected NativeArray<float3> cosLookup;
        protected NativeArray<float3> sinLookup;

        protected Vector3[] vertices2; // only needed until Mesh provides NativeArray methods
        protected Vector3[] normals2; // only needed until Mesh provides NativeArray methods

        protected Mesh mesh;

        protected Measurements _measurements;

        /// <summary>
        /// Returns the measurements of the rope. The measurements remain constant after the rope is first initialized.
        /// </summary>
        public Measurements measurements
        {
            get
            {
                if (!Initialize())
                {
                    return new Measurements();
                }
                return _measurements;
            }
        }

        /// <summary>
        /// The current world-space bounds of the visual mesh
        /// </summary>
        public Bounds currentBounds
        {
            get
            {
                if (!Initialize())
                {
                    return new Bounds();
                }
                return mesh.bounds;
            }
        }

        public void OnValidate()
        {
            simulation.resolution = Mathf.Max(0.01f, simulation.resolution);
            simulation.massPerMeter = Mathf.Max(0.01f, simulation.massPerMeter);
        }

        /// <summary>
        /// Adds a new spawn point to the rope. May be called from edit-mode.
        /// </summary>
        public void PushSpawnPoint()
        {
            if (spawnPoints.Count == 0)
            {
                spawnPoints.Add(Vector3.right);
                return;
            }
            var prev = spawnPoints.Count >= 2 ? spawnPoints[spawnPoints.Count - 2] : float3.zero;
            var current = spawnPoints[spawnPoints.Count - 1];
            spawnPoints.Add(current + math.normalizesafe(current - prev));
        }

        /// <summary>
        /// Removes the last spawn point of the rope. May be called from edit-mode.
        /// </summary>
        public void PopSpawnPoint()
        {
            if (spawnPoints.Count <= 2)
            {
                return;
            }
            spawnPoints.RemoveAt(spawnPoints.Count - 1);
        }

        /// <summary>
        /// Returns the index of the simulation particle at a particular distance along the curve of the rope
        /// </summary>
        /// <param name="distance">The distance along the curve of the rope</param>
        /// <returns>The particle index</returns>
        public int GetParticleIndexAt(float distance)
        {
            if (!Initialize() || _measurements.particleSpacing == 0.0f)
            {
                return 0;
            }
            return _measurements.GetParticleIndexAt(distance);
        }

        /// <summary>
        /// Returns the scalar distance along the curve of the rope that a particular simulation particle is located at. The scalar distance is
        /// a value between 0 and 1. The lengthMultiplier is not taken into account. To get the distance along the rope in world space, multiply
        /// the scalar distance by the realCurveLength measurement.
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <returns>The scalar distance</returns>
        public float GetScalarDistanceAt(int particleIndex)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return 0.0f;
            }
            return math.clamp((float)particleIndex / (measurements.particleCount - 1), 0.0f, 1.0f);
        }

        /// <summary>
        /// Returns the current position of a particular simulation particle
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <returns>The current position in world-space</returns>
        public float3 GetPositionAt(int particleIndex)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return float3.zero;
            }
            CompletePreviousSimulationFrame();
            return positions[particleIndex];
        }

        /// <summary>
        /// Sets the position of a particular simulation particle
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <param name="position">The desired position in world-space</param>
        public void SetPositionAt(int particleIndex, float3 position)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return;
            }
            CompletePreviousSimulationFrame();
            positions[particleIndex] = position;
            if (particleIndex == 0)
            {
                firstParticleExplicitlyMovedFrame = true;
            }
        }

        /// <summary>
        /// Returns the current velocity of a particular simulation particle
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <returns>The velocity in world-space</returns>
        public float3 GetVelocityAt(int particleIndex)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return float3.zero;
            }
            CompletePreviousSimulationFrame();
            return (positions[particleIndex] - prevPositions[particleIndex]) / Time.fixedDeltaTime;
        }

        /// <summary>
        /// Sets the velocity of a particular simulation particle
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <param name="velocity">The desired velocity in world-space</param>
        public void SetVelocityAt(int particleIndex, float3 velocity)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return;
            }
            CompletePreviousSimulationFrame();
            prevPositions[particleIndex] = positions[particleIndex] - velocity * Time.fixedDeltaTime;
        }

        /// <summary>
        /// Returns the mass multiplier of a particular simulation particle. This value can be used to increase or decrease
        /// the weight of a section of the rope. A value of 0 will make the particle immovable. A value of 2 will make the
        /// particle twice as heavy as its neighbors. The default value is 1.
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <returns>The mass multiplier</returns>
        public float GetMassMultiplierAt(int particleIndex)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return 0.0f;
            }
            CompletePreviousSimulationFrame();
            return massMultipliers[particleIndex];
        }

        /// <summary>
        /// Sets the mass multiplier of a particular simulation particle. This value can be used to increase or decrease
        /// the weight of a section of the rope. A value of 0 will make the particle immovable. A value of 2 will make the
        /// particle twice as heavy as its neighbors. The default value is 1.
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle</param>
        /// <param name="value">The desired mass multiplier</param>
        public void SetMassMultiplierAt(int particleIndex, float value)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length)
            {
                return;
            }
            CompletePreviousSimulationFrame();
            massMultipliers[particleIndex] = value;
        }

        /// <summary>
        /// Finds the simulation particle closest to a particular point
        /// </summary>
        /// <param name="point">The point in world-space</param>
        /// <param name="particleIndex">The index of the closest simulation particle</param>
        /// <param name="distance">The distance along the rope of the closest simulation particle in world-space</param>
        public void GetClosestParticle(float3 point, out int particleIndex, out float distance)
        {
            if (!Initialize())
            {
                particleIndex = -1;
                distance = 0.0f;
                return;
            }
            CompletePreviousSimulationFrame();
            positions.GetClosestPoint(point, out particleIndex, out distance);
        }

        /// <summary>
        /// Finds the simulation particle closest to a particular ray
        /// </summary>
        /// <param name="ray">The ray in world-space</param>
        /// <param name="particleIndex">The index of the closest simulation particle</param>
        /// <param name="distance">The distance along the rope of the closest simulation particle in world-space</param>
        /// <param name="distanceAlongRay">The distance along the ray to the point on the ray that is closest to the simulation particle</param>
        public void GetClosestParticle(Ray ray, out int particleIndex, out float distance, out float distanceAlongRay)
        {
            if (!Initialize())
            {
                particleIndex = -1;
                distance = 0.0f;
                distanceAlongRay = 0.0f;
                return;
            }
            CompletePreviousSimulationFrame();
            positions.GetClosestPoint(ray, out particleIndex, out distance, out distanceAlongRay);
        }

        /// <summary>
        /// Registers a rigidbody connection for the next simulation frame. A rigidbody connection is a two-way coupling of a simulation particle
        /// to a traditional rigidbody. Make sure to call this method from FixedUpdate(). Any simulation particle involved in a rigidbody connection
        /// will get its mass multiplier reset to 1 at the end of the simulation frame.
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle to connect</param>
        /// <param name="rigidbody">The rigidbody to connect</param>
        /// <param name="rigidbodyDamping">The amount of damping to apply to the rigidbody in the range [0, 1]</param>
        /// <param name="pointOnBody">The world-space point on the rigidbody to connect</param>
        /// <param name="stiffness">The stiffness of the connection in the range [0, 1]</param>
        public void RegisterRigidbodyConnection(int particleIndex, Rigidbody rigidbody, float rigidbodyDamping, float3 pointOnBody, float stiffness)
        {
            if (!Initialize() || particleIndex < 0 || particleIndex >= positions.Length || !enabled || !simulation.enabled)
            {
                return;
            }
            queuedRigidbodyConnections.Add(new RigidbodyConnection()
            {
                rigidbody = rigidbody,
                rigidbodyDamping = rigidbodyDamping,
                target = new ParticleTarget()
                {
                    particleIndex = particleIndex,
                    position = pointOnBody,
                    stiffness = stiffness,
                },
            });
        }

        /// <summary>
        /// Resets the rope to its original shape relative to the current transform. Useful when activating a pooled game object that is 
        /// deactivated and re-activated instead of destroyed and instantiated.
        /// </summary>
        public void ResetToSpawnCurve()
        {
            if (!Initialize())
            {
                return;
            }

            CompletePreviousSimulationFrame();

            var localToWorld = (float4x4)transform.localToWorldMatrix;

            spawnPoints.GetPointsAlongCurve(ref localToWorld, _measurements.particleSpacing, positions);
            positions.CopyTo(prevPositions);

            transform.position = positions[0];
        }

        /// <summary>
        /// Computes the current length of the rope. In contrast to the measurements.realCurveLength field, this value includes the stretching
        /// of the rope due to stress.
        /// </summary>
        public float GetCurrentLength()
        {
            if (!Initialize())
            {
                return 0.0f;
            }

            CompletePreviousSimulationFrame();

            return positions.GetLengthOfCurve(isLoop);
        }

        protected Rope InstantiateSplitRope(int minIdx, int maxIdx, string identifier)
        {
            var count = maxIdx - minIdx + 1;
            if (minIdx < 0 || maxIdx > positions.Length - 1 || count < 2)
            {
                return null;
            }

            // Create two spawn points that are roughly placed where the new rope will be (this will create nice bitangents)
            var targetLength = _measurements.realCurveLength * ((float)count / _measurements.particleCount);
            var point0 = positions[minIdx];
            var point1 = positions[maxIdx];
            var delta = point1 - point0;
            var simplifiedLength = math.length(delta);
            point1 += math.normalizesafe(delta) * (targetLength - simplifiedLength);

            var rope = Instantiate(gameObject, Vector3.zero, Quaternion.identity).GetComponent<Rope>();
            rope.name = identifier;
            rope.isLoop = false;
            rope.spawnPoints = new List<float3>()
            {
                point0,
                point1,
            };
            rope.ResetToSpawnCurve();

            if (rope.initialized)
            {
                // Now update the simulation particles to exactly match those of the original rope
                for (int i = 0; i < rope.positions.Length; i++)
                {
                    var sourceIdx = minIdx + i;
                    if (sourceIdx >= positions.Length)
                    {
                        break;
                    }
                    rope.positions[i] = positions[sourceIdx];
                    rope.prevPositions[i] = prevPositions[sourceIdx];
                }

                var param = new OnSplitParams()
                {
                    minParticleIndex = minIdx,
                    maxParticleIndex = maxIdx,
                    preSplitMeasurements = _measurements,
                };

                rope.SendMessage("OnRopeSplit", param, SendMessageOptions.DontRequireReceiver);
            }

            return rope;
        }

        /// <summary>
        /// Splits the rope at a specific simulation particle and returns the rope components of the newly instantiated game objects. Make sure
        /// that the supplied array has exactly 2 slots. A Unity message 'OnRopeSplit(Rope.OnSplitParams)' will be sent to each newly created rope.
        /// </summary>
        /// <param name="particleIndex">The index of the simulation particle at which point to split</param>
        /// <param name="outNewRopes">If not null, an array with exactly 2 elements where the new rope game objects will be returned</param>
        public void SplitAt(int particleIndex, Rope[] outNewRopes = null)
        {
            if (!Initialize() || (outNewRopes != null && outNewRopes.Length != 2) || wasSplit)
            {
                return;
            }
            wasSplit = true;

            var fst = InstantiateSplitRope(0, particleIndex, name + "_split0");
            var snd = InstantiateSplitRope(particleIndex + 1, positions.Length - 1, name + "_split1");

            Destroy(gameObject);

            if (outNewRopes != null)
            {
                outNewRopes[0] = fst;
                outNewRopes[1] = snd;
            }
        }

        protected void ComputeRealCurve(Allocator allocator, out Measurements measurements, out NativeArray<float3> points)
        {
            var localToWorld = (float4x4)transform.localToWorldMatrix;

            var spawnCurveLength = spawnPoints.GetLengthOfCurve(ref localToWorld);
            var segmentCount = math.max(1, (int)(spawnCurveLength * simulation.resolution));
            var particleCount = segmentCount + 1;
            var particleSpacing = spawnCurveLength / segmentCount;

            points = new NativeArray<float3>(particleCount, allocator);
            spawnPoints.GetPointsAlongCurve(ref localToWorld, particleSpacing, points);
            var realCurveLength = points.GetLengthOfCurve(ref localToWorld);

            measurements = new Measurements()
            {
                spawnCurveLength = spawnCurveLength,
                realCurveLength = realCurveLength,
                segmentCount = segmentCount,
                particleCount = particleCount,
                particleSpacing = particleSpacing,
            };
        }

        public void OnEnable()
        {
            if (!initialized)
            {
                return;
            }

            CompletePreviousSimulationFrame();
        }

        public void Start()
        {
            Initialize();
        }

        public void OnDisable()
        {
            if (!initialized)
            {
                return;
            }

            CompletePreviousSimulationFrame();

            simulationDisabledPrevFrame = true;
        }

        public void OnDestroy()
        {
            if (!initialized)
            {
                return;
            }

            CompletePreviousSimulationFrame();

            // State
            positions.Dispose();
            prevPositions.Dispose();
            bitangents.Dispose();
            massMultipliers.Dispose();

            // Collision handling
            activeCollisionPlanes.Dispose();
            collisionPlanes.Dispose();

            // Rigidbody connections
            particleTargets.Dispose();
            particleTargetFeedbacks.Dispose();

            // Rendering
            vertices.Dispose();
            normals.Dispose();
            cosLookup.Dispose();
            sinLookup.Dispose();

            Destroy(mesh);
        }

        protected bool Initialize()
        {
            if (initialized)
            {
                return true;
            }
            if (!Application.isPlaying || spawnPoints.Count < 2)
            {
                // Not designed for edit-mode execution
                return false;
            }

            // State
            ComputeRealCurve(Allocator.Persistent, out _measurements, out positions);

            prevPositions = new NativeArray<float3>(_measurements.particleCount, Allocator.Persistent);
            positions.CopyTo(prevPositions);

            massMultipliers = new NativeArray<float>(_measurements.particleCount, Allocator.Persistent);
            for (int i = 0; i < massMultipliers.Length; i++)
            {
                massMultipliers[i] = 1.0f;
            }

            bitangents = new NativeArray<float3>(_measurements.particleCount, Allocator.Persistent);
            {
                var up = new float3(0.0f, 1.0f, 0.0f);

                for (int i = 0; i < bitangents.Length; i++)
                {
                    var tangent = positions[(i + 1) % bitangents.Length] - positions[i];

                    var bitangent = math.normalizesafe(math.cross(up, tangent));
                    if (math.all(bitangent == float3.zero))
                    {
                        bitangent = math.normalizesafe(math.cross(up + new float3(0.0f, 0.0f, -1.0f), tangent));
                    }
                    bitangents[i] = bitangent;

                    up = math.cross(tangent, bitangent);
                }

                if (!isLoop)
                {
                    bitangents[bitangents.Length - 1] = bitangents[bitangents.Length - 2];
                }
            }

            transform.position = positions[0];

            // Collision handling
            activeCollisionPlanes = new NativeArray<int>(_measurements.particleCount, Allocator.Persistent);
            collisionPlanes = new NativeArray<CollisionPlane>(_measurements.particleCount * MaxCollisionPlanesPerParticle, Allocator.Persistent);

            // Rigidbody connections
            queuedRigidbodyConnections = new List<RigidbodyConnection>();
            liveRigidbodyConnections = new List<RigidbodyConnection>();
            particleTargets = new NativeArray<ParticleTarget>(InitialParticleTargets, Allocator.Persistent);
            particleTargetFeedbacks = new NativeArray<float3>(InitialParticleTargets, Allocator.Persistent);

            // Rendering
            vertices = new NativeArray<Vector3>(_measurements.particleCount * radialVertices, Allocator.Persistent);
            normals = new NativeArray<Vector3>(vertices.Length, Allocator.Persistent);
            cosLookup = new NativeArray<float3>(radialVertices, Allocator.Persistent);
            sinLookup = new NativeArray<float3>(radialVertices, Allocator.Persistent);

            for (int i = 0; i < radialVertices; i++)
            {
                var angle = ((float)i / (radialVertices - 1)) * Mathf.PI * 2.0f;
                cosLookup[i] = Mathf.Cos(angle);
                sinLookup[i] = Mathf.Sin(angle);
            }

            vertices2 = new Vector3[vertices.Length];
            normals2 = new Vector3[vertices.Length];

            // Note that triangles and uvs are unchanged after mesh creation
            var triangleParticleMax = isLoop ? _measurements.particleCount : _measurements.particleCount - 1;
            var radialTriangleCount = triangleParticleMax * (radialVertices - 1) * 2 * 3;
            var capTriangleCount = isLoop ? 0 : 2 * (radialVertices - 3) * 3;
            var triangleCount = radialTriangleCount + capTriangleCount;
            var triangles = new int[triangleCount];

            int idx = 0;
            for (int i = 0; i < triangleParticleMax; i++)
            {
                int vertexOffset0 = i * radialVertices;
                int vertexOffset1 = ((i + 1) % _measurements.particleCount) * radialVertices;

                for (int j = 0; j < radialVertices - 1; j++)
                {
                    int v0 = vertexOffset0 + j + 0;
                    int v1 = vertexOffset0 + j + 1;
                    int v2 = vertexOffset1 + j + 0;
                    int v3 = vertexOffset1 + j + 1;

                    triangles[idx++] = v0;
                    triangles[idx++] = v1;
                    triangles[idx++] = v2;
                    triangles[idx++] = v2;
                    triangles[idx++] = v1;
                    triangles[idx++] = v3;
                }
            }
            if (!isLoop)
            {
                for (int i = 1; i < radialVertices - 2; i++)
                {
                    triangles[idx++] = 0;
                    triangles[idx++] = i + 1;
                    triangles[idx++] = i;
                }
                int vertexOffset = triangleParticleMax * radialVertices;
                for (int i = 1; i < radialVertices - 2; i++)
                {
                    triangles[idx++] = vertexOffset;
                    triangles[idx++] = vertexOffset + i;
                    triangles[idx++] = vertexOffset + i + 1;
                }
            }

            var uvs = new Vector2[vertices.Length];
            for (int i = 0; i < _measurements.particleCount; i++)
            {
                var uv = new Vector2
                {
                    x = ((float)i / (_measurements.particleCount - 1)) * _measurements.realCurveLength,
                };

                for (int j = 0; j < radialVertices; j++)
                {
                    uv.y = (float)j / (radialVertices - 1);
                    uvs[i * radialVertices + j] = uv;
                }
            }

            mesh = new Mesh
            {
                name = gameObject.name + "_rope"
            };
            mesh.MarkDynamic();
            mesh.vertices = vertices2;
            mesh.normals = normals2;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            initialized = true;
            computingSimulationFrame = false;
            return true;
        }

        protected Collider[] collisionQueryBuffer = new Collider[MaxCollisionPlanesPerParticle];

        public void UpdateCollisionPlanes()
        {
            if (!collisions.enabled)
            {
                return;
            }

            Profiler.BeginSample(nameof(UpdateCollisionPlanes));

            var deltaTime = Time.fixedDeltaTime;
            var layerMask = ~collisions.ignoreLayers;
            var safeRadius = radius + collisions.collisionMargin;
            var safeRadiusSq = safeRadius * safeRadius;
            var extendedRadius = safeRadius * 1.5f;
            var stride = collisions.stride;

            for (int i = 0; i < activeCollisionPlanes.Length; i++)
            {
                if (i % stride != 0)
                {
                    activeCollisionPlanes[i] = 0;
                    continue;
                }

                var planeCount = 0;

                // Use projected positions for the next frame
                var pos = positions[i];
                var prevPos = prevPositions[i];
                var vel = pos - prevPos;
                prevPos = pos;
                pos += vel;

                // Check for overlap
                var hitCount = Physics.OverlapSphereNonAlloc(pos, extendedRadius, collisionQueryBuffer, layerMask); // use a slightly larger sphere to catch more collisions
                for (int j = 0; j < hitCount && planeCount < MaxCollisionPlanesPerParticle; j++)
                {
                    var collider = collisionQueryBuffer[j];
                    var meshCollider = collider as MeshCollider;

                    if ((meshCollider != null && meshCollider.convex) || collider is BoxCollider || collider is SphereCollider || collider is CapsuleCollider)
                    {
                        var closestPoint = (float3)Physics.ClosestPoint(pos, collider, collider.transform.position, collider.transform.rotation);

                        var normal = math.normalizesafe(pos - closestPoint);
                        if (math.all(normal == float3.zero))
                        {
                            continue;
                        }

                        collisionPlanes[i * MaxCollisionPlanesPerParticle + planeCount++] = new CollisionPlane()
                        {
                            point = closestPoint,
                            normal = normal,
                            velocityChange = collider.attachedRigidbody != null
                                ? (float3)collider.attachedRigidbody.GetPointVelocity(closestPoint) * deltaTime
                                : float3.zero,
                        };
                    }
                }

                // Check fast movements
                if (planeCount < MaxCollisionPlanesPerParticle)
                {
                    var movementSq = math.lengthsq(vel);
                    if (movementSq > safeRadiusSq)
                    {
                        if (Physics.Linecast(prevPos, pos, out RaycastHit hit, layerMask))
                        {
                            collisionPlanes[i * MaxCollisionPlanesPerParticle + planeCount++] = new CollisionPlane()
                            {
                                point = hit.point,
                                normal = hit.normal,
                                velocityChange = hit.rigidbody != null
                                    ? (float3)hit.rigidbody.GetPointVelocity(hit.point) * deltaTime
                                    : float3.zero,
                            };
                        }
                    }
                }

                activeCollisionPlanes[i] = planeCount;
            }

            Profiler.EndSample();
        }

        protected void PrepareRigidbodyConnections()
        {
            Profiler.BeginSample(nameof(PrepareRigidbodyConnections));

            liveRigidbodyConnections.AddRange(queuedRigidbodyConnections);
            queuedRigidbodyConnections.Clear();

            if (liveRigidbodyConnections.Count > particleTargets.Length)
            {
                if (liveRigidbodyConnections.Count > MaxRigidbodyConnections)
                {
                    Debug.LogWarning(
                        $"Encountered too many live rigid body connections ({liveRigidbodyConnections.Count}) this frame. " +
                        $"Limiting enforcement to the max value ({MaxRigidbodyConnections}) to avoid a performance drop...");
                }
                else
                {
                    var doubleLength = liveRigidbodyConnections.Count * 2;

                    particleTargets.Dispose();
                    particleTargets = new NativeArray<ParticleTarget>(doubleLength, Allocator.Persistent);
                    particleTargetFeedbacks.Dispose();
                    particleTargetFeedbacks = new NativeArray<float3>(doubleLength, Allocator.Persistent);
                }
            }

            for (int i = 0; i < particleTargets.Length; i++)
            {
                if (i < liveRigidbodyConnections.Count)
                {
                    var c = liveRigidbodyConnections[i];

                    if (c.rigidbody == null)
                    {
                        c.target.stiffness = 0.0f;
                    }

                    particleTargets[i] = c.target;

                    // Make particle immovable if rigidbody is kinematic
                    if (c.rigidbody != null && c.rigidbody.isKinematic)
                    {
                        massMultipliers[c.target.particleIndex] = 0.0f;
                    }
                }
                else
                {
                    particleTargets[i] = new ParticleTarget()
                    {
                        particleIndex = -1,
                    };
                }
            }

            Profiler.EndSample();
        }

        protected void ApplyRigidbodyConnectionFeedback()
        {
            if (liveRigidbodyConnections.Count == 0)
            {
                return;
            }

            Profiler.BeginSample(nameof(ApplyRigidbodyConnectionFeedback));

            var particleMass = simulation.massPerMeter * _measurements.realCurveLength / _measurements.particleCount;
            var invDtAndSolverIterations = 1.0f / (Time.fixedDeltaTime * simulation.solverIterations);

            var iterationCount = math.min(liveRigidbodyConnections.Count, particleTargetFeedbacks.Length);
            for (int i = 0; i < iterationCount; i++)
            {
                var c = liveRigidbodyConnections[i];

                // Apply impulse
                if (c.rigidbody != null)
                {
                    var massMultiplier = massMultipliers[c.target.particleIndex];
                    if (massMultiplier > 0.0f)
                    {
                        var impulse = particleTargetFeedbacks[i] * (particleMass * massMultiplier * invDtAndSolverIterations);
                        c.rigidbody.AddForceAtPosition(impulse, c.target.position, ForceMode.Impulse);

                        if (c.rigidbodyDamping > 0.0f)
                        {
                            var normal = math.normalizesafe(impulse);
                            c.rigidbody.SetPointVelocityNow(c.target.position, normal, 0.0f, c.rigidbodyDamping);
                        }
                    }
                }

                // Reset particle mass multiplier (may have changed if body was kinematic)
                massMultipliers[c.target.particleIndex] = 1.0f;
            }
            
            liveRigidbodyConnections.Clear();

            Profiler.EndSample();
        }

        protected void ScheduleNextSimulationFrame()
        {
            Profiler.BeginSample(nameof(ScheduleNextSimulationFrame));

            computingSimulationFrame = true;

            var integrateParticles = new IntegrateParticlesJob()
            {
                deltaTime = Time.fixedDeltaTime,
                invDeltaTime = 1.0f / Time.fixedDeltaTime,
                externalAcceleration = Physics.gravity * simulation.gravityMultiplier,
                energyKept = 1.0f - simulation.energyLoss,

                positions = positions,
                prevPositions = prevPositions,
                massMultipliers = massMultipliers,
            }.Schedule();

            var enforceConstraints = new EnforceConstraintsJob()
            {
                positions = positions,
                prevPositions = prevPositions,
                massMultipliers = massMultipliers,

                isLoop = isLoop,
                solverIterations = simulation.solverIterations,
                stiffness = simulation.stiffness,
                desiredSpacing = _measurements.particleSpacing * simulation.lengthMultiplier,

                collisionsEnabled = collisions.enabled,
                radius = radius + collisions.collisionMargin,
                friction = collisions.friction,
                maxCollisionPlanesPerParticle = MaxCollisionPlanesPerParticle,
                activeCollisionPlanes = activeCollisionPlanes,
                collisionPlanes = collisionPlanes,

                particleTargets = particleTargets,
                particleTargetFeedbacks = particleTargetFeedbacks,
            }.Schedule(integrateParticles);

            simulationFrameHandle = new OutputVerticesJob()
            {
                positions = positions,
                bitangents = bitangents,
                isLoop = isLoop,
                radialVertices = radialVertices,
                radius = radius,
                cosLookup = cosLookup,
                sinLookup = sinLookup,
                vertices = vertices,
                normals = normals,
            }.Schedule(enforceConstraints);

            JobHandle.ScheduleBatchedJobs();

            Profiler.EndSample();
        }

        protected void CompletePreviousSimulationFrame()
        {
            if (!computingSimulationFrame)
            {
                return;
            }

            Profiler.BeginSample(nameof(CompletePreviousSimulationFrame));

            simulationFrameHandle.Complete();
            computingSimulationFrame = false;

            Profiler.EndSample();
        }

        protected static Matrix4x4[] customMeshFrames;

        protected void SubmitToRenderer()
        {
            if (material == null)
            {
                return;
            }

            Profiler.BeginSample(nameof(SubmitToRenderer));

            if (customMesh == null)
            {
                // Default rope cylinder
                if (simulation.enabled)
                {
                    vertices.CopyTo(vertices2);
                    normals.CopyTo(normals2);
                    mesh.vertices = vertices2;
                    mesh.normals = normals2;
                    mesh.RecalculateBounds();
                }

                Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0);
            }
            else
            {
                // Custom mesh at each simulation particle
                if (customMeshFrames == null || customMeshFrames.Length < positions.Length)
                {
                    customMeshFrames = new Matrix4x4[positions.Length];
                }

                var scale = Vector3.one * 0.5f * _measurements.particleSpacing;
                var currentRotation = 0.0f;
                for (var i = 0; i < positions.Length; i++) // use the positions array here and hope that the compiler optimizes away bounds checking...
                {
                    var tangent = Vector3.zero;
                    if (isLoop)
                    {
                        tangent = positions[(i + 1) % positions.Length] - positions[i];
                    }
                    else
                    {
                        tangent = i < positions.Length - 1 ? positions[i + 1] - positions[i] : positions[i] - positions[i - 1];
                    }
                    tangent.Normalize();

                    var rotation = Quaternion.LookRotation(tangent, bitangents[i]) * Quaternion.Euler(0.0f, 0.0f, currentRotation);
                    currentRotation += customMeshRotation;

                    customMeshFrames[i] = Matrix4x4.TRS(positions[i], rotation, scale);
                }

                Graphics.DrawMeshInstanced(customMesh, 0, material, customMeshFrames, positions.Length);
            }

            Profiler.EndSample();
        }

        public void FixedUpdate()
        {
            if (!initialized)
            {
                return;
            }
            if (!simulation.enabled)
            {
                simulationDisabledPrevFrame = true;
                return;
            }
            
            CompletePreviousSimulationFrame(); // fixed update might run several times per rendered frame

            if (simulationDisabledPrevFrame)
            {
                queuedRigidbodyConnections.Clear();
                liveRigidbodyConnections.Clear();
            }
            simulationDisabledPrevFrame = false;

            if (!firstParticleExplicitlyMovedFrame)
            {
                positions[0] = transform.position;
            }
            firstParticleExplicitlyMovedFrame = false;

            ApplyRigidbodyConnectionFeedback(); // from previous frame

            UpdateCollisionPlanes();

            PrepareRigidbodyConnections();
            
            ScheduleNextSimulationFrame();
        }

        public void LateUpdate()
        {
            if (!initialized)
            {
                return;
            }

            CompletePreviousSimulationFrame();

            transform.position = positions[0];

            SubmitToRenderer();
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (Application.isPlaying || spawnPoints.Count < 2 || !enabled)
            {
                return;
            }

            ComputeRealCurve(Allocator.Temp, out Measurements measurements, out NativeArray<float3> points);

            Gizmos.color = Colors.ropeSegments;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
            if (isLoop && points.Length > 1)
            {
                Gizmos.DrawLine(points[points.Length - 1], points[0]);
            }

            if (UnityEditor.Selection.Contains(gameObject))
            {
                var collisionsEnabled = collisions.enabled;
                var stride = collisions.stride;
                for (int i = 0; i < points.Length; i++)
                {
                    if (collisionsEnabled && i % stride == 0)
                    {
                        Gizmos.color = Colors.collisionParticle;
                    }
                    else
                    {
                        Gizmos.color = Colors.simulationParticle;
                    }
                    Gizmos.DrawSphere(points[i], radius);
                }
            }

            points.Dispose();
        }

        public void OnDrawGizmosSelected()
        {
            if (!initialized)
            {
                return;
            }

            var bounds = currentBounds;
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
#endif

        [BurstCompile]
        private struct IntegrateParticlesJob : IJob
        {
            [ReadOnly] public float deltaTime;
            [ReadOnly] public float invDeltaTime;
            [ReadOnly] public float3 externalAcceleration;
            [ReadOnly] public float energyKept;

            public NativeArray<float3> positions;
            public NativeArray<float3> prevPositions;
            [ReadOnly] public NativeArray<float> massMultipliers;

            public void Execute()
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (massMultipliers[i] == 0.0f)
                    {
                        continue;
                    }

                    var pos = positions[i];
                    var prevPos = prevPositions[i];

                    var vel = (pos - prevPos) * invDeltaTime;
                    vel += externalAcceleration * deltaTime;
                    vel *= energyKept;

                    prevPositions[i] = pos;
                    positions[i] += vel * deltaTime;
                }
            }
        }

        [BurstCompile]
        private struct EnforceConstraintsJob : IJob
        {
            // State
            public NativeArray<float3> positions;
            public NativeArray<float3> prevPositions;
            [ReadOnly] public NativeArray<float> massMultipliers;

            // Shape
            [ReadOnly] public bool isLoop;
            [ReadOnly] public int solverIterations;
            [ReadOnly] public float stiffness;
            [ReadOnly] public float desiredSpacing;

            // Collision handling
            [ReadOnly] public bool collisionsEnabled;
            [ReadOnly] public float radius;
            [ReadOnly] public float friction;
            [ReadOnly] public int maxCollisionPlanesPerParticle;
            [ReadOnly] public NativeArray<int> activeCollisionPlanes;
            [ReadOnly] public NativeArray<CollisionPlane> collisionPlanes;

            // Rigidbody attachments
            [ReadOnly] public NativeArray<ParticleTarget> particleTargets;
            public NativeArray<float3> particleTargetFeedbacks;

            public void Execute()
            {
                for (int i = 0; i < particleTargetFeedbacks.Length; i++)
                {
                    particleTargetFeedbacks[i] = float3.zero;
                }

                for (int iter = 0; iter < solverIterations; iter++)
                {
                    int loopCount = isLoop ? positions.Length : positions.Length - 1;

                    if (iter % 2 == 0) // alternate solving forwards and backwards to balance out errors
                    {
                        for (int i = 0; i < loopCount; i++)
                        {
                            ApplyStickConstraint(i, (i + 1) % positions.Length);
                        }
                    }
                    else
                    {
                        for (int i = loopCount - 1; i >= 0; i--)
                        {
                            ApplyStickConstraint(i, (i + 1) % positions.Length);
                        }
                    }

                    if (collisionsEnabled)
                    {
                        for (int i = 0; i < positions.Length; i++)
                        {
                            for (int j = 0; j < activeCollisionPlanes[i]; j++)
                            {
                                ApplyCollisionConstraint(i, collisionPlanes[i * maxCollisionPlanesPerParticle + j]);
                            }
                        }
                    }

                    for (int i = 0; i < particleTargets.Length; i++)
                    {
                        var target = particleTargets[i];
                        if (target.particleIndex == -1)
                        {
                            continue;
                        }

                        var delta = (target.position - positions[target.particleIndex]) * target.stiffness;
                        positions[target.particleIndex] += delta;
                        particleTargetFeedbacks[i] -= delta;
                    }
                }
            }

            private void ApplyStickConstraint(int idx0, int idx1)
            {
                var delta = positions[idx0] - positions[idx1];
                var dist = math.length(delta);
                if (dist > 0.0f)
                {
                    delta /= dist;
                }
                else
                {
                    delta = 0.0f;
                }

                var correction = (dist - desiredSpacing) * stiffness;

                var w0 = massMultipliers[idx0];
                if (w0 > 0.0f)
                {
                    w0 = 1.0f / w0;
                }
                var w1 = massMultipliers[idx1];
                if (w1 > 0.0f)
                {
                    w1 = 1.0f / w1;
                }
                var invSumW = w0 + w1;
                if (invSumW > 0.0f)
                {
                    invSumW = 1.0f / invSumW;
                }
                
                positions[idx0] -= delta * (correction * w0 * invSumW);
                positions[idx1] += delta * (correction * w1 * invSumW);
            }

            private void ApplyCollisionConstraint(int idx, CollisionPlane plane)
            {
                float dist = math.dot(positions[idx] - plane.point, plane.normal);
                if (dist <= radius)
                {
                    float depth = radius - dist;
                    positions[idx] += plane.normal * depth;

                    // Friction
                    var delta = (positions[idx] - prevPositions[idx]) - plane.velocityChange;
                    var length = math.lengthsq(delta);
                    if (length > 0.0f)
                    {
                        length = math.sqrt(length);
                        delta /= length;
                    }
                    prevPositions[idx] += delta * math.min(depth * friction, length);
                }
            }
        }

        [BurstCompile]
        private struct OutputVerticesJob : IJob
        {
            [ReadOnly] public NativeArray<float3> positions;

            public NativeArray<float3> bitangents;

            [ReadOnly] public bool isLoop;
            [ReadOnly] public int radialVertices;
            [ReadOnly] public float radius;
            [ReadOnly] public NativeArray<float3> cosLookup;
            [ReadOnly] public NativeArray<float3> sinLookup;

            [WriteOnly] public NativeArray<Vector3> vertices;
            [WriteOnly] public NativeArray<Vector3> normals;

            public void Execute()
            {
                var last = positions.Length - 1;

                // Diffuse bitangents
                var smoothedBitangents = new NativeArray<float3>(bitangents.Length, Allocator.Temp);

                smoothedBitangents[0] = bitangents[0] + bitangents[1];
                if (isLoop)
                {
                    smoothedBitangents[0] += bitangents[last];
                }
                for (int i = 1; i < bitangents.Length - 1; i++)
                {
                    smoothedBitangents[i] = bitangents[i - 1] + bitangents[i] + bitangents[i + 1];
                }
                smoothedBitangents[last] = bitangents[last - 1] + bitangents[last];
                if (isLoop)
                {
                    smoothedBitangents[last] += bitangents[0];
                }

                // Re-normalize bitangents
                for (int i = 0; i < bitangents.Length; i++)
                {
                    var tangent = positions[(i + 1) % positions.Length] - positions[i];
                    var normal = math.cross(tangent, smoothedBitangents[i]);

                    bitangents[i] = math.normalizesafe(math.cross(normal, tangent));
                }
                if (!isLoop)
                {
                    bitangents[last] = bitangents[last - 1];
                }

                // Set vertices
                for (int i = 0; i < positions.Length; i++)
                {
                    var tangent = float3.zero;
                    if (isLoop)
                    {
                        tangent = positions[(i + 1) % positions.Length] - positions[i];
                    }
                    else
                    {
                        tangent = i < last ? positions[i + 1] - positions[i] : positions[i] - positions[i - 1];
                    }
                    var bitangent = bitangents[i];
                    var normal = math.normalizesafe(math.cross(tangent, bitangent));

                    for (int j = 0; j < radialVertices; j++)
                    {
                        float3 extent = bitangent * cosLookup[j] + normal * sinLookup[j];
                        vertices[i * radialVertices + j] = positions[i] + extent * radius;
                        normals[i * radialVertices + j] = extent;
                    }
                }
            }
        }
    }
}
