using Godot;
using System;
using System.Diagnostics;

public partial class Enemy : PathFollow2D
{
	private ProgressBar _progressBar;
	private readonly float maxHP = 120;
	private float _hp;


	public float Attack { get; set; }

	public float Coins { get; set; }

	public float HP
	{
		get { return _hp; }
		set
		{
			_hp = value;
			OnHPChanged(value);
		}
	}

	private void OnHPChanged(float hp)
	{
		_progressBar.Value = hp;
		if (hp < maxHP && _progressBar.Visible)
		{
			_progressBar.Show();
		}
		if (hp <=0)
		{
			Die();
		}
	}

	public float Speed { get; set; } = 40;

	public Enemy()
	{

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_progressBar = GetNode<ProgressBar>("ProgressBar");
		Debug.WriteLine("Enemy Ready");
		HP = maxHP;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Progress += Speed * (float)delta;
	}

	private void Die() { }

}
