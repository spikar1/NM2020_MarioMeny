using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MagicMissile : MonoBehaviour
{
    //float upTime = 1;
    Player target;

    

    private IEnumerator Start() {
        target = SelectTarget();
        var targetPos = target.transform.position;
        var direction = (targetPos - transform.position).normalized;
        var t = 0f;

        var y = transform.position.y;

        while (t < 1) {
            y += Manager.WorldOptions.magicMissileYCurve.Evaluate(t) * Manager.WorldOptions.magicMissileMaxSpeed * Time.deltaTime;
            var x = Mathf.MoveTowards(transform.position.x, target.transform.position.x, Manager.WorldOptions.magicMissileHomingSpeed * Time.deltaTime);

            transform.position = new Vector3(x, y);
            t += Time.deltaTime;
            yield return null;
        }
        while (true) {
            y += Manager.WorldOptions.magicMissileYCurve.Evaluate(1) * Time.deltaTime;
            var x = Mathf.MoveTowards(transform.position.x, target.transform.position.x, Manager.WorldOptions.magicMissileHomingSpeed * Time.deltaTime);

            transform.position = new Vector3(x, y);
            yield return null;
        }

    }

    private Player SelectTarget() {
        var players = FindObjectsOfType<Player>();

        var r = Random.Range(0, players.Length);
        return players[r];
    }
}
