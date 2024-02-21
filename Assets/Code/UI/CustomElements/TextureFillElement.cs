using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomElements
{
	public class TextureFillElement : VisualElement
	{
		private readonly EllipseMesh _ellipseMesh;
		private readonly FillTexture _fillTexture;
		private int _steps = 200;
		private Color _tintColor = Color.white;
		private FillStart _fillStart = FillStart.Up;
		private FillDirection _fillDirection = FillDirection.Clockwise;
		private float _progress;

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
			style.flexGrow = 1;
			style.unityBackgroundImageTintColor = Color.clear;
			_ellipseMesh = new EllipseMesh(_steps);
			_fillTexture = new FillTexture();

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
				context.Allocate(_ellipseMesh.Vertices.Length, indexSlice.Length, _fillTexture.Texture);

			if (_fillTexture != null)
				SetupUvCoords(meshData);

			meshData.SetAllVertices(_ellipseMesh.Vertices);
			meshData.SetAllIndices(indexSlice);
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
				sliceSize = 1;

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
				new Vector2(_fillTexture.Rect.width, _fillTexture.Rect.height)
				/ ContainerScale() * TextureScale + TextureOffset;

			for (var i = 0; i < _ellipseMesh.Vertices.Length; i++)
			{
				Vector2 uvRelative =
					((Vector2)_ellipseMesh.Vertices[i].position
					 - _ellipseMesh.Center
					 + uvTextureScaledSize)
					/ diameter;

				_ellipseMesh.Vertices[i].uv =
					uvRelative
					* meshData.uvRegion.size
					/ TextureScale
					+ meshData.uvRegion.min;

				_ellipseMesh.Vertices[i].uv.y = 1 - _ellipseMesh.Vertices[i].uv.y;
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
			private readonly UxmlEnumAttributeDescription<FillDirection> _fillDirectionAttribute = new()
				{ name = "fill-direction", defaultValue = FillDirection.Clockwise };

			private readonly UxmlEnumAttributeDescription<FillStart> _fillStartAttribute = new()
				{ name = "fill-start", defaultValue = FillStart.Up };

			private readonly UxmlIntAttributeDescription _stepsAttribute = new()
				{ name = "steps", defaultValue = 200 };

			private readonly UxmlFloatAttributeDescription _textureScaleAttribute = new()
				{ name = "texture-scale", defaultValue = 1f };

			private readonly UxmlIntAttributeDescription _textureOffsetXAttribute = new()
				{ name = "texture-offset-x", defaultValue = 0 };

			private readonly UxmlIntAttributeDescription _textureOffsetYAttribute = new()
				{ name = "texture-offset-y", defaultValue = 0 };

			private readonly UxmlColorAttributeDescription _tintColorAttribute = new()
				{ name = "tint-color", defaultValue = Color.white };

			private readonly UxmlFloatAttributeDescription _progressAttribute = new()
				{ name = "progress", defaultValue = 82f };

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