using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
using Zenject;

public class PathFinder : MonoBehaviour
{
    public System.Action onRoadComplete;
    public Vector2[] RoadPoints { get; private set; }
    private Dictionary<int, List<int>> _near = new Dictionary<int, List<int>>();
    [Inject] Player _player;
    void Start()
    {
        var rs = FindObjectsByType<Road>(FindObjectsSortMode.None);

        for (int i = 0; i < rs.Length; i++)
        {
            rs[i].SetData(i,this);
        }
        RoadPoints = rs.Select(x => (Vector2)x.transform.position).ToArray();
        float dist = float.MaxValue;

        for (int i = 1; i < RoadPoints.Length; i++)
        {
            var d = Mathf.Abs(Vector2.Distance(RoadPoints[0], RoadPoints[i]));
            if (RoadPoints[0].x != RoadPoints[i].x && dist > d)
            {
                dist = d;
            }
        }

        for (int i = 0; i < RoadPoints.Length; i++)
        {
            _near.Add(i, new List<int>());
            for (int j = 0; j < RoadPoints.Length; j++)
            {
                if(Mathf.Abs(Vector2.Distance(RoadPoints[i], RoadPoints[j])) == dist)
                {
                    _near[i].Add(j);
                }
            }
        }
        onRoadComplete?.Invoke();
        _player.Init();
    }
    public int GetRoadPosition(Vector2 position)
    {
        float minDist = float.MaxValue;
        int res = 0;
        for (int i = 0; i < RoadPoints.Length; i++)
        {
            var r = RoadPoints[i];
            var dist = Mathf.Abs(Vector2.Distance(position, r));
            if (dist < minDist)
            {
                minDist = dist;
                res = i;
            }
        }
        return res;
    }
    public void SelectCell(int number)
    {
        _player.Move(number);
    }      
    public List<int> FindPath(int start, int end)
    {
        if (!_near.ContainsKey(start) || !_near.ContainsKey(end))
            return null;

        Dictionary<int, int> cameFrom = new Dictionary<int, int>();
        Queue<int> queue = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();
        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            if (current == end)
            {
                return ReconstructPath(cameFrom, start, end);
            }
            foreach (int next in _near[current])
            {
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    cameFrom[next] = current; 
                    queue.Enqueue(next);
                }
            }
        }
        return null;
    }
    private List<int> ReconstructPath(Dictionary<int, int> cameFrom, int start, int end)
    {
        List<int> path = new List<int>();
        int current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(start);
        path.Reverse(); 

        return path;
    }
}
     

