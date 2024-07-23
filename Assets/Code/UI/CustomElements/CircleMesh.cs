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
		private float StepAngle => 360f / Steps;

		private int VerticesCount => Steps + 1;
		private int IndicesCount => Steps * 3;
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
		public ushort[] Indices { get; private set; }
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
		public Vertex[] Vertices { get; private set; }

		public CircleMesh(int steps)
		{
			_steps = steps;
			_isDirty = true;
		}

		public void UpdateMesh()
		{
			if (!_isDirty)
				return;

			float startingAngle = FillStart.GetAngle();

			if (ShouldRepopulateVertices(VerticesCount))
				Vertices = new Vertex[VerticesCount];

			if (ShouldRepopulateIndices(IndicesCount))
				Indices = new ushort[IndicesCount];

			for (var i = 0; i < Steps; i++)
			{
				startingAngle -= StepAngle;

				AddVertices(startingAngle.ToRadians(), i);
				AddIndices(i);
			}

			_isDirty = false;
		}

		private bool ShouldRepopulateVertices(int verticesCount) =>
			Vertices is null || Vertices.Length != verticesCount;

		private bool ShouldRepopulateIndices(int indicesCount) =>
			Indices is null || Indices.Length != indicesCount;

		private void AddVertices(float radians, int i)
		{
			float outerX = Mathf.Cos(radians) * _radius + Center.x;
			float outerY = Mathf.Sin(radians) * _radius + Center.y;

			if (i == 0)
				AddVertex(i, Center.x, Center.y);

			AddVertex(i + 1, outerX, outerY);
		}

		private void AddVertex(int index, float x, float y) =>
			Vertices[index] = new Vertex
			{
				position = new Vector3(x, y, Vertex.nearZ),
				tint = TintColor,
			};

		private void AddIndices(int i)
		{
			Indices[i * 3] = 0;
			Indices[i * 3 + 1] = (ushort)(i + 1);
			Indices[i * 3 + 2] = (ushort)(i == 0 ? Vertices.Length - 1 : i);
		}

		private void SetDirty()
		{
			if (_isDirty)
				return;

			_isDirty = true;
		}

		private static bool IsFloatEquals(float value1, float value2) =>
			Mathf.Abs(value1 - value2) < float.Epsilon;
	}
}