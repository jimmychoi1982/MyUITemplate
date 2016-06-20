using UnityEngine;

public class TiltWindow : MonoBehaviour
{
	public Vector2 range = new Vector2(5f, 3f);

	Transform m_Transform;
	Quaternion m_StartRotation;
	Vector2 m_Rotation = Vector2.zero;

	void Start ()
	{
		m_Transform = transform;
		m_StartRotation = m_Transform.localRotation;
	}

	void Update ()
	{
        // 获得鼠标在世界的坐标
		Vector3 pos = Input.mousePosition;

        // 计算屏幕的长宽（一半）,也就是鼠标，触屏X,Y最大坐标
		float halfWidth = Screen.width * 0.5f;
		float halfHeight = Screen.height * 0.5f;

        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
		float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);

        Debug.Log("x:" + x + ":: y:" + y);

		m_Rotation = Vector2.Lerp(m_Rotation, new Vector2(x, y), Time.deltaTime * 5f);

		m_Transform.localRotation = m_StartRotation * Quaternion.Euler(-m_Rotation.y * range.y, m_Rotation.x * range.x, 0f);
	}
}
