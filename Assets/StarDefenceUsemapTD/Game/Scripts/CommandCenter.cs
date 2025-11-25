using STARTD.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    [SerializeField] private Transform command;
    [SerializeField] private Transform leftMineral;
    [SerializeField] private Transform rightMineral;
    [SerializeField] private GameObject SCV;

    private List<GameObject> SCVS = new();

    private void Awake()
    {
        CreateSCV();
        // 필요하면 SCV 상태 체크
    }

    public void CreateSCV()
    {
        GameObject newScv = Instantiate(SCV, command.position, Quaternion.identity);
        SCVS.Add(newScv);

        StartCoroutine(SCVWork(newScv));
    }

    private IEnumerator SCVWork(GameObject scv)
    {
        Transform targetMineral =
            Random.value > 0.5f ? leftMineral : rightMineral;

        while (true)
        {
            // 1. Mineral로 이동
            yield return MoveTo(scv.transform, targetMineral.position, 2f);

            // 채취 시간
            yield return new WaitForSeconds(1f);

            // 2. Command로 이동
            yield return MoveTo(scv.transform, command.position, 2f);

            // 자원 반납
            GameScene.Singleton.TryAddGold(100);

            // 다시 채굴 반복
        }
    }

    private IEnumerator MoveTo(Transform obj, Vector3 target, float speed)
    {
        while (Vector3.Distance(obj.position, target) > 0.1f)
        {
            obj.position = Vector3.MoveTowards(obj.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
