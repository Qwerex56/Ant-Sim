using Godot;
using System;
using System.Collections.Generic;

public partial class AntCommunicationComponent : Node {
  private HashSet<Vector2I> foodPlaces;
  public HashSet<Vector2I> FoodPlaces {get { return foodPlaces; } }

  private HashSet<Vector2> antPositions;
  public HashSet<Vector2> AntPositions {get { return antPositions; } }

  private HashSet<DeathMesasge> deathMassages;
  public HashSet<DeathMesasge> DeathMassages {get { return deathMassages; } }

  public enum CommuniqueTypeEnum {
    Food,
    Position,
    Death
  }

  public override void _Ready() {
    PushMassage(CommuniqueTypeEnum.Food, "Hello", "Darkness", 123, new Vector2I(1, 1));
  }

  public void PushMassage(CommuniqueTypeEnum type, params object[] args) {
    var argsList = new List<object>(args);

    Vector2I[] positions = (Vector2I[])argsList.Find((obj) => obj.GetType() == typeof(Vector2I[]));
    Vector2I position = (Vector2I)argsList.Find((obj) => obj.GetType() == typeof(Vector2I));
    Node2D ant = (Node2D)argsList.Find((obj) => obj.GetType() == typeof(Node2D));
    int round = (int)argsList.Find((obj) => obj.GetType() == typeof(int));

    switch (type) {
      case CommuniqueTypeEnum.Food:
        PushFood(positions, ant);
      break;
      
      case CommuniqueTypeEnum.Position:
        PushPosition(position, ant);
      break;
      
      case CommuniqueTypeEnum.Death:
        PushDeath(position, ant, round);
      break;
      
      default:
      break; 
    }
  }

  public void PopMassage() {
  }

  private void PushFood(Vector2I[] where, Node2D who) {

  }

  private void PushPosition(Vector2 position, Node2D who) {

  }

  private void PushDeath(Vector2I where, Node2D who, int when) {

  }

  public struct DeathMesasge {
    Vector2I place;
    Node2D ant;
    int round;
  }
}
