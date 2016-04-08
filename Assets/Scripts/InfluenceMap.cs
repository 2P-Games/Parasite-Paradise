using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfluenceMap : MonoBehaviour {

	public GameObject InfluenceMapTexture;
	public GameObject GridPositionObject;
	public List<GameObject> OriginatorObject;
	public LayerMask influenceMask;

	InfluenceGrid iG;

	// Use this for initialization
	void Start () {
		iG = new InfluenceGrid ();
		iG.CreateMap (100, 100, 1f, GridPositionObject, false);
		iG.InfluenceMask = influenceMask;
		for (int i = 0; i < OriginatorObject.Count; i++)
			iG.RegisterOriginator (OriginatorObject [i].GetComponent<Influencer> ().originator);
		iG.UpdateMap ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

[System.Serializable]
public class Originator {
	//World position originator
	public Vector3 worldPosition;

	//Influence produced by the Originator
	public float influence;

	//Range of the influence
	public float influenceRange;

	//Type of influence this Originator creates
	public int type;

	//Color of this type indicated on the map
	public Color32 color;
};

class GridPos {
	//World position of the this Grid space
	public Vector3 worldPosition;

	//list of Type of influce this position is possessed by and the
	//Current influence of this position  <type,influence>       
	public List<KeyValuePair<int,float>> influences;

	//List of all neighbors to this Grid space
	public List<GridPos> neighbors;

	//Color Representing this place on the map
	public Color32 myColor;

	//OPTIONAL: in game object representing the position
	public GameObject worldObject;

	//Returns whether or not the position has influence of a certain type
	public bool HasInfluenceOfType(int type)
	{
		for(int i = 0; i<influences.Count; i++)
		{
			if (influences[i].Key == type)
				return true;
		}
		return false;
	}

	//Returns a KeyValuePair of the influence type requested where KEY is the index of the
	//type in this positions list of influences and VALUE is the amount of influence it has
	//of said type.
	public KeyValuePair<int,float> GetInfluenceOfType(int type)
	{
		for (int i = 0; i < influences.Count; i++)
		{
			if (influences[i].Key == type)
				return new KeyValuePair<int, float>(i,influences[i].Value);
		}
		return new KeyValuePair<int, float>(0, -1);
	}
};

class InfluenceGrid {
	public LayerMask InfluenceMask;
	public Material TestMat;
	public Material TestMat2;
	public Material StartingMat;
	public Texture2D InfluenceMapTexture;
	//Grid of all positions in the map
	GridPos[,] m_Grid;
	bool RenderGroundGrid;

	//List of all Originators for the map
	List<Originator> m_OriginatorList = new List<Originator>();
	public void RegisterOriginator(Originator newOriginator) {
		m_OriginatorList.Add (newOriginator);	
	}
	float updateTimer;

	public void CreateMap (int x, int y, float spacing, GameObject gridObjects, bool RenderGround) {
		RenderGroundGrid = RenderGround;
		InfluenceMapTexture = new Texture2D (x, y);
		updateTimer = .5f;
		m_Grid = new GridPos[x, y];
		for(int i=0; i < x; i++) {
			for(int j = 0; j < y; j++) {
				GridPos gTemp = new GridPos ();
				gTemp.influences = new List<KeyValuePair<int, float>> ();
				gTemp.neighbors = new List<GridPos> ();
				gTemp.worldObject = GameObject.Instantiate (gridObjects, new Vector3 ((-1 * x / 2 + i) * spacing, 0, (-1 * y / 2 + j) * spacing), Quaternion.identity) as GameObject;
				gTemp.worldPosition = gTemp.worldObject.transform.position;
				gTemp.worldObject.GetComponent<InfluencePosition> ().GridPosition = new int[2] { i, j };
				m_Grid [i, j] = gTemp;
			}
		}

		for(int i = 0; i < x; i++) {
			for(int j = 0; j < y; j++) {
				if (i % x == 0)    //Column on the far left
				{
				    m_Grid[i, j].neighbors.Add(m_Grid[i + 1, j]);
				    if (j % y == 0)    //Bottom Left Corner
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j + 1]);
				    else if (j % y == y - 1)    //Top Left Corner
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j - 1]);
				    else
				    {
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j - 1]);
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j + 1]);
				    }

				}
				else if (i % x == x - 1)    //Column on the far right
				{
				    m_Grid[i, j].neighbors.Add(m_Grid[i - 1, j]);
				    if (j % y == 0)    //Bottom Left Corner
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j + 1]);
				    else if (j % y == y - 1)
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j - 1]);
				    else
				    {
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j - 1]);
				        m_Grid[i, j].neighbors.Add(m_Grid[i, j + 1]);
				    }
				}
				else if (j % y == 0)  //Bottom Row (sides excluded)
				{
				    m_Grid[i, j].neighbors.Add(m_Grid[i, j + 1]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i + 1, j]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i - 1, j]);
				    //m_Grid[i, j+1].worldObject.GetComponent<Renderer>().material = testMat;

				}
				else if (j % y == y - 1)    //Top Row (sides excluded)
				{
				    m_Grid[i, j].neighbors.Add(m_Grid[i, j - 1]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i + 1, j]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i - 1, j]);
				}
				else    //Middle only
				{
				    m_Grid[i, j].neighbors.Add(m_Grid[i, j - 1]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i, j + 1]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i + 1, j]);
				    m_Grid[i, j].neighbors.Add(m_Grid[i - 1, j]);
				}
			}
		}
	}

	public void UpdateMap () {
		updateTimer -= Time.deltaTime;
		if (updateTimer > 0)
			return;
		updateTimer = .5f;

		for(int i = 0; i < m_Grid.GetLength(0); i++) {
			for(int j = 0; j<m_Grid.GetLength(1); j++) {
                m_Grid[i, j].influences.Clear();
                m_Grid[i, j].myColor = Color.black;
                if (m_Grid[i, j].worldObject.GetComponent<Renderer>().enabled)
                {
                    m_Grid[i, j].worldObject.GetComponent<Renderer>().material.color = Color.black;

                    m_Grid[i, j].worldObject.GetComponent<Renderer>().enabled = false;

                }
            }
		}

		for (int i = 0; i<m_OriginatorList.Count; i++) {
	        Collider[] influencePositions = Physics.OverlapSphere(m_OriginatorList[i].worldPosition, m_OriginatorList[i].influenceRange, InfluenceMask);
	        for (int j = 0; j < influencePositions.Length; j++) {
	            GridPos currentPos = m_Grid[influencePositions[j].GetComponent<InfluencePosition>().GridPosition[0], influencePositions[j].GetComponent<InfluencePosition>().GridPosition[1]];
	            if (!m_Grid[influencePositions[j].GetComponent<InfluencePosition>().GridPosition[0], influencePositions[j].GetComponent<InfluencePosition>().GridPosition[1]].HasInfluenceOfType(m_OriginatorList[i].type))
	            {
	                m_Grid[influencePositions[j].GetComponent<InfluencePosition>().GridPosition[0], influencePositions[j].GetComponent<InfluencePosition>().GridPosition[1]].influences.Add(
	                    new KeyValuePair<int, float>(m_OriginatorList[i].type,(((Vector3.Distance(m_OriginatorList[i].worldPosition, influencePositions[j].transform.position)))) / m_OriginatorList[i].influenceRange));
	            }
	            else
	            {
	                KeyValuePair<int, float> iPos = m_Grid[influencePositions[j].GetComponent<InfluencePosition>().GridPosition[0], influencePositions[j].GetComponent<InfluencePosition>().GridPosition[1]].GetInfluenceOfType(m_OriginatorList[i].type);
	                if (iPos.Value != -1)
	                {
	                    m_Grid[influencePositions[j].GetComponent<InfluencePosition>().GridPosition[0], influencePositions[j].GetComponent<InfluencePosition>().GridPosition[1]].influences[iPos.Key] = new KeyValuePair<int, float>(m_OriginatorList[i].type, iPos.Value +
	                        (((Vector3.Distance(m_OriginatorList[i].worldPosition, influencePositions[j].transform.position))) / m_OriginatorList[i].influenceRange));
	                }
	            }
	            for (int k = 0; k < currentPos.influences.Count; k++)
	            {
	                if (RenderGroundGrid)
	                {
	                    currentPos.worldObject.GetComponent<Renderer>().enabled = true;
	                    currentPos.worldObject.GetComponent<Renderer>().material.color += ((Color)m_OriginatorList[i].color) * (currentPos.influences[k].Value) * (m_OriginatorList[i].influence);//-= new Color(1 - currentPos.influences[k].Value, 1 - currentPos.influences[k].Value, 0, 0) * m_OriginatorList[i].influence;// = TestMat;
	                }
	                currentPos.myColor += ((Color)m_OriginatorList[i].color)*(currentPos.influences[k].Value) * (m_OriginatorList[i].influence);
	                if (!RenderGroundGrid)
	                   currentPos.worldObject.GetComponent<Renderer>().enabled = false;
	            }
	        }
	    }

		for(int i = 0; i < m_Grid.GetLength(); i++) {
			for (int j = 0; j < m_Grid.GetLength (1); j++) {
				m_Grid [i, j].myColor.a = 255;
				InfluenceMapTexture.SetPixel (i, jvalue, m_Grid [i, j].myColor);
			}
		}
		InfluenceMapTexture.Apply ();
	}
}

public class InfluencePosition : MonoBehaviour {
	public int[] GridPosition;
}

