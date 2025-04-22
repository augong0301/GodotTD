using Godot;
using System;
using static Godot.OpenXRInterface;
public partial class Main : Node2D
{
	[Export]
	public PackedScene[] Enemies;


	private TileMap TileMap;
	public GameForm GameForm { get; set; }

	private Tower tower;

	public Tower Tower
	{
		get { return tower; }
		set
		{
			if (tower != null)
			{
				tower.QueueFree();
				TileMap.RemoveChild(tower);
			}

			tower = value;

			if (tower != null)
			{
				TileMap.AddChild(tower);
			}
		}
	}


	private Timer timer;

	private void GenerateEnemies()
	{
		var rand = new Random();
		var index = rand.Next(0, Enemies.Length);
		var enemy = Enemies[index].Instantiate();
		var path2d = GetNode<Path2D>("./TileMap/Path2D");
		path2d.AddChild(enemy);

		TileMap = GetNode<TileMap>("./TileMap");

		GameForm = GetNode<GameForm>("./CanvasLayer/GameForm");
		GameForm.OnTowerReleased += WTowerClicked;
	}

	private void Timer_Timeout()
	{
		GenerateEnemies();
		timer.Start();
	}

	private void WTowerClicked(PackedScene tower)
	{
		Tower = (Tower)tower.Instantiate();
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
		PreviewTower();
	}

	private void PreviewTower()
	{
		if (Tower == null) return;

		var tile = TileMap.LocalToMap(TileMap.GetLocalMousePosition());
		Tower.Position = TileMap.MapToLocal(tile);

	}

	public override void _UnhandledInput(InputEvent input)
	{
		if (Tower == null) return;
		if (input is InputEventMouseButton ms)
		{
			if (ms.IsPressed() && ms.ButtonIndex == MouseButton.Left)
			{
				// 处理放置tower
				SetTower();
			}
			else if (ms.IsPressed() && ms.ButtonIndex == MouseButton.Right)
			{
				// cancel
				CancelPreviewTower();
			}


		}
		base._UnhandledInput(input);
	}

	private void SetTower()
	{
		var mousePos = TileMap.LocalToMap(TileMap.GetLocalMousePosition());
		if (CanSetTowerToPos(mousePos))
		{
			// play audio
			TileMap.SetCell(1, mousePos);
		}
	}

	private bool CanSetTowerToPos(Vector2I pos)
	{
		return true;
	}

	private void CancelPreviewTower()
	{
		if (Tower == null) return;

		Tower.QueueFree();
		TileMap.RemoveChild(Tower);
	}
}
