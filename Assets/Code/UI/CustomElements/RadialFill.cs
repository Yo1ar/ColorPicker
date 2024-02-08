using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class RadialFill : VisualElement
	{
		private float _progress;
		private int _steps = 50;
		private readonly EllipseMesh _trackMesh;
		private readonly EllipseMesh _fillMesh;
		private Color _trackedColor;
		private Color _fillColor;
		private float _minValue;
		private float _maxValue;

		public float Progress
		{
			get => _progress;
			set => _progress = Mathf.Clamp(value, _minValue, _maxValue);
		}

		public RadialFill()
		{
			_trackMesh = new EllipseMesh(_steps);
			_fillMesh = new EllipseMesh(_steps);

			generateVisualContent += OnGenerateContent;
			_progress = 0;
		}

		private void OnGenerateContent(MeshGenerationContext genContext)
		{
			if (genContext.visualElement is not RadialFill radialFill)
				return;
			radialFill.DrawMeshes(genContext);
		}

		private void DrawMeshes(MeshGenerationContext context)
		{
			float halfWidth = contentRect.width * 0.5f;
			float halfHeight = contentRect.height * 0.5f;

			if (halfWidth < 2f || halfHeight < 2f)
				return;

			_trackMesh.Width = halfWidth;
			_trackMesh.Height = halfHeight;
			_trackMesh.BorderSize = 10f;
			_trackMesh.UpdateMesh();

			_fillMesh.Width = halfWidth;
			_fillMesh.Height = halfHeight;
			_fillMesh.BorderSize = 10f;
			_fillMesh.UpdateMesh();

			MeshWriteData trackMeshData = context.Allocate(_trackMesh.Vertices.Length, _trackMesh.Indices.Length);
			trackMeshData.SetAllVertices(_trackMesh.Vertices);
			trackMeshData.SetAllIndices(_trackMesh.Indices);

			int sliceSize = Mathf.FloorToInt(_steps * _progress / 100f);
			if (sliceSize == 0)
				return;
			sliceSize *= 6;

			MeshWriteData fillMeshData = context.Allocate(_fillMesh.Vertices.Length, sliceSize);
			fillMeshData.SetAllVertices(_fillMesh.Vertices);

			using var tempIndicesArray = new NativeArray<ushort>(_fillMesh.Indices, Allocator.Temp);
			fillMeshData.SetAllIndices(tempIndicesArray.Slice(0, sliceSize));
		}

		#region UXML

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			private readonly UxmlFloatAttributeDescription _minValueAttribute = new()
				{name = "Min value"};

			private readonly UxmlFloatAttributeDescription _maxValueAttribute = new()
				{name = "Max value"};

			private readonly UxmlFloatAttributeDescription _progressAttribute = new()
				{name = "Progress"};

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				Debug.Log("Init");

				base.Init(ve, bag, cc);

				if (ve is not RadialFill radialProgress)
					return;

				_progressAttribute.restriction = new UxmlValueBounds
				{
					min = $"{_minValueAttribute.GetValueFromBag(bag, cc)}",
					max = $"{_maxValueAttribute.GetValueFromBag(bag, cc)}",
				};

				radialProgress.Progress = _progressAttribute.GetValueFromBag(bag, cc);
			}
		}

		public new class UxmlFactory : UxmlFactory<RadialFill, UxmlTraits> { }

		#endregion
	}
}