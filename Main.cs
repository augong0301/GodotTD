using Godot;
using System;
using static Godot.OpenXRInterface;
public partial class Main : Node2D
{
	[Export]
	public PackedScene[] Enemies;

	private Timer timer;

	private void GenerateEnemies()
	{
		var rand = new Random();
		var index = rand.Next(0, Enemies.Length);
		var enemy = Enemies[index].Instantiate();
		var path2d = GetNode<Path2D>("./TileMap/Path2D");
		path2d.AddChild(enemy);



	}

	private void Timer_Timeout()
	{
		GenerateEnemies();
		timer.Start();
	}



	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		var rand = new Random();

		timer = GetNode<Timer>("TimerSpawnEnemy");
		timer.WaitTime = rand.Next(1, 3);
		timer.Timeout += Timer_Timeout;
		timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
