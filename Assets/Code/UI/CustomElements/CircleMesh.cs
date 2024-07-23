using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class CircleMesh
	{
		private bool _isDirty;
		private Color _tintColor;
		private FillStart _fillStart;
		private float _radius;
		private int _steps;
		private Vector2 _center;

		public Vertex[] Vertices { get; private set; }
		public ushort[] Indices { get; private set; }
		public float Radius
		{
			get => _radius;
			set
			{
				if (!IsFloatEquals(_radius, value))
					SetDirty();
				_radius = value;
			}
		}

		public int Steps
		{
			get => _steps;
			set
			{
				if (_steps != value)
					SetDirty();
				_steps = value;
			}
		}
		public Color TintColor
		{
			get => _tintColor;
			set
			{
				if (_tintColor != value)
					SetDirty();
				_tintColor = value;
			}
		}
		public FillStart FillStart
		{
			get => _fillStart;
			set
			{
				if (_fillStart != value)
					SetDirty();

				_fillStart = value;
			}
		}
		public Vector2 Center
		{
			get => _center;
			set
			{
				if (_center != value)
					SetDirty();
				_center = value;
			}
		}

		public CircleMesh(int steps)
		{
			_steps = steps;
			_isDirty = true;
		}

		public void UpdateMesh()
		{
			if (!_isDirty)
				return;

			int verticesCount = Steps + 1; // +1 for center vertex
			int indicesCount = Steps * 3; // 3 indices per triangle

			if (ShouldRecalculateVertices(verticesCount))
				Vertices = new Vertex[verticesCount];

			if (ShouldRecalculateIndices(indicesCount))
				Indices = new ushort[indicesCount];

			float angle = (float)360 / Steps; // angle of each triangle in degrees

			float startingAngle = FillStart.GetAngle();

			for (var i = 0; i < Steps; i++)
			{
				startingAngle -= angle;
				float radians = startingAngle.ToRadians();

				float outerX = Mathf.Cos(radians) * _radius + Center.x;
				float outerY = Mathf.Sin(radians) * _radius + Center.y;

				if (i == 0)
					AddVertex(i, Center.x, Center.y);

				AddVertex(i, outerX, outerY);

				Indices[i * 3] = 0; // for center vertex
				Indices[i * 3 + 1] = (ushort)(i + 1); // for edge vertex
				Indices[i * 3 + 2] = (ushort)(i == 0 ? Vertices.Length - 1 : i); // for previous edge vertex
			}

			_isDirty = false;
		}

		private void AddVertex(int i, float outerX, float outerY) =>
			Vertices[i + 1] = new Vertex
			{
				position = new Vector3(outerX, outerY, Vertex.nearZ),
				tint = TintColor,
			};

		private bool ShouldRecalculateIndices(int indicesCount) =>
			Indices is null || Indices.Length != indicesCount;

		private bool ShouldRecalculateVertices(int verticesCount) =>
			Vertices is null || Vertices.Length != verticesCount;

		private static bool IsFloatEquals(float value1, float value2) =>
			Mathf.Abs(value1 - value2) < float.Epsilon;

		private void SetDirty()
		{
			if (_isDirty)
				return;

			_isDirty = true;
		}
	}
}