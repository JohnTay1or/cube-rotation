using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public float speed = 100f;

    public enum cubeVertexNames : int { TO, TL, TR, FR, BA, BL, BR, BO };

    private Vector3[] cubeVertexPositions = new Vector3[]
    {
        new Vector3(-1, 1, -1),
        new Vector3(1, 1, -1),
        new Vector3(-1, 1, 1),
        new Vector3(1, 1, 1),
        new Vector3(-1, -1, -1),
        new Vector3(1, -1, -1),
        new Vector3(-1, -1, 1),
        new Vector3(1, -1, 1)
    };

    private bool moveInProgress = false;

    Queue moves = new Queue();

    // Use this for initialization
    void Start()
    {
    }

    void LogVertex(int vertex)
    {
        Debug.Log(((cubeVertexNames)vertex).ToString());
        Debug.Log(cubeVertexPositions[vertex].x);
        Debug.Log(cubeVertexPositions[vertex].y);
        Debug.Log(cubeVertexPositions[vertex].z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) moves.Enqueue((int)cubeVertexNames.TO);
        if (Input.GetKeyDown(KeyCode.E)) moves.Enqueue((int)cubeVertexNames.TR);
        if (Input.GetKeyDown(KeyCode.S)) moves.Enqueue((int)cubeVertexNames.BO);
        if (Input.GetKeyDown(KeyCode.A)) moves.Enqueue((int)cubeVertexNames.BL);
        if (Input.GetKeyDown(KeyCode.Q)) moves.Enqueue((int)cubeVertexNames.TL);
        if (Input.GetKeyDown(KeyCode.D)) moves.Enqueue((int)cubeVertexNames.BR);

        if (moves.Count != 0 && !moveInProgress)
        {
            moveInProgress = true;
            StartCoroutine("cubeRotate", (int)moves.Dequeue());
        }
    }

    IEnumerator cubeRotate(int move)
    {
        float cumulativeRotation = 0f;
        float degrees = Vector3.Angle(cubeVertexPositions[move], cubeVertexPositions[(int)cubeVertexNames.FR]);

        Vector3 perp = new Vector3();
        perp = Vector3.Cross(cubeVertexPositions[move], cubeVertexPositions[(int)cubeVertexNames.FR]);

        float thisRotation = speed * Time.deltaTime;

        while ((cumulativeRotation + thisRotation) < degrees)
        {
            transform.RotateAround(new Vector3(0, 0, 0), perp, -thisRotation);
            cumulativeRotation += thisRotation;
            yield return null;
        };
        transform.RotateAround(new Vector3(0, 0, 0), perp, -(degrees - cumulativeRotation));
        cumulativeRotation = 0;
        setCubeVertexPositions(((cubeVertexNames)move).ToString(), degrees > 90.0f);
        yield return new WaitForSeconds(1);
        moveInProgress = false;
    }

    void setCubeVertexPositions(string vertexName, bool bigRotation)
    {
        if (vertexName == "TO")
        {
            cubeVertexPositions = new Vector3[]
                {
                cubeVertexPositions[(int)cubeVertexNames.BA], //Top comes from Back
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BL]:
                    cubeVertexPositions[(int)cubeVertexNames.TL], //TL may stay the same or come from BL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BR]:
                    cubeVertexPositions[(int)cubeVertexNames.TR], //TR may stay the same or come from BR
                cubeVertexPositions[(int)cubeVertexNames.TO], //Front comes from Top
                cubeVertexPositions[(int)cubeVertexNames.BO], //Back comes from Bottom
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TL]:
                    cubeVertexPositions[(int)cubeVertexNames.BL], //BL may stay the same or come from TL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TR]:
                    cubeVertexPositions[(int)cubeVertexNames.BR], //TR may stay the same or come from BR
                cubeVertexPositions[(int)cubeVertexNames.FR] //Bottom comes from Front
                };
        }

        if (vertexName == "BO")
        {
            cubeVertexPositions = new Vector3[]
                {
                cubeVertexPositions[(int)cubeVertexNames.FR], //TO came from FR
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BL]:
                    cubeVertexPositions[(int)cubeVertexNames.TL], //TL may stay the same or come from BL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BR]:
                    cubeVertexPositions[(int)cubeVertexNames.TR], //TR may stay the same or come from BR
                cubeVertexPositions[(int)cubeVertexNames.BO], //FR came from BO
                cubeVertexPositions[(int)cubeVertexNames.TO], //BA came from TO
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TL]:
                    cubeVertexPositions[(int)cubeVertexNames.BL], //BL may stay the same or come from TL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TR]:
                    cubeVertexPositions[(int)cubeVertexNames.BR], //BR may stay the same or come from TR
                cubeVertexPositions[(int)cubeVertexNames.BA], //BO came from BA
                };
        }

        if (vertexName == "TL")
        {
            cubeVertexPositions = new Vector3[]
                {
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TR]:
                    cubeVertexPositions[(int)cubeVertexNames.TO], //TO may stay the same or come from TR
                cubeVertexPositions[(int)cubeVertexNames.BA], //TL comes from BA
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TO]:
                    cubeVertexPositions[(int)cubeVertexNames.TR], //TR may stay the same or come from TO
                cubeVertexPositions[(int)cubeVertexNames.TL], //FR came from TL
                cubeVertexPositions[(int)cubeVertexNames.BR], //BA came from BR
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BO]:
                    cubeVertexPositions[(int)cubeVertexNames.BL], //BL may stay the same or come from BO
                cubeVertexPositions[(int)cubeVertexNames.FR], //BR comes from FR
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BL]:
                    cubeVertexPositions[(int)cubeVertexNames.BO], //BO may stay the same or come from BL
                };
        }

        if (vertexName == "BR")
        {
            cubeVertexPositions = new Vector3[]
                {
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TR]:
                    cubeVertexPositions[(int)cubeVertexNames.TO], //TO may stay the same or come from TR
                cubeVertexPositions[(int)cubeVertexNames.FR], //TL comes from FR
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TO]:
                    cubeVertexPositions[(int)cubeVertexNames.TR], //TR may stay the same or come from TO
                cubeVertexPositions[(int)cubeVertexNames.BR], //FR came from BR
                cubeVertexPositions[(int)cubeVertexNames.TL], //BA came from TL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BO]:
                    cubeVertexPositions[(int)cubeVertexNames.BL], //BL may stay the same or come from BO
                cubeVertexPositions[(int)cubeVertexNames.BA], //BR comes from BA
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BL]:
                    cubeVertexPositions[(int)cubeVertexNames.BO], //BO may stay the same or come from BL
                };
        }

        if (vertexName == "TR")
        {
            cubeVertexPositions = new Vector3[]
                {
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TL]:
                    cubeVertexPositions[(int)cubeVertexNames.TO], //TO may stay the same or come from TL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TO]:
                    cubeVertexPositions[(int)cubeVertexNames.TL], //TL may stay the same or come from TO
                cubeVertexPositions[(int)cubeVertexNames.BA], //TR comes from BA
                cubeVertexPositions[(int)cubeVertexNames.TR], //FR comes from TR
                cubeVertexPositions[(int)cubeVertexNames.BL], //BA comes from BL
                cubeVertexPositions[(int)cubeVertexNames.FR], //BL comes from FR
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BO]:
                    cubeVertexPositions[(int)cubeVertexNames.BR], //BR may stay the same or come from BO
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BR]:
                    cubeVertexPositions[(int)cubeVertexNames.BO] //BO may stay the same or come from BR
                };
        }

        if (vertexName == "BL")
        {
            cubeVertexPositions = new Vector3[]
                {
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TL]:
                    cubeVertexPositions[(int)cubeVertexNames.TO], //TO may stay the same or come from TL
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.TO]:
                    cubeVertexPositions[(int)cubeVertexNames.TL], //TL may stay the same or come from TO
                cubeVertexPositions[(int)cubeVertexNames.FR], //TR comes from FR
                cubeVertexPositions[(int)cubeVertexNames.BL], //FR came from BL
                cubeVertexPositions[(int)cubeVertexNames.TR], //BA came from TR
                cubeVertexPositions[(int)cubeVertexNames.BA], //BL comes from BA
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BO]:
                    cubeVertexPositions[(int)cubeVertexNames.BR], //BR may stay the same or come from BO
                bigRotation ? cubeVertexPositions[(int)cubeVertexNames.BR]:
                    cubeVertexPositions[(int)cubeVertexNames.BO] //BO may stay the same or come from BR
                };
        }
    }
}