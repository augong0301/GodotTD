using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Node2D
{
	private Area2D Area;
	private List<Enemy> Enemies = new List<Enemy>();
	private Sprite2D Turret;
	private Node2D Fort;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Turret = GetNode<Sprite2D>("./Fort/Turret");
		Fort = GetNode<Node2D>("./Fort");
		Area.AreaEntered += _area_AreaEntered;
		Area.AreaExited += _area_AreaExited;
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
			}
			else
			{
				Enemies.RemoveAt(0);
			}
		}
	}
}
