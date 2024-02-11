using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class RadialFill : VisualElement
	{
		private float _progress;
		public float Progress
		{
			get => _progress;
			set
			{
				_progress = value;
				MarkDirtyRepaint();
			}
		}

		public RadialFill()
		{
			generateVisualContent += OnGenerateContent;
		}

		private void OnGenerateContent(MeshGenerationContext context)
		{
			int segments = 200;
			float border = 20f;
			Color color = Color.blue;

			float width = contentRect.width;
			float height = contentRect.height;

			var vertices = new Vertex[segments * 2];
			ushort[] indices = new ushort[segments * 6];

			float angle = 360 / segments;
			float currentAngle = 0f;

			for (int i = 0; i < segments; i++)
			{
				currentAngle -= angle;
				float radians = angle * Mathf.Deg2Rad;

				float outerX = Mathf.Cos(radians) * width;
				float outerY = Mathf.Sin(radians) * height;
				float innerX = Mathf.Cos(radians) * (width - border);
				float innerY = Mathf.Sin(radians) * (height - border);

				vertices[i * 2] = new Vertex
				{
					position = new Vector3(outerX, outerY),
					tint = color,
				};
				vertices[i * 2 + 1] = new Vertex
				{
					position = new Vector3(innerX, innerY),
					tint = color,
				};

			}

			MeshWriteData meshData = context.Allocate(vertices.Length, indices.Length);
			meshData.SetAllVertices(vertices);
			meshData.SetAllIndices(indices);

			// radialFill.GenerateArcs(context);
			// radialFill.DrawMeshes(context);
		}

		// private void DrawMeshes(MeshGenerationContext context)
		// {
		// 	Debug.Log("Drawing mesh");
		//
		// 	float halfWidth = contentRect.width * 0.5f;
		// 	float halfHeight = contentRect.height * 0.5f;
		//
		// 	if (halfWidth < 2f || halfHeight < 2f)
		// 		return;
		//
		// 	_trackMesh.Width = halfWidth;
		// 	_trackMesh.Height = halfHeight;
		// 	_trackMesh.BorderSize = 10f;
		// 	_trackMesh.UpdateMesh();
		//
		// 	_fillMesh.Width = halfWidth;
		// 	_fillMesh.Height = halfHeight;
		// 	_fillMesh.BorderSize = 10f;
		// 	_fillMesh.UpdateMesh();
		//
		// 	MeshWriteData trackMeshData = context.Allocate(_trackMesh.Vertices.Length, _trackMesh.Indices.Length);
		// 	trackMeshData.SetAllVertices(_trackMesh.Vertices);
		// 	trackMeshData.SetAllIndices(_trackMesh.Indices);
		//
		// 	int sliceSize = Mathf.FloorToInt(_steps * _progress / 100f);
		// 	if (sliceSize == 0)
		// 		return;
		// 	sliceSize *= 6;
		//
		// 	MeshWriteData fillMeshData = context.Allocate(_fillMesh.Vertices.Length, sliceSize);
		// 	fillMeshData.SetAllVertices(_fillMesh.Vertices);
		//
		// 	using var tempIndicesArray = new NativeArray<ushort>(_fillMesh.Indices, Allocator.Temp);
		// 	fillMeshData.SetAllIndices(tempIndicesArray.Slice(0, sliceSize));
		// }
		//
		// private void GenerateArcs(MeshGenerationContext context)
		// {
		// 	float halfWidth = contentRect.width * 0.5f;
		// 	float halfHeight = contentRect.height * 0.5f;
		//
		// 	Painter2D painter = context.painter2D;
		// 	var center = new Vector2(halfWidth, halfHeight);
		// 	float radius = halfWidth > halfHeight ? halfHeight : halfWidth;
		//
		// 	painter.lineWidth = 10;
		// 	painter.fillColor = Color.green;
		// 	painter.lineCap = LineCap.Butt;
		//
		// 	painter.BeginPath();
		// 	painter.Arc(center, radius, 0, 360);
		// 	painter.Stroke();
		//
		// 	painter.BeginPath();
		// 	painter.Arc(center, radius, -90f, 360f * (_progress / 100) - 90f);
		// 	painter.Stroke();
		// }

#region UXML

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			private readonly UxmlFloatAttributeDescription _progressAttribute = new()
				{name = "Progress"};

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				if (ve is not RadialFill radialFill)
					return;

				Debug.Log("Init");

				radialFill.Progress = _progressAttribute.GetValueFromBag(bag, cc);
			}
		}

		public new class UxmlFactory : UxmlFactory<RadialFill, UxmlTraits> { }

		#endregion
	}
}