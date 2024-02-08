using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class EllipseMesh
	{
		private int _steps;
		private float _width;
		private float _height;
		private float _borderSize;
		private Color _color;
		private bool _isDirty;

		public Vertex[] Vertices { get; private set; }
		public ushort[] Indices { get; private set; }
		public int Steps
		{
			get => _steps;
			set
			{
				_isDirty = _steps != value;
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
		public float BorderSize
		{
			get => _borderSize;
			set => CompareAndWrite(ref _borderSize, value);
		}
		public Color Color
		{
			get => _color;
			set
			{
				_isDirty = _color != value;
				_color = value;
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

			int verticesCount = _steps * 2;
			int indicesCount = _steps * 6;

			if (Vertices == null || Vertices.Length != verticesCount)
				Vertices = new Vertex[verticesCount];

			if (Indices == null || Indices.Length != indicesCount)
				Indices = new ushort[indicesCount];

			float stepAngle = 360f / _steps;
			float currentAngle = 0f;

			for (int i = 0; i < _steps; i++)
			{
				currentAngle -= stepAngle;
				float radians = currentAngle * Mathf.Deg2Rad;

				float outerX = Mathf.Cos(radians) * _width;
				float outerY = Mathf.Sin(radians) * _width;
				float innerX = Mathf.Cos(radians) * (_width - _borderSize);
				float innerY = Mathf.Sin(radians) * (_width - _borderSize);

				Vertices[i * 2] = new Vertex
				{
					position = new Vector3(outerX, outerY, Vertex.nearZ),
					tint = _color,
				};

				Vertices[i * 2 + 1] = new Vertex
				{
					position = new Vector3(innerX, innerY, Vertex.nearZ),
					tint = _color,
				};

				Indices[i * 6] = (ushort)(i == 0 ? Vertices.Length - 2 : i * 2 - 2);
				Indices[i * 6 + 1] = (ushort)(i * 2);
				Indices[i * 6 + 2] = (ushort)(i == 0 ? Vertices.Length - 1 : i * 2 - 1);

				Indices[i * 6 + 3] = (ushort)(i == 0 ? Vertices.Length - 1 : i * 2 - 1);
				Indices[i * 6 + 4] = (ushort)(i * 2);
				Indices[i * 6 + 5] = (ushort)(i * 2 + 1);
			}

			_isDirty = false;
		}

		private void CompareAndWrite(ref float field, float newValue)
		{
			if (!(Mathf.Abs(field - newValue) > float.Epsilon))
				return;

			_isDirty = true;
			field = newValue;
		}
	}
}