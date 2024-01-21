using Godot;
using System;

public partial class NpcPath : Node2D
{
	public PathNode StartNode  = ResourceLoader.Load<PackedScene>("res://Entities/NPCs/NPCPath/PathNode.tscn").Instantiate() as PathNode;

	[Export]
	public int Size = 300;

	private static RandomNumberGenerator randomGenerator = new RandomNumberGenerator();

	private static string[] PathShapes = new string[]{"Line", "Square"};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CreatePath(GetRandomPathShape());
	}
	
	private string GetRandomPathShape() {
		int maxIndex = PathShapes.Length - 1;

		return PathShapes[randomGenerator.RandiRange(0, maxIndex)];
	}

	private void CreatePath(string shape) {
		Vector2[] positions = Array.Empty<Vector2>();

		if(shape == "Line") {
		 positions = new Vector2[]{new Vector2(Size, 0)};
		}

		else if (shape == "Square"){
		 positions = new Vector2[]{new Vector2(Size, 0), new Vector2(Size, Size), new Vector2(0, Size)};
		}

		CreatePathWithPositions(positions);
	}
	
	//positions should include positions for every point EXCEPT STARTING NODE which will always be at 0,0.
	private void CreatePathWithPositions(Vector2[] positions) {
		AddChild(StartNode);

		PathNode currentNode = StartNode;

		for(int i = 0; i < positions.Length; i++) {
			PathNode nextNode = CreateNextPathNode(positions[i]);
			currentNode.NextNode = nextNode;
			currentNode = nextNode;
		}

		currentNode.NextNode = StartNode;
	}

	private PathNode CreateNextPathNode(Vector2 position) {
		PathNode nextPathNode = ResourceLoader.Load<PackedScene>("res://Entities/NPCs/NPCPath/PathNode.tscn").Instantiate() as PathNode;
		nextPathNode.Position = position;
		AddChild(nextPathNode);
		
		return nextPathNode;
	}
}
