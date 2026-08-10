using UnityEngine;
using TMPro;

public class AngryTextWiggle : MonoBehaviour
{
    [Header("Angry Messages")]
    [TextArea(2, 5)]
    public string[] angryMessages;

    [Header("Wiggle Settings")]
    public float wiggleAmount = 1.5f;
    public float twitchSpeed = 0.06f;

    private TMP_Text text;
    private bool isWiggling = false;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (text == null)
            return;

        bool shouldWiggle = false;

        foreach (string angryMessage in angryMessages)
        {
            if (text.text == angryMessage)
            {
                shouldWiggle = true;
                break;
            }
        }

        if (shouldWiggle)
            StartWiggle();
        else
            StopWiggle();
    }

    public void StartWiggle()
    {
        isWiggling = true;
    }

    public void StopWiggle()
    {
        isWiggling = false;
    }

    void LateUpdate()
    {
        if (!isWiggling)
            return;

        text.ForceMeshUpdate();

        TMP_TextInfo textInfo = text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo character = textInfo.characterInfo[i];

            if (!character.isVisible)
                continue;

            int vertexIndex = character.vertexIndex;
            int materialIndex = character.materialReferenceIndex;

            Vector3[] vertices =
                textInfo.meshInfo[materialIndex].vertices;

            Random.InitState(
                i * 1000 +
                Mathf.FloorToInt(Time.time / twitchSpeed)
            );

            float x = Random.Range(-wiggleAmount, wiggleAmount);
            float y = Random.Range(-wiggleAmount, wiggleAmount);

            Vector3 offset = new Vector3(x, y, 0);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices =
                textInfo.meshInfo[i].vertices;

            text.UpdateGeometry(
                textInfo.meshInfo[i].mesh,
                i
            );
        }
    }
}