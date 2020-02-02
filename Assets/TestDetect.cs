using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDetect : MonoBehaviour
{
	[SerializeField]
	public float RestVelocityMin = 0.1f;
	[SerializeField]
	public float MatchingRayCastDistance = 0.1f; // TODO: Tweak this number

	const int CubeSides = 6;
	
	enum NeighbourDirections
	{
		Up = 0,
		Down,
		Left,
		Right,
		Front,
		Back
	};

	class CubeState
	{
		public int[] neighbours = new int[CubeSides];
		public bool touchingGround = false;
	}

	class DetectShape
	{
		public List<CubeState> matchRules = new List<CubeState>();

		// Go through each shape find the neighbours and there direction
		Dictionary<int, CubeState> GetCubeStates(Dictionary<int, GameObject> cubes, float castDistance)
		{
			Dictionary<int, CubeState> cubeStates = new Dictionary<int, CubeState>();

			foreach (var cubeKeyVal in cubes)
			{
				GameObject testCube = cubeKeyVal.Value;

				CubeState cubeState = new CubeState();

				Transform transform = testCube.transform;
				Vector3 halfBoxSize = transform.lossyScale / 2.1f;

				Vector3[] castDirections = new Vector3[CubeSides] {
					transform.up,
					transform.forward,
					transform.right,
					-transform.up,
					-transform.forward,
					-transform.right
				};

				List<string> alreadyHit = new List<string>();

				foreach (Vector3 castDir in castDirections)
				{
					Debug.DrawRay(transform.position, castDir, Color.yellow, 0.5f);

					RaycastHit[] hitInfos = Physics.BoxCastAll(transform.position, halfBoxSize, castDir, Quaternion.identity, castDistance);
					foreach (RaycastHit hitInfo in hitInfos)
					{
						GameObject otherCube = hitInfo.collider.gameObject;

						if (testCube.name == otherCube.name && !alreadyHit.Contains(testCube.name))
						{
							continue;
						}
						alreadyHit.Add(otherCube.name);

						Vector3 direction = otherCube.transform.position - transform.position;
						Vector3 absDirection = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
						
						if (hitInfo.collider.tag == "Ground")
						{
							cubeState.touchingGround = true;
						}
						else if (hitInfo.collider.tag == "Cube")
						{
							// Neighbour in the positive x dir
							if (absDirection.x > absDirection.y && absDirection.x > absDirection.z)
							{
								if (direction.x > 0)
								{
									cubeState.neighbours[(int)TestDetect.NeighbourDirections.Right]++;
								}
								else
								{
									cubeState.neighbours[(int)TestDetect.NeighbourDirections.Left]++;
								}
							}
							// Neighbour in the positive y dir
							else if (absDirection.y > absDirection.x && absDirection.y > absDirection.z)
							{
								if (direction.y > 0)
								{
									cubeState.neighbours[(int)TestDetect.NeighbourDirections.Up]++;
								}
								else
								{
									cubeState.neighbours[(int)TestDetect.NeighbourDirections.Down]++;
								}
							}
							// Neighbour in the positive z dir
							else if (absDirection.z > absDirection.x && absDirection.z > absDirection.y)
							{
								if (direction.y > 0)
								{
									cubeState.neighbours[(int)TestDetect.NeighbourDirections.Front]++;
								}
								else
								{
									cubeState.neighbours[(int)TestDetect.NeighbourDirections.Back]++;
								}
							}
						}
					}
				}

				cubeStates.Add(testCube.GetInstanceID(), cubeState);
			}

			return cubeStates;
		}

		public bool MatchesRule(Dictionary<int, GameObject> unmatchedCubes, Dictionary<int, GameObject> matchedCubes, float castDist)
		{
			int currentRuleNum = 0;

			bool stillMatching = true;

			Dictionary<int, CubeState> cubeStates = GetCubeStates(unmatchedCubes, castDist);

			// Check each detect rule
			while (stillMatching && currentRuleNum < matchRules.Count)
			{
				CubeState detectRule = matchRules[currentRuleNum];

				bool ruleMatched = true;

				foreach (var cubeKeyVal in unmatchedCubes)
				{
					GameObject testCube = cubeKeyVal.Value;
					CubeState cubeState = cubeStates[cubeKeyVal.Key];

					ruleMatched = true;

					// Loop through the final total of cube neighbours and compare against the detection rules neighbours
					if (cubeState.neighbours.Length == detectRule.neighbours.Length)
					{
						bool groundTouchMatch = (cubeState.touchingGround == detectRule.touchingGround);

						ruleMatched = (ruleMatched && groundTouchMatch);

						/* Comment out the stuff checkign all neighbours
							for (int i = 0; i < cubeState.neighbours.Length; i++)
							{
								int ourCount = cubeState.neighbours[i];
								int ruleCount = detectRule.neighbours[i];
								Debug.LogFormat("{0} - neighbour side {1}, ours count = {2}, rules count = {3}, touching ground match {4}",
									testCube.name, i, ourCount, ruleCount, groundTouchMatch);
								ruleMatched = ruleMatched && (ourCount == ruleCount) && groundTouchMatch;
							}
						*/

						{ // Look above 
							int ourCount = cubeState.neighbours[(int)NeighbourDirections.Up];
							int ruleCount = detectRule.neighbours[(int)NeighbourDirections.Up];

							ruleMatched = ruleMatched && (ourCount == ruleCount);
						}

						{ // Look Below 
							int ourCount = cubeState.neighbours[(int)NeighbourDirections.Down];
							int ruleCount = detectRule.neighbours[(int)NeighbourDirections.Down];

							ruleMatched = ruleMatched && (ourCount == ruleCount);
						}
					}
					else
					{
						ruleMatched = false;
					}

					if (ruleMatched)
					{
						matchedCubes.Add(cubeKeyVal.Key, cubeKeyVal.Value);
						unmatchedCubes.Remove(cubeKeyVal.Key);
						break;
					}
				}

				if (ruleMatched)
				{
					currentRuleNum++;
				}
				else
				{
					stillMatching = false;
				}
			}

			return stillMatching;
		}
	}

	List<DetectShape> detectShapes = new List<DetectShape>();
	DetectShape currentShape = null;

	Dictionary<int, GameObject> activeCubes = new Dictionary<int, GameObject>();

	// Used for the matching 
	Dictionary<int, GameObject> unmatchedCubes;
	Dictionary<int, GameObject> matchedCubes;

	// Start is called before the first frame update
	void Start()
	{
		DetectShape detectShape = null;

		{
			detectShape = new DetectShape();

			{
				CubeState cubeState = new CubeState();
				cubeState.touchingGround = true;
				//cubeState.neighbours[(int)NeighbourDirections.Down] = 1;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.touchingGround = true;
				//cubeState.neighbours[(int)NeighbourDirections.Down] = 1;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.neighbours[(int)NeighbourDirections.Down] = 2;
				detectShape.matchRules.Add(cubeState);
			}

			detectShapes.Add(detectShape);
		}

		{
			detectShape = new DetectShape();

			{
				CubeState cubeState = new CubeState();
				cubeState.touchingGround = true;
				//cubeState.neighbours[(int)NeighbourDirections.Down] = 1;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.neighbours[(int)NeighbourDirections.Down] = 1;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.neighbours[(int)NeighbourDirections.Down] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			detectShapes.Add(detectShape);
		}

		{
			detectShape = new DetectShape();

			{
				CubeState cubeState = new CubeState();
				cubeState.touchingGround = true;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.touchingGround = true;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 2;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.touchingGround = true;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.neighbours[(int)NeighbourDirections.Down] = 2;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.neighbours[(int)NeighbourDirections.Down] = 2;
				cubeState.neighbours[(int)NeighbourDirections.Up] = 1;
				detectShape.matchRules.Add(cubeState);
			}

			{
				CubeState cubeState = new CubeState();
				cubeState.neighbours[(int)NeighbourDirections.Down] = 2;
				detectShape.matchRules.Add(cubeState);
			}

			detectShapes.Add(detectShape);
		}


		SetDetectShape(2);
	}

	bool stopUpdating = false;

	// Update is called once per frame
	void Update()
	{
		if(stopUpdating)
		{
			return;
		}

		if (!IsReadyForDetection())
		{
			return;
		}

		unmatchedCubes = new Dictionary<int, GameObject>(activeCubes);
		matchedCubes = new Dictionary<int, GameObject>();

		bool matches = currentShape.MatchesRule(unmatchedCubes, matchedCubes, MatchingRayCastDistance);

		string matchedNames = "Matched = ";

		foreach(var cube in matchedCubes.Values)
		{
			matchedNames += cube.name + ", ";
		}

		string unmatchedNames = "Unmatched = ";
		foreach (var cube in unmatchedCubes.Values)
		{
			unmatchedNames += cube.name + ", ";
		}

		Debug.Log(matchedNames);
		Debug.Log(unmatchedNames);

		if (matches)
		{
			Time.timeScale = 0.0f;
			stopUpdating = true;
			Debug.Log("Match!");
		}
	}

	bool IsReadyForDetection()
	{
		if (currentShape == null || activeCubes.Count < currentShape.matchRules.Count)
		{
			return false; // Early exit cause we don't have enough cubes
		}

		bool allAtRest = true;

		foreach (GameObject shape in activeCubes.Values)
		{
			Rigidbody rigidbody = shape.GetComponent<Rigidbody>();
			if (rigidbody != null && allAtRest)
			{
				allAtRest = (rigidbody.velocity.magnitude < RestVelocityMin);
			}
		}

		return allAtRest;
	}

	void SetDetectShape(int detectShapeNum)
	{
		if(detectShapeNum > detectShapes.Count)
		{
			Debug.Break(); // We have a problem. Why is the number out of range!
			return;
		}

		currentShape = detectShapes[detectShapeNum];
	}

	private void OnTriggerEnter(Collider other)
	{
		var shape = other.gameObject;		
		activeCubes.Add(shape.GetInstanceID(), shape);
		Debug.Log("Adding shape to activeCubes. New Count = " + activeCubes.Count);
	}

	private void OnTriggerExit(Collider other)
	{
		var shape = other.gameObject;
		activeCubes.Remove(shape.GetInstanceID());
		Debug.Log("Removing shape from activeCubes. New Count = " + activeCubes.Count);
	}
}
