using MinimalisticUIFramework;

// IMPORTANT NOTE: You should NOT change this file as part of your solution! Put you implementation into MinimalisticUIFramework project only.

Console.WriteLine("--- PART 1 (panels support) ---");
Console.WriteLine();

var image1a = new Image().WithUrl("flower1.jpg");
var image1b = new Image().WithUrl("flower1.jpg").WithZoomFactor(0.25f);
var label1 = new Label().WithText("Daisy (lat. Bellis perennis)");

var stackPanel1 = new StackPanel();
stackPanel1.AddChild(image1a);
stackPanel1.AddChild(image1b);
stackPanel1.AddChild(label1);

Console.WriteLine(stackPanel1); Console.WriteLine();

var image2a = new Image().WithUrl("flower2.jpg");
var image2b = new Image().WithUrl("flower2.jpg").WithZoomFactor(1.5f);
var label2 = new Label().WithText("Dandelion (lat. Taraxacum officinale)");

var canvas1 = new Canvas();
canvas1.AddChild(image2a, new Point { X = 10, Y = 10 });
canvas1.AddChild(image2b, new Point { X = 10, Y = 205 });
canvas1.AddChild(label2, new Point { X = 50, Y = 200 });
canvas1.AddChild(stackPanel1, new Point { X = 500, Y = 0 });

Console.WriteLine(canvas1); Console.WriteLine();

Console.WriteLine("--- PART 2 (fluent syntax) ---");
Console.WriteLine();

var stackPanel2 = new StackPanel();
new Image()
	.PlacedIn(stackPanel2)
	.WithUrl("flower1.jpg");
new Image()
	.PlacedIn(stackPanel2)
	.WithUrl("flower1.jpg").WithZoomFactor(0.25f);
new Label()
	.WithText("Daisy (lat. Bellis perennis)")
	.PlacedIn(stackPanel2);
new Image()
	.WithZoomFactor(0.75f)
	.PlacedIn(stackPanel2)
	.WithUrl("flower1.jpg");
Console.WriteLine(stackPanel2); Console.WriteLine();

var canvas2 = new Canvas();
new Image()
	.PlacedIn(canvas2).At(10, 10)
	.WithUrl("flower2.jpg");
new Image()
	.PlacedIn(canvas2).At(10, 205)
	.WithUrl("flower2.jpg").WithZoomFactor(1.5f);
new Label()
	.WithText("Dandelion (lat. Taraxacum officinale)")
	.PlacedIn(canvas2).At(50, 200);
new Image()
	.WithZoomFactor(0.75f)
	.PlacedIn(canvas2).At(0, 0)
	.WithUrl("flower2.jpg");
stackPanel2
	.PlacedIn(canvas2).At(500, 0);
Console.WriteLine(canvas2); Console.WriteLine();

// Should NOT compile:
// new Image().PlacedIn(canvas2).WithUrl("flower2.jpg");
// Can compile, but should NOT add the Label into Canvas:
// new Label().WithText("Dandelion").PlacedIn(canvas2);
