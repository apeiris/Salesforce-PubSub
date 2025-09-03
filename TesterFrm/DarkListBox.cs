using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;

public class DarkListBox : ListBox {
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BgColor { get; set; } = Color.FromArgb(32, 34, 37);
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color FgColor { get; set; } = Color.FromArgb(0, 255, 0);
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelBgColor { get; set; } = Color.FromArgb(55, 60, 67);
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelFgColor { get; set; } = Color.White;

	public DarkListBox() {
		DrawMode = DrawMode.OwnerDrawFixed;
		ItemHeight = 20;
		BorderStyle = BorderStyle.None;
		BackColor = BgColor;
		ForeColor = FgColor;
		IntegralHeight = false;
		// Reduce flicker:
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		UpdateStyles();
	}
	private readonly Dictionary<string, Color> LogColors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase) {
   { "CREATE", Color.LightGreen },
	{ "UPDATE", Color.LightBlue },
	{ "DELETE", Color.IndianRed },
	{ "Error",  Color.Red },
	{ "Warning", Color.Yellow },
	{ "Info", Color.White },
	{"Debug", Color.BlueViolet } };
	protected override void OnDrawItem(DrawItemEventArgs e) {
		if (e.Index < 0) return;
		bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
		var bg = selected ? SelBgColor : BgColor;
		var fg = selected ? SelFgColor : FgColor;
		string text = GetItemText(Items[e.Index]);
		var tag = Regex.Match(text, "<%(.*?)%>");// Extract tag and override color
		if (tag.Success) {
			string key = tag.Groups[1].Value;
			if (LogColors.TryGetValue(key, out var mappedColor))

				fg = mappedColor;
			text = Regex.Replace(text, @"<%.*?%>\s*", "");
		}

		var rect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 2, e.Bounds.Width - 12, e.Bounds.Height - 4);

		using (var bgBrush = new SolidBrush(bg))
		using (var fgBrush = new SolidBrush(fg))
		using (Font boldf = new Font(e.Font??DefaultFont, newStyle: FontStyle.Bold)) {
			e.Graphics.FillRectangle(bgBrush, e.Bounds);
			e.Graphics.DrawString(text, boldf, fgBrush, rect);
		}

		e.DrawFocusRectangle();
	}

};

