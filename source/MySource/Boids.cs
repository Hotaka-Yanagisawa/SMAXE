using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 必要な機能
 * 分離、整列、結合
 * pos,vel,acc
 * 長：中心となる
 * 
 */

public class Boids
{
    [SerializeField]
    private List<Transform> _transformList = new List<Transform>();
    [SerializeField]
    private List<Rigidbody2D> _rigidbodyList = new List<Rigidbody2D>();

    public void SetList(Rigidbody2D rb, Transform tr)
    {
        _rigidbodyList.RemoveAll(item => item == null);
        _transformList.RemoveAll(item => item == null);

        _rigidbodyList.Add(rb);
        _transformList.Add(tr);
    }

    /// <summary>
    /// 分離
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 Separation(Vector3 pos)
    {
        Vector3 total = Vector3.zero;
        Vector3 vec = Vector3.zero;
        Vector3 dif = Vector3.zero;
        foreach (var boid in _transformList)
        {
            if (pos == boid.position) continue;
            dif = pos - boid.position;
            if (dif.magnitude == 0) continue;
            total += dif.normalized / dif.magnitude;
        }

        return total;
    }
    /// <summary>
    /// 整列
    /// </summary>
    /// <param name="rb"></param>
    /// <returns></returns>
    public Vector2 Alignment(Rigidbody2D rb)
    {
        Vector2 vec = Vector2.zero;
        int boidsCnt = 0;
        foreach (var boid in _rigidbodyList)
        {
            if (rb.velocity == boid.velocity || boid.velocity.magnitude < 0.0001f)
            {
                continue;
            }
            boidsCnt++;
            vec += boid.velocity.normalized;
        }
        if (boidsCnt != 0)
        {
            vec = vec / boidsCnt;
        }
        return vec - rb.velocity.normalized;
    }
    /// <summary>
    /// 結合
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 Coupling(Vector3 pos)
    {
        Vector3 centerPos = Vector3.zero;
        return centerPos;
        //本来はこっち
        //foreach (var boid in _transformList)
        //{
        //    centerPos += boid.position;
        //}
        //centerPos /= _rigidbodyList.Count;
        //return new Vector2 (centerPos.x - pos.x, centerPos.y - pos.y);
    }

    public Vector2 Total(Vector3 pos, Rigidbody2D rb)
    {
        return Separation(pos) * 0.8f + Alignment(rb) + Coupling(pos);
    }

}
