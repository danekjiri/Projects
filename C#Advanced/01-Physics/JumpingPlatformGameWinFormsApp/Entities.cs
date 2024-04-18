using PhysicsUnitsLib;
/*
 * IMPLEMENTATION OF GAME OBJECTS (enitities) AND ITS BEHAVIOUR (horizontal, vertical movements), GAME STRUCTURES (worldPoint, movement),
 */
namespace JumpingPlatformGame
{

    public struct WorldPoint
    {
        public Distance X { get; set; }
        public Distance Y { get; set; }
    }

    public struct Movement
    {
        public Distance LowerBound { get; set; } // left window border
        public Distance UpperBound { get; set; } // right window border
        public Speed Speed { get; set; }
    }

    /*
	 * The point [0,0] is located on BOTTOM-LEFT corner
	 */
    public enum HorizontalDirection { Left = -1, Right = 1 }
    public enum VerticalDirection { Up = 1, Down = -1, Stable = 0 }

    public abstract class Entity
    {
        public virtual Color Color => Color.Black;
        public WorldPoint Location;

        /*
         * Handle entity behaviour update after given time
         */
        public abstract void Update(Time sec);
    }

    public class MovableEntity : Entity
    {
        public Movement Horizontal;
        protected HorizontalDirection _horizontalDirection = HorizontalDirection.Right;

        /*
		 * Update location of the element by given speed and time, method implements HORIZONTAL behaviour of the element
		 */
        protected void UpdateHorizontalDirection(Time seconds)
        {
            Distance horizontalPositionDifference = (int)_horizontalDirection * (seconds * this.Horizontal.Speed);

            if (_horizontalDirection == HorizontalDirection.Right)
            {
                Distance windowOverflow = (this.Location.X + horizontalPositionDifference) - this.Horizontal.UpperBound;
                if ((double)windowOverflow >= 0) // went out of RIGHT window border - bounce
                {
                    _horizontalDirection = HorizontalDirection.Left;
                    this.Location.X = this.Horizontal.UpperBound - windowOverflow;
                }
                else
                {
                    this.Location.X += horizontalPositionDifference;
                }
            }
            else if (_horizontalDirection == HorizontalDirection.Left)
            {
                Distance windowOverflow = this.Location.X + horizontalPositionDifference;
                if ((double)windowOverflow <= 0) // went out of LEFT window border - bounce
                {
                    _horizontalDirection = HorizontalDirection.Right;
                    this.Location.X = windowOverflow;
                }
                else
                {
                    this.Location.X += horizontalPositionDifference;
                }
            }
        }

        public override void Update(Time seconds)
        {
            UpdateHorizontalDirection(seconds);
        }
    }

    public class MovableJumpingEntity : MovableEntity
    {
        public Movement Vertical;
        protected VerticalDirection _verticalDirection = VerticalDirection.Stable;

        /*
		 * Update location of the element by given speed and time, method implements VERTICAL behaviour of the element
		 */
        protected void UpdateVerticalDirection(Time seconds)
        {
            if ((double)this.Vertical.Speed > 0) // jump button is clicked
            {
                Distance verticalPositionDifference = (int)_verticalDirection * (seconds * this.Vertical.Speed);

                if (_verticalDirection == VerticalDirection.Stable)
                {
                    _verticalDirection = VerticalDirection.Up;
                }
                else if (_verticalDirection == VerticalDirection.Up)
                {
                    Distance windowOverflow = (this.Location.Y + verticalPositionDifference) - this.Vertical.UpperBound;

                    if ((double)windowOverflow >= 0) // went out of TOP window border - bounce
                    {
                        _verticalDirection = VerticalDirection.Down;
                        this.Location.Y = this.Vertical.UpperBound - windowOverflow;
                    }
                    else
                    {
                        this.Location.Y += verticalPositionDifference;
                    }
                }
                else if (_verticalDirection == VerticalDirection.Down)
                {
                    Distance windowOverflow = this.Location.Y + verticalPositionDifference;

                    if ((double)windowOverflow <= 0) // hit the ground or went out of BOTTON window border - stabilized
                    {
                        _verticalDirection = VerticalDirection.Stable;
                        this.Vertical.Speed = -((double)this.Vertical.Speed); // negative speed means stable
                        this.Location.Y = this.Vertical.LowerBound;
                    }
                    else
                    {
                        this.Location.Y += verticalPositionDifference;
                    }
                }
            }
            else // is stable
            {
                return;
            }
        }

        public override void Update(Time seconds)
        {
            UpdateHorizontalDirection(seconds); // horizontal behaviour is resolved in parent class MovableEntity
            UpdateVerticalDirection(seconds);
        }
    }

    public class Joe : MovableEntity
    {
        public override string ToString() => "Joe";
        public override Color Color => Color.Blue;
    }

    public class Jack : MovableEntity
    {
        public override string ToString() => "Jack";
        public override Color Color => Color.LightBlue;
    }

    public class Jane : MovableJumpingEntity
    {
        public override string ToString() => "Jane";
        public override Color Color => Color.Red;
    }

    public class Jill : MovableJumpingEntity
    {
        public override string ToString() => "Jill";
        public override Color Color => Color.Pink;
    }
}
