using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpikeTrap : MonoBehaviour
{
    public Animator animator;

    // How many spikes in a row their are.
    [SerializeField, Range(1, 10)] private int numberOfSpikes = 1;
    // How long the spikes are retracted for.
    [SerializeField] private float idleTime = 4;
    // How long the spikes are in the ready state.
    [SerializeField] private float readyTime = 1;
    // How long the spikes are up for.
    [SerializeField] private float impaledTime = 2;
    // How much damage the spikes do.
    [SerializeField] private int damage = 1;

    BoxCollider2D col;
    SpriteRenderer rend;
    
    Transform extraColliderHolder;
    BoxCollider2D[] extraColliders;

    void Start ()
    {
        GetRequiredComponents();
        StartCoroutine("SpikeCycle");
        CreateExtraColliders();
    }

    /// <summary>
    /// Make sure the changes in the number of spikes adjust the rest of the components' variables
    /// </summary>
    private void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            GetRequiredComponents();
            col.offset = new Vector2(-(numberOfSpikes - 1) / 2f, col.offset.y);

            bool multiple = (numberOfSpikes > 1);
            rend.drawMode = multiple ? SpriteDrawMode.Tiled : SpriteDrawMode.Simple;
            if (multiple)
            {
                rend.size = new Vector2(numberOfSpikes, 1);
                rend.tileMode = SpriteTileMode.Continuous;
            }
            else
                rend.size = Vector2.one;

            transform.localScale = Vector3.one;
        };
    }

    void GetRequiredComponents()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Creates all the colliders for the number of spikes chosen, all except the first one, which is controlled by animation
    /// These are placed in a child object within this one that stores all the extra colliders as to not cramp the parent object
    /// </summary>
    void CreateExtraColliders()
    {
        if (extraColliderHolder == null)
        {
            extraColliderHolder = new GameObject().transform;
            extraColliderHolder.SetParent(this.transform);
            extraColliderHolder.localPosition = Vector3.zero;
            extraColliderHolder.name = "_ExtraColliders";
        }

        List<BoxCollider2D> extras = new List<BoxCollider2D>();
        
        extras.Add(col);
        for (int i = 1; i < numberOfSpikes; i++)
            extras.Add(extraColliderHolder.gameObject.AddComponent<BoxCollider2D>());

        for(int i = 1; i < numberOfSpikes; i++)
        {
            extras[i].size = col.size;

            float xOffset = -(numberOfSpikes - 1) / 2f;
            extras[i].offset = new Vector2(xOffset + i, col.offset.y);
        }

        extraColliders = extras.ToArray();
    }

    IEnumerator SpikeCycle ()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
            animator.SetTrigger("Ready");
            yield return new WaitForSeconds(readyTime);
            animator.SetTrigger("Impale");
            yield return new WaitForSeconds(impaledTime);
            animator.SetTrigger("Retract");
        }
    }

    private void Update()
    {
        for (int i = 1; i < numberOfSpikes; i++)
        {
            BoxCollider2D extra = extraColliders[i];
            extra.offset = new Vector2(extra.offset.x, col.offset.y);
            extra.size = col.size;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Damageable dmgObj = null;
        if ((dmgObj = col.GetComponent<Damageable>()) != null)
        {
            if(dmgObj as PlayerDamage)
                dmgObj.Hurt(damage);
        }
    }
}
