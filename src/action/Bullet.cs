using Godot;
using System;

public partial class Bullet : Node2D
{

	public float Damage { get; private set; }

	public Area2D Area { get; set; }

	[Export]
	public float Speed { get; set; } = 500;

	private Enemy Target;

	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Area.AreaEntered += OnBulletAreaEntered;
		//Damage = 10;
	}

	private void OnBulletAreaEntered(Area2D area)
	{
		if (area.Owner is Enemy enemy)
		{
			DamageTo(enemy);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Target != null && IsInstanceValid(Target))
		{
			var dir = Target.GlobalPosition - this.GlobalPosition;
			dir = dir.Normalized();
			Rotation = dir.Angle();
			Position += dir * Speed * (float)delta;
		}
		else
		{
			QueueFree();
		}
	}

	internal void Init(Enemy enemy)
	{
		Target = enemy;
	}

	public void DamageTo(Enemy enemy)
	{
		enemy.Hit(Damage);
		QueueFree();
	}

	internal void SetDamage(float damage)
	{
		Damage = damage;
	}
}
