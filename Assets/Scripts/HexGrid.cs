using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[DeclareFoldoutGroup("ref", Title = "References")]
[ExecuteAlways, SelectionBase]
public class HexGrid : MonoBehaviour
{
    [Group("ref"), SerializeField] private Transform emptyPrefab;
    [Group("ref"), SerializeField] private Hole holePrefab;

    [SerializeField] private Vector2 cellOffset = new(1f, 1.155f);
    [SerializeField] private int holeCount = 3;
    [SerializeField] private Vector2Int gridSize = new(5, 5);
    private List<HoleCache> _holes = new();
    private List<HexCache> _hexes = new();
    private List<HoleCache> _holePool = new();
    private List<HexCache> _hexesPool = new();
    private bool _initialized = false;
    
    
    private void Start() => Initialize();
    private void OnEnable() => this.OnAssemblyReload(Initialize);

    [Button]
    private void Initialize()
    {
        if (_initialized) return;
        
        _holes.Clear();
        _hexes.Clear();
        var transformChildCount = transform.childCount;
        for(int i = 0; i < transformChildCount; i++)
        {
            var t = transform.GetChild(i);
            if (t.TryGetComponent(out Hole h))
            {
                if (t.gameObject.activeSelf)
                {
                    h.SetID(_holes.Count);//id starts at 0
                    _holes.Add(new(h.gameObject, t, h, CalculateFromPosition(t)));
                }
                else
                    _holePool.Add(new(h.gameObject, t, h, CalculateFromPosition(t)));
            }
            else
            {
                if (t.gameObject.activeSelf)
                    _hexes.Add(new(t.gameObject, t, CalculateFromPosition(t)));
                else
                    _hexesPool.Add(new(t.gameObject, t, CalculateFromPosition(t)));
            }
        }
    }

    private void OnDisable()
    {
        _initialized = false;
    }

    [Button]
    public void Generate()
    {
        int missingHoles = holeCount - _holes.Count;
        for (int i = 0; i < missingHoles; i++)
        {
            var h = Instantiate(holePrefab, transform);
            h.gameObject.SetActive(true);
            _holes.Add(new(h.gameObject, h.transform, h));
        }

        for (int i = 0; i < -missingHoles; i++)
        {
            _holePool.Add(_holes[^1]);
            _holes[^1].GameObject.SetActive(false);
            _holes.RemoveAt(_holes.Count - 1);
        }

        var hexesMissing = gridSize.x * gridSize.y - _hexes.Count - holeCount;
        for (int i = 0; i < hexesMissing; i++)
        {
            var x = Instantiate(emptyPrefab, transform);
            x.gameObject.SetActive(true);
            _hexes.Add(new(x.gameObject, x));
        }

        for (int i = 0; i < -hexesMissing; i++)
        {
            _hexesPool.Add(_hexes[^1]);
            _hexes[^1].GameObject.SetActive(false);
            _hexes.RemoveAt(_hexes.Count - 1);
        }

        var holesInds = new List<Vector2Int>();

        do
        {
            var index = new Vector2Int(Random.Range(1, gridSize.x - 1), Random.Range(1, gridSize.y - 1));
            if (!holesInds.Contains(index))
                holesInds.Add(index);
        } while (holesInds.Count < holeCount);


        int hexIndex = 0, holeIndex = 0;

        for (int x = 0; x < gridSize.x; x++)
        for (int y = 0; y < gridSize.y; y++)
        {
            var pos = new Vector2Int(x, y);
            if (holesInds.Contains(pos))
            {
                var h = _holes[holeIndex];
                _holes[holeIndex] = h.WithPosition(pos);
                Position(h.Transform, pos);
                holeIndex++;
            }
            else
            {
                var h = _hexes[hexIndex];
                _hexes[hexIndex] = h.WithPosition(pos);
                Position(h.Transform, pos);
                hexIndex++;
            }
        }
    }

    [Button]
    public void MoveHoleRandomly(int ind)
    {
        if (ind < 0 || ind >= _holes.Count) return;
        
        var h = _holes[ind];
        var pos = Vector2Int.zero;
        do
        {
            pos = new(Random.Range(1, gridSize.x - 1), Random.Range(1, gridSize.y - 1));
        } while (pos == h.Position || _holes.Exists(x => x.Position == pos));

        if (_hexes.TryFind(x => x.Position == pos, out var hex, out var hexInd))
        {
            Position(hex.Transform, h.Position);
            _hexes[hexInd] = hex.WithPosition(h.Position);
            Position(h.Transform, pos);
            _holes[ind] = h.WithPosition(pos);
            _holes[ind].Hole.JustMoved();
        }
    }

    private void Position(Transform transform, Vector2Int ind) => transform.localPosition =
        new((-(float)gridSize.x / 2 + ind.x + (ind.y % 2 > 0 ? .5f : 0)) * cellOffset.x, 0, ind.y * cellOffset.y);

    private Vector2Int CalculateFromPosition(Transform transform)
    {
        var pos = transform.localPosition;
        var y = Mathf.RoundToInt( pos.z / cellOffset.y);
        var x = Mathf.RoundToInt(pos.x / cellOffset.x - (y % 2 > 0 ? .5f : 0f) + (float)gridSize.x / 2);
        return new Vector2Int(x , y);
    }

    public struct HexCache : IHexCache
    {
        public GameObject GameObject { get; private set; }
        public Transform Transform {get; private set;}
        public Vector2Int Position { get;  private set; }

        public HexCache(GameObject gameObject, Transform transform)
        {
            GameObject =  gameObject;
            Transform =  transform;
            Position = -Vector2Int.one;
        }

        public HexCache(GameObject gameObject, Transform transform, Vector2Int position)
        {
            GameObject =  gameObject;
            Transform =  transform;
            Position = position;
        }

        public HexCache WithPosition(Vector2Int position)
        {
            Position = position;
            return this;
        }
    }
    public struct HoleCache : IHexCache
    {
        public GameObject GameObject { get; private set; }
        public Transform Transform {get; private set;}
        public Vector2Int Position { get;  private set; }
        public Hole Hole { get; private set; }

        public HoleCache(GameObject gameObject, Transform transform, Hole hole)
        {
            GameObject =  gameObject;
            Transform =  transform;
            Position = -Vector2Int.one;
            Hole = hole;
        }

        public HoleCache(GameObject gameObject, Transform transform, Hole hole, Vector2Int position)
        {
            GameObject =  gameObject;
            Transform =  transform;
            Position = position;
            Hole = hole;
        }

        public HoleCache WithPosition(Vector2Int position)
        {
            Position = position;
            return this;
        }
    }
    
    public interface IHexCache
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        Vector2Int Position { get; }
    }

    public void SubscribeHoles(UnityAction<int> onBallScored, UnityAction<int> onFail)
    {
        foreach (var hole in _holes)
        {
            hole.Hole.OnBallScored += onBallScored;
            hole.Hole.OnFail += onFail;
        }
    }

    public void UnsubscribeHoles(UnityAction<int> onBallScored, UnityAction<int> onFail)
    {
        foreach (var hole in _holes)
        {
            hole.Hole.OnBallScored -= onBallScored;
            hole.Hole.OnFail -= onFail;
        }
    }

    public Hole[] GetHoles()
    {
        if (!_initialized) Initialize();
        return _holes.Select(c => c.Hole).ToArray();
    }
}