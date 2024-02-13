using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class CircleFill : VisualElement
	{
		private readonly DonutMesh _trackMesh;
		private readonly DonutMesh _fillMesh;
		private float _progress = 75f;
		private int _steps = 200;
		private int _circleBorder = 50;
		private Color _trackColor;
		private Color _fillColor = Color.green;
		private FillStart _fillStart = FillStart.Up;
		private FillDirection _fillDirection = FillDirection.Clockwise;
		private Rect _contentRect;
		private bool _isPerfectCircle;

		private FillDirection FillDirection
		{
			get => _fillDirection;
			set
			{
				_fillDirection = value;
				MarkDirtyRepaint();
			}
		}
		private FillStart FillStart
		{
			get => _fillStart;
			set
			{
				_fillStart = value;
				MarkDirtyRepaint();
			}
		}
		private int Steps
		{
			get => _steps;
			set
			{
				_steps = Mathf.Clamp(value, 200, 5000);
				MarkDirtyRepaint();
			}
		}
		private int CircleBorder
		{
			get => _circleBorder;
			set
			{
				_circleBorder = Mathf.FloorToInt(
					Mathf.Clamp(value, 0,
						contentRect.width < contentRect.height
							? contentRect.width / 2
							: contentRect.height / 2));
				MarkDirtyRepaint();
			}
		}
		private float Progress
		{
			get => _progress;
			set
			{
				_progress = Mathf.Clamp(value, 0f, 100f);
				MarkDirtyRepaint();
			}
		}
		private Color TrackColor
		{
			get => _trackColor;
			set
			{
				_trackColor = value;
				MarkDirtyRepaint();
			}
		}
		private Color FillColor
		{
			get => _fillColor;
			set
			{
				_fillColor = value;
				MarkDirtyRepaint();
			}
		}
		private bool IsPerfectCircle
		{
			get => _isPerfectCircle;
			set
			{
				if (_isPerfectCircle == value)
					return;
				_isPerfectCircle = value;
				MarkDirtyRepaint();
			}
		}

		public CircleFill()
		{
			style.flexGrow = 1;
			_trackMesh = new DonutMesh(_steps);
			_fillMesh = new DonutMesh(_steps);
			generateVisualContent += OnGenerateContent;
		}

		private void OnGenerateContent(MeshGenerationContext context)
		{
			_contentRect = contentRect;
			InitMeshValues();
			SetupTrackMesh(context);
			SetupFillMesh(context);
		}

		private void SetupTrackMesh(MeshGenerationContext context)
		{
			MeshWriteData trackMeshData = context.Allocate(_trackMesh.Vertices.Length, _trackMesh.Indices.Length);
			trackMeshData.SetAllVertices(_trackMesh.Vertices);
			trackMeshData.SetAllIndices(_trackMesh.Indices);
		}

		private void SetupFillMesh(MeshGenerationContext context)
		{
			int sliceSize = Mathf.FloorToInt(Progress * _fillMesh.Indices.Length / 100);
			if (sliceSize <= 3)
				return;

			int sliceStart;

			var nativeArray = new NativeArray<ushort>(_fillMesh.Indices, Allocator.Temp);
			if (FillDirection == FillDirection.Clockwise)
			{
				sliceSize = FindClosestMultipleBy3(sliceSize);
				sliceStart = nativeArray.Length - sliceSize;
			}
			else
				sliceStart = 0;

			NativeSlice<ushort> slice = nativeArray.Slice(sliceStart, sliceSize);

			MeshWriteData fillMeshData = context.Allocate(_fillMesh.Vertices.Length, sliceSize);
			fillMeshData.SetAllVertices(_fillMesh.Vertices);
			fillMeshData.SetAllIndices(slice);
		}

		private static int FindClosestMultipleBy3(int sliceSize)
		{
			if (sliceSize % 3 == 0)
				return sliceSize;

			int temp1 = sliceSize;
			int temp2 = sliceSize;

			for (var i = 1; i <= 2; i++)
			{
				temp1 += 1;
				if (temp1 % 3 == 0)
					return temp1;

				temp2 -= 1;
				if (temp2 % 3 == 0)
					return temp2;
			}

			return sliceSize;
		}

		private void InitMeshValues()
		{
			_trackMesh.IsPerfectCircle = IsPerfectCircle;
			_trackMesh.Steps = Steps;
			_trackMesh.Width = _contentRect.width;
			_trackMesh.Height = _contentRect.height;
			_trackMesh.Color = TrackColor;
			_trackMesh.BorderSize = CircleBorder;
			_trackMesh.UpdateMesh();

			_fillMesh.IsPerfectCircle = IsPerfectCircle;
			_fillMesh.Steps = Steps;
			_fillMesh.Width = contentRect.width;
			_fillMesh.Height = contentRect.height;
			_fillMesh.Color = FillColor;
			_fillMesh.BorderSize = CircleBorder;
			_fillMesh.FillStart = FillStart;
			_fillMesh.UpdateMesh();
		}

		#region UXML

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			private readonly UxmlBoolAttributeDescription _isPerfectCircleAttribute = new()
				{ name = "is-perfect-circle", defaultValue = true };

			private readonly UxmlEnumAttributeDescription<FillDirection> _fillDirectionAttribute = new()
				{ name = "fill-direction", defaultValue = FillDirection.Clockwise };

			private readonly UxmlEnumAttributeDescription<FillStart> _fillStartAttribute = new()
				{ name = "fill-start", defaultValue = FillStart.Up };

			private readonly UxmlIntAttributeDescription _stepsAttribute = new()
				{ name = "steps", defaultValue = 200 };

			private readonly UxmlIntAttributeDescription _circleBorderAttribute = new()
				{ name = "circle-border", defaultValue = 50 };

			private readonly UxmlColorAttributeDescription _trackColorAttribute = new()
				{ name = "track-color", defaultValue = new Color(0f, 0f, 0f, 0.5f) };

			private readonly UxmlColorAttributeDescription _fillColorAttribute = new()
				{ name = "fill-color", defaultValue = new Color(0.3f, 0.8f, 0.3f, 1f) };

			private readonly UxmlFloatAttributeDescription _progressAttribute = new()
				{ name = "progress", defaultValue = 82f };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				if (ve is not CircleFill circleFill)
					return;

				circleFill.IsPerfectCircle = _isPerfectCircleAttribute.GetValueFromBag(bag, cc);
				circleFill.FillDirection = _fillDirectionAttribute.GetValueFromBag(bag, cc);
				circleFill.FillStart = _fillStartAttribute.GetValueFromBag(bag, cc);
				circleFill.Steps = _stepsAttribute.GetValueFromBag(bag, cc);
				circleFill.CircleBorder = _circleBorderAttribute.GetValueFromBag(bag, cc);
				circleFill.TrackColor = _trackColorAttribute.GetValueFromBag(bag, cc);
				circleFill.FillColor = _fillColorAttribute.GetValueFromBag(bag, cc);
				circleFill.Progress = _progressAttribute.GetValueFromBag(bag, cc);
			}
		}

		public new class UxmlFactory : UxmlFactory<CircleFill, UxmlTraits> { }

		#endregion
	}
}