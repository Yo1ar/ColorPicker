using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class TextureFillElement : VisualElement
	{
		private readonly CircleMesh _circleMesh;
		private readonly FillTexture _fillTexture;
		private Color _tintColor = Color.white;
		private FillDirection _fillDirection = FillDirection.Clockwise;
		private FillStart _fillStart = FillStart.Up;
		private float _progress;
		private int _steps = 200;

		private int Steps
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
		private FillStart FillStart
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
		private FillDirection FillDirection
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
		public float Progress
		{
			get => _progress;
			set
			{
				_progress = Mathf.Clamp(value, 0f, 100f);
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
		public float TextureScale { get; set; }
		public Vector2 TextureOffset { get; set; }

		public TextureFillElement()
		{
			_circleMesh = new CircleMesh(_steps);
			_fillTexture = new FillTexture();

			style.flexGrow = 1;
			style.unityBackgroundImageTintColor = Color.clear;

			generateVisualContent += OnGenerateVisualContent;
		}

		private void OnGenerateVisualContent(MeshGenerationContext context)
		{
			SetupMesh();
			SetTexture();

			NativeSlice<ushort> indexSlice = CalculateSlice();

			if (indexSlice.Length < 3)
				return;

			MeshWriteData meshData =
				context.Allocate(_circleMesh.Vertices.Length, indexSlice.Length, _fillTexture.Texture);

			if (_fillTexture != null)
				SetupUvCoords(meshData);

			meshData.SetAllVertices(_circleMesh.Vertices);
			meshData.SetAllIndices(indexSlice);
		}

		private void SetupMesh()
		{
			_circleMesh.FillStart = _fillStart;
			_circleMesh.Steps = Steps;
			_circleMesh.TintColor = _tintColor;
			_circleMesh.Center = contentRect.center;
			_circleMesh.Radius = GetMeshRadius();
			_circleMesh.UpdateMesh();
		}

		private void SetTexture()
		{
			Background backgroundImage = contentContainer.resolvedStyle.backgroundImage;

			if (backgroundImage.sprite == null &&
			    backgroundImage.texture == null)
				return;

			Texture2D texture = backgroundImage.sprite
				? backgroundImage.sprite.texture
				: backgroundImage.texture;

			Rect rect = backgroundImage.sprite
				? backgroundImage.sprite.rect
				: new Rect(0, 0, texture.width, texture.height);

			// float diameter = _ellipseMesh.Radius * 2;
			// var textureScale = new Vector2(
			// 	diameter / texture.width,
			// 	diameter / texture.height);
			//
			// var scale = new Vector2(
			// 	rect.width / diameter * textureScale.x,
			// 	rect.height / diameter * textureScale.y);

			_fillTexture.Texture = texture;
			_fillTexture.Rect = rect;
			_fillTexture.Scale = Vector2.one;
		}

		private NativeSlice<ushort> CalculateSlice()
		{
			int sliceSize = Mathf.FloorToInt(Progress * _circleMesh.Indices.Length / 100);

			if (sliceSize < 3)
				sliceSize = 1;

			var indexArray = new NativeArray<ushort>(_circleMesh.Indices, Allocator.Temp);

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
			float diameter = _circleMesh.Radius * 2;
			Vector2 uvTextureScaledSize =
				new Vector2(_fillTexture.Rect.width, _fillTexture.Rect.height)
				/ ContainerScale() * TextureScale + TextureOffset;

			for (var i = 0; i < _circleMesh.Vertices.Length; i++)
			{
				Vector2 uvRelative =
					((Vector2)_circleMesh.Vertices[i].position
					 - _circleMesh.Center
					 + uvTextureScaledSize)
					/ diameter;

				_circleMesh.Vertices[i].uv =
					uvRelative
					* meshData.uvRegion.size
					/ TextureScale
					+ meshData.uvRegion.min;

				_circleMesh.Vertices[i].uv.y = 1 - _circleMesh.Vertices[i].uv.y;
			}

			return;

			float ContainerScale()
			{
				float offset;
				if (contentRect.width < contentRect.height)
					offset = contentRect.height / contentRect.width;
				else
					offset = 1;
				return offset * _fillTexture.Rect.height * 2 / contentRect.height;
			}
		}

		private int FindClosestMultipleBy3(int sliceSize)
		{
			int remainder = sliceSize % 3;

			if (remainder == 0)
				return sliceSize;

			return remainder == 1 ? sliceSize - 1 : sliceSize + 1;
		}

		private float GetMeshRadius() =>
			contentRect.width < contentRect.height ? contentRect.width / 2 : contentRect.height / 2;

		private class FillTexture
		{
			public float WidthPos { get; set; }
			public float HeightPos { get; set; }
			public Texture2D Texture { get; set; }
			public Rect Rect { get; set; }
			public Vector2 Scale { get; set; }

			public override string ToString()
			{
				if (Texture != null)
					return $"TEXTURE_{Texture.name} + POSITION_{Rect.position} + SIZE_{Rect.size}";
				else
					return "TEXTURE_NULL";
			}
		}

		#region UXML
		public class TextureFillElementTraits : UxmlTraits
		{
			private readonly UxmlColorAttributeDescription _tintColorAttribute = new()
				{ name = "tint-color", defaultValue = Color.white };
			private readonly UxmlEnumAttributeDescription<FillDirection> _fillDirectionAttribute = new()
				{ name = "fill-direction", defaultValue = FillDirection.Clockwise };

			private readonly UxmlEnumAttributeDescription<FillStart> _fillStartAttribute = new()
				{ name = "fill-start", defaultValue = FillStart.Up };

			private readonly UxmlFloatAttributeDescription _textureScaleAttribute = new()
				{ name = "texture-scale", defaultValue = 1f };

			private readonly UxmlFloatAttributeDescription _progressAttribute = new()
				{ name = "progress", defaultValue = 82f };

			private readonly UxmlIntAttributeDescription _stepsAttribute = new()
				{ name = "steps", defaultValue = 200 };

			private readonly UxmlIntAttributeDescription _textureOffsetXAttribute = new()
				{ name = "texture-offset-x", defaultValue = 0 };

			private readonly UxmlIntAttributeDescription _textureOffsetYAttribute = new()
				{ name = "texture-offset-y", defaultValue = 0 };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				if (ve is not TextureFillElement textureFill)
					return;
				textureFill.FillDirection = _fillDirectionAttribute.GetValueFromBag(bag, cc);
				textureFill.FillStart = _fillStartAttribute.GetValueFromBag(bag, cc);
				textureFill.Steps = _stepsAttribute.GetValueFromBag(bag, cc);
				textureFill.TextureScale = _textureScaleAttribute.GetValueFromBag(bag, cc);
				textureFill.TintColor = _tintColorAttribute.GetValueFromBag(bag, cc);
				textureFill.Progress = _progressAttribute.GetValueFromBag(bag, cc);
				textureFill.TextureOffset =
					new Vector2(_textureOffsetXAttribute.GetValueFromBag(bag, cc),
						_textureOffsetYAttribute.GetValueFromBag(bag, cc));
			}
		}

		public class TextureFillElementFactory : UxmlFactory<TextureFillElement, TextureFillElementTraits> { }
		#endregion
	}
}