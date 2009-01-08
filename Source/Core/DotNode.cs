namespace Pencil.Core
{
	using System.Drawing;
	using System.Text;

	public class DotNode : Node
	{
		readonly int id;
		Color fillColor = Color.White;
		Color borderColor = Color.Black;

		public DotNode(int id)
		{
			this.id = id;
		}

		public override string Id { get { return id.ToString(); } }

		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}
		
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; }
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("{0}[label=\"{1}\"", Id, Label);
			if(FillColor != Color.White)
				builder.AppendFormat(" style=filled fillcolor=\"#{0:X2}{1:X2}{2:X2}\"",
					FillColor.R, FillColor.G, FillColor.B);
			if(BorderColor != Color.Black)
				builder.AppendFormat(" color=\"#{0:X2}{1:X2}{2:X2}\"",
					BorderColor.R, BorderColor.G, BorderColor.B);  
			builder.Append(']');
			return builder.ToString();
		}
	}
}
