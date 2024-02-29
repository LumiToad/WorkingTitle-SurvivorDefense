using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : AbstractWeapon
{
    [SerializeField, SuffixLabel("seconds", true), TabGroup("Specifics")]
    private float duration = 3;
    
    [SerializeField, MinValue(0.1f), OnValueChanged("UpdateBookDistance"), TabGroup("Specifics")]
    private float bookRange = 4;

    [SerializeField, SuffixLabel("full spins per second", true), TabGroup("Specifics")]
    private float speed = 1;

    [SerializeField, FoldoutGroup("References")]
    private Animator booksAnim;

    private List<Book> books = new List<Book>();

    private void Start()
    {
        booksAnim = GetComponentInChildren<Animator>(true);

        foreach(var book in GetComponentsInChildren<Book>(true))
        {
            books.Add(book);
            book.SetUp(base.damage, this);
        }

        base.cooldown += duration + 0.5f;
    }

    protected override void Fire(AbstractEnemy target)
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        foreach(var book in books)
        {
            book.Show();
        }

        yield return new WaitForSeconds(0.15f);

        UpdateBookDistance();

        booksAnim.gameObject.SetActive(true);
        booksAnim.SetBool("Spin", true);
        booksAnim.SetFloat("speed", speed);

        yield return new WaitForSeconds(duration);

        booksAnim.SetBool("Spin", false);

        foreach (var book in books)
        {
            book.Hide();
        }
    }

    private void UpdateBookDistance()
    {
        if (bookRange < 0.1f) return;

        foreach (var book in GetComponentsInChildren<Book>())
        {
            var delta = book.transform.position - transform.position;
            delta.Normalize();

            book.transform.position = transform.position + delta * (bookRange / 2);
        }
    }

    private void OnDisable()
    {
        foreach (var book in books)
        {
            book.gameObject.SetActive(false);
        }
    }
}
