using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Tower : Node2D
{
	private Area2D Area;
	private List<Enemy> Enemies = new List<Enemy>();
	private Sprite2D Turret;
	private Node2D Fort;
	private AudioStreamPlayer2D AudioStreamPlayer;
	private Timer AttackTimer;
	#region Bullet
	[Export]
	public int CoolDown { get; set; } = 2;

	[Export]
	public int BulletCount { get; set; } = 2;

	[Export]
	public int ReloadTime { get; set; } = 2;
	public bool IsReloading { get; set; } = false;
	private Timer ReloadTimer;

	public int CurrentBulletCount { get; set; }

	[Export]
	public int Damage { get; set; } = 1000;

	public void Attack()
	{
		if (Enemies == null || Enemies.Count == 0) return;

		// 如果子弹耗尽，开始重新装弹
		if (CurrentBulletCount <= 0)
		{
			StartReload();
			return;
		}
		GD.Print($"Attack on CurrentBulletCount = {CurrentBulletCount}.");
		GenerateBullet(Enemies[0]);
		CurrentBulletCount--;  // 每次攻击消耗一颗子弹

		// 如果这是最后一颗子弹，开始重新装弹
		if (CurrentBulletCount <= 0)
		{
			StartReload();
		}
	}

	private void StartReload()
	{
		if (IsReloading) return;  // 已经在重新装弹

		IsReloading = true;
		GD.Print("开始重新装弹...");
		ReloadTimer.Start(ReloadTime);
	}

	public void GenerateBullet(Enemy enemy)
	{
		var scene = ResourceLoader.Load<PackedScene>("res://src/action/Bullet.tscn");
		DefaultBullet = scene.Instantiate<Bullet>() ?? throw new Exception("DefaultBullet cast failed");
		if (DefaultBullet != null)
		{
			DefaultBullet.SetDamage(Damage);
			AddChild(DefaultBullet);

			DefaultBullet.Init(enemy);
			CurrentBulletCount -= 1;
		}
	}

	[Export]
	public Bullet DefaultBullet;
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Turret = GetNode<Sprite2D>("./Fort/Turret");
		Fort = GetNode<Node2D>("./Fort");
		AudioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		AttackTimer = new Timer();
		AddChild(AttackTimer);
		AttackTimer.Timeout += OnAttackTimerTimeout;
		AttackTimer.Start(CoolDown);

		ReloadTimer = new Timer();
		AddChild(ReloadTimer);
		ReloadTimer.Timeout += OnReloadComplete;

		Area.AreaEntered += _area_AreaEntered;
		Area.AreaExited += _area_AreaExited;
		CurrentBulletCount = BulletCount;
	}

	private void OnReloadComplete()
	{
		CurrentBulletCount = BulletCount;
		IsReloading = false;
		GD.Print("重新装弹完成！");
	}

	private void OnAttackTimerTimeout()
	{
		if (Enemies.Count > 0 && IsInstanceValid(Enemies[0]) && !IsReloading)
		{
			Attack();
		}
	}

	private void _area_AreaEntered(Area2D area)
	{
		Enemies.Add(area.Owner as Enemy);
	}

	private void _area_AreaExited(Area2D area)
	{
		Enemies.Remove(area.Owner as Enemy);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Enemies != null && Enemies.Count > 0)
		{
			if (IsInstanceValid(Enemies[0]))
			{
				Fort.LookAt(Enemies[0].GlobalPosition);
				Attack();
			}
			else
			{
				Enemies.RemoveAt(0);
			}
		}
	}
}
