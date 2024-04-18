using System.Text;

namespace MinimalisticUIFramework
{
    public interface IControl
    {
        public Control Control { get; set; }
        public abstract string ToString();
    }

    public record struct ControlWithoutLocation(Control Control) : IControl
    {
        public override string ToString() => this.Control.ToString()!;
    }

    public record struct ControlWithLocation(Control Control, ILocation Location) : IControl
    {
        public override string ToString() => $"{this.Control.ToString()} at {this.Location.ToString()}";
    }

    /*
     * an object template that is able to store other 'IControl' objects
     */
    public abstract class Panel : Control
    {
        protected List<IControl> _childrenControls = new List<IControl>();
        public abstract void AddChild(Control child, ILocation? location = null);
    }

    public interface ILocation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public abstract string ToString();
    }

    /*
     * structure that stores position of an object in 'Canvas'-like object
     */
    public record struct Point(int X, int Y) : ILocation
    {
        public override string ToString() => $"{X}, {Y}";
    }

    /*
     * panel object that stores objects (controls) as they go (FIFO)
     */
    public sealed class StackPanel : Panel
    {
        // append new object (child) to the panel 'memory', location does not matter in StackPanel
        public override void AddChild(Control child, ILocation? location = null)
        {
            this._childrenControls.Add(new ControlWithoutLocation(child));
        }

        // return string of all children.ToString() format as they came
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("StackPanel {\n");

            for (int i = 0; i < _childrenControls.Count; i++)
            {
                sb.Append($"{_childrenControls[i].ToString()}\n");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    /*
     * canvas obejct that stores objects (controls) as they go but with their required location
     */
    public sealed class Canvas : Panel
    {
        // append new object (child) to the panel 'memory' with its location, if location is null then throw exception
        public override void AddChild(Control child, ILocation? location)
        {
            if (location is null)
            {
                throw new LocationNotSetException();
            }
            this._childrenControls.Add(new ControlWithLocation(child, location));
        }

        // return string of all children.ToString() format as they came
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Canvas {\n");

            for (int i = 0; i < _childrenControls.Count; i++)
            {
                if (((ControlWithLocation)_childrenControls[i]).Location == null)
                {
                    _childrenControls.RemoveAt(i);
                }
                else
                {
                    sb.Append($"{_childrenControls[i].ToString()}\n");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    /*
     * generic structure for that support fluent syntax -> solves problem with required location in Canvas panel object 
     *  serves as return 'package' between .PlacedIn(Canvas canvas) and .At(int x, int y)
     */
    public record struct CanvasAndControl<T>(Canvas Canvas, T Control) where T: Control{ }

    /*
     * defined extension methods for fluent syntax using generic methods
     */
    public static class ControlsExtension
    {
        // more general method for non-canvas object that just add given control to wanted panel
        public static T PlacedIn<T>(this T Tcontrol, Panel panel) where T : Control
        {
            panel.AddChild(Tcontrol);
            return Tcontrol;
        }
        
        // more specific method for canvas, that needs to set location in the very next step by extension method .At(int x, int y)
        public static CanvasAndControl<T> PlacedIn<T>(this T Tcontrol, Canvas canvas) where T : Control
        {
            return new CanvasAndControl<T>(canvas, Tcontrol);
        }

        // add an object with its location into the canvas panel object
        public static T At<T>(this CanvasAndControl<T> canvasAndControl, int x, int y) where T : Control
        {
            canvasAndControl.Canvas.AddChild(canvasAndControl.Control, new Point() { X = x, Y = y});
            return canvasAndControl.Control;
        }
    }

    public class LocationNotSetException: ApplicationException
    {
        public LocationNotSetException(string message ="The location is null.") : base(message) { }
    }
}
