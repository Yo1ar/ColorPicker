using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class TextureFill : VisualElement
	{
		private readonly Texture2D _texture;
		private readonly EllipseMesh _ellipseMesh;
		private int _steps = 200;
		private Color _tintColor = Color.white;
		private FillStart _fillStart = FillStart.Up;
		private FillDirection _fillDirection = FillDirection.Clockwise;
		private float _progress;

		public float Progress
		{
			get => _progress;
			set
			{
				_progress = Mathf.Clamp(value, 0f, 100f);
				MarkDirtyRepaint();
			}
		}
		public int Steps
		{
			get => _steps;
			set
			{
				if (_steps == value)
					return;

				_steps = Mathf.Clamp(value, 4, 1000);
				MarkDirtyRepaint();
			}
		}
		public Color TintColor
		{
			get => _tintColor;
			set
			{
				if (_tintColor == value)
					return;
				_tintColor = value;
				MarkDirtyRepaint();
			}
		}
		public FillStart FillStart
		{
			get => _fillStart;
			set
			{
				if (_fillStart == value)
					return;
				_fillStart = value;
				MarkDirtyRepaint();
			}
		}
		public FillDirection FillDirection
		{
			get => _fillDirection;
			set
			{
				if (_fillDirection == value)
					return;
				_fillDirection = value;
				MarkDirtyRepaint();
			}
		}
		public float TextureScale { get; set; }

		public TextureFill()
		{
			_texture = Resources.Load<Texture2D>("spr_leaf");
			_ellipseMesh = new EllipseMesh(_steps);
			style.flexGrow = 1;

			generateVisualContent += OnGenerateVisualContent;
		}

		private void OnGenerateVisualContent(MeshGenerationContext context)
		{
			SetupMesh();

			NativeSlice<ushort> indexSlice = CalculateSlice();

			if (indexSlice.Length < 3)
				return;

			MeshWriteData meshData = context.Allocate(_ellipseMesh.Vertices.Length, indexSlice.Length, _texture);

			SetupUvCoords(meshData);

			meshData.SetAllVertices(_ellipseMesh.Vertices);
			meshData.SetAllIndices(indexSlice);
		}

		private void SetupMesh()
		{
			_ellipseMesh.FillStart = _fillStart;
			_ellipseMesh.Steps = Steps;
			_ellipseMesh.TintColor = _tintColor;
			_ellipseMesh.Center = contentRect.center;
			_ellipseMesh.Radius = GetMeshRadius();
			_ellipseMesh.UpdateMesh();
		}

		private NativeSlice<ushort> CalculateSlice()
		{
			int sliceSize = Mathf.FloorToInt(Progress * _ellipseMesh.Indices.Length / 100);

			if (sliceSize < 3)
				sliceSize = 3;

			var indexArray = new NativeArray<ushort>(_ellipseMesh.Indices, Allocator.Temp);

			int sliceStart;
			if (FillDirection == FillDirection.Clockwise)
			{
				sliceSize = FindClosestMultipleBy3(sliceSize);
				sliceStart = indexArray.Length - sliceSize;
			}
			else
				sliceStart = 0;

			return indexArray.Slice(sliceStart, sliceSize);
		}

		private void SetupUvCoords(MeshWriteData meshData)
		{
			float diameter = _ellipseMesh.Radius * 2;
			Vector2 uvTextureScaledSize =
				new Vector2(_texture.width, _texture.height * HeightOffset()) / TextureOffset() * TextureScale;

			float HeightOffset()
			{
				if (contentRect.height < _texture.height * 2)
					Debug.Log($"less");
				else
					Debug.Log($"more");
				return 1;
			}

			float TextureOffset()
			{
				float some;
				if (contentRect.width < contentRect.height)
					some = contentRect.height / contentRect.width;
				else
					some = 1;
				return some;
			}

			for (var i = 0; i < _ellipseMesh.Vertices.Length; i++)
			{
				Vector2 uvRelative =
					((Vector2)_ellipseMesh.Vertices[i].position - _ellipseMesh.Center + uvTextureScaledSize) / diameter;

				_ellipseMesh.Vertices[i].uv =
					uvRelative * meshData.uvRegion.size / TextureScale + meshData.uvRegion.min;
			}
		}

		private int FindClosestMultipleBy3(int sliceSize)
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

		private float GetMeshRadius() =>
			contentRect.width < contentRect.height ? contentRect.width / 2 : contentRect.height / 2;

		#region UXML

		public class TextureFillTraits : UxmlTraits
		{
			private readonly UxmlEnumAttributeDescription<FillDirection> _fillDirectionAttribute = new()
				{ name = "fill-direction", defaultValue = FillDirection.Clockwise };

			private readonly UxmlEnumAttributeDescription<FillStart> _fillStartAttribute = new()
				{ name = "fill-start", defaultValue = FillStart.Up };

			private readonly UxmlIntAttributeDescription _stepsAttribute = new()
				{ name = "steps", defaultValue = 200 };

			private readonly UxmlFloatAttributeDescription _textureScaleAttribute = new()
				{ name = "texture-scale", defaultValue = 1f };

			private readonly UxmlColorAttributeDescription _tintColorAttribute = new()
				{ name = "tint-color", defaultValue = Color.white };

			private readonly UxmlFloatAttributeDescription _progressAttribute = new()
				{ name = "progress", defaultValue = 82f };


			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				if (ve is not TextureFill textureFill)
					return;
				textureFill.FillDirection = _fillDirectionAttribute.GetValueFromBag(bag, cc);
				textureFill.FillStart = _fillStartAttribute.GetValueFromBag(bag, cc);
				textureFill.Steps = _stepsAttribute.GetValueFromBag(bag, cc);
				textureFill.TextureScale = _textureScaleAttribute.GetValueFromBag(bag, cc);
				textureFill.TintColor = _tintColorAttribute.GetValueFromBag(bag, cc);
				textureFill.Progress = _progressAttribute.GetValueFromBag(bag, cc);
			}
		}

		public class TextureFillFactory : UxmlFactory<TextureFill, TextureFillTraits> { }

		#endregion
	}
}