using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class EllipseMesh
	{
		private Color _tintColor;
		private float _radius;
		private bool _isDirty;
		private int _steps;
		private FillStart _fillStart;
		private Vector2 _center;

		public Vertex[] Vertices { get; private set; }
		public ushort[] Indices { get; private set; }
		public float Radius
		{
			get => _radius;
			set
			{
				if (Mathf.Abs(_radius - value) > float.Epsilon)
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

		public EllipseMesh(int steps)
		{
			_steps = steps;
			_isDirty = true;
		}

		public void UpdateMesh()
		{
			if (!_isDirty)
				return;

			int verticesCount = Steps + 1;
			int indicesCount = Steps * 3;

			if (Vertices is null || Vertices.Length != verticesCount)
				Vertices = new Vertex[verticesCount];

			if (Indices is null || Indices.Length != indicesCount)
				Indices = new ushort[indicesCount];


			float angle = (float)360 / Steps;

			float currentAngle = FillStart switch
			{
				FillStart.Up => -90f,
				FillStart.Left => -180f,
				FillStart.Down => -270f,
				FillStart.Right => 0,
				_ => throw new ArgumentOutOfRangeException(),
			};

			for (var i = 0; i < Steps; i++)
			{
				currentAngle -= angle;
				float radians = currentAngle * Mathf.Deg2Rad;

				float outerX = Mathf.Cos(radians) * _radius + Center.x;
				float outerY = Mathf.Sin(radians) * _radius + Center.y;

				if (i == 0)
					Vertices[i] = new Vertex()
					{
						position = Center,
						tint = TintColor,
					};

				Vertices[i + 1] = new Vertex
				{
					position = new Vector3(outerX, outerY, Vertex.nearZ),
					tint = TintColor,
				};

				Indices[i * 3] = 0;
				Indices[i * 3 + 1] = (ushort)(i + 1);
				Indices[i * 3 + 2] = (ushort)(i == 0 ? Vertices.Length - 1 : i);
			}

			_isDirty = false;
		}

		private void SetDirty()
		{
			if (_isDirty)
				return;

			_isDirty = true;
		}
	}
}