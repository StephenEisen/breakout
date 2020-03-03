using Godot;

public abstract class Powerup : RigidBody2D
{

    protected Vector2 screenSize;
    protected bool isDropped = false;

    public override void _Ready()
    {
        this.Hide();
        this.screenSize = GetViewport().Size;
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("Disabled", true);
    }

    public void DropPowerup()
    {
        this.Show();
        ApplyCentralImpulse(new Vector2(0, 200));
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("Disabled", false);
        this.isDropped = true;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        if (isDropped)
        {
            var bodies = GetCollidingBodies();
            foreach (Node2D node in bodies)
            {
                if (node is Player || Position.y > screenSize.y)
                {
                    GetNode<Node>("/root/GlobalSignals").EmitSignal("AddPowerUp", this);
                    QueueFree();
                }
            }
        }
    }

    public abstract void OnPowerUp();

    public override string ToString()
    {
        return this.GetType().Name;
    }
}



