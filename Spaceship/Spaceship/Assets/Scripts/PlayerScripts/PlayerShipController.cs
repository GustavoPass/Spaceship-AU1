using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public sealed class PlayerShipController : MonoBehaviour, IEndGame{

    public static PlayerShipController instance { get; private set; }

    public delegate void DamageShip(int life);
    public event DamageShip OnDamage;

    private Transform trans;
    private Rigidbody rb;
    private AudioSource aud;

    private Material mat;
    private Color initialColor;
    private float materialTime;

    [Header("Vida")]
    private int life;

    [Header("Movimentação")]
    private float vel;
    [SerializeField] private float limiteMoveX, limiteMoveY;
    [SerializeField] private float limiteRotX, limiteRotZ;

    [Header("Ataque")]
    [SerializeField] private Transform muzzleSetTrans;
    private float reloadTime;
    [HideInInspector] public bool canShoot;
    public static int numHabilidade;
    public Habilidades[] habilidade;
    

    [Header("Habilidade única 1")]
    public AudioClip soundUnico;
    public Sprite spriteHabilidadeUnica1;
    public GameObject habilidadeUnica1;

    public delegate void SendSprite(Sprite sp);
    public SendSprite ShareSprite;

    private bool invunerable;

    private void Awake(){
        instance = this;

        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();

        var rend = GetComponent<Renderer>();
        mat = new Material(rend.material);
        rend.material = mat;
        initialColor = mat.GetColor("_EmissionColor");
    }

    private void Start() {
        GetMuzzles();
        SetValues();

        LevelManager.instance.OnDefeat += Defeat;
        LevelManager.instance.OnVictory += Victory;

        if (LoadManager.instance == null) return;

        LoadManager.instance.OnLoadCompleted += CompletedLoad;
        gameObject.SetActive(false);
        canShoot = false;
        invunerable = false;
    }
    private void OnDestroy() {
        LevelManager.instance.OnDefeat -= Defeat;
        LevelManager.instance.OnVictory -= Victory;
    }

    private void CompletedLoad() {
        gameObject.SetActive(true);
        StartCoroutine(tempo());
        LoadManager.instance.OnLoadCompleted -= CompletedLoad;
    }

    private void GetMuzzles() {
        muzzleSetTrans.parent = null;

        //Pega transforms de todos sets de muzzles
        for (int i = 0; i < habilidade.Length; i++) {
            habilidade[i].muzzles = new Transform[habilidade[i].muzzleSet.childCount];

            for (int k = 0; k < habilidade[i].muzzles.Length; k++) {
                habilidade[i].muzzles[k] = habilidade[i].muzzleSet.GetChild(k).GetComponent<Transform>();
            }
        }
    }

    private void SetValues() {
        for (int i = 0; i < habilidade.Length; i++) {
            habilidade[i].shootTime = (habilidade[i].shootTime / (10 * GameManager.instance.status.reloadMultiply));
        }

        limiteMoveX = Mathf.Abs(limiteMoveX);
        limiteMoveY = Mathf.Abs(limiteMoveY);
        limiteRotX = Mathf.Abs(limiteRotX);
        limiteRotZ = Mathf.Abs(limiteRotZ);
        numHabilidade = 0;

        vel = Constantes.startVelocity * GameManager.instance.status.velocityMultiply;
        life = Constantes.startLife + GameManager.instance.status.healthIncreased;
        aud.pitch = Mathf.Clamp(GameManager.instance.status.reloadMultiply, 1, 3);

        habilidadeUnica1.SetActive(false);
        canShoot = true;

        numHabilidade = 0;
        aud.clip = habilidade[numHabilidade].sound;
        aud.Play();
    }


    private void FixedUpdate(){
        MoveAndRotate();
    }

    private void Update() {
        muzzleSetTrans.position = trans.position;

        EscolherHabilidade();
        Shoot();

        if (materialTime > 0) materialTime -= Time.deltaTime;
        else mat.SetColor("_EmissionColor", initialColor);
    }

    private void Shoot() {
        if (!canShoot) return;

        if (numHabilidade >= 3) return;

        if(reloadTime <= 0) {
            for (int i = 0; i < habilidade[numHabilidade].muzzles.Length; i++) {
                habilidade[numHabilidade].pool.GetTiro().Shoot((habilidade[numHabilidade].muzzles[i]));
            }
            reloadTime = habilidade[numHabilidade].shootTime;
        }
        reloadTime -= Time.deltaTime;
    }

    private void MoveAndRotate() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        

        if (inputX != 0 || inputY != 0) {
            Vector3 moveVector = new Vector3(inputX, inputY, 0);
            rb.MovePosition(trans.position + moveVector * Time.deltaTime * vel);
            
            //Limitar posição
            Vector3 limiteMove = new Vector3(Mathf.Clamp(rb.position.x, -limiteMoveX, limiteMoveX),
                                                                Mathf.Clamp(rb.position.y, -limiteMoveY, limiteMoveY),
                                                                0);

            rb.position = limiteMove;
        }

        Vector3 rotDirection = new Vector3(0, -inputX * limiteRotX + 90, -90);
        trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.Euler(rotDirection), Time.deltaTime * 20);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("XP")){
            SoundLevelManager.instance.PlayXP();
            LevelManager.instance.AddXP(other.GetComponent<XPfragment>().GetXP());
        }
    }

    public void Damage(int dam) {
        if (invunerable) return;

        life -= dam;

        if (life <= 0) {
            life = 0;
            LevelManager.instance.EndGameDefeat();
        }

        materialTime = Constantes.materialDamageTimeReset * 2;
        mat.SetColor("_EmissionColor", Color.white);
        OnDamage?.Invoke(life);
    }
    
    public void Defeat() {
        PoolingExplosaoInimigo.instance.GetParticle().Explode(trans);
        habilidadeUnica1.SetActive(false);
        aud.Stop();
        gameObject.SetActive(false);
    }

    public void Victory() {
        canShoot = false;
        invunerable = true;
        aud.Stop();
        habilidadeUnica1.SetActive(false);
    }

    private void EscolherHabilidade() {
        if (!canShoot) return;

        if (Input.anyKeyDown) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                numHabilidade = 0;
                aud.clip = habilidade[numHabilidade].sound;
                aud.Play();
                if (ShareSprite != null) ShareSprite(habilidade[numHabilidade].spriteHabilidade);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                numHabilidade = 1;
                aud.clip = habilidade[numHabilidade].sound;
                aud.Play();
                if (ShareSprite != null) ShareSprite(habilidade[numHabilidade].spriteHabilidade);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                numHabilidade = 2;
                aud.clip = habilidade[numHabilidade].sound;
                aud.Play();
                if (ShareSprite != null) ShareSprite(habilidade[numHabilidade].spriteHabilidade);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                //HABILIDADE UNICA 1
                numHabilidade = 3;
                aud.clip = soundUnico;
                aud.Play();
                if (ShareSprite != null) ShareSprite(spriteHabilidadeUnica1);
            }
            habilidadeUnica1.SetActive(numHabilidade == 3);
        }
    }

    private IEnumerator tempo() {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;

        numHabilidade = 0;
        aud.clip = habilidade[numHabilidade].sound;
        if (ShareSprite != null) ShareSprite(habilidade[numHabilidade].spriteHabilidade);
        aud.Play();
    }

    public Transform GetTransform() {
        return trans;
    }
    public int GetLife() {
        return life;
    }


}




[System.Serializable]
public struct Habilidades {

    public AudioClip sound;
    public Sprite spriteHabilidade;
    public PoolingTiroPlayer pool;
    public float shootTime;
    public Transform muzzleSet;
    [HideInInspector] public Transform[] muzzles;
}


[System.Serializable]
public struct SpaceShipStatus {
    public int healthPoints;
    public int damagePoints;
    public int reloadPoints;
    public int velocityPoints;

    //variáveis que retornam valor para escalonar status do player
    public int healthIncreased {
        get { return healthPoints * 5; }
        private set { }
    }

    public float damageMultiply {
        get { return 1.0f + damagePoints * 0.05f; }
        private set { }
    }

    public float reloadMultiply {
        get { return 1.0f + (reloadPoints * 0.02f); }
        private set { }
    }

    public float velocityMultiply {
        get { return 1.0f + velocityPoints * 0.02f; }
        private set { }
    }

    //variável que retorna soma de todos pontos usados
    public int totalPontosUsados {
        get { return this.damagePoints + this.healthPoints + this.reloadPoints + this.velocityPoints; }
        private set { }
    }

}