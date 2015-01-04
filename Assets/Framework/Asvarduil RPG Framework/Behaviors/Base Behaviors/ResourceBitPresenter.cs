using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ResourceBitPresenter : PresenterBase, IDisposable
{
	#region Variables / Properties

	public int rows = 1;
	public int bitsPerRow = 16;

	public List<Texture2D> Bits;
	public AsvarduilImage ResourceImage;
	private Texture2D _texture;
	
	private int BitStates
	{
		get { return Bits.Count - 1; }
	}
	
	private Texture2D FullBit
	{
		get { return Bits[Bits.Count - 1]; }
	}
	
	private Texture2D EmptyBit
	{
		get { return Bits[0]; }
	}
	
	#endregion Variables / Properties
	
	#region Hooks

	public abstract void Initialize();

	public void Dispose()
	{
		_texture = null;
	}
	
	public void UpdateImage(int current, int max)
	{
		// Calcluate width and values for the heart system...
		int fullBits = current / BitStates;
		int partialBitIndex = current % BitStates;
		int emptyBits = (max / BitStates) - (int) Mathf.Ceil(((float) current / BitStates));
		
		DebugMessage("There are " + fullBits + " full bits, and " + emptyBits + " empty bits.");

		// Build the dynamic texture...
		int bitX = 0;
		int bitY = 0;
		int bitCount = 0;

		Texture2D tex = PrepareCanvas();
		DrawFullBits(fullBits, tex, ref bitX, ref bitY, ref bitCount);
		DrawPartialBit(partialBitIndex, tex, ref bitX, ref bitY, ref bitCount);
		DrawEmptyBits(emptyBits, tex, ref bitX, ref bitY, ref bitCount);
		
		tex.Apply();
		_texture = tex;
		ResourceImage.Image = _texture;
	}

	private Texture2D PrepareCanvas()
	{
		int canvasWidth = FullBit.width * bitsPerRow;
		int canvasHeight = FullBit.height * rows;
		DebugMessage(String.Format("Canvas: [{0} x {1}]", canvasWidth, canvasHeight));
		Texture2D tex = new Texture2D(canvasWidth, canvasHeight);

		Color empty = new Color(0, 0, 0, 0);
		for(int x = 0; x < canvasWidth; x++)
			for(int y = 0; y < canvasHeight; y++)
				tex.SetPixel(x, y, empty);

		return tex;
	}
	
	private void DrawEmptyBits (int emptyBits, Texture2D tex, ref int bitX, ref int bitY, ref int bitCount)
	{
		DebugMessage(emptyBits + " Empty bits will be drawn.");
		for(int counter = 0; counter < emptyBits; counter++)
		{
			DebugMessage("[Empty Bits] Bit Count: " + bitCount + " X: " + bitX + " Y: " + bitY);

			tex.SetPixels(bitX, bitY, EmptyBit.width, EmptyBit.height, EmptyBit.GetPixels());
			bitX += EmptyBit.width;

			CheckBreakToNewRow(ref bitX, ref bitY, ref bitCount);
		}
	}
	
	private void DrawPartialBit (int bitIndex, Texture2D tex, ref int bitX, ref int bitY, ref int bitCount)
	{
		// If there is no partial, don't bother.
		DebugMessage("Bit #" + bitIndex + " is the partial bit.");
		if(bitIndex == 0)
			return;

		DebugMessage("[Partial Bit] Bit Count: " + bitCount + " X: " + bitX + " Y: " + bitY);

		Texture2D partialBit = Bits[bitIndex];
		tex.SetPixels(bitX, bitY, partialBit.width, partialBit.height, partialBit.GetPixels());
		bitX += partialBit.width;

		CheckBreakToNewRow(ref bitX, ref bitY, ref bitCount);
	}
	
	private void DrawFullBits (int fullBits, Texture2D tex, ref int bitX, ref int bitY, ref int bitCount)
	{
		DebugMessage(fullBits + " Full bits will be drawn.");
		for(int counter = 0; counter < fullBits; counter++)
		{
			DebugMessage("[Full Bits] Bit Count: " + bitCount + " X: " + bitX + " Y: " + bitY);
			tex.SetPixels(bitX, bitY, FullBit.width, FullBit.height, FullBit.GetPixels());
			bitX += FullBit.width;

			CheckBreakToNewRow(ref bitX, ref bitY, ref bitCount);
		}
	}

	private void CheckBreakToNewRow(ref int bitX, ref int bitY, ref int bitCount)
	{
		bitCount++;
		if(bitCount == bitsPerRow)
		{
			bitX = 0;
			bitY += EmptyBit.height;
		}
	}
	
	public override void SetVisibility (bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);
		
		ResourceImage.TargetTint.a = opacity;
	}
	
	public override void DrawMe()
	{
		ResourceImage.DrawMe();
	}
	
	public override void Tween()
	{
		ResourceImage.Tween();
	}
	
	#endregion Hooks
}
