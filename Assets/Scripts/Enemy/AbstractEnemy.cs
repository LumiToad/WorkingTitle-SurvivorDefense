using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractEnemy : MonoBehaviour, IPoolable, IDamageAble, IDamageSource
{
    const float rotationSpeed = 10;
    
    public event Action<AbstractEnemy> died;
    public bool IsOnScreen => gameObject.IsOnScreen(60.0f);

    [Title("Generic Enemy Stats")]
    [SerializeField]
    private FloatStat speed;
    [SerializeField]
    private FloatStat hp;
    [SerializeField]
    private int damage;
    [SerializeField, SuffixLabel("In Seconds", true)]
    private float attackCooldown;

    [SerializeField, FoldoutGroup("NonBalanceSettings")]
    private bool ignoreDeathAnimation;
    [SerializeField, FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true), FoldoutGroup("NonBalanceSettings")]
    private string filePath;
    [SerializeField, FoldoutGroup("NonBalanceSettings")]
    public string glueName;
    public string enemyName { get { return CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)[glueName]; } }

    public string glueSourceName => glueName;

    [SerializeField, FoldoutGroup("LevelSettings"), SuffixLabel("seconds", true)]
    private float hpIncreaseTime;
    [SerializeField, FoldoutGroup("LevelSettings")]
    private float bonusHP;

    private float activeAttackCoolDown = 0;

    private EnemyNavigation navigation;
    private Rigidbody rb;
    private EnemyMesh mesh;
    private Collider collider_;
    private Animator anim;

    public void SetUp()
    {
        navigation.SetUp(transform.position);
        hp += GetBonusHealth();
        GetComponentInChildren<Dissolve>().dissolved += OnDissolved;
    }

    public void SetTargetPos(Vector3 pos, int priority)
    {
        navigation.SetTarget(pos, priority);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider_ = GetComponent<Collider>();
        mesh = GetComponentInChildren<EnemyMesh>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        activeAttackCoolDown -= Time.deltaTime;

        if(speed > 0 && rb.velocity != new Vector3())
        {
            var dir = navigation.GetMovementDirection(transform.position).normalized;
            dir.y = 0;
            if (dir == new Vector3()) return;
            var rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        navigation.Update(transform.position);
        rb.velocity = navigation.GetMovementDirection(transform.position).normalized * speed;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(activeAttackCoolDown <= 0)
        {
            var target = collision.transform.GetComponent<IDamageAble>();
            if(target != null && target.gameObject.layer != this.gameObject.layer)
            {
                target.TakeDamage(damage, this);
                activeAttackCoolDown = attackCooldown;

                if(target.gameObject.GetComponent<PlayerBase>() != null)
                {
                    Die();
                }
            }
        }
    }

    public void GainBonusHealth(float health) => hp = hp + health;
    public void GainBonusSpeed(float speed) => this.speed = this.speed + speed;
    public void GainBonusDamage(int damage) => this.damage = this.damage + damage;

    public void TakeDamage(int damage, IDamageSource source)
    {
        if (damage <= 0 || !this.enabled) return;

        hp -= damage;
        AchivementManager.ProgressAchivement(damage, new[] {source});

        mesh.Blink();

        var damageNumber = Pool.Get<DamageNumber>();
        damageNumber.transform.position = transform.position;
        damageNumber.SetUp(damage);

        var damageAudio = Pool.Get<PoolableSound>("DamageSound");
        damageAudio.transform.position = transform.position;
        damageAudio.Play();

        if (hp <= 0) 
        {
            var killAudio = Pool.Get<PoolableSound>("EnemyDeathSound");
            if(killAudio != null)
            {
                killAudio.transform.position = transform.position;
                killAudio.Play();
            }

            if (IsOnScreen)
            {
                var xp = Pool.Get<XPItem>();
                AchivementManager.ProgressAchivement(1, new[] {this});
                if (xp != null)
                {
                    xp.transform.position = transform.position;
                }
            }

            Die();
        }
    }

    private void Die()
    {
        if (ignoreDeathAnimation)
        {
            GetComponentInChildren<Dissolve>().Begin(dissolveMode.dissolve);
        }
        else
        {
            anim.SetTrigger("Die");
            rb.velocity = new Vector3();
        }


        collider_.enabled = false;
        this.enabled = false;

        died?.Invoke(this);
        died = null;
    }
    
    private float GetBonusHealth()
    {
        return (GameTime.time / hpIncreaseTime) * bonusHP;
    }

    private void OnDissolved()
    {
        Pool.Return<AbstractEnemy>(this.gameObject);
        GetComponentInChildren<Dissolve>().dissolved -= OnDissolved;
    }

    #region IPoolable
    public void OnReturnedToPool()
    {
        hp.Reset();
        speed.Reset();
        mesh.Reset();
        navigation = new EnemyNavigation();
        this.enabled = false;
    }

    public void OnTakenFromPool()
    {
        mesh.Manifest();
        collider_.enabled = true;
        this.enabled = true;
    }
    #endregion
}