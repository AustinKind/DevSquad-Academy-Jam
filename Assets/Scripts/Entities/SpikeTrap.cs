using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpikeTrap : MonoBehaviour
{

    // How many spikes in a row their are.
    [SerializeField, Range(1, 10)] private int numberOfSpikes = 1;
    // Initial delay before starting the spike trap.
    [SerializeField] private float initialDelay = 0;
    // How long the spikes are retracted for.
    [SerializeField] private float idleTime = 4;
    // How long the spikes are in the ready state.
    [SerializeField] private float readyTime = 1;
    // How long the spikes are up for.
    [SerializeField] private float impaledTime = 2;
    // How much damage the spikes do.
    [SerializeField] private int damage = 1;

    BoxCollider2D col;
    Animator animator;
    SpriteRenderer rend;

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
#if UNITY_EDITOR
    private void OnValidate()
    {
        //EditorApplication.delayCall += () =>
        //{
        //    try
        //    {
        //        GetRequiredComponents();
        //        col.offset = new Vector2(-(numberOfSpikes - 1) / 2f, col.offset.y);

        //        bool multiple = (numberOfSpikes > 1);
        //        rend.drawMode = multiple ? SpriteDrawMode.Tiled : SpriteDrawMode.Simple;
        //        if (multiple)
        //        {
        //            rend.size = new Vector2(numberOfSpikes, 1);
        //            rend.tileMode = SpriteTileMode.Continuous;
        //        }
        //        else
        //            rend.size = Vector2.one;

        //        transform.localScale = Vector3.one;
        //    }
        //    catch (MissingReferenceException)
        //    {
        //        //Boi this is fucking ugly to have haha
        //    }
        //};
    }
#endif
    void GetRequiredComponents()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Creates all the colliders for the number of spikes chosen, all except the first one, which is controlled by animation
    /// </summary>
    void CreateExtraColliders()
    {
        List<BoxCollider2D> extras = new List<BoxCollider2D>();
        
        extras.Add(col);
        for (int i = 1; i < numberOfSpikes; i++)
        {
            BoxCollider2D add = gameObject.AddComponent<BoxCollider2D>();
            add.isTrigger = true;
            extras.Add(add);
        }

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
        yield return new WaitForSeconds(initialDelay);
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
            PlayerDamage player = null;
            if ((player = (dmgObj as PlayerDamage)) != null)
                player.SetKnockback(new Vector2(player.transform.position.x - transform.position.x, 1f));
            dmgObj.Hurt(damage);
        }
    }
}
