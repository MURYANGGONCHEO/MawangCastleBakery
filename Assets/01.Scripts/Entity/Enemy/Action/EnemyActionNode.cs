using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EnemyActionNode : MonoBehaviour
{
	[SerializeField] private TextMeshPro text;
	[SerializeField] private SpriteRenderer sp;
	private SpriteRenderer[] sprites;
	private void Awake()
	{
		sprites = GetComponentsInChildren<SpriteRenderer>();
	}
	public void SetData(Sprite icon,int i)
	{
		sp.sprite = icon;
		text.text = i.ToString();
	}
	public void Expand()
	{
		foreach (var sp in sprites)
		{
			sp.DOFade(1f, 0.3f);
		}
		transform.DOScale(Vector3.one * 0.7f, 0.3f);

	}
	public void Reduce()
	{
		foreach (var sp in sprites)
		{
			sp.DOFade(0.7f, 0.3f);
		}
		transform.DOScale(Vector3.one * 0.4f, 0.3f);
	}

}
