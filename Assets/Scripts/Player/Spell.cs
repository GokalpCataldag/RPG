using UnityEngine;

public class Spell : PlayerSkillDamage
{
    public GameObject Explosion;
    public float speed=7;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.rotation = Quaternion.LookRotation(player.transform.forward);
    }
    internal override void Update()
    {
        base.Update();
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        if (colided)
        {
            Instantiate(Explosion,transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
