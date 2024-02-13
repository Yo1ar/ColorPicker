using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class DonutMesh
	{
		private int _borderSize;
		private Color _color;
		private float _height;
		private bool _isDirty;
		private int _steps;
		private float _width;
		private FillStart _fillStart;
		private bool _isPerfectCircle;

		public Vertex[] Vertices { get; private set; }
		public ushort[] Indices { get; private set; }
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
		public float Width
		{
			get => _width;
			set => CompareAndWrite(ref _width, value);
		}
		public float Height
		{
			get => _height;
			set => CompareAndWrite(ref _height, value);
		}
		public int BorderSize
		{
			get => _borderSize;
			set
			{
				if (_borderSize != value)
					SetDirty();
				_borderSize = value;
			}
		}
		public Color Color
		{
			get => _color;
			set
			{
				if (_color != value)
					SetDirty();
				_color = value;
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
		public bool IsPerfectCircle
		{
			get => _isPerfectCircle;
			set
			{
				if (_isPerfectCircle != value)
					SetDirty();
				_isPerfectCircle = value;
			}
		}

		public DonutMesh(int steps)
		{
			_steps = steps;
			_isDirty = true;
		}

		public void UpdateMesh()
		{
			if (!_isDirty)
				return;

			int verticesCount = Steps * 2;
			int indicesCount = Steps * 6;

			if (Vertices is null || Vertices.Length != verticesCount)
				Vertices = new Vertex[verticesCount];

			if (Indices is null || Indices.Length != indicesCount)
				Indices = new ushort[indicesCount];

			float halfWidth = Width / 2;
			float halfHeight = Height / 2;

			var center = new Vector2(halfWidth, halfHeight);

			if (IsPerfectCircle)
			{
				float radius = halfHeight < halfWidth ? halfHeight : halfWidth;
				halfWidth = radius;
				halfHeight = radius;
			}

			float angle = (float)360 / Steps;

			float currentAngle = FillStart switch
			{
				FillStart.Up => -90f,
				FillStart.Down => -270f,
				FillStart.Left => -180f,
				FillStart.Right => 0,
				_ => throw new ArgumentOutOfRangeException(),
			};

			for (var i = 0; i < Steps; i++)
			{
				currentAngle -= angle;
				float radians = currentAngle * Mathf.Deg2Rad;

				float outerX = Mathf.Cos(radians) * halfWidth + center.x;
				float outerY = Mathf.Sin(radians) * halfHeight + center.y;
				float innerX = Mathf.Cos(radians) * (halfWidth - BorderSize) + center.x;
				float innerY = Mathf.Sin(radians) * (halfHeight - BorderSize) + center.y;

				Vertices[i * 2] = new Vertex
				{
					position = new Vector3(outerX, outerY, Vertex.nearZ),
					tint = Color,
				};
				Vertices[i * 2 + 1] = new Vertex
				{
					position = new Vector3(innerX, innerY, Vertex.nearZ),
					tint = Color,
				};

				Indices[i * 6] = (ushort)(i == 0 ? Vertices.Length - 2 : i * 2 - 2);
				Indices[i * 6 + 1] = (ushort)(i == 0 ? Vertices.Length - 1 : i * 2 - 1);
				Indices[i * 6 + 2] = (ushort)(i * 2);

				Indices[i * 6 + 3] = (ushort)(i == 0 ? Vertices.Length - 1 : i * 2 - 1);
				Indices[i * 6 + 4] = (ushort)(i * 2 + 1);
				Indices[i * 6 + 5] = (ushort)(i * 2);
			}

			_isDirty = false;
		}

		private void CompareAndWrite(ref float field, float newValue)
		{
			if (!(Mathf.Abs(field - newValue) > float.Epsilon))
				return;

			SetDirty();
			field = newValue;
		}

		private void SetDirty()
		{
			if (_isDirty)
				return;

			_isDirty = true;
		}
	}
}