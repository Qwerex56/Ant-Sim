using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class Ant : Sprite2D {
  static readonly AntCommunicationComponent antCommunication = new AntCommunicationComponent();
  private Environment env;

  public Vector2I gridPosition { get; set; }
  
  private Queue<Vector2I> stepHistory = new();

  public int saturation;

  public override void _Ready() {
    var root = GetTree().Root;
    env = root.GetNode<Environment>("GlobalEnvironment");

    GlobalPosition = gridPosition * env.CELL_SIZE;
    saturation = env.saturationMax;
  }

  public void Move() {
    saturation -= env.saturationLost;
    antCommunication.PopMassage(AntCommunicationComponent.CommuniqueTypeEnum.Position, gridPosition);
    env.AntPositons.Remove(gridPosition);

    if (saturation <= 0) {
      antCommunication.PushMassage(AntCommunicationComponent.CommuniqueTypeEnum.Death, new() { gridPosition });
      return;
    }

    List<Vector2I> neighborCells = GetAdjecentCells();

    List<Vector2I> ListIntersection(List<Vector2I> l1, List<Vector2I> l2) {
      List<Vector2I> intersection = new List<Vector2I>();

      foreach (var item in (l1.Count > l2.Count)? l1 : l2) {
        if (!(l1.Contains(item) && l2.Contains(item))) continue;

        intersection.Add(item);
      }
      return intersection;
    }

    // Look into 
    var foodIntersection = ListIntersection(neighborCells, env.FoodPositions);
    var antIntersection = ListIntersection(neighborCells, env.AntPositons);

    if (foodIntersection.Count <= 0 || saturation >= env.saturationMax / 10 * 9) {
      // Remove occupied cells from neighborCells list
      foreach (var item in antIntersection) {
        neighborCells.Remove(item);
      }
      foreach (var item in foodIntersection) {
        neighborCells.Remove(item);
      }
      // Go to random legal position
      if (neighborCells.Count <= 0) {
        gridPosition = gridPosition;
      } else {
        gridPosition = neighborCells[(int) (new RandomNumberGenerator().Randi() % neighborCells.Count)];
      }
    } else {
      //go to random food
      gridPosition = foodIntersection[(int) (new RandomNumberGenerator().Randi() % foodIntersection.Count)];
      antCommunication.PopMassage(AntCommunicationComponent.CommuniqueTypeEnum.Food, gridPosition);
      env.FoodPositions.Remove(gridPosition);

      saturation = Mathf.Clamp(saturation + env.saturationRegain, 0, env.saturationMax);
    }

    // Update neighbors list
    neighborCells = GetAdjecentCells();

    // Look for food with new positon
    foodIntersection = ListIntersection(neighborCells, env.FoodPositions);

    // Push notifications
    antCommunication.PushMassage(AntCommunicationComponent.CommuniqueTypeEnum.Food, ListIntersection(neighborCells, env.FoodPositions));
    antCommunication.PushMassage(AntCommunicationComponent.CommuniqueTypeEnum.Position, new() { gridPosition });
    env.AntPositons.Add(gridPosition);

    GlobalPosition = gridPosition * env.CELL_SIZE + Vector2I.One * (env.CELL_SIZE / 2);
  }

  private List<Vector2I> GetAdjecentCells() {
    var adjecentCells = new List<Vector2I>();

    for (int x = 0, y = -1; x < 9; x++) {
      var xmod = (x % 3) - 1;
      adjecentCells.Add(new Vector2I(gridPosition.X + xmod, gridPosition.Y + y));

      if (xmod == 1) {
        y++;
      }
    }
    adjecentCells.Remove(gridPosition);

    adjecentCells.RemoveAll((vec) => { 
      return ( 
        vec.X < 0 || vec.Y < 0 ||
        vec.X >= env.GridSize.X || vec.Y >= env.GridSize.Y
      );
    });

    return adjecentCells;
  }
}
