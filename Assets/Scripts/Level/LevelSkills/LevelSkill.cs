using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkill : MonoBehaviour
{
    public float AreaOfEffect = 15;
    public float effectTime = 4f;
    public ILevelSkill castSkill;

    bool _initialized;
    Vector3 _target;
    GameObject _sphere;

    public void OnInitCast()
    {
        _initialized = true;
        _sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _sphere.GetComponent<SphereCollider>().radius = AreaOfEffect;
        _sphere.transform.localScale = new Vector3(AreaOfEffect * 2, AreaOfEffect * 2, AreaOfEffect * 2);
        var m = new Material(Shader.Find("Standard"));
        m.SetFloat("_Mode", 3);
        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        m.SetInt("_ZWrite", 0);
        m.DisableKeyword("_ALPHATEST_ON");
        m.EnableKeyword("_ALPHABLEND_ON");
        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        m.renderQueue = 3000;
        m.color = new Color(0, 200, 0, .1f);
        _sphere.GetComponent<Renderer>().material = m;
    }

    public void OnCancelCast()
    {
        _initialized = false;
        Destroy(_sphere);
    }

    void Start () {
		
	}
	
	
	void Update ()
    {
        if (_initialized)
        {
            _target = GetTarget();
            _sphere.transform.position = _target;
            OnCheckInput();
        }
        else
            _target = Vector3.zero;
    }

    void OnCheckInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var go = Physics.OverlapSphere(_target, AreaOfEffect);

        }
    }

    Vector3 GetTarget()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = Camera.main.transform.position.y;
        pos = Camera.main.ScreenToWorldPoint(pos);
        var ray = new Ray(pos, (pos - Camera.main.transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Floor")))
        {
            return hit.point;
        }
        else
            return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (_initialized)
        {
            Gizmos.DrawWireSphere(GetTarget(), AreaOfEffect);
        }
    }
}
